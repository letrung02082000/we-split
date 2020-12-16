using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit
{
    class ChartData
    {
        private Dictionary<string, double> slice = new Dictionary<string, double>();

        public Dictionary<string, double> Slice
        {
            get { return slice; }
            set { slice = value; }
        }

        public void AddSlice(string slicename, double slicevalue)
        {
            slice.Add(slicename, slicevalue);
        }
    }
}
