// <copyright file="PutMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService.RestServiceClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
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
        private readonly UniqueKeyElement uniqueKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutMethod"/> class.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema.</param>
        public PutMethod(UniqueKeyElement uniqueKeyElement)
        {
            // Initialize the object.  Note that we decorate the name of every method that's not the primary key to prevent ambiguous signatures.
            this.uniqueKeyElement = uniqueKeyElement ?? throw new ArgumentNullException(nameof(uniqueKeyElement));
            this.Name = $"Put{this.uniqueKeyElement.Table.Name}" + (uniqueKeyElement.IsPrimaryKey ? string.Empty : "By" + uniqueKeyElement.Name);

            //        /// <summary>
            //        /// Put the <see cref="Province"/> record into the dataModel.
            //        /// </summary>
            //        /// <param name="name">The Name identifier.</param>
            //        /// <param name="countryCode">The CountryCode identifier.</param>
            //        /// <param name="jObject">The JSON record.</param>
            //        /// <returns>The result of the PUT verb.</returns>
            //        [HttpPut("provinceExternalKey/{name}/{countryCode}")]
            //        public async Task<IActionResult> PutProvince([FromRoute] string name, [FromRoute] string countryCode, [FromBody] JObject jObject)
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
                    .WithBlock(PutMethod.HandleDbUpdateException));

                //            catch (OperationCanceledException)
                //            {
                //                <HandleOperationCanceledException>
                //            }
                clauses.Add(
                    SyntaxFactory.CatchClause()
                    .WithDeclaration(
                        SyntaxFactory.CatchDeclaration(
                            SyntaxFactory.IdentifierName("OperationCanceledException")))
                    .WithBlock(PutMethod.HandleOperationCanceledException));

                // This is the collection of catch clauses.
                return SyntaxFactory.List<CatchClauseSyntax>(clauses);
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
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> AddRecord
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                        account = jObject.ToObject<Account>();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName()),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("jObject"),
                                    SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier("ToObject"))
                                    .WithTypeArgumentList(
                                        SyntaxFactory.TypeArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name)))))))));

                //            await lockingTransaction.WaitWriterAsync(account).ConfigureAwait(false);
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
                                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName()))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));

                //                        this.dataModel.Countries.Add(country);
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
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName())))))));

                //                            await this.dataModelContext.Countries.AddAsync(country);
                statements.Add(
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
                                            SyntaxFactory.IdentifierName($"{this.uniqueKeyElement.XmlSchemaDocument.DataModel.ToCamelCase()}Context")),
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName("AddAsync")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName()))))))));

                //                    await this.dataModelContext.SaveChangesAsync();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName($"{this.uniqueKeyElement.XmlSchemaDocument.DataModel.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName("SaveChangesAsync"))))));

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

                //        [HttpPut("provinceExternalKey/{name}/{countryCode}")]
                string literal = this.uniqueKeyElement.Name.ToVariableName();
                foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
                {
                    literal += $"/{{{columnReferenceElement.Column.Name.ToCamelCase()}}}";
                }

                string columnName = this.uniqueKeyElement.Columns[0].Column.Name.ToVariableName();
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
                                            SyntaxFactory.Literal(literal)))))))));

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
                    SyntaxFactory.TryStatement(PutMethod.CatchClauses)
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
                //        /// Put the <see cref="Province"/> record into the dataModel.
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
                                                $" Put the <see cref=\"{this.uniqueKeyElement.Table.Name}\"/> record into the dataModel.",
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

                //        /// <param name="name">The Name identifier.</param>
                //        /// <param name="countryCode">The CountryCode identifier.</param>
                foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
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

                //        /// <param name="jObject">The JSON record.</param>
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
                                                        $" <param name=\"jObject\">The JSON record.</param>",
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
                foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
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

                // , [FromBody] JObject jObject
                parameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
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
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> TryBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // We want to find all the relations to parent records in order to resolve symbolic references.
                Dictionary<ColumnElement, List<UniqueKeyElement>> columnResolutionKeys = new Dictionary<ColumnElement, List<UniqueKeyElement>>();
                foreach (ForeignKeyElement parentKeyElement in this.uniqueKeyElement.Table.ParentKeys)
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

                // This attempts to resolve a parent record from a symbolic reference.  Sometimes a primary key index isn't available to the outside
                // world because the data comes from an external system or is kept in a script file.
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
                             SyntaxFactory.Block(ResolveParentRecord(columnElement, uniqueKeys)))
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

                //            await lockingTransaction.WaitReaderAsync(this.dataModel.Accounts);
                //            await lockingTransaction.WaitReaderAsync(this.dataModel.Accounts.ItemAccountKey);
                //            await lockingTransaction.WaitReaderAsync(this.dataModel.Accounts.AccountSymbolKey);
                //            await lockingTransaction.WaitReaderAsync(this.dataModel.Accounts.AccountKey);
                statements.AddRange(LockTableStatements.GetUsingSyntax(this.uniqueKeyElement.Table, this.PutBody));

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
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    var country = this.dataModel.Countries.CountryCountryCodeKey.Find(countryCode);
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("var"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(this.uniqueKeyElement.Table.Name.ToVariableName()))
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
                                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Name)),
                                                SyntaxFactory.IdentifierName("Find")))
                                        .WithArgumentList(
                                        SyntaxFactory.ArgumentList(UniqueKeyExpression.GetSyntax(this.uniqueKeyElement)))))))));

                //                    if (country == null)
                //                    {
                //                        <AddRecord>
                //                    }
                //                    else
                //                    {
                //                        <UpdateRecord>
                //                    }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName()),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(this.AddRecord))
                    .WithElse(
                        SyntaxFactory.ElseClause(
                            SyntaxFactory.Block(this.UpdateRecord))));

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
                                        AnonymousRecordExpression.GetSyntax(this.uniqueKeyElement.Table)))))));

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
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    await lockingTransaction.WaitReaderAsync(account).ConfigureAwait(false);
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
                                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName()))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));

                //                        if ($"\"{taxLot.RowVersion}\"" != this.Request.Headers["If-None-Match"])
                //                        {
                //                            return this.StatusCode(StatusCodes.Status412PreconditionFailed);
                //                        }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.InterpolatedStringExpression(
                                SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                            .WithContents(
                                SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                    new InterpolatedStringContentSyntax[]
                                    {
                                        SyntaxFactory.InterpolatedStringText()
                                        .WithTextToken(
                                            SyntaxFactory.Token(
                                                SyntaxFactory.TriviaList(),
                                                SyntaxKind.InterpolatedStringTextToken,
                                                "\\\"",
                                                "\\\"",
                                                SyntaxFactory.TriviaList())),
                                        SyntaxFactory.Interpolation(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName()),
                                                SyntaxFactory.IdentifierName("RowVersion"))),
                                        SyntaxFactory.InterpolatedStringText()
                                        .WithTextToken(
                                            SyntaxFactory.Token(
                                                SyntaxFactory.TriviaList(),
                                                SyntaxKind.InterpolatedStringTextToken,
                                                "\\\"",
                                                "\\\"",
                                                SyntaxFactory.TriviaList())),
                                    })),
                            SyntaxFactory.ElementAccessExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("Request")),
                                    SyntaxFactory.IdentifierName("Headers")))
                            .WithArgumentList(
                                SyntaxFactory.BracketedArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal("If-None-Match"))))))),
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
                                                        SyntaxFactory.IdentifierName("Status412PreconditionFailed")))))))))));

                //                        Account accountArgument = jObject.ToObject<Account>();
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier($"{this.uniqueKeyElement.Table.Name.ToVariableName()}Argument"))
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
                                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name))))))))))));

                //                        Account accountArgument = jObject.ToObject<Account>();
                //                        account.AccountTypeCode = accountArgument.AccountTypeCode;
                //                        account.Mnemonic = accountArgument.Mnemonic;
                foreach (ColumnElement columnElement in this.uniqueKeyElement.Table.Columns)
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
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName()),
                                    SyntaxFactory.IdentifierName(columnElement.Name)),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName($"{this.uniqueKeyElement.Table.Name.ToVariableName()}Argument"),
                                    SyntaxFactory.IdentifierName(columnElement.Name)))));
                }

                //                        this.dataModel.Countries.Update(country);
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
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Update")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName())))))));

                //                            this.dataModelContext.Countries.Update(country);
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
                                        SyntaxFactory.IdentifierName($"{this.uniqueKeyElement.XmlSchemaDocument.DataModel.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Update")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName())))))));

                //                            await this.dataModelContext.SaveChangesAsync();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName($"{this.uniqueKeyElement.XmlSchemaDocument.DataModel.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName("SaveChangesAsync"))))));

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

            //            await lockingTransaction.WaitReaderAsync(this.dataModel.Accounts.AccountSymbolKey).ConfigureAwait(false);
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
                                                            SyntaxFactory.IdentifierName(uniqueKeyElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name)))))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression))))))));

            //                    country = this.dataModel.Countries.CountryCountryCodeKey.Find(countryCountryCodeKeyCountryCode);
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
                                            SyntaxFactory.IdentifierName(uniqueKeyElement.XmlSchemaDocument.DataModel.ToVariableName())),
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
        private static List<StatementSyntax> ReplaceArgument(TableElement tableElement, ColumnElement columnElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //            await lockingTransaction.WaitReaderAsync(account).ConfigureAwait(false);
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
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()))))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression))))))));

            //                                childIdObject.Replace(new JValue(account.AccountId));
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
                //                    var accountExternalKey = accountIdObject.GetValue("accountExternalKey", StringComparison.OrdinalIgnoreCase) as JObject;
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
                //                    <FindParentRecord>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(uniqueKeyElement.Name.ToVariableName()),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(PutMethod.FindParentRecord(uniqueKeyElement))));
            }

            //                if (country != null)
            //                {
            //                    <ReplaceArgument>
            //                }
            statements.Add(
                SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression)),
                    SyntaxFactory.Block(PutMethod.ReplaceArgument(tableElement, columnElement))));

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code to resolve the parent record.
        /// </summary>
        private static SyntaxList<StatementSyntax> ResolveParentRecord(ColumnElement columnElement, List<UniqueKeyElement> uniqueKeys)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                        var childIdObject = jObject.GetValue("childId", StringComparison.OrdinalIgnoreCase) as JObject;
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
                SyntaxFactory.Block(PutMethod.ResolveColumnFromParent(columnElement, uniqueKeys))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}