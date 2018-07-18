// <copyright file="DeleteAsyncMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.DataServiceClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to delete a record asynchronously.
    /// </summary>
    public class DeleteAsyncMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableSchema tableSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAsyncMethod"/> class.
        /// </summary>
        /// <param name="tableSchema">The unique constraint schema.</param>
        public DeleteAsyncMethod(TableSchema tableSchema)
        {
            // Initialize the object.
            this.tableSchema = tableSchema;
            this.Name = "Delete" + tableSchema.Name + "Async";

            //        /// <summary>
            //        /// Asynchronously deletes a Configuration record.
            //        /// </summary>
            //        void DeleteConfiguration(ConfigurationKey configurationKey, long rowVersion);
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.IdentifierName("Task"),
                    SyntaxFactory.Identifier(this.Name))
                .WithModifiers(this.Modifiers)
                .WithParameterList(this.ParameterList)
                .WithBody(this.Body)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private BlockSyntax Body
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // Collect the arguments for the persistent store method.
                List<KeyValuePair<string, ArgumentSyntax>> argumentPair = new List<KeyValuePair<string, ArgumentSyntax>>();

                // A parameter is needed for each element of the primary key.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    string identifierName = columnSchema.CamelCaseName;
                    argumentPair.Add(
                        new KeyValuePair<string, ArgumentSyntax>(
                            identifierName,
                            SyntaxFactory.Argument(SyntaxFactory.IdentifierName(identifierName))));
                }

                // A parameter is needed for the row version which provides the optimistic concurrency check.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    if (columnSchema.IsRowVersion)
                    {
                        string identifierName = columnSchema.CamelCaseName;
                        argumentPair.Add(
                            new KeyValuePair<string, ArgumentSyntax>(
                                identifierName,
                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName(identifierName))));
                    }
                }

                // Sort and separate the arguments.
                List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                foreach (ArgumentSyntax argumentSyntax in argumentPair.OrderBy(a => a.Key).Select((kp) => kp.Value))
                {
                    arguments.Add(argumentSyntax);
                }

                //            return base.Channel.CreateConfiguration(configurationId, sourceKey, targetKey);
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.BaseExpression(),
                                    SyntaxFactory.IdentifierName("Channel")),
                                SyntaxFactory.IdentifierName("Delete" + this.tableSchema.Name + "Async")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));

                // This is the syntax for the body of the constructor.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
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
                //        /// Asynchronously deletes a Configuration record.
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
                                                " Asynchronously deletes a " + this.tableSchema.Name + " record.",
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
                    //        /// <param name="configurationId">The ConfigurationId primary key element.</param>
                    string identifier = columnSchema.CamelCaseName;
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

                // A parameter is needed for the row version for the concurrency check.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    if (columnSchema.IsRowVersion)
                    {
                        //        /// <param name="configurationId">The optional value for the ConfigurationId column.</param>
                        string identifier = columnSchema.CamelCaseName;
                        string description = columnSchema.IsRowVersion ?
                            "The required value for the " + columnSchema.CamelCaseName + " column." :
                            "The optional value for the " + columnSchema.CamelCaseName + " column.";
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
                }

                // Add the ordered parameter trivia to the method header.
                comments.AddRange(parameterTrivia.OrderBy(pt => pt.Key).Select((kp) => kp.Value));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private SyntaxTokenList Modifiers
        {
            get
            {
                // public
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                    });
            }
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax ParameterList
        {
            get
            {
                // Collect the parameters for this method.
                List<KeyValuePair<string, ParameterSyntax>> parameterPairs = new List<KeyValuePair<string, ParameterSyntax>>();

                // Add a parameter for each column in the primary key.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    string identifier = columnSchema.CamelCaseName;
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            identifier,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(identifier))
                            .WithType(Conversions.FromType(columnSchema.Type))));
                }

                // Add a row version for consistency checking.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    if (columnSchema.IsRowVersion)
                    {
                        string identifier = columnSchema.CamelCaseName;
                        parameterPairs.Add(
                            new KeyValuePair<string, ParameterSyntax>(
                                identifier,
                                SyntaxFactory.Parameter(
                                    SyntaxFactory.Identifier(identifier))
                                        .WithType(Conversions.FromType(columnSchema.Type))));
                    }
                }

                // string Guid countryIdKey, long rowVersion
                return SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(
                        parameterPairs.OrderBy(p => p.Key).Select((kp) => kp.Value)));
            }
        }
    }
}