
#pragma warning( disable: 4049 )  /* more than 64k source lines */

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 6.00.0347 */
/* at Fri Sep 21 10:51:03 2001
 */
/* Compiler settings for ..\babelservice.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data , no_format_optimization
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 440
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __babelservice_h__
#define __babelservice_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IColorSink_FWD_DEFINED__
#define __IColorSink_FWD_DEFINED__
typedef interface IColorSink IColorSink;
#endif 	/* __IColorSink_FWD_DEFINED__ */


#ifndef __IParseSink_FWD_DEFINED__
#define __IParseSink_FWD_DEFINED__
typedef interface IParseSink IParseSink;
#endif 	/* __IParseSink_FWD_DEFINED__ */


#ifndef __IBabelService_FWD_DEFINED__
#define __IBabelService_FWD_DEFINED__
typedef interface IBabelService IBabelService;
#endif 	/* __IBabelService_FWD_DEFINED__ */


#ifndef __IBabelPackage_FWD_DEFINED__
#define __IBabelPackage_FWD_DEFINED__
typedef interface IBabelPackage IBabelPackage;
#endif 	/* __IBabelPackage_FWD_DEFINED__ */


#ifndef __IBabelProject_FWD_DEFINED__
#define __IBabelProject_FWD_DEFINED__
typedef interface IBabelProject IBabelProject;
#endif 	/* __IBabelProject_FWD_DEFINED__ */


#ifndef __INames_FWD_DEFINED__
#define __INames_FWD_DEFINED__
typedef interface INames INames;
#endif 	/* __INames_FWD_DEFINED__ */


#ifndef __IMethods_FWD_DEFINED__
#define __IMethods_FWD_DEFINED__
typedef interface IMethods IMethods;
#endif 	/* __IMethods_FWD_DEFINED__ */


#ifndef __IDeclarations_FWD_DEFINED__
#define __IDeclarations_FWD_DEFINED__
typedef interface IDeclarations IDeclarations;
#endif 	/* __IDeclarations_FWD_DEFINED__ */


#ifndef __IScope_FWD_DEFINED__
#define __IScope_FWD_DEFINED__
typedef interface IScope IScope;
#endif 	/* __IScope_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 

/* interface __MIDL_itf_babelservice_0000 */
/* [local] */ 


/***************************************************************************
         Copyright (c) Microsoft Corporation, All rights reserved.          
***************************************************************************/



extern RPC_IF_HANDLE __MIDL_itf_babelservice_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_babelservice_0000_v0_0_s_ifspec;


#ifndef __BabelServiceLib_LIBRARY_DEFINED__
#define __BabelServiceLib_LIBRARY_DEFINED__

/* library BabelServiceLib */
/* [helpstring][version][uuid] */ 











enum Severity
    {	SevHint	= 0,
	SevWarning	= SevHint + 1,
	SevError	= SevWarning + 1,
	SevFatal	= SevError + 1
    } ;

enum ParseReason
    {	ReasonColorize	= 0,
	ReasonCheck	= ReasonColorize + 1,
	ReasonMemberSelect	= ReasonCheck + 1,
	ReasonCompleteWord	= ReasonMemberSelect + 1,
	ReasonQuickInfo	= ReasonCompleteWord + 1,
	ReasonMethodTip	= ReasonQuickInfo + 1,
	ReasonMatchBraces	= ReasonMethodTip + 1,
	ReasonHighlightBraces	= ReasonMatchBraces + 1,
	ReasonAutos	= ReasonHighlightBraces + 1,
	ReasonCodeSpan	= ReasonAutos + 1
    } ;

enum CharClass
    {	CharDefault	= 0,
	CharText	= CharDefault,
	CharKeyword	= CharText + 1,
	CharIdentifier	= CharKeyword + 1,
	CharString	= CharIdentifier + 1,
	CharLiteral	= CharString + 1,
	CharOperator	= CharLiteral + 1,
	CharDelimiter	= CharOperator + 1,
	CharEmbedded	= CharDelimiter + 1,
	CharWhiteSpace	= CharEmbedded + 1,
	CharLineComment	= CharWhiteSpace + 1,
	CharComment	= CharLineComment + 1,
	CharDefaultLast	= CharComment
    } ;
typedef long TriggerClass;


enum DefaultTriggerClass
    {	TriggerNone	= 0,
	TriggerMemberSelect	= 0x1,
	TriggerMatchBraces	= 0x2,
	TriggerMethodTip	= 0xf0,
	TriggerParamStart	= 0x10,
	TriggerParamNext	= 0x20,
	TriggerParamEnd	= 0x40,
	TriggerParam	= 0x80
    } ;
typedef long ColorClass;


enum DefaultColorClass
    {	ClassDefault	= 0,
	ClassText	= ClassDefault,
	ClassKeyword	= ClassText + 1,
	ClassComment	= ClassKeyword + 1,
	ClassIdentifier	= ClassComment + 1,
	ClassString	= ClassIdentifier + 1,
	ClassNumber	= ClassString + 1,
	ClassDefaultLast	= ClassNumber
    } ;

enum ScopeKind
    {	ScopeUnknown	= 0,
	ScopeModule	= ScopeUnknown + 1,
	ScopeClass	= ScopeModule + 1,
	ScopeInterface	= ScopeClass + 1,
	ScopeUnion	= ScopeInterface + 1,
	ScopeProcedure	= ScopeUnion + 1,
	ScopeVariable	= ScopeProcedure + 1,
	ScopeBlock	= ScopeVariable + 1
    } ;

enum ScopeAccess
    {	AccessPrivate	= 0,
	AccessProtected	= AccessPrivate + 1,
	AccessPublic	= AccessProtected + 1
    } ;

enum ScopeStorage
    {	StorageConstant	= 0,
	StorageStatic	= StorageConstant + 1,
	StorageMember	= StorageStatic + 1,
	StorageVirtual	= StorageMember + 1,
	StorageParameter	= StorageVirtual + 1,
	StorageResult	= StorageParameter + 1,
	StorageLocal	= StorageResult + 1,
	StorageType	= StorageLocal + 1,
	StorageNone	= StorageType + 1,
	StorageOther	= StorageNone + 1
    } ;
#define	IconGroupSize	( 6 )


enum ScopeIconGroup
    {	IconGroupClass	= 0,
	IconGroupType	= IconGroupClass + 1,
	IconGroupDelegate	= IconGroupType + 1,
	IconGroupType2	= IconGroupDelegate + 1,
	IconGroupType3	= IconGroupType2 + 1,
	IconGroupEvent	= IconGroupType3 + 1,
	IconGroupResource	= IconGroupEvent + 1,
	IconGroupFieldBlue	= IconGroupResource + 1,
	IconGroupInterface	= IconGroupFieldBlue + 1,
	IconGroupTextLine	= IconGroupInterface + 1,
	IconGroupScript	= IconGroupTextLine + 1,
	IconGroupScript2	= IconGroupScript + 1,
	IconGroupMethod	= IconGroupScript2 + 1,
	IconGroupMethod2	= IconGroupMethod + 1,
	IconGroupDiagram	= IconGroupMethod2 + 1,
	IconGroupNameSpace	= IconGroupDiagram + 1,
	IconGroupFormula	= IconGroupNameSpace + 1,
	IconGroupProperty	= IconGroupFormula + 1,
	IconGroupStruct	= IconGroupProperty + 1,
	IconGroupTemplate	= IconGroupStruct + 1,
	IconGroupOpenSquare	= IconGroupTemplate + 1,
	IconGroupBits	= IconGroupOpenSquare + 1,
	IconGroupChannel	= IconGroupBits + 1,
	IconGroupFieldRed	= IconGroupChannel + 1,
	IconGroupUnion	= IconGroupFieldRed + 1,
	IconGroupForm	= IconGroupUnion + 1,
	IconGroupFieldYellow	= IconGroupForm + 1,
	IconGroupMisc1	= IconGroupFieldYellow + 1,
	IconGroupMisc2	= IconGroupMisc1 + 1,
	IconGroupMisc3	= IconGroupMisc2 + 1
    } ;

enum ScopeIconItem
    {	IconItemPublic	= 0,
	IconItemInternal	= IconItemPublic + 1,
	IconItemSpecial	= IconItemInternal + 1,
	IconItemProtected	= IconItemSpecial + 1,
	IconItemPrivate	= IconItemProtected + 1,
	IconItemShortCut	= IconItemPrivate + 1,
	IconItemNormal	= IconItemPublic
    } ;

enum ScopeIconMisc
    {	IconBlackBox	= 162,
	IconLibrary	= IconBlackBox + 1,
	IconProgram	= IconLibrary + 1,
	IconWebProgram	= IconProgram + 1,
	IconProgramEmpty	= IconWebProgram + 1,
	IconWebProgramEmpty	= IconProgramEmpty + 1,
	IconComponents	= IconWebProgramEmpty + 1,
	IconEnvironment	= IconComponents + 1,
	IconWindow	= IconEnvironment + 1,
	IconFolderOpen	= IconWindow + 1,
	IconFolder	= IconFolderOpen + 1,
	IconArrowRight	= IconFolder + 1,
	IconAmbigious	= IconArrowRight + 1,
	IconShadowClass	= IconAmbigious + 1,
	IconShadowMethodPrivate	= IconShadowClass + 1,
	IconShadowMethodProtected	= IconShadowMethodPrivate + 1,
	IconShadowMethod	= IconShadowMethodProtected + 1,
	IconInCompleteSource	= IconShadowMethod + 1
    } ;

EXTERN_C const IID LIBID_BabelServiceLib;

#ifndef __IColorSink_INTERFACE_DEFINED__
#define __IColorSink_INTERFACE_DEFINED__

/* interface IColorSink */
/* [dual][oleautomation][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IColorSink;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4E71BA43-5C36-11d3-B3FD-006008D1BF8C")
    IColorSink : public IDispatch
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Colorize( 
            /* [in] */ long startIdx,
            /* [in] */ long endIdx,
            /* [in] */ ColorClass colorClass,
            /* [in] */ enum CharClass charClass,
            /* [in] */ TriggerClass trigger) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IColorSinkVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IColorSink * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IColorSink * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IColorSink * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IColorSink * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IColorSink * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IColorSink * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IColorSink * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Colorize )( 
            IColorSink * This,
            /* [in] */ long startIdx,
            /* [in] */ long endIdx,
            /* [in] */ ColorClass colorClass,
            /* [in] */ enum CharClass charClass,
            /* [in] */ TriggerClass trigger);
        
        END_INTERFACE
    } IColorSinkVtbl;

    interface IColorSink
    {
        CONST_VTBL struct IColorSinkVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IColorSink_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IColorSink_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IColorSink_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IColorSink_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define IColorSink_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define IColorSink_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define IColorSink_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define IColorSink_Colorize(This,startIdx,endIdx,colorClass,charClass,trigger)	\
    (This)->lpVtbl -> Colorize(This,startIdx,endIdx,colorClass,charClass,trigger)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring] */ HRESULT STDMETHODCALLTYPE IColorSink_Colorize_Proxy( 
    IColorSink * This,
    /* [in] */ long startIdx,
    /* [in] */ long endIdx,
    /* [in] */ ColorClass colorClass,
    /* [in] */ enum CharClass charClass,
    /* [in] */ TriggerClass trigger);


void __RPC_STUB IColorSink_Colorize_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IColorSink_INTERFACE_DEFINED__ */


#ifndef __IParseSink_INTERFACE_DEFINED__
#define __IParseSink_INTERFACE_DEFINED__

/* interface IParseSink */
/* [dual][oleautomation][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IParseSink;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4E71BA40-5C36-11d3-B3FD-006008D1BF8C")
    IParseSink : public IDispatch
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ErrorMessage( 
            /* [in] */ long startLine,
            /* [in] */ long endLine,
            /* [in] */ long startIdx,
            /* [in] */ long endIdx,
            /* [in] */ enum Severity sev,
            /* [in] */ BSTR message) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE MatchPair( 
            /* [in] */ long startLine1,
            /* [in] */ long startIdx1,
            /* [in] */ long endLine1,
            /* [in] */ long endIdx1,
            /* [in] */ long startLine2,
            /* [in] */ long startIdx2,
            /* [in] */ long endLine2,
            /* [in] */ long endIdx2) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE MatchTriple( 
            /* [in] */ long startLine1,
            /* [in] */ long startIdx1,
            /* [in] */ long endLine1,
            /* [in] */ long endIdx1,
            /* [in] */ long startLine2,
            /* [in] */ long startIdx2,
            /* [in] */ long endLine2,
            /* [in] */ long endIdx2,
            /* [in] */ long startLine3,
            /* [in] */ long startIdx3,
            /* [in] */ long endLine3,
            /* [in] */ long endIdx3) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE StartName( 
            /* [in] */ long line,
            /* [in] */ long startIdx,
            /* [in] */ long endIdx) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE QualifyName( 
            /* [in] */ long lineSelector,
            /* [in] */ long startIdxSelector,
            /* [in] */ long endIdxSelector,
            /* [in] */ long line,
            /* [in] */ long startIdx,
            /* [in] */ long endIdx) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AutoExpression( 
            /* [in] */ long startLine,
            /* [in] */ long startIdx,
            /* [in] */ long endLine,
            /* [in] */ long endIdx) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CodeSpan( 
            /* [in] */ long startLine,
            /* [in] */ long startIdx,
            /* [in] */ long endLine,
            /* [in] */ long endIdx) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE StartParameters( 
            /* [in] */ long line,
            /* [in] */ long idx) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Parameter( 
            /* [in] */ long line,
            /* [in] */ long idx) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE EndParameters( 
            /* [in] */ long line,
            /* [in] */ long idx) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetPackage( 
            /* [retval][ref][out] */ IBabelPackage **package) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetProject( 
            /* [retval][ref][out] */ IBabelProject **project) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetFileName( 
            /* [retval][ref][out] */ BSTR *filePath) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddScope( 
            /* [in] */ long startLine,
            /* [in] */ long startIdx,
            /* [in] */ long endLine,
            /* [in] */ long endIdx,
            /* [in] */ enum ScopeKind kind,
            /* [in] */ enum ScopeAccess access,
            /* [in] */ enum ScopeStorage storage,
            /* [in] */ long glyph,
            /* [in] */ BSTR name,
            /* [in] */ BSTR type,
            /* [in] */ BSTR display,
            /* [in] */ BSTR description,
            /* [in] */ VARIANT_BOOL merge) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddInclude( 
            /* [in] */ long startLine,
            /* [in] */ long startIdx,
            /* [in] */ long endLine,
            /* [in] */ long endIdx,
            /* [in] */ enum ScopeAccess access,
            /* [in] */ BSTR name) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddExtern( 
            /* [in] */ long startLine,
            /* [in] */ long startIdx,
            /* [in] */ long endLine,
            /* [in] */ long endIdx,
            /* [unique][in] */ IScope *scope) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IParseSinkVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IParseSink * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IParseSink * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IParseSink * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IParseSink * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IParseSink * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IParseSink * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IParseSink * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ErrorMessage )( 
            IParseSink * This,
            /* [in] */ long startLine,
            /* [in] */ long endLine,
            /* [in] */ long startIdx,
            /* [in] */ long endIdx,
            /* [in] */ enum Severity sev,
            /* [in] */ BSTR message);
        
        HRESULT ( STDMETHODCALLTYPE *MatchPair )( 
            IParseSink * This,
            /* [in] */ long startLine1,
            /* [in] */ long startIdx1,
            /* [in] */ long endLine1,
            /* [in] */ long endIdx1,
            /* [in] */ long startLine2,
            /* [in] */ long startIdx2,
            /* [in] */ long endLine2,
            /* [in] */ long endIdx2);
        
        HRESULT ( STDMETHODCALLTYPE *MatchTriple )( 
            IParseSink * This,
            /* [in] */ long startLine1,
            /* [in] */ long startIdx1,
            /* [in] */ long endLine1,
            /* [in] */ long endIdx1,
            /* [in] */ long startLine2,
            /* [in] */ long startIdx2,
            /* [in] */ long endLine2,
            /* [in] */ long endIdx2,
            /* [in] */ long startLine3,
            /* [in] */ long startIdx3,
            /* [in] */ long endLine3,
            /* [in] */ long endIdx3);
        
        HRESULT ( STDMETHODCALLTYPE *StartName )( 
            IParseSink * This,
            /* [in] */ long line,
            /* [in] */ long startIdx,
            /* [in] */ long endIdx);
        
        HRESULT ( STDMETHODCALLTYPE *QualifyName )( 
            IParseSink * This,
            /* [in] */ long lineSelector,
            /* [in] */ long startIdxSelector,
            /* [in] */ long endIdxSelector,
            /* [in] */ long line,
            /* [in] */ long startIdx,
            /* [in] */ long endIdx);
        
        HRESULT ( STDMETHODCALLTYPE *AutoExpression )( 
            IParseSink * This,
            /* [in] */ long startLine,
            /* [in] */ long startIdx,
            /* [in] */ long endLine,
            /* [in] */ long endIdx);
        
        HRESULT ( STDMETHODCALLTYPE *CodeSpan )( 
            IParseSink * This,
            /* [in] */ long startLine,
            /* [in] */ long startIdx,
            /* [in] */ long endLine,
            /* [in] */ long endIdx);
        
        HRESULT ( STDMETHODCALLTYPE *StartParameters )( 
            IParseSink * This,
            /* [in] */ long line,
            /* [in] */ long idx);
        
        HRESULT ( STDMETHODCALLTYPE *Parameter )( 
            IParseSink * This,
            /* [in] */ long line,
            /* [in] */ long idx);
        
        HRESULT ( STDMETHODCALLTYPE *EndParameters )( 
            IParseSink * This,
            /* [in] */ long line,
            /* [in] */ long idx);
        
        HRESULT ( STDMETHODCALLTYPE *GetPackage )( 
            IParseSink * This,
            /* [retval][ref][out] */ IBabelPackage **package);
        
        HRESULT ( STDMETHODCALLTYPE *GetProject )( 
            IParseSink * This,
            /* [retval][ref][out] */ IBabelProject **project);
        
        HRESULT ( STDMETHODCALLTYPE *GetFileName )( 
            IParseSink * This,
            /* [retval][ref][out] */ BSTR *filePath);
        
        HRESULT ( STDMETHODCALLTYPE *AddScope )( 
            IParseSink * This,
            /* [in] */ long startLine,
            /* [in] */ long startIdx,
            /* [in] */ long endLine,
            /* [in] */ long endIdx,
            /* [in] */ enum ScopeKind kind,
            /* [in] */ enum ScopeAccess access,
            /* [in] */ enum ScopeStorage storage,
            /* [in] */ long glyph,
            /* [in] */ BSTR name,
            /* [in] */ BSTR type,
            /* [in] */ BSTR display,
            /* [in] */ BSTR description,
            /* [in] */ VARIANT_BOOL merge);
        
        HRESULT ( STDMETHODCALLTYPE *AddInclude )( 
            IParseSink * This,
            /* [in] */ long startLine,
            /* [in] */ long startIdx,
            /* [in] */ long endLine,
            /* [in] */ long endIdx,
            /* [in] */ enum ScopeAccess access,
            /* [in] */ BSTR name);
        
        HRESULT ( STDMETHODCALLTYPE *AddExtern )( 
            IParseSink * This,
            /* [in] */ long startLine,
            /* [in] */ long startIdx,
            /* [in] */ long endLine,
            /* [in] */ long endIdx,
            /* [unique][in] */ IScope *scope);
        
        END_INTERFACE
    } IParseSinkVtbl;

    interface IParseSink
    {
        CONST_VTBL struct IParseSinkVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IParseSink_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IParseSink_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IParseSink_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IParseSink_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define IParseSink_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define IParseSink_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define IParseSink_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define IParseSink_ErrorMessage(This,startLine,endLine,startIdx,endIdx,sev,message)	\
    (This)->lpVtbl -> ErrorMessage(This,startLine,endLine,startIdx,endIdx,sev,message)

#define IParseSink_MatchPair(This,startLine1,startIdx1,endLine1,endIdx1,startLine2,startIdx2,endLine2,endIdx2)	\
    (This)->lpVtbl -> MatchPair(This,startLine1,startIdx1,endLine1,endIdx1,startLine2,startIdx2,endLine2,endIdx2)

#define IParseSink_MatchTriple(This,startLine1,startIdx1,endLine1,endIdx1,startLine2,startIdx2,endLine2,endIdx2,startLine3,startIdx3,endLine3,endIdx3)	\
    (This)->lpVtbl -> MatchTriple(This,startLine1,startIdx1,endLine1,endIdx1,startLine2,startIdx2,endLine2,endIdx2,startLine3,startIdx3,endLine3,endIdx3)

#define IParseSink_StartName(This,line,startIdx,endIdx)	\
    (This)->lpVtbl -> StartName(This,line,startIdx,endIdx)

#define IParseSink_QualifyName(This,lineSelector,startIdxSelector,endIdxSelector,line,startIdx,endIdx)	\
    (This)->lpVtbl -> QualifyName(This,lineSelector,startIdxSelector,endIdxSelector,line,startIdx,endIdx)

#define IParseSink_AutoExpression(This,startLine,startIdx,endLine,endIdx)	\
    (This)->lpVtbl -> AutoExpression(This,startLine,startIdx,endLine,endIdx)

#define IParseSink_CodeSpan(This,startLine,startIdx,endLine,endIdx)	\
    (This)->lpVtbl -> CodeSpan(This,startLine,startIdx,endLine,endIdx)

#define IParseSink_StartParameters(This,line,idx)	\
    (This)->lpVtbl -> StartParameters(This,line,idx)

#define IParseSink_Parameter(This,line,idx)	\
    (This)->lpVtbl -> Parameter(This,line,idx)

#define IParseSink_EndParameters(This,line,idx)	\
    (This)->lpVtbl -> EndParameters(This,line,idx)

#define IParseSink_GetPackage(This,package)	\
    (This)->lpVtbl -> GetPackage(This,package)

#define IParseSink_GetProject(This,project)	\
    (This)->lpVtbl -> GetProject(This,project)

#define IParseSink_GetFileName(This,filePath)	\
    (This)->lpVtbl -> GetFileName(This,filePath)

#define IParseSink_AddScope(This,startLine,startIdx,endLine,endIdx,kind,access,storage,glyph,name,type,display,description,merge)	\
    (This)->lpVtbl -> AddScope(This,startLine,startIdx,endLine,endIdx,kind,access,storage,glyph,name,type,display,description,merge)

#define IParseSink_AddInclude(This,startLine,startIdx,endLine,endIdx,access,name)	\
    (This)->lpVtbl -> AddInclude(This,startLine,startIdx,endLine,endIdx,access,name)

#define IParseSink_AddExtern(This,startLine,startIdx,endLine,endIdx,scope)	\
    (This)->lpVtbl -> AddExtern(This,startLine,startIdx,endLine,endIdx,scope)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring] */ HRESULT STDMETHODCALLTYPE IParseSink_ErrorMessage_Proxy( 
    IParseSink * This,
    /* [in] */ long startLine,
    /* [in] */ long endLine,
    /* [in] */ long startIdx,
    /* [in] */ long endIdx,
    /* [in] */ enum Severity sev,
    /* [in] */ BSTR message);


void __RPC_STUB IParseSink_ErrorMessage_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_MatchPair_Proxy( 
    IParseSink * This,
    /* [in] */ long startLine1,
    /* [in] */ long startIdx1,
    /* [in] */ long endLine1,
    /* [in] */ long endIdx1,
    /* [in] */ long startLine2,
    /* [in] */ long startIdx2,
    /* [in] */ long endLine2,
    /* [in] */ long endIdx2);


void __RPC_STUB IParseSink_MatchPair_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_MatchTriple_Proxy( 
    IParseSink * This,
    /* [in] */ long startLine1,
    /* [in] */ long startIdx1,
    /* [in] */ long endLine1,
    /* [in] */ long endIdx1,
    /* [in] */ long startLine2,
    /* [in] */ long startIdx2,
    /* [in] */ long endLine2,
    /* [in] */ long endIdx2,
    /* [in] */ long startLine3,
    /* [in] */ long startIdx3,
    /* [in] */ long endLine3,
    /* [in] */ long endIdx3);


void __RPC_STUB IParseSink_MatchTriple_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_StartName_Proxy( 
    IParseSink * This,
    /* [in] */ long line,
    /* [in] */ long startIdx,
    /* [in] */ long endIdx);


void __RPC_STUB IParseSink_StartName_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_QualifyName_Proxy( 
    IParseSink * This,
    /* [in] */ long lineSelector,
    /* [in] */ long startIdxSelector,
    /* [in] */ long endIdxSelector,
    /* [in] */ long line,
    /* [in] */ long startIdx,
    /* [in] */ long endIdx);


void __RPC_STUB IParseSink_QualifyName_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_AutoExpression_Proxy( 
    IParseSink * This,
    /* [in] */ long startLine,
    /* [in] */ long startIdx,
    /* [in] */ long endLine,
    /* [in] */ long endIdx);


void __RPC_STUB IParseSink_AutoExpression_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_CodeSpan_Proxy( 
    IParseSink * This,
    /* [in] */ long startLine,
    /* [in] */ long startIdx,
    /* [in] */ long endLine,
    /* [in] */ long endIdx);


void __RPC_STUB IParseSink_CodeSpan_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_StartParameters_Proxy( 
    IParseSink * This,
    /* [in] */ long line,
    /* [in] */ long idx);


void __RPC_STUB IParseSink_StartParameters_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_Parameter_Proxy( 
    IParseSink * This,
    /* [in] */ long line,
    /* [in] */ long idx);


void __RPC_STUB IParseSink_Parameter_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_EndParameters_Proxy( 
    IParseSink * This,
    /* [in] */ long line,
    /* [in] */ long idx);


void __RPC_STUB IParseSink_EndParameters_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_GetPackage_Proxy( 
    IParseSink * This,
    /* [retval][ref][out] */ IBabelPackage **package);


void __RPC_STUB IParseSink_GetPackage_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_GetProject_Proxy( 
    IParseSink * This,
    /* [retval][ref][out] */ IBabelProject **project);


void __RPC_STUB IParseSink_GetProject_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_GetFileName_Proxy( 
    IParseSink * This,
    /* [retval][ref][out] */ BSTR *filePath);


void __RPC_STUB IParseSink_GetFileName_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_AddScope_Proxy( 
    IParseSink * This,
    /* [in] */ long startLine,
    /* [in] */ long startIdx,
    /* [in] */ long endLine,
    /* [in] */ long endIdx,
    /* [in] */ enum ScopeKind kind,
    /* [in] */ enum ScopeAccess access,
    /* [in] */ enum ScopeStorage storage,
    /* [in] */ long glyph,
    /* [in] */ BSTR name,
    /* [in] */ BSTR type,
    /* [in] */ BSTR display,
    /* [in] */ BSTR description,
    /* [in] */ VARIANT_BOOL merge);


void __RPC_STUB IParseSink_AddScope_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_AddInclude_Proxy( 
    IParseSink * This,
    /* [in] */ long startLine,
    /* [in] */ long startIdx,
    /* [in] */ long endLine,
    /* [in] */ long endIdx,
    /* [in] */ enum ScopeAccess access,
    /* [in] */ BSTR name);


void __RPC_STUB IParseSink_AddInclude_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IParseSink_AddExtern_Proxy( 
    IParseSink * This,
    /* [in] */ long startLine,
    /* [in] */ long startIdx,
    /* [in] */ long endLine,
    /* [in] */ long endIdx,
    /* [unique][in] */ IScope *scope);


void __RPC_STUB IParseSink_AddExtern_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IParseSink_INTERFACE_DEFINED__ */


#ifndef __IBabelService_INTERFACE_DEFINED__
#define __IBabelService_INTERFACE_DEFINED__

/* interface IBabelService */
/* [dual][oleautomation][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IBabelService;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4E71BA41-5C36-11d3-B3FD-006008D1BF8C")
    IBabelService : public IDispatch
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Init( 
            /* [in] */ LCID language,
            /* [in] */ long reserved) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Done( void) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ColorCount( 
            /* [retval][ref][out] */ ColorClass *count) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetColorInfo( 
            /* [in] */ ColorClass index,
            /* [ref][out] */ BSTR *description,
            /* [ref][out] */ BSTR *style) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ColorLine( 
            /* [in] */ BSTR line,
            /* [unique][in] */ IColorSink *sink,
            /* [ref][out][in] */ long *state) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ParseSource( 
            /* [in] */ BSTR text,
            /* [unique][in] */ IParseSink *sink,
            /* [in] */ enum ParseReason reason,
            /* [in] */ long reserved,
            /* [retval][unique][out] */ IScope **scope) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetMethodFormat( 
            /* [ref][out] */ BSTR *parStart,
            /* [ref][out] */ BSTR *parSep,
            /* [ref][out] */ BSTR *parEnd,
            /* [ref][out] */ BSTR *typeStart,
            /* [ref][out] */ BSTR *typeEnd,
            /* [ref][out] */ VARIANT_BOOL *typePrefixed) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetCommentFormat( 
            /* [ref][out] */ BSTR *lineStart,
            /* [ref][out] */ BSTR *blockStart,
            /* [ref][out] */ BSTR *blockEnd,
            /* [ref][out] */ VARIANT_BOOL *useLineComments) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetImageList( 
            /* [ref][out] */ long *imageListHandle,
            /* [ref][out] */ long *glyphCount) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IBabelServiceVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IBabelService * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IBabelService * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IBabelService * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IBabelService * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IBabelService * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IBabelService * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IBabelService * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Init )( 
            IBabelService * This,
            /* [in] */ LCID language,
            /* [in] */ long reserved);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Done )( 
            IBabelService * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ColorCount )( 
            IBabelService * This,
            /* [retval][ref][out] */ ColorClass *count);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetColorInfo )( 
            IBabelService * This,
            /* [in] */ ColorClass index,
            /* [ref][out] */ BSTR *description,
            /* [ref][out] */ BSTR *style);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ColorLine )( 
            IBabelService * This,
            /* [in] */ BSTR line,
            /* [unique][in] */ IColorSink *sink,
            /* [ref][out][in] */ long *state);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ParseSource )( 
            IBabelService * This,
            /* [in] */ BSTR text,
            /* [unique][in] */ IParseSink *sink,
            /* [in] */ enum ParseReason reason,
            /* [in] */ long reserved,
            /* [retval][unique][out] */ IScope **scope);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetMethodFormat )( 
            IBabelService * This,
            /* [ref][out] */ BSTR *parStart,
            /* [ref][out] */ BSTR *parSep,
            /* [ref][out] */ BSTR *parEnd,
            /* [ref][out] */ BSTR *typeStart,
            /* [ref][out] */ BSTR *typeEnd,
            /* [ref][out] */ VARIANT_BOOL *typePrefixed);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetCommentFormat )( 
            IBabelService * This,
            /* [ref][out] */ BSTR *lineStart,
            /* [ref][out] */ BSTR *blockStart,
            /* [ref][out] */ BSTR *blockEnd,
            /* [ref][out] */ VARIANT_BOOL *useLineComments);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetImageList )( 
            IBabelService * This,
            /* [ref][out] */ long *imageListHandle,
            /* [ref][out] */ long *glyphCount);
        
        END_INTERFACE
    } IBabelServiceVtbl;

    interface IBabelService
    {
        CONST_VTBL struct IBabelServiceVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IBabelService_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IBabelService_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IBabelService_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IBabelService_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define IBabelService_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define IBabelService_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define IBabelService_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define IBabelService_Init(This,language,reserved)	\
    (This)->lpVtbl -> Init(This,language,reserved)

#define IBabelService_Done(This)	\
    (This)->lpVtbl -> Done(This)

#define IBabelService_ColorCount(This,count)	\
    (This)->lpVtbl -> ColorCount(This,count)

#define IBabelService_GetColorInfo(This,index,description,style)	\
    (This)->lpVtbl -> GetColorInfo(This,index,description,style)

#define IBabelService_ColorLine(This,line,sink,state)	\
    (This)->lpVtbl -> ColorLine(This,line,sink,state)

#define IBabelService_ParseSource(This,text,sink,reason,reserved,scope)	\
    (This)->lpVtbl -> ParseSource(This,text,sink,reason,reserved,scope)

#define IBabelService_GetMethodFormat(This,parStart,parSep,parEnd,typeStart,typeEnd,typePrefixed)	\
    (This)->lpVtbl -> GetMethodFormat(This,parStart,parSep,parEnd,typeStart,typeEnd,typePrefixed)

#define IBabelService_GetCommentFormat(This,lineStart,blockStart,blockEnd,useLineComments)	\
    (This)->lpVtbl -> GetCommentFormat(This,lineStart,blockStart,blockEnd,useLineComments)

#define IBabelService_GetImageList(This,imageListHandle,glyphCount)	\
    (This)->lpVtbl -> GetImageList(This,imageListHandle,glyphCount)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring] */ HRESULT STDMETHODCALLTYPE IBabelService_Init_Proxy( 
    IBabelService * This,
    /* [in] */ LCID language,
    /* [in] */ long reserved);


void __RPC_STUB IBabelService_Init_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IBabelService_Done_Proxy( 
    IBabelService * This);


void __RPC_STUB IBabelService_Done_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IBabelService_ColorCount_Proxy( 
    IBabelService * This,
    /* [retval][ref][out] */ ColorClass *count);


void __RPC_STUB IBabelService_ColorCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IBabelService_GetColorInfo_Proxy( 
    IBabelService * This,
    /* [in] */ ColorClass index,
    /* [ref][out] */ BSTR *description,
    /* [ref][out] */ BSTR *style);


void __RPC_STUB IBabelService_GetColorInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IBabelService_ColorLine_Proxy( 
    IBabelService * This,
    /* [in] */ BSTR line,
    /* [unique][in] */ IColorSink *sink,
    /* [ref][out][in] */ long *state);


void __RPC_STUB IBabelService_ColorLine_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IBabelService_ParseSource_Proxy( 
    IBabelService * This,
    /* [in] */ BSTR text,
    /* [unique][in] */ IParseSink *sink,
    /* [in] */ enum ParseReason reason,
    /* [in] */ long reserved,
    /* [retval][unique][out] */ IScope **scope);


void __RPC_STUB IBabelService_ParseSource_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IBabelService_GetMethodFormat_Proxy( 
    IBabelService * This,
    /* [ref][out] */ BSTR *parStart,
    /* [ref][out] */ BSTR *parSep,
    /* [ref][out] */ BSTR *parEnd,
    /* [ref][out] */ BSTR *typeStart,
    /* [ref][out] */ BSTR *typeEnd,
    /* [ref][out] */ VARIANT_BOOL *typePrefixed);


void __RPC_STUB IBabelService_GetMethodFormat_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IBabelService_GetCommentFormat_Proxy( 
    IBabelService * This,
    /* [ref][out] */ BSTR *lineStart,
    /* [ref][out] */ BSTR *blockStart,
    /* [ref][out] */ BSTR *blockEnd,
    /* [ref][out] */ VARIANT_BOOL *useLineComments);


void __RPC_STUB IBabelService_GetCommentFormat_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IBabelService_GetImageList_Proxy( 
    IBabelService * This,
    /* [ref][out] */ long *imageListHandle,
    /* [ref][out] */ long *glyphCount);


void __RPC_STUB IBabelService_GetImageList_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IBabelService_INTERFACE_DEFINED__ */


#ifndef __IBabelPackage_INTERFACE_DEFINED__
#define __IBabelPackage_INTERFACE_DEFINED__

/* interface IBabelPackage */
/* [dual][oleautomation][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IBabelPackage;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("58645F94-4EC7-4aa4-92FB-09625D04FC77")
    IBabelPackage : public IDispatch
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE LoadScope( 
            /* [in] */ long reserved,
            /* [in] */ BSTR fileName,
            /* [unique][in] */ IBabelProject *project,
            /* [retval][ref][out] */ IScope **scope) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IBabelPackageVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IBabelPackage * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IBabelPackage * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IBabelPackage * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IBabelPackage * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IBabelPackage * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IBabelPackage * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IBabelPackage * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *LoadScope )( 
            IBabelPackage * This,
            /* [in] */ long reserved,
            /* [in] */ BSTR fileName,
            /* [unique][in] */ IBabelProject *project,
            /* [retval][ref][out] */ IScope **scope);
        
        END_INTERFACE
    } IBabelPackageVtbl;

    interface IBabelPackage
    {
        CONST_VTBL struct IBabelPackageVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IBabelPackage_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IBabelPackage_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IBabelPackage_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IBabelPackage_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define IBabelPackage_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define IBabelPackage_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define IBabelPackage_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define IBabelPackage_LoadScope(This,reserved,fileName,project,scope)	\
    (This)->lpVtbl -> LoadScope(This,reserved,fileName,project,scope)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring] */ HRESULT STDMETHODCALLTYPE IBabelPackage_LoadScope_Proxy( 
    IBabelPackage * This,
    /* [in] */ long reserved,
    /* [in] */ BSTR fileName,
    /* [unique][in] */ IBabelProject *project,
    /* [retval][ref][out] */ IScope **scope);


void __RPC_STUB IBabelPackage_LoadScope_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IBabelPackage_INTERFACE_DEFINED__ */


#ifndef __IBabelProject_INTERFACE_DEFINED__
#define __IBabelProject_INTERFACE_DEFINED__

/* interface IBabelProject */
/* [dual][oleautomation][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IBabelProject;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("58645F95-4EC7-4aa4-92FB-09625D04FC77")
    IBabelProject : public IDispatch
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE SearchFile( 
            /* [in] */ BSTR fileName,
            /* [retval][ref][out] */ BSTR *filePath) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IBabelProjectVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IBabelProject * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IBabelProject * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IBabelProject * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IBabelProject * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IBabelProject * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IBabelProject * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IBabelProject * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        HRESULT ( STDMETHODCALLTYPE *SearchFile )( 
            IBabelProject * This,
            /* [in] */ BSTR fileName,
            /* [retval][ref][out] */ BSTR *filePath);
        
        END_INTERFACE
    } IBabelProjectVtbl;

    interface IBabelProject
    {
        CONST_VTBL struct IBabelProjectVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IBabelProject_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IBabelProject_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IBabelProject_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IBabelProject_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define IBabelProject_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define IBabelProject_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define IBabelProject_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define IBabelProject_SearchFile(This,fileName,filePath)	\
    (This)->lpVtbl -> SearchFile(This,fileName,filePath)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IBabelProject_SearchFile_Proxy( 
    IBabelProject * This,
    /* [in] */ BSTR fileName,
    /* [retval][ref][out] */ BSTR *filePath);


void __RPC_STUB IBabelProject_SearchFile_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IBabelProject_INTERFACE_DEFINED__ */


#ifndef __INames_INTERFACE_DEFINED__
#define __INames_INTERFACE_DEFINED__

/* interface INames */
/* [dual][oleautomation][helpstring][uuid][object] */ 


EXTERN_C const IID IID_INames;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4E71BA45-5C36-11d3-B3FD-006008D1BF8C")
    INames : public IDispatch
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetCount( 
            /* [retval][ref][out] */ long *count) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetName( 
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *name) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetSelector( 
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *selector) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct INamesVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            INames * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            INames * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            INames * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            INames * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            INames * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            INames * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            INames * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        HRESULT ( STDMETHODCALLTYPE *GetCount )( 
            INames * This,
            /* [retval][ref][out] */ long *count);
        
        HRESULT ( STDMETHODCALLTYPE *GetName )( 
            INames * This,
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *name);
        
        HRESULT ( STDMETHODCALLTYPE *GetSelector )( 
            INames * This,
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *selector);
        
        END_INTERFACE
    } INamesVtbl;

    interface INames
    {
        CONST_VTBL struct INamesVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define INames_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define INames_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define INames_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define INames_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define INames_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define INames_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define INames_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define INames_GetCount(This,count)	\
    (This)->lpVtbl -> GetCount(This,count)

#define INames_GetName(This,index,name)	\
    (This)->lpVtbl -> GetName(This,index,name)

#define INames_GetSelector(This,index,selector)	\
    (This)->lpVtbl -> GetSelector(This,index,selector)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE INames_GetCount_Proxy( 
    INames * This,
    /* [retval][ref][out] */ long *count);


void __RPC_STUB INames_GetCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE INames_GetName_Proxy( 
    INames * This,
    /* [in] */ long index,
    /* [retval][ref][out] */ BSTR *name);


void __RPC_STUB INames_GetName_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE INames_GetSelector_Proxy( 
    INames * This,
    /* [in] */ long index,
    /* [retval][ref][out] */ BSTR *selector);


void __RPC_STUB INames_GetSelector_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __INames_INTERFACE_DEFINED__ */


#ifndef __IMethods_INTERFACE_DEFINED__
#define __IMethods_INTERFACE_DEFINED__

/* interface IMethods */
/* [dual][oleautomation][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IMethods;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4E71BA47-5C36-11d3-B3FD-006008D1BF8C")
    IMethods : public IDispatch
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetName( 
            /* [retval][ref][out] */ BSTR *name) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCount( 
            /* [retval][ref][out] */ long *count) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetDescription( 
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *description) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetType( 
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *type) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetParameterCount( 
            /* [in] */ long index,
            /* [retval][ref][out] */ long *count) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetParameterInfo( 
            /* [in] */ long index,
            /* [in] */ long parameter,
            /* [unique][out] */ BSTR *name,
            /* [unique][out] */ BSTR *display,
            /* [unique][out] */ BSTR *description) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IMethodsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IMethods * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IMethods * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IMethods * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IMethods * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IMethods * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IMethods * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IMethods * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        HRESULT ( STDMETHODCALLTYPE *GetName )( 
            IMethods * This,
            /* [retval][ref][out] */ BSTR *name);
        
        HRESULT ( STDMETHODCALLTYPE *GetCount )( 
            IMethods * This,
            /* [retval][ref][out] */ long *count);
        
        HRESULT ( STDMETHODCALLTYPE *GetDescription )( 
            IMethods * This,
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *description);
        
        HRESULT ( STDMETHODCALLTYPE *GetType )( 
            IMethods * This,
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *type);
        
        HRESULT ( STDMETHODCALLTYPE *GetParameterCount )( 
            IMethods * This,
            /* [in] */ long index,
            /* [retval][ref][out] */ long *count);
        
        HRESULT ( STDMETHODCALLTYPE *GetParameterInfo )( 
            IMethods * This,
            /* [in] */ long index,
            /* [in] */ long parameter,
            /* [unique][out] */ BSTR *name,
            /* [unique][out] */ BSTR *display,
            /* [unique][out] */ BSTR *description);
        
        END_INTERFACE
    } IMethodsVtbl;

    interface IMethods
    {
        CONST_VTBL struct IMethodsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IMethods_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IMethods_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IMethods_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IMethods_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define IMethods_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define IMethods_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define IMethods_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define IMethods_GetName(This,name)	\
    (This)->lpVtbl -> GetName(This,name)

#define IMethods_GetCount(This,count)	\
    (This)->lpVtbl -> GetCount(This,count)

#define IMethods_GetDescription(This,index,description)	\
    (This)->lpVtbl -> GetDescription(This,index,description)

#define IMethods_GetType(This,index,type)	\
    (This)->lpVtbl -> GetType(This,index,type)

#define IMethods_GetParameterCount(This,index,count)	\
    (This)->lpVtbl -> GetParameterCount(This,index,count)

#define IMethods_GetParameterInfo(This,index,parameter,name,display,description)	\
    (This)->lpVtbl -> GetParameterInfo(This,index,parameter,name,display,description)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IMethods_GetName_Proxy( 
    IMethods * This,
    /* [retval][ref][out] */ BSTR *name);


void __RPC_STUB IMethods_GetName_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IMethods_GetCount_Proxy( 
    IMethods * This,
    /* [retval][ref][out] */ long *count);


void __RPC_STUB IMethods_GetCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IMethods_GetDescription_Proxy( 
    IMethods * This,
    /* [in] */ long index,
    /* [retval][ref][out] */ BSTR *description);


void __RPC_STUB IMethods_GetDescription_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IMethods_GetType_Proxy( 
    IMethods * This,
    /* [in] */ long index,
    /* [retval][ref][out] */ BSTR *type);


void __RPC_STUB IMethods_GetType_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IMethods_GetParameterCount_Proxy( 
    IMethods * This,
    /* [in] */ long index,
    /* [retval][ref][out] */ long *count);


void __RPC_STUB IMethods_GetParameterCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IMethods_GetParameterInfo_Proxy( 
    IMethods * This,
    /* [in] */ long index,
    /* [in] */ long parameter,
    /* [unique][out] */ BSTR *name,
    /* [unique][out] */ BSTR *display,
    /* [unique][out] */ BSTR *description);


void __RPC_STUB IMethods_GetParameterInfo_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IMethods_INTERFACE_DEFINED__ */


#ifndef __IDeclarations_INTERFACE_DEFINED__
#define __IDeclarations_INTERFACE_DEFINED__

/* interface IDeclarations */
/* [dual][oleautomation][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IDeclarations;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4E71BA46-5C36-11d3-B3FD-006008D1BF8C")
    IDeclarations : public IDispatch
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetCount( 
            /* [retval][ref][out] */ long *count) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetName( 
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *name) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetDescription( 
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *description) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetGlyph( 
            /* [in] */ long index,
            /* [retval][ref][out] */ long *glyph) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetBestMatch( 
            /* [in] */ BSTR text,
            /* [ref][out] */ long *index,
            /* [ref][out] */ VARIANT_BOOL *uniqueMatch) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDeclarationsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDeclarations * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDeclarations * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDeclarations * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IDeclarations * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IDeclarations * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IDeclarations * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IDeclarations * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        HRESULT ( STDMETHODCALLTYPE *GetCount )( 
            IDeclarations * This,
            /* [retval][ref][out] */ long *count);
        
        HRESULT ( STDMETHODCALLTYPE *GetName )( 
            IDeclarations * This,
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *name);
        
        HRESULT ( STDMETHODCALLTYPE *GetDescription )( 
            IDeclarations * This,
            /* [in] */ long index,
            /* [retval][ref][out] */ BSTR *description);
        
        HRESULT ( STDMETHODCALLTYPE *GetGlyph )( 
            IDeclarations * This,
            /* [in] */ long index,
            /* [retval][ref][out] */ long *glyph);
        
        HRESULT ( STDMETHODCALLTYPE *GetBestMatch )( 
            IDeclarations * This,
            /* [in] */ BSTR text,
            /* [ref][out] */ long *index,
            /* [ref][out] */ VARIANT_BOOL *uniqueMatch);
        
        END_INTERFACE
    } IDeclarationsVtbl;

    interface IDeclarations
    {
        CONST_VTBL struct IDeclarationsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDeclarations_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDeclarations_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDeclarations_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDeclarations_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define IDeclarations_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define IDeclarations_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define IDeclarations_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define IDeclarations_GetCount(This,count)	\
    (This)->lpVtbl -> GetCount(This,count)

#define IDeclarations_GetName(This,index,name)	\
    (This)->lpVtbl -> GetName(This,index,name)

#define IDeclarations_GetDescription(This,index,description)	\
    (This)->lpVtbl -> GetDescription(This,index,description)

#define IDeclarations_GetGlyph(This,index,glyph)	\
    (This)->lpVtbl -> GetGlyph(This,index,glyph)

#define IDeclarations_GetBestMatch(This,text,index,uniqueMatch)	\
    (This)->lpVtbl -> GetBestMatch(This,text,index,uniqueMatch)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDeclarations_GetCount_Proxy( 
    IDeclarations * This,
    /* [retval][ref][out] */ long *count);


void __RPC_STUB IDeclarations_GetCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeclarations_GetName_Proxy( 
    IDeclarations * This,
    /* [in] */ long index,
    /* [retval][ref][out] */ BSTR *name);


void __RPC_STUB IDeclarations_GetName_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeclarations_GetDescription_Proxy( 
    IDeclarations * This,
    /* [in] */ long index,
    /* [retval][ref][out] */ BSTR *description);


void __RPC_STUB IDeclarations_GetDescription_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeclarations_GetGlyph_Proxy( 
    IDeclarations * This,
    /* [in] */ long index,
    /* [retval][ref][out] */ long *glyph);


void __RPC_STUB IDeclarations_GetGlyph_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeclarations_GetBestMatch_Proxy( 
    IDeclarations * This,
    /* [in] */ BSTR text,
    /* [ref][out] */ long *index,
    /* [ref][out] */ VARIANT_BOOL *uniqueMatch);


void __RPC_STUB IDeclarations_GetBestMatch_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDeclarations_INTERFACE_DEFINED__ */


#ifndef __IScope_INTERFACE_DEFINED__
#define __IScope_INTERFACE_DEFINED__

/* interface IScope */
/* [dual][oleautomation][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IScope;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4E71BA44-5C36-11d3-B3FD-006008D1BF8C")
    IScope : public IDispatch
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetDeclarations( 
            /* [in] */ long line,
            /* [in] */ long idx,
            /* [unique][in] */ INames *names,
            /* [retval][ref][out] */ IDeclarations **decls) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetDataTipText( 
            /* [in] */ long line,
            /* [in] */ long idx,
            /* [unique][in] */ INames *names,
            /* [retval][ref][out] */ BSTR *text) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetMethods( 
            /* [in] */ long line,
            /* [in] */ long idx,
            /* [unique][in] */ INames *names,
            /* [retval][ref][out] */ IMethods **imethods) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Narrow( 
            /* [in] */ long line,
            /* [in] */ long idx,
            /* [ref][out] */ BSTR *name,
            /* [ref][out] */ long *startLine) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IScopeVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IScope * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IScope * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IScope * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IScope * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IScope * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IScope * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IScope * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        HRESULT ( STDMETHODCALLTYPE *GetDeclarations )( 
            IScope * This,
            /* [in] */ long line,
            /* [in] */ long idx,
            /* [unique][in] */ INames *names,
            /* [retval][ref][out] */ IDeclarations **decls);
        
        HRESULT ( STDMETHODCALLTYPE *GetDataTipText )( 
            IScope * This,
            /* [in] */ long line,
            /* [in] */ long idx,
            /* [unique][in] */ INames *names,
            /* [retval][ref][out] */ BSTR *text);
        
        HRESULT ( STDMETHODCALLTYPE *GetMethods )( 
            IScope * This,
            /* [in] */ long line,
            /* [in] */ long idx,
            /* [unique][in] */ INames *names,
            /* [retval][ref][out] */ IMethods **imethods);
        
        HRESULT ( STDMETHODCALLTYPE *Narrow )( 
            IScope * This,
            /* [in] */ long line,
            /* [in] */ long idx,
            /* [ref][out] */ BSTR *name,
            /* [ref][out] */ long *startLine);
        
        END_INTERFACE
    } IScopeVtbl;

    interface IScope
    {
        CONST_VTBL struct IScopeVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IScope_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IScope_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IScope_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IScope_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define IScope_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define IScope_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define IScope_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define IScope_GetDeclarations(This,line,idx,names,decls)	\
    (This)->lpVtbl -> GetDeclarations(This,line,idx,names,decls)

#define IScope_GetDataTipText(This,line,idx,names,text)	\
    (This)->lpVtbl -> GetDataTipText(This,line,idx,names,text)

#define IScope_GetMethods(This,line,idx,names,imethods)	\
    (This)->lpVtbl -> GetMethods(This,line,idx,names,imethods)

#define IScope_Narrow(This,line,idx,name,startLine)	\
    (This)->lpVtbl -> Narrow(This,line,idx,name,startLine)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IScope_GetDeclarations_Proxy( 
    IScope * This,
    /* [in] */ long line,
    /* [in] */ long idx,
    /* [unique][in] */ INames *names,
    /* [retval][ref][out] */ IDeclarations **decls);


void __RPC_STUB IScope_GetDeclarations_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IScope_GetDataTipText_Proxy( 
    IScope * This,
    /* [in] */ long line,
    /* [in] */ long idx,
    /* [unique][in] */ INames *names,
    /* [retval][ref][out] */ BSTR *text);


void __RPC_STUB IScope_GetDataTipText_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IScope_GetMethods_Proxy( 
    IScope * This,
    /* [in] */ long line,
    /* [in] */ long idx,
    /* [unique][in] */ INames *names,
    /* [retval][ref][out] */ IMethods **imethods);


void __RPC_STUB IScope_GetMethods_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IScope_Narrow_Proxy( 
    IScope * This,
    /* [in] */ long line,
    /* [in] */ long idx,
    /* [ref][out] */ BSTR *name,
    /* [ref][out] */ long *startLine);


void __RPC_STUB IScope_Narrow_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IScope_INTERFACE_DEFINED__ */

#endif /* __BabelServiceLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


