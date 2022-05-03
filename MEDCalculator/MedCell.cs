using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDCalculator
{
    class MedCell
    {
        public int value { get; set; }
        public MedCell parent { get; set; }
        public string method { get; set; }

        public MedCell()
        {

        }

        public MedCell(int val,MedCell par,string met)
        {
            value = val;
            parent = par;
            method = met;
        }
    }
}
