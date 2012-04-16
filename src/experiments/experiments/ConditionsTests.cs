using System;
using Contracts;

namespace experiments
{
    class ConditionsTests
    {
        public ConditionsTests[] TestCondSimple(bool b)
        {
            Contract.Memory.Rsd<ConditionsTests>(Contract.Memory.Return, 1, b);
            Contract.Memory.Rsd<ConditionsTests>(Contract.Memory.Return, 2, !b);

            if (b)
            {
                Contract.Memory.DestRsd(Contract.Memory.Return);
                ConditionsTests c = new ConditionsTests();

                return new ConditionsTests[] { c };
            }
            else
            {
                Contract.Memory.DestRsd(Contract.Memory.Return);
                ConditionsTests c1 = new ConditionsTests();

                Contract.Memory.DestRsd(Contract.Memory.Return);
                ConditionsTests c2 = new ConditionsTests();

                return new ConditionsTests[] { c1, c2 };
            }
        }

        public void TestCondLoop(int n, bool b)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 1);

            //no verificables
            Contract.Memory.Tmp<ConditionsTests>(n, b);
            Contract.Memory.Tmp<ConditionsTests>(2 * n, !b);

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.AddTmp(Contract.Memory.Return);
                TestCondSimple(b);
            }
        }
    }
}
