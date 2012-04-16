using System;
using Contracts;
using Assembly2;

namespace Assembly1
{
    public class C
    {
        public void M()
        {
            Contract.Memory.Tmp<D>(2); //1 is wrong, 2 is ok, if the cross assembly verification works

            Contract.Memory.DestTmp();
            D d = new D();
            d.M();
        }
    }
}
