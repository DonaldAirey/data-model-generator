// <copyright file="Generator.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Formatting;
    using Microsoft.CodeAnalysis.Formatting;
    using Microsoft.CodeAnalysis.Options;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// The data model generator for the service.
    /// </summary>
    [ComVisible(true)]
    [Guid("0DD4EA83-DF46-4546-A5DE-5BC0C327A70C")]
    public sealed class Generator : IVsSingleFileGenerator, IObjectWithSite
    {
        /// <summary>
        /// Gets the Site.
        /// </summary>
        public object Site
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default extension for the selected code generator.
        /// </summary>
        /// <param name="pbstrDefaultExtension">The suffix to be appended to the generated file.</param>
        /// <returns>0</returns>
        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            // The default extension for all modules generated.
            pbstrDefaultExtension = ".Designer.cs";

            // This indicates that the extension was handled.
            return 0;
        }

        /// <summary>
        /// Generate the code from the custom tool.
        /// </summary>
        /// <param name="wszInputFilePath">The name of the input file.</param>
        /// <param name="bstrInputFileContents">The contents of the input file.</param>
        /// <param name="wszDefaultNamespace">The namespace GammaFour.DataModelGenerator.Client the generated code.</param>
        /// <param name="rgbOutputFileContents">The generated code.</param>
        /// <param name="pcbOutput">The buffer size of the generated code.</param>
        /// <param name="pGenerateProgress">An indication of the tools progress.</param>
        /// <returns>0 indicates the tool handled the command.</returns>
        public int Generate(
            string wszInputFilePath,
            string bstrInputFileContents,
            string wszDefaultNamespace,
            IntPtr[] rgbOutputFileContents,
            out uint pcbOutput,
            IVsGeneratorProgress pGenerateProgress)
        {
            // Validate the 'bstrInputFileContents' argument.
            if (bstrInputFileContents == null)
            {
                throw new ArgumentNullException(bstrInputFileContents);
            }

            // This schema describes the data model that is to be generated.
            DataModelSchema dataModelSchema = new DataModelSchema(bstrInputFileContents, wszDefaultNamespace);

            // This creates the compilation unit from the schema.
            CompilationUnit compilationUnit = new CompilationUnit(dataModelSchema);

            // A workspace is needed in order to turn the compilation unit into code.
            AdhocWorkspace adhocWorkspace = new AdhocWorkspace();
            OptionSet options = adhocWorkspace.Options;
            options = options.WithChangedOption(CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine, false);
            adhocWorkspace.Options = options;

            // If a handler was provided for the generation of the code, then call it with an update.
            if (pGenerateProgress != null)
            {
                pGenerateProgress.Progress(50, 100);
            }

            // Format the compilation unit.
            SyntaxNode syntaxNode = Formatter.Format(compilationUnit.Syntax, adhocWorkspace);

            // If a handler was provided for the progress, then let it know that the task is complete.
            if (pGenerateProgress != null)
            {
                pGenerateProgress.Progress(75, 100);
            }

            // This performs the work of outputting the formatted code to a file.
            StringBuilder stringBuilder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(stringBuilder))
            {
                syntaxNode.WriteTo(stringWriter);
                byte[] generatedBuffer = Encoding.UTF8.GetBytes(stringWriter.ToString());
                rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(generatedBuffer.Length);
                Marshal.Copy(generatedBuffer, 0, rgbOutputFileContents[0], generatedBuffer.Length);
                pcbOutput = (uint)generatedBuffer.Length;
            }

            // If a handler was provided for the progress, then let it know that the task is complete.
            if (pGenerateProgress != null)
            {
                pGenerateProgress.Progress(100, 100);
            }

            // At this point the code generation was a success.
            return 0;
        }

        /// <summary>
        /// Gets the site.
        /// </summary>
        /// <param name="riid">The RI identifier.</param>
        /// <param name="ppvSite">A pointer to the site.</param>
        void IObjectWithSite.GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            IntPtr pUnk = Marshal.GetIUnknownForObject(this.Site);
            IntPtr intPointer = IntPtr.Zero;
            Marshal.QueryInterface(pUnk, ref riid, out intPointer);
            ppvSite = intPointer;
        }

        /// <summary>
        /// Sets the site.
        /// </summary>
        /// <param name="pUnkSite">A pointer to the site.</param>
        void IObjectWithSite.SetSite(object pUnkSite)
        {
            this.Site = pUnkSite;
        }
    }
}