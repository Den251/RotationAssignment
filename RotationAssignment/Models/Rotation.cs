using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotationAssignment.Models
{
    public class Rotation
    {
        //public Rotation(Cargo cargo)
        //{
        //    TerminalId = cargo.TerminalId;
        //                
        //}
        public string TerminalId { get; set; }
        public List<string> Cargoes { get; set; }
        
    }
}
