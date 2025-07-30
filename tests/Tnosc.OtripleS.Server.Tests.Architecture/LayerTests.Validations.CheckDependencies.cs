// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using NetArchTest.Rules;
using Shouldly;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Architecture;
public partial class LayerTests
{
    [Fact]
    public void DomainShouldNotHaveDependencyOnApplication()
    {
        TestResult result = Types.InAssembly(_domainAssembly)
            .Should()
            .NotHaveDependencyOn(_applicationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void DomainLayerShouldNotHaveDependencyOnInfrastructureLayer()
    {
        TestResult result = Types.InAssembly(_domainAssembly)
            .Should()
            .NotHaveDependencyOn(_infrastructureAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void DomainLayerShouldNotHaveDependencyOnPresentationLayer()
    {
        TestResult result = Types.InAssembly(_domainAssembly)
            .Should()
            .NotHaveDependencyOn(_presentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void ApplicationLayerShouldNotHaveDependencyOnInfrastructureLayer()
    {
        TestResult result = Types.InAssembly(_applicationAssembly)
            .Should()
            .NotHaveDependencyOn(_infrastructureAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void ApplicationLayerShouldNotHaveDependencyOnPresentationLayer()
    {
        TestResult result = Types.InAssembly(_applicationAssembly)
            .Should()
            .NotHaveDependencyOn(_presentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void InfrastructureLayerShouldNotHaveDependencyOnPresentationLayer()
    {
        TestResult result = Types.InAssembly(_infrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(_presentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}
