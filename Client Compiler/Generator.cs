// <copyright file="Generator.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.ClientCompiler
{
    using System.IO;
    using System.Text;
    using GammaFour.DataModelGenerator.Client;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.VisualStudio;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Formatting;

    /// <summary>
    /// The data model generator for the service.
    /// </summary>
    public sealed class Generator : BaseCodeGeneratorWithSite
    {
        /// <summary>
        /// The method that does the actual work of generating code given the input file.
        /// </summary>
        /// <param name="inputFileContent">File contents as a string.</param>
        /// <returns>The generated code file as a byte-array.</returns>
        protected override byte[] GenerateCode(string inputFileContent)
        {
            // Extract the name of the target class from the input file name.
            string className = Path.GetFileNameWithoutExtension(this.InputFilePath);

            // This creates the compilation unit from the schema.
            // This schema describes the data model that is to be generated.
            XmlSchemaDocument xmlSchemaDocument = new XmlSchemaDocument(inputFileContent);

            // This creates the compilation unit from the schema.
            CompilationUnit compilationUnit = new CompilationUnit(xmlSchemaDocument, this.CustomToolNamespace);

            // A workspace is needed in order to turn the compilation unit into code.
            SyntaxNode syntaxNode = null;
            using (AdhocWorkspace adhocWorkspace = new AdhocWorkspace())
            {
                // Format the compilation unit.
                syntaxNode = Formatter.Format(compilationUnit.Syntax, adhocWorkspace);
            }

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