using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Экзамен_WPF
{
    public class ClassManufacturer
    {
        public ClassManufacturer(int id, string name)
        {
            this.id = id;
            Name = name;
        }

        public int id { get; set; }
        public string Name { get; set; }
    }
}
