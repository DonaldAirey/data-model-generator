// <copyright file="OnModelCreatingMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.DbContextClass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
            .WithModifiers(OnModelCreatingMethod.Modifiers)
            .WithParameterList(OnModelCreatingMethod.Parameters)
            .WithBody(this.Body)
            .WithLeadingTrivia(OnModelCreatingMethod.DocumentationComment);
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
        private static SyntaxTokenList Modifiers
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
        private static ParameterListSyntax Parameters
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
                    string abbreviation = tableElement.Name[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();

                    // Each column will have properties that need to be set by the Fluent API.
                    foreach (ColumnElement columnElement in tableElement.Columns)
                    {
                        // This indicates that a property requires a modification.  If we don't find any modifications after doing the
                        // checks below, we'll ignore this column and assume the Entity Framework defaults.
                        bool hasModification = false;

                        //            modelBuilder.Entity<Country>().Property(c => c.CountryId);
                        ExpressionSyntax expressionSyntax = SyntaxFactory.InvocationExpression(
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
                                                    SyntaxFactory.IdentifierName(columnElement.Name)))))));

                        // Autogenerated values.
                        if (columnElement.IsAutoIncrement)
                        {
                            //            .ValueGeneratedOnAdd();
                            expressionSyntax = SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        expressionSyntax,
                                        SyntaxFactory.IdentifierName("ValueGeneratedOnAdd")));
                            hasModification = true;
                        }

                        // RowVersion columns
                        if (columnElement.IsRowVersion)
                        {
                            //            .IsRowVersion();
                            expressionSyntax = SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        expressionSyntax,
                                        SyntaxFactory.IdentifierName("IsRowVersion")));
                            hasModification = true;
                        }

                        // Provide a reasonble range for decimal datatypes
                        if (columnElement.Type == typeof(decimal))
                        {
                            //            .HasColumnType("decimal(18,2)");
                            expressionSyntax = SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        expressionSyntax,
                                        SyntaxFactory.IdentifierName("HasColumnType")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("decimal(18,2)"))))));
                            hasModification = true;
                        }

                        // Nullable datatypes that can't be null
                        if (!columnElement.IsNullable)
                        {
                            //            .IsRequired();
                            expressionSyntax = SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        expressionSyntax,
                                        SyntaxFactory.IdentifierName("IsRequired")));
                            hasModification = true;
                        }

                        if (columnElement.Type == typeof(string) && columnElement.MaximumLength != int.MaxValue)
                        {
                            //            .HasMaxLength(26);
                            expressionSyntax = SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        expressionSyntax,
                                        SyntaxFactory.IdentifierName("HasMaxLength")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal(columnElement.MaximumLength))))));
                            hasModification = true;
                        }

                        // If we found some modification to the defaults, then emit the statement.
                        if (hasModification)
                        {
                            statements.Add(SyntaxFactory.ExpressionStatement(expressionSyntax));
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
                                                SyntaxFactory.IdentifierName("State")))))));

                    // Add an Ignore invocation for each of the owner (record set) navigation properties.
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
                                                SyntaxFactory.IdentifierName(foreignKeyElement.UniqueParentName)))))));
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
                                                SyntaxFactory.IdentifierName(foreignKeyElement.UniqueChildName)))))));
                    }

                    //            modelBuilder.Entity<Buyer>().Ignore(b => b.Buyers).Ignore(b => b.Country).Ignore(b => b.Province).Ignore(b => b.Subscriptions);
                    statements.Add(SyntaxFactory.ExpressionStatement(ignoredProperties));

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

                    // Create a foreign key index for every parent table.
                    foreach (ForeignKeyElement foreignKeyElement in tableElement.ParentKeys)
                    {
                        //            modelBuilder.Entity<Province>().HasOne<Country>().WithMany().HasForeignKey(p => p.CountryId);
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
                                                        SyntaxFactory.GenericName(
                                                            SyntaxFactory.Identifier("HasOne"))
                                                        .WithTypeArgumentList(
                                                            SyntaxFactory.TypeArgumentList(
                                                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                    SyntaxFactory.IdentifierName(foreignKeyElement.UniqueKey.Table.Name)))))),
                                                SyntaxFactory.IdentifierName("WithMany"))),
                                        SyntaxFactory.IdentifierName("HasForeignKey")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(ForeignKeyExpression.GetForeignKey(foreignKeyElement)))))));
                    }
                }

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }
    }
}