using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DmlCafePos.Entities;

namespace DmlCafePos.Models
{
    public class ChangeOrderViewModel
    {
        public Table Table{ get; set; } = null!;
        public Order Order{ get; set; }= null!;
        public List<Table> Tables{ get; set; } = new List<Table>();
        public List<Area> Areas{ get; set; } = new List<Area>();

    }
}