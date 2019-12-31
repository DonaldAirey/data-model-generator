// <copyright file="ClientDataModelGenerator.cs" company="Gamma Four, Inc.">
//    Copyright © 2019 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudioPackage
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using GammaFour.DataModelGenerator.Client;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.VisualStudio;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Formatting;
    using Microsoft.CodeAnalysis.Formatting;
    using Microsoft.CodeAnalysis.Options;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// The data model generator for the service.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None)]
    [Guid(PackageGuid)]
    [CodeGeneratorRegistration(typeof(ClientDataModelGenerator), nameof(ClientDataModelGenerator), "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(ClientDataModelGenerator), nameof(ClientDataModelGenerator), "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}", GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(ClientDataModelGenerator), nameof(ClientDataModelGenerator), "{694DD9B6-B865-4C5B-AD85-86356E9C88DC}", GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(ClientDataModelGenerator))]
    public sealed class ClientDataModelGenerator : BaseCodeGeneratorWithSite
    {
        /// <summary>
        /// The package identifier.
        /// </summary>
        private const string PackageGuid = "C76BD71A-E000-44D1-B771-E8439DD41F9B";

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
                OptionSet options = adhocWorkspace.Options;
                options = options.WithChangedOption(CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine, false);
                adhocWorkspace.Options = options;

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