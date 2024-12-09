using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Models
{
    public class AccountEditModelClass
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public IFormFile image { get; set; }

    }
}