using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaPDF.Services.Interfaces;

public interface IRedisObserverService
{
    Task PublishAsync(string channel, object message);
}
