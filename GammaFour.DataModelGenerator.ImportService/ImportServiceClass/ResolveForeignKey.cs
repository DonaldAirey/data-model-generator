// <copyright file="ResolveForeignKey.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.ImportService.ImportServiceClass
{
    using System.Collections.Generic;
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
        private RelationSchema relationSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveForeignKey"/> class.
        /// </summary>
        /// <param name="relationSchema">The relation schema.</param>
        /// <returns>A block of code that resolves a foreign key.</returns>
        public ResolveForeignKey(RelationSchema relationSchema)
        {
            // Initialize the object.
            this.relationSchema = relationSchema;

            // This will find the name of the name of the index that is associated with the given configuration.  This name tells us how to interpret
            // the generic key that's passed in from the outside.
            this.AddRange(new TargetKey(this.relationSchema.Name, this.relationSchema.CamelCaseName + "TargetKey"));

            // We are going to populate each of the columns that belong to this table with the corresponding elements from the parent table.
            foreach (ColumnSchema columnSchema in this.relationSchema.ChildColumns)
            {
                //            Guid currentCountryId;
                this.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            Conversions.FromType(columnSchema.Type))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(columnSchema.CamelCaseName))))));
            }

            //                    CountryRow countryRow = null;
            this.Add(
                SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(this.relationSchema.ParentTable.Name + "Row"))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(this.relationSchema.CamelCaseName + "Row"))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)))))));

            // If the value for a nullable key is null, then we don't want to attempt to resolve it.  Otherwise, use the values in the key to find
            // the parent record and populate all the columns related to that key.
            if (this.relationSchema.ChildKeyConstraint.IsNullable)
            {
                this.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(this.relationSchema.UniqueParentName),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(
                            SyntaxFactory.SwitchStatement(
                                SyntaxFactory.IdentifierName(this.relationSchema.CamelCaseName + "TargetKey"))
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
                        SyntaxFactory.IdentifierName(this.relationSchema.CamelCaseName + "TargetKey"))
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
                                SyntaxFactory.IdentifierName(this.relationSchema.CamelCaseName + "Row"),
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
                foreach (UniqueConstraintSchema uniqueConstraintSchema in this.relationSchema.ParentTable.UniqueKeys)
                {
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
                                            SyntaxFactory.Literal(uniqueConstraintSchema.Name)))))
                            .WithStatements(
                                SyntaxFactory.List<StatementSyntax>(
                                    this.FindRecordBlock(uniqueConstraintSchema))));
                }

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
                                SyntaxFactory.IdentifierName(this.relationSchema.CamelCaseName + "Row"),
                                SyntaxFactory.IdentifierName("AcquireReaderLock")))));

                // At this point we've figured out what key we're using to reference the parent table and locked it.  Now it's time to extract from
                // the parent table each column that is used in the target table for this operation.
                for (int index = 0; index < this.relationSchema.ParentColumns.Count; index++)
                {
                    //                    countryId = countryRow.CountryId;
                    ColumnSchema parentColumn = this.relationSchema.ParentColumns[index];
                    ColumnSchema childColumn = this.relationSchema.ChildColumns[index];
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.IdentifierName(childColumn.CamelCaseName),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.relationSchema.CamelCaseName + "Row"),
                                    SyntaxFactory.IdentifierName(parentColumn.Name)))));
                }

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueConstraintSchema">The unique constraint schema.</param>
        /// <returns>A block of code to extract the values from the parent table.</returns>
        private SyntaxList<StatementSyntax> FinallyBlock1(UniqueConstraintSchema uniqueConstraintSchema)
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
                                    SyntaxFactory.IdentifierName("dataModel")),
                                SyntaxFactory.IdentifierName(uniqueConstraintSchema.Name)),
                            SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code to find a record using a unique index.
        /// </summary>
        /// <param name="uniqueConstraintSchema">The unique constraint schema.</param>
        /// <returns>A block of code that finds a record using a unique index.</returns>
        private List<StatementSyntax> FindRecordBlock(UniqueConstraintSchema uniqueConstraintSchema)
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
                    SyntaxFactory.Block(this.TryBlock1(uniqueConstraintSchema)))
                .WithFinally(
                    SyntaxFactory.FinallyClause(
                        SyntaxFactory.Block(this.FinallyBlock1(uniqueConstraintSchema)))));

            //                        break;
            statements.Add(SyntaxFactory.BreakStatement());

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueConstraintSchema">The unique constraint schema.</param>
        /// <returns>A block of code to extract the values from the parent table.</returns>
        private SyntaxList<StatementSyntax> TryBlock1(UniqueConstraintSchema uniqueConstraintSchema)
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
                                    SyntaxFactory.IdentifierName("dataModel")),
                                SyntaxFactory.IdentifierName(uniqueConstraintSchema.Name)),
                            SyntaxFactory.IdentifierName("AcquireReaderLock")))));

            // This creates the list of arguments that are used to find a record using a unique index.  The values coming into the import library are
            // all strings so the values are parsed when another datatype is needed to create a key.
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>();
            for (int index = 0; index < uniqueConstraintSchema.Columns.Count; index++)
            {
                // All variables into the import come in as strings.  Some of the columns need to be parsed into native datatypes before they can be
                // used as part of a key.
                ColumnSchema columnSchema = uniqueConstraintSchema.Columns[index];
                arguments.Add(
                    SyntaxFactory.Argument(
                        Conversions.CreateParseExpression(
                            SyntaxFactory.ElementAccessExpression(
                                SyntaxFactory.IdentifierName(this.relationSchema.UniqueParentName))
                                .WithArgumentList(
                                    SyntaxFactory.BracketedArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(index)))))),
                            columnSchema.Type)));
            }

            //                        countryRow = this.dataModel.CountryKey.Find(Guid.Parse(countryKey[0]));
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(this.relationSchema.CamelCaseName + "Row"),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("dataModel")),
                                    SyntaxFactory.IdentifierName(uniqueConstraintSchema.Name)),
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
                        SyntaxFactory.IdentifierName(this.relationSchema.CamelCaseName + "Row"),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression)),
                    SyntaxFactory.Block(ThrowRecordNotFoundFault.GetSyntax(uniqueConstraintSchema, arguments))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}