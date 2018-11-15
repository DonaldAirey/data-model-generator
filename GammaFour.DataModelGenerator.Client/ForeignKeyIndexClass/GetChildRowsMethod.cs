// <copyright file="GetChildRowsMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.ForeignKeyIndexClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to start editing.
    /// </summary>
    public class GetChildRowsMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private ForeignKeyElement foreignKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChildRowsMethod"/> class.
        /// </summary>
        /// <param name="foreignKeyElement">The unique constraint schema.</param>
        public GetChildRowsMethod(ForeignKeyElement foreignKeyElement)
        {
            // Initialize the object.
            this.foreignKeyElement = foreignKeyElement;
            this.Name = "Get" + foreignKeyElement.Table.Name + "Rows";

            //        /// <summary>
            //        /// Gets a collection of child rows.
            //        /// </summary>
            //        /// <param name="countryKey">The key used to reference the child rows.</param>
            //        /// <returns>A collection of child rows.</returns>
            //        internal List<ProvinceRow> GetProvinceRows(CountryKey countryKey)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("List"))
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                SyntaxFactory.IdentifierName(this.foreignKeyElement.Table.Name + "Row")))),
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

                //            HashSet<ProvinceRow> hashSet;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("HashSet"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                        SyntaxFactory.IdentifierName(this.foreignKeyElement.Table.Name + "Row")))))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("hashSet"))))));

                // These are the arguments for the key lookup.
                List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                foreach (ColumnReferenceElement columnReferenceElement in this.foreignKeyElement.ParentColumns)
                {
                    ColumnElement columnElement = columnReferenceElement.Column;
                    arguments.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(columnElement.Name.ToCamelCase())));
                }

                // Compound keys are handled differently than simple keys.
                if (this.foreignKeyElement.ParentColumns.Count == 1)
                {
                    //            if (!this.dictionary.TryGetValue(countryId, out hashSet))
                    //            {
                    //                <NewHashSetBlock>
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.PrefixUnaryExpression(
                                SyntaxKind.LogicalNotExpression,
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
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.IdentifierName(
                                                        this.foreignKeyElement.UniqueKey.Columns[0].Column.Name.ToCamelCase())),
                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.IdentifierName("hashSet"))
                                                .WithRefOrOutKeyword(
                                                    SyntaxFactory.Token(SyntaxKind.OutKeyword))
                                            })))),
                            SyntaxFactory.Block(this.NewHashSetBlock)));
                }
                else
                {
                    //            if (this.dictionary.TryGetValue(new CustomerLastNameDateOfBirthKeySet(dateOfBirth, lastName), out hashSet))
                    //            {
                    //                <NewHashSetBlock>
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.PrefixUnaryExpression(
                                SyntaxKind.LogicalNotExpression,
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
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.ObjectCreationExpression(
                                                            SyntaxFactory.IdentifierName("CustomerLastNameDateOfBirthKeySet"))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("hashSet"))
                                                    .WithRefOrOutKeyword(
                                                        SyntaxFactory.Token(SyntaxKind.OutKeyword))
                                                })))),
                                    SyntaxFactory.Block(this.NewHashSetBlock)));
                }

                //            return hashSet.ToArray();
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("hashSet"),
                                SyntaxFactory.IdentifierName("ToList")))));

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
                //        /// Gets a collection of child <see cref="ProvinceRow"/> rows.
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
                                                " Gets a collection of child <see cref=\"" + this.foreignKeyElement.Table.Name + "Row" + "\"/> rows.",
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

                // Add a comment for each of the key parameters.
                foreach (ColumnReferenceElement columnReferenceElement in this.foreignKeyElement.ParentColumns)
                {
                    //        /// <param name="configurationId">The ConfigurationId key element.</param>
                    ColumnElement columnElement = columnReferenceElement.Column;
                    string description = "The " + columnElement.Name + " key element.";
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
                                                        " <param name=\"" + columnElement.Name.ToCamelCase() + "\">" + description + "</param>",
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        Environment.NewLine,
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList())
                                                }))))));
                }

                //        /// <returns>A collection of child <see cref="ProvinceRow"/> rows.</returns>
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
                                                    " <returns>A collection of child <see cref=\"" + this.foreignKeyElement.Table.Name + "Row" + "\"/> rows.</returns>",
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                                SyntaxFactory.XmlTextNewLine(
                                                    SyntaxFactory.TriviaList(),
                                                    Environment.NewLine,
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList())
                                            }))))));

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
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                   });
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> NewHashSetBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                hashSet = new HashSet<ProvinceRow>();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("hashSet"),
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("HashSet"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                            SyntaxFactory.IdentifierName(this.foreignKeyElement.Table.Name + "Row")))))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList()))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax ParameterList
        {
            get
            {
                // string keyConfigurationId, string keySource
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();
                foreach (ColumnReferenceElement columnReferenceElement in this.foreignKeyElement.ParentColumns)
                {
                    // Add the next element of the key.
                    ColumnElement columnElement = columnReferenceElement.Column;
                    parameters.Add(
                            SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(columnElement.Name.ToCamelCase()))
                        .WithType(Conversions.FromType(columnElement.Type)));
                }

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }
    }
}