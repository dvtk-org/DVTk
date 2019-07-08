// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>

// CrtWrapper.cpp

// This code verifies that DllMain is not called by the Loader
#pragma once

#include "stdafx.h"
// automatically when linked with /noentry. It also checks some
// functions that the CRT initializes.

#include <windows.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <math.h>
#include "_vcclrit.h"  //MIGRATION_IN_PROGRESS

#using <mscorlib.dll>
using namespace System;

public __gc class CrtWrapper {
public:
   static int minitialize() {
      int retval = 0;
      try {
            __crt_dll_initialize();  //MIGRATION_IN_PROGRESS
      } catch(System::Exception* e) {
         Console::WriteLine(e);
         retval = 1;
      }
      return retval;
   }
   static int mterminate() {
      int retval = 0;
      try {
            __crt_dll_terminate();   //MIGRATION_IN_PROGRESS
      } catch(System::Exception* e) {
         Console::WriteLine(e);
         retval = 1;
      }
      return retval;
   }
};

//BOOL WINAPI DllMain(
//                    HINSTANCE hModule,
//                    DWORD dwReason,
//                    LPVOID lpvReserved)
//{
//   Console::WriteLine(S"DllMain is called...");
//   return TRUE;
//};/* DllMain */
