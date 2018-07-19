// <copyright file="ReadStartingMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.DataModelClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to start editing.
    /// </summary>
    public class ReadStartingMethod : SyntaxElement
    {
        /// <summary>
        /// The data model schema.
        /// </summary>
        private DataModelSchema dataModelSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadStartingMethod"/> class.
        /// </summary>
        /// <param name="dataModelSchema">The data model schema.</param>
        public ReadStartingMethod(DataModelSchema dataModelSchema)
        {
            // Initialize the object.
            this.dataModelSchema = dataModelSchema;
            this.Name = "ReadStarting";

            //        /// <summary>
            //        /// Handles the start of the database load.
            //        /// </summary>
            //        private void OnLoadStarting()
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithModifiers(this.Modifiers)
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

                //            CommittableTransaction committableTransaction = new CommittableTransaction();
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("CommittableTransaction"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("committableTransaction"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("CommittableTransaction"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList())))))));

                //            System.Transactions.Transaction.Current = committableTransaction;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("System"),
                                        SyntaxFactory.IdentifierName("Transactions")),
                                    SyntaxFactory.IdentifierName("Transaction")),
                                SyntaxFactory.IdentifierName("Current")),
                            SyntaxFactory.IdentifierName("committableTransaction"))));

                // Initialize each of the tables.
                foreach (TableSchema tableSchema in this.dataModelSchema.Tables.OrderBy(t => t.Name))
                {
                    //            this.Country.AcquireWriterLock();
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(tableSchema.Name)),
                                    SyntaxFactory.IdentifierName("AcquireWriterLock")))));
                }

                // Initialize each of the (non-primary key) unique keys.
                foreach (TableSchema tableSchema in this.dataModelSchema.Tables.OrderBy(t => t.Name))
                {
                    foreach (UniqueConstraintSchema uniqueConstraintSchema in tableSchema.UniqueKeys.OrderBy(uk => uk.Name))
                    {
                        //            this.CountryKey.AcquireWriterLock();
                        statements.Add(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(uniqueConstraintSchema.Name)),
                                        SyntaxFactory.IdentifierName("AcquireWriterLock")))));
                    }
                }

                // This will initialize each of the foreign relations.
                foreach (RelationSchema relationSchema in this.dataModelSchema.Relations.OrderBy(r => r.ParentTable.Name))
                {
                    //            this.CountryCustomerCountryIdKeyIndex.AcquireWriterLock();
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(relationSchema.Name)),
                                    SyntaxFactory.IdentifierName("AcquireWriterLock")))));
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
                //        /// Handles the start of the database load.
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
                                                " Handles the start of the database load.",
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
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword)
                   });
            }
        }
    }
}