using System;
using Contracts;

namespace Assembly2
{
    public class D
    {
        public void M()
        {
            Contract.Memory.Tmp<D>(1);

            Contract.Memory.DestTmp();
            new D();
        }
    }
}
