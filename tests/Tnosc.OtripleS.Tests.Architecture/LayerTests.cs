// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Reflection;

namespace Tnosc.OtripleS.Tests.Architecture;

public partial class LayerTests
{
    private readonly Assembly _domainAssembly = typeof(Domain.AssemblyReference).Assembly;
    private readonly Assembly _applicationAssembly = typeof(Application.AssemblyReference).Assembly;
    private readonly Assembly _infrastructureAssembly = typeof(Infrastructure.AssemblyReference).Assembly;
    private readonly Assembly _presentationAssembly = typeof(Api.AssemblyReference).Assembly;
}
