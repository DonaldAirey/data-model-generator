// <copyright file="PutMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
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
    using Pluralize.NET;

    /// <summary>
    /// A method to create a record.
    /// </summary>
    public class PutMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private UniqueKeyElement uniqueKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutMethod"/> class.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema.</param>
        public PutMethod(UniqueKeyElement uniqueKeyElement)
        {
            // Initialize the object.
            this.uniqueKeyElement = uniqueKeyElement;
            this.Name = $"Put{this.uniqueKeyElement.Table.Name}";

            //        /// <summary>
            //        /// Put the <see cref="Country"/> record into the domain.
            //        /// </summary>
            //        /// <param name="countryCode">The external identifier for the record.</param>
            //        /// <param name="jObject">The JSON record.</param>
            //        /// <returns>The result of the PUT action.</returns>
            //        [HttpPut("countryCode/{countryCode}")]
            //        public async Task<IActionResult> PutCountry([FromRoute] string countryCode, [FromBody] JObject jObject)
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
            .WithModifiers(this.Modifiers)
            .WithParameterList(this.Parameters)
            .WithBody(this.Body)
            .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> AdditionTransaction
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

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
                                                    SyntaxFactory.IdentifierName($"{this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase()}Context")),
                                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName("AddAsync")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase()))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));

                //                            await this.domainContext.SaveChangesAsync();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName($"{this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName("SaveChangesAsync"))))));

                //                            additionScope.Complete();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("additionScope"),
                                SyntaxFactory.IdentifierName("Complete")))));

                // This is the complete block.
                return statements;
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

                //                        country = new Country() { CountryCode = jObject.Value<string>("countryCode"), Name = jObject.Value<string>("name") };
                List<SyntaxNodeOrToken> propertyInitialization = new List<SyntaxNodeOrToken>();
                foreach (ColumnElement columnElement in this.uniqueKeyElement.Table.Columns)
                {
                    // Don't attempt to insert these kinds of properties.
                    if (columnElement.IsRowVersion || columnElement.IsAutoIncrement)
                    {
                        continue;
                    }

                    // Separate property initialization expressions with commas.
                    if (propertyInitialization.Count != 0)
                    {
                        propertyInitialization.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    // CountryCode = jObject.Value<string>("countryCode")
                    propertyInitialization.Add(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(columnElement.Name),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("jObject"),
                                    SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier("Value"))
                                    .WithTypeArgumentList(
                                        SyntaxFactory.TypeArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                Conversions.FromType(columnElement.Type))))))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(columnElement.Name.ToCamelCase()))))))));
                }

                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase()),
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList())
                            .WithInitializer(
                                SyntaxFactory.InitializerExpression(
                                    SyntaxKind.ObjectInitializerExpression,
                                    SyntaxFactory.SeparatedList<ExpressionSyntax>(propertyInitialization))))));

                //                        country.Enlist();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase()),
                                SyntaxFactory.IdentifierName("Enlist")))));

                //                        country.Lock.EnterWriteLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase()),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("EnterWriteLock")))));

                //                        using (TransactionScope additionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                //                        {
                //                            <AdditionTransaction>
                //                        }
                statements.Add(
                    SyntaxFactory.UsingStatement(
                        SyntaxFactory.Block(this.AdditionTransaction))
                    .WithDeclaration(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("TransactionScope"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("additionScope"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("TransactionScope"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("TransactionScopeAsyncFlowOption"),
                                                            SyntaxFactory.IdentifierName("Enabled"))))))))))));

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
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase())))))));

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
                string literal = this.uniqueKeyElement.Name.ToCamelCase();
                foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
                {
                    literal += $"/{{{columnReferenceElement.Column.Name.ToCamelCase()}}}";
                }

                string columnName = this.uniqueKeyElement.Columns[0].Column.Name.ToCamelCase();
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

                // This determines if any of the parent tables has an alternative way of referencing it than the primary key.  If so, the PUT verb
                // will allow for an additional key to identify the parent table.  This allows the REST operations to use external identifiers, such
                // as 'US' to reference a country rather than the internal (usually integer) key.
                bool hasAdditionalParentKeys = false;
                foreach (ForeignKeyElement parentKeyElement in this.uniqueKeyElement.Table.ParentKeys)
                {
                    foreach (UniqueKeyElement additionalUniqueKeyElement in parentKeyElement.UniqueKey.Table.UniqueKeys)
                    {
                        if (!additionalUniqueKeyElement.IsPrimaryKey)
                        {
                            hasAdditionalParentKeys = true;
                            break;
                        }
                    }
                }

                // Add a transaction to lookup external identifiers (additional unique keys) in the parent tables.
                if (hasAdditionalParentKeys)
                {
                    //            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    //            {
                    //                <ParentKeyTransaction>
                    //            }
                    statements.Add(
                        SyntaxFactory.UsingStatement(
                            SyntaxFactory.Block(this.ParentKeyTransaction))
                        .WithDeclaration(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName("TransactionScope"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier("transactionScope"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.ObjectCreationExpression(
                                                SyntaxFactory.IdentifierName("TransactionScope"))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.IdentifierName("TransactionScopeAsyncFlowOption"),
                                                                SyntaxFactory.IdentifierName("Enabled"))))))))))));
                }

                //            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                //            {
                //                <WriteTransaction>
                //            }
                statements.Add(
                    SyntaxFactory.UsingStatement(
                        SyntaxFactory.Block(this.WriteTransaction))
                    .WithDeclaration(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("TransactionScope"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("transactionScope"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("TransactionScope"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("TransactionScopeAsyncFlowOption"),
                                                            SyntaxFactory.IdentifierName("Enabled"))))))))))));

                //             return this.Ok();
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Ok")))));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<CatchClauseSyntax> CatchClauses
        {
            get
            {
                // The catch clauses are collected in this list.
                List<CatchClauseSyntax> clauses = new List<CatchClauseSyntax>();

                //            catch (DbUpdateException)
                //            {
                //                <HandleException>
                //            }
                clauses.Add(
                    SyntaxFactory.CatchClause()
                    .WithDeclaration(
                        SyntaxFactory.CatchDeclaration(
                            SyntaxFactory.IdentifierName("DbUpdateException")))
                    .WithBlock(
                        SyntaxFactory.Block(this.HandleException)));

                //            catch (FormatException)
                //            {
                //                <HandleException>
                //            }
                clauses.Add(
                    SyntaxFactory.CatchClause()
                    .WithDeclaration(
                        SyntaxFactory.CatchDeclaration(
                            SyntaxFactory.IdentifierName("FormatException")))
                    .WithBlock(
                        SyntaxFactory.Block(this.HandleException)));

                // This is the collection of catch clauses.
                return SyntaxFactory.List<CatchClauseSyntax>(clauses);
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
                //        /// Put the <see cref="Country"/> record into the domain.
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
                                                $" Put the <see cref=\"{this.uniqueKeyElement.Table.Name}\"/> record into the domain.",
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

                //        /// <param name="countryCode">The external identifier for the record.</param>
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
                                                        SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")),
                                                        $" <param name=\"{columnElement.Name.ToCamelCase()}\">The {columnElement.Name} identifier.</param>",
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        Environment.NewLine,
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList())
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
                                                        SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")),
                                                        $" <param name=\"jObject\">The JSON record.</param>",
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        Environment.NewLine,
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList())
                                            }))))));

                //        /// <returns>The result of the PUT action.</returns>
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
                                                        $" <returns>The result of the PUT action.</returns>",
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
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> FinallyBlock1
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.domain.Countries.CountryCountryCodeKey.Lock.ExitReadLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("ExitReadLock")))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }

        /// <summary>
        /// Gets a block of code to handle any communication exceptions.
        /// </summary>
        private List<StatementSyntax> HandleException
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    return this.BadRequest();
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("BadRequest")))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private SyntaxTokenList Modifiers
        {
            get
            {
                // public async
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.AsyncKeyword)
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
                                SyntaxFactory.Identifier(columnReferenceElement.Column.Name.ToCamelCase()))
                            .WithAttributeLists(
                                SyntaxFactory.SingletonList<AttributeListSyntax>(
                                    SyntaxFactory.AttributeList(
                                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                            SyntaxFactory.Attribute(
                                                SyntaxFactory.IdentifierName("FromRoute"))))))
                            .WithType(
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.StringKeyword))));
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
        private List<StatementSyntax> ParentKeyTransaction
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // We want to find all the relations to parent records in order to pull apart the JSON structure.  We can address different
                // indices using different parameters in the JSON. (e.g. "countryCode" will reference the CountryCode
                Dictionary<TableElement, List<UniqueKeyElement>> parentRelations = new Dictionary<TableElement, List<UniqueKeyElement>>();
                foreach (ForeignKeyElement parentKeyElement in this.uniqueKeyElement.Table.ParentKeys)
                {
                    foreach (UniqueKeyElement additionalUniqueKeyElement in parentKeyElement.UniqueKey.Table.UniqueKeys)
                    {
                        if (!additionalUniqueKeyElement.IsPrimaryKey)
                        {
                            if (!parentRelations.TryGetValue(parentKeyElement.UniqueKey.Table, out List<UniqueKeyElement> uniqueKeyElements))
                            {
                                uniqueKeyElements = new List<UniqueKeyElement>();
                                parentRelations.Add(parentKeyElement.UniqueKey.Table, uniqueKeyElements);
                            }

                            uniqueKeyElements.Add(additionalUniqueKeyElement);
                        }
                    }
                }

                foreach (TableElement tableElement in parentRelations.Keys)
                {
                    //                Country country = null;
                    statements.Add(
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(tableElement.Name))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier(tableElement.Name.ToCamelCase()))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.NullLiteralExpression)))))));

                    foreach (UniqueKeyElement additionalUniqueKeyElement in parentRelations[tableElement])
                    {
                        //                var countryCountryCodeKey = jObject.GetValue("countryCountryCodeKey");
                        statements.Add(
                            SyntaxFactory.LocalDeclarationStatement(
                                SyntaxFactory.VariableDeclaration(
                                    SyntaxFactory.IdentifierName("var"))
                                .WithVariables(
                                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        SyntaxFactory.VariableDeclarator(
                                            SyntaxFactory.Identifier(additionalUniqueKeyElement.Name.ToCamelCase()))
                                        .WithInitializer(
                                            SyntaxFactory.EqualsValueClause(
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("jObject"),
                                                        SyntaxFactory.IdentifierName("GetValue")))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.LiteralExpression(
                                                                    SyntaxKind.StringLiteralExpression,
                                                                    SyntaxFactory.Literal(additionalUniqueKeyElement.Name.ToCamelCase()))))))))))));

                        //                if (countryCountryCodeKey != null)
                        //                {
                        //                    <FindRecord>
                        //                }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.IdentifierName(additionalUniqueKeyElement.Name.ToCamelCase()),
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)),
                                SyntaxFactory.Block(this.GetFindRecord(additionalUniqueKeyElement))));

                        //                if (country != null)
                        //                {
                        //                    <LockRecord>
                        //                }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.IdentifierName(tableElement.Name.ToCamelCase()),
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)),
                                SyntaxFactory.Block(this.GetLockRecord(tableElement))));
                    }
                }

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> TryBlock1
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    await this.domain.Countries.CountryCountryCodeKey.Lock.EnterReadLockAsync(this.lockTimeout);
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
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("Lock")),
                                    SyntaxFactory.IdentifierName("EnterReadLockAsync")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName("lockTimeout")))))))));

                //                    country = this.domain.Countries.CountryCountryCodeKey.Find(countryCode);
                List<SyntaxNodeOrToken> tupleArguments = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
                {
                    if (tupleArguments.Count != 0)
                    {
                        tupleArguments.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    tupleArguments.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name.ToCamelCase())));
                }

                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase()),
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
                                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Find")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.TupleExpression(
                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(tupleArguments)))))))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> TryBlock2
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.domain.Countries.Enlist();
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
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Enlist")))));

                //                    this.domain.Countries.CountryKey.Enlist();
                foreach (UniqueKeyElement innerUniqueKeyElement in this.uniqueKeyElement.Table.UniqueKeys)
                {
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
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
                                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                        SyntaxFactory.IdentifierName(innerUniqueKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Enlist")))));
                }

                //                    this.domain.Buyers.CountryBuyerCountryIdKey.Enlist();
                foreach (ForeignKeyElement parentKeyElement in this.uniqueKeyElement.Table.ParentKeys)
                {
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
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
                                                SyntaxFactory.IdentifierName(parentKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                            SyntaxFactory.IdentifierName(parentKeyElement.UniqueKey.Table.Name.ToPlural())),
                                        SyntaxFactory.IdentifierName(parentKeyElement.UniqueKey.Name)),
                                    SyntaxFactory.IdentifierName("Enlist")))));
                }

                //                    this.domain.Countries.Lock.EnterWriteLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
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
                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("EnterWriteLock")))));

                //                    this.domain.Countries.CountryKey.Lock.EnterWriteLock();
                foreach (UniqueKeyElement innerUniqueKeyElement in this.uniqueKeyElement.Table.UniqueKeys)
                {
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(innerUniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                                SyntaxFactory.IdentifierName(innerUniqueKeyElement.Table.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName(innerUniqueKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("Lock")),
                                    SyntaxFactory.IdentifierName("EnterWriteLock")))));
                }

                //                    this.domain.Countries.CountryBuyerCountryIdKey.Lock.EnterReadLock();
                foreach (ForeignKeyElement parentKeyElement in this.uniqueKeyElement.Table.ParentKeys)
                {
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(parentKeyElement.UniqueKey.XmlSchemaDocument.Name.ToCamelCase())),
                                                SyntaxFactory.IdentifierName(parentKeyElement.UniqueKey.Table.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName(parentKeyElement.UniqueKey.Name)),
                                        SyntaxFactory.IdentifierName("Lock")),
                                    SyntaxFactory.IdentifierName("EnterReadLock")))));
                }

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
                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase()),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(this.AddRecord))
                    .WithElse(
                        SyntaxFactory.ElseClause(
                            SyntaxFactory.Block(this.UpdateRecord))));

                //                    transactionScope.Complete();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("transactionScope"),
                                SyntaxFactory.IdentifierName("Complete")))));

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

                //                        country.Enlist();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase()),
                                SyntaxFactory.IdentifierName("Enlist")))));

                //                        country.Lock.EnterWriteLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase()),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("EnterWriteLock")))));

                //                country.CountryCode = jObject.Value<string>("countryCode");
                //                country.Name = jObject.Value<string>("name");
                foreach (ColumnElement columnElement in this.uniqueKeyElement.Table.Columns)
                {
                    // Don't attempt to insert these kinds of properties.
                    if (columnElement.IsRowVersion || columnElement.IsAutoIncrement)
                    {
                        continue;
                    }

                    //                        country.CountryCode = jObject.Value<string>("countryCode");
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase()),
                                    SyntaxFactory.IdentifierName(columnElement.Name)),
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("jObject"),
                                        SyntaxFactory.GenericName(
                                            SyntaxFactory.Identifier("Value"))
                                        .WithTypeArgumentList(
                                            SyntaxFactory.TypeArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                    Conversions.FromType(columnElement.Type))))))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal(columnElement.Name.ToCamelCase())))))))));
                }

                //                        using (TransactionScope updateScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                //                        {
                //                            <UpdateTransaction>
                //                        }
                statements.Add(
                    SyntaxFactory.UsingStatement(
                        SyntaxFactory.Block(this.UpdateTransaction))
                    .WithDeclaration(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("TransactionScope"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("updateScope"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("TransactionScope"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("TransactionScopeAsyncFlowOption"),
                                                            SyntaxFactory.IdentifierName("Enabled"))))))))))));

                //                        this.domain.Countries.Update(country);
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
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                     SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                               SyntaxFactory.IdentifierName("Update")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase())))))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> UpdateTransaction
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                            this.domainContext.Countries.Update(country);
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
                                        SyntaxFactory.IdentifierName($"{this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Update")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToCamelCase())))))));

                //                            await this.domainContext.SaveChangesAsync();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName($"{this.uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase()}Context")),
                                    SyntaxFactory.IdentifierName("SaveChangesAsync"))))));

                //                            updateScope.Complete();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("updateScope"),
                                SyntaxFactory.IdentifierName("Complete")))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> WriteTransaction
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                Country country = null;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(this.uniqueKeyElement.Table.Name.ToCamelCase()))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NullLiteralExpression)))))));

                //                try
                //                {
                //                    <FindCountry>
                //                }
                //                finally
                //                {
                //                    <ReleaseLock>
                //                }
                statements.Add(
                    SyntaxFactory.TryStatement()
                    .WithBlock(
                        SyntaxFactory.Block(this.TryBlock1))
                    .WithFinally(
                        SyntaxFactory.FinallyClause(
                            SyntaxFactory.Block(this.FinallyBlock1))));

                //            try
                //            {
                //                <TryBlock1>
                //            }
                //            catch
                //            {
                //                <CatchClauses>
                //            }
                statements.Add(
                    SyntaxFactory.TryStatement(this.CatchClauses)
                    .WithBlock(
                        SyntaxFactory.Block(this.TryBlock2)));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique key element.</param>
        /// <returns>A block of statements.</returns>
        private SyntaxList<StatementSyntax> GetFindRecord(UniqueKeyElement uniqueKeyElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                    string countryCountryCodeKeyCountryCode = countryCountryCodeKey.Value<string>("countryCode");
            foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.StringKeyword)))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(uniqueKeyElement.Name.ToCamelCase() + columnElement.Name))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name.ToCamelCase()),
                                                SyntaxFactory.GenericName(
                                                    SyntaxFactory.Identifier("Value"))
                                                .WithTypeArgumentList(
                                                    SyntaxFactory.TypeArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                            SyntaxFactory.PredefinedType(
                                                                SyntaxFactory.Token(SyntaxKind.StringKeyword)))))))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal(columnElement.Name.ToCamelCase()))))))))))));
            }

            //                    await this.domain.Countries.CountryKey.Lock.EnterReadLockAsync(this.lockTimeout);
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
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName(uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                            SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name.ToPlural())),
                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("EnterReadLockAsync")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("lockTimeout")))))))));

            //                    country = this.domain.Countries.CountryCountryCodeKey.Find(countryCountryCodeKeyCountryCode);
            //                    region = this.domain.Regions.RegionExternalKey.Find((regionExternalKeyName, regionExternalKeyCountryCode));
            SeparatedSyntaxList<ArgumentSyntax> findParameters;
            if (uniqueKeyElement.Columns.Count == 1)
            {
                findParameters = SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                    SyntaxFactory.Argument(
                        SyntaxFactory.IdentifierName(uniqueKeyElement.Name.ToCamelCase() + uniqueKeyElement.Columns[0].Column.Name)));
            }
            else
            {
                List<SyntaxNodeOrToken> keys = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
                {
                    if (keys.Count != 0)
                    {
                        keys.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    keys.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(uniqueKeyElement.Name.ToCamelCase() + columnReferenceElement.Column.Name)));
                }

                findParameters = SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                    SyntaxFactory.Argument(
                        SyntaxFactory.TupleExpression(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(keys))));
            }

            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name.ToCamelCase()),
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
                                            SyntaxFactory.IdentifierName(uniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Find")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(findParameters)))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>A block of statements.</returns>
        private SyntaxList<StatementSyntax> GetLockRecord(TableElement tableElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                    await country.Lock.EnterReadLockAsync(this.lockTimeout);
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(tableElement.Name.ToCamelCase()),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("EnterReadLockAsync")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("lockTimeout")))))))));

            //                    jObject.Add("countryId", new JValue(country.CountryId));
            foreach (ColumnReferenceElement columnReferenceElement in tableElement.PrimaryKey.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("jObject"),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(columnElement.Name.ToCamelCase()))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.ObjectCreationExpression(
                                                SyntaxFactory.IdentifierName("JValue"))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.IdentifierName(columnElement.Table.Name.ToCamelCase()),
                                                                SyntaxFactory.IdentifierName(columnElement.Name)))))))
                                    })))));
            }

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}