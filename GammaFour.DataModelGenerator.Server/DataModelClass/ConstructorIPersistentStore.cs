// <copyright file="ConstructorIPersistentStore.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.DataModelClass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a constructor.
    /// </summary>
    public class ConstructorIPersistentStore : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorIPersistentStore"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The table schema.</param>
        public ConstructorIPersistentStore(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.Name = this.xmlSchemaDocument.Name;

            //        /// <summary>
            //        /// Initializes a new instance of the <see cref="DataModel"/> class.
            //        /// </summary>
            //        public DataModel(IPersistentStore persistentStore)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.ConstructorDeclaration(
                    SyntaxFactory.Identifier(this.Name))
                .WithAttributeLists(this.AttributeLists)
                .WithModifiers(this.Modifiers)
                .WithParameterList(this.ParameterList)
                .WithBody(this.Body)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> AddValidTransactionBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                            this.AddTransaction(transactionItem);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("AddTransaction")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("transactionItem")))))));

                //                            transactionLog[index] = null;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.ElementAccessExpression(
                                SyntaxFactory.IdentifierName("transactionLog"))
                            .WithArgumentList(
                                SyntaxFactory.BracketedArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("index"))))),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression))));

                //                            long rowVersion = (long)transactionItem[DataModel.rowVersionIndex[(int)transactionItem[0]]];
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
                                        SyntaxFactory.CastExpression(
                                            SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(SyntaxKind.LongKeyword)),
                                            SyntaxFactory.ElementAccessExpression(
                                                SyntaxFactory.IdentifierName("transactionItem"))
                                            .WithArgumentList(
                                                SyntaxFactory.BracketedArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.ElementAccessExpression(
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name),
                                                                    SyntaxFactory.IdentifierName("rowVersionIndex")))
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
                                                                                                    SyntaxFactory.Literal(0))))))))))))))))))))));

                //                            this.rowVersionField = rowVersion > this.rowVersionField ? rowVersion : this.rowVersionField;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("rowVersionField")),
                            SyntaxFactory.ConditionalExpression(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.GreaterThanExpression,
                                    SyntaxFactory.IdentifierName("rowVersion"),
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("rowVersionField"))),
                                SyntaxFactory.IdentifierName("rowVersion"),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("rowVersionField"))))));

                //                            mergedItems++;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.PostfixUnaryExpression(
                            SyntaxKind.PostIncrementExpression,
                            SyntaxFactory.IdentifierName("mergedItems"))));

                // This is the complete block.
                return statements;
            }
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

                //        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Reviewed")]
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
                                                        SyntaxFactory.Literal("CA1506:AvoidExcessiveClassCoupling"))),
                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                SyntaxFactory.AttributeArgument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        SyntaxFactory.Literal("Generated by a tool.")))
                                                .WithNameEquals(
                                                    SyntaxFactory.NameEquals(
                                                        SyntaxFactory.IdentifierName("Justification")))
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

                //            if (persistentStore == null)
                //            {
                //                throw new ArgumentNullException("persistentStore");
                //            }
                statements.Add(CheckNullArgument.GetSyntax("persistentStore"));

                //            this.persistentStore = persistentStore;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("persistentStore")),
                            SyntaxFactory.IdentifierName("persistentStore"))));

                // This will create a table for merging the service data with the client-side data.
                for (int tableIndex = 0; tableIndex < this.xmlSchemaDocument.Tables.Count; tableIndex++)
                {
                    TableElement tableElement = this.xmlSchemaDocument.Tables[tableIndex];

                    //            this.transactionHandlers[0] = (d) => this.Configuration.Merge(d);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.ElementAccessExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("transactionHandlers")))
                                .WithArgumentList(
                                    SyntaxFactory.BracketedArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(tableIndex)))))),
                                SyntaxFactory.ParenthesizedLambdaExpression(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName(tableElement.Name)),
                                            SyntaxFactory.IdentifierName("MergeRecord")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.IdentifierName("d"))))))
                                .WithParameterList(
                                    SyntaxFactory.ParameterList(
                                        SyntaxFactory.SingletonSeparatedList<ParameterSyntax>(
                                            SyntaxFactory.Parameter(
                                                SyntaxFactory.Identifier("d"))))))));
                }

                // Initialize each of the tables.
                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables.OrderBy(t => t.Name))
                {
                    //            this.configurationTableField = new ConfigurationTable(this);
                    string fieldName = tableElement.Name;
                    string typeName = string.Format(CultureInfo.InvariantCulture, "{0}Table", tableElement.Name);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(fieldName)),
                                SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(typeName))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(SyntaxFactory.ThisExpression())))))));
                }

                // Initialize each of the (non-primary key) unique keys.
                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables.OrderBy(t => t.Name))
                {
                    foreach (UniqueKeyElement uniqueKeyElement in tableElement.UniqueKeys)
                    {
                        //            this.CountryExternalId0KeyIndex = new CountryExternalId0KeyIndex(this);
                        statements.Add(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                    SyntaxFactory.ObjectCreationExpression(
                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Name))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.ThisExpression())))))));
                    }
                }

                // This will initialize each of the foreign relations.
                foreach (ForeignKeyElement foreignKeyElement in this.xmlSchemaDocument.ForeignKeys)
                {
                    //            this.CountryCustomerCountryIdKeyIndex = new CountryCustomerCountryIdKeyIndex(this);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName(foreignKeyElement.Name))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.ThisExpression())))))));
                }

                //            this.masterIdentifier = Guid.NewGuid();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("identifierField")),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("Guid"),
                                    SyntaxFactory.IdentifierName("NewGuid"))))));

                //            this.transactionLog = new LinkedList<TransactionLogItem>();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("transactionLog")),
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.GenericName(SyntaxFactory.Identifier("LinkedList"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                            SyntaxFactory.IdentifierName("TransactionLogItem")))))
                            .WithArgumentList(SyntaxFactory.ArgumentList()))));

                //            this.ReadStarting();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("ReadStarting")))));

                //            List<object[]> transactionLog = this.persistentStore.Read();
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("List"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                        SyntaxFactory.ArrayType(
                                            SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                        .WithRankSpecifiers(
                                            SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                                SyntaxFactory.ArrayRankSpecifier(
                                                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                        SyntaxFactory.OmittedArraySizeExpression()))))))))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("transactionLog"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName("persistentStore")),
                                                SyntaxFactory.IdentifierName("Read")))))))));

                //            int mergedItems = 0;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.IntKeyword)))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("mergedItems"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NumericLiteralExpression,
                                            SyntaxFactory.Literal(0))))))));

                //            while (mergedItems < transactionLog.Count)
                //            {
                //                <MergeTransactionLogBlock>
                //            }
                statements.Add(
                    SyntaxFactory.WhileStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.LessThanExpression,
                            SyntaxFactory.IdentifierName("mergedItems"),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("transactionLog"),
                                SyntaxFactory.IdentifierName("Count"))),
                        SyntaxFactory.Block(this.MergeTransactionLogBlock)));

                //            this.ReadCompleted();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("ReadCompleted")))));

                // This is the syntax for the body of the constructor.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> CheckForValidTransactionBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                        if (this.transactionHandlers[(int)transactionItem[0]](transactionItem))
                //                        {
                //                            <this.AddValidTransaction>
                //                        }
                statements.Add(
                    SyntaxFactory.IfStatement(
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
                                        SyntaxFactory.IdentifierName("transactionItem"))))),
                        SyntaxFactory.Block(this.AddValidTransactionBlock)));

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
                //        /// Initializes a new instance of the <see cref="DataModel"/> class.
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
                                                string.Format(
                                                    CultureInfo.InvariantCulture,
                                                    " Initializes a new instance of the <see cref=\"{0}\"/> class.",
                                                    this.Name),
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

                //                /// <param name="persistentStore">The persistent store.</param>
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
                                                    " <param name=\"persistentStore\">The persistent store.</param>",
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
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> GetNextTransactionBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    object[] transactionItem = transactionLog[index];
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
                                            SyntaxFactory.IdentifierName("transactionLog"))
                                        .WithArgumentList(
                                            SyntaxFactory.BracketedArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("index")))))))))));

                //                    if (transactionItem != null)
                //                    {
                //                        <CheckForValidTransactionBlock>
                //                    }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName("transactionItem"),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(this.CheckForValidTransactionBlock)));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> MergeTransactionLogBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                for (int index = 0; index < transactionLog.Count; index++)
                //                {
                //                    <AddTransactionBlock>
                //                }
                statements.Add(
                    SyntaxFactory.ForStatement(
                        SyntaxFactory.Block(this.GetNextTransactionBlock))
                    .WithDeclaration(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.IntKeyword)))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("index"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NumericLiteralExpression,
                                            SyntaxFactory.Literal(0)))))))
                    .WithCondition(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.LessThanExpression,
                            SyntaxFactory.IdentifierName("index"),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("transactionLog"),
                                SyntaxFactory.IdentifierName("Count"))))
                    .WithIncrementors(
                        SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                            SyntaxFactory.PostfixUnaryExpression(
                                SyntaxKind.PostIncrementExpression,
                                SyntaxFactory.IdentifierName("index")))));

                // This is the complete block.
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
                // public
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
                // Create a list of parameters from the columns in the unique constraint.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();

                // ConfigurationTable table, ConfigurationData data
                parameters.Add(SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("persistentStore"))
                    .WithType(
                        SyntaxFactory.IdentifierName("IPersistentStore")));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }
    }
}