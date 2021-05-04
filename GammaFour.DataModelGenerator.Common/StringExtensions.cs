// <copyright file="StringExtensions.cs" company="Gamma Four, Inc.">
//    Copyright © 2021 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Pluralize.NET;

    /// <summary>
    /// String Extension Methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// C# Keywords.
        /// </summary>
        private static readonly HashSet<string> Keywords = new HashSet<string>()
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double",
            "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal",
            "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return",
            "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
            "unsafe", "ushort", "using", "virtual", "void", "volatile", "while",
        };

        /// <summary>
        /// Used to create plurals out of singles.
        /// </summary>
        private static readonly Pluralizer Pluralizer = new Pluralizer();

        /// <summary>
        /// Converts a string to have a lower case starting character.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <returns>The input string with a lower case starting letter.</returns>
        public static string ToCamelCase(this string text)
        {
            // Validate the parameter
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            // Convert the variable to its camel case equivalent.
            return text[0].ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture) + text.Remove(0, 1);
        }

        /// <summary>
        /// Converts a string to have a lower case starting character.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <returns>The input string with a lower case starting letter and an @ prepended if the variable is a keyword..</returns>
        public static string ToVariableName(this string text)
        {
            // Validate the parameter
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            // Convert the variable to its camel case equivalent.
            var name = text[0].ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture) + text.Remove(0, 1);
            return StringExtensions.Keywords.Contains(name) ? "@" + name : name;
        }

        /// <summary>
        /// Turns a singular noun into a plural noun.
        /// </summary>
        /// <param name="text">The text to be pluralized.</param>
        /// <returns>The pluralized text.</returns>
        public static string ToPlural(this string text)
        {
            return Pluralizer.Pluralize(text);
        }
    }
}