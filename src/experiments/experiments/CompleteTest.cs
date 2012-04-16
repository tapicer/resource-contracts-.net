using System;
using System.Collections.Generic;
using Contracts;

namespace experiments
{
    public class CompleteTest
    {
        public void CuadraticTmp(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);

            Contract.Memory.Tmp<CompleteTest>(n * n);

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);
                for (int j = 1; j <= n; j++)
                {
                    Contract.Memory.IterationSpace(1 <= j && j <= n);
                    Contract.Memory.DestTmp();
                    new CompleteTest();
                }
            }
        }

        public CompleteTest[] CuadraticRsd(int n, int m)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);
            System.Diagnostics.Contracts.Contract.Requires(m >= 0);

            Contract.Memory.Rsd<CompleteTest>(Contract.Memory.Return, n * m);

            CompleteTest[] elems = new CompleteTest[n * m];

            for (int i = 0; i < n; i++)
            {
                Contract.Memory.IterationSpace(0 <= i && i < n);

                for (int j = 0; j < m; j++)
                {
                    Contract.Memory.IterationSpace(0 <= j && j < m);

                    Contract.Memory.DestRsd(Contract.Memory.Return);
                    elems[i * n + j] = new CompleteTest();
                }
            }

            return elems;
        }

        public void CuadraticTmpMaxLoop(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);

            Contract.Memory.Tmp<CompleteTest>(n * n);

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);

                this.CuadraticTmp(i);
            }
        }

        public void CuadraticTmpAddLoop(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);

            Contract.Memory.Tmp<CompleteTest>(n / 6 + (n * n) / 2 + (n * n * n) / 3);

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);

                Contract.Memory.AddTmp(Contract.Memory.Return);
                CuadraticRsd(i, i);
            }
        }

        public CompleteTest[] CuadraticRsdAddLoop(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);

            Contract.Memory.Rsd<CompleteTest>(Contract.Memory.Return, n / 6 + (n * n) / 2 + (n * n * n) / 3);

            CompleteTest[] r = null;

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);

                Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
                r = CuadraticRsd(i, i);
            }

            return r;
        }

        public void CompleteTmp(int n)
        {
            ////cota exacta
            //System.Diagnostics.Contracts.Contract.Requires(n >= 1);
            //Contract.Memory.Tmp<CompleteTest>(1 + n * n + n * n + n + n * n + n / 6 + (n * n) / 2 + (n * n * n) / 3);

            ////cota superior
            //System.Diagnostics.Contracts.Contract.Requires(n >= 1);
            //Contract.Memory.Tmp<CompleteTest>(n * n * n + 4 * n * n + 2 * n);

            //cota superior considerando n == 0, con condiciones
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);
            Contract.Memory.Tmp<CompleteTest>(1, n == 0);
            Contract.Memory.Tmp<CompleteTest>(n * n * n + 4 * n * n + 2 * n, n >= 1);

            Contract.Memory.DestTmp();
            new CompleteTest(); //cant_directa --> 1

            CuadraticTmpMaxLoop(n); //tmp_call_1 --> n * n

            Contract.Memory.AddTmp(Contract.Memory.Return);
            CuadraticRsd(n, n); //cant_transf_1 --> n * n

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);

                Contract.Memory.DestTmp();
                new CompleteTest(); //cant_directa_loop_1 --> n

                CuadraticTmpMaxLoop(i); //max_call_loop_1 --> n * n

                Contract.Memory.AddTmp(Contract.Memory.Return);
                CuadraticRsd(i, i); //cant_transf_loop_1 --> n / 6 + (n * n) / 2 + (n * n * n) / 3
            }
        }

        public CompleteTest[] CompleteRsd(int n)
        {
            ////cota exacta
            //System.Diagnostics.Contracts.Contract.Requires(n >= 1);
            //Contract.Memory.Rsd<CompleteTest>(Contract.Memory.Return, n * n + 1 + n / 6 + (n * n) / 2 + (n * n * n) / 3 + n);

            ////cota superior
            //System.Diagnostics.Contracts.Contract.Requires(n >= 1);
            //Contract.Memory.Rsd<CompleteTest>(Contract.Memory.Return, n * n * n + 2 * n * n + 2 * n + 1);

            //cota superior considerando n == 0, con condiciones
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);
            Contract.Memory.Rsd<CompleteTest>(Contract.Memory.Return, 1, n == 0);
            Contract.Memory.Rsd<CompleteTest>(Contract.Memory.Return, n * n * n + 2 * n * n + 2 * n, n >= 1);

            CompleteTest[] r = new CompleteTest[1];

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
            r = CuadraticRsd(n, n); //cant_transf_1 --> n * n

            Contract.Memory.DestRsd(Contract.Memory.Return);
            r[0] = new CompleteTest(); //cant_directa --> 1

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);

                Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
                r = CuadraticRsd(i, i); //cant_transf_loop_1 --> n / 6 + (n * n) / 2 + (n * n * n) / 3

                Contract.Memory.DestRsd(Contract.Memory.Return);
                r[0] = new CompleteTest(); //cant_directa_loop_1 --> n
            }

            return r;
        }
    }
}
