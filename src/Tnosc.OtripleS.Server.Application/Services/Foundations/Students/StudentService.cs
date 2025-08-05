// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.Students;

internal sealed class StudentService : IStudentService
{
    public ValueTask<Student> ModifyStudentAsync(Student student) => 
        throw new NotImplementedException();
    
    public ValueTask<Student> RegisterStudentAsync(Student student) => 
        throw new NotImplementedException();
    
    public ValueTask<Student> RemoveStudentByIdAsync(Guid studentId) => 
        throw new NotImplementedException();
    
    public ValueTask<IEnumerable<Student>> RetrieveAllStudentsAsync() => 
        throw new NotImplementedException();
    
    public ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) => 
        throw new NotImplementedException();
}
