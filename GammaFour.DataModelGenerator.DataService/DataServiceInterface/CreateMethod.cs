// <copyright file="CreateMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.DataService.DataServiceInterface
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
        private TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public CreateMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = string.Format(CultureInfo.InvariantCulture, "Create{0}", tableElement.Name);

            //        [OperationContractAttribute(Action = "http://tempuri.org/IDataModel/CreateLicense", ReplyAction = "http://tempuri.org/IDataModel/CreateLicenseResponse")]
            //        void CreateLicense(Guid customerId, DateTime dateCreated, DateTime dateModified, LicenseTypeCode developerLicenseTypeCode, string externalId0, Guid licenseId, Guid productId, LicenseTypeCode runtimeLicenseTypeCode);
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

                //        [OperationContract(Action = "http://tempuri.org/IDataModel/CreateLicense", ReplyAction = "http://tempuri.org/IDataModel/CreateLicenseResponse")]
                string callUrl = string.Format(CultureInfo.InvariantCulture, "http://tempuri.org/IDataModel/Create{0}", this.tableElement.Name);
                string responseUrl = string.Format(
                    CultureInfo.InvariantCulture,
                    "http://tempuri.org/IDataModel/Create{0}Response",
                    this.tableElement.Name);
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

                // Add a parameter for each of the columns in the table except the row version.
                foreach (ColumnElement columnElement in this.tableElement.Columns)
                {
                    if (!columnElement.IsRowVersion)
                    {
                        string identifier = columnElement.Name.ToCamelCase();
                        parameterPairs.Add(
                            new KeyValuePair<string, ParameterSyntax>(
                                identifier,
                                SyntaxFactory.Parameter(
                                    SyntaxFactory.Identifier(identifier))
                                        .WithType(Conversions.FromType(columnElement.Type))));
                    }
                }

                // string abbreviation, Guid countryId, string externalId0, string name
                return SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(
                        parameterPairs.OrderBy(p => p.Key).Select((kp) => kp.Value)));
            }
        }
    }
}