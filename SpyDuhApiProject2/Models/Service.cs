using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyDuhApiProject2.Models
{
    public class Service
    {
        public string Description { get; set; }
        public Guid SpyId { get; set; }
        public Guid ServiceId { get; set; }
    }
}
