
#pragma warning( disable: 4049 )  /* more than 64k source lines */

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 6.00.0347 */
/* at Tue Sep 25 01:37:10 2001
 */
/* Compiler settings for ee.idl:
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

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __ee_h__
#define __ee_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IDebugExpressionEvaluator_FWD_DEFINED__
#define __IDebugExpressionEvaluator_FWD_DEFINED__
typedef interface IDebugExpressionEvaluator IDebugExpressionEvaluator;
#endif 	/* __IDebugExpressionEvaluator_FWD_DEFINED__ */


#ifndef __IDebugObject_FWD_DEFINED__
#define __IDebugObject_FWD_DEFINED__
typedef interface IDebugObject IDebugObject;
#endif 	/* __IDebugObject_FWD_DEFINED__ */


#ifndef __IDebugArrayObject_FWD_DEFINED__
#define __IDebugArrayObject_FWD_DEFINED__
typedef interface IDebugArrayObject IDebugArrayObject;
#endif 	/* __IDebugArrayObject_FWD_DEFINED__ */


#ifndef __IDebugFunctionObject_FWD_DEFINED__
#define __IDebugFunctionObject_FWD_DEFINED__
typedef interface IDebugFunctionObject IDebugFunctionObject;
#endif 	/* __IDebugFunctionObject_FWD_DEFINED__ */


#ifndef __IDebugManagedObject_FWD_DEFINED__
#define __IDebugManagedObject_FWD_DEFINED__
typedef interface IDebugManagedObject IDebugManagedObject;
#endif 	/* __IDebugManagedObject_FWD_DEFINED__ */


#ifndef __IDebugBinder_FWD_DEFINED__
#define __IDebugBinder_FWD_DEFINED__
typedef interface IDebugBinder IDebugBinder;
#endif 	/* __IDebugBinder_FWD_DEFINED__ */


#ifndef __IDebugPointerObject_FWD_DEFINED__
#define __IDebugPointerObject_FWD_DEFINED__
typedef interface IDebugPointerObject IDebugPointerObject;
#endif 	/* __IDebugPointerObject_FWD_DEFINED__ */


#ifndef __IDebugParsedExpression_FWD_DEFINED__
#define __IDebugParsedExpression_FWD_DEFINED__
typedef interface IDebugParsedExpression IDebugParsedExpression;
#endif 	/* __IDebugParsedExpression_FWD_DEFINED__ */


#ifndef __IEnumDebugObjects_FWD_DEFINED__
#define __IEnumDebugObjects_FWD_DEFINED__
typedef interface IEnumDebugObjects IEnumDebugObjects;
#endif 	/* __IEnumDebugObjects_FWD_DEFINED__ */


/* header files for imported files */
#include "ocidl.h"
#include "msdbg.h"

#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 

/* interface __MIDL_itf_ee_0000 */
/* [local] */ 










#define S_EVAL_EXCEPTION                 MAKE_HRESULT(0, FACILITY_ITF, 0x0002)
#define S_EVAL_ABORTED                   MAKE_HRESULT(0, FACILITY_ITF, 0x0003)
#define S_EVAL_TIMEDOUT                  MAKE_HRESULT(0, FACILITY_ITF, 0x0004)
#define S_EVAL_NO_RESULT                 MAKE_HRESULT(0, FACILITY_ITF, 0x0005)
#define S_EVAL_THREAD_SUSPENDED          MAKE_HRESULT(0, FACILITY_ITF, 0x0006)
#define S_EVAL_THREAD_SLEEP_WAIT_JOIN    MAKE_HRESULT(0, FACILITY_ITF, 0x0007)
#define S_EVAL_BAD_THREAD_STATE          MAKE_HRESULT(0, FACILITY_ITF, 0x0008)
#define S_EVAL_THREAD_NOT_STARTED        MAKE_HRESULT(0, FACILITY_ITF, 0x0009)
#define S_EVAL_BAD_START_POINT           MAKE_HRESULT(0, FACILITY_ITF, 0x000A)
#define E_STATIC_VAR_NOT_AVAILABLE       MAKE_HRESULT(1, FACILITY_ITF, 0x000B)
#define S_EVAL_WEB_METHOD                MAKE_HRESULT(1, FACILITY_ITF, 0x000C)


extern RPC_IF_HANDLE __MIDL_itf_ee_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_ee_0000_v0_0_s_ifspec;

#ifndef __IDebugExpressionEvaluator_INTERFACE_DEFINED__
#define __IDebugExpressionEvaluator_INTERFACE_DEFINED__

/* interface IDebugExpressionEvaluator */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_IDebugExpressionEvaluator;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("C077C822-476C-11d2-B73C-0000F87572EF")
    IDebugExpressionEvaluator : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Parse( 
            /* [in] */ LPCOLESTR upstrExpression,
            /* [in] */ PARSEFLAGS dwFlags,
            /* [in] */ UINT nRadix,
            /* [out] */ BSTR *pbstrError,
            /* [out] */ UINT *pichError,
            /* [out] */ IDebugParsedExpression **ppParsedExpression) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetMethodProperty( 
            /* [in] */ IDebugSymbolProvider *pSymbolProvider,
            /* [in] */ IDebugAddress *pAddress,
            /* [in] */ IDebugBinder *pBinder,
            /* [in] */ BOOL fIncludeHiddenLocals,
            /* [out] */ IDebugProperty2 **ppProperty) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetMethodLocationProperty( 
            /* [in] */ LPCOLESTR upstrFullyQualifiedMethodPlusOffset,
            /* [in] */ IDebugSymbolProvider *pSymbolProvider,
            /* [in] */ IDebugAddress *pAddress,
            /* [in] */ IDebugBinder *pBinder,
            /* [out] */ IDebugProperty2 **ppProperty) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetLocale( 
            /* [in] */ WORD wLangID) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetRegistryRoot( 
            /* [in] */ LPCOLESTR ustrRegistryRoot) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDebugExpressionEvaluatorVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDebugExpressionEvaluator * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDebugExpressionEvaluator * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDebugExpressionEvaluator * This);
        
        HRESULT ( STDMETHODCALLTYPE *Parse )( 
            IDebugExpressionEvaluator * This,
            /* [in] */ LPCOLESTR upstrExpression,
            /* [in] */ PARSEFLAGS dwFlags,
            /* [in] */ UINT nRadix,
            /* [out] */ BSTR *pbstrError,
            /* [out] */ UINT *pichError,
            /* [out] */ IDebugParsedExpression **ppParsedExpression);
        
        HRESULT ( STDMETHODCALLTYPE *GetMethodProperty )( 
            IDebugExpressionEvaluator * This,
            /* [in] */ IDebugSymbolProvider *pSymbolProvider,
            /* [in] */ IDebugAddress *pAddress,
            /* [in] */ IDebugBinder *pBinder,
            /* [in] */ BOOL fIncludeHiddenLocals,
            /* [out] */ IDebugProperty2 **ppProperty);
        
        HRESULT ( STDMETHODCALLTYPE *GetMethodLocationProperty )( 
            IDebugExpressionEvaluator * This,
            /* [in] */ LPCOLESTR upstrFullyQualifiedMethodPlusOffset,
            /* [in] */ IDebugSymbolProvider *pSymbolProvider,
            /* [in] */ IDebugAddress *pAddress,
            /* [in] */ IDebugBinder *pBinder,
            /* [out] */ IDebugProperty2 **ppProperty);
        
        HRESULT ( STDMETHODCALLTYPE *SetLocale )( 
            IDebugExpressionEvaluator * This,
            /* [in] */ WORD wLangID);
        
        HRESULT ( STDMETHODCALLTYPE *SetRegistryRoot )( 
            IDebugExpressionEvaluator * This,
            /* [in] */ LPCOLESTR ustrRegistryRoot);
        
        END_INTERFACE
    } IDebugExpressionEvaluatorVtbl;

    interface IDebugExpressionEvaluator
    {
        CONST_VTBL struct IDebugExpressionEvaluatorVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDebugExpressionEvaluator_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDebugExpressionEvaluator_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDebugExpressionEvaluator_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDebugExpressionEvaluator_Parse(This,upstrExpression,dwFlags,nRadix,pbstrError,pichError,ppParsedExpression)	\
    (This)->lpVtbl -> Parse(This,upstrExpression,dwFlags,nRadix,pbstrError,pichError,ppParsedExpression)

#define IDebugExpressionEvaluator_GetMethodProperty(This,pSymbolProvider,pAddress,pBinder,fIncludeHiddenLocals,ppProperty)	\
    (This)->lpVtbl -> GetMethodProperty(This,pSymbolProvider,pAddress,pBinder,fIncludeHiddenLocals,ppProperty)

#define IDebugExpressionEvaluator_GetMethodLocationProperty(This,upstrFullyQualifiedMethodPlusOffset,pSymbolProvider,pAddress,pBinder,ppProperty)	\
    (This)->lpVtbl -> GetMethodLocationProperty(This,upstrFullyQualifiedMethodPlusOffset,pSymbolProvider,pAddress,pBinder,ppProperty)

#define IDebugExpressionEvaluator_SetLocale(This,wLangID)	\
    (This)->lpVtbl -> SetLocale(This,wLangID)

#define IDebugExpressionEvaluator_SetRegistryRoot(This,ustrRegistryRoot)	\
    (This)->lpVtbl -> SetRegistryRoot(This,ustrRegistryRoot)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDebugExpressionEvaluator_Parse_Proxy( 
    IDebugExpressionEvaluator * This,
    /* [in] */ LPCOLESTR upstrExpression,
    /* [in] */ PARSEFLAGS dwFlags,
    /* [in] */ UINT nRadix,
    /* [out] */ BSTR *pbstrError,
    /* [out] */ UINT *pichError,
    /* [out] */ IDebugParsedExpression **ppParsedExpression);


void __RPC_STUB IDebugExpressionEvaluator_Parse_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugExpressionEvaluator_GetMethodProperty_Proxy( 
    IDebugExpressionEvaluator * This,
    /* [in] */ IDebugSymbolProvider *pSymbolProvider,
    /* [in] */ IDebugAddress *pAddress,
    /* [in] */ IDebugBinder *pBinder,
    /* [in] */ BOOL fIncludeHiddenLocals,
    /* [out] */ IDebugProperty2 **ppProperty);


void __RPC_STUB IDebugExpressionEvaluator_GetMethodProperty_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugExpressionEvaluator_GetMethodLocationProperty_Proxy( 
    IDebugExpressionEvaluator * This,
    /* [in] */ LPCOLESTR upstrFullyQualifiedMethodPlusOffset,
    /* [in] */ IDebugSymbolProvider *pSymbolProvider,
    /* [in] */ IDebugAddress *pAddress,
    /* [in] */ IDebugBinder *pBinder,
    /* [out] */ IDebugProperty2 **ppProperty);


void __RPC_STUB IDebugExpressionEvaluator_GetMethodLocationProperty_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugExpressionEvaluator_SetLocale_Proxy( 
    IDebugExpressionEvaluator * This,
    /* [in] */ WORD wLangID);


void __RPC_STUB IDebugExpressionEvaluator_SetLocale_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugExpressionEvaluator_SetRegistryRoot_Proxy( 
    IDebugExpressionEvaluator * This,
    /* [in] */ LPCOLESTR ustrRegistryRoot);


void __RPC_STUB IDebugExpressionEvaluator_SetRegistryRoot_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDebugExpressionEvaluator_INTERFACE_DEFINED__ */


#ifndef __IDebugObject_INTERFACE_DEFINED__
#define __IDebugObject_INTERFACE_DEFINED__

/* interface IDebugObject */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_IDebugObject;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("C077C823-476C-11d2-B73C-0000F87572EF")
    IDebugObject : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetSize( 
            /* [out] */ UINT *pnSize) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetValue( 
            /* [length_is][size_is][out] */ BYTE *pValue,
            /* [in] */ UINT nSize) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetValue( 
            /* [size_is][in] */ BYTE *pValue,
            /* [in] */ UINT nSize) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetReferenceValue( 
            /* [in] */ IDebugObject *pObject) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetMemoryContext( 
            IDebugMemoryContext2 **pContext) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetManagedDebugObject( 
            /* [out] */ IDebugManagedObject **ppObject) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE IsNullReference( 
            /* [out] */ BOOL *pfIsNull) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE IsEqual( 
            /* [in] */ IDebugObject *pObject,
            /* [out] */ BOOL *pfIsEqual) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE IsReadOnly( 
            /* [out] */ BOOL *pfIsReadOnly) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE IsProxy( 
            /* [out] */ BOOL *pfIsProxy) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDebugObjectVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDebugObject * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDebugObject * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDebugObject * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetSize )( 
            IDebugObject * This,
            /* [out] */ UINT *pnSize);
        
        HRESULT ( STDMETHODCALLTYPE *GetValue )( 
            IDebugObject * This,
            /* [length_is][size_is][out] */ BYTE *pValue,
            /* [in] */ UINT nSize);
        
        HRESULT ( STDMETHODCALLTYPE *SetValue )( 
            IDebugObject * This,
            /* [size_is][in] */ BYTE *pValue,
            /* [in] */ UINT nSize);
        
        HRESULT ( STDMETHODCALLTYPE *SetReferenceValue )( 
            IDebugObject * This,
            /* [in] */ IDebugObject *pObject);
        
        HRESULT ( STDMETHODCALLTYPE *GetMemoryContext )( 
            IDebugObject * This,
            IDebugMemoryContext2 **pContext);
        
        HRESULT ( STDMETHODCALLTYPE *GetManagedDebugObject )( 
            IDebugObject * This,
            /* [out] */ IDebugManagedObject **ppObject);
        
        HRESULT ( STDMETHODCALLTYPE *IsNullReference )( 
            IDebugObject * This,
            /* [out] */ BOOL *pfIsNull);
        
        HRESULT ( STDMETHODCALLTYPE *IsEqual )( 
            IDebugObject * This,
            /* [in] */ IDebugObject *pObject,
            /* [out] */ BOOL *pfIsEqual);
        
        HRESULT ( STDMETHODCALLTYPE *IsReadOnly )( 
            IDebugObject * This,
            /* [out] */ BOOL *pfIsReadOnly);
        
        HRESULT ( STDMETHODCALLTYPE *IsProxy )( 
            IDebugObject * This,
            /* [out] */ BOOL *pfIsProxy);
        
        END_INTERFACE
    } IDebugObjectVtbl;

    interface IDebugObject
    {
        CONST_VTBL struct IDebugObjectVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDebugObject_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDebugObject_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDebugObject_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDebugObject_GetSize(This,pnSize)	\
    (This)->lpVtbl -> GetSize(This,pnSize)

#define IDebugObject_GetValue(This,pValue,nSize)	\
    (This)->lpVtbl -> GetValue(This,pValue,nSize)

#define IDebugObject_SetValue(This,pValue,nSize)	\
    (This)->lpVtbl -> SetValue(This,pValue,nSize)

#define IDebugObject_SetReferenceValue(This,pObject)	\
    (This)->lpVtbl -> SetReferenceValue(This,pObject)

#define IDebugObject_GetMemoryContext(This,pContext)	\
    (This)->lpVtbl -> GetMemoryContext(This,pContext)

#define IDebugObject_GetManagedDebugObject(This,ppObject)	\
    (This)->lpVtbl -> GetManagedDebugObject(This,ppObject)

#define IDebugObject_IsNullReference(This,pfIsNull)	\
    (This)->lpVtbl -> IsNullReference(This,pfIsNull)

#define IDebugObject_IsEqual(This,pObject,pfIsEqual)	\
    (This)->lpVtbl -> IsEqual(This,pObject,pfIsEqual)

#define IDebugObject_IsReadOnly(This,pfIsReadOnly)	\
    (This)->lpVtbl -> IsReadOnly(This,pfIsReadOnly)

#define IDebugObject_IsProxy(This,pfIsProxy)	\
    (This)->lpVtbl -> IsProxy(This,pfIsProxy)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDebugObject_GetSize_Proxy( 
    IDebugObject * This,
    /* [out] */ UINT *pnSize);


void __RPC_STUB IDebugObject_GetSize_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugObject_GetValue_Proxy( 
    IDebugObject * This,
    /* [length_is][size_is][out] */ BYTE *pValue,
    /* [in] */ UINT nSize);


void __RPC_STUB IDebugObject_GetValue_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugObject_SetValue_Proxy( 
    IDebugObject * This,
    /* [size_is][in] */ BYTE *pValue,
    /* [in] */ UINT nSize);


void __RPC_STUB IDebugObject_SetValue_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugObject_SetReferenceValue_Proxy( 
    IDebugObject * This,
    /* [in] */ IDebugObject *pObject);


void __RPC_STUB IDebugObject_SetReferenceValue_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugObject_GetMemoryContext_Proxy( 
    IDebugObject * This,
    IDebugMemoryContext2 **pContext);


void __RPC_STUB IDebugObject_GetMemoryContext_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugObject_GetManagedDebugObject_Proxy( 
    IDebugObject * This,
    /* [out] */ IDebugManagedObject **ppObject);


void __RPC_STUB IDebugObject_GetManagedDebugObject_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugObject_IsNullReference_Proxy( 
    IDebugObject * This,
    /* [out] */ BOOL *pfIsNull);


void __RPC_STUB IDebugObject_IsNullReference_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugObject_IsEqual_Proxy( 
    IDebugObject * This,
    /* [in] */ IDebugObject *pObject,
    /* [out] */ BOOL *pfIsEqual);


void __RPC_STUB IDebugObject_IsEqual_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugObject_IsReadOnly_Proxy( 
    IDebugObject * This,
    /* [out] */ BOOL *pfIsReadOnly);


void __RPC_STUB IDebugObject_IsReadOnly_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugObject_IsProxy_Proxy( 
    IDebugObject * This,
    /* [out] */ BOOL *pfIsProxy);


void __RPC_STUB IDebugObject_IsProxy_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDebugObject_INTERFACE_DEFINED__ */


#ifndef __IDebugArrayObject_INTERFACE_DEFINED__
#define __IDebugArrayObject_INTERFACE_DEFINED__

/* interface IDebugArrayObject */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_IDebugArrayObject;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("29ECD774-75AE-11d2-B74E-0000F87572EF")
    IDebugArrayObject : public IDebugObject
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetCount( 
            /* [out] */ DWORD *pdwElements) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetElement( 
            /* [in] */ DWORD dwIndex,
            /* [out] */ IDebugObject **ppElement) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetElements( 
            /* [out] */ IEnumDebugObjects **ppEnum) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetRank( 
            /* [out] */ DWORD *pdwRank) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetDimensions( 
            /* [in] */ DWORD dwCount,
            /* [length_is][size_is][out] */ DWORD dwDimensions[  ]) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDebugArrayObjectVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDebugArrayObject * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDebugArrayObject * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDebugArrayObject * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetSize )( 
            IDebugArrayObject * This,
            /* [out] */ UINT *pnSize);
        
        HRESULT ( STDMETHODCALLTYPE *GetValue )( 
            IDebugArrayObject * This,
            /* [length_is][size_is][out] */ BYTE *pValue,
            /* [in] */ UINT nSize);
        
        HRESULT ( STDMETHODCALLTYPE *SetValue )( 
            IDebugArrayObject * This,
            /* [size_is][in] */ BYTE *pValue,
            /* [in] */ UINT nSize);
        
        HRESULT ( STDMETHODCALLTYPE *SetReferenceValue )( 
            IDebugArrayObject * This,
            /* [in] */ IDebugObject *pObject);
        
        HRESULT ( STDMETHODCALLTYPE *GetMemoryContext )( 
            IDebugArrayObject * This,
            IDebugMemoryContext2 **pContext);
        
        HRESULT ( STDMETHODCALLTYPE *GetManagedDebugObject )( 
            IDebugArrayObject * This,
            /* [out] */ IDebugManagedObject **ppObject);
        
        HRESULT ( STDMETHODCALLTYPE *IsNullReference )( 
            IDebugArrayObject * This,
            /* [out] */ BOOL *pfIsNull);
        
        HRESULT ( STDMETHODCALLTYPE *IsEqual )( 
            IDebugArrayObject * This,
            /* [in] */ IDebugObject *pObject,
            /* [out] */ BOOL *pfIsEqual);
        
        HRESULT ( STDMETHODCALLTYPE *IsReadOnly )( 
            IDebugArrayObject * This,
            /* [out] */ BOOL *pfIsReadOnly);
        
        HRESULT ( STDMETHODCALLTYPE *IsProxy )( 
            IDebugArrayObject * This,
            /* [out] */ BOOL *pfIsProxy);
        
        HRESULT ( STDMETHODCALLTYPE *GetCount )( 
            IDebugArrayObject * This,
            /* [out] */ DWORD *pdwElements);
        
        HRESULT ( STDMETHODCALLTYPE *GetElement )( 
            IDebugArrayObject * This,
            /* [in] */ DWORD dwIndex,
            /* [out] */ IDebugObject **ppElement);
        
        HRESULT ( STDMETHODCALLTYPE *GetElements )( 
            IDebugArrayObject * This,
            /* [out] */ IEnumDebugObjects **ppEnum);
        
        HRESULT ( STDMETHODCALLTYPE *GetRank )( 
            IDebugArrayObject * This,
            /* [out] */ DWORD *pdwRank);
        
        HRESULT ( STDMETHODCALLTYPE *GetDimensions )( 
            IDebugArrayObject * This,
            /* [in] */ DWORD dwCount,
            /* [length_is][size_is][out] */ DWORD dwDimensions[  ]);
        
        END_INTERFACE
    } IDebugArrayObjectVtbl;

    interface IDebugArrayObject
    {
        CONST_VTBL struct IDebugArrayObjectVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDebugArrayObject_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDebugArrayObject_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDebugArrayObject_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDebugArrayObject_GetSize(This,pnSize)	\
    (This)->lpVtbl -> GetSize(This,pnSize)

#define IDebugArrayObject_GetValue(This,pValue,nSize)	\
    (This)->lpVtbl -> GetValue(This,pValue,nSize)

#define IDebugArrayObject_SetValue(This,pValue,nSize)	\
    (This)->lpVtbl -> SetValue(This,pValue,nSize)

#define IDebugArrayObject_SetReferenceValue(This,pObject)	\
    (This)->lpVtbl -> SetReferenceValue(This,pObject)

#define IDebugArrayObject_GetMemoryContext(This,pContext)	\
    (This)->lpVtbl -> GetMemoryContext(This,pContext)

#define IDebugArrayObject_GetManagedDebugObject(This,ppObject)	\
    (This)->lpVtbl -> GetManagedDebugObject(This,ppObject)

#define IDebugArrayObject_IsNullReference(This,pfIsNull)	\
    (This)->lpVtbl -> IsNullReference(This,pfIsNull)

#define IDebugArrayObject_IsEqual(This,pObject,pfIsEqual)	\
    (This)->lpVtbl -> IsEqual(This,pObject,pfIsEqual)

#define IDebugArrayObject_IsReadOnly(This,pfIsReadOnly)	\
    (This)->lpVtbl -> IsReadOnly(This,pfIsReadOnly)

#define IDebugArrayObject_IsProxy(This,pfIsProxy)	\
    (This)->lpVtbl -> IsProxy(This,pfIsProxy)


#define IDebugArrayObject_GetCount(This,pdwElements)	\
    (This)->lpVtbl -> GetCount(This,pdwElements)

#define IDebugArrayObject_GetElement(This,dwIndex,ppElement)	\
    (This)->lpVtbl -> GetElement(This,dwIndex,ppElement)

#define IDebugArrayObject_GetElements(This,ppEnum)	\
    (This)->lpVtbl -> GetElements(This,ppEnum)

#define IDebugArrayObject_GetRank(This,pdwRank)	\
    (This)->lpVtbl -> GetRank(This,pdwRank)

#define IDebugArrayObject_GetDimensions(This,dwCount,dwDimensions)	\
    (This)->lpVtbl -> GetDimensions(This,dwCount,dwDimensions)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDebugArrayObject_GetCount_Proxy( 
    IDebugArrayObject * This,
    /* [out] */ DWORD *pdwElements);


void __RPC_STUB IDebugArrayObject_GetCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugArrayObject_GetElement_Proxy( 
    IDebugArrayObject * This,
    /* [in] */ DWORD dwIndex,
    /* [out] */ IDebugObject **ppElement);


void __RPC_STUB IDebugArrayObject_GetElement_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugArrayObject_GetElements_Proxy( 
    IDebugArrayObject * This,
    /* [out] */ IEnumDebugObjects **ppEnum);


void __RPC_STUB IDebugArrayObject_GetElements_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugArrayObject_GetRank_Proxy( 
    IDebugArrayObject * This,
    /* [out] */ DWORD *pdwRank);


void __RPC_STUB IDebugArrayObject_GetRank_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugArrayObject_GetDimensions_Proxy( 
    IDebugArrayObject * This,
    /* [in] */ DWORD dwCount,
    /* [length_is][size_is][out] */ DWORD dwDimensions[  ]);


void __RPC_STUB IDebugArrayObject_GetDimensions_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDebugArrayObject_INTERFACE_DEFINED__ */


#ifndef __IDebugFunctionObject_INTERFACE_DEFINED__
#define __IDebugFunctionObject_INTERFACE_DEFINED__

/* interface IDebugFunctionObject */
/* [unique][uuid][object] */ 


enum __MIDL_IDebugFunctionObject_0001
    {	OBJECT_TYPE_BOOLEAN	= 0,
	OBJECT_TYPE_CHAR	= 0x1,
	OBJECT_TYPE_I1	= 0x2,
	OBJECT_TYPE_U1	= 0x3,
	OBJECT_TYPE_I2	= 0x4,
	OBJECT_TYPE_U2	= 0x5,
	OBJECT_TYPE_I4	= 0x6,
	OBJECT_TYPE_U4	= 0x7,
	OBJECT_TYPE_I8	= 0x8,
	OBJECT_TYPE_U8	= 0x9,
	OBJECT_TYPE_R4	= 0xa,
	OBJECT_TYPE_R8	= 0xb,
	OBJECT_TYPE_OBJECT	= 0xc,
	OBJECT_TYPE_NULL	= 0xd,
	OBJECT_TYPE_CLASS	= 0xe
    } ;
typedef DWORD OBJECT_TYPE;


EXTERN_C const IID IID_IDebugFunctionObject;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("F71D9EA0-4269-48dc-9E8D-F86DEFA042B3")
    IDebugFunctionObject : public IDebugObject
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE CreatePrimitiveObject( 
            /* [in] */ OBJECT_TYPE ot,
            /* [out] */ IDebugObject **ppObject) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateObject( 
            /* [in] */ IDebugFunctionObject *pConstructor,
            /* [in] */ DWORD dwArgs,
            /* [size_is][in] */ IDebugObject *pArgs[  ],
            /* [out] */ IDebugObject **ppObject) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateObjectNoConstructor( 
            /* [in] */ IDebugField *pClassField,
            /* [out] */ IDebugObject **ppObject) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateArrayObject( 
            /* [in] */ OBJECT_TYPE ot,
            /* [in] */ IDebugField *pClassField,
            /* [in] */ DWORD dwRank,
            /* [size_is][in] */ DWORD dwDims[  ],
            /* [size_is][in] */ DWORD dwLowBounds[  ],
            /* [out] */ IDebugObject **ppObject) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE CreateStringObject( 
            /* [in] */ LPCOLESTR pcstrString,
            /* [out] */ IDebugObject **ppOjbect) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Evaluate( 
            /* [size_is][in] */ IDebugObject **ppParams,
            /* [in] */ DWORD dwParams,
            /* [in] */ DWORD dwTimeout,
            /* [out] */ IDebugObject **ppResult) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDebugFunctionObjectVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDebugFunctionObject * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDebugFunctionObject * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDebugFunctionObject * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetSize )( 
            IDebugFunctionObject * This,
            /* [out] */ UINT *pnSize);
        
        HRESULT ( STDMETHODCALLTYPE *GetValue )( 
            IDebugFunctionObject * This,
            /* [length_is][size_is][out] */ BYTE *pValue,
            /* [in] */ UINT nSize);
        
        HRESULT ( STDMETHODCALLTYPE *SetValue )( 
            IDebugFunctionObject * This,
            /* [size_is][in] */ BYTE *pValue,
            /* [in] */ UINT nSize);
        
        HRESULT ( STDMETHODCALLTYPE *SetReferenceValue )( 
            IDebugFunctionObject * This,
            /* [in] */ IDebugObject *pObject);
        
        HRESULT ( STDMETHODCALLTYPE *GetMemoryContext )( 
            IDebugFunctionObject * This,
            IDebugMemoryContext2 **pContext);
        
        HRESULT ( STDMETHODCALLTYPE *GetManagedDebugObject )( 
            IDebugFunctionObject * This,
            /* [out] */ IDebugManagedObject **ppObject);
        
        HRESULT ( STDMETHODCALLTYPE *IsNullReference )( 
            IDebugFunctionObject * This,
            /* [out] */ BOOL *pfIsNull);
        
        HRESULT ( STDMETHODCALLTYPE *IsEqual )( 
            IDebugFunctionObject * This,
            /* [in] */ IDebugObject *pObject,
            /* [out] */ BOOL *pfIsEqual);
        
        HRESULT ( STDMETHODCALLTYPE *IsReadOnly )( 
            IDebugFunctionObject * This,
            /* [out] */ BOOL *pfIsReadOnly);
        
        HRESULT ( STDMETHODCALLTYPE *IsProxy )( 
            IDebugFunctionObject * This,
            /* [out] */ BOOL *pfIsProxy);
        
        HRESULT ( STDMETHODCALLTYPE *CreatePrimitiveObject )( 
            IDebugFunctionObject * This,
            /* [in] */ OBJECT_TYPE ot,
            /* [out] */ IDebugObject **ppObject);
        
        HRESULT ( STDMETHODCALLTYPE *CreateObject )( 
            IDebugFunctionObject * This,
            /* [in] */ IDebugFunctionObject *pConstructor,
            /* [in] */ DWORD dwArgs,
            /* [size_is][in] */ IDebugObject *pArgs[  ],
            /* [out] */ IDebugObject **ppObject);
        
        HRESULT ( STDMETHODCALLTYPE *CreateObjectNoConstructor )( 
            IDebugFunctionObject * This,
            /* [in] */ IDebugField *pClassField,
            /* [out] */ IDebugObject **ppObject);
        
        HRESULT ( STDMETHODCALLTYPE *CreateArrayObject )( 
            IDebugFunctionObject * This,
            /* [in] */ OBJECT_TYPE ot,
            /* [in] */ IDebugField *pClassField,
            /* [in] */ DWORD dwRank,
            /* [size_is][in] */ DWORD dwDims[  ],
            /* [size_is][in] */ DWORD dwLowBounds[  ],
            /* [out] */ IDebugObject **ppObject);
        
        HRESULT ( STDMETHODCALLTYPE *CreateStringObject )( 
            IDebugFunctionObject * This,
            /* [in] */ LPCOLESTR pcstrString,
            /* [out] */ IDebugObject **ppOjbect);
        
        HRESULT ( STDMETHODCALLTYPE *Evaluate )( 
            IDebugFunctionObject * This,
            /* [size_is][in] */ IDebugObject **ppParams,
            /* [in] */ DWORD dwParams,
            /* [in] */ DWORD dwTimeout,
            /* [out] */ IDebugObject **ppResult);
        
        END_INTERFACE
    } IDebugFunctionObjectVtbl;

    interface IDebugFunctionObject
    {
        CONST_VTBL struct IDebugFunctionObjectVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDebugFunctionObject_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDebugFunctionObject_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDebugFunctionObject_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDebugFunctionObject_GetSize(This,pnSize)	\
    (This)->lpVtbl -> GetSize(This,pnSize)

#define IDebugFunctionObject_GetValue(This,pValue,nSize)	\
    (This)->lpVtbl -> GetValue(This,pValue,nSize)

#define IDebugFunctionObject_SetValue(This,pValue,nSize)	\
    (This)->lpVtbl -> SetValue(This,pValue,nSize)

#define IDebugFunctionObject_SetReferenceValue(This,pObject)	\
    (This)->lpVtbl -> SetReferenceValue(This,pObject)

#define IDebugFunctionObject_GetMemoryContext(This,pContext)	\
    (This)->lpVtbl -> GetMemoryContext(This,pContext)

#define IDebugFunctionObject_GetManagedDebugObject(This,ppObject)	\
    (This)->lpVtbl -> GetManagedDebugObject(This,ppObject)

#define IDebugFunctionObject_IsNullReference(This,pfIsNull)	\
    (This)->lpVtbl -> IsNullReference(This,pfIsNull)

#define IDebugFunctionObject_IsEqual(This,pObject,pfIsEqual)	\
    (This)->lpVtbl -> IsEqual(This,pObject,pfIsEqual)

#define IDebugFunctionObject_IsReadOnly(This,pfIsReadOnly)	\
    (This)->lpVtbl -> IsReadOnly(This,pfIsReadOnly)

#define IDebugFunctionObject_IsProxy(This,pfIsProxy)	\
    (This)->lpVtbl -> IsProxy(This,pfIsProxy)


#define IDebugFunctionObject_CreatePrimitiveObject(This,ot,ppObject)	\
    (This)->lpVtbl -> CreatePrimitiveObject(This,ot,ppObject)

#define IDebugFunctionObject_CreateObject(This,pConstructor,dwArgs,pArgs,ppObject)	\
    (This)->lpVtbl -> CreateObject(This,pConstructor,dwArgs,pArgs,ppObject)

#define IDebugFunctionObject_CreateObjectNoConstructor(This,pClassField,ppObject)	\
    (This)->lpVtbl -> CreateObjectNoConstructor(This,pClassField,ppObject)

#define IDebugFunctionObject_CreateArrayObject(This,ot,pClassField,dwRank,dwDims,dwLowBounds,ppObject)	\
    (This)->lpVtbl -> CreateArrayObject(This,ot,pClassField,dwRank,dwDims,dwLowBounds,ppObject)

#define IDebugFunctionObject_CreateStringObject(This,pcstrString,ppOjbect)	\
    (This)->lpVtbl -> CreateStringObject(This,pcstrString,ppOjbect)

#define IDebugFunctionObject_Evaluate(This,ppParams,dwParams,dwTimeout,ppResult)	\
    (This)->lpVtbl -> Evaluate(This,ppParams,dwParams,dwTimeout,ppResult)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDebugFunctionObject_CreatePrimitiveObject_Proxy( 
    IDebugFunctionObject * This,
    /* [in] */ OBJECT_TYPE ot,
    /* [out] */ IDebugObject **ppObject);


void __RPC_STUB IDebugFunctionObject_CreatePrimitiveObject_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugFunctionObject_CreateObject_Proxy( 
    IDebugFunctionObject * This,
    /* [in] */ IDebugFunctionObject *pConstructor,
    /* [in] */ DWORD dwArgs,
    /* [size_is][in] */ IDebugObject *pArgs[  ],
    /* [out] */ IDebugObject **ppObject);


void __RPC_STUB IDebugFunctionObject_CreateObject_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugFunctionObject_CreateObjectNoConstructor_Proxy( 
    IDebugFunctionObject * This,
    /* [in] */ IDebugField *pClassField,
    /* [out] */ IDebugObject **ppObject);


void __RPC_STUB IDebugFunctionObject_CreateObjectNoConstructor_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugFunctionObject_CreateArrayObject_Proxy( 
    IDebugFunctionObject * This,
    /* [in] */ OBJECT_TYPE ot,
    /* [in] */ IDebugField *pClassField,
    /* [in] */ DWORD dwRank,
    /* [size_is][in] */ DWORD dwDims[  ],
    /* [size_is][in] */ DWORD dwLowBounds[  ],
    /* [out] */ IDebugObject **ppObject);


void __RPC_STUB IDebugFunctionObject_CreateArrayObject_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugFunctionObject_CreateStringObject_Proxy( 
    IDebugFunctionObject * This,
    /* [in] */ LPCOLESTR pcstrString,
    /* [out] */ IDebugObject **ppOjbect);


void __RPC_STUB IDebugFunctionObject_CreateStringObject_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugFunctionObject_Evaluate_Proxy( 
    IDebugFunctionObject * This,
    /* [size_is][in] */ IDebugObject **ppParams,
    /* [in] */ DWORD dwParams,
    /* [in] */ DWORD dwTimeout,
    /* [out] */ IDebugObject **ppResult);


void __RPC_STUB IDebugFunctionObject_Evaluate_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDebugFunctionObject_INTERFACE_DEFINED__ */


#ifndef __IDebugManagedObject_INTERFACE_DEFINED__
#define __IDebugManagedObject_INTERFACE_DEFINED__

/* interface IDebugManagedObject */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_IDebugManagedObject;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("71AF87C9-66C5-49e4-A602-B9012115AFD5")
    IDebugManagedObject : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetManagedObject( 
            /* [out] */ IUnknown **ppManagedObject) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetFromManagedObject( 
            /* [in] */ IUnknown *pManagedObject) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDebugManagedObjectVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDebugManagedObject * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDebugManagedObject * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDebugManagedObject * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetManagedObject )( 
            IDebugManagedObject * This,
            /* [out] */ IUnknown **ppManagedObject);
        
        HRESULT ( STDMETHODCALLTYPE *SetFromManagedObject )( 
            IDebugManagedObject * This,
            /* [in] */ IUnknown *pManagedObject);
        
        END_INTERFACE
    } IDebugManagedObjectVtbl;

    interface IDebugManagedObject
    {
        CONST_VTBL struct IDebugManagedObjectVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDebugManagedObject_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDebugManagedObject_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDebugManagedObject_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDebugManagedObject_GetManagedObject(This,ppManagedObject)	\
    (This)->lpVtbl -> GetManagedObject(This,ppManagedObject)

#define IDebugManagedObject_SetFromManagedObject(This,pManagedObject)	\
    (This)->lpVtbl -> SetFromManagedObject(This,pManagedObject)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDebugManagedObject_GetManagedObject_Proxy( 
    IDebugManagedObject * This,
    /* [out] */ IUnknown **ppManagedObject);


void __RPC_STUB IDebugManagedObject_GetManagedObject_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugManagedObject_SetFromManagedObject_Proxy( 
    IDebugManagedObject * This,
    /* [in] */ IUnknown *pManagedObject);


void __RPC_STUB IDebugManagedObject_SetFromManagedObject_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDebugManagedObject_INTERFACE_DEFINED__ */


#ifndef __IDebugBinder_INTERFACE_DEFINED__
#define __IDebugBinder_INTERFACE_DEFINED__

/* interface IDebugBinder */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_IDebugBinder;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("C077C833-476C-11d2-B73C-0000F87572EF")
    IDebugBinder : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Bind( 
            /* [in] */ IDebugObject *pContainer,
            /* [in] */ IDebugField *pField,
            /* [out] */ IDebugObject **ppObject) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ResolveDynamicType( 
            /* [in] */ IDebugDynamicField *pDynamic,
            /* [out] */ IDebugField **ppResolved) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ResolveRuntimeType( 
            /* [in] */ IDebugObject *pObject,
            /* [out] */ IDebugField **ppResolved) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetMemoryContext( 
            /* [in] */ IDebugField *pField,
            /* [in] */ DWORD dwConstant,
            /* [out] */ IDebugMemoryContext2 **ppMemCxt) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetFunctionObject( 
            /* [out] */ IDebugFunctionObject **ppFunction) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDebugBinderVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDebugBinder * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDebugBinder * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDebugBinder * This);
        
        HRESULT ( STDMETHODCALLTYPE *Bind )( 
            IDebugBinder * This,
            /* [in] */ IDebugObject *pContainer,
            /* [in] */ IDebugField *pField,
            /* [out] */ IDebugObject **ppObject);
        
        HRESULT ( STDMETHODCALLTYPE *ResolveDynamicType )( 
            IDebugBinder * This,
            /* [in] */ IDebugDynamicField *pDynamic,
            /* [out] */ IDebugField **ppResolved);
        
        HRESULT ( STDMETHODCALLTYPE *ResolveRuntimeType )( 
            IDebugBinder * This,
            /* [in] */ IDebugObject *pObject,
            /* [out] */ IDebugField **ppResolved);
        
        HRESULT ( STDMETHODCALLTYPE *GetMemoryContext )( 
            IDebugBinder * This,
            /* [in] */ IDebugField *pField,
            /* [in] */ DWORD dwConstant,
            /* [out] */ IDebugMemoryContext2 **ppMemCxt);
        
        HRESULT ( STDMETHODCALLTYPE *GetFunctionObject )( 
            IDebugBinder * This,
            /* [out] */ IDebugFunctionObject **ppFunction);
        
        END_INTERFACE
    } IDebugBinderVtbl;

    interface IDebugBinder
    {
        CONST_VTBL struct IDebugBinderVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDebugBinder_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDebugBinder_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDebugBinder_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDebugBinder_Bind(This,pContainer,pField,ppObject)	\
    (This)->lpVtbl -> Bind(This,pContainer,pField,ppObject)

#define IDebugBinder_ResolveDynamicType(This,pDynamic,ppResolved)	\
    (This)->lpVtbl -> ResolveDynamicType(This,pDynamic,ppResolved)

#define IDebugBinder_ResolveRuntimeType(This,pObject,ppResolved)	\
    (This)->lpVtbl -> ResolveRuntimeType(This,pObject,ppResolved)

#define IDebugBinder_GetMemoryContext(This,pField,dwConstant,ppMemCxt)	\
    (This)->lpVtbl -> GetMemoryContext(This,pField,dwConstant,ppMemCxt)

#define IDebugBinder_GetFunctionObject(This,ppFunction)	\
    (This)->lpVtbl -> GetFunctionObject(This,ppFunction)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDebugBinder_Bind_Proxy( 
    IDebugBinder * This,
    /* [in] */ IDebugObject *pContainer,
    /* [in] */ IDebugField *pField,
    /* [out] */ IDebugObject **ppObject);


void __RPC_STUB IDebugBinder_Bind_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugBinder_ResolveDynamicType_Proxy( 
    IDebugBinder * This,
    /* [in] */ IDebugDynamicField *pDynamic,
    /* [out] */ IDebugField **ppResolved);


void __RPC_STUB IDebugBinder_ResolveDynamicType_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugBinder_ResolveRuntimeType_Proxy( 
    IDebugBinder * This,
    /* [in] */ IDebugObject *pObject,
    /* [out] */ IDebugField **ppResolved);


void __RPC_STUB IDebugBinder_ResolveRuntimeType_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugBinder_GetMemoryContext_Proxy( 
    IDebugBinder * This,
    /* [in] */ IDebugField *pField,
    /* [in] */ DWORD dwConstant,
    /* [out] */ IDebugMemoryContext2 **ppMemCxt);


void __RPC_STUB IDebugBinder_GetMemoryContext_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugBinder_GetFunctionObject_Proxy( 
    IDebugBinder * This,
    /* [out] */ IDebugFunctionObject **ppFunction);


void __RPC_STUB IDebugBinder_GetFunctionObject_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDebugBinder_INTERFACE_DEFINED__ */


#ifndef __IDebugPointerObject_INTERFACE_DEFINED__
#define __IDebugPointerObject_INTERFACE_DEFINED__

/* interface IDebugPointerObject */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_IDebugPointerObject;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("112756A1-3F04-4ccd-BFD6-ACB4BCA614C9")
    IDebugPointerObject : public IDebugObject
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Dereference( 
            /* [in] */ DWORD dwIndex,
            /* [out] */ IDebugObject **ppObject) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetBytes( 
            /* [in] */ DWORD dwStart,
            /* [in] */ DWORD dwCount,
            /* [length_is][size_is][out] */ BYTE *pBytes,
            /* [out] */ DWORD *pdwBytes) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetBytes( 
            /* [in] */ DWORD dwStart,
            /* [in] */ DWORD dwCount,
            /* [length_is][size_is][in] */ BYTE *pBytes,
            /* [out] */ DWORD *pdwBytes) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDebugPointerObjectVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDebugPointerObject * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDebugPointerObject * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDebugPointerObject * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetSize )( 
            IDebugPointerObject * This,
            /* [out] */ UINT *pnSize);
        
        HRESULT ( STDMETHODCALLTYPE *GetValue )( 
            IDebugPointerObject * This,
            /* [length_is][size_is][out] */ BYTE *pValue,
            /* [in] */ UINT nSize);
        
        HRESULT ( STDMETHODCALLTYPE *SetValue )( 
            IDebugPointerObject * This,
            /* [size_is][in] */ BYTE *pValue,
            /* [in] */ UINT nSize);
        
        HRESULT ( STDMETHODCALLTYPE *SetReferenceValue )( 
            IDebugPointerObject * This,
            /* [in] */ IDebugObject *pObject);
        
        HRESULT ( STDMETHODCALLTYPE *GetMemoryContext )( 
            IDebugPointerObject * This,
            IDebugMemoryContext2 **pContext);
        
        HRESULT ( STDMETHODCALLTYPE *GetManagedDebugObject )( 
            IDebugPointerObject * This,
            /* [out] */ IDebugManagedObject **ppObject);
        
        HRESULT ( STDMETHODCALLTYPE *IsNullReference )( 
            IDebugPointerObject * This,
            /* [out] */ BOOL *pfIsNull);
        
        HRESULT ( STDMETHODCALLTYPE *IsEqual )( 
            IDebugPointerObject * This,
            /* [in] */ IDebugObject *pObject,
            /* [out] */ BOOL *pfIsEqual);
        
        HRESULT ( STDMETHODCALLTYPE *IsReadOnly )( 
            IDebugPointerObject * This,
            /* [out] */ BOOL *pfIsReadOnly);
        
        HRESULT ( STDMETHODCALLTYPE *IsProxy )( 
            IDebugPointerObject * This,
            /* [out] */ BOOL *pfIsProxy);
        
        HRESULT ( STDMETHODCALLTYPE *Dereference )( 
            IDebugPointerObject * This,
            /* [in] */ DWORD dwIndex,
            /* [out] */ IDebugObject **ppObject);
        
        HRESULT ( STDMETHODCALLTYPE *GetBytes )( 
            IDebugPointerObject * This,
            /* [in] */ DWORD dwStart,
            /* [in] */ DWORD dwCount,
            /* [length_is][size_is][out] */ BYTE *pBytes,
            /* [out] */ DWORD *pdwBytes);
        
        HRESULT ( STDMETHODCALLTYPE *SetBytes )( 
            IDebugPointerObject * This,
            /* [in] */ DWORD dwStart,
            /* [in] */ DWORD dwCount,
            /* [length_is][size_is][in] */ BYTE *pBytes,
            /* [out] */ DWORD *pdwBytes);
        
        END_INTERFACE
    } IDebugPointerObjectVtbl;

    interface IDebugPointerObject
    {
        CONST_VTBL struct IDebugPointerObjectVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDebugPointerObject_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDebugPointerObject_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDebugPointerObject_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDebugPointerObject_GetSize(This,pnSize)	\
    (This)->lpVtbl -> GetSize(This,pnSize)

#define IDebugPointerObject_GetValue(This,pValue,nSize)	\
    (This)->lpVtbl -> GetValue(This,pValue,nSize)

#define IDebugPointerObject_SetValue(This,pValue,nSize)	\
    (This)->lpVtbl -> SetValue(This,pValue,nSize)

#define IDebugPointerObject_SetReferenceValue(This,pObject)	\
    (This)->lpVtbl -> SetReferenceValue(This,pObject)

#define IDebugPointerObject_GetMemoryContext(This,pContext)	\
    (This)->lpVtbl -> GetMemoryContext(This,pContext)

#define IDebugPointerObject_GetManagedDebugObject(This,ppObject)	\
    (This)->lpVtbl -> GetManagedDebugObject(This,ppObject)

#define IDebugPointerObject_IsNullReference(This,pfIsNull)	\
    (This)->lpVtbl -> IsNullReference(This,pfIsNull)

#define IDebugPointerObject_IsEqual(This,pObject,pfIsEqual)	\
    (This)->lpVtbl -> IsEqual(This,pObject,pfIsEqual)

#define IDebugPointerObject_IsReadOnly(This,pfIsReadOnly)	\
    (This)->lpVtbl -> IsReadOnly(This,pfIsReadOnly)

#define IDebugPointerObject_IsProxy(This,pfIsProxy)	\
    (This)->lpVtbl -> IsProxy(This,pfIsProxy)


#define IDebugPointerObject_Dereference(This,dwIndex,ppObject)	\
    (This)->lpVtbl -> Dereference(This,dwIndex,ppObject)

#define IDebugPointerObject_GetBytes(This,dwStart,dwCount,pBytes,pdwBytes)	\
    (This)->lpVtbl -> GetBytes(This,dwStart,dwCount,pBytes,pdwBytes)

#define IDebugPointerObject_SetBytes(This,dwStart,dwCount,pBytes,pdwBytes)	\
    (This)->lpVtbl -> SetBytes(This,dwStart,dwCount,pBytes,pdwBytes)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDebugPointerObject_Dereference_Proxy( 
    IDebugPointerObject * This,
    /* [in] */ DWORD dwIndex,
    /* [out] */ IDebugObject **ppObject);


void __RPC_STUB IDebugPointerObject_Dereference_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugPointerObject_GetBytes_Proxy( 
    IDebugPointerObject * This,
    /* [in] */ DWORD dwStart,
    /* [in] */ DWORD dwCount,
    /* [length_is][size_is][out] */ BYTE *pBytes,
    /* [out] */ DWORD *pdwBytes);


void __RPC_STUB IDebugPointerObject_GetBytes_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDebugPointerObject_SetBytes_Proxy( 
    IDebugPointerObject * This,
    /* [in] */ DWORD dwStart,
    /* [in] */ DWORD dwCount,
    /* [length_is][size_is][in] */ BYTE *pBytes,
    /* [out] */ DWORD *pdwBytes);


void __RPC_STUB IDebugPointerObject_SetBytes_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDebugPointerObject_INTERFACE_DEFINED__ */


#ifndef __IDebugParsedExpression_INTERFACE_DEFINED__
#define __IDebugParsedExpression_INTERFACE_DEFINED__

/* interface IDebugParsedExpression */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_IDebugParsedExpression;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("7895C94C-5A3F-11d2-B742-0000F87572EF")
    IDebugParsedExpression : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE EvaluateSync( 
            /* [in] */ DWORD dwEvalFlags,
            /* [in] */ DWORD dwTimeout,
            /* [in] */ IDebugSymbolProvider *pSymbolProvider,
            /* [in] */ IDebugAddress *pAddress,
            /* [in] */ IDebugBinder *pBinder,
            /* [in] */ BSTR bstrResultType,
            /* [out] */ IDebugProperty2 **ppResult) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDebugParsedExpressionVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDebugParsedExpression * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDebugParsedExpression * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDebugParsedExpression * This);
        
        HRESULT ( STDMETHODCALLTYPE *EvaluateSync )( 
            IDebugParsedExpression * This,
            /* [in] */ DWORD dwEvalFlags,
            /* [in] */ DWORD dwTimeout,
            /* [in] */ IDebugSymbolProvider *pSymbolProvider,
            /* [in] */ IDebugAddress *pAddress,
            /* [in] */ IDebugBinder *pBinder,
            /* [in] */ BSTR bstrResultType,
            /* [out] */ IDebugProperty2 **ppResult);
        
        END_INTERFACE
    } IDebugParsedExpressionVtbl;

    interface IDebugParsedExpression
    {
        CONST_VTBL struct IDebugParsedExpressionVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDebugParsedExpression_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDebugParsedExpression_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDebugParsedExpression_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDebugParsedExpression_EvaluateSync(This,dwEvalFlags,dwTimeout,pSymbolProvider,pAddress,pBinder,bstrResultType,ppResult)	\
    (This)->lpVtbl -> EvaluateSync(This,dwEvalFlags,dwTimeout,pSymbolProvider,pAddress,pBinder,bstrResultType,ppResult)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDebugParsedExpression_EvaluateSync_Proxy( 
    IDebugParsedExpression * This,
    /* [in] */ DWORD dwEvalFlags,
    /* [in] */ DWORD dwTimeout,
    /* [in] */ IDebugSymbolProvider *pSymbolProvider,
    /* [in] */ IDebugAddress *pAddress,
    /* [in] */ IDebugBinder *pBinder,
    /* [in] */ BSTR bstrResultType,
    /* [out] */ IDebugProperty2 **ppResult);


void __RPC_STUB IDebugParsedExpression_EvaluateSync_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDebugParsedExpression_INTERFACE_DEFINED__ */


#ifndef __IEnumDebugObjects_INTERFACE_DEFINED__
#define __IEnumDebugObjects_INTERFACE_DEFINED__

/* interface IEnumDebugObjects */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_IEnumDebugObjects;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("0881751C-99F4-11d2-B767-0000F87572EF")
    IEnumDebugObjects : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Next( 
            /* [in] */ ULONG celt,
            /* [length_is][size_is][out] */ IDebugObject **rgelt,
            /* [out][in] */ ULONG *pceltFetched) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Skip( 
            /* [in] */ ULONG celt) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Reset( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Clone( 
            /* [out] */ IEnumDebugObjects **ppEnum) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetCount( 
            /* [out] */ ULONG *pcelt) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IEnumDebugObjectsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IEnumDebugObjects * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IEnumDebugObjects * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IEnumDebugObjects * This);
        
        HRESULT ( STDMETHODCALLTYPE *Next )( 
            IEnumDebugObjects * This,
            /* [in] */ ULONG celt,
            /* [length_is][size_is][out] */ IDebugObject **rgelt,
            /* [out][in] */ ULONG *pceltFetched);
        
        HRESULT ( STDMETHODCALLTYPE *Skip )( 
            IEnumDebugObjects * This,
            /* [in] */ ULONG celt);
        
        HRESULT ( STDMETHODCALLTYPE *Reset )( 
            IEnumDebugObjects * This);
        
        HRESULT ( STDMETHODCALLTYPE *Clone )( 
            IEnumDebugObjects * This,
            /* [out] */ IEnumDebugObjects **ppEnum);
        
        HRESULT ( STDMETHODCALLTYPE *GetCount )( 
            IEnumDebugObjects * This,
            /* [out] */ ULONG *pcelt);
        
        END_INTERFACE
    } IEnumDebugObjectsVtbl;

    interface IEnumDebugObjects
    {
        CONST_VTBL struct IEnumDebugObjectsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IEnumDebugObjects_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IEnumDebugObjects_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IEnumDebugObjects_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IEnumDebugObjects_Next(This,celt,rgelt,pceltFetched)	\
    (This)->lpVtbl -> Next(This,celt,rgelt,pceltFetched)

#define IEnumDebugObjects_Skip(This,celt)	\
    (This)->lpVtbl -> Skip(This,celt)

#define IEnumDebugObjects_Reset(This)	\
    (This)->lpVtbl -> Reset(This)

#define IEnumDebugObjects_Clone(This,ppEnum)	\
    (This)->lpVtbl -> Clone(This,ppEnum)

#define IEnumDebugObjects_GetCount(This,pcelt)	\
    (This)->lpVtbl -> GetCount(This,pcelt)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IEnumDebugObjects_Next_Proxy( 
    IEnumDebugObjects * This,
    /* [in] */ ULONG celt,
    /* [length_is][size_is][out] */ IDebugObject **rgelt,
    /* [out][in] */ ULONG *pceltFetched);


void __RPC_STUB IEnumDebugObjects_Next_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IEnumDebugObjects_Skip_Proxy( 
    IEnumDebugObjects * This,
    /* [in] */ ULONG celt);


void __RPC_STUB IEnumDebugObjects_Skip_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IEnumDebugObjects_Reset_Proxy( 
    IEnumDebugObjects * This);


void __RPC_STUB IEnumDebugObjects_Reset_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IEnumDebugObjects_Clone_Proxy( 
    IEnumDebugObjects * This,
    /* [out] */ IEnumDebugObjects **ppEnum);


void __RPC_STUB IEnumDebugObjects_Clone_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IEnumDebugObjects_GetCount_Proxy( 
    IEnumDebugObjects * This,
    /* [out] */ ULONG *pcelt);


void __RPC_STUB IEnumDebugObjects_GetCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IEnumDebugObjects_INTERFACE_DEFINED__ */


/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long *, BSTR * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


