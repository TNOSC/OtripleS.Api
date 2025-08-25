// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.Students;

public interface IStudentService
{
    ValueTask<Student> RegisterStudentAsync(Student student);
    IQueryable<Student> RetrieveAllStudents();
    ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId);
    ValueTask<Student> ModifyStudentAsync(Student student);
    ValueTask<Student> RemoveStudentByIdAsync(Guid studentId);
}
