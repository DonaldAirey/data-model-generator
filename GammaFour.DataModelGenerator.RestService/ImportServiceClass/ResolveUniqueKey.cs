// <copyright file="ResolveUniqueKey.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService.RestServiceClass
{
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Create a series of statements that resolves an external reference.
    /// </summary>
    internal class ResolveUniqueKey : List<StatementSyntax>
    {
        /// <summary>
        /// Indication that the key can be resolved implicitly with variables rather than with an explicit key.
        /// </summary>
        private bool isNotDelete;

        /// <summary>
        /// The source key variable name.
        /// </summary>
        private string targetKeyVariable;

        /// <summary>
        /// The table schema.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// The unique constraint parameter item.
        /// </summary>
        private UniqueKeyElement uniqueKeyElement;

        /// <summary>
        /// The table schema.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveUniqueKey"/> class.
        /// </summary>
        /// <param name="tableElement">The table schema.</param>
        /// <param name="isNotDelete">Indicates an explicit key must be provided in the calling parameters.</param>
        /// <returns>A block of code that can be used to resolve a primary key.</returns>
        public ResolveUniqueKey(
            TableElement tableElement,
            bool isNotDelete)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.xmlSchemaDocument = this.tableElement.XmlSchemaDocument;
            this.isNotDelete = isNotDelete;
            this.uniqueKeyElement = this.tableElement.PrimaryKey;
            this.targetKeyVariable = this.uniqueKeyElement.Table.Name.ToCamelCase() + "TargetKey";

            // These are all the variables we are going to resolve with this method.
            foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
            {
                //            string currentCountryId = default(string);
                ColumnElement columnElement = columnReferenceElement.Column;
                string variableName = isNotDelete ? columnElement.Name.ToCamelCase() + "Key" : columnElement.Name.ToCamelCase();
                this.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(Conversions.FromType(columnElement.Type))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(variableName))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.DefaultExpression(Conversions.FromType(columnElement.Type))
                                        .WithKeyword(SyntaxFactory.Token(SyntaxKind.DefaultKeyword))))))));
            }

            // There are two different configurations that a primary index can have: a single constraint with no resolution of the key needed and
            // multiple unique indices where we need to resolve which index is to be used to resolve the target record.
            this.AddRange(this.tableElement.UniqueKeys.Count() == 1 ? this.ResolveSingleUniqueIndex : this.ResolveMultipleUniqueIndex);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> FinallyBlock1
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                this.dataModel.Configuration.ReleaseReaderLock();
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
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> FinallyBlock2
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                        memberRow.ReleaseReaderLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> FinallyBlock4
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                        countryRow.ReleaseReaderLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> GetMissingValues
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            try
                //            {
                //                <TryBlock4>
                //            }
                //            finally
                //            {
                //                <FinallyBlock4>
                //            }
                statements.Add(
                    SyntaxFactory.TryStatement()
                    .WithBlock(
                        SyntaxFactory.Block(this.TryBlock4))
                    .WithFinally(
                        SyntaxFactory.FinallyClause(
                            SyntaxFactory.Block(this.FinallyBlock4))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> GetRowDataBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            try
                //            {
                //                <TryBlock2>
                //            }
                //            finally
                //            {
                //                <FinallyBlock2>
                //            }
                statements.Add(
                    SyntaxFactory.TryStatement()
                    .WithBlock(
                        SyntaxFactory.Block(this.TryBlock2))
                    .WithFinally(
                        SyntaxFactory.FinallyClause(
                            SyntaxFactory.Block(this.FinallyBlock2))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> ResolveMultipleUniqueIndex
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // This will find the name of the name of the key that is associated with the given configuration.  This name is then used to
                // determine which of the parameters is used to create a key used to find the record.  For example, the combination of the
                // configuration id and a source (e.g.  'Customer') would find the CustomerExternalId0 index, which is then used to find the customer
                // record.
                statements.AddRange(new TargetKey(this.uniqueKeyElement.Table.Name, this.targetKeyVariable, this.uniqueKeyElement.XmlSchemaDocument));

                //                CountryRow countryRow = null;
                string foreignRowType = this.uniqueKeyElement.Table.Name + "Row";
                string foreignRow = this.uniqueKeyElement.Table.Name.ToCamelCase() + "Row";
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(foreignRowType))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(foreignRow))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                                        .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword))))))));

                //                switch (countryTargetKey)
                //                {
                //                    <SwitchSections>
                //                }
                statements.Add(
                    SyntaxFactory.SwitchStatement(
                        SyntaxFactory.IdentifierName(this.targetKeyVariable))
                        .WithSections(SyntaxFactory.List<SwitchSectionSyntax>(this.SwitchSections)));

                //                if (countryRow != null)
                //                {
                //                    <GetMissingValues>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                            .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword))),
                        SyntaxFactory.Block(this.GetMissingValues)));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> ResolveSingleUniqueIndex
        {
            get
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
                        SyntaxFactory.Block(this.TryBlock1))
                    .WithFinally(
                        SyntaxFactory.FinallyClause(
                            SyntaxFactory.Block(this.FinallyBlock1))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> TryBlock1
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.dataModel.ConfigurationKey.AcquireReaderLock();
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
                                    SyntaxFactory.IdentifierName(this.uniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("AcquireReaderLock")))));

                // This creates the comma-separated list of parameters that are used to create a key using either the explicit key or, if no explicit
                // key is provided, using the parameters to create a key.
                List<ArgumentSyntax> explicitArguments = new List<ArgumentSyntax>();
                List<ArgumentSyntax> implicitArguments = new List<ArgumentSyntax>();
                List<ColumnReferenceElement> uniqueColumns = this.uniqueKeyElement.Columns;
                for (int index = 0; index < uniqueColumns.Count; index++)
                {
                    // We're going to use this column to create an element of the key.
                    ColumnElement columnElement = uniqueColumns[index].Column;
                    string parsedVariableName = columnElement.Type == typeof(string) || columnElement.IsInParentKey ?
                        columnElement.Name.ToCamelCase() :
                        columnElement.Name.ToCamelCase() + "Parsed";

                    // Each of the columns belonging to the key are added to the explicit parameter list.
                    explicitArguments.Add(
                        SyntaxFactory.Argument(
                            Conversions.CreateParseExpression(
                                SyntaxFactory.ElementAccessExpression(
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Key"))
                                    .WithArgumentList(
                                        SyntaxFactory.BracketedArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.NumericLiteralExpression,
                                                        SyntaxFactory.Literal(index)))))),
                            columnElement.Type)));

                    // Each of the columns belonging to a default key are added to the implicit parameter list.
                    implicitArguments.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(parsedVariableName)));
                }

                // Create and update methods can use the incoming arguments to create a dynamic key based on the selected index that is used to find
                // the record.  The delete method doesn't have those parameters so it needs to insist on an array of string arguments that can be
                // parsed to construct a key into the selected index.
                if (this.isNotDelete)
                {
                    //                    ConfigurationRow configurationRow = configurationKey == null ? this.dataModel.ConfigurationKey.Find(configurationId, source) : this.dataModel.ConfigurationKey.Find(configurationKey[0], configurationKey[1]);
                    statements.Add(
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(this.tableElement.Name + "Row"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier(this.tableElement.Name.ToCamelCase() + "Row"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.ConditionalExpression(
                                                SyntaxFactory.BinaryExpression(
                                                    SyntaxKind.EqualsExpression,
                                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Key"),
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.NullLiteralExpression)),
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.ThisExpression(),
                                                                SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name.ToCamelCase())),
                                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Name)),
                                                        SyntaxFactory.IdentifierName("Find")))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SeparatedList<ArgumentSyntax>(implicitArguments))),
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.ThisExpression(),
                                                                SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name.ToCamelCase())),
                                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Name)),
                                                        SyntaxFactory.IdentifierName("Find")))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SeparatedList<ArgumentSyntax>(explicitArguments))))))))));
                }
                else
                {
                    //                    ConfigurationRow configurationRow = this.dataModel.ConfigurationKey.Find(configurationKey[0], configurationKey[1]);
                    statements.Add(
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(this.tableElement.Name + "Row"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier(this.tableElement.Name.ToCamelCase() + "Row"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name.ToCamelCase())),
                                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Name)),
                                                    SyntaxFactory.IdentifierName("Find")))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(explicitArguments)))))))));
                }

                //                if (configurationRow != null)
                //                {
                //                    <GetRowDataBlock>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                            SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                            .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword))),
                        SyntaxFactory.Block(this.GetRowDataBlock)));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> TryBlock2
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
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                SyntaxFactory.IdentifierName("AcquireReaderLock")))));

                // The general idea here is to create a primary key from the values in the parent table.  The parent table may have been resolved
                // using external identifiers but we need the primary key members in order to add or update the target row.
                foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
                {
                    //                        currentMemberId = memberRow.MemberId;
                    ColumnElement columnElement = columnReferenceElement.Column;
                    string variableName = this.isNotDelete ? columnElement.Name.ToCamelCase() + "Key" : columnElement.Name.ToCamelCase();
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.IdentifierName(variableName),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                    SyntaxFactory.IdentifierName(columnElement.Name)))));
                }

                //                        rowVersion = memberRow.RowVersion;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("rowVersion"),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                SyntaxFactory.IdentifierName("RowVersion")))));

                //                        isFound = true;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("isFound"),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.TrueLiteralExpression))));

                // This is the complete block.
                return statements;
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
                foreach (UniqueKeyElement alternativeUniqueKeyElement in this.tableElement.UniqueKeys)
                {
                    //                    case "CountryExternalId0Key":
                    //                        try
                    //                        {
                    //                            <TryBlock3>
                    //                        }
                    //                        finally
                    //                        {
                    //                            <FinallyBlock3>
                    //                        }
                    switchSections.Add(
                        SyntaxFactory.SwitchSection()
                            .WithLabels(
                                SyntaxFactory.SingletonList<SwitchLabelSyntax>(
                                    SyntaxFactory.CaseSwitchLabel(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(alternativeUniqueKeyElement.Name)))))
                            .WithStatements(
                                SyntaxFactory.List<StatementSyntax>(
                                    new StatementSyntax[]
                                    {
                                        SyntaxFactory.TryStatement()
                                        .WithBlock(SyntaxFactory.Block(this.TryBlock3(alternativeUniqueKeyElement)))
                                        .WithFinally(
                                            SyntaxFactory.FinallyClause(
                                                SyntaxFactory.Block(this.FinallyBlock3(alternativeUniqueKeyElement)))),
                                        SyntaxFactory.BreakStatement()
                                    })));
                }

                // This contains all the case statements used to select a record based on the index selected by the configuration.
                return switchSections;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> TryBlock4
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                        countryRow.AcquireReaderLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                SyntaxFactory.IdentifierName("AcquireReaderLock")))));

                // Even though we found the parent table (most likely using an external identifier), we still need the values that will be used to
                // build a primary key.
                foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
                {
                    //                        countryIdKey = countryRow.CountryId;
                    ColumnElement columnElement = columnReferenceElement.Column;
                    string variableName = this.isNotDelete ? columnElement.Name.ToCamelCase() + "Key" : columnElement.Name.ToCamelCase();
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.IdentifierName(variableName),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                    SyntaxFactory.IdentifierName(columnElement.Name)))));
                }

                //                        rowVersion = countryRow.RowVersion;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("rowVersion"),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                                SyntaxFactory.IdentifierName("RowVersion")))));

                //                        isFound = true;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("isFound"),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.TrueLiteralExpression))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="alternativeUniqueKeyElement">The unique constraint schema to unlock.</param>
        /// <returns>A block of code to unlock a unique constraint.</returns>
        private List<StatementSyntax> FinallyBlock3(UniqueKeyElement alternativeUniqueKeyElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                this.dataModel.ConfigurationKey.ReleaseReaderLock();
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
                                SyntaxFactory.IdentifierName(alternativeUniqueKeyElement.Name)),
                            SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

            // This is the complete block.
            return statements;
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="alternativeUniqueKeyElement">The alternative unique constraint schema.</param>
        /// <returns>A block of code to lock a unique index and read from it.</returns>
        private List<StatementSyntax> TryBlock3(UniqueKeyElement alternativeUniqueKeyElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                    this.dataModel.ConfigurationKey.AcquireReaderLock();
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
                                SyntaxFactory.IdentifierName(alternativeUniqueKeyElement.Name)),
                            SyntaxFactory.IdentifierName("AcquireReaderLock")))));

            // This creates the comma-separated list of parameters that are used to create a key using either the explicit key or, if no explicit key
            // is provided, using the parameters to create a key.
            List<ArgumentSyntax> explicitArguments = new List<ArgumentSyntax>();
            List<ArgumentSyntax> implicitArguments = new List<ArgumentSyntax>();
            List<ColumnReferenceElement> alternativeColumns = alternativeUniqueKeyElement.Columns;
            for (int index = 0; index < alternativeColumns.Count; index++)
            {
                // We're going to use this column to create an element of the key.
                ColumnElement columnElement = alternativeColumns[index].Column;
                string parsedVariableName = columnElement.Type == typeof(string) ?
                    columnElement.Name.ToCamelCase() :
                    columnElement.Name.ToCamelCase() + "Parsed";

                // Each of the columns belonging to the key are added to the explicit parameter list.
                explicitArguments.Add(
                    SyntaxFactory.Argument(
                        Conversions.CreateParseExpression(
                            SyntaxFactory.ElementAccessExpression(
                                SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Key"))
                            .WithArgumentList(
                                SyntaxFactory.BracketedArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                SyntaxFactory.Literal(index)))))),
                        columnElement.Type)));

                // Each of the columns belonging to a default key are added to the implicit parameter list.
                implicitArguments.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(parsedVariableName)));
            }

            // When creating or updating, the arguments can be used to construct a key when an explicit key isn't provided.  The delete method has no
            // such arguments so it can only use an explicit key.
            if (this.isNotDelete)
            {
                //                            countryRow = keyCountry == null ? this.dataModel.CountryExternalId0KeyIndex.Find(externalId0) : this.dataModel.CountryExternalId0KeyIndex.Find(keyCountry[0]);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                            SyntaxFactory.ConditionalExpression(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.EqualsExpression,
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Key"),
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)),
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name.ToCamelCase())),
                                            SyntaxFactory.IdentifierName(alternativeUniqueKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("Find")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SeparatedList<ArgumentSyntax>(implicitArguments))),
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name.ToCamelCase())),
                                            SyntaxFactory.IdentifierName(alternativeUniqueKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("Find")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SeparatedList<ArgumentSyntax>(explicitArguments)))))));
            }
            else
            {
                //                        countryRow = this.dataModel.CountryKey.Find(Guid.Parse(countryKey[0]));
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase() + "Row"),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name.ToCamelCase())),
                                        SyntaxFactory.IdentifierName(alternativeUniqueKeyElement.Name)),
                                    SyntaxFactory.IdentifierName("Find")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(explicitArguments))))));
            }

            // This is the complete block.
            return statements;
        }
    }
}