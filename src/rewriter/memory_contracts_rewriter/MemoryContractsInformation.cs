using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;

namespace memory_contracts_rewriter
{
    class MemoryContractsInformation
    {
        public Dictionary<string, List<ITypeReference>> MethodsTmpTypes { get; set; }
        public Dictionary<string, Dictionary<ITypeReference, string>> MethodsTmpContracts { get; set; }
        public Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>> MethodsTmpContractsWithConds { get; set; }
        public Dictionary<string, List<RsdType>> MethodsRsdTypes { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<ITypeReference, string>>> MethodsRsdContracts { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<ITypeReference, Dictionary<string, Tuple<string, IExpression>>>>> MethodsRsdContractsWithConds { get; set; }
        public Dictionary<string, Dictionary<string, string>> MethodsRsdExprs { get; set; }
    }

    public struct RsdType
    {
        public string Name { get; set; }
        public ITypeReference Type { get; set; }
        public override bool Equals(object obj)
        {
            RsdType other = (RsdType)obj;
            return Name == other.Name && Type.ToString() == other.Type.ToString();
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() + Type.GetHashCode();
        }
    }
}
