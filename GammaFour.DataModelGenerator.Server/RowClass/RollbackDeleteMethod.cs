// <copyright file="RollbackDeleteMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RowClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to acquire a reader lock.
    /// </summary>
    public class RollbackDeleteMethod : SyntaxElement
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
        /// Initializes a new instance of the <see cref="RollbackDeleteMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public RollbackDeleteMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.xmlSchemaDocument = this.tableElement.XmlSchemaDocument;
            this.Name = "RollbackDelete";

            //        /// <summary>
            //        /// Rollback a deleted row.
            //        /// </summary>
            //        private void RollbackDeleteRow()
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
        /// Gets a block of code.
        /// </summary>
        /// <returns>A block of code that removes an entry in a unique index.</returns>
        private SyntaxList<StatementSyntax> AddRowBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // This creates the comma-separated list of parameters that are used to create a key.
                List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryKey.Columns)
                {
                    // Each of the columns belonging to the key are added to the list.
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

                //            this.Table.AddRow(this.current.CountryId, this);
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
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
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

                // Rollback the addition for each of the unique constraints.
                foreach (UniqueKeyElement uniqueKeyElement in this.tableElement.UniqueKeys)
                {
                    // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will ignore
                    // an entry when it all of the components in the key are null.
                    if (uniqueKeyElement.IsNullable)
                    {
                        // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                        // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from here.
                        BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[0].Column.Name)),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression));

                        // Combine multiple key elements with a logical 'AND' binary expression.
                        for (int index = 1; index < uniqueKeyElement.Columns.Count; index++)
                        {
                            expression = SyntaxFactory.BinaryExpression(
                                SyntaxKind.LogicalAndExpression,
                                expression,
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("currentData")),
                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[index].Column.Name)),
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)));
                        }

                        //            if (customerRow.Current.externalId0Field != null)
                        //            {
                        //                <IfKeyNullBlock>
                        //            }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                expression,
                                SyntaxFactory.Block(this.AddUniqueIndexBlock(uniqueKeyElement))));
                    }
                    else
                    {
                        statements.AddRange(this.AddUniqueIndexBlock(uniqueKeyElement));
                    }
                }

                // Delete the row from each of the parent relations.
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

                        //            if (customerRow.Current.externalId0Field != null)
                        //            {
                        //                <DeleteFromUniqueKeyIndexBlock>
                        //            }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                testIndexExpression,
                                SyntaxFactory.Block(this.AddParentKeyIndexBlock(foreignKeyElement))));
                    }
                    else
                    {
                        //                this.Table.CountryExternalId0KeyIndex.Remove(new CountryExternalId0Key(this.Current.ExternalId0));
                        statements.AddRange(this.AddParentKeyIndexBlock(foreignKeyElement));
                    }
                }

                // Put the row back into the table.
                statements.AddRange(this.AddRowBlock);

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
                //        /// Rollback a deleted row.
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
                                                " Rollback a deleted row.",
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
        /// Gets a block of code.
        /// </summary>
        /// <param name="foreignKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code that deletes a row from a unique index.</returns>
        private SyntaxList<StatementSyntax> AddParentKeyIndexBlock(ForeignKeyElement foreignKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the comma-separated list of parameters that are used to create a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.Columns)
            {
                // Each of the columns belonging to the key are added to the list.
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

            //            this.Table.DataModel.CountryProvinceKeyIndex.AddChild(this.current.CountryId, this);
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
        /// <returns>A block of code that removes an entry in a unique index.</returns>
        private SyntaxList<StatementSyntax> AddUniqueIndexBlock(UniqueKeyElement uniqueKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the comma-separated list of parameters that are used to create a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
            {
                // Each of the columns belonging to the key are added to the list.
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

            //                this.Table.DataModel.CountryExternalId0KeyIndex.Add(previousData.ExternalId0, this);
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
    }
}