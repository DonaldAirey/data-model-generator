// <copyright file="PurgeBucketMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
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
    public class PurgeBucketMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeBucketMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public PurgeBucketMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "PurgeBucket";

            //        /// <inheritdoc/>
            //        internal IEnumerable<IRow> PurgeBucket(IEnumerable<IRow> source)
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
                .WithModifiers(PurgeBucketMethod.Modifiers)
                .WithParameterList(PurgeBucketMethod.Parameters)
                .WithBody(this.Body)
                .WithLeadingTrivia(PurgeBucketMethod.DocumentationComment);
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
                        Token(SyntaxKind.InternalKeyword),
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
                    // IEnumerable<object> source
                    Parameter(
                        Identifier("source"))
                    .WithType(
                        GenericName(
                            Identifier("IEnumerable"))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName("IRow"))))),
                };

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
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //                    residuals.Add(buyer);
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
                                        IdentifierName($"old{this.tableElement.Name}")))))),

                    //                    continue;
                    ContinueStatement(),
                };

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
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //            List<IRow> residuals = new List<IRow>();
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
                                            ArgumentList())))))),

                    //            foreach (Buyer oldBuyer in source)
                    //            {
                    //                 <ProcessRecord>
                    //            }
                    ForEachStatement(
                        IdentifierName(this.tableElement.Name),
                        Identifier($"old{this.tableElement.Name}"),
                        IdentifierName("source"),
                        Block(this.ProcessRecord)),

                    //            return residuals;
                    ReturnStatement(
                        IdentifierName("residuals")),
                };

                // This is the syntax for the body of the method.
                return Block(List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the statements checks to check to see if there are child records and remove if not.
        /// </summary>
        private List<StatementSyntax> CheckAndRemove
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // For each parent table, include a check to make sure the parent exists before adding the record.
                foreach (ForeignIndexElement foreignKeyElement in this.tableElement.ChildKeys)
                {
                    //                if (entity.EntityTreesByChildId.Any())
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
                                        IdentifierName(this.tableElement.Name.ToVariableName()),
                                        IdentifierName(foreignKeyElement.UniqueChildName)),
                                    IdentifierName("Any"))),
                            Block(this.AddRecordToResiduals)));
                }

                //                this.Remove(proposedOrder);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Remove")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName())))))));

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
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //            object key = this.primaryKeyFunction(buyer);
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
                                                        IdentifierName($"old{this.tableElement.Name}")))))))))),

                    //                Entity entity = this.EntityKey.Find(key);
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.tableElement.Name))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(this.tableElement.Name.ToVariableName()))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(this.tableElement.XmlSchemaDocument.Name)),
                                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(this.tableElement.PrimaryIndex.Name)),
                                                SyntaxFactory.IdentifierName("Find")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("key")))))))))),

                    //                if (proposedOrder != null)
                    //                {
                    //                    <CheckAndRemove>
                    //                }
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(this.CheckAndRemove)),

                    //                if (entity.RowVersion > this.AlertDataModel.RowVersion)
                    //                {
                    //                    <UpdateRowVersion>
                    //                }
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
                        Block(this.UpdateRowVersion)),
                };

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
                                IdentifierName($"old{this.tableElement.Name}"),
                                IdentifierName("RowVersion")))),
                };

                return statements;
            }
        }
    }
}