// <copyright file="MergeAddMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.TableClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to start editing.
    /// </summary>
    public class MergeAddMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// The XML Schema document.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="MergeAddMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public MergeAddMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.xmlSchemaDocument = this.tableElement.XmlSchemaDocument;
            this.Name = "MergeAdd";

            //        /// <summary>
            //        /// Merges a new record into the table.
            //        /// </summary>
            //        /// <param name="transactionItem">The raw data describing the transaction.</param>
            //        internal bool MergeAddition(object[] transactionItem)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithModifiers(this.Modifiers)
                .WithParameterList(this.ParameterList)
                .WithBody(this.Body)
                .WithLeadingTrivia(this.DocumentationComment);
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

                //            ConfigurationData configurationData = new ConfigurationData();
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.tableElement.Name + "Data"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(this.tableElement.Name.ToCamelCase() + "Data"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName(this.tableElement.Name + "Data"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList())))))));

                // Copy the data from the transaction record into the strongly typed record.
                foreach (ColumnElement columnElement in this.tableElement.Columns)
                {
                    //            this.currentData.ConfigurationId = (string)transactionItem[2];
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Data"),
                                    SyntaxFactory.IdentifierName(columnElement.Name)),
                                SyntaxFactory.CastExpression(
                                    Conversions.FromType(columnElement.Type),
                                    SyntaxFactory.ElementAccessExpression(
                                        SyntaxFactory.IdentifierName("transactionItem"))
                                    .WithArgumentList(
                                        SyntaxFactory.BracketedArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.NumericLiteralExpression,
                                                        SyntaxFactory.Literal(columnElement.Index + 2))))))))));
                }

                //            ConfigurationRow configurationRow = new ConfigurationRow(this.Configuration, configurationData, false);
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.tableElement.Name + "Row"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(this.tableElement.Name.ToCamelCase() + "Row"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName(this.tableElement.Name + "Row"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                        SyntaxFactory.Argument(
                                                                SyntaxFactory.ThisExpression()),
                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Data")),
                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.FalseLiteralExpression))
                                                    })))))))));

                // We need to guarantee that parent relations are merged into the data model before the children.  Here, we check for the existence
                // of the parent and skip the addition operation if the parent doesn't exist yet.
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ParentKeys)
                {
                    // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will
                    // ignore an entry when it all of the key components are null.
                    if (foreignKeyElement.IsNullable)
                    {
                        // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                        // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from
                        // here.
                        bool isFirstColumn = true;
                        ExpressionSyntax testIndexExpression = null;
                        for (int index = 0; index < foreignKeyElement.Columns.Count; index++)
                        {
                            ColumnElement columnElement = foreignKeyElement.Columns[0].Column;
                            if (!columnElement.IsNullable)
                            {
                                continue;
                            }

                            // Create an expression to test if the column is null.  Value types are implemented as the generic 'Nullable' and have a
                            // different syntax than reference types to test for null.
                            ExpressionSyntax testColumnExpression = columnElement.IsNullable && columnElement.IsValueType ?
                                (ExpressionSyntax)SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Data"),
                                        SyntaxFactory.IdentifierName(columnElement.Name)),
                                    SyntaxFactory.IdentifierName("HasValue")) :
                                (ExpressionSyntax)SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Data"),
                                        SyntaxFactory.IdentifierName(columnElement.Name)),
                                    SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                                    .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)));

                            if (isFirstColumn)
                            {
                                testIndexExpression = testColumnExpression;
                                isFirstColumn = false;
                            }
                            else
                            {
                                testIndexExpression = SyntaxFactory.BinaryExpression(
                                    SyntaxKind.LogicalAndExpression,
                                    testIndexExpression,
                                    testColumnExpression);
                            }
                        }

                        //            if (this.currentData.ExternalId0 != null)
                        //            {
                        //                <CheckParentKeyIndexBlock>
                        //            }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                testIndexExpression,
                                SyntaxFactory.Block(this.CheckParentKeyIndexBlock(foreignKeyElement))));
                    }
                    else
                    {
                        //            this.Table.CountryKeyIndex.Add(new CountryKey(this.currentData.CountryId), countryRow);
                        statements.AddRange(this.CheckParentKeyIndexBlock(foreignKeyElement));
                    }
                }

                //            licenseRow.Add();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                SyntaxFactory.IdentifierName("Add")))));

                //            licenseRow.RowState = RowState.Unchanged;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                SyntaxFactory.IdentifierName("RowState")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("RowState"),
                                SyntaxFactory.IdentifierName("Unchanged")))));

                //            return true;
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.TrueLiteralExpression)));

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
                //        /// Merges a new record into the table.
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
                                                " Merges a new record into the table.",
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

                //        /// <param name="transactionItem">The raw data describing the transaction.</param>
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
                                                        " <param name=\"transactionItem\">Raw data for an addition operation.</param>",
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
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword)
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
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();

                // object[] transactionItem
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("transactionItem"))
                    .WithType(
                        SyntaxFactory.ArrayType(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                        .WithRankSpecifiers(
                            SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                SyntaxFactory.ArrayRankSpecifier(
                                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                        SyntaxFactory.OmittedArraySizeExpression()))))));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="foreignKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code to add the row to the index.</returns>
        private SyntaxList<StatementSyntax> CheckParentKeyIndexBlock(ForeignKeyElement foreignKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the comma-separated list of parameters that are used to create a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.Columns)
            {
                // Each of the columns belonging to the key are added to the list.  Note that nullable value-type arguments must extract their value
                // first because they're going to be compared to the parent column which is not nullable.  We've also made sure by this time that
                // they value isn't null, so we don't need to check it before accessing the value.
                arguments.Add(
                    columnReferenceElement.Column.IsNullable && columnReferenceElement.Column.IsValueType ?
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Data"),
                                SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name)),
                            SyntaxFactory.IdentifierName("Value"))) :
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Data"),
                            SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
            }

            //            if (!this.DataModel.CountryKey.ContainsKey(customerData.CountryId))
            //            {
            //                return false;
            //            }
            statements.Add(
                SyntaxFactory.IfStatement(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name)),
                                    SyntaxFactory.IdentifierName(foreignKeyElement.UniqueKey.Name)),
                                SyntaxFactory.IdentifierName("ContainsKey")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))),
                    SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.FalseLiteralExpression)))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}