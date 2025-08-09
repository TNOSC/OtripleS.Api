// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Tnosc.OtripleS.Server.Domain.Students.Exceptions;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTest
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRegisterWhenStudentIsNullAndLogItAsync()
    {
        // given
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Student invalidStudent = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        var nullStudentException = new NullStudentException( message: "The student is null.");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: nullStudentException);

        // when
#pragma warning disable CS8604 // Possible null reference argument.
        ValueTask<Student> registerStudentTask =
                _studentService.RegisterStudentAsync(invalidStudent);
#pragma warning restore CS8604 // Possible null reference argument.

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
                   registerStudentTask.AsTask());

        _loggingBrokerMock.Received(1)
            .LogError(Arg.Is<Xeption>(actualException =>
                actualException.SameExceptionAs(expectedStudentValidationException)));

        _storageBrokerMock.ReceivedCalls().
            ShouldBeEmpty();
        _dateTimeBrokerMock.ReceivedCalls()
            .ShouldBeEmpty();
    }
}
