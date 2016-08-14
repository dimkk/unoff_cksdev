﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

#if VS2012Build_SYMBOL
[assembly: AssemblyTitle("CKS.Dev11.Cmd.Imp.v4")]
[assembly: AssemblyDescription("The SharePoint Visual Studio 2012 extensions.")]
#elif VS2013Build_SYMBOL
    [assembly: AssemblyTitle("CKS.Dev12.Cmd.Imp.v4")]
    [assembly: AssemblyDescription("The SharePoint Visual Studio 2013 extensions.")]
#elif VS2014Build_SYMBOL
    [assembly: AssemblyTitle("CKS.Dev13.Cmd.Imp.v4")]
    [assembly: AssemblyDescription("The SharePoint Visual Studio 2014 extensions.")]
#else
    [assembly: AssemblyTitle("CKS.Dev.Cmd.Imp.v4")]
    [assembly: AssemblyDescription("The SharePoint Visual Studio 2010 extensions.")]
#endif

[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("CKS Dev Team")]
[assembly: AssemblyProduct("Community Kit for SharePoint: Development Edition.")]
[assembly: AssemblyCopyright("Copyright © CKS Dev Team 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ff06eb67-d6b6-4e8f-be1d-e817790d705c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
