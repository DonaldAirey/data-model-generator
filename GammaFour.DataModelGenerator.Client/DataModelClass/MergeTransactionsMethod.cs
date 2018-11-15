// <copyright file="MergeTransactionsMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
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

    /// <summary>
    /// Creates a method to dispose of the resources of a class.
    /// </summary>
    public class MergeTransactionsMethod : SyntaxElement
    {
        /// <summary>
        /// The data model schema.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="MergeTransactionsMethod"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The data model schema.</param>
        public MergeTransactionsMethod(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.Name = "MergeTransactions";

            //        /// <summary>
            //        /// Merge the data from the service into the client's data model.
            //        /// </summary>
            //        /// <param name="state">The (unused) thread state.</param>
            //        private void MergeTransactions(object state)
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

                //        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Calls user code.")]
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
                                                    SyntaxFactory.Literal("Microsoft.Design"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("CA1031:DoNotCatchGeneralExceptionTypes"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("Calls user code.")))
                                            .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("Justification")))
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

                //            int batchCounter = 0;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.IntKeyword)))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("batchCounter"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NumericLiteralExpression,
                                            SyntaxFactory.Literal(0))))))));

                //            while (batchCounter++ < DataModel.batchSize)
                statements.Add(
                    SyntaxFactory.WhileStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.LessThanExpression,
                            SyntaxFactory.PostfixUnaryExpression(
                                SyntaxKind.PostIncrementExpression,
                                SyntaxFactory.IdentifierName("batchCounter")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name),
                                SyntaxFactory.IdentifierName("batchSize"))),
                        SyntaxFactory.Block(this.MergeBatchBlock)));

                //            new Task(() => { Task.Delay(DataModel.courtesyInterval); this.synchronizationContext.Post(this.MergeTransactions, null); }).Start();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName("Task"))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.ParenthesizedLambdaExpression(
                                                    SyntaxFactory.Block(
                                                        SyntaxFactory.ExpressionStatement(
                                                            SyntaxFactory.AwaitExpression(
                                                                SyntaxFactory.InvocationExpression(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.IdentifierName("Task"),
                                                                        SyntaxFactory.IdentifierName("Delay")))
                                                                .WithArgumentList(
                                                                    SyntaxFactory.ArgumentList(
                                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                            SyntaxFactory.Argument(
                                                                                SyntaxFactory.MemberAccessExpression(
                                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name),
                                                                                    SyntaxFactory.IdentifierName("courtesyInterval")))))))),
                                                        SyntaxFactory.ExpressionStatement(
                                                            SyntaxFactory.InvocationExpression(
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.ThisExpression(),
                                                                        SyntaxFactory.IdentifierName("synchronizationContext")),
                                                                    SyntaxFactory.IdentifierName("Post")))
                                                            .WithArgumentList(
                                                                SyntaxFactory.ArgumentList(
                                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                                        new SyntaxNodeOrToken[]
                                                                        {
                                                                            SyntaxFactory.Argument(
                                                                                SyntaxFactory.MemberAccessExpression(
                                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                                    SyntaxFactory.ThisExpression(),
                                                                                    SyntaxFactory.IdentifierName("MergeTransactions"))),
                                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                            SyntaxFactory.Argument(
                                                                                SyntaxFactory.LiteralExpression(
                                                                                    SyntaxKind.NullLiteralExpression))
                                                                        }))))))
                                                .WithAsyncKeyword(
                                                    SyntaxFactory.Token(SyntaxKind.AsyncKeyword)))))),
                                SyntaxFactory.IdentifierName("Start")))));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
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
                //        /// Merge the data from the service into the client's data model.
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
                                                " Merge the data from the service into the client's data model.",
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

                //        /// <param name="state">The (unused) thread state.</param>
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
                                                    " <param name=\"state\">The (unused) thread state.</param>",
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
        /// Gets a block of code to see if we're still reading transactions from the service.
        /// </summary>
        private List<StatementSyntax> IsStillReadingBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    if (this.isReading)
                //                    {
                //                        <ScheduleReadBlock>
                //                    }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("isReading")),
                        SyntaxFactory.Block(this.ScheduleReadBlock)));

                //                    return;
                statements.Add(
                    SyntaxFactory.ReturnStatement());

                // This is the complete statement block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code to merge a batch of transactions.
        /// </summary>
        private List<StatementSyntax> MergeBatchBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                if (this.transactionLogIndex < 0)
                //                {
                //                    <IsStillReadingBlock>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.LessThanExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("transactionLogIndex")),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(0))),
                        SyntaxFactory.Block(this.IsStillReadingBlock)));

                //                object[] transactionItem = this.transactionLog[this.transactionLogIndex--];
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.ArrayType(
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                            .WithRankSpecifiers(
                                SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                    SyntaxFactory.ArrayRankSpecifier(
                                        SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                            SyntaxFactory.OmittedArraySizeExpression())))))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("transactionItem"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ElementAccessExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName("transactionLog")))
                                        .WithArgumentList(
                                            SyntaxFactory.BracketedArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.PostfixUnaryExpression(
                                                            SyntaxKind.PostDecrementExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.ThisExpression(),
                                                                SyntaxFactory.IdentifierName("transactionLogIndex")))))))))))));

                //            try
                //            {
                //                <TryBlock1>
                //            }
                //            catch
                //            {
                //            }
                statements.Add(
                    SyntaxFactory.TryStatement(
                        SyntaxFactory.SingletonList<CatchClauseSyntax>(
                            SyntaxFactory.CatchClause()))
                    .WithBlock(
                        SyntaxFactory.Block(this.TryBlock1)));

                // This is the complete statement block.
                return statements;
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
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword)
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
                // The list of parameters to the destroy method.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();

                // bool isDisposing
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("state"))
                    .WithType(
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.ObjectKeyword))));

                // This is the complete set of comma separated parameters for the method.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets a block of code to see if we're still reading transactions from the service.
        /// </summary>
        private List<StatementSyntax> ScheduleReadBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                        new Task(async () => { await Task.Delay(DataModel.refreshInterval); this.synchronizationContext.Post(this.ReadTransactionsAsync, null); }).Start();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName("Task"))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.ParenthesizedLambdaExpression(
                                                    SyntaxFactory.Block(
                                                        SyntaxFactory.ExpressionStatement(
                                                            SyntaxFactory.AwaitExpression(
                                                                SyntaxFactory.InvocationExpression(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.IdentifierName("Task"),
                                                                        SyntaxFactory.IdentifierName("Delay")))
                                                                .WithArgumentList(
                                                                    SyntaxFactory.ArgumentList(
                                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                            SyntaxFactory.Argument(
                                                                                SyntaxFactory.MemberAccessExpression(
                                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name),
                                                                                    SyntaxFactory.IdentifierName("refreshInterval")))))))),
                                                        SyntaxFactory.ExpressionStatement(
                                                            SyntaxFactory.InvocationExpression(
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.ThisExpression(),
                                                                        SyntaxFactory.IdentifierName("synchronizationContext")),
                                                                    SyntaxFactory.IdentifierName("Post")))
                                                            .WithArgumentList(
                                                                SyntaxFactory.ArgumentList(
                                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                                        new SyntaxNodeOrToken[]
                                                                        {
                                                                            SyntaxFactory.Argument(
                                                                                SyntaxFactory.MemberAccessExpression(
                                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                                    SyntaxFactory.ThisExpression(),
                                                                                    SyntaxFactory.IdentifierName("ReadTransactionsAsync"))),
                                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                            SyntaxFactory.Argument(
                                                                                SyntaxFactory.LiteralExpression(
                                                                                    SyntaxKind.NullLiteralExpression))
                                                                        }))))))
                                                .WithAsyncKeyword(
                                                    SyntaxFactory.Token(SyntaxKind.AsyncKeyword)))))),
                                SyntaxFactory.IdentifierName("Start")))));

                // This is the complete statement block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> TryBlock1
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.transactionHandlers[(int)transactionItem[0]](transactionItem);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.ElementAccessExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("transactionHandlers")))
                            .WithArgumentList(
                                SyntaxFactory.BracketedArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.CastExpression(
                                                SyntaxFactory.PredefinedType(
                                                    SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                                                SyntaxFactory.ElementAccessExpression(
                                                    SyntaxFactory.IdentifierName("transactionItem"))
                                                .WithArgumentList(
                                                    SyntaxFactory.BracketedArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.LiteralExpression(
                                                                    SyntaxKind.NumericLiteralExpression,
                                                                    SyntaxFactory.Literal(0))))))))))))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("transactionItem")))))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }
    }
}