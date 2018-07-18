// <copyright file="StoreMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.ImportService.ImportServiceInterface
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
    /// Creates a method to store a record using external identifiers.
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

            //        [OperationContractAttribute(Action = "http://tempuri.org/IDataModel/StoreLicense", ReplyAction = "http://tempuri.org/IDataModel/StoreLicenseResponse")]
            //        void StoreLicense(string configurationId, object[] customerKey, DateTime dateStored, DateTime dateModified, string externalId0, Guid licenseId, object[] licenseKey, object[] licenseTypeKeyByDeveloperLicenseTypeCode, object[] licenseTypeKeyByRuntimeLicenseTypeCode, object[] productKey);
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithAttributeLists(this.AttributeLists)
                .WithParameterList(this.ParameterList)
                .WithLeadingTrivia(this.DocumentationComment)
                .WithSemicolonToken(
                    SyntaxFactory.Token(SyntaxKind.SemicolonToken));
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

                //        [OperationContractAttribute(Action = "http://tempuri.org/IDataModel/StoreLicense", ReplyAction = "http://tempuri.org/IDataModel/StoreLicenseResponse")]
                string callUrl = "http://tempuri.org/IDataModel/Store" + this.tableSchema.Name;
                string responseUrl = "http://tempuri.org/IDataModel/Store" + this.tableSchema.Name + "Response";
                attributes.Add(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("OperationContract"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SeparatedList<AttributeArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(callUrl)))
                                            .WithNameEquals(
                                                SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("Action"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal(responseUrl)))
                                            .WithNameEquals(
                                                SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("ReplyAction")))
                                        }))))));

                //        [TransactionFlow(TransactionFlowOption.Allowed)]
                attributes.Add(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("TransactionFlow"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                        SyntaxFactory.AttributeArgument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("TransactionFlowOption"),
                                                SyntaxFactory.IdentifierName("Allowed")))))))));

                //        [FaultContract(typeof(ConstraintFault))]
                attributes.Add(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("FaultContract"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.TypeOfExpression(
                                            SyntaxFactory.IdentifierName(nameof(ConstraintFault))))))))));

                //        [FaultContract(typeof(FormatFault))]
                attributes.Add(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("FaultContract"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.TypeOfExpression(
                                            SyntaxFactory.IdentifierName(nameof(FormatFault))))))))));

                //        [FaultContract(typeof(InvalidOperationFault))]
                attributes.Add(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("FaultContract"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.TypeOfExpression(
                                            SyntaxFactory.IdentifierName(nameof(InvalidOperationFault))))))))));

                //        [FaultContract(typeof(OptimisticConcurrencyFault))]
                attributes.Add(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("FaultContract"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.TypeOfExpression(
                                            SyntaxFactory.IdentifierName(nameof(OptimisticConcurrencyFault))))))))));

                //        [FaultContract(typeof(RecordNotFoundFault))]
                attributes.Add(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("FaultContract"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.TypeOfExpression(
                                            SyntaxFactory.IdentifierName(nameof(RecordNotFoundFault))))))))));

                //        [FaultContract(typeof(SecurityFault))]
                attributes.Add(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("FaultContract"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.TypeOfExpression(
                                            SyntaxFactory.IdentifierName(nameof(SecurityFault))))))))));

                // The collection of attributes.
                return SyntaxFactory.List<AttributeListSyntax>(attributes);
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
                    // Like the primary key, the parent relations are specified using an array of strings from the outside world which are parsed
                    // into native datatypes and used to find the parent record based on the index chosen by the 'configurationId' parameter.
                    parameterPairs.Add(
                        new KeyValuePair<string, ParameterSyntax>(
                            relationSchema.UniqueParentName,
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier(relationSchema.UniqueParentName))
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
    }
}