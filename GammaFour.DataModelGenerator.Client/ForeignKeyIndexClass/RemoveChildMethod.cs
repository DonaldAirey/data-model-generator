// <copyright file="RemoveChildMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.ForeignKeyIndexClass
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
    public class RemoveChildMethod : SyntaxElement
    {
        /// <summary>
        /// The name of the row.
        /// </summary>
        private string rowParameter;

        /// <summary>
        /// The type of the row.
        /// </summary>
        private string rowType;

        /// <summary>
        /// The table schema.
        /// </summary>
        private RelationSchema relationSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveChildMethod"/> class.
        /// </summary>
        /// <param name="relationSchema">The unique constraint schema.</param>
        public RemoveChildMethod(RelationSchema relationSchema)
        {
            // Initialize the object.
            this.relationSchema = relationSchema;
            this.Name = "RemoveChild";
            this.rowType = this.relationSchema.ChildTable.Name + "Row";
            this.rowParameter = this.relationSchema.ChildTable.CamelCaseName + "Row";

            //        /// <summary>
            //        /// Removes a <see cref="CustomerRow"/> child relation.
            //        /// </summary>
            //        /// <param name="countryKey">A key that uniquely identifies the parent row.</param>
            //        /// <param name="customerRow">The child row.</param>
            //        internal void Add(CountryKey countryKey, CustomerRow customerRow)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithModifiers(this.Modifiers)
                .WithParameterList(this.ParameterList)
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
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            HashSet<CustomerRow> hashSet;
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("HashSet"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                        SyntaxFactory.IdentifierName(this.rowType)))
                                .WithLessThanToken(SyntaxFactory.Token(SyntaxKind.LessThanToken))
                                .WithGreaterThanToken(SyntaxFactory.Token(SyntaxKind.GreaterThanToken))))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("hashSet"))))));

                // These are the arguments for the key lookup.
                List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
                foreach (ColumnSchema columnSchema in this.relationSchema.ParentColumns)
                {
                    arguments.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(columnSchema.CamelCaseName)));
                }

                // Compound keys are handled differently than simple keys.
                if (this.relationSchema.ParentColumns.Count == 1)
                {
                    //            if (this.dictionary.TryGetValue(keyCountryId, out hashSet))
                    //            {
                    //                <RemoveChildRow>
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
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
                                        }))),
                            SyntaxFactory.Block(this.RemoveChild)));
                }
                else
                {
                    //            CustomerLastNameDateOfBirthKeySet customerLastNameDateOfBirthKeySet = new CustomerLastNameDateOfBirthKeySet(dateOfBirth, lastName);
                    statements.Add(
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.Name + "Set"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier(this.relationSchema.ParentKeyConstraint.CamelCaseName + "Set"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.ObjectCreationExpression(
                                                SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.Name + "Set"))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(arguments)))))))));

                    //            if (this.dictionary.TryGetValue(countryKey, out hashSet))
                    //            {
                    //                <RemoveChildRow>
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
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
                                                SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.CamelCaseName + "Set")),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName("hashSet"))
                                            .WithRefOrOutKeyword(
                                                SyntaxFactory.Token(SyntaxKind.OutKeyword))
                                        }))),
                            SyntaxFactory.Block(this.RemoveChild)));
                }

                // This collects the generic type arguments of the primary key of the parent table.
                List<TypeSyntax> genericTypes = new List<TypeSyntax>();
                genericTypes.Add(
                    this.relationSchema.ParentKeyConstraint.Columns.Count == 1 ?
                    Conversions.FromType(this.relationSchema.ParentKeyConstraint.Columns[0].Type) :
                    SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.Name + "Set"));

                // This list constructs the actual arguments that go into a NotifyRelationChanged call.
                List<ArgumentSyntax> constructorArguments = new List<ArgumentSyntax>();
                constructorArguments.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("NotifyRelationChangedAction"),
                            SyntaxFactory.IdentifierName("Remove"))));
                if (this.relationSchema.ParentKeyConstraint.Columns.Count == 1)
                {
                    constructorArguments.Add(
                        SyntaxFactory.Argument(
                             SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.Columns[0].CamelCaseName)));
                }
                else
                {
                    constructorArguments.Add(
                            SyntaxFactory.Argument(
                                 SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.CamelCaseName + "Set")));
                }

                //            this.RelationChanged?.Invoke(this, new NotifyRelationChangedEventArgs<Guid>(NotifyRelationChangedAction.Remove, customerId));
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.ConditionalAccessExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("RelationChanged")),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberBindingExpression(
                                    SyntaxFactory.IdentifierName("Invoke")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.ThisExpression()),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.ObjectCreationExpression(
                                                    SyntaxFactory.GenericName(
                                                        SyntaxFactory.Identifier("NotifyRelationChangedEventArgs"))
                                                    .WithTypeArgumentList(
                                                        SyntaxFactory.TypeArgumentList(
                                                            SyntaxFactory.SeparatedList(genericTypes))))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SeparatedList<ArgumentSyntax>(constructorArguments))))
                                        }))))));

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
                //        /// Removes a <see cref="CustomerRow"/> child relation.
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
                                                " Removes a <see cref=\"" + this.rowType + "\"/> child relation.",
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
                foreach (ColumnSchema columnSchema in this.relationSchema.ParentKeyConstraint.Columns)
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

                //        /// <param name="customerRow">The child row.</param>
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
                                                    " <param name=\"" + this.rowParameter + "\">The child row.</param>",
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
                        SyntaxFactory.Token(SyntaxKind.InternalKeyword)
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
                // string keyConfigurationId, string keySource
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();
                foreach (ColumnSchema columnSchema in this.relationSchema.ParentKeyConstraint.Columns)
                {
                    // Add the next element of the primary key.
                    parameters.Add(
                            SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(columnSchema.CamelCaseName))
                        .WithType(Conversions.FromType(columnSchema.Type)));
                }

                // CustomerRow customerRow
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier(this.rowParameter))
                        .WithType(
                            SyntaxFactory.IdentifierName(this.rowType)));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> RemoveChild
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                hashSet.Remove(customerRow);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("hashSet"),
                                SyntaxFactory.IdentifierName("Remove")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.rowParameter)))))));

                //                if (hashSet.Count == 0)
                //                {
                //                    <RemoveKey>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("hashSet"),
                                SyntaxFactory.IdentifierName("Count")),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(0))),
                        SyntaxFactory.Block(this.RemoveKey)));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> RemoveKey
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // Compound keys hare handled differently that simple keys.
                if (this.relationSchema.ParentColumns.Count == 1)
                {
                    //                    this.dictionary.Remove(countryKey);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("dictionary")),
                                    SyntaxFactory.IdentifierName("Remove")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(
                                                this.relationSchema.ParentKeyConstraint.Columns[0].CamelCaseName)))))));
                }
                else
                {
                    //                    this.dictionary.Remove(countryKey);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("dictionary")),
                                    SyntaxFactory.IdentifierName("Remove")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(this.relationSchema.ParentKeyConstraint.CamelCaseName + "Set")))))));
                }

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }
    }
}