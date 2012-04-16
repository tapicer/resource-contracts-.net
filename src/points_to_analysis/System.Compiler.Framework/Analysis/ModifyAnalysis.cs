using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Compiler.Contracts;

using System.Compiler;


namespace System.Compiler {



  /// <summary>
  /// Summary description for ModifiesAnalyzer.
  /// </summary>
  public class ModifiesAnalyzer:StandardVisitor {
    TypeSystem typeSystem;
    public ModifiesAnalyzer(TypeSystem t) {
      typeSystem=t;
    }
    public override Method VisitMethod(Method method) {
      Analyzer.WriteLine("\n----------ModifiesAnalyzer: visiting "+method.FullName +"\n");
      ModifiesChecker.check(typeSystem, method);
      return method;
      //return base.VisitMethod(method);
    }
  }

  internal class ModifiesChecker:ForwardDataFlowAnalysis{
    CfgBlock currentBlock;
    TypeSystem typeSystem;
    InstructionVisitor iVisitor;

    static public void check(TypeSystem t, Method m){
      ModifiesChecker mChecker=new ModifiesChecker(t);
      ControlFlowGraph cfg=ControlFlowGraph.For(m);
      mChecker.Run(cfg,
        null // TBD
        );
    } 

    private ModifiesChecker(TypeSystem t){
      typeSystem=t;
      iVisitor=new ModifiesInstructionVisitor();
    }
    
    protected override IDataFlowState Merge(CfgBlock previous, CfgBlock joinPoint, IDataFlowState atMerge, IDataFlowState incoming, out bool resultDiffersFromPreviousMerge, out bool mergeIsPrecise) {
      resultDiffersFromPreviousMerge = false;
      mergeIsPrecise = false;
      return null;
    }
    protected override void SplitExceptions(CfgBlock handler, ref IDataFlowState currentHandlerState, out IDataFlowState nextHandlerState) {
      nextHandlerState = null;
    }
    protected override IDataFlowState VisitStatement(CfgBlock block, Statement statement, IDataFlowState dfstate) {
      // For debug purpose
      try{
        Analyzer.WriteLine(new SampleInstructionVisitor().Visit(statement,null)+"   :::   " +statement.SourceContext.SourceText);
      }catch(Exception){
        Analyzer.WriteLine("Print error: "+statement);
      }
      IDataFlowState result=null;
      try{
        result =(IDataFlowState)(iVisitor.Visit(statement,dfstate));
      }catch(ModifiesException e){
        typeSystem.HandleError(statement,System.Compiler.Error.Warning,":"+e.Message);
      }catch(Exception e){
        typeSystem.HandleError(statement,System.Compiler.Error.Warning,":CFG1:"+e.Message);
      }
      Analyzer.WriteLine(dfstate);
      return dfstate;
    }
    protected override IDataFlowState VisitBlock(CfgBlock block, IDataFlowState stateOnEntry) {
      Debug.Assert(block!=null);

      currentBlock=block;

      Analyzer.Write("---------block: "+block.UniqueId+";");
      Analyzer.Write("   Exit:");
      foreach (CfgBlock b in block.NormalSuccessors)
        Analyzer.Write(b.UniqueId+";");
      if (block.UniqueSuccessor!=null)
        Analyzer.Write("   FallThrough: "+block.UniqueSuccessor+";");
      if (block.ExceptionHandler!=null)
        Analyzer.Write("   ExHandler: "+block.ExceptionHandler.UniqueId+";");
      Analyzer.WriteLine("");

      NonNullables newState=new NonNullables(stateOnEntry);
      if (block.ExceptionHandler!=null)
        this.PushExceptionState(block,newState);
      return base.VisitBlock (block, newState);
    }

  }

  internal class ModifiesInstructionVisitor:InstructionVisitor{
    protected override object DefaultVisit(Statement stat, object arg) {
      throw new ModifiesException("Unimplemented instruction when checking modify clause");
    }
  }

}
