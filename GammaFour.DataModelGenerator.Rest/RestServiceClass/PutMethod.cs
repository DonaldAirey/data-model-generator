// <copyright file="PutMethod.cs" company="Gamma Four, Inc.">
//    Copyright � 2025 - Gamma Four, Inc.  All Rights Reserved.
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
    /// A method to create a record.
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
            //        /// Puts the <see cref="ManagedAccount"/> record.
            //        /// </summary>
            //        /// <param name="managedAccountId">The ManagedAccountId identifier.</param>
            //        /// <param name="jsonObject">The JSON record.</param>
            //        /// <returns>The result of the PUT verb.</returns>
            //        [HttpPut("managedAccounts/{managedAccountId}")]
            //        public async Task<IActionResult> PutManagedAccount([FromRoute] int managedAccountId, [FromBody] JsonObject jsonObject)
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
            .WithAttributeLists(this.Attributes)
            .WithModifiers(PutMethod.Modifiers)
            .WithParameterList(this.Parameters)
            .WithBody(this.Body)
            .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // public async
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
                    });
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> AddRecord
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //                        serverListMap = clientListMap;
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName($"server{this.tableElement.Name}"),
                            SyntaxFactory.IdentifierName($"client{this.tableElement.Name}"))),

                    ////                        await lockingTransaction.WaitWriterAsync(clientManagedAccount).ConfigureAwait(false);
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
                                                    SyntaxFactory.IdentifierName($"server{this.tableElement.Name}"))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))),

                    //                        this.dataModel.ManagedAccounts.Add(clientManagedAccount);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(this.tableElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName($"server{this.tableElement.Name}")))))),

                    //                        await this.dataModelContext.ManagedAccounts.AddAsync(clientManagedAccount);
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
                                            SyntaxFactory.IdentifierName($"{this.tableElement.XmlSchemaDocument.DataModel.ToCamelCase()}Context")),
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName("AddAsync")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName($"server{this.tableElement.Name}"))))))),

                    //                        await this.dataModelContext.SaveChangesAsync();
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName($"{this.tableElement.XmlSchemaDocument.DataModel.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName("SaveChangesAsync"))))),
                };

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets the data contract attribute syntax.
        /// </summary>
        private SyntaxList<AttributeListSyntax> Attributes
        {
            get
            {
                // This collects all the attributes.
                List<AttributeListSyntax> attributes = new List<AttributeListSyntax>();

                //        [HttpPut("{fungibleId}")]
                string endpoint = string.Empty;
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryKey.Columns)
                {
                    if (endpoint != string.Empty)
                    {
                        endpoint += "/";
                    }

                    endpoint += $"{{{columnReferenceElement.Column.Name.ToCamelCase()}}}";
                }

                attributes.Add(
                    SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("HttpPut"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(endpoint)))))))));

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
                List<StatementSyntax> statements = new List<StatementSyntax>
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
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <summary>
                    //        /// Put the <see cref="Province"/> record into the dataModel.
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
                                                $" Puts the <see cref=\"{this.tableElement.Name}\"/> record.",
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

                //        /// <param name="fungibleId">The FungibleId identifier.</param>
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryKey.Columns)
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
                                                        $" <param name=\"{columnElement.Name.ToCamelCase()}\">The {columnElement.Name} identifier.</param>",
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        Environment.NewLine,
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                }))))));
                }

                //        /// <param name="jsonObject">The JSON record.</param>
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
                                                        $" <param name=\"jsonObject\">The JSON record.</param>",
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        Environment.NewLine,
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                            }))))));

                //        /// <returns>The result of the PUT verb.</returns>
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
                                                        $" <returns>The result of the PUT verb.</returns>",
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

                // [FromRoute] string countryCode
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryKey.Columns)
                {
                    if (parameters.Count != 0)
                    {
                        parameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    parameters.Add(
                        SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(columnReferenceElement.Column.Name.ToVariableName()))
                            .WithAttributeLists(
                                SyntaxFactory.SingletonList<AttributeListSyntax>(
                                    SyntaxFactory.AttributeList(
                                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                            SyntaxFactory.Attribute(
                                                SyntaxFactory.IdentifierName("FromRoute"))))))
                            .WithType(Conversions.FromType(columnReferenceElement.Column.ColumnType)));
                }

                // , [FromBody] JsonObject jsonObject
                parameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("jsonObject"))
                    .WithAttributeLists(
                        SyntaxFactory.SingletonList<AttributeListSyntax>(
                            SyntaxFactory.AttributeList(
                                SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                    SyntaxFactory.Attribute(
                                        SyntaxFactory.IdentifierName("FromBody"))))))
                    .WithType(
                        SyntaxFactory.IdentifierName("JsonObject")));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
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
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            await lockingTransaction.WaitReaderAsync(this.dataModel.Accounts);
                //            await lockingTransaction.WaitReaderAsync(this.dataModel.Accounts.ItemAccountKey);
                //            await lockingTransaction.WaitReaderAsync(this.dataModel.Accounts.AccountSymbolKey);
                //            await lockingTransaction.WaitReaderAsync(this.dataModel.Accounts.AccountKey);
                statements.AddRange(LockTableStatements.GetUsingSyntax(this.tableElement, this.PutBody));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> PutBody
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //                    var clientManagedAccount = jsonObject.GetValue<ManagedAccount>();
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
                                    SyntaxFactory.Identifier($"client{this.tableElement.Name}"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("jsonObject"),
                                                SyntaxFactory.GenericName(
                                                    SyntaxFactory.Identifier("Deserialize"))
                                                .WithTypeArgumentList(
                                                    SyntaxFactory.TypeArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                            SyntaxFactory.IdentifierName(this.tableElement.Name))))))))))),

                    //                    var serverCountry = this.dataModel.Countries.CountryCountryCodeKey.Find(countryCode);
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("var"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier($"server{this.tableElement.Name}"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(this.tableElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(this.tableElement.PrimaryKey.Name)),
                                                SyntaxFactory.IdentifierName("Find")))
                                        .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            RestService.UniqueKeyExpression.GetSyntax(this.tableElement.PrimaryKey)))))))),

                    //                    if (serverCountry == null)
                    //                    {
                    //                        <AddRecord>
                    //                    }
                    //                    else
                    //                    {
                    //                        <UpdateRecord>
                    //                    }
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.IdentifierName($"server{this.tableElement.Name}"),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(this.AddRecord))
                    .WithElse(
                        SyntaxFactory.ElseClause(
                            SyntaxFactory.Block(this.UpdateRecord))),

                    //                    lockingTransaction.Complete();
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("lockingTransaction"),
                                SyntaxFactory.IdentifierName("Complete")))),

                    //                    return this.Ok(fungibles);
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
                                        SyntaxFactory.IdentifierName($"server{this.tableElement.Name}")))))),
                };

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> UpdateRecord
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //                        await lockingTransaction.WaitWriterAsync(serverManagedAccount).ConfigureAwait(false);
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
                                                    SyntaxFactory.IdentifierName($"server{this.tableElement.Name}"))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))),

                    //                                if (serverFungible.RowVersion != clientFungible.RowVersion)
                    //                                {
                    //                                    return this.StatusCode(StatusCodes.Status412PreconditionFailed);
                    //                                }
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName($"server{this.tableElement.Name}"),
                                SyntaxFactory.IdentifierName("RowVersion")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName($"client{this.tableElement.Name}"),
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

                //                        serverManagedAccount.AccountId = clientManagedAccount.AccountId;
                //                        serverManagedAccount.BaseFungibleId = clientManagedAccount.BaseFungibleId;
                //                        serverManagedAccount.LotHandlingCode = clientManagedAccount.LotHandlingCode;
                //                        serverManagedAccount.ModelId = clientManagedAccount.ModelId;
                //                        serverManagedAccount.Symbol = clientManagedAccount.Symbol;
                foreach (ColumnElement columnElement in this.tableElement.Columns)
                {
                    // Don't attempt to insert these kinds of properties.
                    if (columnElement.IsRowVersion || columnElement.IsAutoIncrement || columnElement.IsPrimaryKey)
                    {
                        continue;
                    }

                    //                        account.AccountTypeCode = accountArgument.AccountTypeCode;
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName($"server{this.tableElement.Name}"),
                                    SyntaxFactory.IdentifierName(columnElement.Name)),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName($"client{this.tableElement.Name}"),
                                    SyntaxFactory.IdentifierName(columnElement.Name)))));
                }

                //                                if (serverListMap.RecordState != RecordState.Unchanged)
                //                                {
                //                                    <UpdateChangedRecord>
                //                                {
                statements.Add(SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName($"server{this.tableElement.Name}"),
                            SyntaxFactory.IdentifierName("RecordState")),
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("RecordState"),
                            SyntaxFactory.IdentifierName("Unchanged"))),
                    SyntaxFactory.Block(this.UpdateChangedRecord)));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> UpdateChangedRecord
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //                        this.dataModel.ManagedAccounts.Update(serverManagedAccount);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(this.tableElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Update")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName($"server{this.tableElement.Name}")))))),

                    //                        this.dataModelContext.ManagedAccounts.Update(serverManagedAccount);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName($"{this.tableElement.XmlSchemaDocument.DataModel.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Update")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName($"server{this.tableElement.Name}")))))),

                    //                        await this.dataModelContext.SaveChangesAsync();
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName($"{this.tableElement.XmlSchemaDocument.DataModel.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName("SaveChangesAsync"))))),
                };

                // This is the complete block.
                return statements;
            }
        }
    }
}