// <copyright file="AddMethod.cs" company="Gamma Four, Inc.">
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
    public class AddMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableSchema tableSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddMethod"/> class.
        /// </summary>
        /// <param name="tableSchema">The unique constraint schema.</param>
        public AddMethod(TableSchema tableSchema)
        {
            // Initialize the object.
            this.tableSchema = tableSchema;
            this.Name = "Add";

            //        /// <summary>
            //        /// Adds a row to the table.
            //        /// </summary>
            //        public void Add()
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

                //  Add each of the non-null keys to the parent indices.
                foreach (RelationSchema relationSchema in this.tableSchema.ParentRelations)
                {
                    // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will ignore an entry when it all
                    // of the key components are null.
                    if (relationSchema.ChildKeyConstraint.IsNullable)
                    {
                        // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                        // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from
                        // here.
                        bool isFirstColumn = true;
                        ExpressionSyntax testIndexExpression = null;
                        for (int index = 0; index < relationSchema.ChildKeyConstraint.Columns.Count; index++)
                        {
                            ColumnSchema columnSchema = relationSchema.ChildKeyConstraint.Columns[0];
                            if (!columnSchema.IsNullable)
                            {
                                continue;
                            }

                            // Create an expression to test if the column is null.  Value types are implemented as the generic 'Nullable' and have a
                            // different syntax than reference types to test for null.
                            ExpressionSyntax testColumnExpression = columnSchema.IsNullable && columnSchema.IsValueType ?
                                (ExpressionSyntax)SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("currentData")),
                                        SyntaxFactory.IdentifierName(columnSchema.Name)),
                                    SyntaxFactory.IdentifierName("HasValue")) :
                                (ExpressionSyntax)SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("currentData")),
                                        SyntaxFactory.IdentifierName(columnSchema.Name)),
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
                        //                <AddRowToIndexBlock>
                        //            }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                testIndexExpression,
                                SyntaxFactory.Block(this.AddParentKeyIndexBlock(relationSchema))));
                    }
                    else
                    {
                        //            this.Table.CountryKeyIndex.Add(new CountryKey(this.currentData.CountryId), countryRow);
                        statements.AddRange(this.AddParentKeyIndexBlock(relationSchema));
                    }
                }

                // Add each of the non-null keys to the unique indices.
                foreach (UniqueConstraintSchema uniqueConstraintSchema in this.tableSchema.UniqueKeys)
                {
                    // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will ignore an entry when it all
                    // of the key components are null.
                    if (uniqueConstraintSchema.IsNullable)
                    {
                        // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                        // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from
                        // here.
                        BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")),
                                SyntaxFactory.IdentifierName(uniqueConstraintSchema.Columns[0].Name)),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                            .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)));

                        // Combine multiple key elements with a logical 'AND' binary expression.
                        for (int index = 1; index < uniqueConstraintSchema.Columns.Count; index++)
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
                                        SyntaxFactory.IdentifierName(uniqueConstraintSchema.Columns[index].Name)),
                                    SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                                    .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.LogicalAndExpression)));
                        }

                        //            if (this.currentData.ExternalId0 != null)
                        //            {
                        //                <AddRowToIndexBlock>
                        //            }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                expression,
                                SyntaxFactory.Block(this.AddUniqueKeyIndexBlock(uniqueConstraintSchema))));
                    }
                    else
                    {
                        //            this.Table.CountryKeyIndex.Add(new CountryKey(this.currentData.CountryId), countryRow);
                        statements.AddRange(this.AddUniqueKeyIndexBlock(uniqueConstraintSchema));
                    }
                }

                // This creates the comma-separated list of parameters that are used to create a key.
                List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    // Each of the columns belonging to the key are added to the list.
                    arguments.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")),
                                SyntaxFactory.IdentifierName(columnSchema.Name))));
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

                //            this.Table.OnRowChanging(DataAction.Add, this);
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
                                                SyntaxFactory.IdentifierName("Add"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.ThisExpression())
                                    })))));

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
                //        /// Adds a row to the table.
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
                                                " Adds a row to the table.",
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
        /// <param name="relationSchema">The unique constraint schema.</param>
        /// <returns>A block of code to add the row to the index.</returns>
        private SyntaxList<StatementSyntax> AddParentKeyIndexBlock(RelationSchema relationSchema)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the comma-separated list of parameters that are used to create a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnSchema columnSchema in relationSchema.ChildKeyConstraint.Columns)
            {
                // Each of the columns belonging to the key are added to the list.  Note that nullable value-type arguments must extract their value
                // first because they're going to be compared to the parent column which is not nullable.  We've also made sure by this time that
                // they value isn't null, so we don't need to check it before accessing the value.
                arguments.Add(
                    columnSchema.IsNullable && columnSchema.IsValueType ?
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")),
                                SyntaxFactory.IdentifierName(columnSchema.Name)),
                            SyntaxFactory.IdentifierName("Value"))) :
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.IdentifierName(columnSchema.Name))));
            }

            // The current object is the final parameter.
            arguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

            //                this.Table.DataModel.CountryExternalId0KeyIndex.Add(this.currentData.ExternalId0, this);
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
                                    SyntaxFactory.IdentifierName("DataModel")),
                                SyntaxFactory.IdentifierName(relationSchema.Name)),
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
        /// <param name="uniqueConstraintSchema">The unique constraint schema.</param>
        /// <returns>A block of code to add the row to the index.</returns>
        private SyntaxList<StatementSyntax> AddUniqueKeyIndexBlock(UniqueConstraintSchema uniqueConstraintSchema)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // This creates the comma-separated list of parameters that are used to create a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnSchema columnSchema in uniqueConstraintSchema.Columns)
            {
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("currentData")),
                            SyntaxFactory.IdentifierName(columnSchema.Name))));
            }

            // The current object is the final parameter.
            arguments.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));

            //                this.Table.DataModel.CountryExternalId0KeyIndex.Add(this.currentData.ExternalId0, this);
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
                                    SyntaxFactory.IdentifierName("DataModel")),
                                SyntaxFactory.IdentifierName(uniqueConstraintSchema.Name)),
                            SyntaxFactory.IdentifierName("Add")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}