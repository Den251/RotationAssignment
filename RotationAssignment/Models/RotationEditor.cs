using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotationAssignment.Models
{
    public class RotationEditor
    {
        public List<Terminal> terminals { get; set; }
        public class Cargo
        {
            public string CargoId { get; set; }
        }

        public class Terminal
        {
            public string TerminalId { get; set; }
            public List<Cargo> Cargoes { get; set; }
        }
    }
}
