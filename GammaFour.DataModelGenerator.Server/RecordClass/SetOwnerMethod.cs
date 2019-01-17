// <copyright file="SetOwnerMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RecordClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to set the parent record set.
    /// </summary>
    public class SetOwnerMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetOwnerMethod"/> class.
        /// </summary>
        /// <param name="tableElement">A description of the table.</param>
        public SetOwnerMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "SetOwner";

            //        /// <summary>
            //        /// Sets the parent record set.
            //        /// </summary>
            //        /// <param name="countries">A <see cref="CountrySet"/> to which this record belongs (or null to leave a set).</param>
            //        internal void SetOwner(CountrySet countries)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithModifiers(this.Modifiers)
                .WithParameterList(this.Parameters)
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

                //            if (this.RecordState == RecordState.Unchanged)
                //            {
                //                <UnchangedStateChange>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("State")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("RecordState"),
                                SyntaxFactory.IdentifierName("Unchanged"))),
                        SyntaxFactory.Block(this.UnchangedStateChange)));

                //            if (this.RecordState == RecordState.Detached)
                //            {
                //                <DetachedStateChange>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("State")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("RecordState"),
                                SyntaxFactory.IdentifierName("Detached"))),
                        SyntaxFactory.Block(this.DetachedStateChange)));

                //            if (countries == null)
                //            {
                //                <UseDefaultChildren>
                //            }
                //            else
                //            {
                //                <UseOwnerChildren>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase().ToPlural()),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(this.UseDefaultChildren))
                    .WithElse(
                        SyntaxFactory.ElseClause(
                            SyntaxFactory.Block(this.UseOwnerChildren))));

                //            this.countries = countries;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase().ToPlural())),
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase().ToPlural()))));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the statements that sets the underlying field to the new value.
        /// </summary>
        private List<StatementSyntax> DetachedStateChange
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                this.State = provinces == null ? RecordState.Detached : RecordState.Added;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("State")),
                            SyntaxFactory.ConditionalExpression(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.EqualsExpression,
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase().ToPlural()),
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("RecordState"),
                                    SyntaxFactory.IdentifierName("Detached")),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("RecordState"),
                                    SyntaxFactory.IdentifierName("Added"))))));

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
                //        /// Sets the parent record set.
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
                                                " Sets the parent record set.",
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

                //        /// <param name="countries">A set of records to which this record belongs (or null to leave a set).</param>
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
                                                    $" <param name=\"{this.tableElement.Name.ToCamelCase().ToPlural()}\">A set of records to which this record belongs (or null to leave a set).</param>",
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
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters from the columns in the unique constraint.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();

                // CountrySet countries
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier(this.tableElement.Name.ToCamelCase().ToPlural()))
                    .WithType(
                        SyntaxFactory.IdentifierName($"{this.tableElement.Name}Set")));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets the statements that sets the underlying field to the new value.
        /// </summary>
        private List<StatementSyntax> UnchangedStateChange
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            this.RecordState = countries == null ? RecordState.Deleted : RecordState.Added;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("State")),
                            SyntaxFactory.ConditionalExpression(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.EqualsExpression,
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase().ToPlural()),
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("RecordState"),
                                    SyntaxFactory.IdentifierName("Deleted")),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("RecordState"),
                                    SyntaxFactory.IdentifierName("Added"))))));

                //                    this.originalData = (object[])this.currentData.Clone();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("originalData")),
                            SyntaxFactory.CastExpression(
                                SyntaxFactory.ArrayType(
                                    SyntaxFactory.PredefinedType(
                                        SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                .WithRankSpecifiers(
                                    SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                        SyntaxFactory.ArrayRankSpecifier(
                                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                SyntaxFactory.OmittedArraySizeExpression())))),
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("currentData")),
                                        SyntaxFactory.IdentifierName("Clone")))))));

                //                    this.previousData = (object[])this.currentData.Clone();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("previousData")),
                            SyntaxFactory.CastExpression(
                                SyntaxFactory.ArrayType(
                                    SyntaxFactory.PredefinedType(
                                        SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                .WithRankSpecifiers(
                                    SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                        SyntaxFactory.ArrayRankSpecifier(
                                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                SyntaxFactory.OmittedArraySizeExpression())))),
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("currentData")),
                                        SyntaxFactory.IdentifierName("Clone")))))));

                return statements;
            }
        }

        /// <summary>
        /// Gets the statements.
        /// </summary>
        private List<StatementSyntax> UseDefaultChildren
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            this.getBuyers = () => Country.defaultBuyers;
                //            this.getProvinces = () => Country.defaultProvinces;
                //            this.getRegions = () => Country.defaultRegions;
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ChildKeys)
                {
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName($"get{foreignKeyElement.Table.Name.ToPlural()}")),
                                SyntaxFactory.ParenthesizedLambdaExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(this.tableElement.Name),
                                        SyntaxFactory.IdentifierName($"default{foreignKeyElement.Table.Name.ToPlural()}"))))));
                }

                return statements;
            }
        }

        /// <summary>
        /// Gets the statements.
        /// </summary>
        private List<StatementSyntax> UseOwnerChildren
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                this.getBuyers = () => this.Countries.Domain.Buyers.CountryBuyerCountryIdKey.GetChildren(this);
                //                this.getProvinces = () => this.Countries.Domain.Provinces.CountryProvinceKey.GetChildren(this);
                //                this.getRegions = () => this.Countries.Domain.Regions.CountryRegionKey.GetChildren(this);
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ChildKeys)
                {
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName($"get{foreignKeyElement.Table.Name.ToPlural()}")),
                                SyntaxFactory.ParenthesizedLambdaExpression(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(foreignKeyElement.UniqueKey.Table.Name.ToPlural())),
                                                        SyntaxFactory.IdentifierName(foreignKeyElement.XmlSchemaDocument.Name)),
                                                    SyntaxFactory.IdentifierName(foreignKeyElement.Table.Name.ToPlural())),
                                                SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                            SyntaxFactory.IdentifierName("GetChildren")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.ThisExpression()))))))));
                }

                return statements;
            }
        }
    }
}