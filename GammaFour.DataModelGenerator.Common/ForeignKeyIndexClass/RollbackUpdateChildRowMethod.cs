// <copyright file="RollbackUpdateChildRowMethod.cs" company="Dark Bond, Inc.">
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
    public class RollbackUpdateChildRowMethod : SyntaxElement
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
        /// Initializes a new instance of the <see cref="RollbackUpdateChildRowMethod"/> class.
        /// </summary>
        /// <param name="relationSchema">The unique constraint schema.</param>
        public RollbackUpdateChildRowMethod(RelationSchema relationSchema)
        {
            // Initialize the object.
            this.relationSchema = relationSchema;

            // This is the name of the method.
            this.Name = "RollbackUpdateChildRow";

            // The row type.
            this.rowType = string.Format(CultureInfo.InvariantCulture, "{0}Row", this.relationSchema.ChildTable.Name);

            // The row name.
            this.rowName = string.Format(CultureInfo.InvariantCulture, "{0}Row", this.relationSchema.ChildTable.CamelCaseName);

            // The parent key name.
            this.parentKeyName = this.relationSchema.ParentKeyConstraint.CamelCaseName;

            //        /// <summary>
            //        /// Rejects the update of a <see cref="CustomerRow"/> child relation.
            //        /// </summary>
            //        /// <param name="customerRow">The Customer row.</param>
            //        internal void RollbackUpdateChildRow(CustomerRow customerRow)
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
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> AddToIndexBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    hashSet = new HashSet<CustomerRow>();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("hashSet"),
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.GenericName(SyntaxFactory.Identifier("HashSet"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                            SyntaxFactory.IdentifierName(this.rowType)))
                                    .WithLessThanToken(SyntaxFactory.Token(SyntaxKind.LessThanToken))
                                    .WithGreaterThanToken(SyntaxFactory.Token(SyntaxKind.GreaterThanToken))))
                            .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList()
                                .WithOpenParenToken(SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                                .WithCloseParenToken(SyntaxFactory.Token(SyntaxKind.CloseParenToken))))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.EqualsToken)))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                //                    this.dictionary.Add(countryKey, hashSet);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression()
                                    .WithToken(SyntaxFactory.Token(SyntaxKind.ThisKeyword)),
                                    SyntaxFactory.IdentifierName("dictionary"))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.IdentifierName("Add"))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.CamelCaseName)),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName("hashSet"))
                                    }))
                            .WithOpenParenToken(SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                            .WithCloseParenToken(SyntaxFactory.Token(SyntaxKind.CloseParenToken))))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                // This is the complete block.
                return SyntaxFactory.List(statements.ToArray());
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

                // This will construct a series of binary expressions that will test to see if the original key is different than the current key.
                string propertyName = this.relationSchema.ChildKeyConstraint.Columns[0].Name;
                BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                    SyntaxKind.NotEqualsExpression,
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.rowName),
                            SyntaxFactory.IdentifierName("Original"))
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
                for (int index = 1; index < this.relationSchema.ChildKeyConstraint.Columns.Count; index++)
                {
                    propertyName = this.relationSchema.ChildKeyConstraint.Columns[index].Name;
                    expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.LogicalOrExpression,
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.rowName),
                                    SyntaxFactory.IdentifierName("Original"))
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

                //            if (customerRow.Original.countryIdField != customerRow.Current.countryIdField)
                //            {
                //                <RemoveRowBlock>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        expression,
                        SyntaxFactory.Block(this.UpdateIndexBlock)));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements.ToArray()))
                    .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                    .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken));
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
                //        /// Rejects the update of a <see cref="CustomerRow"/> child relation.
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
                                                    " Rejects the update of a <see cref=\"{0}Row\"/> child relation.",
                                                    this.relationSchema.ChildTable.Name),
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
                                                        this.relationSchema.ChildTable.CamelCaseName,
                                                        this.relationSchema.ChildTable.Name),
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
        private SyntaxList<StatementSyntax> RemoveKeyBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                this.dictionary.Remove(countryKey);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression()
                                    .WithToken(SyntaxFactory.Token(SyntaxKind.ThisKeyword)),
                                    SyntaxFactory.IdentifierName("dictionary"))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.IdentifierName("Remove"))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName(this.parentKeyName))))
                            .WithOpenParenToken(SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                            .WithCloseParenToken(SyntaxFactory.Token(SyntaxKind.CloseParenToken))))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                // This is the complete block.
                return SyntaxFactory.List(statements.ToArray());
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> UpdateIndexBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // This creates the comma-separated list of parameters that are used to create a key.
                List<SyntaxNodeOrToken> keyParameters = new List<SyntaxNodeOrToken>();
                foreach (ColumnSchema columnSchema in this.relationSchema.ChildKeyConstraint.Columns)
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

                //                CountryKey countryKey = new CountryKey(customerRow.Original.countryIdField);
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.Name))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(this.parentKeyName))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.Name))
                                        .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(keyParameters.ToArray())))))))));

                //                HashSet<CustomerRow> hashSet = this.Original[countryKey];
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("HashSet"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(SyntaxFactory.IdentifierName(this.rowType)))
                                .WithLessThanToken(SyntaxFactory.Token(SyntaxKind.LessThanToken))
                                .WithGreaterThanToken(SyntaxFactory.Token(SyntaxKind.GreaterThanToken))))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("hashSet"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ElementAccessExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression()
                                                .WithToken(SyntaxFactory.Token(SyntaxKind.ThisKeyword)),
                                                SyntaxFactory.IdentifierName("dictionary"))
                                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)))
                                        .WithArgumentList(
                                            SyntaxFactory.BracketedArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName(this.parentKeyName))))
                                            .WithOpenBracketToken(SyntaxFactory.Token(SyntaxKind.OpenBracketToken))
                                            .WithCloseBracketToken(SyntaxFactory.Token(SyntaxKind.CloseBracketToken))))
                                    .WithEqualsToken(SyntaxFactory.Token(SyntaxKind.EqualsToken))))))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                //                hashSet.Remove(customerRow);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("hashSet"),
                                SyntaxFactory.IdentifierName("Remove")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName(this.rowName)))))));

                //                if (hashSet.Count == 0)
                //                {
                //                    <RemoveKeyBlock>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("hashSet"),
                                SyntaxFactory.IdentifierName("Count"))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(0)))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.EqualsEqualsToken)),
                        SyntaxFactory.Block(this.RemoveKeyBlock)));

                // This creates the comma-separated list of parameters that are used to create a key.
                keyParameters = new List<SyntaxNodeOrToken>();
                foreach (ColumnSchema columnSchema in this.relationSchema.ChildKeyConstraint.Columns)
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
                                    SyntaxFactory.IdentifierName("Original"))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.IdentifierName(propertyName))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken))));
                }

                //                countryKey = new CountryKey(provinceRow.Original.CountryId);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(this.parentKeyName),
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.Name))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(keyParameters.ToArray()))))));

                //                if (!this.dictionary.TryGetValue(countryKey, out hashSet))
                //                {
                //                    <AddToIndexBlock>
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
                                    SyntaxFactory.IdentifierName("TryGetValue")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName(this.parentKeyName)),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName("hashSet"))
                                            .WithRefOrOutKeyword(
                                                SyntaxFactory.Token(SyntaxKind.OutKeyword))
                                        })))),
                        SyntaxFactory.Block(this.AddToIndexBlock)));

                //                hashSet.Add(customerRow);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("hashSet"),
                                SyntaxFactory.IdentifierName("Add"))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName(this.rowName))))
                            .WithOpenParenToken(SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                            .WithCloseParenToken(SyntaxFactory.Token(SyntaxKind.CloseParenToken))))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                // This is the complete block.
                return SyntaxFactory.List(statements.ToArray());
            }
        }
    }
}