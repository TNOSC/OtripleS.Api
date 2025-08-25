// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

namespace Tnosc.OtripleS.Server.Api.Routes;

internal static class StudentsRoutes
{
    internal const string Tag = "Students";

    internal const string BaseUri = "api/students";

    internal const string ResourceId = "studentId";

    internal const string Get = $"{BaseUri}";

    internal const string Post = $"{BaseUri}";

    internal const string Put = $"{BaseUri}";

    internal const string Delete = $"{BaseUri}/{{{ResourceId}:guid}}";

    internal const string GetById = $"{BaseUri}/{{{ResourceId}:guid}}";
}
