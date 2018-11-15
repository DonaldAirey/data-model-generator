// <copyright file="CreateMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.DataModelClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to start editing.
    /// </summary>
    public class CreateMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public CreateMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "Create" + tableElement.Name;

            //        /// <summary>
            //        /// Creates a Configuration record.
            //        /// </summary>
            //        /// <param name="configurationId">The required value for the ConfigurationId column.</param>
            //        /// <param name="sourceKey">The required value for the SourceKey column.</param>
            //        /// <param name="targetKey">The required value for the TargetKey column.</param>
            //        public void CreateConfiguration(string configurationId, string sourceKey, string targetKey)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.IdentifierName(this.tableElement.Name + "Row"),
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

                // Scrub the incoming data for any violations.
                foreach (ColumnElement columnElement in this.tableElement.Columns)
                {
                    // If this column doesn't allow nulls, and the argument is a null, then throw an exception.
                    if (!columnElement.IsNullable)
                    {
                        if (!columnElement.Type.GetTypeInfo().IsValueType)
                        {
                            statements.Add(
                                SyntaxFactory.IfStatement(
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.EqualsExpression,
                                        SyntaxFactory.IdentifierName(columnElement.Name.ToCamelCase()),
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NullLiteralExpression)),
                                    SyntaxFactory.Block(ThrowArgumentNullException.GetSyntax(columnElement.Name.ToCamelCase()))));
                        }
                    }
                }

                //            VolatileTransaction volatileTransaction = this.Transaction;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("VolatileTransaction"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("volatileTransaction"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("Transaction"))))))));

                // For constraint checking we need to be able to read the primary index of the parent tables to make sure the parent exists.
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ParentKeys)
                {
                    // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will
                    // ignore an entry when it all of the key components are null.
                    if (foreignKeyElement.IsNullable)
                    {
                        // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                        // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from
                        // here.
                        string columnParameter = foreignKeyElement.Columns[0].Column.Name.ToCamelCase();
                        BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(columnParameter),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                            .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)));

                        // Combine multiple key elements with a logical 'AND' binary expression.
                        for (int index = 1; index < foreignKeyElement.Columns.Count; index++)
                        {
                            columnParameter = foreignKeyElement.Columns[index].Column.Name;
                            expression = SyntaxFactory.BinaryExpression(
                                SyntaxKind.LogicalAndExpression,
                                expression,
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.IdentifierName(columnParameter),
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
                                SyntaxFactory.Block(this.LockParentPrimaryKeyBlock(foreignKeyElement))));
                    }
                    else
                    {
                        //            this.Table.CountryKey.AddReaderLock();
                        statements.AddRange(this.LockParentPrimaryKeyBlock(foreignKeyElement));
                    }
                }

                // Lock the unique index of all the non-null keys.
                foreach (UniqueKeyElement uniqueKeyElement in this.tableElement.UniqueKeys)
                {
                    // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will ignore an entry when it all
                    // of the key components are null.
                    if (uniqueKeyElement.IsNullable)
                    {
                        // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The first column acts as the
                        // seed and the binary expressions are built up by a succession of logical 'AND' statements from here.
                        string columnParameter = uniqueKeyElement.Columns[0].Column.Name.ToCamelCase();
                        BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(columnParameter),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                            .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)));

                        // Combine multiple key elements with a logical 'AND' binary expression.
                        for (int index = 1; index < uniqueKeyElement.Columns.Count; index++)
                        {
                            columnParameter = uniqueKeyElement.Columns[index].Column.Name;
                            expression = SyntaxFactory.BinaryExpression(
                                SyntaxKind.LogicalAndExpression,
                                expression,
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.IdentifierName(columnParameter),
                                    SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                                    .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.LogicalAndExpression)));
                        }

                        //            if (externalId0 != null)
                        //            {
                        //                <LockIndex>
                        //            }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                expression,
                                SyntaxFactory.Block(this.LockUniqueKeyIndex(uniqueKeyElement))));
                    }
                    else
                    {
                        //            this.ConfigurationKeyIndex.AddWriterLock(this.Timeout);
                        statements.AddRange(this.LockUniqueKeyIndex(uniqueKeyElement));
                    }
                }

                // Lock each of the non-null parent indices.
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ParentKeys)
                {
                    // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will ignore an entry when it all
                    // of the key components are null.
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
                                    SyntaxFactory.IdentifierName(columnElement.Name.ToCamelCase()),
                                    SyntaxFactory.IdentifierName("HasValue")) :
                                (ExpressionSyntax)SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.IdentifierName(columnElement.Name.ToCamelCase()),
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
                        //                <LockParentIndexBlock>
                        //            }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                previousIndexExpression,
                                SyntaxFactory.Block(
                                    this.LockParentIndexBlock(foreignKeyElement))));
                    }
                    else
                    {
                        //            this.Table.CountryProvinceCountryIdKey.AddWriterLock();
                        statements.AddRange(this.LockParentIndexBlock(foreignKeyElement));
                    }
                }

                //            this.Configuration.AddWriterLock(this.Timeout);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(this.tableElement.Name)),
                                SyntaxFactory.IdentifierName("AddWriterLock")))));

                //            CustomerData customerData = new CustomerData();
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

                // Copy the parameters into the row.
                foreach (ColumnElement columnElement in this.tableElement.Columns)
                {
                    if (!columnElement.IsAutoIncrement)
                    {
                        if (columnElement.IsRowVersion)
                        {
                            //            configurationData.RowVersion = this.IncrementRowVersion();
                            statements.Add(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Data"),
                                            SyntaxFactory.IdentifierName("RowVersion")),
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName("IncrementRowVersion"))))));
                        }
                        else
                        {
                            //            configurationData.ConfigurationId = configurationId;
                            statements.Add(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Data"),
                                            SyntaxFactory.IdentifierName(columnElement.Name)),
                                        SyntaxFactory.IdentifierName(columnElement.Name.ToCamelCase()))));
                        }
                    }
                }

                //            CustomerRow customerRow = new CustomerRow(this.Customer, customerData, true);
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
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.ThisExpression(),
                                                                SyntaxFactory.IdentifierName(this.tableElement.Name))),
                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Data")),
                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.TrueLiteralExpression))
                                                    })))))))));

                //            configurationRow.AddWriterLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                SyntaxFactory.IdentifierName("AddWriterLock")))));

                //            configurationRow.Add();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                SyntaxFactory.IdentifierName("Add")))));

                //            configurationRow.RowState = RowState.Added;
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
                                SyntaxFactory.IdentifierName("Added")))));

                //            volatileTransaction.AddActions(configurationRow.CommitAddRow, configurationRow.RollbackAddRow);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("volatileTransaction"),
                                SyntaxFactory.IdentifierName("AddActions")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                                SyntaxFactory.IdentifierName("CommitAdd"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                                SyntaxFactory.IdentifierName("RollbackAdd")))
                                    })))));

                // Only write to the database when the table is persistent.  Volatile tables, like ticker feeds, do not need to be written.
                if (this.tableElement.IsPersistent)
                {
                    // Collect the arguments for the persistent store method.
                    List<KeyValuePair<string, ArgumentSyntax>> arguments = new List<KeyValuePair<string, ArgumentSyntax>>();

                    // Add the columns of the table to the set of arguments.
                    foreach (ColumnElement columnElement in this.tableElement.Columns)
                    {
                        string identifierName = columnElement.Name;
                        arguments.Add(
                            new KeyValuePair<string, ArgumentSyntax>(
                                identifierName,
                                SyntaxFactory.Argument(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Data"),
                                        SyntaxFactory.IdentifierName(identifierName)))));
                    }

                    //            this.persistentStore.CreateCountry(countryRow.Abbreviation, countryRow.CountryId, countryRow.ExternalId0, countryRow.Name, countryRow.RowVersion);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("persistentStore")),
                                    SyntaxFactory.IdentifierName("Create" + this.tableElement.Name)))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments.OrderBy(a => a.Key).Select((kp) => kp.Value))))));
                }

                //            return configurationRow;
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row")));

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
                //        /// Creates a Configuration record.
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
                                                " Creates a " + this.tableElement.Name + " record.",
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

                // The trivia for the method header needs to sort the parameter comments in the same order they appear in the method declaration.
                List<KeyValuePair<string, SyntaxTrivia>> parameterTrivia = new List<KeyValuePair<string, SyntaxTrivia>>();

                // Add comments for each of the parameters.
                foreach (ColumnElement columnElement in this.tableElement.Columns)
                {
                    // The row's version is not part of the input for a create operation.
                    if (columnElement.IsRowVersion)
                    {
                        continue;
                    }

                    //        /// <param name="configurationId">The required value for the ConfigurationId column.</param>
                    string identifier = columnElement.Name.ToCamelCase();
                    string description = "The " + (columnElement.IsNullable ? "optional" : "required") + " value for the " + columnElement.Name + " column.";
                    parameterTrivia.Add(
                        new KeyValuePair<string, SyntaxTrivia>(
                            identifier,
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
                                                            " <param name=\"" + columnElement.Name.ToCamelCase() + "\">" + description + "</param>",
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList()),
                                                        SyntaxFactory.XmlTextNewLine(
                                                            SyntaxFactory.TriviaList(),
                                                            Environment.NewLine,
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList())
                                                    })))))));
                }

                // Add the ordered parameter trivia to the method header.
                comments.AddRange(parameterTrivia.OrderBy(pt => pt.Key).Select((kp) => kp.Value));

                //        /// <returns>The created row.</returns>
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
                                                    " <returns>The created row.</returns>",
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
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
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
                // Collect the parameters for this method.
                List<KeyValuePair<string, ParameterSyntax>> parameterPair = new List<KeyValuePair<string, ParameterSyntax>>();

                // Add a parameter for each of the columns in the table except the row version.
                foreach (ColumnElement columnElement in this.tableElement.Columns)
                {
                    if (!columnElement.IsRowVersion)
                    {
                        string identifier = columnElement.Name.ToCamelCase();
                        parameterPair.Add(
                            new KeyValuePair<string, ParameterSyntax>(
                                identifier,
                                SyntaxFactory.Parameter(
                                    SyntaxFactory.Identifier(identifier))
                                        .WithType(Conversions.FromType(columnElement.Type))));
                    }
                }

                // Alphabetically sort and separate the parameters.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();
                foreach (ParameterSyntax parameterSyntax in parameterPair.OrderBy(p => p.Key).Select((kp) => kp.Value))
                {
                    parameters.Add(parameterSyntax);
                }

                // string abbreviation, Guid countryId, string externalId0, string name
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="foreignKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code to add the row to the index.</returns>
        private SyntaxList<StatementSyntax> LockParentIndexBlock(ForeignKeyElement foreignKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //            this.CountryKey.AddReaderLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(foreignKeyElement.UniqueKey.Name)),
                            SyntaxFactory.IdentifierName("AddReaderLock")))));

            //            this.CountryCustomerCountryIdKey.AddWriterLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                            SyntaxFactory.IdentifierName("AddWriterLock")))));

            // Collect the arguments needed to find the record.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                arguments.Add(
                    columnElement.IsNullable && columnElement.IsValueType ?
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(columnElement.Name.ToCamelCase()),
                            SyntaxFactory.IdentifierName("Value"))) :
                    SyntaxFactory.Argument(
                        SyntaxFactory.IdentifierName(columnElement.Name.ToCamelCase())));
            }

            //            if (!this.DataModel.CountryKey.ContainsKey(countryId))
            //            {
            //                <ThrowConstraintException>
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
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(foreignKeyElement.UniqueKey.Name)),
                                SyntaxFactory.IdentifierName("ContainsKey")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))),
                    SyntaxFactory.Block(
                        ThrowConstraintException.GetSyntax("insert", foreignKeyElement.Name))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="foreignKeyElement">The relation schema.</param>
        /// <returns>A block of code to add the row to the index.</returns>
        private SyntaxList<StatementSyntax> LockParentPrimaryKeyBlock(ForeignKeyElement foreignKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //            this.CountryKey.AddReaderLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(foreignKeyElement.UniqueKey.Name)),
                            SyntaxFactory.IdentifierName("AddReaderLock")))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code to add the row to the index.</returns>
        private SyntaxList<StatementSyntax> LockUniqueKeyIndex(UniqueKeyElement uniqueKeyElement)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //            this.ConfigurationKeyIndex.AddWriterLock(this.Timeout);
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                            SyntaxFactory.IdentifierName("AddWriterLock")))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}