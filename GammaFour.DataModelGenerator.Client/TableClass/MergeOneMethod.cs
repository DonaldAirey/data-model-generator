// <copyright file="MergeOneMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.TableClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    /// <summary>
    /// Creates a method to merge a record.
    /// </summary>
    public class MergeOneMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="MergeOneMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public MergeOneMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "Merge";

            //        /// <inheritdoc/>
            //        public void Merge(IRow row)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                SyntaxFactory.Identifier(this.Name))
                .WithModifiers(MergeOneMethod.Modifiers)
                .WithParameterList(MergeOneMethod.Parameters)
                .WithBody(this.Body)
                .WithLeadingTrivia(MergeOneMethod.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static SyntaxTriviaList DocumentationComment
        {
            get
            {
                // This is used to collect the trivia.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <inheritdoc/>
                    Trivia(
                        DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            SingletonList<XmlNodeSyntax>(
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        new[]
                                        {
                                            XmlTextLiteral(
                                                TriviaList(DocumentationCommentExterior(Strings.CommentExterior)),
                                                " <inheritdoc/>",
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextNewLine(
                                                TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                TriviaList()),
                                        }))))),
                };

                // This is the complete document comment.
                return TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // private
                return TokenList(
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
                    });
            }
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private static ParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>
                {
                    // IRow row
                    Parameter(
                        Identifier("row"))
                    .WithType(
                        SyntaxFactory.IdentifierName("IRow")),
                };

                // This is the complete parameter specification for this constructor.
                return ParameterList(SeparatedList<ParameterSyntax>(parameters));
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
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //            RuleArgument newRuleArgument = row as RuleArgument;
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.tableElement.Name))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier($"new{this.tableElement.Name}"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.BinaryExpression(
                                            SyntaxKind.AsExpression,
                                            SyntaxFactory.IdentifierName("row"),
                                            SyntaxFactory.IdentifierName(this.tableElement.Name))))))),
                };

                // For each parent table, include a check to make sure the parent exists before adding the record.
                foreach (ForeignElement foreignKeyElement in this.tableElement.ParentKeys)
                {
                    //                if (!this.AccountRuleArgumentIndex.HasParent(newRuleArgument))
                    //                {
                    //                    throw new InvalidOperationException("Unable to merge results to RuleArguments table");
                    //                }
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
                                            SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("HasParent")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName($"new{this.tableElement.Name}")))))),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ThrowStatement(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("InvalidOperationException"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal($"Unable to merge results to {this.tableElement.Name.ToPlural()} table")))))))))));
                }

                //            object key = this.primaryKeyFunction(buyer);
                statements.Add(
                    LocalDeclarationStatement(
                    VariableDeclaration(
                        PredefinedType(
                            Token(SyntaxKind.ObjectKeyword)))
                    .WithVariables(
                        SingletonSeparatedList<VariableDeclaratorSyntax>(
                            VariableDeclarator(
                                Identifier("key"))
                            .WithInitializer(
                                EqualsValueClause(
                                    InvocationExpression(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            ThisExpression(),
                                            IdentifierName("primaryKeyFunction")))
                                    .WithArgumentList(
                                        ArgumentList(
                                            SingletonSeparatedList<ArgumentSyntax>(
                                                Argument(
                                                    IdentifierName($"new{this.tableElement.Name}")))))))))));

                //                Entity entity = this.EntityKey.Find(key);
                statements.Add(
                    LocalDeclarationStatement(
                        VariableDeclaration(
                            IdentifierName(this.tableElement.Name))
                        .WithVariables(
                            SingletonSeparatedList<VariableDeclaratorSyntax>(
                                VariableDeclarator(
                                    Identifier(this.tableElement.Name.ToVariableName()))
                                .WithInitializer(
                                    EqualsValueClause(
                                        InvocationExpression(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    ThisExpression(),
                                                    IdentifierName(this.tableElement.PrimaryKey.Name)),
                                                IdentifierName("Find")))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList<ArgumentSyntax>(
                                                    Argument(
                                                        IdentifierName("key")))))))))));

                //                if (entity == null)
                //                {
                //                    <AddRecord>
                //                }
                //                else
                //                {
                //                    <UpdateRecordIfNewer>
                //                }
                statements.Add(
                    IfStatement(
                        BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            IdentifierName(this.tableElement.Name.ToVariableName()),
                            LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        Block(this.AddRecord))
                    .WithElse(
                        ElseClause(
                            Block(this.UpdateRecordIfNewer))));

                //                if (entity.RowVersion > this.AlertDataModel.RowVersion)
                //                {
                //                    <UpdateRowVersion>
                //                }
                statements.Add(
                    IfStatement(
                        BinaryExpression(
                            SyntaxKind.GreaterThanExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName(this.tableElement.Name.ToVariableName()),
                                IdentifierName("RowVersion")),
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    IdentifierName(this.tableElement.XmlSchemaDocument.Name)),
                                IdentifierName("RowVersion"))),
                        Block(this.UpdateRowVersion)));

                // This is the syntax for the body of the method.
                return Block(List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the statements checks to see if the parent record exists.
        /// </summary>
        private List<StatementSyntax> AddRecord
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //                this.Add(buyer = newBuyer);
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName("Add")))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList<ArgumentSyntax>(
                                    Argument(
                                        AssignmentExpression(
                                            SyntaxKind.SimpleAssignmentExpression,
                                            IdentifierName(this.tableElement.Name.ToVariableName()),
                                            IdentifierName($"new{this.tableElement.Name}"))))))),
                };

                return statements;
            }
        }

        /// <summary>
        /// Gets the statements checks to see if the parent record exists.
        /// </summary>
        private List<StatementSyntax> UpdateRecordIfNewer
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //                    if (outsideOrder.RowVersion < newOutsideOrder.RowVersion)
                    //                    {
                    //                        <UpdateRecord>
                    //                    }
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.LessThanExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()),
                                SyntaxFactory.IdentifierName("RowVersion")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName($"new{this.tableElement.Name}"),
                                SyntaxFactory.IdentifierName("RowVersion"))),
                        SyntaxFactory.Block(this.UpdateRecord)),
                };

                return statements;
            }
        }

        /// <summary>
        /// Gets the statements checks to see if the parent record exists.
        /// </summary>
        private List<StatementSyntax> UpdateRecord
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                        account.AccountTypeCode = newAccount.AccountTypeCode;
                //                        account.Mnemonic = newAccount.Mnemonic;
                foreach (ColumnElement columnElement in this.tableElement.Columns)
                {
                    //                        account.AccountTypeCode = newAccount.AccountTypeCode;
                    statements.Add(
                        ExpressionStatement(
                            AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName(this.tableElement.Name.ToVariableName()),
                                    IdentifierName(columnElement.Name)),
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName($"new{this.tableElement.Name}"),
                                    IdentifierName(columnElement.Name)))));
                }

                //                this.Update(buyer);
                statements.Add(
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName("Update")))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList<ArgumentSyntax>(
                                    Argument(
                                        IdentifierName(this.tableElement.Name.ToVariableName())))))));

                return statements;
            }
        }

        /// <summary>
        /// Gets the statements checks to see if the parent record exists.
        /// </summary>
        private List<StatementSyntax> UpdateRowVersion
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //                    this.AlertDataModel.RowVersion = entity.RowVersion;
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    IdentifierName(this.tableElement.XmlSchemaDocument.Name)),
                                IdentifierName("RowVersion")),
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName(this.tableElement.Name.ToVariableName()),
                                IdentifierName("RowVersion")))),
                };

                return statements;
            }
        }
    }
}