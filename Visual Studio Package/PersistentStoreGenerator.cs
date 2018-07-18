﻿// <copyright file="PersistentStoreGenerator.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudioPackage
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.DataModelGenerator.PersistentStore;
    using GammaFour.VisualStudio;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Formatting;
    using Microsoft.CodeAnalysis.Formatting;
    using Microsoft.CodeAnalysis.Options;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// The data model generator for the service.
    /// </summary>
    [ComVisible(true)]
    [Guid(PackageGuid)]
    [CodeGeneratorRegistration(typeof(PersistentStoreGenerator), nameof(PersistentStoreGenerator), "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(PersistentStoreGenerator), nameof(PersistentStoreGenerator), "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}", GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(PersistentStoreGenerator), nameof(PersistentStoreGenerator), "{694DD9B6-B865-4C5B-AD85-86356E9C88DC}", GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(PersistentStoreGenerator))]
    public sealed class PersistentStoreGenerator : BaseCodeGeneratorWithSite
    {
        /// <summary>
        /// The package identifier.
        /// </summary>
        private const string PackageGuid = "68493683-DE55-4991-8579-88E5FAE91DFB";

        /// <summary>
        /// The method that does the actual work of generating code given the input file
        /// </summary>
        /// <param name="inputFileContent">File contents as a string</param>
        /// <returns>The generated code file as a byte-array</returns>
        protected override byte[] GenerateCode(string inputFileContent)
        {
            // Extract the name of the target class from the input file name.
            string className = Path.GetFileNameWithoutExtension(this.InputFilePath);

            // This creates the compilation unit from the schema.
            // This schema describes the data model that is to be generated.
            DataModelSchema dataModelSchema = new DataModelSchema(inputFileContent, this.TargetNamespace);

            // This creates the compilation unit from the schema.
            CompilationUnit compilationUnit = new CompilationUnit(dataModelSchema);

            // A workspace is needed in order to turn the compilation unit into code.
            AdhocWorkspace adhocWorkspace = new AdhocWorkspace();
            OptionSet options = adhocWorkspace.Options;
            options = options.WithChangedOption(CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine, false);
            adhocWorkspace.Options = options;

            // Format the compilation unit.
            SyntaxNode syntaxNode = Formatter.Format(compilationUnit.Syntax, adhocWorkspace);

            // This performs the work of outputting the formatted code to a file.
            byte[] buffer = null;
            using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
            {
                syntaxNode.WriteTo(stringWriter);
                buffer = Encoding.UTF8.GetBytes(stringWriter.ToString());
            }

            return buffer;
        }
    }
}