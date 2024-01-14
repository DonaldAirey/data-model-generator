// <copyright file="MergeOneMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.DataModelClass
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
        /// The XML schema.
        /// </summary>
        private readonly XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="MergeOneMethod"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The XML document schema.</param>
        public MergeOneMethod(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.Name = "Merge";

            //        /// <summary>
            //        /// Merge the results of an incremental update.
            //        /// </summary>
            //        /// <param name="jsonObject">The JSON object containg the incremental data.</param>
            //        public void Merge(JsonObject jsonObject)
            //        {
            this.Syntax = MethodDeclaration(
                    PredefinedType(
                        Token(SyntaxKind.VoidKeyword)),
                    Identifier(this.Name))
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
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <summary>
                    //        /// Initializes a new instance of the <see cref="DataModel"/> class.
                    //        /// </summary>
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
                                                " <summary>",
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextNewLine(
                                                TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextLiteral(
                                                TriviaList(DocumentationCommentExterior(Strings.CommentExterior)),
                                                " Merge the results of an incremental update.",
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextNewLine(
                                                TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextLiteral(
                                                TriviaList(DocumentationCommentExterior(Strings.CommentExterior)),
                                                " </summary>",
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextNewLine(
                                                TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                TriviaList()),
                                        }))))),

                    //        /// <param name="jsonObject">The JSON object containg the incremental data.</param>
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
                                                    $" <param name=\"jsonObject\">The JSON object containg the incremental data.</param>",
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
                    // IEnumerable<object> source
                    Parameter(
                        Identifier("jsonObject"))
                    .WithType(
                        IdentifierName("JsonObject")),
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
                    //            Dictionary<ITable, IEnumerable<IRow>> mergeBuckets = new Dictionary<ITable, IEnumerable<IRow>>();
                    LocalDeclarationStatement(
                        VariableDeclaration(
                            GenericName(
                                Identifier("Dictionary"))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SeparatedList<TypeSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            IdentifierName("ITable"),
                                            Token(SyntaxKind.CommaToken),
                                            GenericName(
                                                Identifier("IEnumerable"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        SyntaxFactory.IdentifierName("IRow")))),
                                        }))))
                        .WithVariables(
                            SingletonSeparatedList<VariableDeclaratorSyntax>(
                                VariableDeclarator(
                                    Identifier("mergeBuckets"))
                                .WithInitializer(
                                    EqualsValueClause(
                                        ObjectCreationExpression(
                                            GenericName(
                                                Identifier("Dictionary"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SeparatedList<TypeSyntax>(
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            IdentifierName("ITable"),
                                                            Token(SyntaxKind.CommaToken),
                                                            GenericName(
                                                                Identifier("IEnumerable"))
                                                            .WithTypeArgumentList(
                                                                TypeArgumentList(
                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                        SyntaxFactory.IdentifierName("IRow")))),
                                                        }))))
                                        .WithArgumentList(
                                            ArgumentList())))))),
                };

                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    //            var accountsToken = jsonObject["accounts"];
                    //            var canonsToken = jsonObject["canons"];
                    //            var entitiesToken = jsonObject["entities"];
                    statements.Add(
                        LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(
                                    SyntaxFactory.Identifier(
                                        SyntaxFactory.TriviaList(),
                                        SyntaxKind.VarKeyword,
                                        "var",
                                        "var",
                                        SyntaxFactory.TriviaList())))
                            .WithVariables(
                                SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    VariableDeclarator(
                                        Identifier($"{tableElement.Name.ToPlural().ToCamelCase()}Token"))
                                    .WithInitializer(
                                        EqualsValueClause(
                                            ElementAccessExpression(
                                                IdentifierName("jsonObject"))
                                            .WithArgumentList(
                                                BracketedArgumentList(
                                                    SingletonSeparatedList<ArgumentSyntax>(
                                                        Argument(
                                                            LiteralExpression(
                                                                SyntaxKind.StringLiteralExpression,
                                                                Literal($"{tableElement.Name.ToPlural().ToCamelCase()}"))))))))))));
                }

                //            if (accountsToken != null)
                //            {
                //                <GetMergeTable>
                //            }
                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    statements.Add(
                        IfStatement(
                            BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                IdentifierName($"{tableElement.Name.ToPlural().ToCamelCase()}Token"),
                                LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            Block(MergeOneMethod.GetMergeTable(tableElement))));
                }

                //            int count = 0;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.IntKeyword)))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("count"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NumericLiteralExpression,
                                            SyntaxFactory.Literal(0))))))));

                //            while (mergeBuckets.Values.Where(v => v.Any()).Any())
                //            {
                //                <EmptyMergeBuckets>
                //            }
                statements.Add(
                    SyntaxFactory.WhileStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("mergeBuckets"),
                                            SyntaxFactory.IdentifierName("Values")),
                                        SyntaxFactory.IdentifierName("Where")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.SimpleLambdaExpression(
                                                    SyntaxFactory.Parameter(
                                                        SyntaxFactory.Identifier("v")))
                                                .WithExpressionBody(
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("v"),
                                                            SyntaxFactory.IdentifierName("Any")))))))),
                                SyntaxFactory.IdentifierName("Any"))),
                        SyntaxFactory.Block(this.EmptyMergeBuckets)));

                //            Dictionary<ITable, IEnumerable<IRow>> purgeBuckets = new Dictionary<ITable, IEnumerable<IRow>>();
                statements.Add(
                    LocalDeclarationStatement(
                        VariableDeclaration(
                            GenericName(
                                Identifier("Dictionary"))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SeparatedList<TypeSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            IdentifierName("ITable"),
                                            Token(SyntaxKind.CommaToken),
                                            GenericName(
                                                Identifier("IEnumerable"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        SyntaxFactory.IdentifierName("IRow")))),
                                        }))))
                        .WithVariables(
                            SingletonSeparatedList<VariableDeclaratorSyntax>(
                                VariableDeclarator(
                                    Identifier("purgeBuckets"))
                                .WithInitializer(
                                    EqualsValueClause(
                                        ObjectCreationExpression(
                                            GenericName(
                                                Identifier("Dictionary"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SeparatedList<TypeSyntax>(
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            IdentifierName("ITable"),
                                                            Token(SyntaxKind.CommaToken),
                                                            GenericName(
                                                                Identifier("IEnumerable"))
                                                            .WithTypeArgumentList(
                                                                TypeArgumentList(
                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                        SyntaxFactory.IdentifierName("IRow")))),
                                                        }))))
                                        .WithArgumentList(
                                            ArgumentList())))))));

                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    //            var deletedAccountsToken = jsonObject["purgedAccounts"];
                    //            var deletedCanonsToken = jsonObject["purgedCanons"];
                    //            var deletedEntitiesToken = jsonObject["purgedEntities"];
                    statements.Add(
                        LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(
                                    SyntaxFactory.Identifier(
                                        SyntaxFactory.TriviaList(),
                                        SyntaxKind.VarKeyword,
                                        "var",
                                        "var",
                                        SyntaxFactory.TriviaList())))
                            .WithVariables(
                                SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    VariableDeclarator(
                                        Identifier($"purged{tableElement.Name.ToPlural()}Token"))
                                    .WithInitializer(
                                        EqualsValueClause(
                                            ElementAccessExpression(
                                                IdentifierName("jsonObject"))
                                            .WithArgumentList(
                                                BracketedArgumentList(
                                                    SingletonSeparatedList<ArgumentSyntax>(
                                                        Argument(
                                                            LiteralExpression(
                                                                SyntaxKind.StringLiteralExpression,
                                                                Literal($"purged{tableElement.Name.ToPlural()}"))))))))))));
                }

                //            if (deletedAccountsToken != null)
                //            {
                //                <GetPurgeTable>
                //            }
                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    statements.Add(
                        IfStatement(
                            BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                IdentifierName($"purged{tableElement.Name.ToPlural()}Token"),
                                LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            Block(MergeOneMethod.GetPurgeTable(tableElement))));
                }

                //            count = 0;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("count"),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(0)))));

                //            while (purgeBuckets.Values.Where(v => v.Any()).Any())
                //            {
                //                <EmptyPurgeBuckets>
                //            }
                statements.Add(
                    WhileStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                InvocationExpression(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("purgeBuckets"),
                                            IdentifierName("Values")),
                                        IdentifierName("Where")))
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList<ArgumentSyntax>(
                                            Argument(
                                                SimpleLambdaExpression(
                                                    Parameter(
                                                        Identifier("v")),
                                                    InvocationExpression(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName("v"),
                                                            IdentifierName("Any")))))))),
                                IdentifierName("Any"))),
                        Block(this.EmptyPurgeBuckets)));

                // This is the syntax for the body of the method.
                return Block(List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the 'empty the merge buckets' code.
        /// </summary>
        private List<StatementSyntax> EmptyMergeBuckets
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // Merge each of the tables explicitly.
                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    //                if (mergeBuckets.ContainsKey(this.Accounts))
                    //                {
                    //                    <MergeBucket>
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("mergeBuckets"),
                                    SyntaxFactory.IdentifierName("ContainsKey")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))),
                            SyntaxFactory.Block(this.MergeBucket(tableElement))));
                }

                //                if (count++ > 8)
                //                {
                //                    throw new InvalidOperationException("Unable to merge results");
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.GreaterThanExpression,
                            SyntaxFactory.PostfixUnaryExpression(
                                SyntaxKind.PostIncrementExpression,
                                SyntaxFactory.IdentifierName("count")),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(8))),
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
                                                        SyntaxFactory.Literal("Unable to merge results")))))))))));

                // This is the syntax for the body of the method.
                return statements;
            }
        }

        /// <summary>
        /// Gets generate code to empty the purge buckets.
        /// </summary>
        private List<StatementSyntax> EmptyPurgeBuckets
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // Purge each of the tables explicitly.
                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    //                if (purgeBuckets.ContainsKey(this.Accounts))
                    //                {
                    //                    <PurgeBucket>
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("purgeBuckets"),
                                    SyntaxFactory.IdentifierName("ContainsKey")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))),
                            SyntaxFactory.Block(this.PurgeBucket(tableElement))));
                }

                //                if (count++ > 8)
                //                {
                //                    throw new InvalidOperationException("Unable to merge results");
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.GreaterThanExpression,
                            SyntaxFactory.PostfixUnaryExpression(
                                SyntaxKind.PostIncrementExpression,
                                SyntaxFactory.IdentifierName("count")),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(8))),
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
                                                        SyntaxFactory.Literal("Unable to purge results")))))))))));

                // This is the syntax for the body of the method.
                return statements;
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static List<StatementSyntax> GetMergeTable(TableElement tableElement)
        {
            // The elements of the body are added to this collection as they are assembled.
            List<StatementSyntax> statements = new List<StatementSyntax>
            {
                //                mergeBuckets.Add(this.Accounts, this.Accounts.Merge(accountsToken.Deserialize<List<Account>>()));
                ExpressionStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("mergeBuckets"),
                            IdentifierName("Add")))
                    .WithArgumentList(
                        ArgumentList(
                            SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    Argument(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            ThisExpression(),
                                            IdentifierName(tableElement.Name.ToPlural()))),
                                    Token(SyntaxKind.CommaToken),
                                    Argument(
                                        InvocationExpression(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    ThisExpression(),
                                                    IdentifierName(tableElement.Name.ToPlural())),
                                                IdentifierName("MergeBucket")))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList<ArgumentSyntax>(
                                                    Argument(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName($"{tableElement.Name.ToPlural().ToCamelCase()}Token"),
                                                                GenericName(
                                                                    Identifier("Deserialize"))
                                                                .WithTypeArgumentList(
                                                                    TypeArgumentList(
                                                                        SingletonSeparatedList<TypeSyntax>(
                                                                            GenericName(
                                                                                Identifier("List"))
                                                                            .WithTypeArgumentList(
                                                                                TypeArgumentList(
                                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                                        IdentifierName(tableElement.Name)))))))))))))),
                                })))),
            };

            // This is the syntax for the body of the method.
            return statements;
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static List<StatementSyntax> GetPurgeTable(TableElement tableElement)
        {
            // The elements of the body are added to this collection as they are assembled.
            List<StatementSyntax> statements = new List<StatementSyntax>
            {
                //                purgeBuckets.Add(this.Accounts, this.Accounts.Purge(deletedAccountsToken.Deserialize<List<Account>>()));
                ExpressionStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("purgeBuckets"),
                            IdentifierName("Add")))
                    .WithArgumentList(
                        ArgumentList(
                            SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    Argument(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            ThisExpression(),
                                            IdentifierName(tableElement.Name.ToPlural()))),
                                    Token(SyntaxKind.CommaToken),
                                    Argument(
                                        InvocationExpression(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    ThisExpression(),
                                                    IdentifierName(tableElement.Name.ToPlural())),
                                                IdentifierName("PurgeBucket")))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList<ArgumentSyntax>(
                                                    Argument(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName($"purged{tableElement.Name.ToPlural()}Token"),
                                                                GenericName(
                                                                    Identifier("Deserialize"))
                                                                .WithTypeArgumentList(
                                                                    TypeArgumentList(
                                                                        SingletonSeparatedList<TypeSyntax>(
                                                                            GenericName(
                                                                                Identifier("List"))
                                                                            .WithTypeArgumentList(
                                                                                TypeArgumentList(
                                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                                        IdentifierName(tableElement.Name)))))))))))))),
                                })))),
            };

            // This is the syntax for the body of the method.
            return statements;
        }

        /// <summary>
        /// Gets the 'empty the merge buckets' code.
        /// </summary>
        private List<StatementSyntax> MergeBucket(TableElement tableElement)
        {
            // The elements of the body are added to this collection as they are assembled.
            List<StatementSyntax> statements = new List<StatementSyntax>
            {
                //                    mergeBuckets[this.Accounts] = this.Accounts.MergeBucket(mergeBuckets[this.Accounts]);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.ElementAccessExpression(
                            SyntaxFactory.IdentifierName("mergeBuckets"))
                        .WithArgumentList(
                            SyntaxFactory.BracketedArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("MergeBucket")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ElementAccessExpression(
                                            SyntaxFactory.IdentifierName("mergeBuckets"))
                                        .WithArgumentList(
                                            SyntaxFactory.BracketedArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))))))))),

                //                    if (!mergeBuckets[this.Accounts].Any())
                //                    {
                //                        mergeBuckets.Remove(this.Accounts);
                //                    }
                SyntaxFactory.IfStatement(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ElementAccessExpression(
                                    SyntaxFactory.IdentifierName("mergeBuckets"))
                                .WithArgumentList(
                                    SyntaxFactory.BracketedArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))),
                                SyntaxFactory.IdentifierName("Any")))),
                    SyntaxFactory.Block(
                        SyntaxFactory.SingletonList<StatementSyntax>(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("mergeBuckets"),
                                        SyntaxFactory.IdentifierName("Remove")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))))))),
            };

            // This is the syntax for the body of the method.
            return statements;
        }

        /// <summary>
        /// Gets the 'empty the purge buckets' code.
        /// </summary>
        private List<StatementSyntax> PurgeBucket(TableElement tableElement)
        {
            // The elements of the body are added to this collection as they are assembled.
            List<StatementSyntax> statements = new List<StatementSyntax>
            {
                //                    purgeBuckets[this.Accounts] = this.Accounts.PurgeBucket(purgeBuckets[this.Accounts]);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.ElementAccessExpression(
                            SyntaxFactory.IdentifierName("purgeBuckets"))
                        .WithArgumentList(
                            SyntaxFactory.BracketedArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("PurgeBucket")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ElementAccessExpression(
                                            SyntaxFactory.IdentifierName("purgeBuckets"))
                                        .WithArgumentList(
                                            SyntaxFactory.BracketedArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))))))))),

                //                    if (!purgeBuckets[this.Accounts].Any())
                //                    {
                //                        purgeBuckets.Remove(this.Accounts);
                //                    }
                SyntaxFactory.IfStatement(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ElementAccessExpression(
                                    SyntaxFactory.IdentifierName("purgeBuckets"))
                                .WithArgumentList(
                                    SyntaxFactory.BracketedArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))),
                                SyntaxFactory.IdentifierName("Any")))),
                    SyntaxFactory.Block(
                        SyntaxFactory.SingletonList<StatementSyntax>(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("purgeBuckets"),
                                        SyntaxFactory.IdentifierName("Remove")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))))))),
            };

            // This is the syntax for the body of the method.
            return statements;
        }
    }
}