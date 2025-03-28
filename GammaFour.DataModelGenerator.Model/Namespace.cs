// <copyright file="Namespace.cs" company="Gamma Four, Inc.">
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
    public class Namespace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Namespace"/> class.
        /// </summary>
        /// <param name="customToolNamespace">The namespace of the generated module.</param>
        public Namespace(string customToolNamespace)
        {
            // This is the syntax of the namespace.
            this.Syntax = SyntaxFactory.NamespaceDeclaration(
                    SyntaxFactory.IdentifierName(customToolNamespace))
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
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Collections.Generic")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Transactions")),
                };

                // Add the non-system namespace references.
                List<UsingDirectiveSyntax> usingStatements = new List<UsingDirectiveSyntax>
                {
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("DotNext.Threading")),
                };

                // This sorts and combines the two lists. The 'System' namespace comes before the rest.
                return systemUsingStatements
                    .OrderBy(ud => ud.Name.ToString())
                    .Concat(usingStatements.OrderBy(ud => ud.Name.ToString())).ToList();
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
                members = this.CreatePublicEnums(members);
                members = this.CreatePublicClasses(members);
                return members;
            }
        }

        /// <summary>
        /// Creates the classes.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the classes.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicClasses(SyntaxList<MemberDeclarationSyntax> members)
        {
            // The common classes.
            List<SyntaxElement> syntaxElements = new List<SyntaxElement>
            {
                new AsyncTransactionClass.Class(),
                new ConcurrencyExceptionClass.Class(),
                new ConstraintExceptionClass.Class(),
                new RowChangedEventArgsClass.Class(),
            };

            // Alphabetize the list of classes and add them to the structure.
            foreach (var syntaxElement in syntaxElements.OrderBy(se => se.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // This is the collection of alphabetized fields.
            return members;
        }

        /// <summary>
        /// Creates the enums.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the enums.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicEnums(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Create the common enums.
            List<SyntaxElement> syntaxElements = new List<SyntaxElement>
            {
                new DataActionEnum.Enum(),
            };

            // Alphabetize the list of enums.
            foreach (var syntaxElement in syntaxElements.OrderBy(se => se.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // This is the collection of alphabetized fields.
            return members;
        }
    }
}