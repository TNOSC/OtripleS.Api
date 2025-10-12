using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tnosc.Lib.Application.Observabilities;
using Tnosc.OtripleS.Server.Application.Observabilities;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Api.Endpoints.Students;

public partial class GetStudentByIdEndpoint
{
    private static async ValueTask<ActionResult<Student>> WithTracing(ReturningActionResult returningActionResult, TracingActivity tracing)
    {
        using Activity? activity = DiagnosticsConfig.ActivitySource.StartActivity(
            tracing.ActivityName,
            ActivityKind.Internal,
            Activity.Current?.Context ?? new ActivityContext(),
            tracing.Tags);
        try
        {
            ActionResult<Student> result = await returningActionResult();
            activity?.SetStatus(ActivityStatusCode.Ok);
            return result;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
    }

    private static TracingActivity AddTrace(Guid studentId) =>
       new()
       {
           ActivityName = "GetById",
           Tags =
           [
               new KeyValuePair<string, object?>(nameof(studentId),studentId),
           ]
       };
}
