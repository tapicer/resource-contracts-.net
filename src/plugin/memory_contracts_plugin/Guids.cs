// Guids.cs
// MUST match guids.h
using System;

namespace DCFCENUBA.memory_contracts_plugin
{
    static class GuidList
    {
        public const string guidmemory_contracts_pluginPkgString = "346f579b-79e4-4e11-9154-1c8ced3b7656";
        public const string guidmemory_contracts_pluginCmdSetString = "54033e0b-e1a1-4adc-8251-d87424df375f";

        public static readonly Guid guidmemory_contracts_pluginCmdSet = new Guid(guidmemory_contracts_pluginCmdSetString);
    };
}