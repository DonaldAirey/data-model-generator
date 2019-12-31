// <copyright file="MergeMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2019 - Gamma Four, Inc.  All Rights Reserved.
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
    public class MergeMethod : SyntaxElement
    {
        /// <summary>
        /// The XML schema.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="MergeMethod"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The XML document schema.</param>
        public MergeMethod(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.Name = "Merge";

            //        /// <summary>
            //        /// Merge the results of an incremental update.
            //        /// </summary>
            //        /// <param name="jObject">The JSON object containg the incremental data.</param>
            //        public void Merge(JObject jObject)
            //        {
            this.Syntax = MethodDeclaration(
                    PredefinedType(
                        Token(SyntaxKind.VoidKeyword)),
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
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <summary>
                //        /// Initializes a new instance of the <see cref="DataModel"/> class.
                //        /// </summary>
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
                                        }))))));

                //        /// <param name="jObject">The JSON object containg the incremental data.</param>
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
                                                    $" <param name=\"jObject\">The JSON object containg the incremental data.</param>",
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
        /// Generate code to empty the merge buckets.
        /// </summary>
        private static List<StatementSyntax> EmptyMergeBuckets
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                foreach (IMergable mergable in mergeBuckets.Keys.ToList())
                //                {
                //                    mergeBuckets[mergable] = mergable.Merge(mergeBuckets[mergable]);
                //                }
                statements.Add(
                    ForEachStatement(
                        IdentifierName("IMergable"),
                        Identifier("mergable"),
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("mergeBuckets"),
                                    IdentifierName("Keys")),
                                IdentifierName("ToList"))),
                        Block(MergeMethod.MergeBucket)));

                // This is the syntax for the body of the method.
                return statements;
            }
        }

        /// <summary>
        /// Generate code to empty the purge buckets.
        /// </summary>
        private static List<StatementSyntax> EmptyPurgeBuckets
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                foreach (IPurgable purgable in mergeBuckets.Keys.ToList())
                //                {
                //                    <PurgeBucket>
                //                }
                statements.Add(
                    ForEachStatement(
                        IdentifierName("IPurgable"),
                        Identifier("purgable"),
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("purgeBuckets"),
                                    IdentifierName("Keys")),
                                IdentifierName("ToList"))),
                        Block(MergeMethod.PurgeBucket)));

                // This is the syntax for the body of the method.
                return statements;
            }
        }

        /// <summary>
        /// Generate code to merge a bucket.
        /// </summary>
        private static List<StatementSyntax> MergeBucket
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    mergeBuckets[mergable] = mergable.Merge(mergeBuckets[mergable]);
                statements.Add(
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            ElementAccessExpression(
                                IdentifierName("mergeBuckets"))
                            .WithArgumentList(
                                BracketedArgumentList(
                                    SingletonSeparatedList<ArgumentSyntax>(
                                        Argument(
                                            IdentifierName("mergable"))))),
                            InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("mergable"),
                                    IdentifierName("Merge")))
                            .WithArgumentList(
                                ArgumentList(
                                    SingletonSeparatedList<ArgumentSyntax>(
                                        Argument(
                                            ElementAccessExpression(
                                                IdentifierName("mergeBuckets"))
                                            .WithArgumentList(
                                                BracketedArgumentList(
                                                    SingletonSeparatedList<ArgumentSyntax>(
                                                        Argument(
                                                            IdentifierName("mergable"))))))))))));

                // This is the syntax for the body of the method.
                return statements;
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
                        Identifier("jObject"))
                    .WithType(
                        IdentifierName("JObject")));

                // This is the complete parameter specification for this constructor.
                return ParameterList(SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Generate code to purge a bucket.
        /// </summary>
        private static List<StatementSyntax> PurgeBucket
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    purgeBuckets[mergable] = mergable.Purge(purgeBuckets[mergable]);
                statements.Add(
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            ElementAccessExpression(
                                IdentifierName("purgeBuckets"))
                            .WithArgumentList(
                                BracketedArgumentList(
                                    SingletonSeparatedList<ArgumentSyntax>(
                                        Argument(
                                            IdentifierName("purgable"))))),
                            InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("purgable"),
                                    IdentifierName("Purge")))
                            .WithArgumentList(
                                ArgumentList(
                                    SingletonSeparatedList<ArgumentSyntax>(
                                        Argument(
                                            ElementAccessExpression(
                                                IdentifierName("purgeBuckets"))
                                            .WithArgumentList(
                                                BracketedArgumentList(
                                                    SingletonSeparatedList<ArgumentSyntax>(
                                                        Argument(
                                                            IdentifierName("purgable"))))))))))));

                // This is the syntax for the body of the method.
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

                //            Dictionary<IMergable, IEnumerable<object>> mergeBuckets = new Dictionary<IMergable, IEnumerable<object>>();
                statements.Add(
                    LocalDeclarationStatement(
                        VariableDeclaration(
                            GenericName(
                                Identifier("Dictionary"))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SeparatedList<TypeSyntax>(
                                        new SyntaxNodeOrToken[]{
                                            IdentifierName("IMergable"),
                                            Token(SyntaxKind.CommaToken),
                                            GenericName(
                                                Identifier("IEnumerable"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        PredefinedType(
                                                            Token(SyntaxKind.ObjectKeyword)))))}))))
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
                                                        new SyntaxNodeOrToken[]{
                                                            IdentifierName("IMergable"),
                                                            Token(SyntaxKind.CommaToken),
                                                            GenericName(
                                                                Identifier("IEnumerable"))
                                                            .WithTypeArgumentList(
                                                                TypeArgumentList(
                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                        PredefinedType(
                                                                            Token(SyntaxKind.ObjectKeyword)))))}))))
                                        .WithArgumentList(
                                            ArgumentList())))))));

                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    //            JToken accountsToken = jObject["accounts"];
                    //            JToken canonsToken = jObject["canons"];
                    //            JToken entitiesToken = jObject["entities"];
                    statements.Add(
                        LocalDeclarationStatement(
                            VariableDeclaration(
                                IdentifierName("JToken"))
                            .WithVariables(
                                SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    VariableDeclarator(
                                        Identifier($"{tableElement.Name.ToPlural().ToCamelCase()}Token"))
                                    .WithInitializer(
                                        EqualsValueClause(
                                            ElementAccessExpression(
                                                IdentifierName("jObject"))
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
                            Block(MergeMethod.GetMergeTable(tableElement))));
                }

                //            while (mergeBuckets.Values.Where(v => v.Any()).Any())
                //            {
                //                <EmptyMergeBuckets>
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
                                            IdentifierName("mergeBuckets"),
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
                        Block(MergeMethod.EmptyMergeBuckets)));
                
                //            Dictionary<IPurgable, IEnumerable<object>> purgeBuckets = new Dictionary<IPurgable, IEnumerable<object>>();
                statements.Add(
                    LocalDeclarationStatement(
                        VariableDeclaration(
                            GenericName(
                                Identifier("Dictionary"))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SeparatedList<TypeSyntax>(
                                        new SyntaxNodeOrToken[]{
                                            IdentifierName("IPurgable"),
                                            Token(SyntaxKind.CommaToken),
                                            GenericName(
                                                Identifier("IEnumerable"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SingletonSeparatedList<TypeSyntax>(
                                                        PredefinedType(
                                                            Token(SyntaxKind.ObjectKeyword)))))}))))
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
                                                        new SyntaxNodeOrToken[]{
                                                            IdentifierName("IPurgable"),
                                                            Token(SyntaxKind.CommaToken),
                                                            GenericName(
                                                                Identifier("IEnumerable"))
                                                            .WithTypeArgumentList(
                                                                TypeArgumentList(
                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                        PredefinedType(
                                                                            Token(SyntaxKind.ObjectKeyword)))))}))))
                                        .WithArgumentList(
                                            ArgumentList())))))));

                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    //            JToken deletedAccountsToken = jObject["deletedAccounts"];
                    //            JToken deletedCanonsToken = jObject["deletedCanons"];
                    //            JToken deletedEntitiesToken = jObject["deletedEntities"];
                    statements.Add(
                        LocalDeclarationStatement(
                            VariableDeclaration(
                                IdentifierName("JToken"))
                            .WithVariables(
                                SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    VariableDeclarator(
                                        Identifier($"deleted{tableElement.Name.ToPlural()}Token"))
                                    .WithInitializer(
                                        EqualsValueClause(
                                            ElementAccessExpression(
                                                IdentifierName("jObject"))
                                            .WithArgumentList(
                                                BracketedArgumentList(
                                                    SingletonSeparatedList<ArgumentSyntax>(
                                                        Argument(
                                                            LiteralExpression(
                                                                SyntaxKind.StringLiteralExpression,
                                                                Literal($"deleted{tableElement.Name.ToPlural()}"))))))))))));
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
                                IdentifierName($"deleted{tableElement.Name.ToPlural()}Token"),
                                LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            Block(MergeMethod.GetPurgeTable(tableElement))));
                }

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
                        Block(MergeMethod.EmptyPurgeBuckets)));

                // This is the syntax for the body of the method.
                return Block(List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static List<StatementSyntax> GetMergeTable(TableElement tableElement)
        {
            // The elements of the body are added to this collection as they are assembled.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                mergeBuckets.Add(this.Accounts, this.Accounts.Merge(accountsToken.ToObject<List<Account>>()));
            statements.Add(
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
                                                IdentifierName("Merge")))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList<ArgumentSyntax>(
                                                    Argument(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName($"{tableElement.Name.ToPlural().ToCamelCase()}Token"),
                                                                GenericName(
                                                                    Identifier("ToObject"))
                                                                .WithTypeArgumentList(
                                                                    TypeArgumentList(
                                                                        SingletonSeparatedList<TypeSyntax>(
                                                                            GenericName(
                                                                                Identifier("List"))
                                                                            .WithTypeArgumentList(
                                                                                TypeArgumentList(
                                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                                        IdentifierName(tableElement.Name))))))))))))))
                                })))));

            // This is the syntax for the body of the method.
            return statements;
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static List<StatementSyntax> GetPurgeTable(TableElement tableElement)
        {
            // The elements of the body are added to this collection as they are assembled.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                purgeBuckets.Add(this.Accounts, this.Accounts.Purge(deletedAccountsToken.ToObject<List<Account>>()));
            statements.Add(
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
                                                IdentifierName("Purge")))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList<ArgumentSyntax>(
                                                    Argument(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName($"deleted{tableElement.Name.ToPlural()}Token"),
                                                                GenericName(
                                                                    Identifier("ToObject"))
                                                                .WithTypeArgumentList(
                                                                    TypeArgumentList(
                                                                        SingletonSeparatedList<TypeSyntax>(
                                                                            GenericName(
                                                                                Identifier("List"))
                                                                            .WithTypeArgumentList(
                                                                                TypeArgumentList(
                                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                                        IdentifierName(tableElement.Name))))))))))))))
                                })))));

            // This is the syntax for the body of the method.
            return statements;
        }
    }
}