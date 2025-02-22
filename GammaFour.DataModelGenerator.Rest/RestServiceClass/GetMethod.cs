// <copyright file="GetMethod.cs" company="Gamma Four, Inc.">
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
    /// A method to create a record.
    /// </summary>
    public class GetMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly UniqueIndexElement uniqueKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMethod"/> class.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema.</param>
        public GetMethod(UniqueIndexElement uniqueKeyElement)
        {
            // Initialize the object.  Note that we decorate the name of every method that's not the primary key to prevent ambiguous signatures.
            this.uniqueKeyElement = uniqueKeyElement;
            this.Name = $"Get{this.uniqueKeyElement.Table.Name}";

            //        /// <summary>
            //        /// Gets a specific <see cref="Province"/> record.
            //        /// </summary>
            //        /// <param name="provinceId">The primary key identifier.</param>
            //        /// <returns>The result of the GET verb.</returns>
            //        [HttpGet("{provinceId}")]
            //        public async Task<IActionResult> GetProvince([FromRoute] int provinceId)
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
            .WithModifiers(GetMethod.Modifiers)
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
        /// Gets the data contract attribute syntax.
        /// </summary>
        private SyntaxList<AttributeListSyntax> Attributes
        {
            get
            {
                // This collects all the attributes.
                List<AttributeListSyntax> attributes = new List<AttributeListSyntax>();

                //        [HttpGet("{countryCode}")]
                string literal = string.Empty;
                foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
                {
                    if (!string.IsNullOrEmpty(literal))
                    {
                        literal += "/";
                    }

                    literal += $"{{{columnReferenceElement.Column.Name.ToCamelCase()}}}";
                }

                string columnName = this.uniqueKeyElement.Columns[0].Column.Name.ToVariableName();
                attributes.Add(
                    SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("HttpGet"))
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
                return SyntaxFactory.Block(statements);
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
                    //        /// Gets a specific <see cref="Province"/> record.
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
                                                $" Gets a specific <see cref=\"{this.uniqueKeyElement.Table.Name}\"/> record.",
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

                //        /// <param name="provinceId">The primary key identifier.</param>
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

                //        /// <returns>The result of the GET verb.</returns>
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
                                                        $" <returns>The result of the GET verb.</returns>",
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
                            .WithType(columnReferenceElement.Column.GetTypeSyntax()));
                }

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
                var statements = new List<StatementSyntax>
                {
                    //            using var lockingTransaction = new LockingTransaction(this.transactionTimeout);
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

                    //                await lockingTransaction.WaitReaderAsync(this.dataModel.ItemTrees.ItemTreeChildParentKey).ConfigureAwait(false);
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
                                                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.XmlSchemaDocument.DataModel.ToVariableName())),
                                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToPlural())),
                                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Name)))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))),

                    //                Province province = this.dataModel.Provinces.ProvinceKey.Find(provinceId);
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name))
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
                                            SyntaxFactory.ArgumentList(
                                                RestService.UniqueKeyExpression.GetSyntax(this.uniqueKeyElement)))))))),

                    //                if (province == null)
                    //                {
                    //                    return this.NotFound();
                    //                }
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name.ToVariableName()),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ReturnStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("NotFound"))))))),

                    //            await lockingTransaction.WaitReaderAsync(account).ConfigureAwait(false);
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
                                                SyntaxKind.FalseLiteralExpression))))))),

                    //                    return this.Ok(new { province.CountryCode, province.CountryId, province.Name, province.ProvinceId, province.RegionName, province.RegionId, province.Short, province.RowVersion });
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
                                        AnonymousRecordExpression.GetSyntax(this.uniqueKeyElement.Table)))))),
                };

                // This is the complete block.
                return statements;
            }
        }
    }
}