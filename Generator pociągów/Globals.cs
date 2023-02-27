using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator_pociągów
{
    public class Globals
    {
        public static string start;
        public static string finish;
        public static List<viaStation> posrednie = new List<viaStation>();
        public static string number, name;
    }
    public class viaStation
    {
        public string name;
        public bool isBold;
        public viaStation(string name, bool isBold)
        {
            this.name = name;
            this.isBold = isBold;
        }
    }
}
