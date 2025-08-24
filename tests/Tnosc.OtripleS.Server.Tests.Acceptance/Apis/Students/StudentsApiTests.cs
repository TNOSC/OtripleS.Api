using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Domain.Students;
using Tnosc.OtripleS.Server.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Acceptance.Apis.Students;

[Collection(nameof(ApiTestCollection))]
public partial class StudentsApiTests
{
    private readonly OtripleSApiBroker _otripleSApiBroker;

    public StudentsApiTests(OtripleSApiBroker otripleSApiBroker) =>
        _otripleSApiBroker = otripleSApiBroker;

    private async ValueTask<Student> PostRandomStudentAsync()
    {
        Student randomStudent = CreateRandomStudent();
        await _otripleSApiBroker.PostStudentAsync(randomStudent);

        return randomStudent;
    }

    private static Student UpdateStudentRandom(Student student)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        student.UpdatedDate = now;

        return student;
    }

    private static Student CreateRandomStudent() =>
           CreateRandomStudentFiller().Create();

    private static Filler<Student> CreateRandomStudentFiller()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var userId = Guid.NewGuid();
        var filler = new Filler<Student>();

        filler.Setup()
            .OnProperty(student => student.CreatedBy).Use(userId)
            .OnProperty(student => student.UpdatedBy).Use(userId)
            .OnProperty(student => student.CreatedDate).Use(now)
            .OnProperty(student => student.UpdatedDate).Use(now)
            .OnType<DateTimeOffset>().Use(GetRandomDateTime());

        return filler;
    }

    private static DateTimeOffset GetRandomDateTime() =>
           new DateTimeRange(earliestDate: DateTime.UtcNow).GetValue();
}




