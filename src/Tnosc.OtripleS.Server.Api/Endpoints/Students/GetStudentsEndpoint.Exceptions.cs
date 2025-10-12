// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tnosc.Lib.Application.Observabilities;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Api.Endpoints.Students;

public partial class GetStudentsEndpoint
{
    private delegate ValueTask<ActionResult<IQueryable<Student>>> ReturningActionResult();

    private async ValueTask<ActionResult<IQueryable<Student>>> TryCatch(
        ReturningActionResult returningActionResult,
        TracingActivity? withTracing = null)
    {
        try
        {
            ReturningActionResult composedActionResult = returningActionResult;
            if (withTracing is not null)
            {
                composedActionResult = () => WithTracing(returningActionResult, withTracing);
            }
            return await composedActionResult();
        }
        catch (StudentDependencyException studentDependencyException)
        {
            return InternalServerError(exception: studentDependencyException);
        }
        catch (StudentServiceException studentServiceException)
        {
            return InternalServerError(exception: studentServiceException);
        }
    }
}
