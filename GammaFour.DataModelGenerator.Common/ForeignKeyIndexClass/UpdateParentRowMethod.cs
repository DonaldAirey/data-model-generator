// <copyright file="UpdateParentRowMethod.cs" company="Dark Bond, Inc.">
//    Copyright © 2016 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.DataModelGenerator.ForeignIndexClass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to start editing.
    /// </summary>
    public class UpdateParentRowMethod : SyntaxElement
    {
        /// <summary>
        /// The parent key name.
        /// </summary>
        private string parentKeyName;

        /// <summary>
        /// The name of the row.
        /// </summary>
        private string rowName;

        /// <summary>
        /// The type of the row.
        /// </summary>
        private string rowType;

        /// <summary>
        /// The table schema.
        /// </summary>
        private RelationSchema relationSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateParentRowMethod"/> class.
        /// </summary>
        /// <param name="relationSchema">The unique constraint schema.</param>
        public UpdateParentRowMethod(RelationSchema relationSchema)
        {
            // Initialize the object.
            this.relationSchema = relationSchema;

            // This is the name of the method.
            this.Name = "UpdateParentRow";

            // The row type.
            this.rowType = string.Format(CultureInfo.InvariantCulture, "{0}Row", this.relationSchema.ParentTable.Name);

            // The row name.
            this.rowName = string.Format(CultureInfo.InvariantCulture, "{0}Row", this.relationSchema.ParentTable.CamelCaseName);

            // The parent key name.
            this.parentKeyName = this.relationSchema.ParentKeyConstraint.CamelCaseName;

            //        /// <summary>
            //        /// Updates a <see cref="CountryRow"/> parent relation.
            //        /// </summary>
            //        /// <param name="countryRow">The Country row.</param>
            //        internal void UpdateParentRow(CountryRow countryRow)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                SyntaxFactory.Identifier(this.Name))
                .WithModifiers(this.Modifiers)
                .WithParameterList(this.ParameterList)
                .WithBody(this.Body)
                .WithLeadingTrivia(this.DocumentionComment);
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

                // This will construct a series of binary expressions that will test to see if the original key is different than the current key.
                string propertyName = this.relationSchema.ParentKeyConstraint.Columns[0].Name;
                BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                    SyntaxKind.NotEqualsExpression,
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.rowName),
                            SyntaxFactory.IdentifierName("Previous"))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                        SyntaxFactory.IdentifierName(propertyName))
                    .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.rowName),
                            SyntaxFactory.IdentifierName("Current"))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                        SyntaxFactory.IdentifierName(propertyName))
                    .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)))
                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.ExclamationEqualsToken));

                // Continue to extend the binary expression with additional columns in the key.
                for (int index = 1; index < this.relationSchema.ParentKeyConstraint.Columns.Count; index++)
                {
                    propertyName = this.relationSchema.ParentKeyConstraint.Columns[index].Name;
                    expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.LogicalOrExpression,
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.rowName),
                                    SyntaxFactory.IdentifierName("Previous"))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.IdentifierName(propertyName))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.rowName),
                                    SyntaxFactory.IdentifierName("Current"))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.IdentifierName(propertyName))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.ExclamationEqualsToken)),
                        expression);
                }

                //            if (countryRow.Previous.CountryId != countryRow.Current.CountryId)
                //            {
                //                <CheckIndexBlock>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        expression,
                        SyntaxFactory.Block(this.CheckIndexBlock)));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements.ToArray()))
                    .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                    .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> CheckIndexBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // This creates the comma-separated list of parameters that are used to create a key.
                List<SyntaxNodeOrToken> keyParameters = new List<SyntaxNodeOrToken>();
                foreach (ColumnSchema columnSchema in this.relationSchema.ParentKeyConstraint.Columns)
                {
                    // Separate the parameters with a comma.
                    if (keyParameters.Count != 0)
                    {
                        keyParameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    // Each of the columns belonging to the key are added to the list.
                    string propertyName = columnSchema.Name;
                    keyParameters.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.rowName),
                                    SyntaxFactory.IdentifierName("Current"))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.IdentifierName(propertyName))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken))));
                }

                //                if (!this.dictionary.ContainsKey(new CountryKey(countryRow.Current.CountryId)))
                //                {
                //                    <ThrowExceptionBlock>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.PrefixUnaryExpression(
                            SyntaxKind.LogicalNotExpression,
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("dictionary")),
                                    SyntaxFactory.IdentifierName("ContainsKey")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.Name))
                                        .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(keyParameters.ToArray())))))))),
                        SyntaxFactory.Block(this.ThrowExceptionBlock)));

                // This is the complete block.
                return SyntaxFactory.List(statements.ToArray());
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentionComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <summary>
                //        /// Updates a <see cref="CountryRow"/> parent relation.
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
                                                string.Format(
                                                    CultureInfo.InvariantCulture,
                                                    " Updates a <see cref=\"{0}Row\"/> parent relation.",
                                                    this.relationSchema.ParentTable.Name),
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

                //        /// <param name="customerRow">The Customer row.</param>
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
                                                    string.Format(
                                                        CultureInfo.InvariantCulture,
                                                        " <param name=\"{0}Row\">The {1} row.</param>",
                                                        this.relationSchema.ParentTable.CamelCaseName,
                                                        this.relationSchema.ParentTable.Name),
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                                SyntaxFactory.XmlTextNewLine(
                                                    SyntaxFactory.TriviaList(),
                                                    Environment.NewLine,
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList())
                                            }))))));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments.ToArray());
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
                        SyntaxFactory.Token(SyntaxKind.InternalKeyword)
                   });
            }
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax ParameterList
        {
            get
            {
                // Create a list of parameters.
                List<SyntaxNodeOrToken> parameters = new List<SyntaxNodeOrToken>();

                // ConfigurationRow configurationRow
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier(this.rowName))
                        .WithType(SyntaxFactory.IdentifierName(this.rowType)));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters.ToArray()));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> ThrowExceptionBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                throw new ConstraintException("The index FK_Country_Customer_CountryId is not empty.");
                statements.Add(
                    SyntaxFactory.ThrowStatement(
                        SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName("ConstraintException"))
                        .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(
                                                string.Format(
                                                    CultureInfo.InvariantCulture,
                                                    "The index {0} is not empty.",
                                                    this.relationSchema.Name))))))
                            .WithOpenParenToken(SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                            .WithCloseParenToken(SyntaxFactory.Token(SyntaxKind.CloseParenToken))))
                    .WithThrowKeyword(SyntaxFactory.Token(SyntaxKind.ThrowKeyword))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                // This is the complete block.
                return SyntaxFactory.List(statements.ToArray());
            }
        }
    }
}