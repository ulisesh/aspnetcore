// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading;
using System.Collections.Immutable;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;

namespace Microsoft.AspNetCore.Analyzers.DelegateEndpoints;

public partial class DelegateEndpointFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create("ASP0006");

    public sealed override FixAllProvider GetFixAllProvider()
    {
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        foreach (var diagnostic in context.Diagnostics)
        {
            switch (diagnostic.Id)
            {
                case "ASP0006":
                    context.RegisterCodeFix(
                        CodeAction.Create("Fix mismatched parameter optionality", cancellationToken => FixMismatchedParameterOptionality(context, cancellationToken), equivalenceKey: DiagnosticDescriptors.DetectMismatchedParameterOptionality.Id),
                        diagnostic);
                    break;
                default:
                    break;
            }
        }

        return Task.CompletedTask;
    }
}
