using System;
using Contracts;
using System.Collections.Generic;

namespace Example
{
    class SomeClass
    {
        public void Simple()
        {
            Contract.Memory.Tmp<SomeClass>(1);

            Contract.Memory.DestTmp();
            new SomeClass();
        }

        public void Multiple()
        {
            Contract.Memory.Tmp<C1>(2);
            Contract.Memory.Tmp<C2>(1);

            Contract.Memory.DestTmp();
            new C1();

            Contract.Memory.DestTmp();
            new C2(null);

            Contract.Memory.DestTmp();
            new C1();
        }

        public void Loop(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n > 0);
            Contract.Memory.Tmp<SomeClass>(n);

            for (int i = 0; i < n; i++)
            {
                Contract.Memory.DestTmp();
                new SomeClass();
            }

            //uncomment these lines and the contract doesn't validate
            //Contract.Memory.DestTmp();
            //new SomeClass();
        }

        public void DoubleLoop(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n > 0);
            Contract.Memory.Tmp<SomeClass>(2 * n);

            for (int i = 0; i < n; i++)
            {
                Contract.Memory.DestTmp();
                new SomeClass();
            }

            for (int i = 0; i < n; i++)
            {
                Contract.Memory.DestTmp();
                new SomeClass();
            }

            //uncomment these lines and the contract doesn't validate
            //Contract.Memory.DestTmp();
            //new SomeClass();
        }

        public SomeClass TestNewsTypes()
        {
            Contract.Memory.Tmp<SomeClass>(3);

            Contract.Memory.DestTmp();
            SomeClass s = new SomeClass();

            Contract.Memory.DestTmp();
            new SomeClass();

            Contract.Memory.DestRsd(Contract.Memory.Return);
            s = new SomeClass();

            s.Multiple();

            return s;
        }

        public void TestCond(bool b)
        {
            Contract.Memory.Tmp<SomeClass>(1, b);
            Contract.Memory.Tmp<SomeClass>(2, !b);

            if (b)
            {
                Contract.Memory.DestTmp();
                new SomeClass();
            }
            else
            {
                Contract.Memory.DestTmp();
                new SomeClass();
                Contract.Memory.DestTmp();
                new SomeClass();
            }
        }

        public void TestCalls(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n > 0);
            //Contract.Memory.Tmp<SomeClass>(n); //should be false
            Contract.Memory.Tmp<SomeClass>(n + 1); //should be true

            Contract.Memory.DestTmp();
            new SomeClass();

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);
                LinealCons(i);
            }
        }

        public void LinealCons(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n > 0);
            Contract.Memory.Tmp<SomeClass>(n);

            for (int i = 0; i < n; i++)
            {
                Contract.Memory.DestTmp();
                new SomeClass();
            }
        }

        public void TestCallsNestedLoop(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n > 0);
            Contract.Memory.Tmp<SomeClass>(n);

            for (int i = 1; i <= n; i++)
            {
                LinealCons(i);

                for (int j = 1; j <= n; j++)
                {
                    LinealCons(j);
                }
            }
        }

        public void TestCallsMultiple()
        {
            Contract.Memory.Tmp<C1>(3);
            Contract.Memory.Tmp<C2>(2);

            Contract.Memory.DestTmp();
            new C1();

            Contract.Memory.DestTmp();
            new C2(null);

            Multiple();
        }

        //tests rsds

        public SomeClass attr { get; set; }

        public static Contract.Memory.RsdType rsdParam;

        public void TestRsd(SomeClass s)
        {
            Contract.Memory.BindRsd(rsdParam, s);
            Contract.Memory.Rsd<SomeClass>(rsdParam, 1);

            Contract.Memory.DestRsd(rsdParam);
            s.attr = new SomeClass();
        }

        public SomeClass TestRsd2()
        {
            Contract.Memory.Tmp<SomeClass>(2);
            Contract.Memory.Rsd<SomeClass>(Contract.Memory.Return, 2);

            Contract.Memory.DestTmp(true); //bug, use trust
            SomeClass s = new SomeClass();

            Contract.Memory.AddTmp(rsdParam, true); //bug, use trust
            TestRsd(s);

            Contract.Memory.DestRsd(Contract.Memory.Return);
            SomeClass s2 = new SomeClass();

            Contract.Memory.AddRsd(Contract.Memory.Return, rsdParam);
            s2.TestRsd(s2);

            return s2;
        }

        public SomeClass attrPub;

        public SomeClass TestTmps()
        {
            Contract.Memory.DestRsd(Contract.Memory.This);
            this.attr = new SomeClass();

            Contract.Memory.DestRsd(Contract.Memory.This);
            this.attrPub = new SomeClass();

            Contract.Memory.DestTmp();
            SomeClass s = new SomeClass();

            s.Simple();

            Contract.Memory.DestTmp();
            s = new SomeClass();

            Contract.Memory.DestTmp(true); //bug, use trust
            s.attr = new SomeClass();

            Contract.Memory.DestTmp(true); //bug, use trust
            s.attrPub = new SomeClass();

            return null;
        }

        public static SomeClass staticAttr;

        public SomeClass TestEscapeStatic()
        {
            Contract.Memory.DestRsd(null);
            SomeClass.staticAttr = new SomeClass();

            Contract.Memory.DestRsd(null);
            staticAttr = new SomeClass();

            Contract.Memory.DestRsd(null);
            C1.attr = new SomeClass();

            Contract.Memory.DestRsd(Contract.Memory.Return);
            return new SomeClass();
        }

        public void TestExprs()
        {
            Contract.Memory.DestRsd(Contract.Memory.This);
            this.attrPub.attrPub = new SomeClass();

            Contract.Memory.DestRsd(Contract.Memory.This);
            this.attrPub = new SomeClass();

            Contract.Memory.DestRsd(Contract.Memory.This);
            this.attr = new SomeClass();

            Contract.Memory.DestRsd(Contract.Memory.This);
            this.attr.attr = new SomeClass();

            Contract.Memory.DestRsd(Contract.Memory.This);
            this.attrPub.attr = new SomeClass();

            Contract.Memory.DestRsd(Contract.Memory.This);
            this.attr.attrPub = new SomeClass();

            SomeClass s = new SomeClass();

            Contract.Memory.DestTmp(true); //bug, use trust
            s.attrPub = new SomeClass();

            Contract.Memory.DestTmp(true); //bug, use trust
            s.attrPub.attr = new SomeClass();

            Contract.Memory.DestTmp(true); //bug, use trust
            s.attr.attrPub = new SomeClass();
        }

        public SomeClass M1()
        {
            Contract.Memory.DestTmp();
            SomeClass s = new SomeClass();
            Contract.Memory.DestRsd(Contract.Memory.Return);
            s.attrPub = new SomeClass();
            return s.attrPub;
        }

        public SomeClass M2()
        {
            Contract.Memory.DestRsd(Contract.Memory.Return);
            SomeClass s = new SomeClass();
            Contract.Memory.DestRsd(Contract.Memory.Return);
            s.attrPub = new SomeClass();
            return s;
        }

        public SomeClass M3()
        {
            Contract.Memory.DestRsd(Contract.Memory.Return);
            SomeClass s = new SomeClass();
            Contract.Memory.DestRsd(Contract.Memory.Return);
            SomeClass t = new SomeClass();
            s.attrPub = t;
            return s;
        }

        public static Contract.Memory.RsdType rsdType;

        private SomeClass _attr2;
        public SomeClass attr2
        {
            get { return _attr2; }
            set { _attr2 = value; }
        }

        public SomeClass TestRsdAttr()
        {
            Contract.Memory.BindRsd(rsdType, this._attr2);

            Contract.Memory.DestRsd(rsdType);
            this.attr2 = new SomeClass();

            return null;
        }

        //adds tests

        public SomeClass()
        {
            Contract.Memory.Rsd<SomeClass>(Contract.Memory.This, 1);

            Contract.Memory.DestRsd(Contract.Memory.This);
            this.attrPub = new SomeClass();
        }

        public SomeClass RsdRet()
        {
            Contract.Memory.Rsd<SomeClass>(Contract.Memory.Return, 1);
            Contract.Memory.DestRsd(Contract.Memory.Return);
            return new SomeClass();
        }

        public void RsdThis()
        {
            Contract.Memory.Rsd<SomeClass>(Contract.Memory.This, 1);
            Contract.Memory.DestRsd(Contract.Memory.This);
            this.attrPub = new SomeClass();
        }

        public static Contract.Memory.RsdType rsdAttr;
        public SomeClass RsdThisAttr()
        {
            Contract.Memory.BindRsd(rsdAttr, this.attrPub);
            Contract.Memory.Rsd<SomeClass>(rsdAttr, 1);
            Contract.Memory.DestRsd(rsdAttr);
            this.attrPub = new SomeClass();

            return null;
        }

        public static Contract.Memory.RsdType rsdParam_p;
        public void RsdParam(SomeClass p)
        {
            Contract.Memory.BindRsd(rsdParam_p, p);
            Contract.Memory.Rsd<SomeClass>(rsdParam_p, 1);
            Contract.Memory.DestRsd(rsdParam_p);
            p = new SomeClass();
        }

        public void RsdParamAttr(SomeClass p, SomeClass q)
        {
            Contract.Memory.BindRsd(rsdParam_p, q.attrPub);
            Contract.Memory.Rsd<SomeClass>(rsdParam_p, 1);
            Contract.Memory.DestRsd(rsdParam_p);
            q.attrPub = new SomeClass();
        }

        public static Contract.Memory.RsdType rsdPpp;

        public SomeClass UseRsds(SomeClass ppp)
        {
            Contract.Memory.BindRsd(rsdPpp, ppp);

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
            SomeClass s = RsdRet();

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
            s = RsdRet();

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
            this.attrPub = RsdRet();

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
            attrPub = RsdRet();

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.This);
            s.RsdThis();

            Contract.Memory.AddRsd(Contract.Memory.This, Contract.Memory.This);
            this.RsdThis();

            Contract.Memory.AddRsd(Contract.Memory.This, Contract.Memory.This);
            RsdThis();

            Contract.Memory.AddRsd(Contract.Memory.This, Contract.Memory.This);
            attrPub.RsdThis();

            Contract.Memory.AddRsd(Contract.Memory.This, Contract.Memory.This);
            this.attrPub.RsdThis();

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.This);
            s = attrPub.RsdRet();

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.This);
            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
            s = this.attrPub.RsdRet();

            ///

            Contract.Memory.AddRsd(Contract.Memory.Return, rsdAttr);
            s.RsdThisAttr();

            Contract.Memory.AddRsd(Contract.Memory.This, rsdAttr);
            RsdThisAttr();

            Contract.Memory.AddRsd(Contract.Memory.This, rsdAttr);
            this.RsdThisAttr();

            SomeClass t;

            Contract.Memory.AddRsd(Contract.Memory.This, rsdAttr);
            t = RsdThisAttr();

            Contract.Memory.AddRsd(Contract.Memory.This, rsdAttr);
            t = this.RsdThisAttr();

            Contract.Memory.AddRsd(Contract.Memory.Return, rsdParam_p);
            RsdParam(s);

            Contract.Memory.AddRsd(Contract.Memory.Return, rsdParam_p);
            RsdParamAttr(t, s);

            Contract.Memory.AddTmp(Contract.Memory.This);
            SomeClass s2 = new SomeClass();

            Contract.Memory.AddRsd(Contract.Memory.Return, C2.rsdParam);
            C2 c = new C2(s);

            Contract.Memory.AddRsd(rsdPpp, Contract.Memory.Return);
            ppp.attrPub = RsdRet();

            return s;
        }

        public void RsdDest()
        {
            Contract.Memory.AddRsd(Contract.Memory.This, Contract.Memory.Return);
            //Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return); //fails
            this.attrPub = RsdRet();
        }

        //***//

        public void CuadTestDest(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);

            Contract.Memory.Tmp<SomeClass>(n * n);

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);
                for (int j = 1; j <= n; j++)
                {
                    Contract.Memory.IterationSpace(1 <= j && j <= n);
                    Contract.Memory.DestTmp();
                    new SomeClass();
                }
            }
        }

        public SomeClass[] CuadTestDest2(int n, int m)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);
            System.Diagnostics.Contracts.Contract.Requires(m >= 0);

            Contract.Memory.Rsd<SomeClass>(Contract.Memory.Return, n * m);
            Contract.Memory.Tmp<C1>(n);

            SomeClass[] elems = new SomeClass[n * m];

            for (int i = 0; i < n; i++)
            {
                Contract.Memory.IterationSpace(0 <= i && i < n);

                Contract.Memory.DestTmp();
                new C1();

                for (int j = 0; j < m; j++)
                {
                    Contract.Memory.IterationSpace(0 <= j && j < m);

                    Contract.Memory.DestRsd(Contract.Memory.Return);
                    elems[i * n + j] = new SomeClass();
                }
            }

            return elems;
        }

        public void CuadTestMaxLoop(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);

            Contract.Memory.Tmp<SomeClass>(n * n);

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);

                this.CuadTestDest(i);
            }
        }

        public void CuadAddTmpTest(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);

            Contract.Memory.Tmp<SomeClass>(n / 6 + (n * n) / 2 + (n * n * n) / 3);

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);

                Contract.Memory.AddTmp(Contract.Memory.Return);
                CuadTestDest2(i, i);
            }
        }

        public SomeClass[] CuadAddRsdTest(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);

            Contract.Memory.Rsd<SomeClass>(Contract.Memory.Return, n / 6 + (n * n) / 2 + (n * n * n) / 3);

            SomeClass[] r = null;

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);

                Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
                r = CuadTestDest2(i, i);
            }

            return r;
        }

        public void CompleteTmp(int n)
        {
            ////exact bound!
            //System.Diagnostics.Contracts.Contract.Requires(n >= 1);
            //Contract.Memory.Tmp<SomeClass>(1 + n * n + n * n + n + n * n + n / 6 + (n * n) / 2 + (n * n * n) / 3);

            ////upper bound
            //System.Diagnostics.Contracts.Contract.Requires(n >= 1);
            //Contract.Memory.Tmp<SomeClass>(n * n * n + 4 * n * n + 2 * n);

            //upper bound with conditions
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);
            Contract.Memory.Tmp<SomeClass>(1, n == 0);
            Contract.Memory.Tmp<SomeClass>(n * n * n + 4 * n * n + 2 * n, n >= 1);

            Contract.Memory.DestTmp();
            new SomeClass(); //cant_directa --> 1

            CuadTestMaxLoop(n); //max_tmp_calls --> n * n

            Contract.Memory.AddTmp(Contract.Memory.Return);
            CuadTestDest2(n, n); //cant_transf_1 --> n * n

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);

                Contract.Memory.DestTmp();
                new SomeClass(); //cant_directa_loop_1 --> n

                CuadTestMaxLoop(i); //max_call_loop_1 --> n * n

                Contract.Memory.AddTmp(Contract.Memory.Return);
                CuadTestDest2(i, i); //cant_transf_loop_1 --> n / 6 + (n * n) / 2 + (n * n * n) / 3
            }
        }

        public SomeClass[] CompleteRsd(int n)
        {
            ////exact bound!
            //System.Diagnostics.Contracts.Contract.Requires(n >= 1);
            //Contract.Memory.Rsd<SomeClass>(Contract.Memory.Return, n * n + 1 + n / 6 + (n * n) / 2 + (n * n * n) / 3 + n);

            ////upper bound
            //System.Diagnostics.Contracts.Contract.Requires(n >= 1);
            //Contract.Memory.Rsd<SomeClass>(Contract.Memory.Return, n * n * n + 2 * n * n + 2 * n + 1);

            //upper bound with conditions
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);
            Contract.Memory.Rsd<SomeClass>(Contract.Memory.Return, 1, n == 0);
            Contract.Memory.Rsd<SomeClass>(Contract.Memory.Return, n * n * n + 2 * n * n + 2 * n, n >= 1);

            SomeClass[] r = new SomeClass[1];

            Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
            r = CuadTestDest2(n, n); //cant_transf_1 --> n * n

            Contract.Memory.DestRsd(Contract.Memory.Return);
            r[0] = new SomeClass(); //cant_directa --> 1

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);

                Contract.Memory.AddRsd(Contract.Memory.Return, Contract.Memory.Return);
                r = CuadTestDest2(i, i); //cant_transf_loop_1 --> n / 6 + (n * n) / 2 + (n * n * n) / 3

                Contract.Memory.DestRsd(Contract.Memory.Return);
                r[0] = new SomeClass(); //cant_directa_loop_1 --> n
            }

            return r;
        }

        public void ConsExpr(List<int> nums)
        {
            Contract.Memory.Tmp<SomeClass>(nums.Count * nums.Count);

            for (int i = 1; i <= nums.Count; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= nums.Count);
                CuadTestMaxLoop(i);
            }
        }

        public void ConsExpr2(int[] nums)
        {
            Contract.Memory.Tmp<SomeClass>(nums.Length);

            for (int i = 1; i <= nums.Length; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= nums.Length);

                Contract.Memory.DestTmp();
                new SomeClass();
            }
        }

        public int intProp { get; set; }

        public void ConsExpr3(SomeClass s)
        {
            Contract.Memory.Tmp<SomeClass>(s.intProp);

            s.intProp = 5;

            for (int i = 1; i <= s.intProp; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= s.intProp);

                Contract.Memory.DestTmp();
                new SomeClass();
            }
        }

        public void ConsExpr4()
        {
            Contract.Memory.Tmp<SomeClass>(this.intProp);

            this.intProp = 5;

            for (int i = 1; i <= this.intProp; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= this.intProp);

                Contract.Memory.DestTmp();
                new SomeClass();
            }
        }

        private SomeClass _Field;
        public SomeClass Field { get { return _Field; } set { _Field = value; } }

        public static Contract.Memory.RsdType rsdField;

        public void EscapeFieldTest(SomeClass param)
        {
            Contract.Memory.BindRsd(rsdField, this.Field);
            Contract.Memory.Rsd<SomeClass>(rsdField, 1);

            Contract.Memory.DestRsd(rsdField);
            this.Field = new SomeClass();

            Contract.Memory.BindRsd(rsdParam, param._Field.Field);
            Contract.Memory.Rsd<SomeClass>(rsdParam, 1);

            Contract.Memory.DestRsd(rsdParam);
            param.Field.Field = new SomeClass();
        }

        public int IntField1 { get; set; }

        public void ConsDepField1(SomeClass s)
        {
            Contract.Memory.Tmp<SomeClass>(s.IntField1 * s.IntField1);

            for (int i = 1; i <= s.IntField1; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= s.IntField1);
                for (int j = 1; j <= s.IntField1; j++)
                {
                    Contract.Memory.IterationSpace(1 <= j && j <= s.IntField1);
                    Contract.Memory.DestTmp();
                    new SomeClass();
                }
            }
        }

        public int IntField2;

        public void ConsDepField2(SomeClass s)
        {
            Contract.Memory.Tmp<SomeClass>(s.IntField2 * s.IntField2);

            for (int i = 1; i <= s.IntField2; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= s.IntField2);
                for (int j = 1; j <= s.IntField2; j++)
                {
                    Contract.Memory.IterationSpace(1 <= j && j <= s.IntField2);
                    Contract.Memory.DestTmp();
                    new SomeClass();
                }
            }
        }

        public void MultMinus1(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);
            Contract.Memory.Tmp<SomeClass>(n * (n - 1) / 2);

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);
                for (int j = 1; j <= i; j++)
                {
                    Contract.Memory.IterationSpace(1 <= j && j <= i);
                    Contract.Memory.DestTmp();
                    new SomeClass();
                }
            }
        }

        public void MultMinus2(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);
            Contract.Memory.Tmp<SomeClass>(n * (n + 1) / 2);

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);
                for (int j = 1; j <= i; j++)
                {
                    Contract.Memory.IterationSpace(1 <= j && j <= i);
                    Contract.Memory.DestTmp();
                    new SomeClass();
                }
            }
        }

        public void MultMinus3(int n)
        {
            System.Diagnostics.Contracts.Contract.Requires(n >= 0);
            Contract.Memory.Tmp<SomeClass>(n * (n - 1) / 2);

            for (int i = 1; i <= n; i++)
            {
                Contract.Memory.IterationSpace(1 <= i && i <= n);
                for (int j = 1; j <= i; j++)
                {
                    Contract.Memory.IterationSpace(1 <= j && i <= j);
                    Contract.Memory.DestTmp();
                    new SomeClass();
                }
            }
        }
    }
}
