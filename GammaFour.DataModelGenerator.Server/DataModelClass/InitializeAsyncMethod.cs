// <copyright file="InitializeAsyncMethod.cs" company="Gamma Four, Inc.">
//    Copyright � 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.DataModelClass
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
    public class InitializeAsyncMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeAsyncMethod"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The table schema.</param>
        public InitializeAsyncMethod(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.Name = "InitializeAsync";

            //        /// <inheritdoc/>
            //        public async Task StartAsync(CancellationToken cancellationToken)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.IdentifierName("Task"),
                SyntaxFactory.Identifier(this.Name))
            .WithModifiers(InitializeAsyncMethod.Modifiers)
            .WithBody(this.Body)
            .WithLeadingTrivia(InitializeAsyncMethod.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <inheritdoc/>
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
                                                " <inheritdoc/>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                        }))))),
                };

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
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
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

                //                Dictionary<ITable, IEnumerable<object>> bucket = new Dictionary<ITable, IEnumerable<object>>();
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("Dictionary"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SeparatedList<TypeSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.IdentifierName("ITable"),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.GenericName(
                                                SyntaxFactory.Identifier("IEnumerable"))
                                            .WithTypeArgumentList(
                                                SyntaxFactory.TypeArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                        SyntaxFactory.PredefinedType(
                                                            SyntaxFactory.Token(SyntaxKind.ObjectKeyword))))),
                                        }))))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("bucket"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.GenericName(
                                                SyntaxFactory.Identifier("Dictionary"))
                                            .WithTypeArgumentList(
                                                SyntaxFactory.TypeArgumentList(
                                                    SyntaxFactory.SeparatedList<TypeSyntax>(
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            SyntaxFactory.IdentifierName("ITable"),
                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                            SyntaxFactory.GenericName(
                                                                SyntaxFactory.Identifier("IEnumerable"))
                                                            .WithTypeArgumentList(
                                                                SyntaxFactory.TypeArgumentList(
                                                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                        SyntaxFactory.PredefinedType(
                                                                            SyntaxFactory.Token(SyntaxKind.ObjectKeyword))))),
                                                        }))))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList())))))));

                // Merge each of the tables.
                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    //            bucket.Add(this.Accounts, this.Accounts.Merge(await this.dataModelContext.Accounts.ToListAsync()));
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("bucket"),
                                    SyntaxFactory.IdentifierName("Add")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural()))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                                        SyntaxFactory.IdentifierName("Merge")))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.AwaitExpression(
                                                                    SyntaxFactory.InvocationExpression(
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                SyntaxFactory.MemberAccessExpression(
                                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                                    SyntaxFactory.ThisExpression(),
                                                                                    SyntaxFactory.IdentifierName($"{this.xmlSchemaDocument.Name.ToCamelCase()}Context")),
                                                                                SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                                                            SyntaxFactory.IdentifierName("ToListAsync"))))))))),
                                        })))));
                }

                //                while (bucket.Values.Where(v => v.Any()).Any())
                //                {
                //                    <MergeTables>
                //                }
                statements.Add(
                    SyntaxFactory.WhileStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("bucket"),
                                            SyntaxFactory.IdentifierName("Values")),
                                        SyntaxFactory.IdentifierName("Where")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.SimpleLambdaExpression(
                                                    SyntaxFactory.Parameter(
                                                        SyntaxFactory.Identifier("v")),
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("v"),
                                                            SyntaxFactory.IdentifierName("Any")))))))),
                                SyntaxFactory.IdentifierName("Any"))),
                        SyntaxFactory.Block(this.MergeTables)));

                // Merge each of the tables.
                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    //            foreach (var account in this.dataModelContext.Accounts)
                    //            {
                    //                account.Commit(null);
                    //            }
                    statements.Add(
                        SyntaxFactory.ForEachStatement(
                            SyntaxFactory.IdentifierName(
                                SyntaxFactory.Identifier(
                                    SyntaxFactory.TriviaList(),
                                    SyntaxKind.VarKeyword,
                                    "var",
                                    "var",
                                    SyntaxFactory.TriviaList())),
                            SyntaxFactory.Identifier(tableElement.Name.ToVariableName()),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName($"{this.xmlSchemaDocument.Name.ToVariableName()}Context")),
                                SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                                                SyntaxFactory.IdentifierName("Commit")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.NullLiteralExpression))))))))));
                }

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(statements);
            }
        }

        /// <summary>
        /// Gets the statements that load a dataModel from a DbContext.
        /// </summary>
        private List<StatementSyntax> MergeTables
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    //                    bucket[this.Buyers] = this.Buyers.Merge(bucket[this.Buyers]);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.ElementAccessExpression(
                                    SyntaxFactory.IdentifierName("bucket"))
                                .WithArgumentList(
                                    SyntaxFactory.BracketedArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))),
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                        SyntaxFactory.IdentifierName("Merge")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.ElementAccessExpression(
                                                    SyntaxFactory.IdentifierName("bucket"))
                                                .WithArgumentList(
                                                    SyntaxFactory.BracketedArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.ThisExpression(),
                                                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))))))))));
                }

                return statements;
            }
        }
    }
}