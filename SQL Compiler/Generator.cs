// <copyright file="Generator.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator
{
    using System.IO;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.DataModelGenerator.Database;
    using GammaFour.VisualStudio;

    /// <summary>
    /// The data model generator for the service.
    /// </summary>
    public sealed class Generator : BaseCodeGeneratorWithSite
    {
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

            // This creates the schema unit from the schema.
            SchemaUnit schemaUnit = new SchemaUnit(dataModelSchema);

            // This performs the work of outputting the formatted code to a file.
            return schemaUnit.Generate();
        }
    }
}