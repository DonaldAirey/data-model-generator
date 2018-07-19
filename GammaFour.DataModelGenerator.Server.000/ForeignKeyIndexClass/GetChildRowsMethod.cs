// <copyright file="GetChildRowsMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.ForeignKeyIndexClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to start editing.
    /// </summary>
    public class GetChildRowsMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private RelationSchema relationSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChildRowsMethod"/> class.
        /// </summary>
        /// <param name="relationSchema">The unique constraint schema.</param>
        public GetChildRowsMethod(RelationSchema relationSchema)
        {
            // Initialize the object.
            this.relationSchema = relationSchema;
            this.Name = "Get" + relationSchema.ChildTable.Name + "Rows";

            //        /// <summary>
            //        /// Gets a collection of child rows.
            //        /// </summary>
            //        /// <param name="countryKey">The key used to reference the child rows.</param>
            //        /// <returns>A collection of child rows.</returns>
            //        internal List<ProvinceRow> GetProvinceRows(CountryKey countryKey)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("List"))
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                SyntaxFactory.IdentifierName(this.relationSchema.ChildTable.Name + "Row")))),
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

                //        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Performance")]
                attributes.Add(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("SuppressMessage"))
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
                                                    SyntaxFactory.Literal("CA1002:DoNotExposeGenericLists"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("Performance")))
                                            .WithNameEquals(
                                                SyntaxFactory.NameEquals(
                                                    SyntaxFactory.IdentifierName("Justification")))
                                        }))))));

                //        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ConfigurationKey")]
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
                                                    SyntaxFactory.Literal("Microsoft.Naming"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("CA2204:Literals should be spelled correctly"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal(this.relationSchema.Name)))
                                            .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("MessageId"))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal("Diagnostic message.")))
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

                //                if (!this.IsLockHeld)
                //                {
                //                      <ThrowLockException>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.PrefixUnaryExpression(
                            SyntaxKind.LogicalNotExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("IsLockHeld"))),
                        SyntaxFactory.Block(
                            ThrowLockException.GetSyntax(this.relationSchema.Name + " index is not locked."))));

                //            HashSet<ProvinceRow> hashSet;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("HashSet"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                        SyntaxFactory.IdentifierName(this.relationSchema.ChildTable.Name + "Row")))))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("hashSet"))))));

                // These are the arguments for the key lookup.
                List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                foreach (ColumnSchema columnSchema in this.relationSchema.ParentColumns)
                {
                    arguments.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(columnSchema.CamelCaseName)));
                }

                // Compound keys are handled differently than simple keys.
                if (this.relationSchema.ParentColumns.Count == 1)
                {
                    //            if (!this.dictionary.TryGetValue(countryId, out hashSet))
                    //            {
                    //                <NewHashSetBlock>
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.PrefixUnaryExpression(
                                SyntaxKind.LogicalNotExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("dictionary")),
                                        SyntaxFactory.IdentifierName("TryGetValue")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                            new SyntaxNodeOrToken[]
                                            {
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.IdentifierName(
                                                        this.relationSchema.ParentKeyConstraint.Columns[0].CamelCaseName)),
                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.IdentifierName("hashSet"))
                                                .WithRefOrOutKeyword(
                                                    SyntaxFactory.Token(SyntaxKind.OutKeyword))
                                            })))),
                            SyntaxFactory.Block(this.NewHashSetBlock)));
                }
                else
                {
                    //            if (this.dictionary.TryGetValue(new CustomerLastNameDateOfBirthKeySet(dateOfBirth, lastName), out hashSet))
                    //            {
                    //                <NewHashSetBlock>
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.PrefixUnaryExpression(
                                SyntaxKind.LogicalNotExpression,
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName("dictionary")),
                                            SyntaxFactory.IdentifierName("TryGetValue")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                new SyntaxNodeOrToken[]
                                                {
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.ObjectCreationExpression(
                                                            SyntaxFactory.IdentifierName("CustomerLastNameDateOfBirthKeySet"))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("hashSet"))
                                                    .WithRefOrOutKeyword(
                                                        SyntaxFactory.Token(SyntaxKind.OutKeyword))
                                                })))),
                                    SyntaxFactory.Block(this.NewHashSetBlock)));
                }

                //            return hashSet.ToArray();
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("hashSet"),
                                SyntaxFactory.IdentifierName("ToList")))));

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
                //        /// Gets a collection of child <see cref="ProvinceRow"/> rows.
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
                                                " Gets a collection of child <see cref=\"" + this.relationSchema.ChildTable.Name + "Row" + "\"/> rows.",
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

                // Add a comment for each of the key parameters.
                foreach (ColumnSchema columnSchema in this.relationSchema.ParentColumns)
                {
                    //        /// <param name="configurationId">The ConfigurationId key element.</param>
                    string description = "The " + columnSchema.Name + " key element.";
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
                                                        " <param name=\"" + columnSchema.CamelCaseName + "\">" + description + "</param>",
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        Environment.NewLine,
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList())
                                                }))))));
                }

                //        /// <returns>A collection of child <see cref="ProvinceRow"/> rows.</returns>
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
                                                    " <returns>A collection of child <see cref=\"" + this.relationSchema.ChildTable.Name + "Row" + "\"/> rows.</returns>",
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

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> NewHashSetBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                hashSet = new HashSet<ProvinceRow>();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("hashSet"),
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("HashSet"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                            SyntaxFactory.IdentifierName(this.relationSchema.ChildTable.Name + "Row")))))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList()))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax ParameterList
        {
            get
            {
                // string keyConfigurationId, string keySource
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();
                foreach (ColumnSchema columnSchema in this.relationSchema.ParentColumns)
                {
                    // Add the next element of the key.
                    parameters.Add(
                            SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(columnSchema.CamelCaseName))
                        .WithType(Conversions.FromType(columnSchema.Type)));
                }

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }
    }
}