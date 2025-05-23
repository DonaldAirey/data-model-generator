// <copyright file="PutOneMethod.cs" company="Gamma Four, Inc.">
//    Copyright � 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.TableClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to merge a row.
    /// </summary>
    public class PutOneMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutOneMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public PutOneMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "PutAsync";

            //        /// <summary>
            //        /// Patches a collection of <see cref="Thing"/> rows.
            //        /// </summary>
            //        /// <param name="things">The collection of <see cref="Thing"/> rows.</param>
            //        /// <returns>The patched <see cref="Thing"/> rows.</returns>
            //        public async Task<(Account? AddedRow, Account? UpdatedRow)> PutAsync(Account account)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("Task"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                            SyntaxFactory.TupleType(
                                SyntaxFactory.SeparatedList<TupleElementSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.TupleElement(
                                            SyntaxFactory.NullableType(
                                                SyntaxFactory.IdentifierName(this.tableElement.Name)))
                                        .WithIdentifier(
                                            SyntaxFactory.Identifier("AddedRow")),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.TupleElement(
                                            SyntaxFactory.NullableType(
                                                SyntaxFactory.IdentifierName(this.tableElement.Name)))
                                        .WithIdentifier(
                                            SyntaxFactory.Identifier("UpdatedRow")),
                                    }))))),
                SyntaxFactory.Identifier(this.Name))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
                    }))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList<ParameterSyntax>(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(this.tableElement.Name.ToVariableName()))
                        .WithType(
                            SyntaxFactory.IdentifierName(this.tableElement.Name)))))
            .WithBody(this.Body)
            .WithLeadingTrivia(this.LeadingTrivia);
        }

        /// <summary>
        /// Gets the statements to add a row.
        /// </summary>
        private IEnumerable<StatementSyntax> AddRow
        {
            get
            {
                var statements = new List<StatementSyntax>();
                statements.AddRange(RowUtilities.AddRow(this.tableElement));

                //                return (AddedRow: account, UpdatedRow: null);
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.TupleExpression(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()))
                                    .WithNameColon(
                                        SyntaxFactory.NameColon(
                                            SyntaxFactory.IdentifierName("AddedRow"))),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NullLiteralExpression))
                                    .WithNameColon(
                                        SyntaxFactory.NameColon(
                                            SyntaxFactory.IdentifierName("UpdatedRow"))),
                                }))));

                return statements;
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private BlockSyntax Body
        {
            get
            {
                return SyntaxFactory.Block(
                    new List<StatementSyntax>
                    {
                        //            var enlistmentState = this.EnlistmentState;
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(
                                    SyntaxFactory.Identifier(
                                        SyntaxFactory.TriviaList(),
                                        SyntaxKind.VarKeyword,
                                        "var",
                                        "var",
                                        SyntaxFactory.TriviaList())))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier("enlistmentState"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName("EnlistmentState"))))))),

                        //            ArgumentNullException.ThrowIfNull(enlistmentState);
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("ArgumentNullException"),
                                    SyntaxFactory.IdentifierName("ThrowIfNull")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("enlistmentState")))))),

                        //            if (this.dictionary.TryGetValue(Thing.ThingId, out var updatedRow))
                        //            {
                        //                <UpdateRow>
                        //            }
                        //            else
                        //            {
                        //                <AddRow>
                        //            }
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("dictionary")),
                                    SyntaxFactory.IdentifierName("TryGetValue")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            this.tableElement.PrimaryIndex.GetKeyAsArguments(
                                                this.tableElement.Name.ToVariableName()),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.DeclarationExpression(
                                                    SyntaxFactory.IdentifierName(
                                                        SyntaxFactory.Identifier(
                                                            SyntaxFactory.TriviaList(),
                                                            SyntaxKind.VarKeyword,
                                                            "var",
                                                            "var",
                                                            SyntaxFactory.TriviaList())),
                                                    SyntaxFactory.SingleVariableDesignation(
                                                        SyntaxFactory.Identifier("foundRow"))))
                                            .WithRefOrOutKeyword(
                                                SyntaxFactory.Token(SyntaxKind.OutKeyword)),
                                        }))),
                            SyntaxFactory.Block(this.UpdateRow))
                        .WithElse(
                            SyntaxFactory.ElseClause(
                                SyntaxFactory.Block(this.AddRow))),
                    });
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private IEnumerable<SyntaxTrivia> LeadingTrivia
        {
            get
            {
                return new List<SyntaxTrivia>
                {
                    //        /// <summary>
                    //        /// Puts a <see cref="Thing"/> row.
                    //        /// </summary>
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
                                                " <summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior(Strings.CommentExterior)),
                                                $" Puts a <see cref=\"{this.tableElement.Name}\"/> row.",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior(Strings.CommentExterior)),
                                                " </summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                        }))))),

                    //        /// <param name="thing">The <see cref="Thing"/> row.</param>
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
                                                $" <param name=\"{this.tableElement.Name.ToCamelCase()}\">The <see cref=\"{this.tableElement.Name}\"/> row.</param>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                        }))))),

                    //        /// <returns>The added or updated <see cref="Thing"/> row.</returns>
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
                                                $" <returns>The added or updated <see cref=\"{this.tableElement.Name}\"/> row.</returns>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                        }))))),
                };
            }
        }

        /// <summary>
        /// Gets the statements to update a row.
        /// </summary>
        private IEnumerable<StatementSyntax> UpdateRow
        {
            get
            {
                var statements = new List<StatementSyntax>();
                statements.AddRange(RowUtilities.UpdateRow(this.tableElement));

                //                return (AddedRow: null, UpdatedRow: foundRow);
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.TupleExpression(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NullLiteralExpression))
                                    .WithNameColon(
                                        SyntaxFactory.NameColon(
                                            SyntaxFactory.IdentifierName("AddedRow"))),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("foundRow"))
                                    .WithNameColon(
                                        SyntaxFactory.NameColon(
                                            SyntaxFactory.IdentifierName("UpdatedRow"))),
                                }))));

                return statements;
            }
        }
    }
}