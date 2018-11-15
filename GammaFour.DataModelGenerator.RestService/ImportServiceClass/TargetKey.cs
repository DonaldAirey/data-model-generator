// <copyright file="TargetKey.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService.RestServiceClass
{
    using System.Collections.Generic;
    using GammaFour.ClientModel;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Create a block of statements that get the name of an index from the configuration table.
    /// </summary>
    internal class TargetKey : List<StatementSyntax>
    {
        /// <summary>
        /// The literal name for the source (used with the configuration id to reference the target key).
        /// </summary>
        private string source;

        /// <summary>
        /// The index source key variable name.
        /// </summary>
        private string targetKey;

        /// <summary>
        /// The XML Schema document.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetKey"/> class.
        /// </summary>
        /// <param name="source">The value for the source (with the configuration id, makes up a key that references an index).</param>
        /// <param name="targetKey">The target key variable.</param>
        /// <param name="xmlSchemaDocument">The XML Schema document.</param>
        public TargetKey(string source, string targetKey, XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.source = source;
            this.targetKey = targetKey;
            this.xmlSchemaDocument = xmlSchemaDocument;

            //            string licenseTypeTargetKey = default(string);
            this.Add(
                SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.StringKeyword)))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(this.targetKey))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.DefaultExpression(
                                        SyntaxFactory.PredefinedType(
                                            SyntaxFactory.Token(SyntaxKind.StringKeyword)))))))));

            //            try
            //            {
            //                <TryBlock1>
            //            }
            //            finally
            //            {
            //                <FinallyBlock1>
            //            }
            this.Add(
                SyntaxFactory.TryStatement()
                .WithBlock(
                    SyntaxFactory.Block(this.TryBlock1))
                .WithFinally(
                    SyntaxFactory.FinallyClause(
                        SyntaxFactory.Block(this.FinallyBlock1))));
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> FinallyBlock1
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.dataModel.Configuration.ReleaseReaderLock();
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
                                    SyntaxFactory.IdentifierName("ConfigurationKey")),
                                SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
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

                //                    configurationRow.ReleaseReaderLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("configurationRow"),
                                SyntaxFactory.IdentifierName("ReleaseReaderLock")))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> TryBlock1
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.dataModel.Configuration.AcquireReaderLock();
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
                                    SyntaxFactory.IdentifierName("ConfigurationKey")),
                                SyntaxFactory.IdentifierName("AcquireReaderLock")))));

                //                    ConfigurationRow configurationRow = this.dataModel.Configuration.Find(configurationId, "Country");
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("ConfigurationRow"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("configurationRow"))
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
                                                    SyntaxFactory.IdentifierName("ConfigurationKey")),
                                                SyntaxFactory.IdentifierName("Find")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName("configurationId")),
                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.StringLiteralExpression,
                                                                SyntaxFactory.Literal(this.source)))
                                                    })))))))));

                //                if (configurationRow == null)
                //                {
                //                    <ThrowRecordNotFoundExceptionBlock>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.IdentifierName("configurationRow"),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(this.ThrowRecordNotFoundExceptionBlock)));

                //                try
                //                {
                //                    <TryBlock2>
                //                }
                //                finally
                //                {
                //                    <FinallyBlock2>
                //                }
                statements.Add(
                    SyntaxFactory.TryStatement()
                    .WithBlock(
                        SyntaxFactory.Block(this.TryBlock2))
                    .WithFinally(
                        SyntaxFactory.FinallyClause(
                            SyntaxFactory.Block(this.FinallyBlock2))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
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

                //                        configurationRow.AcquireReaderLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("configurationRow"),
                                SyntaxFactory.IdentifierName("AcquireReaderLock")))));

                //                        countryTargetKey = configurationRow.TargetKey;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(this.targetKey),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("configurationRow"),
                                SyntaxFactory.IdentifierName("TargetKey")))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<StatementSyntax> ThrowRecordNotFoundExceptionBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                        throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("ConfigurationKey", new object[] { configurationId, "Country" }));
                statements.Add(
                    SyntaxFactory.ThrowStatement(
                        SyntaxFactory.ObjectCreationExpression(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("FaultException"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                        SyntaxFactory.IdentifierName(nameof(RecordNotFoundFault))))))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName(nameof(RecordNotFoundFault)))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.StringLiteralExpression,
                                                                SyntaxFactory.Literal("ConfigurationKey"))),
                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.ArrayCreationExpression(
                                                                SyntaxFactory.ArrayType(
                                                                    SyntaxFactory.PredefinedType(
                                                                        SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                                                .WithRankSpecifiers(
                                                                    SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                                                        SyntaxFactory.ArrayRankSpecifier(
                                                                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                                                SyntaxFactory.OmittedArraySizeExpression())))))
                                                            .WithInitializer(
                                                                SyntaxFactory.InitializerExpression(
                                                                    SyntaxKind.ArrayInitializerExpression,
                                                                    SyntaxFactory.SeparatedList<ExpressionSyntax>(
                                                                        new SyntaxNodeOrToken[]
                                                                        {
                                                                            SyntaxFactory.IdentifierName("configurationId"),
                                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                            SyntaxFactory.LiteralExpression(
                                                                                SyntaxKind.StringLiteralExpression,
                                                                                SyntaxFactory.Literal(this.source))
                                                                        }))))
                                                    })))))))));

                // This is the complete block.
                return SyntaxFactory.List(statements);
            }
        }
    }
}