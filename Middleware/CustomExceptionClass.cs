using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Middleware
{
    public class CustomExceptionClass :Exception
    {
        public CustomExceptionClass(string message) : base(message){
            
        }
    }
}