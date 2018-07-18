// <copyright file="Class.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.DataModelClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// The data model class.
    /// </summary>
    public class Class : SyntaxElement
    {
        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private DataModelSchema dataModelSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class.
        /// </summary>
        /// <param name="dataModelSchema">A description of a unique constraint.</param>
        public Class(DataModelSchema dataModelSchema)
        {
            // Initialize the object.
            this.dataModelSchema = dataModelSchema;
            this.Name = dataModelSchema.Name;

            //    /// <summary>
            //    /// A thread-safe data model.
            //    /// </summary>
            //    public class DataModel : IDisposable
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.ClassDeclaration(this.Name)
                .WithModifiers(this.Modifiers)
                .WithBaseList(this.BaseList)
                .WithMembers(this.Members)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the base class syntax.
        /// </summary>
        private BaseListSyntax BaseList
        {
            get
            {
                // This is the list of base classes.
                return SyntaxFactory.BaseList(
                        SyntaxFactory.SeparatedList<BaseTypeSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                SyntaxFactory.SimpleBaseType(
                                    SyntaxFactory.IdentifierName("IDisposable"))
                            }));
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                //    /// <summary>
                //    /// A thread-safe DataSet able to handle transactions.
                //    /// </summary>
                return SyntaxFactory.TriviaList(
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
                                                " A thread-safe DataSet able to handle transactions.",
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
            }
        }

        /// <summary>
        /// Gets the members syntax.
        /// </summary>
        private SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // Create the members.
                SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>);
                members = this.CreatePrivateConstantFields(members);
                members = this.CreatePrivateInstanceFields(members);
                members = this.CreateConstructors(members);
                members = this.CreatePublicInstanceProperties(members);
                members = this.CreatePublicInstanceMethods(members);
                members = this.CreateProtectedInstanceMethods(members);
                members = this.CreatePrivateInstanceMethods(members);
                return members;
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private SyntaxTokenList Modifiers
        {
            get
            {
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                    });
            }
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateConstructors(SyntaxList<MemberDeclarationSyntax> members)
        {
            // These are the constructors.
            members = members.Add(new ConstructorEtc(this.dataModelSchema).Syntax);

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePrivateConstantFields(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the internal instance properties.
            List<SyntaxElement> fields = new List<SyntaxElement>();
            fields.Add(new BatchSizeField());
            fields.Add(new CourtesyIntervalField());
            fields.Add(new RefreshIntervalField());

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePrivateInstanceFields(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the internal instance properties.
            List<SyntaxElement> fields = new List<SyntaxElement>();
            fields.Add(new BindingField());
            fields.Add(new CommunicationExceptionHandlerField());
            fields.Add(new DataServiceClientField());
            fields.Add(new DataSetIdField());
            fields.Add(new EndpointAddressField());
            fields.Add(new IsReadingField());
            fields.Add(new SecurityTokenField());
            fields.Add(new SequenceField());
            fields.Add(new SynchronizationContextField());
            fields.Add(new TransactionHandlersField(this.dataModelSchema));
            fields.Add(new TransactionLogField());
            fields.Add(new TransactionLogIndexField());

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceProperties(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the internal instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>();
            properties.Add(new IsReadingProperty());

            // Create a field for each of the tables.
            foreach (TableSchema tableSchema in this.dataModelSchema.Tables)
            {
                // This insures that we only have one relation to the parent table when there are multiple relations.
                foreach (RelationSchema relationSchema in tableSchema.ParentRelations)
                {
                    // Create a field to hold each of the parent relations.
                    properties.Add(new ParentRelationProperty(relationSchema));
                }

                // Add a field for each of the unique constraints that are not the primary key.
                foreach (ConstraintSchema constraintSchema in tableSchema.Constraints)
                {
                    UniqueConstraintSchema uniqueConstraintSchema = constraintSchema as UniqueConstraintSchema;
                    if (uniqueConstraintSchema != null)
                    {
                        properties.Add(new UniqueIndexProperty(uniqueConstraintSchema));
                    }
                }

                // And the tables.
                properties.Add(new TableProperty(tableSchema));
            }

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in properties.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the public instance methods.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the methods added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new DisposeMethod());

            // Alphabetize and add the methods as members of the class.
            foreach (SyntaxElement syntaxElement in methods.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the public instance methods.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the methods added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateProtectedInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new DisposeBooleanMethod());

            // Alphabetize and add the methods as members of the class.
            foreach (SyntaxElement syntaxElement in methods.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the private instance methods.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the methods added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePrivateInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new MergeTransactionsMethod(this.dataModelSchema));
            methods.Add(new OnChannelFaultedMethod(this.dataModelSchema));
            methods.Add(new ReadTransactionsAsyncMethod(this.dataModelSchema));
            methods.Add(new ClearMethod(this.dataModelSchema));

            // Alphabetize and add the methods as members of the class.
            foreach (SyntaxElement syntaxElement in methods.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }
    }
}