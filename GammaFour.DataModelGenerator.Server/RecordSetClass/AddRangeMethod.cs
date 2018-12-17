// <copyright file="AddRangeMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RecordSetClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Pluralize.NET;

    /// <summary>
    /// Creates a method to add a record to the set.
    /// </summary>
    public class AddRangeMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddRangeMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public AddRangeMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "AddRange";

            //        /// <summary>
            //        /// Adds a collection of <see cref="Buyer"/> records to the set.
            //        /// </summary>
            //        /// <param name="buyers">The collection of buyers to be added.</param>
            //        public void Add(Buyer buyer)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithModifiers(this.Modifiers)
                .WithParameterList(this.Parameters)
                .WithBody(this.Body)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the statements adds a record to the set.
        /// </summary>
        private List<StatementSyntax> AddRecord
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            object key = this.primaryKeyFunction(buyer);
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier("key"))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("primaryKeyFunction")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase())))))))))));

                //            buyer.Buyers = this;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase()),
                                SyntaxFactory.IdentifierName(new Pluralizer().Pluralize(this.tableElement.Name))),
                            SyntaxFactory.ThisExpression())));

                //            this.collection.Add(key, buyer);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("collection")),
                            SyntaxFactory.IdentifierName("Add")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("key")),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase()))
                                })))));

                //            this.undoStack.Push(() => { buyer.Buyers = null; this.collection.Remove(key); });
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("undoStack")),
                                SyntaxFactory.IdentifierName("Push")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression(
                                            SyntaxFactory.Block(
                                                SyntaxFactory.ExpressionStatement(
                                                    SyntaxFactory.AssignmentExpression(
                                                        SyntaxKind.SimpleAssignmentExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase()),
                                                            SyntaxFactory.IdentifierName(new Pluralizer().Pluralize(this.tableElement.Name))),
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.NullLiteralExpression))),
                                                SyntaxFactory.ExpressionStatement(
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.ThisExpression(),
                                                                SyntaxFactory.IdentifierName("collection")),
                                                            SyntaxFactory.IdentifierName("Remove")))
                                                    .WithArgumentList(
                                                        SyntaxFactory.ArgumentList(
                                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.IdentifierName("key"))))))))))))));

                // Add the record to each of the unique key indices on this set.
                foreach (UniqueKeyElement uniqueKeyElement in this.tableElement.UniqueKeys)
                {
                    //            this.BuyerKey.Add(buyer);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase())))))));
                }

                // Add the record to each of the foreign key indices on this set.
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ParentKeys)
                {
                    //            this.CountryBuyerCountryIdKey.Add(buyer);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase())))))));
                }

                //            this.OnRecordChanging(DataAction.Add, null, buyer);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("OnRecordChanging")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("DataAction"),
                                                SyntaxFactory.IdentifierName("Add"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.NullLiteralExpression)),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase()))
                                    })))));

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
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                if (Transaction.Current != null)
                //                {
                //                    <Enroll in Transaction>
                //                }
                statements.Add(EnlistInTransactionExpression.Statement);

                //                if (!this.Lock.IsWriteLockHeld)
                //                {
                //                    <Acquire Write Lock>
                //                }
                statements.Add(AcquireWriteLockExpression.Statement);

                //            foreach (Buyer buyer in buyers)
                //            {
                statements.Add(
                    SyntaxFactory.ForEachStatement(
                        SyntaxFactory.IdentifierName(this.tableElement.Name),
                        SyntaxFactory.Identifier(this.tableElement.Name.ToCamelCase()),
                        SyntaxFactory.IdentifierName(new Pluralizer().Pluralize(this.tableElement.Name).ToCamelCase()),
                        SyntaxFactory.Block(this.AddRecord)));

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
                //        /// Adds a collection of <see cref="Buyer"/> records to the set.
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
                                                $" Adds a collection of <see cref=\"{this.tableElement.Name}\"/> records to the set.",
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

                //        /// <param name="buyers">The collection of buyer records to be added.</param>
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
                                                    $" <param name=\"{new Pluralizer().Pluralize(this.tableElement.Name).ToCamelCase()}\">The collection of {this.tableElement.Name.ToCamelCase()} records to be added.</param>",
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
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();

                // The row parameter comes after the key elements.
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier(new Pluralizer().Pluralize(this.tableElement.Name).ToCamelCase()))
                    .WithType(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("IEnumerable"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName(this.tableElement.Name))))));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }
    }
}