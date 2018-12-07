// <copyright file="ResolveForeignKey.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService.RestServiceClass
{
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Create a series of statements that resolves an external reference.
    /// </summary>
    internal class ResolveForeignKey : List<StatementSyntax>
    {
        /// <summary>
        /// The relation schema.
        /// </summary>
        private ForeignKeyElement foreignKeyElement;

        /// <summary>
        /// The table schema.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveForeignKey"/> class.
        /// </summary>
        /// <param name="foreignKeyElement">The relation schema.</param>
        /// <returns>A block of code that resolves a foreign key.</returns>
        public ResolveForeignKey(ForeignKeyElement foreignKeyElement)
        {
            // Initialize the object.
            this.foreignKeyElement = foreignKeyElement;
            this.xmlSchemaDocument = this.foreignKeyElement.XmlSchemaDocument;

            // This will find the name of the name of the index that is associated with the given configuration.  This name tells us how to interpret
            // the generic key that's passed in from the outside.
            this.AddRange(new TargetKey(this.foreignKeyElement.Name, this.foreignKeyElement.Name.ToCamelCase() + "TargetKey", this.foreignKeyElement.XmlSchemaDocument));

            // We are going to populate each of the columns that belong to this table with the corresponding elements from the parent table.
            foreach (ColumnReferenceElement columnReferenceElement in this.foreignKeyElement.Columns)
            {
                //            Guid currentCountryId;
                ColumnElement columnElement = columnReferenceElement.Column;
                this.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            Conversions.FromType(columnElement.Type))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(columnElement.Name.ToCamelCase()))))));
            }

            //                    CountryRow countryRow = null;
            this.Add(
                SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(this.foreignKeyElement.UniqueKey.Name))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(this.foreignKeyElement.Name.ToCamelCase()))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)))))));

            // If the value for a nullable key is null, then we don't want to attempt to resolve it.  Otherwise, use the values in the key to find
            // the parent record and populate all the columns related to that key.
            if (this.foreignKeyElement.IsNullable)
            {
                this.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(this.foreignKeyElement.UniqueParentName),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(
                            SyntaxFactory.SwitchStatement(
                                SyntaxFactory.IdentifierName(this.foreignKeyElement.Name.ToCamelCase() + "TargetKey"))
                                .WithSections(SyntaxFactory.List<SwitchSectionSyntax>(this.SwitchSections)))));
            }
            else
            {
                //                switch (countryCustomerCountryIdKeyTargetKey)
                //                {
                //                    <SwitchSections>
                //                }
                this.Add(
                    SyntaxFactory.SwitchStatement(
                        SyntaxFactory.IdentifierName(this.foreignKeyElement.Name.ToCamelCase() + "TargetKey"))
                        .WithSections(SyntaxFactory.List<SwitchSectionSyntax>(this.SwitchSections)));
            }

            //                try
            //                {
            //                    <TryBlock2>
            //                }
            //                finally
            //                {
            //                    <FinallyBlock2>
            //                }
            this.Add(
                SyntaxFactory.TryStatement()
                .WithBlock(
                    SyntaxFactory.Block(this.TryBlock2))
                .WithFinally(
                    SyntaxFactory.FinallyClause(
                        SyntaxFactory.Block(this.FinallyBlock2))));
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> FinallyBlock2
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    countryRow.ReleaseReaderLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.foreignKeyElement.Name.ToCamelCase()),
                                SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }

        /// <summary>
        /// Gets the switch sections.
        /// </summary>
        private List<SwitchSectionSyntax> SwitchSections
        {
            get
            {
                // This list will collect all the sections.
                List<SwitchSectionSyntax> switchSections = new List<SwitchSectionSyntax>();

                // Every unique key will create a possible 'case' statement for looking up a value using an alternative index.
                UniqueKeyElement uniqueKeyElement = this.foreignKeyElement.UniqueKey;

                //                    case "CountryExternalId0Key":
                //                        countryRow = this.dataModel.Country.Find(new CountryExternalId0Key(countryKey[0]));
                //                        break;
                switchSections.Add(
                    SyntaxFactory.SwitchSection()
                        .WithLabels(
                            SyntaxFactory.SingletonList<SwitchLabelSyntax>(
                                SyntaxFactory.CaseSwitchLabel(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        SyntaxFactory.Literal(uniqueKeyElement.Name)))))
                        .WithStatements(
                            SyntaxFactory.List<StatementSyntax>(
                                this.FindRecordBlock(uniqueKeyElement))));

                // This contains all the case statements used to select a record based on the index selected by the configuration.
                return switchSections;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> TryBlock2
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    countryRow.AcquireReaderLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.foreignKeyElement.Name.ToCamelCase()),
                                SyntaxFactory.IdentifierName("AcquireReaderLock")))));

                // At this point we've figured out what key we're using to reference the parent table and locked it.  Now it's time to extract from
                // the parent table each column that is used in the target table for this operation.
                List<ColumnReferenceElement> parentColumns = this.foreignKeyElement.ParentColumns;
                List<ColumnReferenceElement> childColumns = this.foreignKeyElement.Columns;
                for (int index = 0; index < parentColumns.Count; index++)
                {
                    //                    countryId = countryRow.CountryId;
                    ColumnElement parentColumn = parentColumns[index].Column;
                    ColumnElement childColumn = childColumns[index].Column;
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.IdentifierName(childColumn.Name.ToCamelCase()),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.foreignKeyElement.Name.ToCamelCase()),
                                    SyntaxFactory.IdentifierName(parentColumn.Name)))));
                }

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code to extract the values from the parent table.</returns>
        private SyntaxList<StatementSyntax> FinallyBlock1(UniqueKeyElement uniqueKeyElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                            this.dataModel.CountryExternalId0KeyIndex.ReleaseReaderLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name.ToCamelCase())),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                            SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code to find a record using a unique index.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code that finds a record using a unique index.</returns>
        private List<StatementSyntax> FindRecordBlock(UniqueKeyElement uniqueKeyElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //            try
            //            {
            //                <TryBlock1>
            //            }
            //            finally
            //            {
            //                <FinallyBlock1>
            //            }
            statements.Add(
                SyntaxFactory.TryStatement()
                .WithBlock(
                    SyntaxFactory.Block(this.TryBlock1(uniqueKeyElement)))
                .WithFinally(
                    SyntaxFactory.FinallyClause(
                        SyntaxFactory.Block(this.FinallyBlock1(uniqueKeyElement)))));

            //                        break;
            statements.Add(SyntaxFactory.BreakStatement());

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema.</param>
        /// <returns>A block of code to extract the values from the parent table.</returns>
        private SyntaxList<StatementSyntax> TryBlock1(UniqueKeyElement uniqueKeyElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                            this.dataModel.CountryExternalId0KeyIndex.AcquireReaderLock();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name.ToCamelCase())),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                            SyntaxFactory.IdentifierName("AcquireReaderLock")))));

            // This creates the list of arguments that are used to find a record using a unique index.  The values coming into the import library are
            // all strings so the values are parsed when another datatype is needed to create a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            List<ColumnReferenceElement> uniqueKeyColumns = uniqueKeyElement.Columns;
            for (int index = 0; index < uniqueKeyColumns.Count; index++)
            {
                // All variables into the import come in as strings.  Some of the columns need to be parsed into native datatypes before they can be
                // used as part of a key.
                ColumnElement columnElement = uniqueKeyColumns[index].Column;
                arguments.Add(
                    SyntaxFactory.Argument(
                        Conversions.CreateParseExpression(
                            SyntaxFactory.ElementAccessExpression(
                                SyntaxFactory.IdentifierName(this.foreignKeyElement.UniqueParentName))
                                .WithArgumentList(
                                    SyntaxFactory.BracketedArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(index)))))),
                            columnElement.Type)));
            }

            //                        countryRow = this.dataModel.CountryKey.Find(Guid.Parse(countryKey[0]));
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(this.foreignKeyElement.Name.ToCamelCase()),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name.ToCamelCase())),
                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Find")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList(arguments))))));

            //                    if (countryRow == null)
            //                    {
            //                        <ThrowRecordNotFoundFault>
            //                    }
            statements.Add(
                SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.EqualsExpression,
                        SyntaxFactory.IdentifierName(this.foreignKeyElement.Name.ToCamelCase()),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression)),
                    SyntaxFactory.Block(ThrowRecordNotFoundFault.GetSyntax(uniqueKeyElement, arguments))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}