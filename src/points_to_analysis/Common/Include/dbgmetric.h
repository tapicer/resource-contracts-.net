#ifndef __METRIC_H__
#define __METRIC_H__

// ------------------------------------------------------------------
// Predefined metric names

// "CLSID"
extern LPCWSTR metricCLSID;
// "Name"
extern LPCWSTR metricName;
// "Language"
extern LPCWSTR metricLanguage;

// ------------------------------------------------------------------
// General purpose metric routines

//HRESULT GetMetric(LPCWSTR pszMachine, LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, VARIANT* pvarValue, LPWSTR pszAltRoot);
HRESULT __stdcall GetMetric(LPCWSTR pszMachine, LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, DWORD* pdwValue, LPWSTR pszAltRoot);
HRESULT __stdcall GetMetric(LPCWSTR pszMachine, LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, BSTR* pbstrValue, LPWSTR pszAltRoot);
HRESULT __stdcall GetMetric(LPCWSTR pszMachine, LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, GUID* pguidValue, LPWSTR pszAltRoot);
HRESULT __stdcall GetMetric(LPCWSTR pszMachine, LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, GUID* rgguidValues, DWORD* pdwSize, LPWSTR pszAltRoot);

//HRESULT SetMetric(LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, const VARIANT varValue);
HRESULT __stdcall SetMetric(LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, const DWORD dwValue, bool fUserSpecific, LPWSTR pszAltRoot);
HRESULT __stdcall SetMetric(LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, LPCWSTR pszValue, bool fUserSpecific, LPWSTR pszAltRoot);
HRESULT __stdcall SetMetric(LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, REFGUID guidValue, bool fUserSpecific, LPWSTR pszAltRoot);
HRESULT __stdcall SetMetric(LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, GUID* rgguidValues, DWORD dwSize, bool fUserSpecific, LPWSTR pszAltRoot);

HRESULT __stdcall EnumMetricSections(LPCWSTR pszMachine, LPCWSTR pszType, GUID* rgguidSections, DWORD* pdwSize, LPWSTR pszAltRoot);

HRESULT __stdcall RemoveMetric(LPCWSTR pszType, REFGUID guidSection, LPCWSTR pszMetric, LPWSTR pszAltRoot);

// ------------------------------------------------------------------
// Security
// This function will set the security for the root of the metric
// hierarchy and all nodes underneath it. For registry-based metrics,
// this means that the root key and all subkeys will get the given SD.
HRESULT __stdcall SetMetricSecurity(SECURITY_INFORMATION SecurityInfo, PSECURITY_DESCRIPTOR pSD, LPCWSTR pszAltRoot);

// Predefined metric types
// "Engine"
extern LPCWSTR metrictypeEngine;
// "PortSupplier"
extern LPCWSTR metrictypePortSupplier;
// "Exception"
extern LPCWSTR metrictypeException;

// Predefined engine metric names
// AddressBP
extern LPCWSTR metricAddressBP;
// AlwaysLoadLocal
extern LPCWSTR metricAlwaysLoadLocal;
// PutInDebuggeeSession
extern LPCWSTR metricLoadInDebuggeeSession;
// Attach
extern LPCWSTR metricAttach;
// CallStackBP
extern LPCWSTR metricCallStackBP;
// ConditionalBP
extern LPCWSTR metricConditionalBP;
// DataBP
extern LPCWSTR metricDataBP;
// Disassembly
extern LPCWSTR metricDisassembly;
// Dump writing
extern LPCWSTR metricDumpWriting;
// ENC
extern LPCWSTR metricENC;
// Exceptions
extern LPCWSTR metricExceptions;
// FunctionBP
extern LPCWSTR metricFunctionBP;
// HitCountBP
extern LPCWSTR metricHitCountBP;
// JITDebug
extern LPCWSTR metricJITDebug;
// Memory
extern LPCWSTR metricMemory;
// Port supplier
extern LPCWSTR metricPortSupplier;
// Registers
extern LPCWSTR metricRegisters;
// SetNextStatement
extern LPCWSTR metricSetNextStatement;
// SuspendThread
extern LPCWSTR metricSuspendThread;
// WarnIfNoSymbols
extern LPCWSTR metricWarnIfNoSymbols;
// NativeInteropOK
extern LPCWSTR metricNativeInteropOK;

// Predefined EE metric names
// Engine
extern LPCWSTR metricEngine;
// ThisObjectName
extern LPCWSTR metricThisObjectName;

// ------------------------------------------------------------------
// Engine-specific metric routines

HRESULT __stdcall EnumDebugEngines(LPCWSTR pszMachine, REFGUID guidPortSupplier, GUID* rgguidEngines, DWORD* pdwSize, LPWSTR pszAltRoot);

// ------------------------------------------------------------------
// EE-specific metric routines

HRESULT __stdcall GetEEMetric(REFGUID guidLang, REFGUID guidVendor, LPCWSTR pszMetric, BSTR* pbstrValue, LPWSTR pszAltRoot);
HRESULT __stdcall GetEEMetric(REFGUID guidLang, REFGUID guidVendor, LPCWSTR pszMetric, GUID* pguidValue, LPWSTR pszAltRoot);
HRESULT __stdcall GetEEMetric(REFGUID guidLang, REFGUID guidVendor, LPCWSTR pszMetric, GUID* rgguidValues, DWORD* pdwSize, LPWSTR pszAltRoot);

HRESULT __stdcall SetEEMetric(REFGUID guidLang, REFGUID guidVendor, LPCWSTR pszMetric, LPCWSTR pszValue, LPWSTR pszAltRoot);
HRESULT __stdcall SetEEMetric(REFGUID guidLang, REFGUID guidVendor, LPCWSTR pszMetric, REFGUID guidValue, LPWSTR pszAltRoot);
HRESULT __stdcall SetEEMetric(REFGUID guidLang, REFGUID guidVendor, LPCWSTR pszMetric, GUID* rgguidValues, DWORD dwSize, LPWSTR pszAltRoot);

HRESULT __stdcall EnumEEs(GUID* rgguidLang, GUID* rgguidVendor, DWORD* pdwSize, LPWSTR pszAltRoot);

HRESULT __stdcall RemoveEEMetric(REFGUID guidLang, REFGUID guidVendor, LPCWSTR pszMetric, LPWSTR pszAltRoot);

// ------------------------------------------------------------------
// SP-specific metric routines

HRESULT __stdcall GetSPMetric(REFGUID guidSymbolType, LPCWSTR pszStoreType, LPCWSTR pszMetric, BSTR* pbstrValue, LPWSTR pszAltRoot);
HRESULT __stdcall GetSPMetric(REFGUID guidSymbolType, LPCWSTR pszStoreType, LPCWSTR pszMetric, GUID* pguidValue, LPWSTR pszAltRoot);

HRESULT __stdcall SetSPMetric(REFGUID guidSymbolType, LPCWSTR pszStoreType, LPCWSTR pszMetric, LPCWSTR pszValue, LPWSTR pszAltRoot);
HRESULT __stdcall SetSPMetric(REFGUID guidSymbolType, LPCWSTR pszStoreType, LPCWSTR pszMetric, REFGUID guidValue, LPWSTR pszAltRoot);

HRESULT __stdcall RemoveSPMetric(REFGUID guidSymbolType, LPCWSTR pszStoreType, LPCWSTR pszMetric, LPWSTR pszAltRoot);

// Predefined SP store types
// "file"
extern LPCWSTR storetypeFile;
// "metadata"
extern LPCWSTR storetypeMetadata;

// ------------------------------------------------------------------
// Exception metric routines

HRESULT __stdcall GetExceptionMetric(REFGUID guidType, LPCWSTR pszException, DWORD* pdwState, DWORD* pdwCode, LPWSTR pszAltRoot);

HRESULT __stdcall SetExceptionMetric(REFGUID guidType, LPCWSTR pszParent, LPCWSTR pszException, DWORD dwState, DWORD dwCode, LPWSTR pszAltRoot);

HRESULT __stdcall EnumExceptionMetrics(REFGUID guidType, LPCWSTR pszParent, BSTR* rgbstrExceptions, DWORD* rgdwState, DWORD* rgdwCode, DWORD* pdwSize, LPWSTR pszAltRoot);

HRESULT __stdcall RemoveExceptionMetric(REFGUID guidType, LPCWSTR pszParent, LPCWSTR pszException, LPWSTR pszAltRoot);
HRESULT __stdcall RemoveAllExceptionMetrics(REFGUID guidType, LPWSTR pszAltRoot);

#endif // __METRIC_H__
