// <copyright file="EndUpdateMethod.cs" company="Gamma Four, Inc.">
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
    /// Creates a method to start editing.
    /// </summary>
    public class EndUpdateMethod : SyntaxElement
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
        /// Initializes a new instance of the <see cref="EndUpdateMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public EndUpdateMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.xmlSchemaDocument = this.tableElement.XmlSchemaDocument;

            // This is the name of the method.
            this.Name = "EndUpdate";

            //        /// <summary>
            //        /// Raises an event that indicates the row has been modified.
            //        /// </summary>
            //        public void EndEdit()
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
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            if (this.RowState == RowState.Modified)
                //            {
                //                <UpdateRow>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("RowState")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("RowState"),
                                SyntaxFactory.IdentifierName("Modified"))),
                        SyntaxFactory.Block(this.UpdateRow)));

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
                //        /// Raises an event that indicates the row has been modified.
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
                                                " Raises an event that indicates the row has been modified.",
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
        private SyntaxList<StatementSyntax> UpdateRow
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                ConfigurationData previousData = this.data[this.data.Count - 2];
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
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.ThisExpression(),
                                                                    SyntaxFactory.IdentifierName("data")),
                                                                SyntaxFactory.IdentifierName("Count")),
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.NumericLiteralExpression,
                                                                SyntaxFactory.Literal(2)))))))))))));

                // For every column that has changed that is associated with a parent index, update that index.
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ParentKeys)
                {
                    // This will construct a series of binary expressions that will test to see if the original key is different than the current key.
                    string propertyName = foreignKeyElement.Columns[0].Column.Name;
                    BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(propertyName)),
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.IdentifierName(propertyName)));

                    // Continue to extend the binary expression with additional columns in the key.
                    for (int index = 1; index < foreignKeyElement.Columns.Count; index++)
                    {
                        propertyName = foreignKeyElement.Columns[index].Column.Name;
                        expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalOrExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("previousData"),
                                    SyntaxFactory.IdentifierName(propertyName)),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("currentData")),
                                    SyntaxFactory.IdentifierName(propertyName))),
                            expression);
                    }

                    //                if (previousData.CountryId != this.currentData.CountryId)
                    //                {
                    //                    <IfKeyChangedBlock>
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

             //                this.Table.OnRowChanging(DataAction.Update, this);
            statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("Table")),
                                SyntaxFactory.IdentifierName("OnRowChanging")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("DataAction"),
                                                SyntaxFactory.IdentifierName("Update"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.ThisExpression())
                                    })))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
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

            //                        this.Table.DataModel.ProvinceCustomerProvinceIdKeyIndex.AddChild(this.currentData.ProvinceId, this);
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
                string propertyName = columnElement.Name;
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.IdentifierName(propertyName))));
            }

            // The current object is the final parameter.
            arguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

            //                        this.Table.DataModel.CountryExternalId0KeyIndex.Add(this.currentData.ExternalId0, this);
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
            if (foreignKeyElement.IsNullable)
            {
                // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from
                // here.
                bool isFirstColumn = true;
                ExpressionSyntax previousIndexExpression = null;
                for (int index = 0; index < foreignKeyElement.Columns.Count; index++)
                {
                    ColumnElement columnElement = foreignKeyElement.Columns[index].Column;
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
                for (int index = 0; index < foreignKeyElement.Columns.Count; index++)
                {
                    ColumnElement columnElement = foreignKeyElement.Columns[index].Column;
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

            // If the key is also the primary index then we need to change the physical order of the table as well as the index.
            if (uniqueKeyElement.IsPrimaryKey)
            {
                //                    this.currentData = this.data[this.data.Count - 2];
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
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(2))))))))));

                // This creates the comma-separated list of parameters that are used to create a key.
                List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryKey.Columns)
                {
                    ColumnElement columnElement = columnReferenceElement.Column;
                    arguments.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("previousData"),
                                SyntaxFactory.IdentifierName(columnElement.Name))));
                }

                //                    this.Table.RemoveRow(previousData.ConfigurationId, previousData.Source);
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
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));

                //                    this.currentData = this.data[this.data.Count - 1];
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
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(1))))))))));

                // This creates the comma-separated list of parameters that are used to create a key.
                List<ArgumentSyntax> currentArguments = new List<ArgumentSyntax>();
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryKey.Columns)
                {
                    // Each of the columns belonging to the key are added to the current key element list.
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

                // The current object is the final parameter.
                currentArguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

                //                    this.Table.AddRow(this.currentData.ConfigurationId, this.currentData.Source, this);
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
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(currentArguments)))));
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
                        SyntaxFactory.IdentifierName("previousData"),
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
                                SyntaxFactory.IdentifierName("previousData"),
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
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("currentData")),
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
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[index].Column.Name)),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression))
                    .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.LogicalAndExpression)));
                }

                //            if (this.current.externalId0Field != null)
                //            {
                //                <AddRowBlock>
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
                                SyntaxFactory.IdentifierName("previousData"),
                                SyntaxFactory.IdentifierName(columnElement.Name)),
                            SyntaxFactory.IdentifierName("Value"))) :
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(columnElement.Name))));
            }

            // The current object is the final parameter.
            arguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

            //                        this.Table.DataModel.ProvinceCustomerProvinceIdKeyIndex.RemoveChild(previousData.ProvinceId, this);
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
                string propertyName = columnElement.Name;
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(propertyName))));
            }

            //                        this.Table.DataModel.CountryExternalId0KeyIndex.Remove(previousData.ExternalId0);
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

            // This creates the comma-separated list of the old key parameters.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.Columns)
            {
                // Each of the columns belonging to the key are added to the previous key element list.
                ColumnElement columnElement = columnReferenceElement.Column;
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(columnElement.Name))));
            }

            // Add the new key parameters.
            foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.Columns)
            {
                // Each of the columns belonging to the key are added to the current key element list.
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

            // The current object is the final parameter.
            arguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

            //                    this.Table.DataModel.CountryCustomerCountryIdKeyIndex.UpdateChild(previousData.CountryId, this.currentData.CountryId, this);
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
                            SyntaxFactory.IdentifierName("previousData"),
                            SyntaxFactory.IdentifierName(columnElement.Name))));
            }

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

            //                    this.Table.DataModel.ConfigurationKeyIndex.Update(previousData.ConfigurationId, previousData.Source, this.currentData.ConfigurationId, this.currentData.Source);
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