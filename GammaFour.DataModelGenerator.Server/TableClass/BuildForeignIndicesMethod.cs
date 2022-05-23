// <copyright file="BuildForeignIndicesMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.TableClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to acquire a reader lock.
    /// </summary>
    public class BuildForeignIndicesMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildForeignIndicesMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The table schema.</param>
        public BuildForeignIndicesMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "BuildForeignIndices";

            //        /// <inheritdoc/>
            //        public void BuildForeignIndices(Enlistment enlistment)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                SyntaxFactory.Identifier(this.Name))
            .WithModifiers(BuildForeignIndicesMethod.Modifiers)
            .WithBody(this.Body)
            .WithLeadingTrivia(BuildForeignIndicesMethod.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static SyntaxTriviaList DocumentationComment
        {
            get
            {
                // This is used to collect the trivia.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <inheritdoc/>
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
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior(Strings.CommentExterior)),
                                                " Builds the foreign indices for a table.",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                        }))))));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // private
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.InternalKeyword),
                    });
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private BlockSyntax Body
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // Initialize the foreign index properties.
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ParentKeys)
                {
                    //            this.CountryCodeKey = new UniqueKeyIndex<Country>("CountryCodeKey").HasIndex(c => c.Code);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                BuildForeignIndicesMethod.GetForeignKeyInitializer(foreignKeyElement))));
                }

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Constructs an initializer for a unique index.
        /// </summary>
        /// <param name="foreignKeyElement">The foreign index description.</param>
        /// <returns>Code to initialize a unique index.</returns>
        private static ExpressionSyntax GetForeignKeyInitializer(ForeignKeyElement foreignKeyElement)
        {
            //        new ForeignKeyIndex<Account,Item>("AccountSymbolKey")
            ExpressionSyntax expressionSyntax = SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("ForeignKeyIndex"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SeparatedList<TypeSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                    SyntaxFactory.IdentifierName(foreignKeyElement.Table.Name),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.IdentifierName(foreignKeyElement.UniqueKey.Table.Name),
                            }))))
            .WithArgumentList(
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                                SyntaxFactory.Argument(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        SyntaxFactory.Literal(foreignKeyElement.Name))),
                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                SyntaxFactory.Argument(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName(foreignKeyElement.XmlSchemaDocument.Name)),
                                            SyntaxFactory.IdentifierName(foreignKeyElement.UniqueKey.Table.Name.ToPlural())),
                                        SyntaxFactory.IdentifierName(foreignKeyElement.UniqueKey.Name))),
                        })));

            // .HasIndex(a => a.ItemId)
            expressionSyntax = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    expressionSyntax,
                    SyntaxFactory.IdentifierName("HasIndex")))
            .WithArgumentList(
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                        SyntaxFactory.Argument(
                            ForeignKeyExpression.GetForeignKey(foreignKeyElement)))));

            //  .HasFilter(a => a.Symbol != null)
            if (foreignKeyElement.IsNullable)
            {
                expressionSyntax = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        expressionSyntax,
                        SyntaxFactory.IdentifierName("HasFilter")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(NullableKeyFilterExpression.GetNullableKeyFilter(foreignKeyElement)))));
            }

            // = new UniqueKeyIndex<Security>("SecurityFigiKey").HasIndex(s => s.Figi).HasFilter(s => s.Figi != null);
            return expressionSyntax;
        }

        /// <summary>
        /// Constructs an initializer for a unique index.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique index description.</param>
        /// <returns>Code to initialize a unique index.</returns>
        private static ExpressionSyntax GetUniqueKeyInitializer(UniqueKeyElement uniqueKeyElement)
        {
            //        new ForeignKeyIndex<Account,Item>("AccountSymbolKey")
            ExpressionSyntax expressionSyntax = SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("UniqueKeyIndex"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                            SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name)))))
            .WithArgumentList(
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                        SyntaxFactory.Argument(
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                SyntaxFactory.Literal(uniqueKeyElement.Name))))));

            // .HasIndex(a => a.ItemId)
            expressionSyntax = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    expressionSyntax,
                    SyntaxFactory.IdentifierName("HasIndex")))
             .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                            SyntaxFactory.Argument(UniqueKeyExpression.GetUniqueKey(uniqueKeyElement)))));

            //  .HasFilter(a => a.Symbol != null)
            if (uniqueKeyElement.IsNullable)
            {
                expressionSyntax = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        expressionSyntax,
                        SyntaxFactory.IdentifierName("HasFilter")))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                            SyntaxFactory.Argument(NullableKeyFilterExpression.GetNullableKeyFilter(uniqueKeyElement)))));
            }

            return expressionSyntax;
        }
    }
}