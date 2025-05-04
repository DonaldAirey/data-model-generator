// <copyright file="DtoNamespace.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model
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
    public class DtoNamespace
    {
        /// <summary>
        /// The custom tool namespace.
        /// </summary>
        private readonly string customToolNamespace;

        /// <summary>
        /// The data model schema.
        /// </summary>
        private readonly XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="DtoNamespace"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The name of the namespace.</param>
        /// <param name="customToolNamespace">The namespace of the generated module.</param>
        public DtoNamespace(XmlSchemaDocument xmlSchemaDocument, string customToolNamespace)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.customToolNamespace = customToolNamespace;

            // This is the slave data model.
            this.xmlSchemaDocument.IsMaster = false;

            // This is the syntax of the namespace.
            this.Syntax = SyntaxFactory.NamespaceDeclaration(
                SyntaxFactory.IdentifierName(customToolNamespace + ".DataTransferObjects"))
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
        /// Gets the 'using' statements.
        /// </summary>
        private List<UsingDirectiveSyntax> UsingStatements
        {
            get
            {
                // Add the system namespace references.
                List<UsingDirectiveSyntax> systemUsingStatements = new List<UsingDirectiveSyntax>
                {
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Text.Json.Serialization")),
                };

                // Add the non-system namespace references.
                List<UsingDirectiveSyntax> usingStatements = new List<UsingDirectiveSyntax>
                {
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(this.customToolNamespace)),
                };

                // This sorts and combines the two lists. The 'System' namespace comes before the rest.
                return systemUsingStatements
                    .OrderBy(ud => ud.Name.ToString())
                    .Concat(usingStatements.OrderBy(ud => ud.Name.ToString()))
                    .ToList();
            }
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
                members = this.CreatePublicInterfaces(members);
                members = this.CreatePublicClasses(members);
                return members;
            }
        }

        /// <summary>
        /// Creates the classes.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the classes.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInterfaces(SyntaxList<MemberDeclarationSyntax> members)
        {
            List<SyntaxElement> syntaxElements = new List<SyntaxElement>();

            // Add the members.
            syntaxElements.Add(new IRowVersionedInterface.Interface());

            // Alphabetize the list of classes and add them to the structure.
            foreach (var syntaxElement in syntaxElements.OrderBy(se => se.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // This is the collection of alphabetized fields.
            return members;
        }

        /// <summary>
        /// Creates the classes.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the classes.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicClasses(SyntaxList<MemberDeclarationSyntax> members)
        {
            List<SyntaxElement> syntaxElements = new List<SyntaxElement>();

            // Create the row classes.
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                syntaxElements.Add(new DtoClass.Class(tableElement));
            }

            // Alphabetize the list of classes and add them to the structure.
            foreach (var syntaxElement in syntaxElements.OrderBy(se => se.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // This is the collection of alphabetized fields.
            return members;
        }
    }
}