using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Экзамен_WPF
{
    public class ClassProduct
    {
        public ClassProduct()
        {
        }

        public ClassProduct(int id, string productname, int manufacturer, string category)
        {
            this.id = id;
            this.productname = productname;
            this.manufacturer = manufacturer;
            Category = category;
        }

        public int id { get; set; }
        public string productname { get; set; }
        public int manufacturer { get; set; }
        public string Category { get; set; }
    }
}
