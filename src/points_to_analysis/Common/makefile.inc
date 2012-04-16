
#=========================================================================
# Make sure our output build output directory contains the all the COM+
# bits we need to build and that the needed bits are registered.
cortools:
!ifdef PASS0ONLY
!if "$(NO_REG_EE)" == ""
    regit $(URTTARGET)\mscoree.dll
    regit $(URTTARGET)\mscordbi.dll
!endif
!endif


