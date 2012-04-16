using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace memory_contracts_rewriter
{
    public class GlobalPolyInfo
    {
        public GlobalPolyInfo()
        {
            AllContractsRequiredAvailable = true;
            DirectQuantityLoops = new List<PolyCond>();
            MaxCallsLoops = new List<PolyCond>();
            QuantityTransferLoops = new List<PolyCond>();
            DirectQuantity = 0;
            MaxCalls = new List<string>();
            QuantityTransfer = new List<string>();
        }

        public bool AllContractsRequiredAvailable { get; set; }

        //inside loops
        public List<PolyCond> DirectQuantityLoops { get; set; }
        public List<PolyCond> MaxCallsLoops { get; set; } //only for tmps
        public List<PolyCond> QuantityTransferLoops { get; set; }

        //outside loops
        public int DirectQuantity { get; set; }
        public List<string> MaxCalls { get; set; } //only for tmps
        public List<string> QuantityTransfer { get; set; }
    }
}
