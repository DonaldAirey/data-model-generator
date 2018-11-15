// <copyright file="RollbackUpdateMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RowClass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to acquire a reader lock.
    /// </summary>
    public class RollbackUpdateMethod : SyntaxElement
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
        /// Initializes a new instance of the <see cref="RollbackUpdateMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public RollbackUpdateMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.xmlSchemaDocument = this.tableElement.XmlSchemaDocument;
            this.Name = "RollbackUpdate";

            //        /// <summary>
            //        /// Rollback an updated row.
            //        /// </summary>
            //        private void RollbackUpdateRow()
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                SyntaxFactory.Identifier(this.Name))
                .WithModifiers(this.Modifiers)
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
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            ConfigurationData previousData = this.data[this.data.Count - this.actionIndex - 1];
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.tableElement.Name + "Data"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("previousData"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ElementAccessExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName("data")))
                                        .WithArgumentList(
                                            SyntaxFactory.BracketedArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.BinaryExpression(
                                                            SyntaxKind.SubtractExpression,
                                                            SyntaxFactory.BinaryExpression(
                                                                SyntaxKind.SubtractExpression,
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.ThisExpression(),
                                                                        SyntaxFactory.IdentifierName("data")),
                                                                    SyntaxFactory.IdentifierName("Count")),
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.ThisExpression(),
                                                                    SyntaxFactory.IdentifierName("actionIndex"))),
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.NumericLiteralExpression,
                                                                SyntaxFactory.Literal(1)))))))))))));

                //            this.currentData = this.data[this.data.Count - this.actionIndex];
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.ElementAccessExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("data")))
                            .WithArgumentList(
                                SyntaxFactory.BracketedArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.BinaryExpression(
                                                SyntaxKind.SubtractExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName("data")),
                                                    SyntaxFactory.IdentifierName("Count")),
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName("actionIndex"))))))))));

                // For every column that has changed that is associated with a parent index, update that index.
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ParentKeys)
                {
                    // This will construct a series of binary expressions that will test to see if the original key is different than the current key.
                    BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(foreignKeyElement.Columns[0].Column.Name)),
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.IdentifierName(foreignKeyElement.Columns[0].Column.Name)));

                    // Continue to extend the binary expression with additional columns in the key.
                    for (int index = 1; index < foreignKeyElement.Columns.Count; index++)
                    {
                        expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalOrExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("previousData"),
                                    SyntaxFactory.IdentifierName(foreignKeyElement.Columns[index].Column.Name)),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("currentData")),
                                    SyntaxFactory.IdentifierName(foreignKeyElement.Columns[index].Column.Name))),
                            expression);
                    }

                    //                if (previousData.CountryId != this.currentData.CountryId)
                    //                {
                    //                    <ChangeParentKeyIndexBlock>
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            expression,
                            SyntaxFactory.Block(this.ChangeParentKeyIndexBlock(foreignKeyElement))));
                }

                // For every column that has changed that is associated with a unique index, update that index.
                foreach (UniqueKeyElement uniqueKeyElement in this.tableElement.UniqueKeys)
                {
                    // This will construct a series of binary expressions that will test to see if the original key is different than the current key.
                    BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[0].Column.Name)),
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[0].Column.Name)));

                    // Continue to extend the binary expression with additional columns in the key.
                    for (int index = 1; index < uniqueKeyElement.Columns.Count; index++)
                    {
                        expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalOrExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("previousData"),
                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[index].Column.Name)),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("currentData")),
                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[index].Column.Name))),
                            expression);
                    }

                    //                if (previousData.Source != this.currentData.Source || previousData.ConfigurationId != this.currentData.ConfigurationId)
                    //                {
                    //                    <ChangeUniqueKeyIndexBlock>
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            expression,
                            SyntaxFactory.Block(this.ChangeUniqueKeyIndexBlock(uniqueKeyElement))));
                }

                //            if (this.actionIndex++ == this.data.Count - 1)
                //            {
                //                <LastAction>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.PostfixUnaryExpression(
                                SyntaxKind.PostIncrementExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("actionIndex"))),
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.SubtractExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("data")),
                                    SyntaxFactory.IdentifierName("Count")),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    SyntaxFactory.Literal(1)))),
                        SyntaxFactory.Block(this.LastAction)));

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
                // This is used to collect the trivia.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <summary>
                //        /// Rollback an updated row.
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
                                                " Rollback an updated row.",
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

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> LastAction
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                this.RowState = RowState.Unchanged;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("RowState")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("RowState"),
                                SyntaxFactory.IdentifierName("Unchanged")))));

                //                this.data.Clear();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("data")),
                                SyntaxFactory.IdentifierName("Clear")))));

                //                this.data.Add(this.currentData = previousData);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("data")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.AssignmentExpression(
                                            SyntaxKind.SimpleAssignmentExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName("currentData")),
                                            SyntaxFactory.IdentifierName("previousData"))))))));

                //                this.actionIndex = 1;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("actionIndex")),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(1)))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
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
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();

                // Guid transactionId
                parameters.Add(SyntaxFactory.Parameter(
                    SyntaxFactory.Identifier("transactionId")).WithType(SyntaxFactory.IdentifierName(typeof(Guid).Name)));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="foreignKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code to add the key to the index.</returns>
        private SyntaxList<StatementSyntax> AddParentKeyIndexBlock(ForeignKeyElement foreignKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the comma-separated parameters that go into a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.Columns)
            {
                arguments.Add(
                    columnReferenceElement.Column.IsNullable && columnReferenceElement.Column.IsValueType ?
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("previousData"),
                                SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name)),
                            SyntaxFactory.IdentifierName("Value"))) :
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
            }

            // The current object is the final parameter.
            arguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

            //                    this.Table.DataModel.ProvinceCustomerProvinceIdKeyIndex.RemoveChild(previousData.ProvinceId, this);
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
                                        SyntaxFactory.IdentifierName("Table")),
                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name)),
                                SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                            SyntaxFactory.IdentifierName("AddChild")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code to add the key to the index.</returns>
        private SyntaxList<StatementSyntax> AddUniqueKeyIndexBlock(UniqueKeyElement uniqueKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the comma-separated parameters that go into a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(columnElement.Name))));
            }

            // The current object is the final parameter.
            arguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

            //                    this.Table.DataModel.CountryExternalId0KeyIndex.Add(previousData.ExternalId0, this);
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
                                        SyntaxFactory.IdentifierName("Table")),
                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name)),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                            SyntaxFactory.IdentifierName("Add")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="foreignKeyElement">The relation schema.</param>
        /// <returns>A block of code that handles a changed key.</returns>
        private SyntaxList<StatementSyntax> ChangeParentKeyIndexBlock(ForeignKeyElement foreignKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will ignore an
            // entry when it all of the components in the key are null.
            if (foreignKeyElement.UniqueKey.IsNullable)
            {
                // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from
                // here.
                bool isFirstColumn = true;
                ExpressionSyntax previousIndexExpression = null;
                for (int index = 0; index < foreignKeyElement.UniqueKey.Columns.Count; index++)
                {
                    ColumnElement columnElement = foreignKeyElement.UniqueKey.Columns[index].Column;
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
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")),
                                SyntaxFactory.IdentifierName(columnElement.Name)),
                            SyntaxFactory.IdentifierName("HasValue")) :
                        (ExpressionSyntax)SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")),
                                SyntaxFactory.IdentifierName(columnElement.Name)),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                            .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)));

                    if (isFirstColumn)
                    {
                        previousIndexExpression = testColumnExpression;
                        isFirstColumn = false;
                    }
                    else
                    {
                        previousIndexExpression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            previousIndexExpression,
                            testColumnExpression);
                    }
                }

                //            if (this.previous.externalId0Field != null)
                //            {
                //                <RemoveRowBlock>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        previousIndexExpression,
                        SyntaxFactory.Block(this.RemoveParentKeyIndexBlock(foreignKeyElement))));

                // Create a test expression for the current index.
                isFirstColumn = true;
                ExpressionSyntax currentIndexExpression = null;
                for (int index = 0; index < foreignKeyElement.UniqueKey.Columns.Count; index++)
                {
                    ColumnElement columnElement = foreignKeyElement.UniqueKey.Columns[index].Column;
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
                                SyntaxFactory.IdentifierName("previousData"),
                                SyntaxFactory.IdentifierName(columnElement.Name)),
                            SyntaxFactory.IdentifierName("HasValue")) :
                        (ExpressionSyntax)SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("previousData")),
                                SyntaxFactory.IdentifierName(columnElement.Name)),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                            .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)));

                    if (isFirstColumn)
                    {
                        currentIndexExpression = testColumnExpression;
                        isFirstColumn = false;
                    }
                    else
                    {
                        currentIndexExpression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            currentIndexExpression,
                            testColumnExpression);
                    }
                }

                //            if (this.current.externalId0Field != null)
                //            {
                //                <AddRowBlock>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        currentIndexExpression,
                        SyntaxFactory.Block(this.AddParentKeyIndexBlock(foreignKeyElement))));
            }
            else
            {
                //            this.dictionary.Update(new ConfigurationKey(this.previousData.configurationIdField, configurationRow.previousData.targetKeyField), new ConfigurationKey(configurationRow.currentData.configurationIdField, configurationRow.currentData.targetKeyField));
                statements.AddRange(this.UpdateParentKeyIndexBlock(foreignKeyElement));
            }

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code that handles a changed key.</returns>
        private SyntaxList<StatementSyntax> ChangeUniqueKeyIndexBlock(UniqueKeyElement uniqueKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // If the unique index is also the primary key, then we need to change the physical order of the table as well.
            if (uniqueKeyElement.IsPrimaryKey)
            {
                // This creates the comma-separated list of parameters that are used to create a key.
                List<ArgumentSyntax> currentArguments = new List<ArgumentSyntax>();
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryKey.Columns)
                {
                    ColumnElement columnElement = columnReferenceElement.Column;
                    currentArguments.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")),
                                SyntaxFactory.IdentifierName(columnElement.Name))));
                }

                //                this.Table.RemoveRow(this.currentData.ConfigurationId, this.currentData.Source);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("Table")),
                                SyntaxFactory.IdentifierName("RemoveRow")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(currentArguments)))));

                // This creates the comma-separated list of parameters that are used to create a key.
                List<ArgumentSyntax> previousArguments = new List<ArgumentSyntax>();
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryKey.Columns)
                {
                    ColumnElement columnElement = columnReferenceElement.Column;
                    previousArguments.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("previousData"),
                                SyntaxFactory.IdentifierName(columnElement.Name))));
                }

                // The current object is the final parameter.
                previousArguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

                //                this.Table.AddRow(previousData.ConfigurationId, previousData.Source, this);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("Table")),
                                SyntaxFactory.IdentifierName("AddRow")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(previousArguments)))));
            }

            // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will ignore an
            // entry when it all of the components in the key are null.
            if (uniqueKeyElement.IsNullable)
            {
                // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The first
                // column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from here.
                BinaryExpressionSyntax previousExpression = SyntaxFactory.BinaryExpression(
                    SyntaxKind.NotEqualsExpression,
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("currentData")),
                        SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[0].Column.Name)),
                    SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));

                // Combine multiple key elements with a logical 'AND' binary expression.
                for (int index = 1; index < uniqueKeyElement.Columns.Count; index++)
                {
                    previousExpression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.LogicalAndExpression,
                        previousExpression,
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[index].Column.Name)),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression))
                    .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.LogicalAndExpression)));
                }

                //            if (this.previous.externalId0Field != null)
                //            {
                //                <RemoveRowBlock>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        previousExpression,
                        SyntaxFactory.Block(this.RemoveUniqueKeyIndexBlock(uniqueKeyElement))));

                // Now we need to construct the same expression for the current index.
                ExpressionSyntax currentExpression = SyntaxFactory.BinaryExpression(
                    SyntaxKind.NotEqualsExpression,
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("previousData"),
                        SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[0].Column.Name)),
                    SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));

                // Combine multiple key elements with a logical 'AND' binary expression.
                for (int index = 1; index < uniqueKeyElement.Columns.Count; index++)
                {
                    currentExpression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.LogicalAndExpression,
                        previousExpression,
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("previousData"),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[index].Column.Name)),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression))
                    .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.LogicalAndExpression)));
                }

                //            if (previousData.externalId0Field != null)
                //            {
                //                <AddUniqueKeyIndexBlock>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        currentExpression,
                        SyntaxFactory.Block(this.AddUniqueKeyIndexBlock(uniqueKeyElement))));
            }
            else
            {
                //                    this.Table.CustomerKeyIndex.Update(new CustomerKey(previousData.CustomerId), new CustomerKey(this.currentData.CustomerId));
                statements.AddRange(this.UpdateUniqueKeyIndexBlock(uniqueKeyElement));
            }

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="foreignKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code to remove the key from the index.</returns>
        private SyntaxList<StatementSyntax> RemoveParentKeyIndexBlock(ForeignKeyElement foreignKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the comma-separated parameters that go into a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                arguments.Add(
                    columnElement.IsNullable && columnElement.IsValueType ?
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")),
                                SyntaxFactory.IdentifierName(columnElement.Name)),
                            SyntaxFactory.IdentifierName("Value"))) :
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.IdentifierName(columnElement.Name))));
            }

            // The current object is the final parameter.
            arguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

            //                    this.Table.DataModel.ProvinceCustomerProvinceIdKeyIndex.RemoveChild(this.currentData.ProvinceId, this);
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
                                        SyntaxFactory.IdentifierName("Table")),
                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name)),
                                SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                            SyntaxFactory.IdentifierName("RemoveChild")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code to remove the key from the index.</returns>
        private SyntaxList<StatementSyntax> RemoveUniqueKeyIndexBlock(UniqueKeyElement uniqueKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the comma-separated parameters that go into a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.IdentifierName(columnElement.Name))));
            }

            //                    this.Table.DataModel.CountryExternalId0KeyIndex.Remove(this.currentData.ExternalId0);
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
                                        SyntaxFactory.IdentifierName("Table")),
                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name)),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                            SyntaxFactory.IdentifierName("Remove")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="foreignKeyElement">The relation schema</param>
        /// <returns>A block of code that updates an index.</returns>
        private SyntaxList<StatementSyntax> UpdateParentKeyIndexBlock(ForeignKeyElement foreignKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the comma-separated current key, which appears first in the list of parameters.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.UniqueKey.Columns)
            {
                // Each of the columns belonging to the key are added to the current key element list.
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
            }

            // This creates the previous key, which appears after the current key.
            foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.UniqueKey.Columns)
            {
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
            }

            // The current object is the final parameter.
            arguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

            //                this.Table.DataModel.CountryCustomerCountryIdKeyIndex.UpdateChild(this.currentData.CountryId, previousData.CountryId, this);
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
                                        SyntaxFactory.IdentifierName("Table")),
                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name)),
                                SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                            SyntaxFactory.IdentifierName("UpdateChild")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The relation schema</param>
        /// <returns>A block of code that updates an index.</returns>
        private SyntaxList<StatementSyntax> UpdateUniqueKeyIndexBlock(UniqueKeyElement uniqueKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the list of parameters for updating the unique index from the old to the new values.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.IdentifierName(columnElement.Name))));
            }

            foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(columnElement.Name))));
            }

            //                this.Table.DataModel.ConfigurationKeyIndex.Update(this.currentData.ConfigurationId, this.currentData.Source, previousData.ConfigurationId, previousData.Source);
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
                                        SyntaxFactory.IdentifierName("Table")),
                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name)),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                            SyntaxFactory.IdentifierName("Update")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}