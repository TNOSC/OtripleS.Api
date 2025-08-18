// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal partial class StorageBroker
{
    public DbSet<Student> Students { get; set; }

    public ValueTask<Student> InsertStudentAsync(Student student) =>
         TryCatch(async () => await InsertAsync(@object: student));

    public async ValueTask<IEnumerable<Student>> SelectAllStudentsAsync() => 
        await SelectAllAsync<Student>();

    public async ValueTask<Student> SelectStudentByIdAsync(Guid studentId) =>
        await SelectAsync<Student>(objectIds: studentId);

    public async ValueTask<Student> UpdateStudentAsync(Student student) =>
        await UpdateAsync(@object: student);

    public async ValueTask<Student> DeleteStudentAsync(Student student) =>
        await DeleteAsync(@object: student);
}
