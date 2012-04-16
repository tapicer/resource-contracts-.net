using System;
using Microsoft.Cci.MutableCodeModel;
using Microsoft.Cci;
using System.Collections.Generic;
using Microsoft.Cci.Contracts;
using Microsoft.Cci.MutableContracts;
using System.IO;
using Microsoft.Cci.ILToCodeModel;
using System.Text.RegularExpressions;

namespace memory_contracts_rewriter
{
    class MemoryVisitor : BaseCodeTraverser
    {
        #region Private fields

        private CodeContractAwareHostEnvironment _host;
        private PdbReader _pdbReader;
        private ContractProvider _contractProvider;
        private MemoryContractsInformation _memoryContractsInformation;
        private List<ErrorReportItem> _errorReportItems;
        private PointsToInformation _pointsToInformation;

        #endregion

        #region Properties

        public List<ErrorReportItem> ErrorReportItems { get { return _errorReportItems; } }

        #endregion

        #region Ctor

        public MemoryVisitor(CodeContractAwareHostEnvironment host, PdbReader pdbReader, ContractProvider contractProvider, MemoryContractsInformation memoryContractsInformation, PointsToInformation pointsToInformation)
        {
            _host = host;
            _pdbReader = pdbReader;
            _contractProvider = contractProvider;
            _memoryContractsInformation = memoryContractsInformation;
            _errorReportItems = new List<ErrorReportItem>();
            _pointsToInformation = pointsToInformation;
        }

        #endregion

        #region Visitations

        /// <summary>
        /// The method being visited
        /// </summary>
        protected IMethodDefinition _currentMethod;

        /// <summary>
        /// The instrumentation information of the method being visited
        /// </summary>
        protected InstrumentationInformation _currentInstrInfo;

        /// <summary>
        /// The nest level of loops in the current visiting statement
        /// </summary>
        protected int _loopNestLevel = 0;

        /// <summary>
        /// Contains all the goto statements used to check the nesting level of the current method
        /// </summary>
        protected List<IGotoStatement> _visitedGotoStatements;

        /// <summary>
        /// Stores all the loop invariants defined in each loop where to the nesting level - 1 is the list index
        /// </summary>
        protected List<HashSet<string>> _loopInvariants;

        /// <summary>
        /// Stores assignment to be done after the outmost loop
        /// </summary>
        protected List<Tuple<FieldDefinition, PolyCond>> _afterLoopAssignments;

        /// <summary>
        /// Visits each method and starts the visitation of each statement with the context of a method to collect intrumentation information
        /// </summary>
        public override void Visit(IMethodDefinition method)
        {
            System.Diagnostics.Debug.WriteLine("Visiting method: " + method.Name);

            var instrInfo = new InstrumentationInformation();

            //this dictionary contains all the type of tmp to instrument in the keys
            //and the statements where an increment should be done in the values
            instrInfo.TmpFieldsToInstrument = new Dictionary<FieldDefinition, List<IStatement>>();

            //this dictionary contains all the type of rsd to instrument in the keys
            //and the statements where an increment should be done in the values
            instrInfo.RsdFieldsToInstrument = new Dictionary<FieldDefinition, List<IStatement>>();

            //this boolean is true after finding a Contract.Memory.DestTmp() call
            instrInfo.StoreNextTmp = false;

            //this boolean is true after finding a Contract.Memory.DestRsd(rsdName) call
            instrInfo.StoreNextRsd = false;

            //stores the containing block of each statement, needed later for the instrumentation
            instrInfo.StatementsContainingBlocks = new Dictionary<IStatement, IBlockStatement>();

            //stores a list of statement after which a copy of tmp should be done
            instrInfo.StatementsTmpCopy = new List<StatementTmpCopyInformation>();

            //stores a list of statement after which a transference of rsd should be done
            instrInfo.StatementsRsdTransference = new List<StatementRsdTransferenceInformation>();

            //these two list store information about AddTmp and AddRsd calls
            instrInfo.AddToTmpRsdSources = new List<RsdNames>();
            instrInfo.AddToRsdRsdSources = new List<RsdNames>();

            //these dicts store the global poly info collected to verify with the calculator at the end of the method
            instrInfo.GlobalPolyInfoTmp = new Dictionary<string, GlobalPolyInfo>(); //<type, GlobalPolyInfo>
            instrInfo.GlobalPolyInfoRsd = new Dictionary<Tuple<string, string>, GlobalPolyInfo>(); //<<type, name>, GlobalPolyInfo>

            //stores the fields to add polys after loops
            instrInfo.LoopsPolyAdd = new List<PolyAddInformation>();

            _currentMethod = method;
            _currentInstrInfo = instrInfo;
            _loopNestLevel = 0;
            _visitedGotoStatements = new List<IGotoStatement>();
            _loopInvariants = new List<HashSet<string>>();
            _afterLoopAssignments = new List<Tuple<FieldDefinition, PolyCond>>();

            //set method info needed by PointsToInformation
            _pointsToInformation.CurrentMethodInfo = new PointsToInformation.CurrentMethodInfoData()
                {
                    ContainingType = method.ContainingTypeDefinition.ResolvedType,
                    Parameters = method.Parameters
                };

            //these call triggers the visitation of all the statements of the method,
            //instrumentation data will be collected in instrInfo
            base.Visit(method);

            //do tmp instrumentation for each type of tmp detected in the method
            foreach (var tmp in instrInfo.TmpFieldsToInstrument.Keys)
            {
                InstrumentTmp(method, tmp, instrInfo.TmpFieldsToInstrument[tmp], instrInfo.StatementsContainingBlocks);
            }

            //appends the statements to copy the tmps of the calls and calculate the maximum
            AppendStatementsTmpCopy(method, instrInfo.StatementsTmpCopy, instrInfo.StatementsContainingBlocks, instrInfo.GlobalPolyInfoTmp);

            //do rsd instrumentation for each type of rsd detected in the method
            foreach (var rsd in instrInfo.RsdFieldsToInstrument.Keys)
            {
                InstrumentRsd(method, rsd, instrInfo.RsdFieldsToInstrument[rsd], instrInfo.StatementsContainingBlocks);
            }

            //do rsd to tmp/rsd transference for each AddTmp/AddRsd detected in the methos
            AppendStatementsRsdTransference(method, instrInfo.StatementsRsdTransference, instrInfo.StatementsContainingBlocks);

            //do loop poly add instrumentation
            foreach (var loopPolyAdd in _currentInstrInfo.LoopsPolyAdd)
            {
                AppendStatementsLoopPolyAdd(method, loopPolyAdd, instrInfo.StatementsContainingBlocks);
            }

            //do global tmp poly verification
            foreach (var type in instrInfo.GlobalPolyInfoTmp.Keys)
            {
                GlobalPolyVerifcationTmp(method, instrInfo, type, instrInfo.GlobalPolyInfoTmp[type]);
            }

            //do global rsd poly verification
            foreach (var type in instrInfo.GlobalPolyInfoRsd.Keys)
            {
                GlobalPolyVerifcationRsd(method, instrInfo, type.Item1, type.Item2, instrInfo.GlobalPolyInfoRsd[type]);
            }
        }

        /// <summary>
        /// The statement previous to the one being visited
        /// </summary>
        protected IStatement _previousStatement;
        
        /// <summary>
        /// The statement being visited
        /// </summary>
        protected IStatement _currentStatement;

        /// <summary>
        /// Remembers the statement that is being visted
        /// </summary>
        public override void Visit(IStatement statement)
        {
            _previousStatement = _currentStatement;

            _currentStatement = statement;

            base.Visit(statement);
        }

        protected IGotoStatement _outmostLoopStart = null;
        protected List<StatementTmpCopyInformation> _pendingOutmostLoopEndToRegister = null;

        public override void Visit(IGotoStatement gotoStatement)
        {
            //update loop nest level
            if (!_visitedGotoStatements.Contains(gotoStatement))
            {
                _loopNestLevel++;
                _visitedGotoStatements.Add(gotoStatement);
                _loopInvariants.Add(new HashSet<string>());

                if (_loopNestLevel == 1)
                {
                    _outmostLoopStart = gotoStatement;
                    _pendingOutmostLoopEndToRegister = new List<StatementTmpCopyInformation>();
                }
            }

            base.Visit(gotoStatement);
        }

        public override void Visit(IConditionalStatement conditionalStatement)
        {
            //update loop nest level
            if (conditionalStatement.TrueBranch is IBlockStatement)
            {
                var block = (IBlockStatement)conditionalStatement.TrueBranch;
                var iter = block.Statements.GetEnumerator();
                if (iter.MoveNext() && iter.Current is IGotoStatement)
                {
                    var gotoStatement = (IGotoStatement)iter.Current;

                    if (!_visitedGotoStatements.Contains(gotoStatement))
                    {
                        _loopNestLevel--;
                        _visitedGotoStatements.Add(gotoStatement);
                        _loopInvariants.RemoveAt(_loopInvariants.Count - 1);

                        //out of the outmost loop, register the instrumentation of calculated poly assignments
                        if (_loopNestLevel == 0)
                        {
                            foreach (var poly in _afterLoopAssignments)
                            {
                                _currentInstrInfo.LoopsPolyAdd.Add(new PolyAddInformation()
                                    {
                                        Poly = poly.Item2,
                                        AddTarget = poly.Item1,
                                        AfterStatement = _currentStatement
                                    });
                            }

                            _afterLoopAssignments = new List<Tuple<FieldDefinition, PolyCond>>();

                            foreach (var tmpInfo in _pendingOutmostLoopEndToRegister)
                            {
                                tmpInfo.LoopEndAt = _currentStatement;
                            }
                            _pendingOutmostLoopEndToRegister = null;
                        }
                    }
                }
            }

            base.Visit(conditionalStatement);
        }

        /// <summary>
        /// Stores the containing block for every statement
        /// </summary>
        public override void Visit(IBlockStatement block)
        {
            foreach (var statement in block.Statements)
            {
                if (!_currentInstrInfo.StatementsContainingBlocks.ContainsKey(statement))
                {
                    _currentInstrInfo.StatementsContainingBlocks.Add(statement, block);
                }
            }

            base.Visit(block);
        }

        static Regex reLoopInv = new Regex(@"IterationSpace\((.*)\);", RegexOptions.Compiled);

        /// <summary>
        /// Detects calls to memory contracts methods and stores the needed information for the instrumentation
        /// </summary>
        public void VisitMethodCall(IMethodReference methodReference, IEnumerable<IExpression> arguments)
        {
            System.Diagnostics.Debug.WriteLine("Visiting method call: " + methodReference.ToString());

            if (methodReference.ContainingType.ToString() == "Contracts.Contract.Memory")
            {
                switch (methodReference.Name.Value)
                {
                    #region Tmp
                    case "Tmp":
                        {
                            if (methodReference is Microsoft.Cci.MutableCodeModel.GenericMethodInstanceReference)
                            {
                                var methodToCall = (Microsoft.Cci.MutableCodeModel.GenericMethodInstanceReference)methodReference;
                                var argsIt = arguments.GetEnumerator();
                                argsIt.MoveNext();
                                var limit = argsIt.Current;
                                var cond = (IExpression)null;
                                if (argsIt.MoveNext())
                                {
                                    cond = argsIt.Current;
                                }

                                var tmpField = AddTmpContractToMethod(_currentMethod, methodToCall.GenericArguments[0], limit, cond);

                                if (!_currentInstrInfo.TmpFieldsToInstrument.ContainsKey(tmpField))
                                {
                                    _currentInstrInfo.TmpFieldsToInstrument.Add(tmpField, new List<IStatement>());
                                }
                            }
                        }
                        break;
                    #endregion
                    #region DestTmp
                    case "DestTmp":
                        {
                            _currentInstrInfo.StoreNextTmp = true;
                            var argsIt = arguments.GetEnumerator();
                            _currentInstrInfo.TrustNextAnnotation = argsIt.MoveNext() &&
                                                                    argsIt.Current is CompileTimeConstant &&
                                                                    (int)((CompileTimeConstant)argsIt.Current).Value == 1;
                            _currentInstrInfo.Instrument = true;

                            //if inside loop and we have all the necessary invariants
                            if (_loopNestLevel > 0 && _loopInvariants.TrueForAll(s => s.Count > 0))
                            {
                                _currentInstrInfo.Instrument = false;
                                _currentInstrInfo.NextStmtPoly = PolytopesCalculator.CountPoints(JoinLoopInvariants(_loopInvariants), GetMethodFreeVars(_currentMethod));
                                //if the count failed, do regular instrumentation
                                if (_currentInstrInfo.NextStmtPoly == null)
                                {
                                    _currentInstrInfo.Instrument = true;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Rsd
                    case "Rsd":
                        {
                            if (methodReference is Microsoft.Cci.MutableCodeModel.GenericMethodInstanceReference)
                            {
                                var methodToCall = (Microsoft.Cci.MutableCodeModel.GenericMethodInstanceReference)methodReference;
                                var argsIt = arguments.GetEnumerator();
                                argsIt.MoveNext();
                                var rsdName = "";
                                if (argsIt.Current is BoundExpression)
                                {
                                    rsdName = ((BoundExpression)argsIt.Current).Definition.ToString();
                                }
                                argsIt.MoveNext();
                                var limit = argsIt.Current;
                                var cond = (IExpression)null;
                                if (argsIt.MoveNext())
                                {
                                    cond = argsIt.Current;
                                }

                                var rsdField = AddRsdContractToMethod(_currentMethod, methodToCall.GenericArguments[0], rsdName, limit, cond);

                                if (!_currentInstrInfo.RsdFieldsToInstrument.ContainsKey(rsdField))
                                {
                                    _currentInstrInfo.RsdFieldsToInstrument.Add(rsdField, new List<IStatement>());
                                }
                            }
                        }
                        break;
                    #endregion
                    #region DestRsd
                    case "DestRsd":
                        {
                            var argsIt = arguments.GetEnumerator();
                            argsIt.MoveNext();
                            var rsdName = "";
                            if (argsIt.Current is BoundExpression)
                            {
                                rsdName = ((BoundExpression)argsIt.Current).Definition.ToString();
                            }
                            _currentInstrInfo.TrustNextAnnotation = argsIt.MoveNext() &&
                                                                   argsIt.Current is CompileTimeConstant &&
                                                                   (int)((CompileTimeConstant)argsIt.Current).Value == 1;

                            _currentInstrInfo.StoreNextRsd = true;
                            _currentInstrInfo.NextStoreRsdName = rsdName;
                            _currentInstrInfo.Instrument = true;

                            //if inside loop and we have all the necessary invariants
                            if (_loopNestLevel > 0 && _loopInvariants.TrueForAll(s => s.Count > 0))
                            {
                                _currentInstrInfo.Instrument = false;
                                _currentInstrInfo.NextStmtPoly = PolytopesCalculator.CountPoints(JoinLoopInvariants(_loopInvariants), GetMethodFreeVars(_currentMethod));
                                //if the count failed, do regular instrumentation
                                if (_currentInstrInfo.NextStmtPoly == null)
                                {
                                    _currentInstrInfo.Instrument = true;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region AddTmp/AddRsd
                    case "AddTmp":
                    case "AddRsd":
                        {
                            var argsIt = arguments.GetEnumerator();
                            argsIt.MoveNext();
                            var rsdNameLocal = "";
                            if (argsIt.Current is BoundExpression)
                            {
                                rsdNameLocal = ((BoundExpression)argsIt.Current).Definition.ToString();
                            }

                            var rsdNameCall = "";

                            if (argsIt.MoveNext())
                            {
                                if (argsIt.Current is BoundExpression)
                                {
                                    rsdNameCall = ((BoundExpression)argsIt.Current).Definition.ToString();
                                }

                                //two parameters, Rsd to Rsd
                                _currentInstrInfo.AddToRsdRsdSources.Add(
                                        new RsdNames()
                                        {
                                            LocalRsdName = rsdNameLocal,
                                            CallRsdName = rsdNameCall,
                                            Trust = argsIt.MoveNext() && argsIt.Current is CompileTimeConstant && (int)((CompileTimeConstant)argsIt.Current).Value == 1,
                                            Statement = _currentStatement
                                        });
                            }
                            else
                            {
                                rsdNameCall = rsdNameLocal; //in this case rsdNameLocal is actually the name of the rsd in the called method

                                //one parameter, Rsd to Tmp
                                _currentInstrInfo.AddToTmpRsdSources.Add(
                                        new RsdNames()
                                        {
                                            CallRsdName = rsdNameCall,
                                            Trust = argsIt.MoveNext() && argsIt.Current is CompileTimeConstant && (int)((CompileTimeConstant)argsIt.Current).Value == 1,
                                            Statement = _currentStatement
                                        });
                            }
                        }
                        break;
                    #endregion
                    #region IterationSpace
                    case "IterationSpace":
                        {
                            //store the loop invariant _loopInvariants for the current loop according to _loopNestLevel
                            var argsIt = arguments.GetEnumerator();
                            argsIt.MoveNext();
                            var cond = argsIt.Current;
                            var locsIt = cond.Locations.GetEnumerator();
                            if (locsIt.MoveNext())
                            {
                                var match = reLoopInv.Match(((IPrimarySourceLocation)locsIt.Current).Source);

                                if (match.Success)
                                {
                                    _loopInvariants[_loopNestLevel - 1].Add(match.Groups[1].Value);
                                }
                            }
                        }
                        break;
                    #endregion
                }
            }
            else
            {
                var currentMethodName = _currentMethod.ToString();
                var calledMethodName = methodReference.ToString();

                //register the tmps of the called method to calculate the maximum later
                if (_memoryContractsInformation.MethodsTmpTypes.ContainsKey(currentMethodName))
                {
                    var calledMethodTmps = new List<ITypeReference>();

                    if (_memoryContractsInformation.MethodsTmpTypes.ContainsKey(calledMethodName))
                    {
                        calledMethodTmps = _memoryContractsInformation.MethodsTmpTypes[calledMethodName];
                    }

                    foreach (var tmpType in _memoryContractsInformation.MethodsTmpTypes[currentMethodName])
                    {
                        //if the called method has a contract for the tmp type
                        if (TypeListContainsType(calledMethodTmps, tmpType))
                        {
                            //register the copy statement to be added later --> int local_tmp = called_class.tmp;
                            var tmpInfo = new StatementTmpCopyInformation()
                                {
                                    AfterStatement = _currentStatement,
                                    CalledMethod = methodReference.ResolvedMethod,
                                    TmpType = tmpType,
                                    InsideLoop = _loopNestLevel > 0,
                                    LoopStartAt = _loopNestLevel > 0 ? _outmostLoopStart : null,
                                    LoopInvariants = _loopNestLevel > 0 && _loopInvariants.TrueForAll(s => s.Count > 0) ? JoinLoopInvariants(_loopInvariants) : null,
                                    CalledMethodReference = methodReference,
                                    CalledMethodArguments = arguments
                                };
                            _currentInstrInfo.StatementsTmpCopy.Add(tmpInfo);
                            if (_loopNestLevel > 0)
                            {
                                _pendingOutmostLoopEndToRegister.Add(tmpInfo);
                            }
                        }
                    }
                }

                //register the copy of Rsd to Tmp if there were previous AddTmp statements
                if (_currentInstrInfo.AddToTmpRsdSources.Count > 0)
                {
                    //intrumentation
                    if (_memoryContractsInformation.MethodsRsdTypes.ContainsKey(calledMethodName))
                    {
                        foreach (var rsdName in _currentInstrInfo.AddToTmpRsdSources)
                        {
                            //find all the rsds in the called method with the given name
                            foreach (var rsdType in _memoryContractsInformation.MethodsRsdTypes[calledMethodName])
                            {
                                if (rsdType.Name == rsdName.CallRsdName)
                                {
                                    //see if there is a local tmp for the rsd type in the called method contract
                                    if (_memoryContractsInformation.MethodsTmpTypes.ContainsKey(currentMethodName) &&
                                        TypeListContainsType(_memoryContractsInformation.MethodsTmpTypes[currentMethodName], rsdType.Type))
                                    {
                                        bool doRegularInstrumentation = true;

                                        if (!_currentInstrInfo.GlobalPolyInfoTmp.ContainsKey(rsdType.Type.ToString()))
                                        {
                                            _currentInstrInfo.GlobalPolyInfoTmp.Add(rsdType.Type.ToString(), new GlobalPolyInfo());
                                        }

                                        //see if we can calculate the sum inside a loop using the polytopes calc
                                        if (_loopNestLevel > 0 && _loopInvariants.TrueForAll(s => s.Count > 0))
                                        {
                                            if (_memoryContractsInformation.MethodsRsdContracts.ContainsKey(calledMethodName) &&
                                                _memoryContractsInformation.MethodsRsdContracts[calledMethodName].ContainsKey(rsdName.CallRsdName) &&
                                                _memoryContractsInformation.MethodsRsdContracts[calledMethodName][rsdName.CallRsdName].ContainsKey(rsdType.Type))
                                            {
                                                var contractLocalExpr = TranslateMethodContractToLocalExpression(
                                                    methodReference,
                                                    _currentStatement,
                                                    _memoryContractsInformation.MethodsRsdContracts[calledMethodName][rsdName.CallRsdName][rsdType.Type],
                                                    arguments);
                                                var sumExprPolyCond = PolytopesCalculator.SumOver(contractLocalExpr, JoinLoopInvariants(_loopInvariants), GetMethodFreeVars(_currentMethod));
                                                var sumPolyExpr = ExpressionGenerator.GenerateExpressionFromString(sumExprPolyCond.Poly, _host, _currentMethod);
                                                var sumCondExpr = ExpressionGenerator.GenerateExpressionFromString(sumExprPolyCond.Cond, _host, _currentMethod);

                                                if (sumPolyExpr != null && sumCondExpr != null)
                                                {
                                                    //register the poly to be added to the tmp field after the loop
                                                    _afterLoopAssignments.Add(new Tuple<FieldDefinition, PolyCond>(GetTmpField(_currentMethod, rsdType.Type), sumExprPolyCond));
                                                    doRegularInstrumentation = false;

                                                    //store in the global poly info
                                                    _currentInstrInfo.GlobalPolyInfoTmp[rsdType.Type.ToString()].QuantityTransferLoops.Add(sumExprPolyCond);
                                                }
                                            }
                                        }

                                        if (doRegularInstrumentation)
                                        {
                                            //register the transference statement to be added later --> local_tmp += called_class.rsd;
                                            if (_loopNestLevel > 0)
                                            {
                                                _currentInstrInfo.GlobalPolyInfoTmp[rsdType.Type.ToString()].AllContractsRequiredAvailable = false;
                                            }
                                            else if (_memoryContractsInformation.MethodsRsdContracts.ContainsKey(calledMethodName) &&
                                                     _memoryContractsInformation.MethodsRsdContracts[calledMethodName].ContainsKey(rsdName.CallRsdName) &&
                                                     _memoryContractsInformation.MethodsRsdContracts[calledMethodName][rsdName.CallRsdName].ContainsKey(rsdType.Type))
                                            {
                                                var contractLocalExpr = TranslateMethodContractToLocalExpression(
                                                    methodReference,
                                                    _currentStatement,
                                                    _memoryContractsInformation.MethodsRsdContracts[calledMethodName][rsdName.CallRsdName][rsdType.Type],
                                                    arguments);
                                                _currentInstrInfo.GlobalPolyInfoTmp[rsdType.Type.ToString()].QuantityTransfer.Add(contractLocalExpr);
                                            }
                                            _currentInstrInfo.StatementsRsdTransference.Add(
                                                new StatementRsdTransferenceInformation()
                                                {
                                                    AfterStatement = _currentStatement,
                                                    AddSource = GetRsdField(methodReference.ResolvedMethod, rsdName.CallRsdName, rsdType.Type),
                                                    AddTarget = GetTmpField(_currentMethod, rsdType.Type)
                                                });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //verify the correctness
                    foreach (var rsdName in _currentInstrInfo.AddToTmpRsdSources)
                    {
                        //if not trust
                        if (!rsdName.Trust)
                        {
                            var rsdNameCall = rsdName.CallRsdName;
                            var localExpr = "";
                            if (rsdNameCall == "Contracts.Contract.Memory.Return")
                            {
                                localExpr = GetReturnExprFromStatement(_currentStatement);
                            }
                            else if (rsdNameCall == "Contracts.Contract.Memory.This")
                            {
                                localExpr = GetCallStatementThisExpr(methodReference, _currentStatement);
                            }
                            else if (_memoryContractsInformation.MethodsRsdExprs.ContainsKey(methodReference.ResolvedMethod.ToString()) &&
                                     _memoryContractsInformation.MethodsRsdExprs[methodReference.ResolvedMethod.ToString()].ContainsKey(rsdNameCall))
                            {
                                var calledMethodExpr = _memoryContractsInformation.MethodsRsdExprs[methodReference.ResolvedMethod.ToString()][rsdNameCall];

                                localExpr = GetBoundRsdExprFromStatement(methodReference, _currentStatement, calledMethodExpr, arguments);
                            }

                            if (localExpr != "")
                            {
                                if (_pointsToInformation.Escapes(_currentMethod.ContainingType.ToString(), _currentMethod.Name.Value, localExpr))
                                {
                                    string location = LocationsToString(rsdName.Statement.Locations);
                                    if (location != null)
                                    {
                                        _errorReportItems.Add(new ErrorReportItem()
                                        {
                                            Message = "The called method rsd destination escapes from the method.",
                                            Location = location
                                        });
                                    }
                                }
                            }
                        }
                    }

                    //clean the list for the next call
                    _currentInstrInfo.AddToTmpRsdSources = new List<RsdNames>();
                }

                //register the copy of Rsd to Rsd if there were previous AddRsd statements
                if (_currentInstrInfo.AddToRsdRsdSources.Count > 0)
                {
                    //instrumentation
                    if (_memoryContractsInformation.MethodsRsdTypes.ContainsKey(calledMethodName))
                    {
                        foreach (var rsdNames in _currentInstrInfo.AddToRsdRsdSources)
                        {
                            //find all the rsds in the called method with the given name
                            foreach (var rsdType in _memoryContractsInformation.MethodsRsdTypes[calledMethodName])
                            {
                                if (rsdType.Name == rsdNames.CallRsdName)
                                {
                                    //see if there is a local rsd for the rsd type in the called method contract
                                    if (_memoryContractsInformation.MethodsRsdTypes.ContainsKey(currentMethodName) &&
                                        _memoryContractsInformation.MethodsRsdTypes[currentMethodName].Contains(new RsdType() { Name = rsdNames.LocalRsdName, Type = rsdType.Type }))
                                    {
                                        bool doRegularInstrumentation = true;

                                        var typeAndName = new Tuple<string, string>(rsdType.Type.ToString(), rsdNames.LocalRsdName);
                                        if (!_currentInstrInfo.GlobalPolyInfoRsd.ContainsKey(typeAndName))
                                        {
                                            _currentInstrInfo.GlobalPolyInfoRsd.Add(typeAndName, new GlobalPolyInfo());
                                        }

                                        //see if we can calculate the sum inside a loop using the polytopes calc
                                        if (_loopNestLevel > 0 && _loopInvariants.TrueForAll(s => s.Count > 0))
                                        {
                                            if (_memoryContractsInformation.MethodsRsdContracts.ContainsKey(calledMethodName) &&
                                                _memoryContractsInformation.MethodsRsdContracts[calledMethodName].ContainsKey(rsdNames.CallRsdName) &&
                                                _memoryContractsInformation.MethodsRsdContracts[calledMethodName][rsdNames.CallRsdName].ContainsKey(rsdType.Type))
                                            {
                                                var contractLocalExpr = TranslateMethodContractToLocalExpression(
                                                    methodReference,
                                                    _currentStatement,
                                                    _memoryContractsInformation.MethodsRsdContracts[calledMethodName][rsdNames.CallRsdName][rsdType.Type],
                                                    arguments);
                                                var sumExprPolyCond = PolytopesCalculator.SumOver(contractLocalExpr, JoinLoopInvariants(_loopInvariants), GetMethodFreeVars(_currentMethod));
                                                var sumPolyExpr = ExpressionGenerator.GenerateExpressionFromString(sumExprPolyCond.Poly, _host, _currentMethod);
                                                var sumCondExpr = ExpressionGenerator.GenerateExpressionFromString(sumExprPolyCond.Cond, _host, _currentMethod);

                                                if (sumPolyExpr != null && sumCondExpr != null)
                                                {
                                                    //register the poly to be added to the rsd field after the loop
                                                    _afterLoopAssignments.Add(new Tuple<FieldDefinition, PolyCond>(GetRsdField(_currentMethod, rsdNames.LocalRsdName, rsdType.Type), sumExprPolyCond));
                                                    doRegularInstrumentation = false;

                                                    //store in the global poly info
                                                    _currentInstrInfo.GlobalPolyInfoRsd[typeAndName].QuantityTransferLoops.Add(sumExprPolyCond);
                                                }
                                            }
                                        }

                                        if (doRegularInstrumentation)
                                        {
                                            //register the transference statement to be added later --> local_rsd += called_class.rsd;
                                            if (_loopNestLevel > 0)
                                            {
                                                _currentInstrInfo.GlobalPolyInfoRsd[typeAndName].AllContractsRequiredAvailable = false;
                                            }
                                            else if (_memoryContractsInformation.MethodsRsdContracts.ContainsKey(calledMethodName) &&
                                                     _memoryContractsInformation.MethodsRsdContracts[calledMethodName].ContainsKey(rsdNames.CallRsdName) &&
                                                     _memoryContractsInformation.MethodsRsdContracts[calledMethodName][rsdNames.CallRsdName].ContainsKey(rsdType.Type))
                                            {
                                                var contractLocalExpr = TranslateMethodContractToLocalExpression(
                                                    methodReference,
                                                    _currentStatement,
                                                    _memoryContractsInformation.MethodsRsdContracts[calledMethodName][rsdNames.CallRsdName][rsdType.Type],
                                                    arguments);
                                                _currentInstrInfo.GlobalPolyInfoRsd[typeAndName].QuantityTransfer.Add(contractLocalExpr);
                                            }
                                            _currentInstrInfo.StatementsRsdTransference.Add(
                                                new StatementRsdTransferenceInformation()
                                                {
                                                    AfterStatement = _currentStatement,
                                                    AddSource = GetRsdField(methodReference.ResolvedMethod, rsdNames.CallRsdName, rsdType.Type),
                                                    AddTarget = GetRsdField(_currentMethod, rsdNames.LocalRsdName, rsdType.Type)
                                                });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //verify the correctness
                    foreach (var rsdName in _currentInstrInfo.AddToRsdRsdSources)
                    {
                        //if not trust
                        if (!rsdName.Trust)
                        {
                            var rsdNameCall = rsdName.CallRsdName;
                            var localExpr = "";
                            if (rsdNameCall == "Contracts.Contract.Memory.Return")
                            {
                                localExpr = GetReturnExprFromStatement(_currentStatement);
                            }
                            else if (rsdNameCall == "Contracts.Contract.Memory.This")
                            {
                                localExpr = GetCallStatementThisExpr(methodReference, _currentStatement);
                            }
                            else if (_memoryContractsInformation.MethodsRsdExprs.ContainsKey(methodReference.ResolvedMethod.ToString()) &&
                                     _memoryContractsInformation.MethodsRsdExprs[methodReference.ResolvedMethod.ToString()].ContainsKey(rsdNameCall))
                            {
                                var calledMethodExpr = _memoryContractsInformation.MethodsRsdExprs[methodReference.ResolvedMethod.ToString()][rsdNameCall];

                                localExpr = GetBoundRsdExprFromStatement(methodReference, _currentStatement, calledMethodExpr, arguments);
                            }

                            if (localExpr != "")
                            {
                                if (!_pointsToInformation.Escapes(_currentMethod.ContainingType.ToString(), _currentMethod.Name.Value, localExpr))
                                {
                                    string location = LocationsToString(rsdName.Statement.Locations);
                                    if (location != null)
                                    {
                                        _errorReportItems.Add(new ErrorReportItem()
                                        {
                                            Message = "The called method rsd destination doesn't escape from the method.",
                                            Location = location
                                        });
                                    }
                                }
                                else
                                {
                                    //verify that it effectively escapes through the expression given in BindRsd
                                    var localRsdName = rsdName.LocalRsdName;
                                    var localRsdExpr = "";
                                    if (localRsdName == "Contracts.Contract.Memory.Return")
                                    {
                                        localRsdExpr = "vRet";
                                    }
                                    else if (localRsdName == "Contracts.Contract.Memory.This")
                                    {
                                        localRsdExpr = "this";
                                    }
                                    else if (_memoryContractsInformation.MethodsRsdExprs.ContainsKey(_currentMethod.ToString()) &&
                                             _memoryContractsInformation.MethodsRsdExprs[_currentMethod.ToString()].ContainsKey(localRsdName))
                                    {
                                        localRsdExpr = _memoryContractsInformation.MethodsRsdExprs[_currentMethod.ToString()][localRsdName];
                                    }

                                    if (!_pointsToInformation.Escapes(_currentMethod.ContainingType.ToString(), _currentMethod.Name.Value, localExpr, localRsdExpr))
                                    {
                                        string location = LocationsToString(rsdName.Statement.Locations);
                                        if (location != null)
                                        {
                                            _errorReportItems.Add(new ErrorReportItem()
                                            {
                                                Message = string.Format("The called method rsd destination escapes from the method but not through the expression '{0}'.", localRsdExpr),
                                                Location = location
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //clean the list for the next call
                    _currentInstrInfo.AddToRsdRsdSources = new List<RsdNames>();
                }
            }
        }

        /// <summary>
        /// Passes along the call info to VisitMethodCall
        /// </summary>
        public override void Visit(IMethodCall methodCall)
        {
            VisitMethodCall(methodCall.MethodToCall, methodCall.Arguments);

            base.Visit(methodCall);
        }

        /// <summary>
        /// Stores information for the instrumentation of counters increments after "new" calls and
        /// passes along the call info to VisitMethodCall
        /// </summary>
        public override void Visit(ICreateObjectInstance createObjectInstance)
        {
            if (_currentInstrInfo.StoreNextTmp)
            {
                var type = createObjectInstance.Type;
                var tmpField = GetTmpField(_currentMethod, type);
                if (_currentInstrInfo.TmpFieldsToInstrument.ContainsKey(tmpField))
                {
                    if (!_currentInstrInfo.GlobalPolyInfoTmp.ContainsKey(type.ToString()))
                    {
                        _currentInstrInfo.GlobalPolyInfoTmp.Add(type.ToString(), new GlobalPolyInfo());
                    }
                    if (_currentInstrInfo.Instrument)
                    {
                        if (_loopNestLevel > 0)
                        {
                            _currentInstrInfo.GlobalPolyInfoTmp[type.ToString()].AllContractsRequiredAvailable = false;
                        }
                        else
                        {
                            _currentInstrInfo.GlobalPolyInfoTmp[type.ToString()].DirectQuantity++;
                        }
                        //register the counter to be instrumented
                       _currentInstrInfo.TmpFieldsToInstrument[tmpField].Add(_currentStatement);
                    }
                    else
                    {
                        //register the poly to be added to the tmp field after the loop
                        _afterLoopAssignments.Add(new Tuple<FieldDefinition, PolyCond>(tmpField, _currentInstrInfo.NextStmtPoly));
                        //store in the global poly info
                        _currentInstrInfo.GlobalPolyInfoTmp[type.ToString()].DirectQuantityLoops.Add(_currentInstrInfo.NextStmtPoly);
                    }
                }

                //verify that the object doesn't escape
                if (!_currentInstrInfo.TrustNextAnnotation &&
                    _pointsToInformation.Escapes(_currentMethod.ContainingType.ToString(), _currentMethod.Name.Value, _currentStatement) &&
                    _previousStatement != null)
                {
                    string location = LocationsToString(_previousStatement.Locations);
                    if (location != null)
                    {
                        _errorReportItems.Add(new ErrorReportItem()
                            {
                                Message = string.Format("The object created escapes from the method."),
                                Location = location
                            });
                    }
                }

                _currentInstrInfo.StoreNextTmp = false;
            }

            if (_currentInstrInfo.StoreNextRsd)
            {
                var type = createObjectInstance.Type;
                var rsdField = GetRsdField(_currentMethod, _currentInstrInfo.NextStoreRsdName, type);
                if (_currentInstrInfo.RsdFieldsToInstrument.ContainsKey(rsdField))
                {
                    var typeAndName = new Tuple<string, string>(type.ToString(), _currentInstrInfo.NextStoreRsdName);
                    if (!_currentInstrInfo.GlobalPolyInfoRsd.ContainsKey(typeAndName))
                    {
                        _currentInstrInfo.GlobalPolyInfoRsd.Add(typeAndName, new GlobalPolyInfo());
                    }
                    if (_currentInstrInfo.Instrument)
                    {
                        if (_loopNestLevel > 0)
                        {
                            _currentInstrInfo.GlobalPolyInfoRsd[typeAndName].AllContractsRequiredAvailable = false;
                        }
                        else
                        {
                            _currentInstrInfo.GlobalPolyInfoRsd[typeAndName].DirectQuantity++;
                        }
                        //register the counter to be instrumented
                        _currentInstrInfo.RsdFieldsToInstrument[rsdField].Add(_currentStatement);
                    }
                    else
                    {
                        //register the poly to be added to the rsd field after the loop
                        _afterLoopAssignments.Add(new Tuple<FieldDefinition, PolyCond>(rsdField, _currentInstrInfo.NextStmtPoly));
                        //store in the global poly info
                        _currentInstrInfo.GlobalPolyInfoRsd[typeAndName].DirectQuantityLoops.Add(_currentInstrInfo.NextStmtPoly);
                    }
                }

                //verify that the object escapes
                if (!_currentInstrInfo.TrustNextAnnotation && _previousStatement != null)
                {
                    if (!_pointsToInformation.Escapes(_currentMethod.ContainingType.ToString(), _currentMethod.Name.Value, _currentStatement))
                    {
                        string location = LocationsToString(_previousStatement.Locations);
                        if (location != null)
                        {
                            _errorReportItems.Add(new ErrorReportItem()
                            {
                                Message = "The object created doesn't escape from the method.",
                                Location = location
                            });
                        }
                    }
                    //if it does escape, verify that escape expression is correct
                    else
                    {
                        var rsdName = _currentInstrInfo.NextStoreRsdName;
                        var expr = "";
                        if (rsdName == "Contracts.Contract.Memory.Return")
                        {
                            expr = "vRet";
                        }
                        else if (rsdName == "Contracts.Contract.Memory.This")
                        {
                            expr = "this";
                        }
                        else if (_memoryContractsInformation.MethodsRsdExprs.ContainsKey(_currentMethod.ToString()) &&
                                 _memoryContractsInformation.MethodsRsdExprs[_currentMethod.ToString()].ContainsKey(rsdName))
                        {
                            expr = _memoryContractsInformation.MethodsRsdExprs[_currentMethod.ToString()][rsdName];
                        }
                        if (expr != "" && !_pointsToInformation.Escapes(_currentMethod.ContainingType.ToString(), _currentMethod.Name.Value, _currentStatement, expr))
                        {
                            string location = LocationsToString(_previousStatement.Locations);
                            if (location != null)
                            {
                                _errorReportItems.Add(new ErrorReportItem()
                                {
                                    Message = string.Format("The object created escapes but not through the expression '{0}'.", expr),
                                    Location = location
                                });
                            }
                        }
                    }
                }

                _currentInstrInfo.StoreNextRsd = false;
            }

            VisitMethodCall(createObjectInstance.MethodToCall, createObjectInstance.Arguments);

            base.Visit(createObjectInstance);
        }

        #endregion
        
        #region Tmp-related methods
        
        /// <summary>
        /// Stores all the defined tmp fields for all methods
        /// </summary>
        Dictionary<string, FieldDefinition> _tmpFields = new Dictionary<string, FieldDefinition>();

        /// <summary>
        /// Returns the tmp field for the method and type, if it doesn't exist it is created and added to the class
        /// </summary>
        protected FieldDefinition GetTmpField(IMethodDefinition method, ITypeReference type)
        {
            return GetTmpField(method, type.ToString());
        }

        /// <summary>
        /// Returns the tmp field for the method and type, if it doesn't exist it is created and added to the class
        /// </summary>
        protected FieldDefinition GetTmpField(IMethodDefinition method, string type)
        {
            //@todo: modify to support overloads and containig types with the same name
            string tmpId = method.ContainingType.ToString().Replace('.', '_') + "_" + method.Name + "_tmp_" + type.Replace('.', '_');

            if (_tmpFields.ContainsKey(tmpId))
            {
                return _tmpFields[tmpId];
            }

            var tmpField = new FieldDefinition
            {
                Type = _host.PlatformType.SystemInt32,
                Name = _host.NameTable.GetNameFor(tmpId),
                IsStatic = true,
                Visibility = TypeMemberVisibility.Public,
                IsReadOnly = false,
                ContainingTypeDefinition = method.Container
            };

            //the container isn't a TypeDefinition when the class isn't in the current assembly
            if (method.Container is TypeDefinition)
            {
                ((TypeDefinition)method.Container).Fields.Add(tmpField);
            }
            _tmpFields.Add(tmpId, tmpField);

            return tmpField;
        }

        /// <summary>
        /// Stores of the types of tmps that a method has contracts for
        /// </summary>
        Dictionary<IMethodDefinition, List<ITypeReference>> _methodsTmpTypes = new Dictionary<IMethodDefinition, List<ITypeReference>>();

        protected List<ITypeReference> GetTmpTypesForMethod(IMethodDefinition method)
        {
            return null;
        }

        /// <summary>
        /// Adds the tmp contract to the method, it is added as a postcondition where the tmp field of the type is less than or equal to limit
        /// </summary>
        protected FieldDefinition AddTmpContractToMethod(IMethodDefinition method, ITypeReference type, IExpression limit, IExpression cond)
        {
            var tmpField = GetTmpField(method, type);

            var newContract = new Microsoft.Cci.MutableContracts.MethodContract();
            var postconditions = new List<IPostcondition>();

            var limitCondition = new LessThanOrEqual()
                    {
                        LeftOperand = new BoundExpression()
                        {
                            Definition = tmpField,
                            Type = tmpField.Type
                        },
                        RightOperand = limit,
                        Type = _host.PlatformType.SystemBoolean,
                    };

            var int32Type = new PlatformType(_host).SystemInt32;

            if (cond != null)
            {
                //cond ==> tmp <= limit =~=
                //!cond || tmp <= limit =~=
                //!cond ? tmp <= limit : true
                postconditions.Add(
                    new Microsoft.Cci.MutableContracts.PostCondition()
                    {
                        Condition = new Conditional()
                        {
                            Condition = new LogicalNot() { Operand = cond },
                            Type = int32Type,
                            ResultIfTrue = new CompileTimeConstant() { Type = int32Type, Value = 1 },
                            ResultIfFalse = limitCondition
                        },
                        OriginalSource = "!cond ? tmp <= limit : true",
                    }
                );
            }
            else
            {
                postconditions.Add(
                    new Microsoft.Cci.MutableContracts.PostCondition()
                    {
                        Condition = limitCondition,
                        OriginalSource = "tmp <= limit",
                    }
                );
            }

            newContract.Postconditions = postconditions;

            var contract = _contractProvider.GetMethodContractFor(method);
            if (contract != null)
            {
                Microsoft.Cci.MutableContracts.ContractHelper.AddMethodContract(newContract, contract);
            }
            _contractProvider.AssociateMethodWithContract(method, newContract);

            return tmpField;
        }

        /// <summary>
        /// Instruments the tmp field, inserting and initialization to 0 at the beginning of the method and an increment before each new
        /// </summary>
        protected void InstrumentTmp(IMethodDefinition method, FieldDefinition tmpField, List<IStatement> incrementBefore, Dictionary<IStatement, IBlockStatement> statementsContainingBlocks)
        {
            var methodStatements = ((BasicBlock)((Microsoft.Cci.ILToCodeModel.SourceMethodBody)method.Body).Block).Statements;

            var int32Type = _host.PlatformType.SystemInt32;

            //initialize the field to 0
            methodStatements.Insert(method.IsConstructor ? 1 : 0, new ExpressionStatement() // after the call to base if it is a ctor
            {
                Expression = new Assignment()
                {
                    Type = int32Type,
                    Source = new CompileTimeConstant() { Type = int32Type, Value = 0 },
                    Target = new TargetExpression() { Type = int32Type, Definition = tmpField }
                }
            });

            //add increments
            foreach (var stmt in incrementBefore)
            {
                var statements = ((BlockStatement)statementsContainingBlocks[stmt]).Statements;

                var index = statements.IndexOf(stmt);
                if (index >= 0)
                {
                    statements.Insert(index, new ExpressionStatement()
                    {
                        Expression = new Assignment()
                        {
                            Type = int32Type,
                            Source = new Addition()
                            {
                                Type = int32Type,
                                LeftOperand = new BoundExpression() { Type = int32Type, Definition = tmpField },
                                RightOperand = new CompileTimeConstant() { Type = int32Type, Value = 1 }
                            },
                            Target = new TargetExpression() { Type = int32Type, Definition = tmpField }
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Appends the statements afters the calls used to calculate the max of the tmp of all the calls
        /// </summary>
        protected void AppendStatementsTmpCopy(IMethodDefinition method, List<StatementTmpCopyInformation> statementsTmpCopy, Dictionary<IStatement, IBlockStatement> statementsContainingBlocks, Dictionary<string, GlobalPolyInfo> globalPolyInfoTmp)
        {
            var block = ((BasicBlock)((Microsoft.Cci.ILToCodeModel.SourceMethodBody)method.Body).Block);
            var methodStatements = block.Statements;

            var int32Type = _host.PlatformType.SystemInt32;

            var callNumber = 1;

            var localsInsertedByType = new Dictionary<ITypeReference, List<LocalDeclarationStatement>>();

            foreach (var statementCopyInformation in statementsTmpCopy)
            {
                var stmt = statementCopyInformation.AfterStatement;
                var statements = ((BlockStatement)statementsContainingBlocks[stmt]).Statements;

                var index = statements.IndexOf(stmt);
                if (index >= 0)
                {
                    IExpression initialValue = new BoundExpression() { Type = int32Type, Definition = GetTmpField(statementCopyInformation.CalledMethod, statementCopyInformation.TmpType) };
                    
                    var localDecl = new LocalDeclarationStatement()
                    {
                        InitialValue = initialValue,
                        LocalVariable = new LocalDefinition() { Type = int32Type, Name = _host.NameTable.GetNameFor("tmp_call_" + callNumber) }
                    };

                    callNumber++;

                    var tmpType = statementCopyInformation.TmpType;
                    if (!localsInsertedByType.ContainsKey(tmpType))
                    {
                        localsInsertedByType.Add(tmpType, new List<LocalDeclarationStatement>());
                    }
                    localsInsertedByType[tmpType].Add(localDecl);

                    if (!statementCopyInformation.InsideLoop)
                    {
                        statements.Insert(index + 1, localDecl);

                        var calledMethodName = statementCopyInformation.CalledMethodReference.ToString();
                        var contractLocalExpr = TranslateMethodContractToLocalExpression(
                               statementCopyInformation.CalledMethodReference,
                               stmt,
                               _memoryContractsInformation.MethodsTmpContracts[calledMethodName][statementCopyInformation.TmpType],
                               statementCopyInformation.CalledMethodArguments);

                        if (!_currentInstrInfo.GlobalPolyInfoTmp.ContainsKey(tmpType.ToString()))
                        {
                            _currentInstrInfo.GlobalPolyInfoTmp.Add(tmpType.ToString(), new GlobalPolyInfo());
                        }
                        _currentInstrInfo.GlobalPolyInfoTmp[tmpType.ToString()].MaxCalls.Add(contractLocalExpr);
                    }
                    else
                    {
                        localDecl.InitialValue = new CompileTimeConstant() { Type = int32Type, Value = 0 };
                        var loopContainingStatements = ((BasicBlock)statementsContainingBlocks[statementCopyInformation.LoopStartAt]).Statements;
                        loopContainingStatements.Insert(0, localDecl);
                        
                        bool doRegularInstrumentation = true;

                        if (!_currentInstrInfo.GlobalPolyInfoTmp.ContainsKey(tmpType.ToString()))
                        {
                            _currentInstrInfo.GlobalPolyInfoTmp.Add(tmpType.ToString(), new GlobalPolyInfo());
                        }

                        if (statementCopyInformation.LoopInvariants != null)
                        {
                            //invariants available, calculate the max directly and insert the assignment after the loop
                            
                            var calledMethodName = statementCopyInformation.CalledMethodReference.ToString();
                            if (_memoryContractsInformation.MethodsTmpContracts.ContainsKey(calledMethodName) &&
                                _memoryContractsInformation.MethodsTmpContracts[calledMethodName].ContainsKey(statementCopyInformation.TmpType))
                            {
                                var contractLocalExpr = TranslateMethodContractToLocalExpression(
                                    statementCopyInformation.CalledMethodReference,
                                    stmt,
                                    _memoryContractsInformation.MethodsTmpContracts[calledMethodName][statementCopyInformation.TmpType],
                                    statementCopyInformation.CalledMethodArguments);
                                var maxExprPolyCond = PolytopesCalculator.MaxOver(contractLocalExpr, statementCopyInformation.LoopInvariants, GetMethodFreeVars(_currentMethod));
                                IExpression maxPolyExpr = null;
                                IExpression maxCondExpr = null;

                                if (maxExprPolyCond != null)
                                {
                                    maxPolyExpr = ExpressionGenerator.GenerateExpressionFromString(maxExprPolyCond.Poly, _host, _currentMethod);
                                    maxCondExpr = ExpressionGenerator.GenerateExpressionFromString(maxExprPolyCond.Cond, _host, _currentMethod);
                                }

                                index = loopContainingStatements.IndexOf(statementCopyInformation.LoopEndAt);
                                if (index >= 0 && maxPolyExpr != null && maxCondExpr != null)
                                {
                                    doRegularInstrumentation = false;
                                    loopContainingStatements.Insert(index + 1,
                                        new ConditionalStatement()
                                        {
                                            Condition = maxCondExpr,
                                            TrueBranch = new ExpressionStatement()
                                            {
                                                Expression = new Assignment()
                                                {
                                                    Type = int32Type,
                                                    Source = maxPolyExpr,
                                                    Target = new TargetExpression() { Type = int32Type, Definition = localDecl.LocalVariable }
                                                }
                                            },
                                            FalseBranch = new EmptyStatement()
                                        });

                                    //store in the global poly info
                                    _currentInstrInfo.GlobalPolyInfoTmp[tmpType.ToString()].MaxCallsLoops.Add(maxExprPolyCond);
                                }
                            }
                        }

                        if (doRegularInstrumentation)
                        {
                            //no invariants available or could not calculate max, calculate instrumenting Math.Max calls

                            if (statementCopyInformation.InsideLoop)
                            {
                                _currentInstrInfo.GlobalPolyInfoTmp[tmpType.ToString()].AllContractsRequiredAvailable = false;
                            }
                            index = statements.IndexOf(stmt); //adjust the index, it could have changed because of the previous localDecl insertion
                            statements.Insert(index + 1,
                                new ExpressionStatement()
                                {
                                    Expression = new Assignment()
                                    {
                                        Type = int32Type,
                                        Source = BuildMaxCall(new BoundExpression() { Definition = localDecl.LocalVariable, Type = int32Type }, initialValue),
                                        Target = new TargetExpression() { Type = int32Type, Definition = localDecl.LocalVariable }
                                    }
                                });
                        }
                    }
                }
            }

            var appendToStatementList = ((BasicBlock)statementsContainingBlocks[_currentStatement]).Statements;

            foreach (var tmpType in localsInsertedByType.Keys)
            {
                bool doRegularInstrumentation = true;
                var tmpField = GetTmpField(method, tmpType);
                var globalInfoForTmp = globalPolyInfoTmp[tmpType.ToString()];

                //check if we can calculate the max with the polytopes calculator
                if (globalInfoForTmp.AllContractsRequiredAvailable)
                {
                    var polys = new List<string>();
                    var conds = new List<string>();

                    conds.AddRange(GetMethodRequiresExprs(method));

                    polys.AddRange(globalInfoForTmp.MaxCalls);
                    foreach (var poly in globalInfoForTmp.MaxCallsLoops)
                    {
                        polys.Add(poly.Poly);
                        conds.Add(poly.Cond);
                    }

                    var max = PolytopesCalculator.MaxPoly(polys, conds, GetMethodFreeVars(method));

                    if (max != null)
                    {
                        var condExpr = ExpressionGenerator.GenerateExpressionFromString(max.Cond, _host, method);
                        var maxExpr = ExpressionGenerator.GenerateExpressionFromString(max.Poly, _host, method);

                        if (condExpr != null && maxExpr != null)
                        {
                            InsertStatementAtBottom(appendToStatementList,
                                new ConditionalStatement()
                                {
                                    Condition = condExpr,
                                    TrueBranch =
                                        new ExpressionStatement()
                                        {
                                            Expression = new Assignment()
                                            {
                                                Type = int32Type,
                                                Source = new Addition()
                                                {
                                                    Type = int32Type,
                                                    LeftOperand = new BoundExpression() { Type = int32Type, Definition = tmpField },
                                                    RightOperand = maxExpr
                                                },
                                                Target = new TargetExpression() { Type = int32Type, Definition = tmpField }
                                            }
                                        },
                                    FalseBranch = new EmptyStatement()
                                });

                            doRegularInstrumentation = false;
                        }
                    }
                }

                if (doRegularInstrumentation)
                {
                    var maxExpr = GetMaxExpressionOverLocals(localsInsertedByType[tmpType]);

                    //add the statement tmpField += Math.Max(...);
                    InsertStatementAtBottom(appendToStatementList,
                        new ExpressionStatement()
                        {
                            Expression = new Assignment()
                            {
                                Type = int32Type,
                                Source = new Addition()
                                {
                                    Type = int32Type,
                                    LeftOperand = new BoundExpression() { Type = int32Type, Definition = tmpField },
                                    RightOperand = maxExpr
                                },
                                Target = new TargetExpression() { Type = int32Type, Definition = tmpField }
                            }
                        });
                }
            }
        }

        protected void GlobalPolyVerifcationTmp(IMethodDefinition method, InstrumentationInformation instrInfo, string type, GlobalPolyInfo polyInfo)
        {
            var methodStatements = ((BasicBlock)((Microsoft.Cci.ILToCodeModel.SourceMethodBody)method.Body).Block).Statements;

            var int32Type = _host.PlatformType.SystemInt32;

            var tmpField = GetTmpField(method, type);

            if (polyInfo.AllContractsRequiredAvailable)
            {
                var polysCalls = new List<string>();
                var condsCalls = new List<string>();

                polysCalls.AddRange(polyInfo.MaxCalls);
                foreach (var polyCond in polyInfo.MaxCallsLoops)
                {
                    polysCalls.Add(polyCond.Poly);
                    condsCalls.Add(polyCond.Cond);
                }

                condsCalls.AddRange(GetMethodRequiresExprs(method));

                PolyCond maxCalls = null;

                if (polysCalls.Count > 0)
                {
                    maxCalls = PolytopesCalculator.MaxPoly(polysCalls, condsCalls, GetMethodFreeVars(method));
                }

                if (polysCalls.Count == 0 || maxCalls != null)
                {
                    var candPolys = new List<string>();
                    var conds = new List<string>();
                    conds.AddRange(GetMethodRequiresExprs(method));
                    if (maxCalls != null)
                    {
                        candPolys.Add(maxCalls.Poly);
                        conds.Add(maxCalls.Cond);
                    }
                    foreach (var poly in polyInfo.DirectQuantityLoops)
                    {
                        candPolys.Add(poly.Poly);
                        conds.Add(poly.Cond);
                    }
                    foreach (var poly in polyInfo.QuantityTransferLoops)
                    {
                        candPolys.Add(poly.Poly);
                        conds.Add(poly.Cond);
                    }
                    if (polyInfo.DirectQuantity > 0)
                    {
                        candPolys.Add(polyInfo.DirectQuantity.ToString());
                    }
                    candPolys.AddRange(polyInfo.QuantityTransfer);
                    if (candPolys.Count > 0)
                    {
                        var candPoly = string.Join(" + ", candPolys);
                        //find tmp contracts for the type and compare with candPoly
                        var methodName = method.ToString();
                        if (_memoryContractsInformation.MethodsTmpContractsWithConds.ContainsKey(methodName))
                        {
                            var methodTmpContracts = _memoryContractsInformation.MethodsTmpContractsWithConds[methodName];
                            //methodName -> type of tmp -> cond -> contract (string expr + code model expr)
                            //public Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>> MethodsTmpContractsWithConds = new Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>>();
                            foreach (var tmpType in methodTmpContracts.Keys)
                            {
                                if (tmpType.ToString() == type)
                                {
                                    foreach (var contractCond in methodTmpContracts[tmpType].Keys)
                                    {
                                        var methodContract = methodTmpContracts[tmpType][contractCond].Item1;
                                        var methodContractExpressionObject = methodTmpContracts[tmpType][contractCond].Item2;
                                        //see if candPoly <= methodContract
                                        string cond;
                                        var newConds = new List<string>();
                                        newConds.AddRange(conds);
                                        if (contractCond != "true")
                                        {
                                            newConds.Add(contractCond);
                                        }
                                        if (PolytopesCalculator.LowerOrEqual(candPoly, methodContract, newConds, GetMethodFreeVars(method), out cond))
                                        {
                                            //insert the statement: if (cond) Contract.Assume(candPoly <= methodContract);
                                            var condExpr = ExpressionGenerator.GenerateExpressionFromString(cond, _host, method);

                                            if (condExpr != null)
                                            {
                                                InsertStatementAtBottom(methodStatements,
                                                    new ConditionalStatement()
                                                    {
                                                        Condition = condExpr,
                                                        TrueBranch =
                                                            new AssumeStatement()
                                                            {
                                                                Condition =
                                                                    new LessThanOrEqual()
                                                                    {
                                                                        LeftOperand = new BoundExpression()
                                                                        {
                                                                            Definition = tmpField,
                                                                            Type = tmpField.Type
                                                                        },
                                                                        RightOperand = methodContractExpressionObject,
                                                                        Type = _host.PlatformType.SystemBoolean,
                                                                    }
                                                            },
                                                        FalseBranch = new EmptyStatement()
                                                    });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Rsd-related methods

        /// <summary>
        /// Stores all the defined rsd fields for all methods
        /// </summary>
        Dictionary<string, FieldDefinition> _rsdFields = new Dictionary<string, FieldDefinition>();

        /// <summary>
        /// Returns the rsd field for the method, type and name, if it doesn't exist it is created and added to the class
        /// </summary>
        protected FieldDefinition GetRsdField(IMethodDefinition method, string rsdName, ITypeReference type)
        {
            return GetRsdField(method, rsdName, type.ToString());
        }

        /// <summary>
        /// Returns the rsd field for the method, type and name, if it doesn't exist it is created and added to the class
        /// </summary>
        protected FieldDefinition GetRsdField(IMethodDefinition method, string rsdName, string type)
        {
            //@todo: modify to support overloads and containig types with the same name
            string rsdId = method.ContainingType.ToString().Replace('.', '_') + "_" + method.Name + "_rsd_" + rsdName + "_" + type.Replace('.', '_');

            if (_rsdFields.ContainsKey(rsdId))
            {
                return _rsdFields[rsdId];
            }

            var rsdField = new FieldDefinition
            {
                Type = _host.PlatformType.SystemInt32,
                Name = _host.NameTable.GetNameFor(rsdId),
                IsStatic = true,
                Visibility = TypeMemberVisibility.Public,
                IsReadOnly = false,
                ContainingTypeDefinition = method.Container
            };

            ((TypeDefinition)method.Container).Fields.Add(rsdField);
            _rsdFields.Add(rsdId, rsdField);

            return rsdField;
        }

        /// <summary>
        /// Adds the rsd contract to the method, it is added as a postcondition where the rsd field of the type is less than or equal to limit
        /// </summary>
        protected FieldDefinition AddRsdContractToMethod(IMethodDefinition method, ITypeReference type, string rsdName, IExpression limit, IExpression cond)
        {
            var rsdField = GetRsdField(method, rsdName, type);

            var newContract = new Microsoft.Cci.MutableContracts.MethodContract();
            var postconditions = new List<IPostcondition>();

            var limitCondition = new LessThanOrEqual()
            {
                LeftOperand = new BoundExpression()
                {
                    Definition = rsdField,
                    Type = rsdField.Type
                },
                RightOperand = limit,
                Type = _host.PlatformType.SystemBoolean,
            };

            var int32Type = new PlatformType(_host).SystemInt32;

            if (cond != null)
            {
                //cond ==> tmp <= limit =~=
                //!cond || tmp <= limit =~=
                //!cond ? tmp <= limit : true
                postconditions.Add(
                    new Microsoft.Cci.MutableContracts.PostCondition()
                    {
                        Condition = new Conditional()
                        {
                            Condition = new LogicalNot() { Operand = cond },
                            Type = int32Type,
                            ResultIfTrue = new CompileTimeConstant() { Type = int32Type, Value = 1 },
                            ResultIfFalse = limitCondition
                        },
                        OriginalSource = "!cond ? rsd <= limit : true",
                    }
                );
            }
            else
            {
                postconditions.Add(
                    new Microsoft.Cci.MutableContracts.PostCondition()
                    {
                        Condition = limitCondition,
                        OriginalSource = "rsd <= limit",
                    }
                );
            }

            newContract.Postconditions = postconditions;

            var contract = _contractProvider.GetMethodContractFor(method);
            if (contract != null)
            {
                Microsoft.Cci.MutableContracts.ContractHelper.AddMethodContract(newContract, contract);
            }
            _contractProvider.AssociateMethodWithContract(method, newContract);

            return rsdField;
        }

        /// <summary>
        /// Instruments the rsd field, inserting and initialization to 0 at the beginning of the method and an increment before each new
        /// </summary>
        protected void InstrumentRsd(IMethodDefinition method, FieldDefinition rsdField, List<IStatement> incrementBefore, Dictionary<IStatement, IBlockStatement> statementsContainingBlocks)
        {
            var methodStatements = ((BasicBlock)((Microsoft.Cci.ILToCodeModel.SourceMethodBody)method.Body).Block).Statements;

            var int32Type = _host.PlatformType.SystemInt32;

            //initialize the field to 0
            methodStatements.Insert(method.IsConstructor ? 1 : 0, new ExpressionStatement() // after the call to base if it is a ctor
            {
                Expression = new Assignment()
                {
                    Type = int32Type,
                    Source = new CompileTimeConstant() { Type = int32Type, Value = 0 },
                    Target = new TargetExpression() { Type = int32Type, Definition = rsdField }
                }
            });

            //add increments
            foreach (var stmt in incrementBefore)
            {
                var statements = ((BlockStatement)statementsContainingBlocks[stmt]).Statements;

                var index = statements.IndexOf(stmt);
                if (index >= 0)
                {
                    statements.Insert(index, new ExpressionStatement()
                    {
                        Expression = new Assignment()
                        {
                            Type = int32Type,
                            Source = new Addition()
                            {
                                Type = int32Type,
                                LeftOperand = new BoundExpression() { Type = int32Type, Definition = rsdField },
                                RightOperand = new CompileTimeConstant() { Type = int32Type, Value = 1 }
                            },
                            Target = new TargetExpression() { Type = int32Type, Definition = rsdField }
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Appends the statements afters the calls used to calculate the max of the tmp of all the calls
        /// </summary>
        protected void AppendStatementsRsdTransference(IMethodDefinition method, List<StatementRsdTransferenceInformation> statementsRsdTransference, Dictionary<IStatement, IBlockStatement> statementsContainingBlocks)
        {
            var int32Type = _host.PlatformType.SystemInt32;

            foreach (var rsdTrans in statementsRsdTransference)
            {
                var stmt = rsdTrans.AfterStatement;
                var statements = ((BlockStatement)statementsContainingBlocks[stmt]).Statements;
                var index = statements.IndexOf(stmt);
                if (index >= 0)
                {
                    //add the statement tmpField += rsdField; after the call
                    statements.Insert(index + 1,
                        new ExpressionStatement()
                        {
                            Expression = new Assignment()
                            {
                                Type = int32Type,
                                Source = new Addition()
                                {
                                    Type = int32Type,
                                    LeftOperand = new BoundExpression() { Type = int32Type, Definition = rsdTrans.AddTarget },
                                    RightOperand = new BoundExpression() { Type = int32Type, Definition = rsdTrans.AddSource }
                                },
                                Target = new TargetExpression() { Type = int32Type, Definition = rsdTrans.AddTarget }
                            }
                        });
                }
            }
        }

        protected void GlobalPolyVerifcationRsd(IMethodDefinition method, InstrumentationInformation instrInfo, string type, string name, GlobalPolyInfo polyInfo)
        {
            var methodStatements = ((BasicBlock)((Microsoft.Cci.ILToCodeModel.SourceMethodBody)method.Body).Block).Statements;

            var int32Type = _host.PlatformType.SystemInt32;

            var rsdField = GetRsdField(method, name, type);

            if (polyInfo.AllContractsRequiredAvailable)
            {
                var candPolys = new List<string>();
                var conds = new List<string>();
                conds.AddRange(GetMethodRequiresExprs(method));
                foreach (var poly in polyInfo.DirectQuantityLoops)
                {
                    candPolys.Add(poly.Poly);
                    conds.Add(poly.Cond);
                }
                foreach (var poly in polyInfo.QuantityTransferLoops)
                {
                    candPolys.Add(poly.Poly);
                    conds.Add(poly.Cond);
                }
                if (polyInfo.DirectQuantity > 0)
                {
                    candPolys.Add(polyInfo.DirectQuantity.ToString());
                }
                candPolys.AddRange(polyInfo.QuantityTransfer);
                if (candPolys.Count > 0)
                {
                    var candPoly = string.Join(" + ", candPolys);
                    //find rsd contracts for the type and compare with candPoly
                    var methodName = method.ToString();
                    if (_memoryContractsInformation.MethodsRsdContractsWithConds.ContainsKey(methodName))
                    {
                        var methodRsdContracts = _memoryContractsInformation.MethodsRsdContractsWithConds[methodName];
                        //methodName -> rsd name -> type of rsd -> cond -> contract (string expr + code model expr)
                        //public Dictionary<string, Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>>> MethodsRsdContractsWithConds = new Dictionary<string, Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>>>();
                        foreach (var rsdName in methodRsdContracts.Keys)
                        {
                            foreach (var rsdType in methodRsdContracts[rsdName].Keys)
                            {
                                if (rsdName == name && rsdType.ToString() == type)
                                {
                                    foreach (var contractCond in methodRsdContracts[rsdName][rsdType].Keys)
                                    {
                                        var methodContract = methodRsdContracts[rsdName][rsdType][contractCond].Item1;
                                        var methodContractExpressionObject = methodRsdContracts[rsdName][rsdType][contractCond].Item2;
                                        //see if candPoly <= methodContract
                                        string cond;
                                        var newConds = new List<string>();
                                        newConds.AddRange(conds);
                                        if (contractCond != "true")
                                        {
                                            newConds.Add(contractCond);
                                        }
                                        if (PolytopesCalculator.LowerOrEqual(candPoly, methodContract, newConds, GetMethodFreeVars(method), out cond))
                                        {
                                            //insert the statement: if (cond) Contract.Assume(candPoly <= methodContract);
                                            var condExpr = ExpressionGenerator.GenerateExpressionFromString(cond, _host, method);

                                            if (condExpr != null)
                                            {
                                                InsertStatementAtBottom(methodStatements,
                                                    new ConditionalStatement()
                                                    {
                                                        Condition = condExpr,
                                                        TrueBranch =
                                                            new AssumeStatement()
                                                            {
                                                                Condition =
                                                                    new LessThanOrEqual()
                                                                    {
                                                                        LeftOperand = new BoundExpression()
                                                                        {
                                                                            Definition = rsdField,
                                                                            Type = rsdField.Type
                                                                        },
                                                                        RightOperand = methodContractExpressionObject,
                                                                        Type = _host.PlatformType.SystemBoolean,
                                                                    }
                                                            },
                                                        FalseBranch = new EmptyStatement()
                                                    });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region LoopsPoly

        protected void AppendStatementsLoopPolyAdd(IMethodDefinition method, PolyAddInformation polyAddInfo, Dictionary<IStatement, IBlockStatement> statementsContainingBlocks)
        {
            var int32Type = _host.PlatformType.SystemInt32;
            var boolType = _host.PlatformType.SystemBoolean;

            var stmt = polyAddInfo.AfterStatement;
            var statements = ((BlockStatement)statementsContainingBlocks[stmt]).Statements;
            var index = statements.IndexOf(stmt);
            if (index >= 0)
            {
                var cond = ExpressionGenerator.GenerateExpressionFromString(polyAddInfo.Poly.Cond, _host, _currentMethod);
                var polyExpr = ExpressionGenerator.GenerateExpressionFromString(polyAddInfo.Poly.Poly, _host, _currentMethod);

                if (cond != null && polyExpr != null)
                {
                    //add the statement if (cond) field += poly;
                    statements.Insert(index + 1,
                        new ConditionalStatement()
                        {
                            Condition = cond,
                            TrueBranch = new ExpressionStatement()
                                         {
                                             Expression = new Assignment()
                                             {
                                                 Type = int32Type,
                                                 Source = new Addition()
                                                 {
                                                     Type = int32Type,
                                                     LeftOperand = new BoundExpression() { Type = int32Type, Definition = polyAddInfo.AddTarget },
                                                     RightOperand = polyExpr
                                                 },
                                                 Target = new TargetExpression() { Type = int32Type, Definition = polyAddInfo.AddTarget }
                                             }
                                         },
                            FalseBranch = new EmptyStatement()
                        });
                }
            }
        }

        #endregion

        #region Max helpers

        protected IExpression GetMaxExpressionOverLocals(List<LocalDeclarationStatement> locals)
        {
            var int32Type = _host.PlatformType.SystemInt32;

            if (locals.Count == 0)
            {
                return new CompileTimeConstant() { Type = int32Type, Value = 0 };
            }
            else if (locals.Count == 1)
            {
                return new BoundExpression() { Type = int32Type, Definition = locals[0].LocalVariable };
            }
            else
            {
                var local = locals[0];
                locals.RemoveAt(0);

                return BuildMaxCall(new BoundExpression() { Definition = local.LocalVariable, Type = int32Type }, GetMaxExpressionOverLocals(locals));
            }
        }

        protected MethodCall BuildMaxCall(IExpression expr1, IExpression expr2)
        {
            var int32Type = _host.PlatformType.SystemInt32;
            var nameTable = _host.NameTable;
            var coreAssembly = _host.LoadAssembly(_host.CoreAssemblySymbolicIdentity);
            var systemMath = UnitHelper.FindType(nameTable, coreAssembly, "System.Math");
            var max = TypeHelper.GetMethod(systemMath, nameTable.GetNameFor("Max"), int32Type, int32Type);

            return new MethodCall()
                {
                    Type = int32Type,
                    IsStaticCall = true,
                    MethodToCall = max,
                    Arguments = new List<IExpression>()
                        {
                            expr1,
                            expr2
                        }
                };
        }

        #endregion

        #region Other Helpers

        protected bool TypeListContainsType(List<ITypeReference> list, ITypeReference type)
        {
            foreach (var t in list)
            {
                if (t.ToString() == type.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        protected string LocationsToString(IEnumerable<ILocation> locations)
        {
            IEnumerator<ILocation> i = locations.GetEnumerator();
            if (i.MoveNext())
            {
                var loc = (IPrimarySourceLocation)i.Current;
                //eg c:\\...\\SomeClass.cs,183,13,183,39
                return String.Format("{0},{1},{2},{3},{4}", loc.Document.Location.Replace("\\", "\\\\"), loc.StartLine, loc.StartColumn, loc.EndLine, loc.EndColumn);
            }

            return null;
        }

        protected string GetLocalName(ILocalDefinition local)
        {
            var name = local.Name.Value;
            if (_pdbReader != null)
            {
                foreach (IPrimarySourceLocation psloc in _pdbReader.GetPrimarySourceLocationsForDefinitionOf(local))
                {
                    if (psloc.Source.Length > 0)
                    {
                        name = psloc.Source;
                        break;
                    }
                }
            }
            return name;
        }

        protected string GetStatementSource(IStatement statement)
        {
            var locsIt = statement.Locations.GetEnumerator();
            if (locsIt.MoveNext())
            {
                return ((IPrimarySourceLocation)locsIt.Current).Source;
            }
            return "";
        }

        protected Dictionary<IMethodReference, Regex> reCallThisExprs = new Dictionary<IMethodReference, Regex>();

        protected string GetCallStatementThisExpr(IMethodReference methodReference, IStatement statement)
        {
            Regex reCallThis;
            if (reCallThisExprs.ContainsKey(methodReference))
            {
                reCallThis = reCallThisExprs[methodReference];
            }
            else
            {
                reCallThis = new Regex(string.Format(@"([^=]*=[ ]*)?(.*\.|){0}\(", methodReference.Name.Value), RegexOptions.Compiled);
                reCallThisExprs.Add(methodReference, reCallThis);
            }
            var source = GetStatementSource(statement);
            var matches = reCallThis.Matches(source);
            var thisExpr = "";
            if (matches.Count > 0 && matches[0].Groups.Count > 2)
            {
                thisExpr = matches[0].Groups[2].Value.Trim();
                if (thisExpr.EndsWith("."))
                {
                    thisExpr = thisExpr.Substring(0, thisExpr.Length - 1);
                }
                if (thisExpr == "")
                {
                    thisExpr = "this";
                }
                //prepend "this" if needed
                else if (!thisExpr.StartsWith("this."))
                {
                    //check if it is:
                    //-a call to a self method without this --> LocalMethod();
                    //-an assigment to a local attribute without this --> localAttr = Method();

                    MethodCall methodCall = null;

                    if (statement is ExpressionStatement)
                    {
                        var expr = (ExpressionStatement)statement;
                        if (expr.Expression is MethodCall)
                        {
                            methodCall = (MethodCall)expr.Expression;
                        }
                        if (expr.Expression is Assignment)
                        {
                            var assing = (Assignment)expr.Expression;
                            if (assing.Source is MethodCall)
                            {
                                methodCall = (MethodCall)assing.Source;
                            }
                        }
                    }

                    if (methodCall != null && methodCall.ThisArgument is BoundExpression &&
                        ((BoundExpression)methodCall.ThisArgument).Instance is ThisReference)
                    {
                        thisExpr = "this." + thisExpr;
                    }
                }
            }
            else if (methodReference.Name.Value == ".ctor")
            {
                //T t = new T();
                reCallThis = new Regex(@"[ ]+(.*)[ ]+=[ ]+new");
                var matchesCtor = reCallThis.Matches(source);
                if (matchesCtor.Count > 0 && matchesCtor[0].Groups.Count > 1)
                {
                    thisExpr = matchesCtor[0].Groups[1].Value.Trim();
                }
                else
                {
                    //expr = new T();
                    reCallThis = new Regex(@"(.*)[ ]+=[ ]+new");
                    matchesCtor = reCallThis.Matches(source);
                    if (matchesCtor.Count > 0 && matchesCtor[0].Groups.Count > 1)
                    {
                        thisExpr = matchesCtor[0].Groups[1].Value.Trim();
                    }
                }
            }
            return thisExpr;
        }

        static Regex reCallReturn = new Regex(@"([^=]+)=", RegexOptions.Compiled);

        protected string GetReturnExprFromStatement(IStatement statement)
        {
            var expr = "";
            var source = GetStatementSource(statement);
            var matches = reCallReturn.Matches(source);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
            {
                expr = matches[0].Groups[1].Value.Trim();
                if (expr.Contains(" "))
                {
                    expr = expr.Split(new char[] { ' ' }, 2)[1];
                }
                //prepend "this" if needed
                if (!expr.StartsWith("this.") &&
                    statement is ExpressionStatement &&
                    ((ExpressionStatement)statement).Expression is Assignment &&
                    ((Assignment)((ExpressionStatement)statement).Expression).Target.Instance is ThisReference)
                {
                    expr = "this." + expr;
                }
            }
            return expr;
        }

        protected string GetBoundRsdExprFromStatement(IMethodReference methodReference, IStatement statement, string calledMethodExpr, IEnumerable<IExpression> arguments)
        {
            var expr = "";
            if (calledMethodExpr == "this")
            {
                expr = GetCallStatementThisExpr(methodReference, statement);
            }
            else if (calledMethodExpr.StartsWith("this."))
            {
                expr = calledMethodExpr.Replace("this.", GetCallStatementThisExpr(methodReference, statement) + ".");
            }
            else //param
            {
                var callSrc = GetStatementSource(statement);

                var argsRe = new List<String>();
                var argsLen = ((IList<IExpression>)arguments).Count;
                for (int i = 1; i <= argsLen; i++)
                {
                    argsRe.Add("(.*)");
                }

                var name = methodReference.Name.Value;

                if (name == ".ctor")
                {
                    name = methodReference.ResolvedMethod.ContainingTypeDefinition.ToString();
                    var parts = name.Split('.');
                    name = parts[parts.Length - 1];
                }

                Regex reParams = new Regex(string.Format(@"{0}\({1}\)", name, string.Join(",", argsRe)));

                var argsExprs = new List<string>();
                var matches = reParams.Matches(callSrc);
                if (matches.Count > 0 && matches[0].Groups.Count == argsLen + 1)
                {
                    for (int i = 1; i <= argsLen; i++)
                    {
                        argsExprs.Add(matches[0].Groups[i].Value.Trim());
                    }
                }

                var exprObjParts = calledMethodExpr.Split(new char[] { '.' }, 2);

                int argNumber = 0;
                foreach (var param in methodReference.ResolvedMethod.Parameters)
                {
                    if (exprObjParts[0] == param.Name.Value)
                    {
                        if (argNumber < argsExprs.Count)
                        {
                            //this replaces the name of the arg in called method with the local object used as arg
                            expr = argsExprs[argNumber] + (exprObjParts.Length > 1 ? ("." + exprObjParts[1]) : "");
                            break;
                        }
                    }
                    argNumber++;
                }
            }
            return expr;
        }

        protected Regex _reContractSymbols = new Regex(@"([a-zA-Z0-9_\.]+)", RegexOptions.Compiled);
        protected Regex _reNum = new Regex(@"[0-9]+(.[0-9]+)?", RegexOptions.Compiled);

        protected string TranslateMethodContractToLocalExpression(IMethodReference methodReference, IStatement statement, string methodContract, IEnumerable<IExpression> arguments)
        {
            return _reContractSymbols.Replace(methodContract, delegate(Match m)
                {
                    var expr = m.Groups[1].Value;

                    if (_reNum.IsMatch(expr))
                    {
                        return expr;
                    }

                    if (methodContract == "this")
                    {
                        expr = GetCallStatementThisExpr(methodReference, statement);
                    }
                    else if (methodContract.StartsWith("this."))
                    {
                        expr = methodContract.Replace("this.", GetCallStatementThisExpr(methodReference, statement) + ".");
                    }
                    else //param
                    {
                        var callSrc = GetStatementSource(statement);

                        var argsRe = new List<String>();
                        var argsLen = ((IList<IExpression>)arguments).Count;
                        for (int i = 1; i <= argsLen; i++)
                        {
                            argsRe.Add("(.*)");
                        }

                        var name = methodReference.Name.Value;

                        if (name == ".ctor")
                        {
                            name = methodReference.ResolvedMethod.ContainingTypeDefinition.ToString();
                            var parts = name.Split('.');
                            name = parts[parts.Length - 1];
                        }

                        Regex reParams = new Regex(string.Format(@"{0}\({1}\)", name, string.Join(",", argsRe)));

                        var argsExprs = new List<string>();
                        var matches = reParams.Matches(callSrc);
                        if (matches.Count > 0 && matches[0].Groups.Count == argsLen + 1)
                        {
                            for (int i = 1; i <= argsLen; i++)
                            {
                                argsExprs.Add(matches[0].Groups[i].Value.Trim());
                            }
                        }

                        var exprObjParts = expr.Split(new char[] { '.' }, 2);

                        int argNumber = 0;
                        foreach (var param in methodReference.ResolvedMethod.Parameters)
                        {
                            if (exprObjParts[0] == param.Name.Value)
                            {
                                if (argNumber < argsExprs.Count)
                                {
                                    //this replaces the name of the arg in called method with the local object used as arg
                                    expr = argsExprs[argNumber] + (exprObjParts.Length > 1 ? ("." + exprObjParts[1]) : "");
                                    break;
                                }
                            }
                            argNumber++;
                        }
                    }

                    return expr;
                });
        }

        protected Dictionary<IMethodDefinition, List<string>> _methodsFreeVars = new Dictionary<IMethodDefinition, List<string>>();

        protected List<string> GetMethodFreeVars(IMethodDefinition method)
        {
            if (_methodsFreeVars.ContainsKey(method))
            {
                return _methodsFreeVars[method];
            }
            List<string> freeVars = new List<string>() { "this" };
            foreach (var param in method.Parameters)
            {
                freeVars.Add(param.Name.Value);
            }
            _methodsFreeVars.Add(method, freeVars);
            return freeVars;
        }

        protected string JoinLoopInvariants(List<HashSet<string>> loopInvariants)
        {
            var invs = new List<string>();
            foreach (var invsSet in loopInvariants)
            {
                invs.AddRange(invsSet);
            }
            return string.Join(" && ", invs);
        }

        protected Dictionary<IMethodDefinition, List<string>> _methodsRequires = new Dictionary<IMethodDefinition, List<string>>();

        protected List<string> GetMethodRequiresExprs(IMethodDefinition method)
        {
            if (_methodsRequires.ContainsKey(method))
            {
                return _methodsRequires[method];
            }

            List<string> reqs = new List<string>();

            var contract = _contractProvider.GetMethodContractFor(method);

            if (contract != null)
            {
                foreach (var req in contract.Preconditions)
                {
                    if (!string.IsNullOrEmpty(req.OriginalSource))
                    {
                        reqs.Add(req.OriginalSource);
                    }
                }
            }

            _methodsRequires.Add(method, reqs);

            return reqs;
        }

        protected void InsertStatementAtBottom(List<IStatement> statements, IStatement statement)
        {
            while (statements[statements.Count - 1] is BasicBlock)
            {
                statements = ((BasicBlock)statements[statements.Count - 1]).Statements;
            }
            var index = statements.Count;
            if (statements[index - 1] is ReturnStatement)
            {
                index--;
            }
            statements.Insert(index, statement);
        }

        #endregion
    }
}
