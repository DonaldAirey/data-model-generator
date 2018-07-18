// <copyright file="DatabaseGenerator.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudioPackage
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.DataModelGenerator.Database;
    using GammaFour.VisualStudio;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// The data model generator for the service.
    /// </summary>
    [ComVisible(true)]
    [Guid(PackageGuid)]
    [CodeGeneratorRegistration(typeof(DatabaseGenerator), nameof(DatabaseGenerator), "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(DatabaseGenerator), nameof(DatabaseGenerator), "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}", GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(DatabaseGenerator), nameof(DatabaseGenerator), "{694DD9B6-B865-4C5B-AD85-86356E9C88DC}", GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(DatabaseGenerator))]
    public sealed class DatabaseGenerator : BaseCodeGeneratorWithSite
    {
        /// <summary>
        /// The package identifier.
        /// </summary>
        private const string PackageGuid = "EC3EAD72-D95A-414B-B7FB-EFE0CA80475A";

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
            SchemaUnit schemaUnit = new SchemaUnit(dataModelSchema);

            // This performs the work of outputting the formatted code to a file.
            return schemaUnit.Generate();
        }
    }
}