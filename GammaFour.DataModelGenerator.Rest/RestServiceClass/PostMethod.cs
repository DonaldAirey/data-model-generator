// <copyright file="PostMethod.cs" company="Gamma Four, Inc.">
//    Copyright � 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService.RestServiceClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// A method to create a record.
    /// </summary>
    public class PostMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public PostMethod(TableElement tableElement)
        {
            // Initialize the object.  Note that we decorate the name of every method that's not the primary key to prevent ambiguous signatures.
            this.tableElement = tableElement ?? throw new ArgumentNullException(nameof(tableElement));
            this.Name = $"Post{this.tableElement.Name}";

            //        /// <summary>
            //        /// Post the <see cref="Province"/> record into the domain.
            //        /// </summary>
            //        /// <param name="name">The Name identifier.</param>
            //        /// <param name="countryCode">The CountryCode identifier.</param>
            //        /// <param name="jObject">The JSON record.</param>
            //        /// <returns>The result of the POST verb.</returns>
            //        [HttpPost]
            //        public async Task<IActionResult> PostProvince([From`] JObject jObject)
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
                .WithAttributeLists(PostMethod.Attributes)
                .WithModifiers(PostMethod.Modifiers)
                .WithParameterList(PostMethod.Parameters)
                .WithBody(this.Body)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the data contract attribute syntax.
        /// </summary>
        private static SyntaxList<AttributeListSyntax> Attributes
        {
            get
            {
                // This collects all the attributes.
                List<AttributeListSyntax> attributes = new List<AttributeListSyntax>();

                //        [HttpPost]
                attributes.Add(
                    SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("HttpPost")))));

                // The collection of attributes.
                return SyntaxFactory.List<AttributeListSyntax>(attributes);
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static BlockSyntax HandleDbUpdateException
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                this.logger.LogError(dbUpdateException.Message);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("logger")),
                                SyntaxFactory.IdentifierName("LogError")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("dbUpdateException"),
                                            SyntaxFactory.IdentifierName("Message"))))))));

                //                throw dbUpdateException;
                statements.Add(
                    SyntaxFactory.ThrowStatement(
                        SyntaxFactory.IdentifierName("dbUpdateException")));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(statements);
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static BlockSyntax HandleOperationCanceledException
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                return this.StatusCode(408);
                statements.Add(
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
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NumericLiteralExpression,
                                            SyntaxFactory.Literal(408))))))));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(statements);
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private static SyntaxList<CatchClauseSyntax> CatchClauses
        {
            get
            {
                // The catch clauses are collected in this list.
                List<CatchClauseSyntax> clauses = new List<CatchClauseSyntax>();

                //            catch (DbUpdateException dbUpdateException)
                //            {
                //                <HandleDbUpdateException>
                //            }
                clauses.Add(
                    SyntaxFactory.CatchClause()
                    .WithDeclaration(
                        SyntaxFactory.CatchDeclaration(
                            SyntaxFactory.IdentifierName("DbUpdateException"))
                        .WithIdentifier(
                            SyntaxFactory.Identifier("dbUpdateException")))
                    .WithBlock(PostMethod.HandleDbUpdateException));

                //            catch (OperationCanceledException)
                //            {
                //                <HandleOperationCanceledException>
                //            }
                clauses.Add(
                    SyntaxFactory.CatchClause()
                    .WithDeclaration(
                        SyntaxFactory.CatchDeclaration(
                            SyntaxFactory.IdentifierName("OperationCanceledException")))
                    .WithBlock(PostMethod.HandleOperationCanceledException));

                // This is the collection of catch clauses.
                return SyntaxFactory.List<CatchClauseSyntax>(clauses);
            }
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private static ParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters.
                List<SyntaxNodeOrToken> parameters = new List<SyntaxNodeOrToken>();

                // [FromBody] JObject jObject
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("jObject"))
                    .WithAttributeLists(
                        SyntaxFactory.SingletonList<AttributeListSyntax>(
                            SyntaxFactory.AttributeList(
                                SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                    SyntaxFactory.Attribute(
                                        SyntaxFactory.IdentifierName("FromBody"))))))
                    .WithType(
                        SyntaxFactory.IdentifierName("JObject")));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
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
        /// Gets the body.
        /// </summary>
        private BlockSyntax Body
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            if (!this.ModelState.IsValid)
                //            {
                //                return this.BadRequest(this.ModelState);
                //            }
                statements.Add(CheckStateExpression.Syntax);

                //            try
                //            {
                //                <TryBlock>
                //            }
                //            catch
                //            {
                //                <CatchClauses>
                //            }
                statements.Add(
                    SyntaxFactory.TryStatement(PostMethod.CatchClauses)
                    .WithBlock(
                        SyntaxFactory.Block(this.TryBlock)));

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
                //        /// Post the <see cref="Province"/> record into the domain.
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
                                                $" Post the <see cref=\"{this.tableElement.Name}\"/> record into the domain.",
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
                                        }))))));

                //        /// <param name="jObject">The message body.</param>
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
                                                        $" <param name=\"jObject\">The message body.</param>",
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        Environment.NewLine,
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                            }))))));

                //        /// <returns>The result of the POST verb.</returns>
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
                                                        $" <returns>The result of the POST verb.</returns>",
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
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> PostBody
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                        Account account = jObject.ToObject<Account>();
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.tableElement.Name))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(this.tableElement.Name.ToVariableName()))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("jObject"),
                                                SyntaxFactory.GenericName(
                                                    SyntaxFactory.Identifier("ToObject"))
                                                .WithTypeArgumentList(
                                                    SyntaxFactory.TypeArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                            SyntaxFactory.IdentifierName(this.tableElement.Name))))))))))));

                //                    await lockingTransaction.WaitWriterAsync(account).ConfigureAwait(false);
                statements.Add(
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
                                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));

                //                        this.domain.Countries.Add(country);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(this.tableElement.XmlSchemaDocument.Domain.ToVariableName())),
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName())))))));

                //                            await this.domainContext.Countries.AddAsync(country).ConfigureAwait(false);
                statements.Add(
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
                                                    SyntaxFactory.IdentifierName($"{this.tableElement.XmlSchemaDocument.Domain.ToCamelCase()}Context")),
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
                                                SyntaxKind.FalseLiteralExpression))))))));

                //                            await this.domainContext.SaveChangesAsync().ConfigureAwait(false);
                statements.Add(
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
                                                SyntaxFactory.IdentifierName($"{this.tableElement.XmlSchemaDocument.Domain.ToCamelCase()}Context")),
                                            SyntaxFactory.IdentifierName("SaveChangesAsync"))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));

                //                    lockingTransaction.Complete();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("lockingTransaction"),
                                SyntaxFactory.IdentifierName("Complete")))));

                //                    return this.Ok(new { province.CountryCode, province.CountryId, province.Name, province.ProvinceId, province.RegionName, province.RegionId, province.Short, province.RowVersion });
                statements.Add(
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
                                        AnonymousRecordExpression.GetSyntax(this.tableElement)))))));

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
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // We want to find all the relations to parent records in order to pull apart the JSON structure.  We can address different indices
                // using different parameters in the JSON.
                Dictionary<ColumnElement, List<UniqueKeyElement>> columnResolutionKeys = new Dictionary<ColumnElement, List<UniqueKeyElement>>();
                foreach (ForeignKeyElement parentKeyElement in this.tableElement.ParentKeys)
                {
                    foreach (ColumnReferenceElement columnReferenceElement in parentKeyElement.Columns)
                    {
                        ColumnElement columnElement = columnReferenceElement.Column;
                        foreach (UniqueKeyElement additionalUniqueKeyElement in parentKeyElement.UniqueKey.Table.UniqueKeys)
                        {
                            if (!additionalUniqueKeyElement.IsPrimaryKey)
                            {
                                if (!columnResolutionKeys.TryGetValue(columnElement, out List<UniqueKeyElement> uniqueKeyElements))
                                {
                                    uniqueKeyElements = new List<UniqueKeyElement>();
                                    columnResolutionKeys.Add(columnElement, uniqueKeyElements);
                                }

                                uniqueKeyElements.Add(additionalUniqueKeyElement);
                            }
                        }
                    }
                }

                // This constructs code that attempts to resolve a column from the input parameters.  Sometimes a primary key index isn't available
                // to the outside world because the data comes from an external system or is kept in a script file.  This makes use of the fact that
                // a relationship may exist between a child and parent table.
                foreach (var columnKeyPair in columnResolutionKeys)
                {
                    ColumnElement columnElement = columnKeyPair.Key;
                    List<UniqueKeyElement> uniqueKeys = columnKeyPair.Value;

                    //                using (var lockingTransaction = new LockingTransaction(this.transactionTimeout))
                    //                {
                    //                    <ResolveParentRecord>
                    //                }
                    statements.Add(
                        SyntaxFactory.UsingStatement(
                             SyntaxFactory.Block(PostMethod.ResolveParentRecord(columnElement, uniqueKeys)))
                         .WithDeclaration(
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
                                                                 SyntaxFactory.IdentifierName("transactionTimeout"))))))))))));
                }

                //            await lockingTransaction.WaitWriterAsync(this.domain.Accounts).ConfigureAwait(false);
                //            await lockingTransaction.WaitWriterAsync(this.domain.Accounts.ItemAccountKey).ConfigureAwait(false);
                //            await lockingTransaction.WaitWriterAsync(this.domain.Accounts.AccountSymbolKey).ConfigureAwait(false);
                //            await lockingTransaction.WaitWriterAsync(this.domain.Accounts.AccountKey).ConfigureAwait(false);
                statements.AddRange(LockTableStatements.GetUsingSyntax(this.tableElement, this.PostBody));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique key element.</param>
        /// <returns>A block of statements.</returns>
        private static List<StatementSyntax> FindParentRecord(UniqueKeyElement uniqueKeyElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                                    if (securityExternalKey.HasValues)
            //                                    {
            //                                        <TranslateArgument>
            //                                    }
            statements.Add(
                SyntaxFactory.IfStatement(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(uniqueKeyElement.Name.ToVariableName()),
                        SyntaxFactory.IdentifierName("HasValues")),
                    SyntaxFactory.Block(PostMethod.TranslateArgument(uniqueKeyElement))));

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint element.</param>
        /// <returns>A block of code to lock the record and replace the incoming JSON parameters with data from the record.</returns>
        private static List<StatementSyntax> TranslateArgument(UniqueKeyElement uniqueKeyElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                            var accountExternalKeyMnemonic = (string)accountExternalKey.GetValue("mnemonic", StringComparison.OrdinalIgnoreCase);
            foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("var"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier($"{uniqueKeyElement.Name.ToCamelCase()}{columnElement.Name}"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.CastExpression(
                                            SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name.ToVariableName()),
                                                    SyntaxFactory.IdentifierName("GetValue")))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.LiteralExpression(
                                                                    SyntaxKind.StringLiteralExpression,
                                                                    SyntaxFactory.Literal(columnElement.Name.ToVariableName()))),
                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.IdentifierName("StringComparison"),
                                                                    SyntaxFactory.IdentifierName("OrdinalIgnoreCase"))),
                                                        }))))))))));
            }

            //                            await lockingTransaction.WaitReaderAsync(this.domain.Accounts.AccountSymbolKey).ConfigureAwait(false);
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("lockingTransaction"),
                                        SyntaxFactory.IdentifierName("WaitReaderAsync")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(uniqueKeyElement.XmlSchemaDocument.Domain.ToVariableName())),
                                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name)))))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression))))))));

            //                    region = this.domain.Regions.RegionExternalKey.Find((regionExternalKeyName, regionExternalKeyCountryCode));
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name.ToVariableName()),
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
                                            SyntaxFactory.IdentifierName(uniqueKeyElement.XmlSchemaDocument.Domain.ToVariableName())),
                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Find")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(UniqueKeyExpression.GetSyntax(uniqueKeyElement, true))))));

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <param name="columnElement">The column element.</param>
        /// <returns>A block of statements.</returns>
        private static List<StatementSyntax> LockParentRecord(TableElement tableElement, ColumnElement columnElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                        await lockingTransaction.WaitWriterAsync(account).ConfigureAwait(false);
            statements.Add(
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
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()))))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression))))))));

            //                        accountIdObject.Replace(new JValue(account.AccountId));
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName($"{columnElement.Name.ToCamelCase()}Object"),
                            SyntaxFactory.IdentifierName("Replace")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ObjectCreationExpression(
                                        SyntaxFactory.IdentifierName("JValue"))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                                                        SyntaxFactory.IdentifierName(tableElement.PrimaryKey.Columns[0].Column.Name))))))))))));

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <returns>A block of statements.</returns>
        private static List<StatementSyntax> ResolveParentRecord(ColumnElement columnElement, List<UniqueKeyElement> uniqueKeys)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                        var childIdObject = jObject.GetValue("childId", StringComparison.InvariantCulture) as JObject;
            statements.Add(
                SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName("var"))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier($"{columnElement.Name.ToCamelCase()}Object"))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.AsExpression,
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("jObject"),
                                                SyntaxFactory.IdentifierName("GetValue")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.LiteralExpression(
                                                                        SyntaxKind.StringLiteralExpression,
                                                                        SyntaxFactory.Literal(columnElement.Name.ToVariableName()))),
                                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.IdentifierName("StringComparison"),
                                                                        SyntaxFactory.IdentifierName("OrdinalIgnoreCase"))),
                                                    }))),
                                        SyntaxFactory.IdentifierName("JObject"))))))));

            //                        if (childIdObject != null)
            //                        {
            //                            <ResolveColumnFromParent>
            //                        }
            statements.Add(SyntaxFactory.IfStatement(
                SyntaxFactory.BinaryExpression(
                    SyntaxKind.NotEqualsExpression,
                    SyntaxFactory.IdentifierName($"{columnElement.Name.ToCamelCase()}Object"),
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.NullLiteralExpression)),
                SyntaxFactory.Block(PostMethod.ResolveColumnFromParent(columnElement, uniqueKeys))));

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="columnElement">The column element.</param>
        /// <param name="uniqueKeyElements">The unique key used to resolve the given column.</param>
        /// <returns>A block of statements.</returns>
        private static List<StatementSyntax> ResolveColumnFromParent(ColumnElement columnElement, List<UniqueKeyElement> uniqueKeyElements)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // All the unique indices in this list should share the same table 'cause that's how we grouped them.
            TableElement tableElement = uniqueKeyElements[0].Table;

            //                Country country = null;
            statements.Add(
                SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(tableElement.Name))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(tableElement.Name.ToVariableName()))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)))))));

            foreach (UniqueKeyElement uniqueKeyElement in uniqueKeyElements)
            {
                //                var countryCountryCodeKey = jObject.GetValue("countryCountryCodeKey");
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("var"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(uniqueKeyElement.Name.ToVariableName()))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.BinaryExpression(
                                            SyntaxKind.AsExpression,
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName($"{columnElement.Name.ToCamelCase()}Object"),
                                                    SyntaxFactory.IdentifierName("GetValue")))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.LiteralExpression(
                                                                    SyntaxKind.StringLiteralExpression,
                                                                    SyntaxFactory.Literal(uniqueKeyElement.Name.ToVariableName()))),
                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.IdentifierName("StringComparison"),
                                                                    SyntaxFactory.IdentifierName("OrdinalIgnoreCase"))),
                                                        }))),
                                            SyntaxFactory.IdentifierName("JObject"))))))));

                //                if (countryCountryCodeKey != null)
                //                {
                //                    <FindRecord>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(uniqueKeyElement.Name.ToVariableName()),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(PostMethod.FindParentRecord(uniqueKeyElement))));
            }

            //                if (country != null)
            //                {
            //                    <LockParentRecord>
            //                }
            statements.Add(
                SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression)),
                    SyntaxFactory.Block(PostMethod.LockParentRecord(tableElement, columnElement))));

            // This is the complete block.
            return statements;
        }
    }
}