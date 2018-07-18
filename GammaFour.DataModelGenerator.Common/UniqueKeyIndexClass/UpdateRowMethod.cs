// <copyright file="UpdateRowMethod.cs" company="Dark Bond, Inc.">
//    Copyright © 2016 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.DataModelGenerator.UniqueIndexClass
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
    public class UpdateRowMethod : SyntaxElement
    {
        /// <summary>
        /// The name of the row.
        /// </summary>
        private string rowVariable;

        /// <summary>
        /// The type of the row.
        /// </summary>
        private string rowType;

        /// <summary>
        /// The table schema.
        /// </summary>
        private UniqueConstraintSchema uniqueConstraintSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateRowMethod"/> class.
        /// </summary>
        /// <param name="uniqueConstraintSchema">The unique constraint schema.</param>
        public UpdateRowMethod(UniqueConstraintSchema uniqueConstraintSchema)
        {
            // Initialize the object.
            this.uniqueConstraintSchema = uniqueConstraintSchema;

            // This is the name of the method.
            this.Name = "UpdateRow";

            // The row type.
            this.rowType = string.Format(CultureInfo.InvariantCulture, "{0}Row", this.uniqueConstraintSchema.Table.Name);

            // The row name.
            this.rowVariable = string.Format(CultureInfo.InvariantCulture, "{0}Row", this.uniqueConstraintSchema.Table.CamelCaseName);

            //        /// <summary>
            //        /// Rejects the changes when updating a <see cref="ConfigurationRow"/> in the index.
            //        /// </summary>
            //        /// <param name="configurationRow">The updated <see cref="ConfigurationRow"/>.</param>
            //        internal void UpdateRow(ConfigurationRow configurationRow)
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
                string propertyName = this.uniqueConstraintSchema.Columns[0].Name;
                BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                    SyntaxKind.NotEqualsExpression,
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.rowVariable),
                            SyntaxFactory.IdentifierName("Previous"))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                        SyntaxFactory.IdentifierName(propertyName))
                    .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.rowVariable),
                            SyntaxFactory.IdentifierName("Current"))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                        SyntaxFactory.IdentifierName(propertyName))
                    .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)))
                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.ExclamationEqualsToken));

                // Continue to extend the binary expression with additional columns in the key.
                for (int index = 1; index < this.uniqueConstraintSchema.Columns.Count; index++)
                {
                    propertyName = this.uniqueConstraintSchema.Columns[index].Name;
                    expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.LogicalOrExpression,
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.rowVariable),
                                    SyntaxFactory.IdentifierName("Previous"))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.IdentifierName(propertyName))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.rowVariable),
                                    SyntaxFactory.IdentifierName("Current"))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.IdentifierName(propertyName))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.ExclamationEqualsToken)),
                        expression);
                }

                //            if (configurationRow.Previous.targetKeyField != configurationRow.Current.targetKeyField || configurationRow.Previous.configurationIdField != configurationRow.Current.configurationIdField)
                //            {
                //                <IfKeyChangedBlock>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        expression,
                        SyntaxFactory.Block(this.IfKeyChangedBlock)));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements.ToArray()));
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
                //        /// Handles the updating of a <see cref="ConfigurationRow"/> record.
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
                                                    " Handles the updating of a <see cref=\"{0}Row\"/> record.",
                                                    this.uniqueConstraintSchema.Table.Name),
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

                //        /// <param name="configurationRow">The Configuration row.</param>
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
                                                        this.uniqueConstraintSchema.Table.CamelCaseName,
                                                        this.uniqueConstraintSchema.Table.Name),
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
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> IfKeyChangedBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will ignore
                // an entry when it all of the components in the key are null.
                if (this.uniqueConstraintSchema.IsNullable)
                {
                    // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                    // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from here.
                    string propertyName = this.uniqueConstraintSchema.Columns[0].Name;
                    BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.rowVariable),
                                SyntaxFactory.IdentifierName("Previous"))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                            SyntaxFactory.IdentifierName(propertyName))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                        SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                        .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)));

                    // Combine multiple key elements with a logical 'AND' binary expression.
                    for (int index = 1; index < this.uniqueConstraintSchema.Columns.Count; index++)
                    {
                        propertyName = this.uniqueConstraintSchema.Columns[index].Name;
                        expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            expression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.rowVariable),
                                        SyntaxFactory.IdentifierName("Previous"))
                                    .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                    SyntaxFactory.IdentifierName(propertyName))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                                .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.LogicalAndExpression)));
                    }

                    //            if (customerRow.Previous.externalId0Field != null)
                    //            {
                    //                <IfKeyNullBlock>
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            expression,
                            SyntaxFactory.Block(this.RemoveRowBlock)));
                }
                else
                {
                    //            this.dictionary.Remove(new ConfigurationKey(configurationRow.Previous.configurationIdField, configurationRow.Previous.targetKeyField));
                    statements.AddRange(this.RemoveRowBlock);
                }

                // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will ignore an entry when it all
                // of the key components are null.
                if (this.uniqueConstraintSchema.IsNullable)
                {
                    // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The first column acts as the
                    // seed and the binary expressions are built up by a succession of logical 'AND' statements from here.
                    string propertyName = this.uniqueConstraintSchema.Columns[0].Name;
                    BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.rowVariable),
                                SyntaxFactory.IdentifierName("Current"))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                            SyntaxFactory.IdentifierName(propertyName))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                        SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                        .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)));

                    // Combine multiple key elements with a logical 'AND' binary expression.
                    for (int index = 1; index < this.uniqueConstraintSchema.Columns.Count; index++)
                    {
                        propertyName = this.uniqueConstraintSchema.Columns[index].Name;
                        expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            expression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.rowVariable),
                                        SyntaxFactory.IdentifierName("Current"))
                                    .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                    SyntaxFactory.IdentifierName(propertyName))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                                .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)))
                        .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.LogicalAndExpression)));
                    }

                    //            if (customerRow.Current.externalId0Field != null)
                    //            {
                    //                <RollbackDeleteRowBlock>
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            expression,
                            SyntaxFactory.Block(this.RollbackDeleteRowBlock)));
                }
                else
                {
                    //            this.Current.Add(new ConfigurationKey(configurationRow.Current.configurationIdField, configurationRow.Current.targetKeyField), configurationRow);
                    statements.AddRange(this.RollbackDeleteRowBlock);
                }

                // This is the complete block.
                return SyntaxFactory.List(statements.ToArray());
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
                        SyntaxFactory.Identifier(this.rowVariable))
                        .WithType(SyntaxFactory.IdentifierName(this.rowType)));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters.ToArray()));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> RemoveRowBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // This creates the comma-separated parameters that go into a key.
                List<SyntaxNodeOrToken> keyParameters = new List<SyntaxNodeOrToken>();
                foreach (ColumnSchema columnSchema in this.uniqueConstraintSchema.Columns)
                {
                    if (keyParameters.Count != 0)
                    {
                        keyParameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    string propertyName = columnSchema.Name;
                    keyParameters.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.rowVariable),
                                    SyntaxFactory.IdentifierName("Previous"))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.IdentifierName(propertyName))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken))));
                }

                //            this.Current.Remove(new ConfigurationKey(configurationRow.Current.configurationIdField, configurationRow.Current.targetKeyField));
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
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(this.uniqueConstraintSchema.Name))
                                            .WithNewKeyword(SyntaxFactory.Token(SyntaxKind.NewKeyword))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(keyParameters.ToArray()))
                                                .WithOpenParenToken(SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                                                .WithCloseParenToken(SyntaxFactory.Token(SyntaxKind.CloseParenToken))))
                                    }))
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
        private SyntaxList<StatementSyntax> RollbackDeleteRowBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // This creates the comma-separated parameters that go into a key.
                List<SyntaxNodeOrToken> keyParameters = new List<SyntaxNodeOrToken>();
                foreach (ColumnSchema columnSchema in this.uniqueConstraintSchema.Columns)
                {
                    if (keyParameters.Count != 0)
                    {
                        keyParameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    string propertyName = columnSchema.Name;
                    keyParameters.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.rowVariable),
                                    SyntaxFactory.IdentifierName("Current"))
                                .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)),
                                SyntaxFactory.IdentifierName(propertyName))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken))));
                }

                //                this.dictionary.Add(new ProvinceExternalId0Key(provinceRow.Original.ExternalId0), provinceRow);
                statements.Add(
                           SyntaxFactory.ExpressionStatement(
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
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.ObjectCreationExpression(
                                                        SyntaxFactory.IdentifierName(this.uniqueConstraintSchema.Name))
                                                    .WithArgumentList(
                                                        SyntaxFactory.ArgumentList(
                                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(keyParameters.ToArray())))),
                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName(this.rowVariable))
                                            })))));

                // This is the complete block.
                return SyntaxFactory.List(statements.ToArray());
            }
        }
    }
}