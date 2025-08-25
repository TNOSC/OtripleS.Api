// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;

namespace Tnosc.Lib.Api;

public static class EndpointBaseAsync
{
    public static class WithRequest<TRequest>
    {
        public abstract class WithActionResult<TResponse> : EndpointBase
        {
            public abstract ActionResult<TResponse> HandleAsync(TRequest request);
        }

        public abstract class WithActionResultValueTask<TResponse> : EndpointBase
        {
            public abstract ValueTask<ActionResult<TResponse>> HandleAsync(TRequest request);
        }

        public abstract class WithActionResult : EndpointBase
        {
            public abstract ActionResult HandleAsync(TRequest request);
        }

        public abstract class WithActionResultValueTask : EndpointBase
        {
            public abstract ValueTask<ActionResult> HandleAsync(TRequest request);
        }
    }

    public static class WithoutRequest
    {
        public abstract class WithActionResult<TResponse> : EndpointBase
        {
            public abstract ActionResult<TResponse> HandleAsync();
        }
        public abstract class WithActionResultValueTask<TResponse> : EndpointBase
        {
            public abstract ValueTask<ActionResult<TResponse>> HandleAsync();
        }

        public abstract class WithActionResult : EndpointBase
        {
            public abstract ActionResult HandleAsync();
        }

        public abstract class WithActionResultValueTask : EndpointBase
        {
            public abstract ValueTask<ActionResult> HandleAsync();
        }
    }
}
