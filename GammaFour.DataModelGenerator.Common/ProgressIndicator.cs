// <copyright file="ProgressIndicator.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Globalization;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Enables the single file generator to report on its progress and to provide additional warning and/or error information.
    /// </summary>
    public class ProgressIndicator : IVsGeneratorProgress
    {
        /// <summary>
        /// Returns warning and error information to the project system.
        /// </summary>
        /// <param name="fWarning">Flag that indicates whether this message is a warning or an error.</param>
        /// <param name="dwLevel">Severity level of the error.  Currently ignored.</param>
        /// <param name="bstrError">Text of the error to be displayed to the user by means of the Task List.</param>
        /// <param name="dwLine">Zero-based line number that indicates where in the source file the error occurred.</param>
        /// <param name="dwColumn">One-based column number that indicates where in the source file the error occurred.</param>
        /// <returns>If the method succeeds, it returns 0.  If it fails, it returns an error code.</returns>
        public int GeneratorError(int fWarning, uint dwLevel, string bstrError, uint dwLine, uint dwColumn)
        {
            // Dump the error to the console in a format compatible with Visual Studio.
            System.Diagnostics.Debug.WriteLine(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "({0},{1}): {2} {3}: {4}",
                    dwLine,
                    dwColumn,
                    fWarning == 0 ? "Error" : "Warning",
                    dwLevel,
                    bstrError));
            return 0;
        }

        /// <summary>
        /// Sets an index that specifies how much of the generation has been completed.
        /// </summary>
        /// <param name="nComplete">Index that specifies how much of the generation has been completed.</param>
        /// <param name="nTotal">The maximum value for nComplete.</param>
        /// <returns>If the method succeeds, it returns 0.  If it fails, it returns an error code.</returns>
        public int Progress(uint nComplete, uint nTotal)
        {
            // Dump the progress to the console as a percent.
            System.Diagnostics.Debug.WriteLine(
                string.Format(CultureInfo.InvariantCulture, "Progress: {0:0}%", Convert.ToDecimal(nComplete) * 100.0M / Convert.ToDecimal(nTotal)));
            return 0;
        }
    }
}