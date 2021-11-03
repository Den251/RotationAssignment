using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotationAssignment.Models
{
    public class Prospect
    {
        public Prospect()
        {
            Terminals = new List<TerminalInProspect>();
        }
        public DateTime? ATA { get; set; }
        public List<TerminalInProspect> Terminals {get;set;}
        public class TerminalInProspect : Terminal
        {
            public TerminalInProspect()
            {
                Cargoes = new List<Cargo>();
            }
            public List<Cargo> Cargoes { get; set; }
        }
    }
}

   
        
    

