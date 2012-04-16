/**
@file CEE.h
@version 1.4
@author George Chu <gchu@microsoft.com>
@copyright Microsoft 1999
**/
#ifndef __EESDK_EXPRESSION_EVALUATOR_COMPONENT__
#define __EESDK_EXPRESSION_EVALUATOR_COMPONENT__

#ifdef __EE_DOMAIN__
#else
#error must #include "EEDomain.h" before #include "CEE.h"
#endif

namespace EEDomain {

	/**
	@semantics fLanguageSpecificUseCases

	Specifies the domain of use-cases supplied by the language to customize
	the instance of CEE.
	**/
	struct fLanguageSpecific {

		// registry root setting functor
		EEDomain::SetRegistryRootUseCase* pfSetRegistryRoot;

		// locale setting functor
		EEDomain::SetLocaleUseCase* pfSetLocale;

		// primitive value-setting functor
		EEDomain::SetValueUseCase* pfSetValue;

		// intrinsic type related functors
		Functor<BOOL, EEDomain::fIsIntrinsicType&>* pfIsIntrinsicType;
		Functor<BSTR, EEDomain::fTranslateIntrinsicType&>* pfTranslateIntrinsicType;
		Functor<BSTR, EEDomain::fTranslateIntrinsicValue&>* pfTranslateIntrinsicValue;
		Functor<DBG_ATTRIB_FLAGS, EEDomain::fTranslateIntrinsicAttrib&>* pfTranslateIntrinsicAttrib;
		Functor<BSTR, EEDomain::fTranslateIntrinsicForDerivedMost&>* pfTranslateIntrinsicForDerivedMost;

		// primitive type related functors
		EEDomain::TranslatePrimitiveTypeUseCase* pfTranslatePrimitiveType;
		EEDomain::TranslatePrimitiveValueUseCase* pfTranslatePrimitiveValue;
		EEDomain::TranslateAttribUseCase* pfTranslateAttrib;
		EEDomain::CastExpressionUseCase* pfCastExpression;

		// appearance related functors
		EEDomain::TranslateBindOperatorUseCase* pfTranslateBindOperator;
		EEDomain::TranslateIdentifierUseCase* pfTranslateIdentifier;
		EEDomain::TranslateArrayElementUseCase* pfTranslateArrayElement;
		EEDomain::TranslateArrayTypeUseCase* pfTranslateArrayType; // VB-specific

		// complex expression evaluation functor
		Functor<HRESULT, EEDomain::fParseExpression&>* pfParseExpression;

		// function breakpoint functor
		Functor<BOOL, EEDomain::fFormatMethodLocation&>* pfFormatMethodLocation;

		// filtering property children functor
		EEDomain::FilterPropertyChildrenUseCase* pfFilterPropertyChildren;

		// obtaining "this" field if SH doesn't return one.
		EEDomain::GetThisFieldUseCase* pfGetThisField; // JScript-specific

		// format error messages [C#/MC++ specific]
		EEDomain::FormatErrorMessageUseCase* pfFormatErrorMessage;
	};
};

/**
@semantics CEE

Denotes the language-specific EE providing expression evaluator
on behalf of the managed DE.
**/
class CEE : public IDebugExpressionEvaluator {
public:

	EEDomain::fLanguageSpecific m_LanguageSpecificUseCases;

private:

	LONG m_ReferenceCount;

public:

	// class invariant
	bool ClassInvariant(void) const
	{
		return NULL != m_LanguageSpecificUseCases.pfTranslatePrimitiveType &&
			   NULL != m_LanguageSpecificUseCases.pfTranslatePrimitiveValue &&
			   NULL != m_LanguageSpecificUseCases.pfTranslateBindOperator &&
			   NULL != m_LanguageSpecificUseCases.pfTranslateIdentifier &&
			   NULL != m_LanguageSpecificUseCases.pfParseExpression;
	}

	// constructor & destructor
	CEE(EEDomain::fLanguageSpecific& in_LanguageSpecificUseCases);
	virtual ~CEE(void);

	// public methods for IUnknown interface
	virtual DWORD __stdcall AddRef(void);
	virtual DWORD __stdcall Release(void);
	virtual HRESULT __stdcall QueryInterface(REFIID, void**);

	// public methods for IDebugExpressionEvaluator interface
	virtual HRESULT __stdcall Parse
		( LPCOLESTR in_szExprText,
          PARSEFLAGS in_FLAGS,
          UINT in_Radix,
		  BSTR* out_pbstrError,
		  UINT* inout_pichError,
		  IDebugParsedExpression** out_ppParsedExpression );

	virtual HRESULT __stdcall GetMethodProperty
		( IDebugSymbolProvider* in_pSymbolProvider,
		  IDebugAddress* in_pAddress,
		  IDebugBinder* in_pBinder,
          BOOL fIncludeHiddenLocals,
		  IDebugProperty2** out_ppProperty );

	virtual HRESULT __stdcall GetMethodLocationProperty
		( LPCOLESTR in_szFullyQualifiedMethodPlusOffset,
		  IDebugSymbolProvider* in_pSymbolProvider,
		  IDebugAddress* in_pAddress,
		  IDebugBinder* in_pBinder,
		  IDebugProperty2** out_ppProperty );

	virtual HRESULT __stdcall SetLocale
		( WORD wLangID );

	virtual HRESULT __stdcall SetRegistryRoot
		( LPCOLESTR in_szRegistryRoot );

	// public methods for creating the IDebugProperty2 component
	HRESULT MakeProperty
		( PropertyContext* in_pPropertyContext,
		  IDebugField* in_pField,
		  IDebugProperty2** out_ppProperty,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib = DBG_ATTRIB_NONE,
		  EVALFLAGS in_EVALFLAGS = 0 );

	HRESULT MakeProperty
		( PropertyContext* in_pPropertyContext,
		  IDebugField* in_pField,
		  IDebugObject* in_pObject,
		  IDebugProperty2** out_ppProperty,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib = DBG_ATTRIB_NONE,
		  EVALFLAGS in_EVALFLAGS = 0 );

	HRESULT MakeProperty
		( LPCOLESTR in_szPropertyName,
		  VARIANTARG* in_pVarValue,
		  IDebugProperty2** out_ppProperty );

	HRESULT MakeProperty
		( LPCOLESTR in_szPropertyName,
		  LPCOLESTR in_szImplicitScope,
		  VARIANTARG* in_pVarValue,
		  IDebugProperty2** out_ppProperty );

	HRESULT MakeProperty
		( LPCOLESTR in_szPropertyName,
		  VARIANTARG* in_pVarValue,
		  IDebugBinder* in_pBinder,
		  IDebugProperty2** out_ppProperty,
		  EVALFLAGS in_EVALFLAGS = 0 );

	HRESULT MakeProperty
		( LPCOLESTR in_szPropertyName,
		  LPCOLESTR in_szPropertyValue,
		  LPCOLESTR in_szPropertyType,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib,
		  IDebugProperty2** out_ppProperty );

	HRESULT MakeProperty // creation of overloaded properties
		( LPCOLESTR in_szPropertyName,
		  LPCOLESTR in_szPropertyValue,
		  LPCOLESTR in_szPropertyType,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib,
		  PropertyContext* in_pPropertyContext,
		  IEnumDebugFields* in_pEnumFields,
		  IDebugProperty2** out_ppProperty );

	HRESULT MakeProperty // creation of a dummy IDebugProperty2 object that has expandable children
		( LPCOLESTR in_szPropertyName,
		  LPCOLESTR in_szPropertyValue,
		  LPCOLESTR in_szPropertyType,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib,
		  IEnumDebugPropertyInfo2* in_pEnumPropertyInfo,
		  IDebugProperty2** out_ppProperty );

	HRESULT MakeProperty // creation of a dummy IDebugProperty2 object that has one expandable child
		( LPCOLESTR in_szPropertyName,
		  LPCOLESTR in_szPropertyValue,
		  LPCOLESTR in_szPropertyType,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib,
		  IDebugProperty2* in_pOnlyChild,
		  IDebugProperty2** out_ppProperty );

	HRESULT MakeProperty
		( IDebugSymbolProvider* in_pSymbolProvider,
		  IDebugAddress* in_pAddress,
		  IDebugBinder* in_pBinder,
		  IDebugField* in_pField,
		  IDebugObject* in_pObject,
		  IDebugProperty2** out_ppProperty,
		  UINT64 in_MemoryOffset = 0L,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib = DBG_ATTRIB_NONE,
		  EVALFLAGS in_EVALFLAGS = 0 );

	HRESULT MakeProperty // VB hack to enable expansion of "me" children
		( IDebugSymbolProvider* in_pSymbolProvider,
		  IDebugAddress* in_pAddress,
		  IDebugBinder* in_pBinder,
		  REFGUID in_guidFilterThis, // signals the translation of "this" to "me"
		  IDebugField* in_pField,
		  IDebugObject* in_pObject,
		  IDebugProperty2** out_ppProperty,
		  UINT64 in_MemoryOffset = 0L,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib = DBG_ATTRIB_NONE,
		  EVALFLAGS in_EVALFLAGS = 0 );

	HRESULT MakeProperty
		( PropertyContext* in_pPropertyContext,
		  IDebugObject* in_pObject,
		  LONG in_ObjectID,
		  IDebugProperty2** out_ppProperty,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib = DBG_ATTRIB_NONE,
		  EVALFLAGS in_EVALFLAGS = 0 );

	HRESULT MakeProperty
		( CProperty* in_pCParentProperty,
		  IDebugSymbolProvider* in_pSymbolProvider,
		  IDebugAddress* in_pAddress,
		  IDebugBinder* in_pBinder,
		  IDebugField* in_pField,
		  IDebugObject* in_pObject,
		  IDebugProperty2** out_ppProperty,
		  UINT64 in_MemoryOffset = 0L,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib = DBG_ATTRIB_NONE,
		  EVALFLAGS in_EVALFLAGS = 0 );

	HRESULT MakeProperty
		( IDebugSymbolProvider* in_pSymbolProvider,
		  IDebugAddress* in_pAddress,
		  IDebugBinder* in_pBinder,
		  IDebugField* in_pField,
		  IDebugObject* in_pObject,
		  IDebugField* in_pParentField,
		  IDebugObject* in_pParentObject,
		  IDebugProperty2** out_ppProperty,
		  UINT64 in_MemoryOffset = 0L,
		  DBG_ATTRIB_FLAGS in_PropertyAttrib = DBG_ATTRIB_NONE,
		  EVALFLAGS in_EVALFLAGS = 0 );

	// public method for creating the IEnumDebugPropertyInfo2 component
	HRESULT MakeEnumPropertyInfo
		( CProperty* in_pCParentProperty,
		  DEBUGPROP_INFO_FLAGS in_FLAGS,
		  UINT in_RADIX,
		  REFGUID in_guidFILTER,
		  DBG_ATTRIB_FLAGS in_attribFILTER,
		  LPCOLESTR in_szNameFILTER,
		  IEnumDebugPropertyInfo2** out_ppEnumPropertyInfo );

private:

	// do not define
	CEE(CEE&);
	CEE& operator =(CEE&);
};


#endif __EESDK_EXPRESSION_EVALUATOR_COMPONENT__