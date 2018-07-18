// <copyright file="AssemblyInfo.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General information about the assembly.
[assembly: AssemblyTitle("GammaFour.DataModelGenerator.Common")]
[assembly: AssemblyDescription("A library classes common to the server and client generators.")]
[assembly: AssemblyCompany("Gamma Four, Inc.")]
[assembly: AssemblyProduct("GammaFour")]
[assembly: AssemblyCopyright("Copyright © 2016, Gamma Four, Inc.  All rights reserved.")]

// Indicates that this assembly is not compliant with the Common Language Specification (CLS).
[assembly: CLSCompliant(false)]

// Disables the accessibility of this assembly to COM.
[assembly: ComVisible(false)]

// Describes the default language used for the resources.
[assembly: NeutralResourcesLanguageAttribute("en-US")]

// Version information for this assembly.
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Suppress these FXCop issues.
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Has strong name.")]
