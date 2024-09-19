using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DmlCafePos.Entities
{
    public class Area
    {
        public int AreaId { get; set; }
        public string? AreaName { get; set; }
        public List<Table> Tables {get; set;} = new List<Table>();
        public string MandantID {get;set;} = null!;
    }
}