// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.Students;

public partial class StudentService
{
    private static IEnumerable<Type> GetRetryableExceptions()
        => [typeof(FailedStudentStorageException)];

    private async ValueTask<Student> WithRetry(
           ReturningStudentFunction returningStudentFunction,
           IEnumerable<Type> retryExceptionTypes)
    {
        int attempts = 0;

        while (true)
        {
            try
            {
                attempts++;
                return await returningStudentFunction();
            }
            catch (Exception ex)
            {
                if (retryExceptionTypes.Any(exceptionType => exceptionType == ex.GetType()))
                {
                    _loggingBroker
                        .LogInformation(
                            $"Error found. Retry attempt {attempts}/{_retryConfig.MaxRetryAttempts}. " +
                                $"Exception: {ex.Message}");

                    if (attempts == _retryConfig.MaxRetryAttempts)
                    {
                        throw;
                    }

                    await Task.Delay(_retryConfig.DelayBetweenFailures);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
