// <copyright file="MergeMethod.cs" company="Gamma Four, Inc.">
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
    public class MergeMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="MergeMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public MergeMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "Merge";

            //        /// <inheritdoc/>
            //        public IEnumerable<IRow> Merge(IEnumerable<IRow> source)
            //        {
            //            <Body>
            //        }
            this.Syntax = MethodDeclaration(
                GenericName(
                    Identifier("IEnumerable"))
                .WithTypeArgumentList(
                    TypeArgumentList(
                        SingletonSeparatedList<TypeSyntax>(
                            SyntaxFactory.IdentifierName("IRow")))),
                Identifier(this.Name))
                .WithModifiers(MergeMethod.Modifiers)
                .WithParameterList(MergeMethod.Parameters)
                .WithBody(this.Body)
                .WithLeadingTrivia(MergeMethod.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static SyntaxTriviaList DocumentationComment
        {
            get
            {
                // This is used to collect the trivia.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <inheritdoc/>
                comments.Add(
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
                                        }))))));

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
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();

                // IEnumerable<object> source
                parameters.Add(
                    Parameter(
                        Identifier("source"))
                    .WithType(
                        GenericName(
                            Identifier("IEnumerable"))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName("IRow"))))));

                // This is the complete parameter specification for this constructor.
                return ParameterList(SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets the statements add the record to the residuals.
        /// </summary>
        private List<StatementSyntax> AddRecordToResiduals
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    residuals.Add(newBuyer);
                statements.Add(
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("residuals"),
                                IdentifierName("Add")))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList<ArgumentSyntax>(
                                    Argument(
                                        IdentifierName($"new{this.tableElement.Name}")))))));

                //                    continue;
                statements.Add(
                    ContinueStatement());

                return statements;
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

                //            List<IRow> residuals = new List<IRow>();
                statements.Add(
                    LocalDeclarationStatement(
                        VariableDeclaration(
                            GenericName(
                                Identifier("List"))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SingletonSeparatedList<TypeSyntax>(
                                        SyntaxFactory.IdentifierName("IRow")))))
                        .WithVariables(
                            SingletonSeparatedList<VariableDeclaratorSyntax>(
                                VariableDeclarator(
                                    Identifier("residuals"))
                                .WithInitializer(
                                    EqualsValueClause(
                                        ObjectCreationExpression(
                                            GenericName(
                                                Identifier("List"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        SyntaxFactory.IdentifierName("IRow")))))
                                        .WithArgumentList(
                                            ArgumentList())))))));

                //            foreach (Buyer newBuyer in source)
                //            {
                //                 <ProcessRecord>
                //            }
                statements.Add(
                    ForEachStatement(
                        IdentifierName(this.tableElement.Name),
                        Identifier($"new{this.tableElement.Name}"),
                        IdentifierName("source"),
                        Block(this.ProcessRecord)));

                //            return residuals;
                statements.Add(
                    ReturnStatement(
                        IdentifierName("residuals")));

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
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                this.Add(buyer = newBuyer);
                statements.Add(
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
                                            IdentifierName($"new{this.tableElement.Name}"))))))));

                return statements;
            }
        }

        /// <summary>
        /// Gets the statements checks to see if the parent record exists.
        /// </summary>
        private List<StatementSyntax> ProcessRecord
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // For each parent table, include a check to make sure the parent exists before adding the record.
                foreach (ForeignElement foreignKeyElement in this.tableElement.ParentKeys)
                {
                    //                if (!this.CountryBuyerCountryIdKey.HasParent(newBuyer))
                    //                {
                    //                    <AddRecordToResiduals>
                    //                }
                    statements.Add(
                        IfStatement(
                            PrefixUnaryExpression(
                                SyntaxKind.LogicalNotExpression,
                                InvocationExpression(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            ThisExpression(),
                                            IdentifierName(foreignKeyElement.Name)),
                                        IdentifierName("HasParent")))
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList<ArgumentSyntax>(
                                            Argument(
                                                IdentifierName($"new{this.tableElement.Name}")))))),
                            Block(this.AddRecordToResiduals)));
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
                //                    <UpdateRecord>
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
                            Block(this.UpdateRecord))));

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
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.AlertDataModel.RowVersion = entity.RowVersion;
                statements.Add(
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
                                IdentifierName("RowVersion")))));

                return statements;
            }
        }
    }
}