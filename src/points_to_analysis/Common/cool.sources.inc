# This file defines standard macros for use when building managed COM+
# components using the COOL compiler. It also contains definitions for COM+
# supplied tools.

# See cool.makefile.inc for a description of the overall structure of the
# build.


# Turn on flags for integration. Use the following prototoype:

# URTXXXX=0
# !if "$(URTXXXX)"=="1"
# COOLC_FLAGS=$(COOLC_FLAGS) /D:URTXXXX
# USER_C_DEFINES=$(USER_C_DEFINES) /DURTXXXX=1 
# !endif

# cool.makefile.inc makes sure this file was included by seeing if 
# COOL_SOURCES_INC is defined.
COOL_SOURCES_INC=1      


# Output directories
OBJDIR=$(_OBJ_DIR)\$(TARGET_DIRECTORY)
OUTDIR=$(VSBUILT)\$(BUILT_FLAVOR)\bin\$(TARGET_DIRECTORY)
INCDIR=$(VSBUILT)\$(BUILT_FLAVOR)\inc\$(TARGET_DIRECTORY)
TLBDIR=$(VSBUILT)\$(BUILT_FLAVOR)\tlb


# We now use the debug bits when building the debug versions and retail
# bits when building retail versions.
COREXEDIR=$(VSROOT)\external\bin\$(TARGET_DIRECTORY)\cor
CORDLLDIR=$(URTTARGET)


# COM+ tools needed for build
# trevors: TLBIMP_CMD points to tlbimp.exe.  For the 64-bit cross platform
# compile we need to force it to the 32-bit version of tlbimp.exe
# Note: This will NOT work for the special URT_COMPAT_BUILD switch
TLBIMP_CMD=$(URTSDKROOT)\bin\tlbimp.exe /asmversion:7.0.5000.0
!if "$(BUILD_64BIT)" != ""
TLBIMP_CMD=%VSROOT%\public\tools\setdevpath.bat & $(VSROOT)\src\common\buildrun\x86\complus\$(BUILT_FLAVOR)\sdk\bin\tlbimp.exe
!endif
TLBIMP_FLAGS=

# trevors: TLBIMP_CMD points to tlbimp.exe.  For the 64-bit cross platform
# compile we need to force it to the 32-bit version of tlbimp.exe
# Note: This will NOT work for the special URT_COMPAT_BUILD switch
TLBEXP_CMD=%VSROOT%\public\tools\setdevpath.bat & $(URTSDKROOT)\bin\tlbexp.exe
!if "$(BUILD_64BIT)" != ""
TLBEXP_CMD=%VSROOT%\public\tools\setdevpath.bat & $(VSROOT)\src\common\buildrun\x86\complus\$(BUILT_FLAVOR)\sdk\bin\tlbexp.exe
!endif
TLBEXP_FLAGS=


GACUTIL_CMD=%VSROOT%\public\tools\setdevpath.bat & $(URTTARGET)\gacutil.exe
!if "$(BUILD_64BIT)" != ""
GACUTIL_CMD=%VSROOT%\public\tools\setdevpath.bat & $(VSROOT)\src\common\buildrun\x86\complus\$(BUILT_FLAVOR)\gacutil.exe
!endif
GACUTIL_FLAGS=

REGASM_CMD=%VSROOT%\public\tools\setdevpath.bat & $(URTTARGET)\regasm.exe
!if "$(BUILD_64BIT)" != ""
REGASM_CMD=%VSROOT%\public\tools\setdevpath.bat & $(VSROOT)\src\common\buildrun\x86\complus\$(BUILT_FLAVOR)\regasm.exe
!endif
REGASM_FLAGS=


# trevors: RESGEN_CMD points to tlbimp.exe.  For the 64-bit cross platform
# compile we need to force it to the 32-bit version of tlbimp.exe
# Note: This will NOT work for the special URT_COMPAT_BUILD switch
RESGEN_CMD=%VSROOT%\public\tools\setdevpath.bat & $(URTSDKROOT)\bin\resgen.exe
!if "$(BUILD_64BIT)" != ""
RESGEN_CMD=%VSROOT%\public\tools\setdevpath.bat & $(VSROOT)\src\common\buildrun\x86\complus\$(BUILT_FLAVOR)\sdk\bin\resgen.exe
!endif
RESGEN_FLAGS=

# trevors: LM_CMD points to tlbimp.exe.  For the 64-bit cross platform
# compile we need to force it to the 32-bit version of tlbimp.exe
# Note: This will NOT work for the special URT_COMPAT_BUILD switch
LM_CMD=%VSROOT%\public\tools\setdevpath.bat & $(URTSDKROOT)\bin\lm.exe
!if "$(BUILD_64BIT)" != ""
LM_CMD=%VSROOT%\public\tools\setdevpath.bat & $(VSROOT)\src\common\buildrun\x86\complus\$(BUILT_FLAVOR)\sdk\bin\lm.exe
!endif
LM_FLAGS=

# trevors: LM_CMD points to tlbimp.exe.  For the 64-bit cross platform
# compile we need to force it to the 32-bit version of tlbimp.exe
# Note: This will NOT work for the special URT_COMPAT_BUILD switch
ALINK_CMD=$(COOLC_DIR)\al.exe
ALINK_FLAGS=

# Default settings for building COOL targets. We always build assemblies.
# We never use the COOL build rules in makefile.def. 
COOLC_BUILDASSEMBLY=1
COOLC_SKIPCOMMONCOMPILE=1
COOLC_NOT_DEBUGGABLE_AMBIVALENT=1

