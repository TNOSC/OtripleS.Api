using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tnosc.Lib.Application.Observabilities;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Api.Endpoints.Students;

public partial class GetStudentsEndpoint
{
    private static readonly ActivitySource source = new(nameof(GetStudentsEndpoint));

    private static async ValueTask<ActionResult<IQueryable<Student>>> WithTracing(ReturningActionResult returningActionResult, TracingActivity tracing)
    {
        using Activity? activity = source.StartActivity(
            tracing.ActivityName,
            ActivityKind.Internal,
            Activity.Current?.Context ?? new ActivityContext(),
            tracing.Tags);
        try
        {
            ActionResult<IQueryable<Student>> result = await returningActionResult();
            activity?.SetStatus(ActivityStatusCode.Ok);
            return result;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
    }

    private static TracingActivity AddTrace() =>
       new()
       {
           ActivityName = "Get"
       };
}
