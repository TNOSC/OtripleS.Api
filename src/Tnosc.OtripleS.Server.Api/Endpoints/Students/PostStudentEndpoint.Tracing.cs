using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tnosc.Lib.Application.Observabilities;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Api.Endpoints.Students;

public partial class PostStudentEndpoint
{
    private static readonly ActivitySource source = new(nameof(PostStudentEndpoint));

    private static async ValueTask<ActionResult<Student>> WithTracing(ReturningActionResult returningActionResult, TracingActivity tracing)
    {
        using Activity? activity = source.StartActivity(
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

    private static TracingActivity AddTrace(Student student) =>
       new()
       {
           ActivityName = "Add",
           Tags =
           [
                new KeyValuePair<string, object?>(nameof(student.Id), student?.Id),
                new KeyValuePair<string, object?>(nameof(student.IdentityNumber), student?.IdentityNumber),
                new KeyValuePair<string, object?>(nameof(student.FirstName), student?.FirstName),
                new KeyValuePair<string, object?>(nameof(student.MiddleName), student?.MiddleName),
                new KeyValuePair<string, object?>(nameof(student.LastName), student?.LastName),
                new KeyValuePair<string, object?>(nameof(student.Gender), student?.Gender),
                new KeyValuePair<string, object?>(nameof(student.BirthDate), student?.BirthDate),
                new KeyValuePair<string, object?>(nameof(student.CreatedBy), student?.CreatedBy),
                new KeyValuePair<string, object?>(nameof(student.CreatedDate), student?.CreatedDate),
           ]
       };
}
