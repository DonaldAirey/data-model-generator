﻿// <copyright file="ArgumentState.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.ModelCompiler
{
    /// <summary>
    /// These are the parsing states used to read the arguments on the command line.
    /// </summary>
    internal enum ArgumentState
    {
        /// <summary>
        /// The target namespace for the generated code.
        /// </summary>
        TargetNamespace,

        /// <summary>
        /// The input file name.
        /// </summary>
        InputFileName,

        /// <summary>
        /// The output file name.
        /// </summary>
        OutputFileName,
    }
}