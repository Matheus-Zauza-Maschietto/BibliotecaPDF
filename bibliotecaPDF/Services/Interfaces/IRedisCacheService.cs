using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaPDF.Services.Interfaces;

public interface IRedisCacheService
{
    Task<bool> SetValueAsync(string key, object value);
    Task<string?> GetValueAsync(string key);
}
