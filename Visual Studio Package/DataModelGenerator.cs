﻿// <copyright file="DataModelGenerator.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudioPackage
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.DataModelGenerator.Model;
    using GammaFour.VisualStudio;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Formatting;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// The data model generator for the service.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None)]
    [Guid(PackageGuid)]
    [CodeGeneratorRegistration(typeof(DataModelGenerator), nameof(DataModelGenerator), "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(DataModelGenerator), nameof(DataModelGenerator), "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}", GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(DataModelGenerator), nameof(DataModelGenerator), "{694DD9B6-B865-4C5B-AD85-86356E9C88DC}", GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(DataModelGenerator))]
    public sealed class DataModelGenerator : BaseCodeGeneratorWithSite
    {
        /// <summary>
        /// The package identifier.
        /// </summary>
        private const string PackageGuid = "4E010E1F-FFAF-4A68-9015-C01FF3A68EBA";

        /// <summary>
        /// The method that does the actual work of generating code given the input file.
        /// </summary>
        /// <param name="inputFileContent">File contents as a string.</param>
        /// <returns>The generated code file as a byte-array.</returns>
        protected override byte[] GenerateCode(string inputFileContent)
        {
            byte[] buffer = Array.Empty<byte>();

            try
            {
                // Extract the name of the target class from the input file name.
                string className = Path.GetFileNameWithoutExtension(this.InputFilePath);

                // This creates the compilation unit from the schema.
                // This schema describes the data model that is to be generated.
                XmlSchemaDocument xmlSchemaDocument = new XmlSchemaDocument(inputFileContent);

                // This creates the compilation unit from the schema.
                CompilationUnit compilationUnit = new CompilationUnit(xmlSchemaDocument, this.CustomToolNamespace);

                // A workspace is needed in order to turn the compilation unit into code.
                AdhocWorkspace adhocWorkspace = new AdhocWorkspace();

                // Format the compilation unit.
                SyntaxNode syntaxNode = Formatter.Format(compilationUnit.Syntax, adhocWorkspace);

                // This performs the work of outputting the formatted code to a file.
                using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
                {
                    syntaxNode.WriteTo(stringWriter);
                    buffer = Encoding.UTF8.GetBytes(stringWriter.ToString());
                }
            }
            catch (Exception exception)
            {
                this.ErrorMessage(exception.Message);
            }

            return buffer;
        }
    }
}