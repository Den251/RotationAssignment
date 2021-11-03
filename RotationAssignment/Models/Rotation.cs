using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotationAssignment.Models
{
    public class Rotation
    {
        public Rotation()
        {
            Terminals = new List<Terminal>();
        }
        public List<Terminal> Terminals { get; set; }
        public class Cargo
        {
            public string CargoId { get; set; }
        }

        public class Terminal
        {
            public Terminal()
            {
                Cargoes = new List<Cargo>();
            }
            public string TerminalId { get; set; }
            public List<Cargo> Cargoes { get; set; }
        }
    }
}
