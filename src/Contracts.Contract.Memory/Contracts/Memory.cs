using System;
using System.Diagnostics.Contracts;

namespace Contracts
{
    public static class Contract
    {
        public static class Memory
        {
            #region Residual Types

            /// <summary>
            /// Class that represents a method's kind of residual memory
            /// </summary>
            public class RsdType { }

            /// <summary>
            /// RsdType that represents the return value of a method
            /// </summary>
            public static RsdType Return;

            /// <summary>
            /// RsdType that represents the argument this of a method
            /// </summary>
            public static RsdType This;

            /// <summary>
            /// Binds the expression expr to the RsdType rsdType
            /// </summary>
            public static void BindRsd(RsdType rsdType, object expr) { }

            #endregion

            #region Contracts

            /// <summary>
            /// Contract that indicates that the method has quantity residual memory of type T through the RsdType rsdType under the condition cond
            /// </summary>
            public static void Rsd<T>(RsdType rsdType, int quantity, bool cond) { }

            /// <summary>
            /// Contract that indicates that the method has quantity residual memory of type T through the RsdType rsdType
            /// </summary>
            public static void Rsd<T>(RsdType rsdType, int quantity) { }

            /// <summary>
            /// Contract that indicates that the method has quantity temporal memory of type T under the condition cond
            /// </summary>
            public static void Tmp<T>(int quantity, bool cond) { }

            /// <summary>
            /// Contract that indicates that the method has quantity temporal memory of type T
            /// </summary>
            public static void Tmp<T>(int quantity) { }

            #endregion

            #region Residual transference

            /// <summary>
            /// The following method call residual memory through rsdType is added to the local temporal memory
            /// </summary>
            public static void AddTmp(RsdType rsdType) { }

            /// <summary>
            /// The following method call residual memory through rsdType is added to the local temporal memory,
            /// if trust is true the annotation isn't verified and assumed to be correct
            /// </summary>
            public static void AddTmp(RsdType rsdType, bool trust) { }

            /// <summary>
            /// The following method call residual memory through rsdType_method is added to the local residual memory through rsdType_local
            /// </summary>
            public static void AddRsd(RsdType rsdType_local, RsdType rsdType_method) { }

            /// <summary>
            /// The following method call residual memory through rsdType_method is added to the local residual memory through rsdType_local,
            /// if trust is true the annotation isn't verified and assumed to be correct
            /// </summary>
            public static void AddRsd(RsdType rsdType_local, RsdType rsdType_method, bool trust) { }

            #endregion

            #region news destination

            /// <summary>
            /// Indicates that the destination of the following new is the residual called rsdType
            /// </summary>
            public static void DestRsd(RsdType rsdType) { }

            /// <summary>
            /// Indicates that the destination of the following new is the residual called rsdType,
            /// if trust is true the annotation isn't verified and assumed to be correct
            /// </summary>
            public static void DestRsd(RsdType rsdType, bool trust) { }

            /// <summary>
            /// Indicates tha the destination of the following new is the temporal memory
            /// </summary>
            public static void DestTmp() { }

            /// <summary>
            /// Indicates tha the destination of the following new is the temporal memory,
            /// if trust is true the annotation isn't verified and assumed to be correct
            /// </summary>
            public static void DestTmp(bool trust) { }

            #endregion

            #region Invariants

            /// <summary>
            /// Used to annotate loops with the iteration space
            /// </summary>
            public static void IterationSpace(bool cond) { }

            #endregion
        }
    }
}
