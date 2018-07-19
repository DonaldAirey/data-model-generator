// <copyright file="DeleteMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.DataModelClass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Deletes a method to start editing.
    /// </summary>
    public class DeleteMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableSchema tableSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteMethod"/> class.
        /// </summary>
        /// <param name="tableSchema">The unique constraint schema.</param>
        public DeleteMethod(TableSchema tableSchema)
        {
            // Initialize the object.
            this.tableSchema = tableSchema;
            this.Name = string.Format(CultureInfo.InvariantCulture, "Delete{0}", tableSchema.Name);

            //        /// <summary>
            //        /// Deletes a Configuration record.
            //        /// </summary>
            //        /// <param name="configurationKey">The primary key for the Configuration table.</param>
            //        /// <param name="rowVersion">The required value for the rowVersion column.</param>
            //        public void DeleteConfiguration(ConfigurationKey configurationKey, long rowVersion)
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

                // Scrub the incoming data for any violations.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
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

                //            ConfigurationRow configurationRow;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(this.tableSchema.Name + "Row"))
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

                //            if (customerRow.RowState == RowState.Deleted)
                //            {
                //                return;
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
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ReturnStatement()))));

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

                // Lock each of the child tables so we can query them to make sure they're empty.
                foreach (RelationSchema relationSchema in this.tableSchema.ChildRelations)
                {
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
                }

                //            this.Country.AddWriterLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(this.tableSchema.Name)),
                                SyntaxFactory.IdentifierName("AddWriterLock")))));

                // Lock each of the unique key indices that are associated with this record.
                foreach (UniqueConstraintSchema uniqueConstraintSchema in this.tableSchema.UniqueKeys)
                {
                    // We don't need to lock indices that not connected to this record because the key values are null.
                    if (uniqueConstraintSchema.IsNullable)
                    {
                        // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                        // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from
                        // here.
                        string columnParameter = uniqueConstraintSchema.Columns[0].Name;
                        BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                SyntaxFactory.IdentifierName(columnParameter)),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                            .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)));

                        // Combine multiple key elements with a logical 'AND' binary expression.
                        for (int andIndex = 1; andIndex < uniqueConstraintSchema.Columns.Count; andIndex++)
                        {
                            columnParameter = uniqueConstraintSchema.Columns[andIndex].Name;
                            expression = SyntaxFactory.BinaryExpression(
                                SyntaxKind.LogicalAndExpression,
                                expression,
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                        SyntaxFactory.IdentifierName(columnParameter)),
                                    SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                                    .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.LogicalAndExpression)));
                        }

                        //            if (customerRow.ExternalId0 != null)
                        //            {
                        //                <LockUniqueKeyIndexBlock>
                        //            }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                expression,
                                SyntaxFactory.Block(this.LockUniqueKeyIndexBlock(uniqueConstraintSchema))));
                    }
                    else
                    {
                        //            this.CountryKey.AddWriterLock();
                        statements.AddRange(this.LockUniqueKeyIndexBlock(uniqueConstraintSchema));
                    }
                }

                // Lock the parent key indices that are related to this record.
                foreach (RelationSchema relationSchema in this.tableSchema.ParentRelations)
                {
                    // Indices with nullable columns are handled differently than non-nullable columns.  We are going to generate code that will
                    // ignore an entry when it all of the key components are null.
                    if (relationSchema.ChildKeyConstraint.IsNullable)
                    {
                        // The general idea here is build a sequence of binary expressions, each of them testing for a null value in the index.  The
                        // first column acts as the seed and the binary expressions are built up by a succession of logical 'AND' statements from
                        // here.
                        string columnParameter = relationSchema.ChildKeyConstraint.Columns[0].Name;
                        BinaryExpressionSyntax expression = SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                SyntaxFactory.IdentifierName(columnParameter)),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                            .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)));

                        // Combine multiple key elements with a logical 'AND' binary expression.
                        for (int andIndex = 1; andIndex < relationSchema.ChildKeyConstraint.Columns.Count; andIndex++)
                        {
                            columnParameter = relationSchema.ChildKeyConstraint.Columns[andIndex].Name;
                            expression = SyntaxFactory.BinaryExpression(
                                SyntaxKind.LogicalAndExpression,
                                expression,
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.NotEqualsExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                        SyntaxFactory.IdentifierName(columnParameter)),
                                    SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                                    .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword)))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.LogicalAndExpression)));
                        }

                        //            if (customerRow.Current.externalId0Field != null)
                        //            {
                        //                <DeleteFromUniqueKeyIndexBlock>
                        //            }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                expression,
                                SyntaxFactory.Block(this.LockParentKeyIndexBlock(relationSchema))));
                    }
                    else
                    {
                        //                this.Table.CountryExternalId0KeyIndex.Remove(new CountryExternalId0Key(this.Current.ExternalId0));
                        statements.AddRange(this.LockParentKeyIndexBlock(relationSchema));
                    }
                }

                //            customerRow.Delete();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                SyntaxFactory.IdentifierName("Delete")))));

                //            volatileTransaction.AddActions(customerRow.CommitDelete, customerRow.RollbackDelete);
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
                                                SyntaxFactory.IdentifierName("CommitDelete"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                                SyntaxFactory.IdentifierName("RollbackDelete")))
                                    })))));

                // This will destroy the persistent version of the data.
                if (this.tableSchema.IsPersistent)
                {
                    // Collect the arguments for the persistent store method.
                    List<KeyValuePair<string, ArgumentSyntax>> argumentPair = new List<KeyValuePair<string, ArgumentSyntax>>();

                    // Add the primary key to the set of arguments.
                    foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                    {
                        string identifierName = columnSchema.CamelCaseName;
                        argumentPair.Add(
                            new KeyValuePair<string, ArgumentSyntax>(
                                identifierName,
                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName(identifierName))));
                    }

                    // Sort and separate the arguments.
                    List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                    foreach (ArgumentSyntax argumentSyntax in argumentPair.OrderBy(a => a.Key).Select((kp) => kp.Value))
                    {
                        arguments.Add(argumentSyntax);
                    }

                    //            this.persistentStore.DeleteCustomer(customerId);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("persistentStore")),
                                    SyntaxFactory.IdentifierName("Delete" + this.tableSchema.Name)))
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
                //        /// Deletes a Configuration record.
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
                                                " Deletes a " + this.tableSchema.Name + " record.",
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
                    string identifier = columnSchema.CamelCaseName;
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

                // A parameter is needed for the row version for the concurrency check.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    if (columnSchema.IsRowVersion)
                    {
                        //        /// <param name="configurationId">The optional value for the ConfigurationId column.</param>
                        string identifier = columnSchema.CamelCaseName;
                        string description = columnSchema.IsRowVersion ?
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
        private List<StatementSyntax> ExecuteQueryBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            sqlCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@abbreviation", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Current, abbreviation));
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    // These are the replaceable parameters.
                    string propertyName = columnSchema.Name;
                    string parameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", columnSchema.CamelCaseName);

                    //            sqlCommand.Parameters.Add(new SqlParameter("@configurationId", configurationId));
                    statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("sqlCommand"),
                                    SyntaxFactory.IdentifierName("Parameters")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("SqlParameter"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.LiteralExpression(
                                                                        SyntaxKind.StringLiteralExpression,
                                                                        SyntaxFactory.Literal(parameterName))),
                                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                                                                            SyntaxFactory.IdentifierName("original")),
                                                                        SyntaxFactory.IdentifierName(propertyName)))
                                                    })))))))));
                }

                //            dataModelTransaction.Execute(sqlCommand);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("dataModelTransaction"),
                                SyntaxFactory.IdentifierName("Execute")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName("sqlCommand")))))));

                // This is the complete block.
                return statements;
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

                //                this.Customer.ReleaseReaderLock();
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
                    string identifier = columnSchema.CamelCaseName;
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            identifier,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(identifier))
                            .WithType(Conversions.FromType(columnSchema.Type))));
                }

                // Add a row version for consistency checking.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    if (columnSchema.IsRowVersion)
                    {
                        string identifier = columnSchema.CamelCaseName;
                        parameterPairs.Add(
                            new KeyValuePair<string, ParameterSyntax>(
                                identifier,
                                SyntaxFactory.Parameter(
                                    SyntaxFactory.Identifier(identifier))
                                        .WithType(Conversions.FromType(columnSchema.Type))));
                    }
                }

                // string Guid countryIdKey, long rowVersion
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

                //                this.CountryKey.AcquireReaderLock();
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
                            SyntaxFactory.IdentifierName(columnSchema.CamelCaseName)));
                }

                //                countryRow = this.CountryKey.Find(countryIdKey);
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

                //                if (countryRow == null)
                //                {
                //                    return;
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ReturnStatement()))));

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
        /// <param name="relationSchema">The unique constraint schema.</param>
        /// <returns>A block of code that deletes a row from a unique index.</returns>
        private SyntaxList<StatementSyntax> LockParentKeyIndexBlock(RelationSchema relationSchema)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //            this.CountryCustomerCountryIdKeyIndex.AddWriterLock();
            statements.Add(SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName(relationSchema.Name)),
                        SyntaxFactory.IdentifierName("AddWriterLock")))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueConstraintSchema">The unique constraint schema.</param>
        /// <returns>A block of code that deletes a row from a unique index.</returns>
        private SyntaxList<StatementSyntax> LockUniqueKeyIndexBlock(UniqueConstraintSchema uniqueConstraintSchema)
        {
            // This list collects the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                this.Customer.CustomerExternalId0KeyIndex.AddWriterLock();
            statements.Add(SyntaxFactory.ExpressionStatement(
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

            //            this.CustomerLicenseCustomerIdKeyIndex.AddReaderLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(relationSchema.ChildKeyConstraint.Name)),
                            SyntaxFactory.IdentifierName("AcquireReaderLock")))));

            // This creates the comma-separated list of parameters that are used to create a key.
            List<ArgumentSyntax> childArguments = new List<ArgumentSyntax>();
            foreach (ColumnSchema columnSchema in relationSchema.ParentKeyConstraint.Columns)
            {
                childArguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(this.tableSchema.CamelCaseName + "Row"),
                            SyntaxFactory.IdentifierName(columnSchema.Name))));
            }

            //            if (this.CountryCustomerCountryIdKey.ContainsKey(countryRow.CountryId))
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
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(childArguments))),
                    SyntaxFactory.Block(
                        ThrowConstraintException.GetSyntax("delete", relationSchema.ChildKeyConstraint.Name))));

            // This is the complete block.
            return statements;
        }
    }
}