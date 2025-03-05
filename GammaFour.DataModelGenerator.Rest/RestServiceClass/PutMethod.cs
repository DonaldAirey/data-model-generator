// <copyright file="PutMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService.RestServiceClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.DataModelGenerator.RestService;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// A method to create a row.
    /// </summary>
    public class PutMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The table schema.</param>
        public PutMethod(TableElement tableElement)
        {
            // Initialize the object.  Note that we decorate the name of every method that's not the primary key to prevent ambiguous signatures.
            this.tableElement = tableElement;
            this.Name = $"Put{this.tableElement.Name}";

            //        /// <summary>
            //        /// Puts the <see cref="Account"/> row.
            //        /// </summary>
            //        /// <param name="accountId">The AccountId key.</param>
            //        /// <param name="account">The <see cref="Account"/> row.</param>
            //        /// <returns>The result of applying the put action.</returns>
            //        [HttpPut("{accountId}")]
            //        public async Task<IActionResult> PutAccount([FromRoute] System.Guid accountId, [FromBody] Account account)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("Task"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                            SyntaxFactory.IdentifierName("IActionResult")))),
                SyntaxFactory.Identifier(this.Name))
            .WithAttributeLists(this.tableElement.PrimaryIndex.GetKeyAsHttpAttributes("HttpPut"))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
                    }))
            .WithParameterList(this.Parameters)
            .WithBody(this.Body)
            .WithLeadingTrivia(this.LeadingTrivia);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> AddRow
        {
            get
            {
                // This is used to collect the statements.
                var statements = new List<StatementSyntax>
                {
                    //                        await lockingTransaction.WaitWriterAsync(account);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("lockingTransaction"),
                                    SyntaxFactory.IdentifierName("WaitWriterAsync")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()))))))),

                    //                        await this.dataModel.Accounts.AddAsync(account).ConfigureAwait(false);
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
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName($"{this.tableElement.Document.DataModel.ToCamelCase()}")),
                                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName("AddAsync")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))),

                    //                        await this.dataModelContext.Accounts.AddAsync(account);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName($"{this.tableElement.Document.DataModel.ToCamelCase()}Context")),
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName("AddAsync")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()))))))),

                    //                existingRow = account;
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("existingRow"),
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()))),
                };

                // This is the complete block.
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
                var statements = new List<StatementSyntax>
                {
                    //            if (!this.ModelState.IsValid)
                    //            {
                    //                return this.BadRequest(this.ModelState);
                    //            }
                    CheckStateExpression.Syntax,

                    //            try
                    //            {
                    //                <TryBlock>
                    //            }
                    //            catch
                    //            {
                    //                <CommonCatchClauses>
                    //            }
                    SyntaxFactory.TryStatement(CommonStatements.CommonCatchClauses)
                    .WithBlock(SyntaxFactory.Block(this.TryBlock)),
                };

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private IEnumerable<SyntaxTrivia> LeadingTrivia
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <summary>
                    //        /// Puts the <see cref="Account"/> row.
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
                                                $" Puts the <see cref=\"{this.tableElement.Name}\"/> row.",
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

                //        /// <param name="account">The <see cref="Account"/> row.</param>
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

                //        /// <returns>The result of applying the put action.</returns>
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
                                                " <returns>The result of applying the put action.</returns>",
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
                parameters.AddRange(this.tableElement.PrimaryIndex.GetKeyAsHttpArguments());
                parameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));

                // [FromBody] Account account
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier(this.tableElement.Name.ToVariableName()))
                    .WithAttributeLists(
                        SyntaxFactory.SingletonList<AttributeListSyntax>(
                            SyntaxFactory.AttributeList(
                                SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                    SyntaxFactory.Attribute(
                                        SyntaxFactory.IdentifierName("FromBody"))))))
                    .WithType(
                        SyntaxFactory.IdentifierName(this.tableElement.Name)));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> IfModified
        {
            get
            {
                // This is used to collect the statements.
                var statements = new List<StatementSyntax>
                {
                    //                        await this.dataModel.Accounts.UpdateAsync(account).ConfigureAwait(false);
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
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName($"{this.tableElement.Document.DataModel.ToCamelCase()}")),
                                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName("UpdateAsync")))
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

                    //                            this.dataModelContext.Accounts.Update(existingRow);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName($"{this.tableElement.Document.DataModel.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Update")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("existingRow")))))),
                };

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> PatchRow
        {
            get
            {
                // This is used to collect the statements.
                var statements = new List<StatementSyntax>
                {
                    //                    var existingRow = this.dataModel.Accounts.Find(account.AccountId);
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
                                    SyntaxFactory.Identifier("existingRow"))
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
                                                        SyntaxFactory.IdentifierName(
                                                            this.tableElement.Document.DataModel.ToVariableName())),
                                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                                SyntaxFactory.IdentifierName("Find")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                    this.tableElement.PrimaryIndex.GetKeyAsFindArguments(
                                                        this.tableElement.Name.ToVariableName())))))))),

                    //                        if (existingRow == null)
                    //                        {
                    //                            <AddRow>
                    //                        }
                    //                        else
                    //                        {
                    //                            <UpdateRow>
                    //                        }
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.IdentifierName("existingRow"),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(this.AddRow))
                    .WithElse(
                        SyntaxFactory.ElseClause(
                            SyntaxFactory.Block(this.UpdateRow))),
                };

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> TryBlock
        {
            get
            {
                // This is used to collect the statements.
                var statements = new List<StatementSyntax>()
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
                                                            SyntaxFactory.IdentifierName("transactionTimeout")))))))))))
                    .WithUsingKeyword(
                        SyntaxFactory.Token(SyntaxKind.UsingKeyword)),

                    //                await lockingTransaction.WaitWriterAsync(this.dataModel.Accounts);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("lockingTransaction"),
                                    SyntaxFactory.IdentifierName("WaitWriterAsync")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(this.tableElement.Document.DataModel.ToCamelCase())),
                                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())))))))),

                    //                var existingRow = this.dataModel.Provinces.Find(accountId);
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
                                    SyntaxFactory.Identifier("existingRow"))
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
                                                        SyntaxFactory.IdentifierName(
                                                            this.tableElement.Document.DataModel.ToVariableName())),
                                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                                SyntaxFactory.IdentifierName("Find")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                    this.tableElement.PrimaryIndex.GetKeyAsFindArguments()))))))),

                    //                        if (existingRow == null)
                    //                        {
                    //                            <AddRow>
                    //                        }
                    //                        else
                    //                        {
                    //                            <UpdateRow>
                    //                        }
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.IdentifierName("existingRow"),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(this.AddRow))
                    .WithElse(
                        SyntaxFactory.ElseClause(
                            SyntaxFactory.Block(this.UpdateRow))),

                    //                    await this.dataModelContext.SaveChangesAsync();
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName($"{this.tableElement.Document.DataModel.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName("SaveChangesAsync"))))),

                    //                existingRow = new Account(existingRow);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("existingRow"),
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.IdentifierName(this.tableElement.Name))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("existingRow"))))))),

                    //                    lockingTransaction.Complete();
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("lockingTransaction"),
                                SyntaxFactory.IdentifierName("Complete")))),

                    //                    return this.Ok(existingRows);
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Ok")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("existingRow")))))),
                };

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> UpdateRow
        {
            get
            {
                // This is used to collect the statements.
                var statements = new List<StatementSyntax>
                {
                    //                        await lockingTransaction.WaitWriterAsync(existingRow);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("lockingTransaction"),
                                    SyntaxFactory.IdentifierName("WaitWriterAsync")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("existingRow"))))))),

                    //                        if (existingRow.RowVersion != account.RowVersion)
                    //                        {
                    //                            return this.StatusCode(StatusCodes.Status412PreconditionFailed);
                    //                        }
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
                                SyntaxFactory.ReturnStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("StatusCode")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("StatusCodes"),
                                                        SyntaxFactory.IdentifierName("Status412PreconditionFailed")))))))))),
                };

                //                                existingRow.CurrencyCode = account.CurrencyCode;
                //                                existingRow.Cusip = account.Cusip;
                //                                existingRow.Description = account.Description;
                //                                existingRow.Figi = account.Figi;
                //                                existingRow.AccountType = account.AccountType;
                //                                existingRow.Image = account.Image;
                //                                existingRow.Isin = account.Isin;
                //                                existingRow.Mnemonic = account.Mnemonic;
                //                                existingRow.Name = account.Name;
                //                                existingRow.PriceFactor = account.PriceFactor;
                //                                existingRow.QuantityFactor = account.QuantityFactor;
                //                                existingRow.Ric = account.Ric;
                //                                existingRow.Sedol = account.Sedol;
                //                                existingRow.Symbol = account.Symbol;
                foreach (var columnElement in this.tableElement.Columns)
                {
                    // Don't attempt to update these kinds of columns.
                    if (!columnElement.IsRowVersion && !columnElement.IsPrimaryKey)
                    {
                        statements.Add(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("existingRow"),
                                        SyntaxFactory.IdentifierName(columnElement.Name)),
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()),
                                        SyntaxFactory.IdentifierName(columnElement.Name)))));
                    }
                }

                //                        if (existingRow.IsModified)
                //                        {
                //                             <IfModified>
                //                        }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("existingRow"),
                            SyntaxFactory.IdentifierName("IsModified")),
                        SyntaxFactory.Block(this.IfModified)));

                // This is the complete block.
                return statements;
            }
        }
    }
}