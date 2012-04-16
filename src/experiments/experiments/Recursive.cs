using System;
using Contracts;

namespace experiments
{
    class Recursive
    {
        public void Rec(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 1);
            Contract.Memory.Tmp<Recursive>(n);

            Contract.Memory.DestTmp();
            new Recursive();

            if (n > 1)
            {
                Rec(n - 1);
            }
        }
    }
}
