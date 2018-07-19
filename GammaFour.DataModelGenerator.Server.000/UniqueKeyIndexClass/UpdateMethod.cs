// <copyright file="UpdateMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.UniqueKeyIndexClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method add an record to a unique index.
    /// </summary>
    public class UpdateMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private UniqueConstraintSchema uniqueConstraintSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMethod"/> class.
        /// </summary>
        /// <param name="uniqueConstraintSchema">The unique constraint schema.</param>
        public UpdateMethod(UniqueConstraintSchema uniqueConstraintSchema)
        {
            // Initialize the object.
            this.uniqueConstraintSchema = uniqueConstraintSchema;
            this.Name = "Update";

            //        /// <summary>
            //        /// Updates a record in the index.
            //        /// </summary>
            //        /// <param name="oldCustomerKey">The previous <see cref="CustomerKey"/> that uniquely identifies the row.</param>
            //        /// <param name="newCustomerKey">The new <see cref="CustomerKey"/> that uniquely identifies the row.</param>
            //        internal void Update(CustomerKey oldCustomerKey, CustomerKey newCustomerKey)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithAttributeLists(this.AttributeLists)
                .WithModifiers(this.Modifiers)
                .WithParameterList(this.ParameterList)
                .WithBody(this.Body)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the data contract attribute syntax.
        /// </summary>
        private SyntaxList<AttributeListSyntax> AttributeLists
        {
            get
            {
                // This collects all the attributes.
                List<AttributeListSyntax> attributes = new List<AttributeListSyntax>();

                //        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ConfigurationKey")]
                attributes.Add(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("SuppressMessage"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SeparatedList<AttributeArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("Microsoft.Naming"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("CA2204:Literals should be spelled correctly"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal(this.uniqueConstraintSchema.Name)))
                                            .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("MessageId"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("Diagnostic message.")))
                                            .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("Justification")))
                                        }))))));

                // The collection of attributes.
                return SyntaxFactory.List<AttributeListSyntax>(attributes);
            }
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

                //                if (!this.IsWriterLockHeld)
                //                {
                //                      <ThrowLockException>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.PrefixUnaryExpression(
                            SyntaxKind.LogicalNotExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("IsWriterLockHeld"))),
                        SyntaxFactory.Block(
                            ThrowLockException.GetSyntax(this.uniqueConstraintSchema.Name + " index is not locked."))));

                // Keys with a single element are optimized so they don't need to use a compound key to access the dictionary.
                if (this.uniqueConstraintSchema.Columns.Count == 1)
                {
                    //            CustomerRow customerRow = this.dictionary[oldCustomerKey];
                    statements.Add(
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(this.uniqueConstraintSchema.Table.Name + "Row"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier(this.uniqueConstraintSchema.Table.CamelCaseName + "Row"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.ElementAccessExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName("dictionary")))
                                            .WithArgumentList(
                                                SyntaxFactory.BracketedArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.IdentifierName(
                                                                "old" + this.uniqueConstraintSchema.Columns[0].Name)))))))))));

                    //            this.dictionary.Remove(oldCustomerKey);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("dictionary")),
                                    SyntaxFactory.IdentifierName("Remove")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(
                                                "old" + this.uniqueConstraintSchema.Columns[0].Name)))))));

                    //            this.dictionary.Add(newCustomerKey, customerRow);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("dictionary")),
                                    SyntaxFactory.IdentifierName("Add")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName(
                                                    "new" + this.uniqueConstraintSchema.Columns[0].Name)),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName(
                                                    this.uniqueConstraintSchema.Table.CamelCaseName + "Row"))
                                        })))));
                }
                else
                {
                    // Constructing a compound key for the old record requires the old key elements.
                    List<ArgumentSyntax> oldArguments = new List<ArgumentSyntax>();
                    foreach (ColumnSchema columnSchema in this.uniqueConstraintSchema.Columns)
                    {
                        oldArguments.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName("old" + columnSchema.Name)));
                    }

                    //            ConfigurationKeySet configurationKeySet = new ConfigurationKeySet(configurationIdKey, sourceKey);
                    statements.Add(
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(this.uniqueConstraintSchema.Name + "Set"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier("old" + this.uniqueConstraintSchema.Name + "Set"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.ObjectCreationExpression(
                                                SyntaxFactory.IdentifierName(this.uniqueConstraintSchema.Name + "Set"))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(oldArguments)))))))));

                    //            ConfigurationRow configurationRow = this.dictionary[oldConfigurationKeySet];
                    statements.Add(
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(this.uniqueConstraintSchema.Table.Name + "Row"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier(this.uniqueConstraintSchema.Table.CamelCaseName + "Row"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.ElementAccessExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName("dictionary")))
                                            .WithArgumentList(
                                                SyntaxFactory.BracketedArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.IdentifierName("old" + this.uniqueConstraintSchema.Name + "Set")))))))))));

                    //            this.dictionary.Remove(oldConfigurationKeySet);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("dictionary")),
                                    SyntaxFactory.IdentifierName("Remove")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("old" + this.uniqueConstraintSchema.Name + "Set")))))));

                    // Constructing a compound key for the new record requires the new key elements.
                    List<ArgumentSyntax> newArguments = new List<ArgumentSyntax>();
                    foreach (ColumnSchema columnSchema in this.uniqueConstraintSchema.Columns)
                    {
                        newArguments.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName("new" + columnSchema.Name)));
                    }

                    //            this.dictionary.Add(new ConfigurationKeySet(newConfigurationIdKey, newSource), configurationRowKey);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("dictionary")),
                                    SyntaxFactory.IdentifierName("Add")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.ObjectCreationExpression(
                                                    SyntaxFactory.IdentifierName(this.uniqueConstraintSchema.Name + "Set"))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SeparatedList<ArgumentSyntax>(newArguments)))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName(this.uniqueConstraintSchema.Table.CamelCaseName + "Row"))
                                        })))));
                }

                // This is the syntax for the body of the method.
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
                //        /// Updates a record in the index.
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
                                                " Adds a record to the index.",
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

                // Add a comment for each of the old key parameters.
                foreach (ColumnSchema columnSchema in this.uniqueConstraintSchema.Columns)
                {
                    //        /// <param name="configurationId">The ConfigurationId key element.</param>
                    string description = "The old " + columnSchema.Name + " key element.";
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
                                                        " <param name=\"" + "old" + columnSchema.Name + "\">" + description + "</param>",
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        Environment.NewLine,
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList())
                                                }))))));
                }

                // Add a comment for each of the new key parameters.
                foreach (ColumnSchema columnSchema in this.uniqueConstraintSchema.Columns)
                {
                    //        /// <param name="configurationId">The ConfigurationId key element.</param>
                    string description = "The new " + columnSchema.Name + " key element.";
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
                                                        " <param name=\"" + "new" + columnSchema.Name + "\">" + description + "</param>",
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        Environment.NewLine,
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList())
                                                }))))));
                }

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
                // private
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.InternalKeyword)
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
                // string oldConfigurationIdKey, string oldSourceKey
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();
                foreach (ColumnSchema columnSchema in this.uniqueConstraintSchema.Columns)
                {
                    parameters.Add(
                            SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier("old" + columnSchema.Name))
                        .WithType(Conversions.FromType(columnSchema.Type)));
                }

                // string newConfigurationIdKey, string newSourceKey
                foreach (ColumnSchema columnSchema in this.uniqueConstraintSchema.Columns)
                {
                    parameters.Add(
                            SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier("new" + columnSchema.Name))
                        .WithType(Conversions.FromType(columnSchema.Type)));
                }

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }
    }
}