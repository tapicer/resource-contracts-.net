/**
@file Functor.h
@version 1.4
@author George Chu <gchu@microsoft.com>
@copyright Microsoft 1999
**/
#ifndef __FUNCTOR__
#define __FUNCTOR__

template <typename CODOMAIN, typename DOMAIN> struct Functor {

	virtual CODOMAIN operator ()(DOMAIN in_DomainVal) = 0;
};

#endif __FUNCTOR__