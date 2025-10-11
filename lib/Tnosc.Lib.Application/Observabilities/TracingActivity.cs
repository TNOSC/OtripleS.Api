// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

namespace Tnosc.Lib.Application.Observabilities;

public class TracingActivity
{
    public required string ActivityName { get; set; }
    public IEnumerable<KeyValuePair<string, object?>>? Tags { get; set; }
}
