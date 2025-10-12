using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tnosc.Lib.Application.Observabilities;
using Tnosc.OtripleS.Server.Application.Observabilities;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.Students;

public partial class StudentService
{
    private static async ValueTask<Student> WithTracing(ReturningStudentFunction returningStudentFunction, TracingActivity tracing)
    {
        using Activity? activity = DiagnosticsConfig.ActivitySource.StartActivity(
            tracing.ActivityName,
            ActivityKind.Internal,
            Activity.Current?.Context ?? new ActivityContext(),
            tracing.Tags);
        try
        {
            Student result = await returningStudentFunction();
            activity?.SetStatus(ActivityStatusCode.Ok);
            return result;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
    }

    private static async ValueTask<IQueryable<Student>> WithTracing(ReturningStudentsFunction returningStudentsFunction, TracingActivity tracing)
    {
        using Activity? activity = DiagnosticsConfig.ActivitySource.StartActivity(
            tracing.ActivityName,
            ActivityKind.Internal,
            Activity.Current?.Context ?? new ActivityContext(),
            tracing.Tags);
        try
        {
            IQueryable<Student> result = await returningStudentsFunction();
            activity?.SetStatus(ActivityStatusCode.Ok);
            return result;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
    }

    private static TracingActivity AddTraceOnRegister(Student student) =>
        new()
        {
            ActivityName = nameof(RegisterStudentAsync),
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

    private static TracingActivity AddTraceOnModify(Student student) =>
        new()
        {
            ActivityName = nameof(ModifyStudentAsync),
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
                new KeyValuePair<string, object?>(nameof(student.UpdatedBy), student?.UpdatedBy),
                new KeyValuePair<string, object?>(nameof(student.UpdatedDate), student?.UpdatedDate),
            ]
        };

    private static TracingActivity AddTraceOnRemove(Guid studentId) =>
       new()
       {
           ActivityName = nameof(RemoveStudentByIdAsync),
           Tags =
           [
               new KeyValuePair<string, object?>(nameof(studentId),studentId),
           ]
       };

    private static TracingActivity AddTraceOnGetById(Guid studentId) =>
     new()
     {
         ActivityName = nameof(RetrieveStudentByIdAsync),
         Tags =
         [
             new KeyValuePair<string, object?>(nameof(studentId),studentId),
         ]
     };

    private static TracingActivity AddTraceOnGetAll() =>
       new()
       {
           ActivityName = nameof(RetrieveAllStudentsAsync),
       };
}
