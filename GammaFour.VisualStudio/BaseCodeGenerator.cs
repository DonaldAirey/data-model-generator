// <copyright file="BaseCodeGenerator.cs" company="Dark Bond, Inc.">
//    Copyright © 2016-2017 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudio
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// A managed wrapper for an IVsSingleFileGenerator which is a custom tool invoked at design time which can take any file as an input and provide any file as output.
    /// </summary>
    [ComVisible(true)]
    public abstract class BaseCodeGenerator : IVsSingleFileGenerator
    {
        /// <summary>
        /// Used to display the progress of the tool.
        /// </summary>
        private IVsGeneratorProgress generateProgress;

        /// <summary>
        /// Gets the input file path.
        /// </summary>
        protected string InputFilePath { get; private set; }

        /// <summary>
        /// Gets the namespace for the generated module.
        /// </summary>
        protected string CustomToolNamespace { get; private set; }

        /// <summary>
        /// Retrieves the file extension that is given to the output file name.
        /// </summary>
        /// <param name="pbstrDefaultExtension">Returns the file extension that is to be given to the output file name. The returned extension must include a leading period.</param>
        /// <returns>S_OK if successful, E_FAIL if not</returns>
        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = this.GetDefaultExtension();
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Implements the IVsSingleFileGenerator.Generate method.
        /// Executes the transformation and returns the newly generated output file, whenever a custom tool is loaded, or the input file is saved
        /// </summary>
        /// <param name="wszInputFilePath">The full path of the input file. May be a null reference (Nothing in Visual Basic) in future releases of Visual Studio, so generators should not rely on this value</param>
        /// <param name="bstrInputFileContents">The contents of the input file. This is either a UNICODE BSTR (if the input file is text) or a binary BSTR (if the input file is binary). If the input file is a text file, the project system automatically converts the BSTR to UNICODE</param>
        /// <param name="wszDefaultNamespace">This parameter is meaningful only for custom tools that generate code. It represents the namespace into which the generated code will be placed. If the parameter is not a null reference (Nothing in Visual Basic) and not empty, the custom tool can use the following syntax to enclose the generated code</param>
        /// <param name="rgbOutputFileContents">[out] Returns an array of bytes to be written to the generated file. You must include UNICODE or UTF-8 signature bytes in the returned byte array, as this is a raw stream. The memory for rgbOutputFileContents must be allocated using the .NET Framework call, System.Runtime.InteropServices.AllocCoTaskMem, or the equivalent Win32 system call, CoTaskMemAlloc. The project system is responsible for freeing this memory</param>
        /// <param name="pcbOutput">[out] Returns the count of bytes in the rgbOutputFileContent array</param>
        /// <param name="pGenerateProgress">A reference to the IVsGeneratorProgress interface through which the generator can report its progress to the project system</param>
        /// <returns>If the method succeeds, it returns S_OK. If it fails, it returns E_FAIL</returns>
        public int Generate(
            string wszInputFilePath,
            string bstrInputFileContents,
            string wszDefaultNamespace,
            IntPtr[] rgbOutputFileContents,
            out uint pcbOutput,
            IVsGeneratorProgress pGenerateProgress)
        {
            // Validate the bstrInputFileContents argument.
            if (bstrInputFileContents == null)
            {
                throw new ArgumentNullException(nameof(bstrInputFileContents));
            }

            // Initialize the object.
            this.InputFilePath = wszInputFilePath ?? throw new ArgumentNullException(nameof(wszInputFilePath));
            this.CustomToolNamespace = wszDefaultNamespace ?? throw new ArgumentNullException(nameof(wszDefaultNamespace));
            this.generateProgress = pGenerateProgress;

            // Generate the file using the input contents.
            byte[] bytes = this.GenerateCode(bstrInputFileContents);
            if (bytes == null)
            {
                // This indicates that the file generation has failed.  Set the output parameters accordingly.
                rgbOutputFileContents = null;
                pcbOutput = 0;
                return VSConstants.E_FAIL;
            }

            // The contract between IVsSingleFileGenerator implementors and consumers is that the output from the code generation is returned through
            // memory allocated via CoTaskMemAlloc().  This will convert the bytes into an unmanaged blob.
            int outputLength = bytes.Length;
            rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(outputLength);
            Marshal.Copy(bytes, 0, rgbOutputFileContents[0], outputLength);
            pcbOutput = (uint)outputLength;

            // This indicates that we're done generating code and it was successful.
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Display an error message in the IDE.
        /// </summary>
        /// <param name="message">The error message.</param>
        protected void ErrorMessage(string message)
        {
            this.generateProgress.GeneratorError(0, 0, message, 0xFFFFFFFF, 0xFFFFFFFF);
        }

        /// <summary>
        /// Display an error message in the IDE.
        /// </summary>
        /// <param name="message">The error message.</param>
        protected void WarningMessage(string message)
        {
            this.generateProgress.GeneratorError(1, 0, message, 0xFFFFFFFF, 0xFFFFFFFF);
        }

        /// <summary>
        /// Gets the default extension for this generator
        /// </summary>
        /// <returns>String with the default extension for this generator</returns>
        protected virtual string GetDefaultExtension()
        {
            // The default extension for all modules generated.
            return ".Designer.cs";
        }

        /// <summary>
        /// The method that does the actual work of generating code given the input file
        /// </summary>
        /// <param name="inputFileContent">File contents as a string</param>
        /// <returns>The generated code file as a byte-array</returns>
        protected abstract byte[] GenerateCode(string inputFileContent);
    }
}