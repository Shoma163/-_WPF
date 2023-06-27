using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Экзамен_WPF
{
    public class ClassOrder
    {
        public ClassOrder(int id, int product, string client)
        {
            this.id = id;
            this.product = product;
            this.client = client;
        }

        public int id {  get; set; }
        public int product { get; set; }
        public string client { get; set; }
    }
}
