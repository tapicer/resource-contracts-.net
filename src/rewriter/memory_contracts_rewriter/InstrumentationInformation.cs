using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci.MutableCodeModel;
using Microsoft.Cci;

namespace memory_contracts_rewriter
{
    public class InstrumentationInformation
    {
        public InstrumentationInformation()
        {
            Instrument = true;
        }

        #region Tmp
        
        public Dictionary<FieldDefinition, List<IStatement>> TmpFieldsToInstrument { get; set; }
        public bool StoreNextTmp { get; set; }
        public List<StatementTmpCopyInformation> StatementsTmpCopy { get; set; }
        public Dictionary<string, GlobalPolyInfo> GlobalPolyInfoTmp { get; set; }

        #endregion

        #region Rsd

        public Dictionary<FieldDefinition, List<IStatement>> RsdFieldsToInstrument { get; set; }
        public bool StoreNextRsd { get; set; }
        public string NextStoreRsdName { get; set; }
        public List<RsdNames> AddToTmpRsdSources { get; set; }
        public List<RsdNames> AddToRsdRsdSources { get; set; }
        public List<StatementRsdTransferenceInformation> StatementsRsdTransference { get; set; }
        public Dictionary<Tuple<string, string>, GlobalPolyInfo> GlobalPolyInfoRsd { get; set; }

        #endregion

        public bool TrustNextAnnotation { get; set; }
        public Dictionary<IStatement, IBlockStatement> StatementsContainingBlocks { get; set; }
        public bool Instrument { get; set; }
        public PolyCond NextStmtPoly { get; set; }
        public List<PolyAddInformation> LoopsPolyAdd { get; set; }
    }

    public class StatementTmpCopyInformation
    {
        public IStatement AfterStatement { get; set; }
        public IMethodDefinition CalledMethod { get; set; }
        public ITypeReference TmpType { get; set; }
        public bool InsideLoop { get; set; }
        public IStatement LoopStartAt { get; set; }
        public IStatement LoopEndAt { get; set; }
        public string LoopInvariants { get; set; }
        public IMethodReference CalledMethodReference { get; set; }
        public IEnumerable<IExpression> CalledMethodArguments { get; set; }
    }

    public struct StatementRsdTransferenceInformation
    {
        public IStatement AfterStatement { get; set; }
        public IFieldDefinition AddTarget { get; set; }
        public IFieldDefinition AddSource { get; set; }
    }

    public struct RsdNames
    {
        public string LocalRsdName { get; set; }
        public string CallRsdName { get; set; }
        public bool Trust { get; set; }
        public IStatement Statement { get; set; }
    }

    public struct PolyAddInformation
    {
        public PolyCond Poly { get; set; }
        public IStatement AfterStatement { get; set; }
        public IFieldDefinition AddTarget { get; set; }
    }
}
