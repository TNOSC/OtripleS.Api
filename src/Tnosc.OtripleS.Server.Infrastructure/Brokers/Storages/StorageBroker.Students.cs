// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal partial class StorageBroker
{
    public DbSet<Student> Students { get; set; }

    public async ValueTask<Student> InsertStudentAsync(Student student) =>
         await TryCatch(async () => await InsertAsync(@object: student));

    public async ValueTask<IQueryable<Student>> SelectAllStudents() =>
         await TryCatch(async () => await SelectAll<Student>());

    public async ValueTask<Student> SelectStudentByIdAsync(Guid studentId) =>
        await TryCatch(async () => await SelectAsync<Student>(objectIds: studentId));

    public async ValueTask<Student> UpdateStudentAsync(Student student) =>
         await TryCatch(async () => await UpdateAsync(@object: student));

    public async ValueTask<Student> DeleteStudentAsync(Student student) =>
         await TryCatch(async () => await DeleteAsync(@object: student));
}
