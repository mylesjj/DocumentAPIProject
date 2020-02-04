using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class DocFileDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }
        public string Filesize { get; set; }

        public byte[] Content { get; set; }
    }

    
}
