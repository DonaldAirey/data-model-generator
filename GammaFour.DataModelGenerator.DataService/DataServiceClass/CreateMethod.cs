// <copyright file="CreateMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.DataService.DataServiceClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.ClientModel;
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
        private TableSchema tableSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMethod"/> class.
        /// </summary>
        /// <param name="tableSchema">The unique constraint schema.</param>
        public CreateMethod(TableSchema tableSchema)
        {
            // Initialize the object.
            this.tableSchema = tableSchema;
            this.Name = "Create" + tableSchema.Name;

            //        /// <summary>
            //        /// Creates a record in the Configuration table.
            //        /// </summary>
            //        /// <param name="configurationId">The required value for the ConfigurationId column.</param>
            //        /// <param name="sourceKey">The required value for the SourceKey column.</param>
            //        /// <param name="targetKey">The required value for the TargetKey column.</param>
            //        [OperationBehavior(TransactionScopeRequired = true)]
            //        [ClaimsPermission(SecurityAction.Demand, ClaimType = ClaimTypes.Create, Resource = ClaimResources.Application)]
            //        public void CreateConfiguration(string configurationId, string sourceKey, string targetKey)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithModifiers(this.Modifiers)
                .WithAttributeLists(this.AttributeLists)
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

                //        [OperationBehavior(TransactionScopeRequired = true)]
                attributes.Add(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("OperationBehavior"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                        SyntaxFactory.AttributeArgument(
                                            SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression)
                                            .WithToken(SyntaxFactory.Token(SyntaxKind.TrueKeyword)))
                                        .WithNameEquals(
                                            SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("TransactionScopeRequired")))))))));

                //        [ClaimsPermission(SecurityAction.Demand, ClaimType = ClaimTypes.Create, Resource = ClaimResources.Application)]
                attributes.Add(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("ClaimsPermission"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SeparatedList<AttributeArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName("SecurityAction"),
                                                    SyntaxFactory.IdentifierName("Demand"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName("ClaimTypes"),
                                                    SyntaxFactory.IdentifierName("Create")))
                                            .WithNameEquals(
                                                SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("ClaimType"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName("ClaimResources"),
                                                    SyntaxFactory.IdentifierName("Application")))
                                            .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("Resource")))
                                        }))))));

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

                //            try
                //            {
                //                <TryBlock1>
                //            }
                //            <CatchClauses>
                statements.Add(
                    SyntaxFactory.TryStatement(this.CatchClauses1)
                    .WithBlock(SyntaxFactory.Block(this.TryBlock1)));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<CatchClauseSyntax> CatchClauses1
        {
            get
            {
                // The catch clauses are collected in this list.
                List<CatchClauseSyntax> clauses = new List<CatchClauseSyntax>();

                //            catch (ConstraintException constraintException)
                //            {
                //                <ThrowConstraintFault>
                //            }
                string constraintException = "constraintException";
                clauses.Add(
                    SyntaxFactory.CatchClause()
                    .WithDeclaration(
                        SyntaxFactory.CatchDeclaration(SyntaxFactory.IdentifierName(typeof(ConstraintException).Name))
                        .WithIdentifier(SyntaxFactory.Identifier(constraintException)))
                    .WithBlock(
                        SyntaxFactory.Block(
                            ThrowConstraintFault.GetSyntax(constraintException))));

                // This is the collection of catch clauses.
                return SyntaxFactory.List<CatchClauseSyntax>(clauses);
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
                                                " Creates a " + this.tableSchema.Name + " record.",
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
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    // The row's version is not part of the input for a create operation.
                    if (columnSchema.IsRowVersion)
                    {
                        continue;
                    }

                    //        /// <param name="configurationId">The required value for the ConfigurationId column.</param>
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
                                                            " <param name=\"" + columnSchema.CamelCaseName + "\">" + description + "</param>",
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
                List<KeyValuePair<string, ParameterSyntax>> parameters = new List<KeyValuePair<string, ParameterSyntax>>();

                // Add a parameter for each of the columns in the table except the row version.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    if (!columnSchema.IsRowVersion)
                    {
                        string identifier = columnSchema.CamelCaseName;
                        parameters.Add(
                            new KeyValuePair<string, ParameterSyntax>(
                                identifier,
                                SyntaxFactory.Parameter(
                                    SyntaxFactory.Identifier(identifier))
                                        .WithType(Conversions.FromType(columnSchema.Type))));
                    }
                }

                // Alphabetically sort and separate the parameters.
                List<SyntaxNodeOrToken> nodesAndTokens = new List<SyntaxNodeOrToken>();
                foreach (ParameterSyntax parameterSyntax in parameters.OrderBy(p => p.Key).Select((kp) => kp.Value))
                {
                    if (nodesAndTokens.Count != 0)
                    {
                        nodesAndTokens.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    nodesAndTokens.Add(parameterSyntax);
                }

                // string abbreviation, Guid countryId, string externalId0, string name
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(nodesAndTokens));
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

                // Collect the arguments for this method.
                List<KeyValuePair<string, ArgumentSyntax>> argumentPairs = new List<KeyValuePair<string, ArgumentSyntax>>();

                // Add a argument for each of the columns in the table except the row version.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    if (!columnSchema.IsRowVersion)
                    {
                        string identifier = columnSchema.CamelCaseName;
                        argumentPairs.Add(
                            new KeyValuePair<string, ArgumentSyntax>(
                                identifier,
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(identifier))));
                    }
                }

                //                this.dataModel.CreateConfiguration(configurationId, sourceKey, targetKey);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("dataModel")),
                                SyntaxFactory.IdentifierName(this.Name)))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    argumentPairs.OrderBy(p => p.Key).Select((kp) => kp.Value))))));

                // This is the complete block.
                return statements;
            }
        }
    }
}