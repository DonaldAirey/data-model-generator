// <copyright file="PurgeMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2019 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.RecordSetClass
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
    public class PurgeMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public PurgeMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "Purge";

            //        /// <inheritdoc/>
            //        public IEnumerable<object> Purge(IEnumerable<object> source)
            //        {
            //            <Body>
            //        }
            this.Syntax = MethodDeclaration(
                GenericName(
                    Identifier("IEnumerable"))
                .WithTypeArgumentList(
                    TypeArgumentList(
                        SingletonSeparatedList<TypeSyntax>(
                            PredefinedType(
                                Token(SyntaxKind.ObjectKeyword))))),
                Identifier(this.Name))
                .WithModifiers(PurgeMethod.Modifiers)
                .WithParameterList(PurgeMethod.Parameters)
                .WithBody(this.Body)
                .WithLeadingTrivia(PurgeMethod.DocumentationComment);
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
                                    PredefinedType(
                                        Token(SyntaxKind.ObjectKeyword)))))));

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

                //                    residuals.Add(buyer);
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
                                        IdentifierName($"old{this.tableElement.Name}")))))));

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

                //            List<object> residuals = new List<object>();
                statements.Add(
                    LocalDeclarationStatement(
                        VariableDeclaration(
                            GenericName(
                                Identifier("List"))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SingletonSeparatedList<TypeSyntax>(
                                        PredefinedType(
                                            Token(SyntaxKind.ObjectKeyword))))))
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
                                                        PredefinedType(
                                                            Token(SyntaxKind.ObjectKeyword))))))
                                        .WithArgumentList(
                                            ArgumentList())))))));

                //            foreach (Buyer oldBuyer in source)
                //            {
                //                 <ProcessRecord>
                //            }
                statements.Add(
                    ForEachStatement(
                        IdentifierName(this.tableElement.Name),
                        Identifier($"old{this.tableElement.Name}"),
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
        private List<StatementSyntax> ProcessRecord
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // For each parent table, include a check to make sure the parent exists before adding the record.
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ChildKeys)
                {
                    //                if (oldEntity.EntityTreesByChildId.Any())
                    //                {
                    //                    <AddRecordToResiduals>
                    //                }
                    statements.Add(
                        IfStatement(
                            InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName($"old{this.tableElement.Name}"),
                                        IdentifierName(foreignKeyElement.UniqueChildName)),
                                    IdentifierName("Any"))),
                            Block(this.AddRecordToResiduals)));
                }

                //                this.Remove(oldBuyer);
                statements.Add(
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName("Remove")))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList<ArgumentSyntax>(
                                    Argument(
                                        IdentifierName($"old{this.tableElement.Name}")))))));

                //                if (entity.RowVersion > this.AlertDomain.RowVersion)
                //                {
                //                    <UpdateRowVersion>
                //                }
                statements.Add(
                    IfStatement(
                        BinaryExpression(
                            SyntaxKind.GreaterThanExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName($"old{this.tableElement.Name}"),
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
        private List<StatementSyntax> UpdateRowVersion
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.AlertDomain.RowVersion = entity.RowVersion;
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
                                IdentifierName($"old{this.tableElement.Name}"),
                                IdentifierName("RowVersion")))));

                return statements;
            }
        }
    }
}