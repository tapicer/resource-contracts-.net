/**
@file EEDomain.h
@version 1.2
@author George Chu <gchu@microsoft.com>
@copyright Microsoft 1999
**/
#ifndef __EE_DOMAIN__
#define __EE_DOMAIN__

// forward declaration
class PropertyContext;
class ArrayPropertyContext;
class CProperty;
class CEE;

// {F6786E85-5C8E-4350-8F96-F85199804B66}
DEFINE_GUID(GUID_EESDK_UNDERLYING_FIELD, 
0xf6786e85, 0x5c8e, 0x4350, 0x8f, 0x96, 0xf8, 0x51, 0x99, 0x80, 0x4b, 0x66);

// {1F925578-D53F-455b-977C-80B0231D65F9}
DEFINE_GUID(GUID_EESDK_UNDERLYING_OBJECT, 
0x1f925578, 0xd53f, 0x455b, 0x97, 0x7c, 0x80, 0xb0, 0x23, 0x1d, 0x65, 0xf9);

// {2CAAFD33-DD45-45b6-B5D9-4276C9B8835E}
DEFINE_GUID(GUID_EESDK_PROPERTY_IMPL, 
0x2caafd33, 0xdd45, 0x45b6, 0xb5, 0xd9, 0x42, 0x76, 0xc9, 0xb8, 0x83, 0x5e);

namespace EEDomain {

	struct fSetRegistryRoot {

		LPCOLESTR szRegistryRoot;
	};
	typedef Functor<BOOL, EEDomain::fSetRegistryRoot&> SetRegistryRootUseCase;

	struct fSetLocale {

		WORD LangID;
	};
	typedef Functor<BOOL, EEDomain::fSetLocale&> SetLocaleUseCase;

	struct fSetValue {

		CEE* pCEE;

		// [LHS] expression upon which the value will be set
		LPCOLESTR szExpression;
		LPCOLESTR szTranslatedType;

		// [RHS] value
		LPCOLESTR szValue;
		LPCOLESTR szTranslatedValue;
		LPCOLESTR szTranslatedValueType;

		// context related parameters
		IDebugSymbolProvider* pSymbolProvider;
		IDebugAddress* pAddress;
		IDebugBinder* pBinder;
		EVALFLAGS EvalFlags;

		// SH object denoting the LHS type
		IDebugField* pField;
		// DE object denoting the LHS expression
		IDebugObject* pObject;
		// DE object denoting the LHS parent object
		IDebugObject* pParentObject;
		// LHS property
		IDebugProperty2* pLHSProp;
		// RHS property
		IDebugProperty2* pRHSProp;
	};

	typedef Functor<BOOL, EEDomain::fSetValue&> SetValueUseCase;

	struct fTranslatePrimitiveType {

		LPCOLESTR szPrimitiveType;
		DWORD dwPrimitiveSize;
                IDebugField *pField;
	};

	typedef Functor<BSTR, EEDomain::fTranslatePrimitiveType&> TranslatePrimitiveTypeUseCase;

	struct fTranslatePrimitiveValue {

		BYTE* pValue;
		UINT ValueSize;
		UINT RADIX;
		LPCOLESTR szTranslatedPrimitiveType; // language-specific primitive type
	};
	typedef Functor<BSTR, EEDomain::fTranslatePrimitiveValue&> TranslatePrimitiveValueUseCase;

	struct fTranslateAttrib {

		LPCOLESTR szPropertyType;
		LPCOLESTR szPropertyValue;

		DBG_ATTRIB_FLAGS dwAttrib;
		FIELD_KIND FieldKind;

		// additional parameters for JScript EE
		IDebugSymbolProvider* pSymbolProvider;
		IDebugAddress* pAddress;
		IDebugBinder* pBinder;
		IDebugField* pField;
		IDebugObject* pObject;
		GUID* pGuidFILTER;
	};
	typedef Functor<DBG_ATTRIB_FLAGS, EEDomain::fTranslateAttrib&> TranslateAttribUseCase;

	struct fCastExpression {

		LPCOLESTR szType;
		LPCOLESTR szExpression;
	};
	typedef Functor<BSTR, EEDomain::fCastExpression&> CastExpressionUseCase;

	struct fIsIntrinsicType {

		IDebugField* pField;
		IDebugObject* pObject;

		LPCOLESTR szUntranslatedType;
	};
	typedef Functor<BOOL, EEDomain::fIsIntrinsicType&> IsIntrinsicTypeUseCase;

	struct fTranslateIntrinsicType {

		IDebugField* pField;
		IDebugObject* pObject;
		LPCOLESTR szUntranslatedType;
		UINT RADIX;

		IDebugSymbolProvider* pSymbolProvider;
		IDebugAddress* pAddress;
		IDebugBinder* pBinder;
	};
	typedef Functor<BSTR, EEDomain::fTranslateIntrinsicType&> TranslateIntrinsicTypeUseCase;

	struct fTranslateIntrinsicValue {

		CEE* pCEE;
		IDebugField* pField;
		IDebugObject* pObject;
		IDebugProperty2* pProperty;
		LPCOLESTR szUntranslatedValue;
		LPCOLESTR szIntrinsicType;
		UINT RADIX;
		EVALFLAGS EvalFlags;

		IDebugSymbolProvider* pSymbolProvider;
		IDebugAddress* pAddress;
		IDebugBinder* pBinder;
	};
	typedef Functor<BSTR, EEDomain::fTranslateIntrinsicValue&> TranslateIntrinsicValueUseCase;

	struct fTranslateIntrinsicAttrib {

		IDebugField* pField;
		IDebugObject* pObject;
		DBG_ATTRIB_FLAGS UntranslatedAttrib;
		LPCOLESTR szIntrinsicType;

		IDebugSymbolProvider* pSymbolProvider;
		IDebugAddress* pAddress;
		IDebugBinder* pBinder;
	};
	typedef Functor<DBG_ATTRIB_FLAGS, EEDomain::fTranslateIntrinsicAttrib&> TranslateIntrinsicAttribUseCase;

	struct fTranslateIntrinsicForDerivedMost {

		IDebugProperty2* pProperty;
		LPOLESTR szDerivedMost;
	};
	typedef Functor<BSTR, EEDomain::fTranslateIntrinsicForDerivedMost&> TranslateIntrinsicForDerivedMostUseCase;

	struct fTranslateIdentifier {

		LPCOLESTR szIdentifier;
		IDebugField* pField;
		const GUID* pGuidFILTER; // {guidFilterThis} condition for translating this into me in VB.
	};
	typedef Functor<BSTR, EEDomain::fTranslateIdentifier&> TranslateIdentifierUseCase;

	struct fTranslateBindOperator {

		IDebugField* pField;
		IDebugObject* pObject;
		IDebugBinder* pBinder;

		// JSEE-specific
		IDebugField* pChildField;
		IDebugObject* pChildObject;
	};
	typedef Functor<BSTR, EEDomain::fTranslateBindOperator&> TranslateBindOperatorUseCase;

	struct fTranslateArrayElement {

		IDebugObject* pParentObject; // denotes the object of array, not array element
		LONG IndexID;
		UINT RADIX;
	};
	typedef Functor<BSTR, EEDomain::fTranslateArrayElement&> TranslateArrayElementUseCase;

	struct fTranslateArrayType {

		LPCOLESTR szUntranslatedArrayType;
	};
	typedef Functor<BSTR, EEDomain::fTranslateArrayType&> TranslateArrayTypeUseCase;

	struct fFormatMethodLocation {
	// [in parameters]
		CEE* pCEE;
		IDebugSymbolProvider* pSymbolProvider;
		IDebugAddress* pAddress;
		LPCOLESTR szMethodLocation;
	// [out parameters]
		BSTR bstrMethod;
		BSTR bstrArguments;
		UINT NoOfArguments;
		UINT64 Offset;
		NAME_MATCH NameMatch;
		GUID* pLangGuid;
		BOOL bDisambiguate;
	};

	struct fGetThisField {

		IDebugMethodField *pMethodField;

		IDebugSymbolProvider *pSymbolProvider;
		IDebugAddress *pAddress;
		IDebugBinder *pBinder;
	};
	typedef Functor<IDebugClassField*, EEDomain::fGetThisField&> GetThisFieldUseCase;

	struct fFilterPropertyChildren {

		// parent field/object/property info
		IDebugField* pParentField; // parent field enumerating its children
		IDebugObject* pParentObject; // parent object enumerating its children
		CProperty* pCParentProperty; // parent property

		// flags used to specify the kind of enumeration
		DEBUGPROP_INFO_FLAGS FLAGS;
		UINT RADIX;
		const GUID* pGuidFILTER;
		DBG_ATTRIB_FLAGS AttribFILTER;
		LPCOLESTR szNameFILTER;
		EVALFLAGS EvalFlags;

		// binder/symbol provider/expression context info
		IDebugSymbolProvider* pSymbolProvider;
		IDebugAddress* pAddress;
		IDebugBinder* pBinder;

		// resultant enumeration
		IEnumDebugPropertyInfo2* pEnumPropertyInfo; // chidren enumerated

		CEE* pCEE;
	};
	typedef Functor<IEnumDebugPropertyInfo2*, EEDomain::fFilterPropertyChildren&> FilterPropertyChildrenUseCase;

	struct fFormatErrorMessage {

		UINT StringResID;
		LPOLESTR* ArrayOfNames;
		WORD ArraySize;
	};
	typedef Functor<BSTR, EEDomain::fFormatErrorMessage&> FormatErrorMessageUseCase;

	struct fMakeProperty {

		PropertyContext* pPropertyContext;
		IDebugField* pField;
		IDebugObject* pObject;
		LONG ID;
	};

	struct fMakeEnumPropertyInfo {

		PropertyContext* pPropertyContext;
		DEBUGPROP_INFO_FLAGS FLAGS;
		UINT RADIX;
		IEnumDebugFields* pEnumFields;
		IEnumDebugObjects* pEnumObjects;
	};

	struct fObtainPropertyName {

		IDebugSymbolProvider* pSymbolProvider;
		IDebugAddress* pAddress;
		IDebugBinder* pBinder;

		IDebugField* pField;
		IDebugObject* pObject;

		LONG ObjectID;
		DWORD RADIX;
		const GUID* pGuidFILTER;

		EEDomain::TranslateIdentifierUseCase* pfTranslateIdentifier;
		EEDomain::TranslatePrimitiveTypeUseCase* pfTranslatePrimitiveType;
		EEDomain::TranslateArrayElementUseCase* pfTranslateArrayElement;
		EEDomain::TranslateArrayTypeUseCase* pfTranslateArrayType;
		EEDomain::IsIntrinsicTypeUseCase* pfIsIntrinsicType;
		EEDomain::TranslateIntrinsicTypeUseCase* pfTranslateIntrinsicType;
	};

	typedef fObtainPropertyName fObtainPropertyType;

	struct fObtainPropertyFullName {

		PropertyContext* pPropertyContext;
		IDebugProperty2* pProperty;
	};

	struct fObtainPropertyValue {
		
		DWORD PropertyKind;
		IDebugBinder* pBinder;
		IDebugField* pField;
		IDebugObject* pObject;
		IDebugObject* pParentObject; // could be NULL
		UINT RADIX;
		EVALFLAGS EvalFlags;
		DWORD TimeOut;

		EEDomain::TranslatePrimitiveValueUseCase* pfTranslatePrimitiveValue;
		EEDomain::TranslatePrimitiveTypeUseCase* pfTranslatePrimitiveType;
		EEDomain::TranslateIdentifierUseCase* pfTranslateIdentifier;
		EEDomain::FormatErrorMessageUseCase* pfFormatErrorMessage;

		LPCOLESTR szTranslatedFullName;
		LPCOLESTR szTranslatedType;
		UINT64 MemoryOffset;

	// [in,out] parameter
		bool bErrorIsValue;
	};

	struct fObtainPropertyAttrib {

		// language-specific functor
		LPCOLESTR szPropertyType;
		LPCOLESTR szPropertyValue;
		EEDomain::TranslateAttribUseCase* pfTranslateAttrib;
		EEDomain::GetThisFieldUseCase* pfGetThisField;

		IDebugField* pField;
		IDebugSymbolProvider* pSymbolProvider;
		IDebugAddress* pAddress;
		GUID* pGuidFILTER;
		EVALFLAGS EvalFlags;
		DWORD TimeOut;

		PropertyContext* pPropertyContext;

		// parameters below can be NULL 
		IDebugObject* pObject;
		IDebugObject* pParentObject;
		IDebugBinder* pBinder;
	};

	struct fObtainPropertyFullNamePrefix {

		IDebugField* pField;
		const GUID* pGuidFILTER;
		EEDomain::TranslateIdentifierUseCase* pfTranslateIdentifier;
		EEDomain::TranslateBindOperatorUseCase* pfTranslateBindOperator;
	};

	struct fBindToObject {

		PropertyContext* pPropertyContext;
		IDebugField* pField;
		IDebugObject* pParentObject;
	};

	struct fComputeFullNamePrefix {

		IDebugProperty2* pParentProperty;
		FIELD_KIND FieldTypeKind;
		EEDomain::TranslateBindOperatorUseCase* pfTranslateBindOperator;
	};

	struct fParseExpression {
		
		CEE* pCEE; // expression evaluator that delegates task to the language-specific functor
		LPCOLESTR szExpression;
		PARSEFLAGS ParseFlags; //EvalFlags;
		UINT RADIX; // use this radix to parse intergral literal
		BSTR* pbstrError; // out-parameter
		UINT* pichError; // out-parameter
		IDebugParsedExpression** ppParsedExpression; // out-parameter
	};

	struct fBindToSymbol {

		IDebugContainerField* pScopeField;
		FIELD_MODIFIERS FieldModifiers;
		LPCOLESTR szSymbol;
		NAME_MATCH NameMatch;
	};
};

#endif __EE_DOMAIN__