using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;

namespace Example
{
    class C2
    {
        public static Contract.Memory.RsdType rsdParam;

        public C2(SomeClass param)
        {
            Contract.Memory.BindRsd(rsdParam, param);
            Contract.Memory.Rsd<SomeClass>(rsdParam, 1);

            Contract.Memory.DestRsd(rsdParam);
            param = new SomeClass();
        }
    }
}
