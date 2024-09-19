using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Entities;
using DmlCafePos.Entities;

namespace DmlCafePos.Models
{
    public class AreaViewModel
    {
        public AppUser User {get;set;} = null!;
        public List<Area> Areas {get;set;} = new List<Area>(); 
        public int AreaId {get;set;}
        public Area Area {get;set;} = null!;
        public List<Table> Tables{get;set;}= new List<Table>();
        public string? AreaName { get; set; }
        public string? TableName { get; set; }
    }
}