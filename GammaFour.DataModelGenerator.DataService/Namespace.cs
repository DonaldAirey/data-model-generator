// <copyright file="Namespace.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.DataService
{
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// The root namespace.
    /// </summary>
    public class Namespace
    {
        /// <summary>
        /// The data model schema.
        /// </summary>
        private DataModelSchema dataModelSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="Namespace"/> class.
        /// </summary>
        /// <param name="dataModelSchema">The name of the namespace.</param>
        public Namespace(DataModelSchema dataModelSchema)
        {
            // Initialize the object.
            this.dataModelSchema = dataModelSchema;

            // This is the syntax of the namespace.
            this.Syntax = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(dataModelSchema.TargetNamespace))
                .WithUsings(SyntaxFactory.List<UsingDirectiveSyntax>(this.UsingStatements))
                .WithMembers(this.Members);
        }

        /// <summary>
        /// Gets or sets gets the syntax.
        /// </summary>
        public NamespaceDeclarationSyntax Syntax
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        private SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // Create the members.
                SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>);
                members = this.CreateInterfaces(members);
                members = this.CreatePublicClasses(members);
                return members;
            }
        }

        /// <summary>
        /// Gets the 'using' statements.
        /// </summary>
        private List<UsingDirectiveSyntax> UsingStatements
        {
            get
            {
                // Create the 'using' statements.
                // [TODO] Make the addition of user namespaces part of the initialization.  Run through the tables and extract the
                // namespaces from the types found therein.
                List<UsingDirectiveSyntax> usingStatements = new List<UsingDirectiveSyntax>();
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Security.Permissions")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.ServiceModel")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("GammaFour.ClientModel")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("GammaFour.ServiceModel")));
                return usingStatements;
            }
        }

        /// <summary>
        /// Creates the classes.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the classes.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicClasses(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This is the actual service component - the part that speaks to the outside world.
            members = members.Add(new DataServiceClass.Class(this.dataModelSchema).Syntax);

            // This is the collection of alphabetized fields.
            return members;
        }

        /// <summary>
        /// Creates the classes.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the classes.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateInterfaces(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This is the actual service component - the part that speaks to the outside world.
            members = members.Add(new DataServiceInterface.Interface(this.dataModelSchema).Syntax);

            // This is the collection of alphabetized fields.
            return members;
        }
    }
}