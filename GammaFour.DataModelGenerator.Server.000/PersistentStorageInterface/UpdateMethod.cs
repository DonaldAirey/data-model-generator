// <copyright file="UpdateMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.PersistentStoreageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to start editing.
    /// </summary>
    public class UpdateMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableSchema tableSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMethod"/> class.
        /// </summary>
        /// <param name="tableSchema">The unique constraint schema.</param>
        public UpdateMethod(TableSchema tableSchema)
        {
            // Initialize the object.
            this.tableSchema = tableSchema;
            this.Name = "Update" + tableSchema.Name;

            //        /// <summary>
            //        /// Creates a Configuration record.
            //        /// </summary>
            //        /// <param name="configurationId">The required value for the ConfigurationId column.</param>
            //        /// <param name="sourceKey">The required value for the SourceKey column.</param>
            //        /// <param name="targetKey">The required value for the TargetKey column.</param>
            //        void CreateConfiguration(string configurationId, long rowVersion, string source, string targetKey);
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                SyntaxFactory.Identifier(this.Name))
                .WithParameterList(this.ParameterList)
                .WithSemicolonToken(
                    SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <summary>
                //        /// Updates a Configuration record.
                //        /// </summary>
                comments.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                SyntaxFactory.XmlText()
                                .WithTextTokens(
                                    SyntaxFactory.TokenList(
                                        new[]
                                        {
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")),
                                                " <summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("         ///")),
                                                " Updates a " + this.tableSchema.Name + " record.",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("         ///")),
                                                " </summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList())
                                        }))))));

                // The trivia for the method header needs to sort the parameter comments in the same order they appear in the method declaration.
                List<KeyValuePair<string, SyntaxTrivia>> parameterTrivia = new List<KeyValuePair<string, SyntaxTrivia>>();

                // A parameter is needed for each element of the primary key.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    //        /// <param name="configurationIdKey">The ConfigurationId primary key element.</param>
                    string identifier = columnSchema.CamelCaseName + "Key";
                    string description = "The " + columnSchema.Name + " key element.";
                    parameterTrivia.Add(
                        new KeyValuePair<string, SyntaxTrivia>(
                            identifier,
                            SyntaxFactory.Trivia(
                                SyntaxFactory.DocumentationCommentTrivia(
                                    SyntaxKind.SingleLineDocumentationCommentTrivia,
                                        SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                            SyntaxFactory.XmlText()
                                            .WithTextTokens(
                                                SyntaxFactory.TokenList(
                                                    new[]
                                                    {
                                                        SyntaxFactory.XmlTextLiteral(
                                                            SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")),
                                                            " <param name=\"" + identifier + "\">" + description + "</param>",
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList()),
                                                        SyntaxFactory.XmlTextNewLine(
                                                            SyntaxFactory.TriviaList(),
                                                            Environment.NewLine,
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList())
                                                    })))))));
                }

                // A parameter is needed for each column in the table.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    //        /// <param name="configurationId">The optional value for the ConfigurationId column.</param>
                    string identifier = columnSchema.CamelCaseName;
                    string description = "The " + (columnSchema.IsNullable ? "optional" : "required") + " value for the " + columnSchema.Name + " column.";
                    parameterTrivia.Add(
                        new KeyValuePair<string, SyntaxTrivia>(
                            identifier,
                            SyntaxFactory.Trivia(
                                SyntaxFactory.DocumentationCommentTrivia(
                                    SyntaxKind.SingleLineDocumentationCommentTrivia,
                                        SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                            SyntaxFactory.XmlText()
                                            .WithTextTokens(
                                                SyntaxFactory.TokenList(
                                                    new[]
                                                    {
                                                        SyntaxFactory.XmlTextLiteral(
                                                            SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")),
                                                            " <param name=\"" + identifier + "\">" + description + "</param>",
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList()),
                                                        SyntaxFactory.XmlTextNewLine(
                                                            SyntaxFactory.TriviaList(),
                                                            Environment.NewLine,
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList())
                                                    })))))));
                }

                // Add the ordered parameter trivia to the method header.
                comments.AddRange(parameterTrivia.OrderBy(pt => pt.Key).Select((kp) => kp.Value));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax ParameterList
        {
            get
            {
                // Collect the parameters for the persistent store method.
                List<KeyValuePair<string, ParameterSyntax>> parameterPairs = new List<KeyValuePair<string, ParameterSyntax>>();

                // Add the primary key to the set of parameters.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    string identifier = columnSchema.CamelCaseName + "Key";
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            identifier,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(identifier))
                            .WithType(
                                Conversions.FromType(columnSchema.Type))));
                }

                // Add the columns of the table to the set of parameters.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    string identifier = columnSchema.CamelCaseName;
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            identifier,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(identifier))
                            .WithType(
                                Conversions.FromType(columnSchema.Type))));
                }

                // This is the complete set of comma separated parameters for the method.
                return SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(
                        parameterPairs.OrderBy(a => a.Key).Select((kp) => kp.Value)));
            }
        }
    }
}