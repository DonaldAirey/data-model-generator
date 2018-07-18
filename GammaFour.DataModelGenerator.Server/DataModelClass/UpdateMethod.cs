// <copyright file="UpdateMethod.cs" company="Gamma Four, Inc.">
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
    /// Updates a method to start editing.
    /// </summary>
    public class UpdateMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableSchema tableSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMethod"/> class.
        /// </summary>
        /// <param name="tableSchema">The unique constraint schema.</param>
        public UpdateMethod(TableSchema tableSchema)
        {
            // Initialize the object.
            this.tableSchema = tableSchema;
            this.Name = "Update" + tableSchema.Name;

            //        /// <summary>
            //        /// Updates a Configuration record.
            //        /// </summary>
            //        /// <param name="configurationId">The optional value for the configurationId column.</param>
            //        /// <param name="configurationKey">The required key for the Configuration table.</param>
            //        /// <param name="rowVersion">The required value for the rowVersion column.</param>
            //        /// <param name="sourceKey">The optional value for the sourceKey column.</param>
            //        /// <param name="targetKey">The optional value for the targetKey column.</param>
            //        public void UpdateConfiguration(string configurationId, ConfigurationKey configurationKey, long rowVersion, string sourceKey, string targetKey)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithAttributeLists(this.AttributeLists)
                .WithModifiers(this.Modifiers)
                .WithParameterList(this.ParameterList)
                .WithBody(this.Body)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the data contract attribute syntax.
        /// </summary>
        private SyntaxList<AttributeListSyntax> AttributeLists
        {
            get
            {
                // This collects all the attributes.
                List<AttributeListSyntax> attributes = new List<AttributeListSyntax>();

                // Ignore the spelling in the diagnostic messages for the child constraints.
                foreach (RelationSchema relationSchema in this.tableSchema.ChildRelations)
                {
                    //        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ConfigurationKey")]
                    attributes.Add(
                        SyntaxFactory.AttributeList(
                            SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("SuppressMessage"))
                                .WithArgumentList(
                                    SyntaxFactory.AttributeArgumentList(
                                        SyntaxFactory.SeparatedList<AttributeArgumentSyntax>(
                                            new SyntaxNodeOrToken[]
                                            {
                                                SyntaxFactory.AttributeArgument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        SyntaxFactory.Literal("Microsoft.Naming"))),
                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                SyntaxFactory.AttributeArgument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        SyntaxFactory.Literal("CA2204:Literals should be spelled correctly"))),
                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                SyntaxFactory.AttributeArgument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        SyntaxFactory.Literal(relationSchema.ChildKeyConstraint.Name)))
                                                .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("MessageId"))),
                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                SyntaxFactory.AttributeArgument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        SyntaxFactory.Literal("Diagnostic message.")))
                                                .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("Justification")))
                                            }))))));
                }

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
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // Scrub the incoming set of parameters for the primary key for any violations.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    if (!columnSchema.Type.GetTypeInfo().IsValueType)
                    {
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.EqualsExpression,
                                    SyntaxFactory.IdentifierName(columnSchema.CamelCaseName + "Key"),
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)),
                                SyntaxFactory.Block(
                                    ThrowArgumentNullException.GetSyntax(columnSchema.CamelCaseName + "Key"))));
                    }
                }

                // Scrub the incoming data for any violations.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    // If this column doesn't allow nulls, and the argument is a null, then throw an exception.
                    if (!columnSchema.IsNullable)
                    {
                        if (!columnSchema.Type.GetTypeInfo().IsValueType)
                        {
                            statements.Add(
                                SyntaxFactory.IfStatement(
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.EqualsExpression,
                                        SyntaxFactory.IdentifierName(columnSchema.CamelCaseName),
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NullLiteralExpression)),
                                    SyntaxFactory.Block(ThrowArgumentNullException.GetSyntax(columnSchema.CamelCaseName))));
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

                //            CountryRow countryRow;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(
                                this.tableSchema.Name + "Row"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(this.tableSchema.CamelCaseName + "Row"))))));

                //            try
                //            {
                //                <TryBlock1>
                //            }
                //            finally
                //            {
                //                <FinallyBlock1>
                //            }
                statements.Add(
                    SyntaxFactory.TryStatement()
                    .WithBlock(
                        SyntaxFactory.Block(this.TryBlock1))
                    .WithFinally(
                        SyntaxFactory.FinallyClause(
                            SyntaxFactory.Block(this.FinallyBlock1))));

                //            customerRow.AddWriterLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                SyntaxFactory.IdentifierName("AddWriterLock")))));

                // Create the list of parameters that were used in the previous try/finally block to find the record.  This set of parameters is used
                // in the exception message below when the record is deleted after finding it.
                List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    arguments.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(columnSchema.CamelCaseName + "Key")));
                }

                //            if (customerRow.RowState == RowState.Deleted)
                //            {
                //                    <ThrowRecordNotFoundException>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                SyntaxFactory.IdentifierName("RowState")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("RowState"),
                                SyntaxFactory.IdentifierName("Detached"))),
                        SyntaxFactory.Block(ThrowRecordNotFoundException.GetSyntax(this.tableSchema.PrimaryKey, arguments))));

                //            if (configurationRow.RowVersion != rowVersion)
                //            {
                //                <ThrowOptimisticConcurrencyFault>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                SyntaxFactory.IdentifierName("RowVersion")),
                            SyntaxFactory.IdentifierName("rowVersion")),
                        SyntaxFactory.Block(ThrowOptimisticConcurrencyException.GetSyntax(this.tableSchema))));

                // Lock each of the child tables so we can see if they have any children.
                foreach (RelationSchema relationSchema in this.tableSchema.ChildRelations)
                {
                    // This will construct a series of binary expressions that will test to see if the original key is different than the current key.
                    string propertyName = relationSchema.ParentColumns[0].Name;
                    BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                            SyntaxFactory.IdentifierName(propertyName)),
                        SyntaxFactory.IdentifierName(relationSchema.ParentColumns[0].CamelCaseName));

                    // Continue to extend the binary expression with additional columns in the key.
                    for (int index = 1; index < relationSchema.ParentColumns.Count; index++)
                    {
                        propertyName = relationSchema.ParentColumns[index].Name;
                        expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalOrExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                    SyntaxFactory.IdentifierName(propertyName)),
                                SyntaxFactory.IdentifierName(relationSchema.ParentColumns[index].CamelCaseName)),
                            expression);
                    }

                    //            if (customerRow.CustomerId != customerId)
                    //            {
                    //                    <LockChildKeyIndexBlock>
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            expression,
                            SyntaxFactory.Block(this.LockChildKeyIndexBlock(relationSchema))));
                }

                // For every column that has changed that is associated with a unique index, update that index.
                foreach (UniqueConstraintSchema uniqueConstraintSchema in this.tableSchema.UniqueKeys)
                {
                    // This will construct a series of binary expressions that will test to see if the original key is different than the current key.
                    BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                            SyntaxFactory.IdentifierName(uniqueConstraintSchema.Columns[0].Name)),
                        SyntaxFactory.IdentifierName(uniqueConstraintSchema.Columns[0].CamelCaseName));

                    // Continue to extend the binary expression with additional columns in the key.
                    for (int index = 1; index < uniqueConstraintSchema.Columns.Count; index++)
                    {
                        expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalOrExpression,
                            expression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                    SyntaxFactory.IdentifierName(uniqueConstraintSchema.Columns[index].Name)),
                                SyntaxFactory.IdentifierName(uniqueConstraintSchema.Columns[index].CamelCaseName)));
                    }

                    //                if (source != configurationRow.Source || configurationId != configurationRow.ConfigurationId)
                    //                {
                    //                    <LockUniqueIndexBlock>
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            expression,
                            SyntaxFactory.Block(this.LockUniqueKeyIndexBlock(uniqueConstraintSchema))));
                }

                // For every column that has changed that is associated with a parent index, update that index.
                foreach (RelationSchema relationSchema in this.tableSchema.ParentRelations)
                {
                    // This will construct a series of binary expressions that will test to see if the original key is different than the current key.
                    BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                            SyntaxFactory.IdentifierName(relationSchema.ChildColumns[0].Name)),
                        SyntaxFactory.IdentifierName(relationSchema.ChildColumns[0].CamelCaseName));

                    // Continue to extend the binary expression with additional columns in the key.
                    for (int index = 1; index < relationSchema.ChildColumns.Count; index++)
                    {
                        expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalOrExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                    SyntaxFactory.IdentifierName(relationSchema.ChildColumns[0].Name)),
                                SyntaxFactory.IdentifierName(relationSchema.ChildColumns[0].CamelCaseName)),
                            expression);
                    }

                    //                if (countryId != customerRow.CountryId)
                    //                {
                    //                    <LockParentKeyIndexBlock>
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            expression,
                            SyntaxFactory.Block(this.LockParentKeyIndexBlock(relationSchema))));
                }

                //            customerRow.BeginUpdate();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                SyntaxFactory.IdentifierName("BeginUpdate")))));

                // Create a line of code to update the row from the parameters.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    // We don't set the value of auto-increment columns.
                    if (columnSchema.IsAutoIncrement)
                    {
                        continue;
                    }

                    // The row version column is updated each time a change is made.  All other columns simply are set to the new value.
                    if (columnSchema.IsRowVersion)
                    {
                        //                configurationRow.RowVersion = this.IncrementRowVersion();
                        statements.Add(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                        SyntaxFactory.IdentifierName("RowVersion")),
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("IncrementRowVersion"))))));
                    }
                    else
                    {
                        //                configurationRow.ConfigurationId = configurationId;
                        statements.Add(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                        SyntaxFactory.IdentifierName(columnSchema.Name)),
                                    SyntaxFactory.IdentifierName(columnSchema.CamelCaseName))));
                    }
                }

                //            customerRow.EndUpdate();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                SyntaxFactory.IdentifierName("EndUpdate")))));

                //            volatileTransaction.AddActions(customerRow.CommitUpdate, customerRow.RollbackUpdate);
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
                                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                                SyntaxFactory.IdentifierName("CommitUpdate"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                                SyntaxFactory.IdentifierName("RollbackUpdate")))
                                    })))));

                // Only write to the database when the table is persistent.  Volatile tables, like ticker feeds, do not need to be written.
                if (this.tableSchema.IsPersistent)
                {
                    // Collect the arguments for the persistent store method.
                    List<KeyValuePair<string, ArgumentSyntax>> argumentPair = new List<KeyValuePair<string, ArgumentSyntax>>();

                    // Add the primary key to the set of arguments.
                    foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                    {
                        string identifierName = columnSchema.CamelCaseName + "Key";
                        argumentPair.Add(
                            new KeyValuePair<string, ArgumentSyntax>(
                                identifierName,
                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName(identifierName))));
                    }

                    // Add the columns of the table to the set of arguments.
                    foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                    {
                        string identifierName = columnSchema.Name;
                        argumentPair.Add(
                            new KeyValuePair<string, ArgumentSyntax>(
                                identifierName,
                                SyntaxFactory.Argument(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                        SyntaxFactory.IdentifierName(identifierName)))));
                    }

                    // Sort and separate the arguments.
                    arguments = new List<ArgumentSyntax>();
                    foreach (ArgumentSyntax argumentSyntax in argumentPair.OrderBy(a => a.Key).Select((kp) => kp.Value))
                    {
                        arguments.Add(argumentSyntax);
                    }

                    //            this.persistentStore.UpdateCountry(countryIdKey, abbreviation, countryId, externalId0, name, countryRow.RowVersion);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("persistentStore")),
                                    SyntaxFactory.IdentifierName("Update" + this.tableSchema.Name)))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))));
                }

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
                //        /// Updates a Configuration record.
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
                                                " Updates a " + this.tableSchema.Name + " record.",
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

                // A parameter is needed for each element of the primary key.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    //        /// <param name="configurationIdKey">The ConfigurationId primary key element.</param>
                    string identifier = columnSchema.CamelCaseName + "Key";
                    string description = "The " + columnSchema.Name + " key element.";
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
                                                            " <param name=\"" + identifier + "\">" + description + "</param>",
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList()),
                                                        SyntaxFactory.XmlTextNewLine(
                                                            SyntaxFactory.TriviaList(),
                                                            Environment.NewLine,
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList())
                                                    })))))));
                }

                // A parameter is needed for each column in the table.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    //        /// <param name="configurationId">The optional value for the ConfigurationId column.</param>
                    string identifier = columnSchema.CamelCaseName;
                    string description = columnSchema.IsNullable ?
                        "The required value for the " + columnSchema.CamelCaseName + " column." :
                        "The optional value for the " + columnSchema.CamelCaseName + " column.";
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
                                                            " <param name=\"" + identifier + "\">" + description + "</param>",
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

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> FinallyBlock1
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                this.CustomerKey.ReleaseReaderLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(this.tableSchema.PrimaryKey.Name)),
                                SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

                // This is the complete block.
                return statements;
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
                List<KeyValuePair<string, ParameterSyntax>> parameterPairs = new List<KeyValuePair<string, ParameterSyntax>>();

                // Add a parameter for each column in the primary key.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    string identifier = columnSchema.CamelCaseName + "Key";
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            identifier,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(identifier))
                            .WithType(Conversions.FromType(columnSchema.Type))));
                }

                // Add a parameter for each of the columns in the table.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    string identifier = columnSchema.CamelCaseName;
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            identifier,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(identifier))
                                    .WithType(Conversions.FromType(columnSchema.Type))));
                }

                // string abbreviation, Guid countryId, Guid countryIdKey, string externalId0, string name, long rowVersion
                return SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(
                        parameterPairs.OrderBy(p => p.Key).Select((kp) => kp.Value)));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> TryBlock1
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                this.CustomerKey.AcquireReaderLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(this.tableSchema.PrimaryKey.Name)),
                                SyntaxFactory.IdentifierName("AcquireReaderLock")))));

                // Create a list of parameters that comprise the primary key.
                List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    arguments.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(columnSchema.CamelCaseName + "Key")));
                }

                //                customerRow = this.CustomerKey.Find(customerKey);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(this.tableSchema.PrimaryKey.Name)),
                                    SyntaxFactory.IdentifierName("Find")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments))))));

                //                if (customerRow == null)
                //                {
                //                    throw new RecordNotFoundException("Customer", new object[] { customerIdKey });
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(ThrowRecordNotFoundException.GetSyntax(this.tableSchema.PrimaryKey, arguments))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="relationSchema">The relation schema.</param>
        /// <returns>A block of code that releases the reader lock.</returns>
        private List<StatementSyntax> FinallyBlock2(RelationSchema relationSchema)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                    this.CountryProvinceKey.ReleaseReaderLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(relationSchema.Name)),
                            SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="relationSchema">The relation schema.</param>
        /// <returns>A block of code that handles a changed key.</returns>
        private SyntaxList<StatementSyntax> LockChildKeyIndexBlock(RelationSchema relationSchema)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //            try
            //            {
            //                <TryBlock1>
            //            }
            //            finally
            //            {
            //                <FinallyBlock1>
            //            }
            statements.Add(
                SyntaxFactory.TryStatement()
                .WithBlock(
                    SyntaxFactory.Block(this.TryBlock2(relationSchema)))
                .WithFinally(
                    SyntaxFactory.FinallyClause(
                        SyntaxFactory.Block(this.FinallyBlock2(relationSchema)))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="relationSchema">The relation schema.</param>
        /// <returns>A block of code that handles a changed key.</returns>
        private SyntaxList<StatementSyntax> LockParentKeyIndexBlock(RelationSchema relationSchema)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                this.CustomerKey.AddReaderLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(relationSchema.ParentKeyConstraint.Name)),
                            SyntaxFactory.IdentifierName("AddReaderLock")))));

            //                this.CustomerLicenseCustomerIdKeyIndex.AddWriterLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(relationSchema.Name)),
                            SyntaxFactory.IdentifierName("AddWriterLock")))));

            // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will ignore an
            // entry when it all of the components in the key are null.
            if (relationSchema.ChildKeyConstraint.IsNullable)
            {
                // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from
                // here.
                bool isFirstColumn = true;
                ExpressionSyntax previousIndexExpression = null;
                for (int index = 0; index < relationSchema.ChildKeyConstraint.Columns.Count; index++)
                {
                    ColumnSchema columnSchema = relationSchema.ChildKeyConstraint.Columns[index];
                    if (!columnSchema.IsNullable)
                    {
                        continue;
                    }

                    // Create an expression to test if the column is null.  Value types are implemented as the generic 'Nullable' and have a
                    // different syntax than reference types to test for null.
                    ExpressionSyntax testColumnExpression = columnSchema.IsNullable && columnSchema.IsValueType ?
                        (ExpressionSyntax)SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(columnSchema.CamelCaseName),
                            SyntaxFactory.IdentifierName("HasValue")) :
                        (ExpressionSyntax)SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(columnSchema.CamelCaseName),
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
                        SyntaxFactory.Block(
                            this.ValidateParentBlock(relationSchema))));
            }
            else
            {
                statements.AddRange(
                    this.ValidateParentBlock(relationSchema));
            }

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueConstraintSchema">The unique constraint schema.</param>
        /// <returns>A block of code that handles a changed key.</returns>
        private SyntaxList<StatementSyntax> LockUniqueKeyIndexBlock(UniqueConstraintSchema uniqueConstraintSchema)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // If we're updating the primary index, we need to update the physical order of the table also.
            if (uniqueConstraintSchema.IsPrimaryKey)
            {
                //                this.Customer.AddWriterLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(uniqueConstraintSchema.Table.Name)),
                                SyntaxFactory.IdentifierName("AddWriterLock")))));
            }

            //                this.Customer.CustomerLicenseCustomerIdKeyIndex.AddWriterLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(uniqueConstraintSchema.Name)),
                            SyntaxFactory.IdentifierName("AddWriterLock")))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="relationSchema">The relation schema.</param>
        /// <returns>A block of code that locks a child relation and examines it for a constraint violation.</returns>
        private List<StatementSyntax> TryBlock2(RelationSchema relationSchema)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                this.CustomerLicenseCustomerIdKeyIndex.AcquireReaderLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(relationSchema.Name)),
                            SyntaxFactory.IdentifierName("AcquireReaderLock")))));

            // This creates the comma-separated list of parameters that are used to create a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnSchema columnSchema in relationSchema.ParentKeyConstraint.Columns)
            {
                arguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                            SyntaxFactory.IdentifierName(columnSchema.Name))));
            }

            //            if (this.CountryProvinceKeyIndex.ContainsKey(previousData.CountryId))
            //            {
            //                <ThrowConstraintException>
            //            }
            statements.Add(
                SyntaxFactory.IfStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(relationSchema.ChildKeyConstraint.Name)),
                            SyntaxFactory.IdentifierName("ContainsKey")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments))),
                    SyntaxFactory.Block(
                        ThrowConstraintException.GetSyntax("update", relationSchema.ChildKeyConstraint.Name))));

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="relationSchema">The relation schema.</param>
        /// <returns>A block of code that locks a child relation and examines it for a constraint violation.</returns>
        private List<StatementSyntax> ValidateParentBlock(RelationSchema relationSchema)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // Collect the arguments needed to find the record.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            foreach (ColumnSchema columnSchema in relationSchema.ChildColumns)
            {
                arguments.Add(
                    columnSchema.IsNullable && columnSchema.IsValueType ?
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(columnSchema.CamelCaseName),
                            SyntaxFactory.IdentifierName("Value"))) :
                    SyntaxFactory.Argument(
                        SyntaxFactory.IdentifierName(columnSchema.CamelCaseName)));
            }

            //            if (!this.CountryKey.ContainsKey(countryId))
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
                                    SyntaxFactory.IdentifierName(relationSchema.ParentKeyConstraint.Name)),
                                SyntaxFactory.IdentifierName("ContainsKey")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))),
                    SyntaxFactory.Block(
                        ThrowConstraintException.GetSyntax("update", relationSchema.ChildKeyConstraint.Name))));

            // This is the complete block.
            return statements;
        }
    }
}