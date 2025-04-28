using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaPDF.Models;

public class WebsocketRedisMessage
{
    public string Message { get; set; }
    public string OwnerHash { get; set; }


    public WebsocketRedisMessage(string message, string ownerHash)
    {
        Message = message;
        OwnerHash = ownerHash;
    }
    
    public WebsocketRedisMessage()
    {
        
    }

}
