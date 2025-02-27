// <copyright file="DeleteManyMethod.cs" company="Gamma Four, Inc.">
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
    public class DeleteManyMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteManyMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public DeleteManyMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "Delete";

            //        /// <summary>
            //        /// Deletes a collection of<see cref="Allocation"/> rows.
            //        /// </summary>
            //        /// <param name="allocations">A collection of <see cref="Allocation"/> rows.</param>
            //        public void Delete(IEnumerable<Allocation> allocations)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                SyntaxFactory.Identifier("Delete"))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList<ParameterSyntax>(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(this.tableElement.Name.ToPlural().ToVariableName()))
                        .WithType(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("IEnumerable"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                        SyntaxFactory.IdentifierName(this.tableElement.Name))))))))
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
                    SyntaxFactory.ForEachStatement(
                        SyntaxFactory.IdentifierName(
                            SyntaxFactory.Identifier(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.VarKeyword,
                                "var",
                                "var",
                                SyntaxFactory.TriviaList())),
                        SyntaxFactory.Identifier(this.tableElement.Name.ToVariableName()),
                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural().ToVariableName()),
                        SyntaxFactory.Block(this.DeleteRow)),
                };

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets a set of statements to delete a row.
        /// </summary>
        private IEnumerable<StatementSyntax> DeleteRow
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                var statements = new List<StatementSyntax>
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
                                    this.tableElement.PrimaryIndex.GetKeyAsArguments(this.tableElement.Name.ToVariableName()))))),

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
                                                            this.tableElement.PrimaryIndex.GetKeyAsArguments(
                                                                this.tableElement.Name.ToVariableName()),
                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName())),
                                                        }))))))))),
                };

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

                //            assetClass.RowVersion = this.DataModel.IncrementRowVersion();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()),
                                SyntaxFactory.IdentifierName("RowVersion")),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(this.tableElement.XmlSchemaDocument.Name)),
                                    SyntaxFactory.IdentifierName("IncrementRowVersion"))))));

                //                this.commitStack.Push(() => this.DeletedRows.AddFirst(allocation));
                statements.Add(
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
                                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()))))))))))));

                //            this.commitStack.Push(() => this.RowChanged?.Invoke(this, new RowChangedEventArgs<Account>(DataAction.Add, account)));
                statements.Add(
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
                                                                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName())),
                                                                                })))),
                                                            })))))))))));

                // This is the syntax for the body of the method.
                return SyntaxFactory.List<StatementSyntax>(statements);
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
                    //        /// Deletes a collection of<see cref="Allocation"/> rows.
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
                                                $" Deletes a <see cref=\"{this.tableElement.Name}\"/> row from the table.",
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

                    //        /// <param name="allocations">A collection of <see cref="Allocation"/> rows.</param>
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
                                                    $" <param name=\"{this.tableElement.Name.ToPlural().ToCamelCase()}\">A collection of <see cref=\"{this.tableElement.Name}\"/> rows.</param>",
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
        /// Deletes the row from the parent row.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <returns>The statements to delete a row from the parent row.</returns>
        private IEnumerable<StatementSyntax> DeleteFromParent(ForeignIndexElement foreignIndexElement)
        {
            var statements = new List<StatementSyntax>();

            //            var account = this.DataModel.Accounts.Find(order.AccountCode);
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
                                                    SyntaxFactory.IdentifierName(uniqueIndexElement.Table.XmlSchemaDocument.Name)),
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