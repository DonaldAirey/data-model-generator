// <copyright file="DisplayLocksMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RecordSetClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to prepare a resource for a transaction completion.
    /// </summary>
    public class DisplayLocksMethod : SyntaxElement
    {
        /// <summary>
        /// The table element.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayLocksMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        public DisplayLocksMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "DisplayLocks";

            //        /// <summary>
            //        /// Display the locks on any records in the set.
            //        /// </summary>
            //        public void DisplayLocks()
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                SyntaxFactory.Identifier(this.Name))
            .WithModifiers(this.Modifiers)
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
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            if (this.Lock.IsReadLockHeld)
                //            {
                //                Debug.WriteLine($"ReadLock held on {this.Name}");
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Lock")),
                            SyntaxFactory.IdentifierName("IsReadLockHeld")),
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("Debug"),
                                            SyntaxFactory.IdentifierName("WriteLine")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.InterpolatedStringExpression(
                                                        SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                                    .WithContents(
                                                        SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                            new InterpolatedStringContentSyntax[]
                                                            {
                                                                SyntaxFactory.InterpolatedStringText()
                                                                .WithTextToken(
                                                                    SyntaxFactory.Token(
                                                                        SyntaxFactory.TriviaList(),
                                                                        SyntaxKind.InterpolatedStringTextToken,
                                                                        "ReadLock held on ",
                                                                        "ReadLock held on ",
                                                                        SyntaxFactory.TriviaList())),
                                                                SyntaxFactory.Interpolation(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.ThisExpression(),
                                                                        SyntaxFactory.IdentifierName("Name")))
                                                            })))))))))));

                //            if (this.Lock.IsUpgradeableReadLockHeld)
                //            {
                //                Debug.WriteLine($"UpgradableReadLock held on {this.Name}");
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Lock")),
                            SyntaxFactory.IdentifierName("IsUpgradeableReadLockHeld")),
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("Debug"),
                                            SyntaxFactory.IdentifierName("WriteLine")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.InterpolatedStringExpression(
                                                        SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                                    .WithContents(
                                                        SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                            new InterpolatedStringContentSyntax[]
                                                            {
                                                                SyntaxFactory.InterpolatedStringText()
                                                                .WithTextToken(
                                                                    SyntaxFactory.Token(
                                                                        SyntaxFactory.TriviaList(),
                                                                        SyntaxKind.InterpolatedStringTextToken,
                                                                        "UpgradableReadLock held on ",
                                                                        "UpgradableReadLock held on ",
                                                                        SyntaxFactory.TriviaList())),
                                                                SyntaxFactory.Interpolation(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.ThisExpression(),
                                                                        SyntaxFactory.IdentifierName("Name")))
                                                            })))))))))));

                //            if (this.Lock.IsWriteLockHeld)
                //            {
                //                Debug.WriteLine($"WriteLock held on {this.Name}");
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Lock")),
                            SyntaxFactory.IdentifierName("IsWriteLockHeld")),
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("Debug"),
                                            SyntaxFactory.IdentifierName("WriteLine")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.InterpolatedStringExpression(
                                                        SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                                    .WithContents(
                                                        SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                            new InterpolatedStringContentSyntax[]
                                                            {
                                                                SyntaxFactory.InterpolatedStringText()
                                                                .WithTextToken(
                                                                    SyntaxFactory.Token(
                                                                        SyntaxFactory.TriviaList(),
                                                                        SyntaxKind.InterpolatedStringTextToken,
                                                                        "WriteLock held on ",
                                                                        "WriteLock held on ",
                                                                        SyntaxFactory.TriviaList())),
                                                                SyntaxFactory.Interpolation(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.ThisExpression(),
                                                                        SyntaxFactory.IdentifierName("Name")))
                                                            })))))))))));

                // Display the lock status for each of the unique key indices.
                foreach (UniqueKeyElement uniqueKeyElement in this.tableElement.UniqueKeys)
                {
                    //            if (this.BuyerKey.Lock.IsReadLockHeld)
                    //            {
                    //                Debug.WriteLine($"ReadLock held on {this.BuyerKey.Name}");
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("IsReadLockHeld")),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("Debug"),
                                                SyntaxFactory.IdentifierName("WriteLine")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.InterpolatedStringExpression(
                                                            SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                                        .WithContents(
                                                            SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                                new InterpolatedStringContentSyntax[]
                                                                {
                                                                    SyntaxFactory.InterpolatedStringText()
                                                                    .WithTextToken(
                                                                        SyntaxFactory.Token(
                                                                            SyntaxFactory.TriviaList(),
                                                                            SyntaxKind.InterpolatedStringTextToken,
                                                                            "ReadLock held on ",
                                                                            "ReadLock held on ",
                                                                            SyntaxFactory.TriviaList())),
                                                                    SyntaxFactory.Interpolation(
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                SyntaxFactory.ThisExpression(),
                                                                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                                                            SyntaxFactory.IdentifierName("Name")))
                                                                })))))))))));

                    //            if (this.BuyerKey.Lock.IsUpgradeableReadLockHeld)
                    //            {
                    //                System.Diagnostics.Debug.WriteLine($"UpgradableReadLock held on {this.BuyerKey.Name}");
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("IsUpgradeableReadLockHeld")),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("Debug"),
                                                SyntaxFactory.IdentifierName("WriteLine")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.InterpolatedStringExpression(
                                                            SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                                        .WithContents(
                                                            SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                                new InterpolatedStringContentSyntax[]
                                                                {
                                                                    SyntaxFactory.InterpolatedStringText()
                                                                    .WithTextToken(
                                                                        SyntaxFactory.Token(
                                                                            SyntaxFactory.TriviaList(),
                                                                            SyntaxKind.InterpolatedStringTextToken,
                                                                            "UpgradableReadLock held on ",
                                                                            "UpgradableReadLock held on ",
                                                                            SyntaxFactory.TriviaList())),
                                                                    SyntaxFactory.Interpolation(
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                SyntaxFactory.ThisExpression(),
                                                                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                                                            SyntaxFactory.IdentifierName("Name")))
                                                                })))))))))));

                    //                    if (this.BuyerKey.Lock.IsWriteLockHeld)
                    //                    {
                    //                        Debug.WriteLine($"WriteLock held on {this.BuyerKey.Name}");
                    //                    }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("IsWriteLockHeld")),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("Debug"),
                                                SyntaxFactory.IdentifierName("WriteLine")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.InterpolatedStringExpression(
                                                            SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                                        .WithContents(
                                                            SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                                new InterpolatedStringContentSyntax[]
                                                                {
                                                                    SyntaxFactory.InterpolatedStringText()
                                                                    .WithTextToken(
                                                                        SyntaxFactory.Token(
                                                                            SyntaxFactory.TriviaList(),
                                                                            SyntaxKind.InterpolatedStringTextToken,
                                                                            "WriteLock held on ",
                                                                            "WriteLock held on ",
                                                                            SyntaxFactory.TriviaList())),
                                                                    SyntaxFactory.Interpolation(
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                SyntaxFactory.ThisExpression(),
                                                                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                                                            SyntaxFactory.IdentifierName("Name")))
                                                                })))))))))));
                }

                // Display the lock status for each of the foreign key indices.
                foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ParentKeys)
                {
                    //            if (this.BuyerKey.Lock.IsReadLockHeld)
                    //            {
                    //                Debug.WriteLine($"ReadLock held on {this.BuyerKey.Name}");
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("IsReadLockHeld")),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("Debug"),
                                                SyntaxFactory.IdentifierName("WriteLine")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.InterpolatedStringExpression(
                                                            SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                                        .WithContents(
                                                            SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                                new InterpolatedStringContentSyntax[]
                                                                {
                                                                    SyntaxFactory.InterpolatedStringText()
                                                                    .WithTextToken(
                                                                        SyntaxFactory.Token(
                                                                            SyntaxFactory.TriviaList(),
                                                                            SyntaxKind.InterpolatedStringTextToken,
                                                                            "ReadLock held on ",
                                                                            "ReadLock held on ",
                                                                            SyntaxFactory.TriviaList())),
                                                                    SyntaxFactory.Interpolation(
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                SyntaxFactory.ThisExpression(),
                                                                                SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                                                            SyntaxFactory.IdentifierName("Name")))
                                                                })))))))))));

                    //            if (this.BuyerKey.Lock.IsUpgradeableReadLockHeld)
                    //            {
                    //                System.Diagnostics.Debug.WriteLine($"UpgradableReadLock held on {this.BuyerKey.Name}");
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("IsUpgradeableReadLockHeld")),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("Debug"),
                                                SyntaxFactory.IdentifierName("WriteLine")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.InterpolatedStringExpression(
                                                            SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                                        .WithContents(
                                                            SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                                new InterpolatedStringContentSyntax[]
                                                                {
                                                                    SyntaxFactory.InterpolatedStringText()
                                                                    .WithTextToken(
                                                                        SyntaxFactory.Token(
                                                                            SyntaxFactory.TriviaList(),
                                                                            SyntaxKind.InterpolatedStringTextToken,
                                                                            "UpgradableReadLock held on ",
                                                                            "UpgradableReadLock held on ",
                                                                            SyntaxFactory.TriviaList())),
                                                                    SyntaxFactory.Interpolation(
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                SyntaxFactory.ThisExpression(),
                                                                                SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                                                            SyntaxFactory.IdentifierName("Name")))
                                                                })))))))))));

                    //                    if (this.BuyerKey.Lock.IsWriteLockHeld)
                    //                    {
                    //                        Debug.WriteLine($"WriteLock held on {this.BuyerKey.Name}");
                    //                    }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("IsWriteLockHeld")),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("Debug"),
                                                SyntaxFactory.IdentifierName("WriteLine")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.InterpolatedStringExpression(
                                                            SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                                        .WithContents(
                                                            SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                                new InterpolatedStringContentSyntax[]
                                                                {
                                                                    SyntaxFactory.InterpolatedStringText()
                                                                    .WithTextToken(
                                                                        SyntaxFactory.Token(
                                                                            SyntaxFactory.TriviaList(),
                                                                            SyntaxKind.InterpolatedStringTextToken,
                                                                            "WriteLock held on ",
                                                                            "WriteLock held on ",
                                                                            SyntaxFactory.TriviaList())),
                                                                    SyntaxFactory.Interpolation(
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                SyntaxFactory.ThisExpression(),
                                                                                SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                                                            SyntaxFactory.IdentifierName("Name")))
                                                                })))))))))));
                }

                //            foreach (Buyer buyer in this.collection.Values)
                //            {
                //                buyer.DisplayLocks();
                //            }
                statements.Add(
                    SyntaxFactory.ForEachStatement(
                        SyntaxFactory.IdentifierName(this.tableElement.Name),
                        SyntaxFactory.Identifier(this.tableElement.Name.ToCamelCase()),
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("collection")),
                            SyntaxFactory.IdentifierName("Values")),
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase()),
                                            SyntaxFactory.IdentifierName("DisplayLocks"))))))));

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
                //        /// Display the locks on any records in the set.
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
                                                $" Display the locks on any records in the set.",
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
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                   });
            }
        }
    }
}