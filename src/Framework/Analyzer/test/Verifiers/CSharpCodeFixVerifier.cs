// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.AspNetCore.Analyzer.Testing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;
using Microsoft.AspNetCore.Analyzers.DelegateEndpoints;

namespace Microsoft.AspNetCore.Analyzers.Testing.Utilities;

public static class CSharpAnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DelegateEndpointAnalyzer, new()
{
    public static DiagnosticResult Diagnostic(string diagnosticId = null)
        => CSharpAnalyzerVerifier<DelegateEndpointAnalyzer>.Diagnostic(diagnosticId);

    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
        => new DiagnosticResult(descriptor);

    public static Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
    {
        var test = new Test { TestCode = source };
        test.ExpectedDiagnostics.AddRange(expected);
        return test.RunAsync();
    }

    // Code fix tests support both analyzer and code fix testing. This test class is derived from the code fix test
    // to avoid the need to maintain duplicate copies of the customization work.
    public class Test : CSharpCodeFixTest<TAnalyzer, EmptyCodeFixProvider, XUnitVerifier>
    {
    }
}
public static class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
    where TAnalyzer : DelegateEndpointAnalyzer, new()
    where TCodeFix : DelegateEndpointFixer, new()
{
    public static DiagnosticResult Diagnostic(string diagnosticId = null)
        => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, XUnitVerifier>.Diagnostic(diagnosticId);

    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
        => new DiagnosticResult(descriptor);

    public static Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
    {
        var test = new CSharpAnalyzerVerifier<TAnalyzer>.Test { TestCode = source };
        test.ExpectedDiagnostics.AddRange(expected);
        return test.RunAsync();
    }

    public static Task VerifyCodeFixAsync(string source, string fixedSource)
        => VerifyCodeFixAsync(source, DiagnosticResult.EmptyDiagnosticResults, fixedSource);

    public static Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource)
        => VerifyCodeFixAsync(source, new[] { expected }, fixedSource);

    public static Task VerifyCodeFixAsync(string source, DiagnosticResult[] expected, string fixedSource)
        => VerifyCodeFixAsync(source, expected, fixedSource);

    public static Task VerifyCodeFixAsync(string sources, DiagnosticResult[] expected, string fixedSources, string usageSource = "")
    {
        var test = new DelegateEndpointAnalyzerTest
        {
            TestState = {
                Sources = { sources, usageSource },
            },
            FixedState = {
                Sources =  { fixedSources, usageSource }
            }
        };
        test.TestState.ExpectedDiagnostics.AddRange(expected);
        return test.RunAsync();
    }

    public class DelegateEndpointAnalyzerTest : CSharpCodeFixTest<TAnalyzer, TCodeFix, XUnitVerifier>
    {
        public DelegateEndpointAnalyzerTest()
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net60.AddAssemblies(ImmutableArray.Create(
                typeof(Microsoft.AspNetCore.Builder.WebApplication).Assembly.Location.Replace(".dll", string.Empty),
                typeof(Microsoft.AspNetCore.Builder.DelegateEndpointRouteBuilderExtensions).Assembly.Location.Replace(".dll", string.Empty),
                typeof(Microsoft.AspNetCore.Builder.IApplicationBuilder).Assembly.Location.Replace(".dll", string.Empty),
                typeof(Microsoft.AspNetCore.Builder.IEndpointConventionBuilder).Assembly.Location.Replace(".dll", string.Empty),
                typeof(Microsoft.Extensions.Hosting.IHost).Assembly.Location.Replace(".dll", string.Empty),
                typeof(Microsoft.AspNetCore.Mvc.ModelBinding.IBinderTypeProviderMetadata).Assembly.Location.Replace(".dll", string.Empty),
                typeof(Microsoft.AspNetCore.Mvc.BindAttribute).Assembly.Location.Replace(".dll", string.Empty)));
        }
    }
}

