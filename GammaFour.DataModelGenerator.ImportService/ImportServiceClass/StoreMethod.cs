// <copyright file="StoreMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.ImportService.ImportServiceClass
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
    /// A method to create a record.
    /// </summary>
    public class StoreMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableSchema tableSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreMethod"/> class.
        /// </summary>
        /// <param name="tableSchema">The unique constraint schema.</param>
        public StoreMethod(TableSchema tableSchema)
        {
            // Initialize the object.
            this.tableSchema = tableSchema;
            this.Name = "Store" + tableSchema.Name;

            //        /// <summary>
            //        /// Create a record in the Province table.
            //        /// </summary>
            //        /// <param name="abbreviation">The required value for the abbreviation column.</param>
            //        /// <param name="configurationId">Selects a configuration of unique indices used to resolve external identifiers.</param>
            //        /// <param name="countryKey">A required unique key for the parent Country record.</param>
            //        /// <param name="externalId0">The optional value for the externalId0 column.</param>
            //        /// <param name="name">The required value for the name column.</param>
            //        /// <param name="provinceId">The required value for the provinceId column.</param>
            //        /// <param name="provinceKey">A required unique key for the Province record.</param>
            //        [OperationBehavior(TransactionScopeRequired = true)]
            //        [ClaimsPermission(SecurityAction.Demand, ClaimType = ClaimTypes.Create, Resource = ClaimResources.Application)]
            //        public void CreateProvince(string abbreviation, string configurationId, string[] countryKey, string externalId0, string name, Guid provinceId, string[] provinceKey)
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

                //        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Reviewed")]
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
                                                    SyntaxFactory.Literal("Microsoft.Maintainability"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("CA1502:AvoidExcessiveComplexity"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("Generated by a tool.")))
                                            .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("Justification")))
                                        }))))));

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

                // Check for a key to resolve any of the parent tables.
                foreach (RelationSchema relationSchema in this.tableSchema.ParentRelations)
                {
                    if (!relationSchema.ChildKeyConstraint.IsNullable)
                    {
                        statements.Add(CheckNullArgument.GetSyntax(relationSchema.UniqueParentName));
                    }
                }

                //            try
                //            {
                //                <TryBlock1>
                //            }
                //            <CatchClauses1>
                statements.Add(
                SyntaxFactory.TryStatement(this.CatchClauses1)
                .WithBlock(
                    SyntaxFactory.Block(this.TryBlock1)));

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

                //            catch (ArgumentException argumentException)
                //            {
                //                throw new FaultException<FormatFault>(new FormatFault(argumentException.Message));
                //            }
                clauses.Add(
                    SyntaxFactory.CatchClause()
                    .WithDeclaration(
                        SyntaxFactory.CatchDeclaration(
                            SyntaxFactory.IdentifierName("ArgumentException"))
                        .WithIdentifier(
                            SyntaxFactory.Identifier("argumentException")))
                    .WithBlock(
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ThrowStatement(
                                    SyntaxFactory.ObjectCreationExpression(
                                        SyntaxFactory.GenericName(
                                            SyntaxFactory.Identifier("FaultException"))
                                        .WithTypeArgumentList(
                                            SyntaxFactory.TypeArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                    SyntaxFactory.IdentifierName(nameof(FormatFault))))))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.ObjectCreationExpression(
                                                        SyntaxFactory.IdentifierName(nameof(FormatFault)))
                                                    .WithArgumentList(
                                                        SyntaxFactory.ArgumentList(
                                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.IdentifierName("argumentException"),
                                                                        SyntaxFactory.IdentifierName("Message")))))))))))))));

                //            catch (FormatException formatException)
                //            {
                //                throw new FaultException<FormatFault>(new FormatFault(formatException.Message));
                //            }
                clauses.Add(
                    SyntaxFactory.CatchClause()
                    .WithDeclaration(
                        SyntaxFactory.CatchDeclaration(
                            SyntaxFactory.IdentifierName("FormatException"))
                        .WithIdentifier(
                            SyntaxFactory.Identifier("formatException")))
                    .WithBlock(
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ThrowStatement(
                                    SyntaxFactory.ObjectCreationExpression(
                                        SyntaxFactory.GenericName(
                                            SyntaxFactory.Identifier("FaultException"))
                                        .WithTypeArgumentList(
                                            SyntaxFactory.TypeArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                    SyntaxFactory.IdentifierName(nameof(FormatFault))))))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.ObjectCreationExpression(
                                                        SyntaxFactory.IdentifierName(nameof(FormatFault)))
                                                    .WithArgumentList(
                                                        SyntaxFactory.ArgumentList(
                                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.IdentifierName("formatException"),
                                                                        SyntaxFactory.IdentifierName("Message")))))))))))))));

                // This is the collection of catch clauses.
                return SyntaxFactory.List<CatchClauseSyntax>(clauses);
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> CreateRecordBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // This will provide a default value for any auto-generated identifiers.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    if (columnSchema.Type == typeof(Guid))
                    {
                        //                if (customerIdParsed == default(Guid))
                        //                {
                        //                    <GenerateGuid>
                        //                }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.EqualsExpression,
                                    SyntaxFactory.IdentifierName(columnSchema.CamelCaseName + "Parsed"),
                                    SyntaxFactory.DefaultExpression(
                                        SyntaxFactory.IdentifierName("Guid"))),
                                SyntaxFactory.Block(this.GenerateGuid(columnSchema))));
                    }
                }

                // Collect the arguments for this method.
                List<KeyValuePair<string, ArgumentSyntax>> arguments = new List<KeyValuePair<string, ArgumentSyntax>>();

                // Add a argument for each of the columns in the table except the row version.  Use the parsed version for any data types that are
                // not strings.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    // Columns that are either natively strings, or extracted from parent relations, are not parsed and the variable names are not
                    // decorated.
                    bool isParsed = columnSchema.Type != typeof(string);
                    foreach (RelationSchema relationSchema in this.tableSchema.ParentRelations)
                    {
                        foreach (ColumnSchema childColumnSchema in relationSchema.ChildColumns)
                        {
                            if (columnSchema == childColumnSchema)
                            {
                                isParsed = false;
                            }
                        }
                    }

                    if (!columnSchema.IsRowVersion)
                    {
                        string identifier = columnSchema.CamelCaseName;
                        arguments.Add(
                            new KeyValuePair<string, ArgumentSyntax>(
                                identifier,
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(isParsed ? identifier + "Parsed" : identifier))));
                    }
                }

                //                    this.dataModel.CreateCustomer(address1, address2, city, company, countryId, customerIdParsed, dateCreatedParsed, dateModifiedParsed, email, externalId0, firstName, lastName, middleName, phone, postalCode, provinceId);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("dataModel")),
                                SyntaxFactory.IdentifierName("Create" + this.tableSchema.Name)))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    arguments.OrderBy(p => p.Key).Select((kp) => kp.Value))))));

                // This is the complete block.
                return statements;
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
                //        /// Stores a record in the Configuration table.
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
                                                " Stores a record in the " + this.tableSchema.Name + " table.",
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

                //        /// <param name="configurationId">Selects a configuration of unique indices used to resolve external identifiers.</param>
                if (this.tableSchema.Name != "Configuration")
                {
                    string configurationIdentifier = "configurationId";
                    string configurationDescription = "Selects a configuration of unique indices used to resolve external identifiers.";
                    parameterTrivia.Add(
                        new KeyValuePair<string, SyntaxTrivia>(
                            configurationIdentifier,
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
                                                            " <param name=\"" + configurationIdentifier + "\">" + configurationDescription + "</param>",
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList()),
                                                        SyntaxFactory.XmlTextNewLine(
                                                            SyntaxFactory.TriviaList(),
                                                            Environment.NewLine,
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList())
                                                    })))))));
                }

                // Add a parameter for finding the record using a unique key.  This is an array of string values that will be parsed into native
                // types depending on the requirements of the index specified by the 'configurationId' parameter.  The choice of index will determine
                // which of the unique indices is used to find the record.  We might want to use 'externalId0' for an import an identity from one
                // vendor and 'externalId2' import an identity from a second vendor.
                string primaryKeyIdentifier = this.tableSchema.CamelCaseName + "Key";
                string primaryKeyDescription = "A required unique key for the " + this.tableSchema.Name + " record.";
                parameterTrivia.Add(
                    new KeyValuePair<string, SyntaxTrivia>(
                        primaryKeyIdentifier,
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
                                                            " <param name=\"" + primaryKeyIdentifier + "\">" + primaryKeyDescription + "</param>",
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList()),
                                                        SyntaxFactory.XmlTextNewLine(
                                                            SyntaxFactory.TriviaList(),
                                                            Environment.NewLine,
                                                            string.Empty,
                                                            SyntaxFactory.TriviaList())
                                                })))))));

                // Add a parameter for each of the parent tables referenced by foreign keys.
                foreach (RelationSchema relationSchema in this.tableSchema.ParentRelations)
                {
                    string uniqueKeyDescription = default(string);
                    if (relationSchema.IsDistinctPathToParent)
                    {
                        uniqueKeyDescription = relationSchema.ChildKeyConstraint.IsNullable ?
                            "An optional unique key for the parent " + relationSchema.ParentTable.Name + " record." :
                            "A required unique key for the parent " + relationSchema.ParentTable.Name + " record.";
                    }
                    else
                    {
                        string parentTableDescription = relationSchema.ParentTable.CamelCaseName + " using";
                        foreach (ColumnSchema columnSchema in relationSchema.ChildColumns)
                        {
                            parentTableDescription += " " + columnSchema.Name;
                        }

                        uniqueKeyDescription = relationSchema.ChildKeyConstraint.IsNullable ?
                            "An optional unique key for the parent " + parentTableDescription + " record." :
                            "A required unique key for the parent " + parentTableDescription + " record.";
                    }

                    parameterTrivia.Add(
                        new KeyValuePair<string, SyntaxTrivia>(
                            relationSchema.UniqueParentName,
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
                                                            " <param name=\"" + relationSchema.UniqueParentName + "\">" + uniqueKeyDescription + "</param>",
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
                    if (!columnSchema.IsRowVersion && !columnSchema.IsInParentKey)
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
                List<KeyValuePair<string, ParameterSyntax>> parameterPairs = new List<KeyValuePair<string, ParameterSyntax>>();

                // Add a parameter for finding the record using a unique key.  This is an array of string values that will be parsed into native
                // types depending on the requirements of the index specified by the 'configurationId' parameter.  The choice of index will determine
                // which of the unique indices is used to find the record.  We might want to use 'externalId0' for an import an identity from one
                // vendor and 'externalId2' import an identity from a second vendor.
                string uniqueKeyIdentifier = this.tableSchema.CamelCaseName + "Key";
                parameterPairs.Add(
                    new KeyValuePair<string, ParameterSyntax>(
                        uniqueKeyIdentifier,
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(uniqueKeyIdentifier))
                        .WithType(
                            SyntaxFactory.ArrayType(
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.StringKeyword)))
                            .WithRankSpecifiers(
                                SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                    SyntaxFactory.ArrayRankSpecifier(
                                        SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                            SyntaxFactory.OmittedArraySizeExpression())))))));

                // Add a parameter for each of the parent tables referenced by foreign keys.
                foreach (RelationSchema relationSchema in this.tableSchema.ParentRelations)
                {
                    string parentIdentifier = default(string);
                    if (relationSchema.IsDistinctPathToParent)
                    {
                        parentIdentifier = relationSchema.ParentTable.CamelCaseName + "Key";
                    }
                    else
                    {
                        parentIdentifier = relationSchema.ParentTable.CamelCaseName + "By";
                        foreach (ColumnSchema columnSchema in relationSchema.ChildColumns)
                        {
                            parentIdentifier += columnSchema.Name;
                        }

                        parentIdentifier += "Key";
                    }

                    // Like the primary key, the parent relations are specified using an array of strings from the outside world which are parsed
                    // into native datatypes and used to find the parent record based on the index chosen by the 'configurationId' parameter.
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            parentIdentifier,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(parentIdentifier))
                            .WithType(
                                SyntaxFactory.ArrayType(
                                    SyntaxFactory.PredefinedType(
                                        SyntaxFactory.Token(SyntaxKind.StringKeyword)))
                                .WithRankSpecifiers(
                                    SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                        SyntaxFactory.ArrayRankSpecifier(
                                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                SyntaxFactory.OmittedArraySizeExpression())))))));
                }

                // Add a parameter for each of the columns in the table that is not part of a parent key (which are provided when we find the parent
                // record) or the row version (which is not provided for import functions because we bypass the concurrency checking).
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    if (!columnSchema.IsRowVersion && !columnSchema.IsInParentKey)
                    {
                        string identifier = columnSchema.CamelCaseName;
                        parameterPairs.Add(
                            new KeyValuePair<string, ParameterSyntax>(
                                identifier,
                                SyntaxFactory.Parameter(
                                    SyntaxFactory.Identifier(identifier))
                                .WithType(
                                    SyntaxFactory.PredefinedType(
                                        SyntaxFactory.Token(SyntaxKind.StringKeyword)))));
                    }
                }

                // Every table except for the Configuration table uses a 'configurationId' to identify a set of indices that will be used for
                // resolving external identifiers.
                if (this.tableSchema.Name != "Configuration")
                {
                    string identifier = "configurationId";
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            identifier,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(identifier))
                            .WithType(
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.StringKeyword)))));
                }

                // This is the complete set of comma separated parameters for the method.
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

                // The function of the import is to take data from external systems.  Being external systems, the only guaranteed, common encoding
                // for data is strings.  So we're going to convert all the strings that are destined for columns having native data types (other than
                // strings) and parse them.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    if (!columnSchema.IsRowVersion && !columnSchema.IsInParentKey)
                    {
                        if (columnSchema.Type != typeof(string))
                        {
                            statements.Add(
                                SyntaxFactory.LocalDeclarationStatement(
                                    SyntaxFactory.VariableDeclaration(Conversions.FromType(columnSchema.Type))
                                    .WithVariables(
                                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                            SyntaxFactory.VariableDeclarator(
                                                SyntaxFactory.Identifier(columnSchema.CamelCaseName + "Parsed"))
                                            .WithInitializer(
                                                SyntaxFactory.EqualsValueClause(
                                                    Conversions.CreateConditionalParseExpression(
                                                        SyntaxFactory.IdentifierName(columnSchema.CamelCaseName),
                                                        columnSchema.Type)))))));
                        }
                    }
                }

                // This will resolve the external identifiers that relate to foreign tables.  The main idea is to map elements from foreign rows into
                // parameters that can be used to call the internal methods.
                foreach (RelationSchema relationSchema in this.tableSchema.ParentRelations)
                {
                    statements.AddRange(new ResolveForeignKey(relationSchema));
                }

                //            long rowVersion = default(long);
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.LongKeyword)))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("rowVersion"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.DefaultExpression(
                                            SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(SyntaxKind.LongKeyword)))))))));

                //            bool isNew = true;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.BoolKeyword)))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("isFound"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression)))))));

                // At this point, we've taken generic keys that describe the parent rows and found the parent rows.  From the parent rows we've taken
                // the values that will uniquely identify them and placed those values in variables.  Now we need to find the target record (for an
                // update) or determine that it doesn't exist (for an insert).  This will find the record using a method similar to the parent
                // records: use the configuration and a generic array of values to create a key.
                statements.AddRange(new ResolveUniqueKey(this.tableSchema, true));

                //            if (isFound)
                //            {
                //                <UpdateRecordBlock>
                //            }
                //            else
                //            {
                //                <CreateRecordBlock>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.IdentifierName("isFound"),
                        SyntaxFactory.Block(this.UpdateRecordBlock))
                    .WithElse(
                        SyntaxFactory.ElseClause(
                            SyntaxFactory.Block(this.CreateRecordBlock))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> UpdateRecordBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // In rare cases, the imported data may want to change the key identifiers.  To do this, you need to specify the old key as well as
                // the new key.  Since this is a rare case and we don't want to bugger up the incoming parameter list with an old and new key each
                // time, we assume that the unique key used to find the record is the same as the updated value for the key (unless the updated value
                // for the key is explicitly provided).
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    if (columnSchema.Type == typeof(Guid))
                    {
                        //                if (customerIdParsed == default(Guid))
                        //                {
                        //                    <AssignImpliedGuid>
                        //                }
                        statements.Add(
                            SyntaxFactory.IfStatement(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.EqualsExpression,
                                    SyntaxFactory.IdentifierName(columnSchema.CamelCaseName + "Parsed"),
                                    SyntaxFactory.DefaultExpression(
                                        SyntaxFactory.IdentifierName("Guid"))),
                                SyntaxFactory.Block(this.AssignImpliedGuid(columnSchema))));
                    }
                }

                // Collect the arguments for this method.
                List<KeyValuePair<string, ArgumentSyntax>> arguments = new List<KeyValuePair<string, ArgumentSyntax>>();

                // Add an argument for each member of the key that uniquely identifies the target record.
                foreach (ColumnSchema columnSchema in this.tableSchema.PrimaryKey.Columns)
                {
                    string identifier = columnSchema.CamelCaseName + "Key";
                    arguments.Add(
                        new KeyValuePair<string, ArgumentSyntax>(
                            identifier,
                            SyntaxFactory.Argument(
                                SyntaxFactory.IdentifierName(identifier))));
                }

                // Add a argument for each of the columns in the table except the row version.  Use the parsed version for any data types that are
                // not strings.
                foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
                {
                    // Columns that are either natively strings, or extracted from parent relations, are not parsed and the variable names are not
                    // decorated.
                    bool isParsed = !columnSchema.IsRowVersion && columnSchema.Type != typeof(string);
                    foreach (RelationSchema relationSchema in this.tableSchema.ParentRelations)
                    {
                        foreach (ColumnSchema childColumnSchema in relationSchema.ChildColumns)
                        {
                            if (columnSchema == childColumnSchema)
                            {
                                isParsed = false;
                            }
                        }
                    }

                    string identifier = columnSchema.CamelCaseName;
                    arguments.Add(
                        new KeyValuePair<string, ArgumentSyntax>(
                            identifier,
                            SyntaxFactory.Argument(
                                SyntaxFactory.IdentifierName(isParsed ? identifier + "Parsed" : identifier))));
                }

                //                    this.dataModel.UpdateCustomer(address1, address2, city, company, countryIdKey, customerIdKey, customerIdKey, dateCreatedKey, dateModifiedKey, email, externalId0, firstName, lastName, middleName, phone, postalCode, provinceIdKey, rowVersion);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("dataModel")),
                                SyntaxFactory.IdentifierName("Update" + this.tableSchema.Name)))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    arguments.OrderBy(p => p.Key).Select((kp) => kp.Value))))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="columnSchema">The column schema.</param>
        /// <returns>A block of code that generates a <see cref="Guid"/> for a unique identifier.</returns>
        private List<StatementSyntax> AssignImpliedGuid(ColumnSchema columnSchema)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                        customerIdParsed = Guid.NewGuid();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(columnSchema.CamelCaseName + "Parsed"),
                        SyntaxFactory.IdentifierName(columnSchema.CamelCaseName + "Key"))));

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="columnSchema">The column schema.</param>
        /// <returns>A block of code that generates a <see cref="Guid"/> for a unique identifier.</returns>
        private List<StatementSyntax> GenerateGuid(ColumnSchema columnSchema)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                        customerIdParsed = Guid.NewGuid();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(columnSchema.CamelCaseName + "Parsed"),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("Guid"),
                                SyntaxFactory.IdentifierName("NewGuid"))))));

            // This is the complete block.
            return statements;
        }
    }
}