using System;
using Contracts;

namespace experiments
{
    class PointsFail
    {
        public PointsFail attr;

        public PointsFail RsdPorRet()
        {
            Contract.Memory.Rsd<PointsFail>(Contract.Memory.Return, 1);

            Contract.Memory.DestRsd(Contract.Memory.Return);
            return new PointsFail();
        }

        public PointsFail NecesitaTrust()
        {
            Contract.Memory.Tmp<PointsFail>(3);
            Contract.Memory.Rsd<PointsFail>(Contract.Memory.Return, 1);

            Contract.Memory.DestTmp();
            PointsFail p1 = new PointsFail();

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
            p1.attr = RsdPorRet();

            Contract.Memory.DestTmp();
            PointsFail p2 = new PointsFail();

            Contract.Memory.AddTmp(Contract.Memory.Return, true); //se necesita trust
            p2.attr = RsdPorRet();

            return p1.attr;
        }
    }
}
