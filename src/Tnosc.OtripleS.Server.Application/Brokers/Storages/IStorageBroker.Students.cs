// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<Student> InsertStudentAsync(Student student);
    ValueTask<IEnumerable<Student>> SelectAllStudentsAsync();
    ValueTask<Student> SelectStudentByIdAsync(Guid studentId);
    ValueTask<Student> UpdateStudentAsync(Student student);
    ValueTask<Student> DeleteStudentAsync(Student student);
}
