// <copyright file="DeleteOneMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
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
    /// Creates a method to delete a row from the set.
    /// </summary>
    public class DeleteOneMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteOneMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public DeleteOneMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "Delete";

            //        /// <summary>
            //        /// Deletes a <see cref="Order"/> row.
            //        /// </summary>
            //        /// <param name="order">The <see cref="Order"/> row.</param>
            //        public async Task<Thing> Delete(System.Guid thingId, Thing thing)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("Task"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                            SyntaxFactory.IdentifierName(this.tableElement.Name)))),
                SyntaxFactory.Identifier(this.Name))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
                    }))
            .WithParameterList(this.Parameters)
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
                var statements = new List<StatementSyntax>
                {
                    //                using var lockingTransaction = new LockingTransaction(this.transactionTimeout);
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
                                    SyntaxFactory.Identifier("lockingTransaction"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("LockingTransaction"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName("timeout")))))))))))
                    .WithUsingKeyword(
                        SyntaxFactory.Token(SyntaxKind.UsingKeyword)),

                    //            await lockingTransaction.WaitWriterAsync(this).ConfigureAwait(false);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("lockingTransaction"),
                                            SyntaxFactory.IdentifierName("WaitWriterAsync")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.ThisExpression())))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))),
                };

                // Find the row and delete it.
                statements.AddRange(
                    new StatementSyntax[]
                    {
                        //            if (this.dictionary.TryGetValue(thingId, out var existingRow))
                        //            {
                        //                <DeleteRow>
                        //            }
                        //            {
                        //                return thing;
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
                                            this.tableElement.PrimaryIndex.GetKeyAsArguments(),
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
                                                        SyntaxFactory.Identifier("existingRow"))))
                                            .WithRefOrOutKeyword(
                                                SyntaxFactory.Token(SyntaxKind.OutKeyword)),
                                        }))),
                            SyntaxFactory.Block(this.DeleteRow))
                        .WithElse(
                            SyntaxFactory.ElseClause(
                                SyntaxFactory.Block(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.AssignmentExpression(
                                            SyntaxKind.SimpleAssignmentExpression,
                                            SyntaxFactory.IdentifierName("existingRow"),
                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName())))))),

                        //                    await this.dataModelContext.SaveChangesAsync().ConfigureAwait(false);
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AwaitExpression(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName($"{this.tableElement.Document.Name.ToCamelCase()}Context")),
                                                SyntaxFactory.IdentifierName("SaveChangesAsync"))),
                                        SyntaxFactory.IdentifierName("ConfigureAwait")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.FalseLiteralExpression))))))),

                        //                    lockingTransaction.Complete();
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("lockingTransaction"),
                                    SyntaxFactory.IdentifierName("Complete")))),

                        //                    return (new Thing(existingThing));
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.ParenthesizedExpression(
                                SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName(this.tableElement.Name))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName("existingRow"))))))),
                    });

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the statements that deletes a row.
        /// </summary>
        /// <returns>The statements to delete a row.</returns>
        private IEnumerable<StatementSyntax> DeleteRow
        {
            get
            {
                // Lock the row and perform optimistic concurrency check.
                var statements = new List<StatementSyntax>
                {
                        //                await lockingTransaction.WaitWriterAsync(existingRow).ConfigureAwait(false);
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AwaitExpression(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("lockingTransaction"),
                                                SyntaxFactory.IdentifierName("WaitWriterAsync")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("existingRow"))))),
                                        SyntaxFactory.IdentifierName("ConfigureAwait")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.FalseLiteralExpression))))))),

                        //                if (existingRow.RowVersion != thing.RowVersion)
                        //                {
                        //                    throw new DBConcurrencyException();
                        //                }
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("existingRow"),
                                    SyntaxFactory.IdentifierName("RowVersion")),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()),
                                    SyntaxFactory.IdentifierName("RowVersion"))),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ThrowStatement(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("DBConcurrencyException"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList()))))),
                };

                // Enforce referential integrity constraints.
                foreach (ForeignIndexElement foreignIndexElement in this.tableElement.ChildKeys)
                {
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("existingRow"),
                                        SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                    SyntaxFactory.IdentifierName("Any"))),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ThrowStatement(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("ConstraintException"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal(
                                                                $"The delete action conflicted with the constraint {foreignIndexElement.Name}")))))))))));
                }

                // Delete this row from each of the parent rows.
                foreach (ForeignIndexElement foreignIndexElement in this.tableElement.ParentKeys)
                {
                    var condition = foreignIndexElement.GetKeyAsInequalityConditional(this.tableElement.Name.ToVariableName());
                    if (condition == null)
                    {
                        statements.AddRange(this.DeleteFromParent(foreignIndexElement));
                    }
                    else
                    {
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                condition,
                                SyntaxFactory.Block(this.DeleteFromParent(foreignIndexElement))));
                    }
                }

                // Delete the row from the table.
                statements.AddRange(
                    new StatementSyntax[]
                    {
                        //            this.dictionary.Remove((order.AccountCode, order.AssetCode, order.Date));
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
                                        this.tableElement.PrimaryIndex.GetKeyAsArguments("existingRow"))))),

                        //            this.rollbackStack.Push(() => this.dictionary.Add((order.AccountCode, order.AssetCode, order.Date), order));
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("rollbackStack")),
                                    SyntaxFactory.IdentifierName("Push")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.ParenthesizedLambdaExpression()
                                            .WithExpressionBody(
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
                                                                this.tableElement.PrimaryIndex.GetKeyAsArguments("existingRow"),
                                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.IdentifierName("existingRow")),
                                                            }))))))))),

                        //            existingRow.RowVersion = this.DataModel.IncrementRowVersion();
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("existingRow"),
                                    SyntaxFactory.IdentifierName("RowVersion")),
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(this.tableElement.Document.Name.ToCamelCase())),
                                        SyntaxFactory.IdentifierName("IncrementRowVersion"))))),

                        //                this.commitStack.Push(() => this.DeletedRows.AddFirst(allocation));
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("commitStack")),
                                    SyntaxFactory.IdentifierName("Push")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.ParenthesizedLambdaExpression()
                                            .WithExpressionBody(
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName("DeletedRows")),
                                                        SyntaxFactory.IdentifierName("AddFirst")))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName("existingRow"))))))))))),

                        //            this.commitStack.Push(() => this.RowChanged?.Invoke(this, new RowChangedEventArgs<Account>(DataAction.Add, account)));
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("commitStack")),
                                    SyntaxFactory.IdentifierName("Push")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.ParenthesizedLambdaExpression()
                                            .WithExpressionBody(
                                                SyntaxFactory.ConditionalAccessExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName("RowChanged")),
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberBindingExpression(
                                                            SyntaxFactory.IdentifierName("Invoke")))
                                                    .WithArgumentList(
                                                        SyntaxFactory.ArgumentList(
                                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                                new SyntaxNodeOrToken[]
                                                                {
                                                                    SyntaxFactory.Argument(
                                                                        SyntaxFactory.ThisExpression()),
                                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                    SyntaxFactory.Argument(
                                                                        SyntaxFactory.ObjectCreationExpression(
                                                                            SyntaxFactory.GenericName(
                                                                                SyntaxFactory.Identifier("RowChangedEventArgs"))
                                                                            .WithTypeArgumentList(
                                                                                SyntaxFactory.TypeArgumentList(
                                                                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                                        SyntaxFactory.IdentifierName(this.tableElement.Name)))))
                                                                        .WithArgumentList(
                                                                            SyntaxFactory.ArgumentList(
                                                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                                                    new SyntaxNodeOrToken[]
                                                                                    {
                                                                                        SyntaxFactory.Argument(
                                                                                            SyntaxFactory.MemberAccessExpression(
                                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                                SyntaxFactory.IdentifierName("DataAction"),
                                                                                                SyntaxFactory.IdentifierName("Delete"))),
                                                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                                        SyntaxFactory.Argument(
                                                                                            SyntaxFactory.IdentifierName("existingRow")),
                                                                                    })))),
                                                                })))))))))),

                        //                this.dataModelContext.Things.Remove(existingRow);
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(
                                                $"{this.tableElement.Document.Name.ToVariableName()}Context")),
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName("Remove")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("existingRow")))))),
                    });

                return statements;
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
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <summary>
                    //        /// Adds a <see cref="Order"/> row to the table.
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
                                                $" Deletes a <see cref=\"{this.tableElement.Name}\"/> row.",
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
                };

                //        /// <param name="accountId">The AccountId key.</param>
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryIndex.Columns)
                {
                    ColumnElement columnElement = columnReferenceElement.Column;
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
                                                    $" <param name=\"{columnElement.Name.ToCamelCase()}\">The {columnElement.Name} key.</param>",
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                                SyntaxFactory.XmlTextNewLine(
                                                    SyntaxFactory.TriviaList(),
                                                    Environment.NewLine,
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                            }))))));
                }

                //        /// <param name="order">The <see cref="Order"/> row.</param>
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
                                                        $" <param name=\"{this.tableElement.Name.ToCamelCase()}\">The <see cref=\"{this.tableElement.Name}\"/> row.</param>",
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
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters.
                List<SyntaxNodeOrToken> parameters = new List<SyntaxNodeOrToken>();

                // Add the key elements to the parameter list.
                parameters.AddRange(this.tableElement.PrimaryIndex.GetKeyAsParameters());
                parameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));

                // Thing thing
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier(this.tableElement.Name.ToVariableName()))
                    .WithType(
                        SyntaxFactory.IdentifierName(this.tableElement.Name)));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Deletes the row from the parent row.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <returns>The statements to delete a row from the parent row.</returns>
        private IEnumerable<StatementSyntax> DeleteFromParent(ForeignIndexElement foreignIndexElement)
        {
            var statements = new List<StatementSyntax>();

            //            var account = this.dataModel.Accounts.Find(order.AccountCode);
            var uniqueIndexElement = foreignIndexElement.UniqueIndex;
            statements.Add(
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
                                SyntaxFactory.Identifier(foreignIndexElement.UniqueParentName.ToVariableName()))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Document.Name.ToCamelCase())),
                                                SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName("Find")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                foreignIndexElement.GetKeyAsArguments(
                                                    this.tableElement.Name.ToVariableName()))))))))));

            //            ArgumentNullException.ThrowIfNull(account);
            statements.Add(
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
                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueParentName.ToVariableName())))))));

            //                account.Orders.Remove(order);
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(foreignIndexElement.UniqueParentName.ToVariableName()),
                                SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                            SyntaxFactory.IdentifierName("Remove")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName())))))));

            //            this.rollbackStack.Push(() => account.Orders.Add(order));
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("rollbackStack")),
                            SyntaxFactory.IdentifierName("Push")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ParenthesizedLambdaExpression()
                                    .WithExpressionBody(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueParentName.ToVariableName()),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Add")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()))))))))))));

            //            order.Account = null;
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()),
                            SyntaxFactory.IdentifierName(foreignIndexElement.UniqueParentName)),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression))));

            return statements;
        }
    }
}