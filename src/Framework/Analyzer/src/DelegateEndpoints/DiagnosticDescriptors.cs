// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Analyzers.DelegateEndpoints
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisReleaseTracking", "RS2008:Enable analyzer release tracking")]
    internal static class DiagnosticDescriptors
    {
        internal static readonly DiagnosticDescriptor DoNotUseModelBindingAttributesOnDelegateEndpointParameters = new(
            "ASP0003",
            "Do not use model binding attributes with Map handlers",
            "{0} should not be specified for a {1} Delegate parameter",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            helpLinkUri: "https://aka.ms/aspnet/analyzers");

        internal static readonly DiagnosticDescriptor DetectMismatchedParameterOptionality = new(
            "ASP0006",
            "Parameter optionality is mismatched",
            "'{0}' argument should be annotated as optional to match route parameter",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            helpLinkUri: "https://aka.ms/aspnet/analyzers");
    }
}
