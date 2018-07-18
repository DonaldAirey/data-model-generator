// <copyright file="UpdateMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.PersistentStoreClass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to start editing.
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
            //        /// Creates a Configuration record.
            //        /// </summary>
            //        /// <param name="configurationId">The required value for the ConfigurationId column.</param>
            //        /// <param name="sourceKey">The required value for the SourceKey column.</param>
            //        /// <param name="targetKey">The required value for the TargetKey column.</param>
            //        void CreateConfiguration(string configurationId, long rowVersion, string source, string targetKey);
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
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

                //            SqlConnection sqlConnection = this.CurrentConnection;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("SqlConnection"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("sqlConnection"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("CurrentConnection"))))))));

                // This constructs the SQL Command to update a record.
                string variableList = string.Empty;
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    if (!columnSchema.IsAutoIncrement)
                    {
                        variableList += (variableList == string.Empty ? "@" : ",@") + columnSchema.CamelCaseName;
                    }
                }

                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    variableList += ",@" + columnSchema.CamelCaseName + "Key";
                }

                string updateCommandText = "update" + this.tableSchema.Name + " " + variableList;

                //            using (SqlCommand sqlCommand = new SqlCommand("updateConfiguration @configurationId,...,@targetKey", dataModelTransaction.SqlConnection))
                //            {
                //                <ExecuteQueryBlock>
                //            }
                statements.Add(
                    SyntaxFactory.UsingStatement(SyntaxFactory.Block(this.ExecuteQueryBlock))
                    .WithDeclaration(
                        SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("SqlCommand"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("sqlCommand"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName("SqlCommand"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.LiteralExpression(
                                                                    SyntaxKind.StringLiteralExpression,
                                                                    SyntaxFactory.Literal(updateCommandText))),
                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName("sqlConnection"))
                                                    })))))))));

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
                    string description = "The " + (columnSchema.IsNullable ? "optional" : "required") + " value for the " + columnSchema.Name + " column.";
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
        private List<StatementSyntax> ExecuteQueryBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            sqlCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@abbreviation", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Current, abbreviation));
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    string variableName = columnSchema.CamelCaseName;
                    string parameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", columnSchema.CamelCaseName);
                    if (!columnSchema.IsAutoIncrement)
                    {
                        // Nullable properties need to check for null values before adding the parameters.
                        if (columnSchema.IsNullable)
                        {
                            //            if (externalId0 == null)
                            //            {
                            //                <AddNullParameterBlock>
                            //            }
                            //            else
                            //            {
                            //                <AddNonNullParameterBlock>
                            //            }
                            statements.Add(
                                SyntaxFactory.IfStatement(
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.EqualsExpression,
                                        SyntaxFactory.IdentifierName(columnSchema.CamelCaseName),
                                        SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)),
                                    SyntaxFactory.Block(
                                        this.CreateParameterBlock(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal(parameterName))),
                                            Conversions.ToSqlType(columnSchema.Type),
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName("DBNull"),
                                                    SyntaxFactory.IdentifierName("Value"))))))
                                .WithElse(
                                    SyntaxFactory.ElseClause(
                                        SyntaxFactory.Block(
                                            this.CreateParameterBlock(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        SyntaxFactory.Literal(parameterName))),
                                                Conversions.ToSqlType(columnSchema.Type),
                                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName(variableName)))))));
                        }
                        else
                        {
                            if (columnSchema.IsRowVersion)
                            {
                                statements.AddRange(
                                    this.CreateParameterBlock(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(parameterName))),
                                        Conversions.ToSqlType(typeof(long)),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(columnSchema.CamelCaseName))));
                            }
                            else
                            {
                                statements.AddRange(
                                    this.CreateParameterBlock(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(parameterName))),
                                        Conversions.ToSqlType(columnSchema.Type),
                                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName(variableName))));
                            }
                        }
                    }
                }

                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    string parameterName = "@" + columnSchema.CamelCaseName + "Key";
                    statements.AddRange(
                        this.CreateParameterBlock(
                            SyntaxFactory.Argument(
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    SyntaxFactory.Literal(parameterName))),
                            Conversions.ToSqlType(columnSchema.Type),
                            SyntaxFactory.Argument(
                                SyntaxFactory.IdentifierName(columnSchema.CamelCaseName + "Key"))));
                }

                //                sqlCommand.ExecuteNonQuery();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("sqlCommand"),
                                SyntaxFactory.IdentifierName("ExecuteNonQuery")))));

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
                // Collect the parameters for the persistent store method.
                List<KeyValuePair<string, ParameterSyntax>> parameterPairs = new List<KeyValuePair<string, ParameterSyntax>>();

                // Add the primary key to the set of parameters.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    string identifier = columnSchema.CamelCaseName + "Key";
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            identifier,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(identifier))
                            .WithType(
                                Conversions.FromType(columnSchema.Type))));
                }

                // Add the columns of the table to the set of parameters.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    string identifier = columnSchema.CamelCaseName;
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            identifier,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(identifier))
                            .WithType(
                                Conversions.FromType(columnSchema.Type))));
                }

                // This is the complete set of comma separated parameters for the method.
                return SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(
                        parameterPairs.OrderBy(a => a.Key).Select((kp) => kp.Value)));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="argument">The destination argument.</param>
        /// <param name="type">The argument type.</param>
        /// <param name="source">The source data.</param>
        /// <returns>A block of code</returns>
        private List<StatementSyntax> CreateParameterBlock(ArgumentSyntax argument, ArgumentSyntax type, ArgumentSyntax source)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                sqlCommand.Parameters.Add(new SqlParameter("@externalId0", SqlDbType.NVarChar, 0, ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Current, externalId0));
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
                                                    argument,
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    type,
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.NumericLiteralExpression,
                                                            SyntaxFactory.Literal(0))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("ParameterDirection"),
                                                            SyntaxFactory.IdentifierName("Input"))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.FalseLiteralExpression)),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.NumericLiteralExpression,
                                                            SyntaxFactory.Literal(0))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.NumericLiteralExpression,
                                                            SyntaxFactory.Literal(0))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.NullLiteralExpression)),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.IdentifierName("System"),
                                                                    SyntaxFactory.IdentifierName("Data")),
                                                                SyntaxFactory.IdentifierName("DataRowVersion")),
                                                            SyntaxFactory.IdentifierName("Current"))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    source
                                                })))))))));

            // This is the complete block.
            return statements;
        }
    }
}