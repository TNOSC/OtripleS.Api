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
        TestResult result = Types.InAssembly(assembly: _domainAssembly)
            .Should()
            .NotHaveDependencyOn(dependency: _applicationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void DomainLayerShouldNotHaveDependencyOnInfrastructureLayer()
    {
        TestResult result = Types.InAssembly(assembly: _domainAssembly)
            .Should()
            .NotHaveDependencyOn(dependency: _infrastructureAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void DomainLayerShouldNotHaveDependencyOnPresentationLayer()
    {
        TestResult result = Types.InAssembly(assembly: _domainAssembly)
            .Should()
            .NotHaveDependencyOn(dependency: _presentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void ApplicationLayerShouldNotHaveDependencyOnInfrastructureLayer()
    {
        TestResult result = Types.InAssembly(assembly: _applicationAssembly)
            .Should()
            .NotHaveDependencyOn(dependency: _infrastructureAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void ApplicationLayerShouldNotHaveDependencyOnPresentationLayer()
    {
        TestResult result = Types.InAssembly(assembly: _applicationAssembly)
            .Should()
            .NotHaveDependencyOn(dependency: _presentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void InfrastructureLayerShouldNotHaveDependencyOnPresentationLayer()
    {
        TestResult result = Types.InAssembly(assembly: _infrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(dependency: _presentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}
