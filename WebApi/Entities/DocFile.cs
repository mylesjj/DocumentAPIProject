using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class DocFile
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }
        public string Filesize { get; set; }

        public byte[] Content { get; set; }
    }
}
