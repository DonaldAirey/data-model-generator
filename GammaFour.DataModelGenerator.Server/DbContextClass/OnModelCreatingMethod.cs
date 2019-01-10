// <copyright file="OnModelCreatingMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.DbContextClass
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
    public class OnModelCreatingMethod : SyntaxElement
    {
        /// <summary>
        /// The XML schema document.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnModelCreatingMethod"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The XML schema document.</param>
        public OnModelCreatingMethod(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.Name = "OnModelCreating";

            //        /// <inheritdoc/>
            //        protected override void OnModelCreating(ModelBuilder modelBuilder)
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
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // This will configure each of the tables and their indices.
                foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
                {
                    // Used as a variable when constructing the lambda expression.
                    string abbreviation = tableElement.Name[0].ToString().ToLower();

                    // Create a key for each index that is unique to the set.
                    foreach (UniqueKeyElement uniqueKeyElement in tableElement.UniqueKeys)
                    {
                        if (uniqueKeyElement.IsPrimaryKey)
                        {
                            statements.Add(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName("modelBuilder"),
                                                    SyntaxFactory.GenericName(
                                                        SyntaxFactory.Identifier("Entity"))
                                                    .WithTypeArgumentList(
                                                        SyntaxFactory.TypeArgumentList(
                                                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name)))))),
                                            SyntaxFactory.IdentifierName("HasKey")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(UniqueKeyExpression.GetUniqueKey(uniqueKeyElement, true)))))));
                        }
                        else
                        {
                            //            modelBuilder.Entity<Buyer>().HasIndex(b => b.ExternalId0).IsUnique();
                            statements.Add(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("modelBuilder"),
                                                            SyntaxFactory.GenericName(
                                                                SyntaxFactory.Identifier("Entity"))
                                                            .WithTypeArgumentList(
                                                                SyntaxFactory.TypeArgumentList(
                                                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name)))))),
                                                    SyntaxFactory.IdentifierName("HasIndex")))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(UniqueKeyExpression.GetUniqueKey(uniqueKeyElement, true))))),
                            SyntaxFactory.IdentifierName("IsUnique")))));
                        }
                    }

                    // Each column will have properties that need to be set by the Fluent API.
                    foreach (ColumnElement columnElement in tableElement.Columns)
                    {
                        // Autogenerated values.
                        if (columnElement.IsAutoIncrement)
                        {
                            //            modelBuilder.Entity<Country>().Property(c => c.CountryId).ValueGeneratedOnAdd();
                            statements.Add(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("modelBuilder"),
                                                            SyntaxFactory.GenericName(
                                                                SyntaxFactory.Identifier("Entity"))
                                                            .WithTypeArgumentList(
                                                                SyntaxFactory.TypeArgumentList(
                                                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                        SyntaxFactory.IdentifierName(tableElement.Name)))))),
                                                    SyntaxFactory.IdentifierName("Property")))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.SimpleLambdaExpression(
                                                                SyntaxFactory.Parameter(
                                                                    SyntaxFactory.Identifier(abbreviation)),
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.IdentifierName(abbreviation),
                                                                    SyntaxFactory.IdentifierName(columnElement.Name))))))),
                                            SyntaxFactory.IdentifierName("ValueGeneratedOnAdd")))));
                        }

                        // RowVersion columns
                        if (columnElement.IsRowVersion)
                        {
                            //            modelBuilder.Entity<Country>().Property(c => c.RowVersion).IsRowVersion();
                            statements.Add(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("modelBuilder"),
                                                            SyntaxFactory.GenericName(
                                                                SyntaxFactory.Identifier("Entity"))
                                                            .WithTypeArgumentList(
                                                                SyntaxFactory.TypeArgumentList(
                                                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                        SyntaxFactory.IdentifierName(tableElement.Name)))))),
                                                    SyntaxFactory.IdentifierName("Property")))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.SimpleLambdaExpression(
                                                                SyntaxFactory.Parameter(
                                                                    SyntaxFactory.Identifier(abbreviation)),
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.IdentifierName(abbreviation),
                                                                    SyntaxFactory.IdentifierName(columnElement.Name))))))),
                                            SyntaxFactory.IdentifierName("IsRowVersion")))));
                        }

                        // Provide a reasonble range for decimal datatypes
                        if (columnElement.Type == typeof(decimal))
                        {
                            //            modelBuilder.Entity<Offering>().Property(o => o.Cash).HasColumnType("decimal(18,2)");
                            statements.Add(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("modelBuilder"),
                                                            SyntaxFactory.GenericName(
                                                                SyntaxFactory.Identifier("Entity"))
                                                            .WithTypeArgumentList(
                                                                SyntaxFactory.TypeArgumentList(
                                                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                        SyntaxFactory.IdentifierName(tableElement.Name)))))),
                                                    SyntaxFactory.IdentifierName("Property")))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.SimpleLambdaExpression(
                                                                SyntaxFactory.Parameter(
                                                                    SyntaxFactory.Identifier(abbreviation)),
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.IdentifierName(abbreviation),
                                                                    SyntaxFactory.IdentifierName(columnElement.Name))))))),
                                            SyntaxFactory.IdentifierName("HasColumnType")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        SyntaxFactory.Literal("decimal(18,2)"))))))));
                        }
                    }

                    // The next chunk of code produces a Fluent API call for ignoring all the navigation columns.  The core of all this is an
                    // expression that will ignore the table that owns the records.
                    //            modelBuilder.Entity<Buyer>().Ignore(b => b.Buyers)
                    ExpressionSyntax ignoredProperties = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("modelBuilder"),
                                    SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier("Entity"))
                                    .WithTypeArgumentList(
                                        SyntaxFactory.TypeArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                SyntaxFactory.IdentifierName(tableElement.Name)))))),
                            SyntaxFactory.IdentifierName("Ignore")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.SimpleLambdaExpression(
                                        SyntaxFactory.Parameter(
                                            SyntaxFactory.Identifier(abbreviation)),
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName(abbreviation),
                                            SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))));

                    // .Ignore(b => b.State)
                    ignoredProperties = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            ignoredProperties,
                            SyntaxFactory.IdentifierName("Ignore")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.SimpleLambdaExpression(
                                        SyntaxFactory.Parameter(
                                            SyntaxFactory.Identifier(abbreviation)),
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName(abbreviation),
                                            SyntaxFactory.IdentifierName("RecordState")))))));

                    // Add an Ignore invocation for each of the parent set navigation properties.
                    // .Ignore(b => b.Country)
                    foreach (ForeignKeyElement foreignKeyElement in tableElement.ParentKeys)
                    {
                        ignoredProperties = SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ignoredProperties,
                                SyntaxFactory.IdentifierName("Ignore")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.SimpleLambdaExpression(
                                            SyntaxFactory.Parameter(
                                                SyntaxFactory.Identifier(abbreviation)),
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(abbreviation),
                                                SyntaxFactory.IdentifierName(foreignKeyElement.UniqueKey.Table.Name)))))));
                    }

                    // Add an Ignore invocation for each of the child set navigation properties.
                    // .Ignore(b => b.Subscriptions)
                    foreach (ForeignKeyElement foreignKeyElement in tableElement.ChildKeys)
                    {
                        ignoredProperties = SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ignoredProperties,
                                SyntaxFactory.IdentifierName("Ignore")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.SimpleLambdaExpression(
                                            SyntaxFactory.Parameter(
                                                SyntaxFactory.Identifier(abbreviation)),
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(abbreviation),
                                                SyntaxFactory.IdentifierName(foreignKeyElement.Table.Name.ToPlural())))))));
                    }

                    //            modelBuilder.Entity<Buyer>().Ignore(b => b.Buyers).Ignore(b => b.Country).Ignore(b => b.Province).Ignore(b => b.Subscriptions);
                    statements.Add(SyntaxFactory.ExpressionStatement(ignoredProperties));
                }

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
                // This is used to collect the trivia.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <inheritdoc/>
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
                                                " <inheritdoc/>",
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
                        SyntaxFactory.Token(SyntaxKind.ProtectedKeyword),
                        SyntaxFactory.Token(SyntaxKind.OverrideKeyword)
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

                // ModelBuilder modelBuilder
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("modelBuilder"))
                    .WithType(
                        SyntaxFactory.IdentifierName("ModelBuilder")));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }
    }
}