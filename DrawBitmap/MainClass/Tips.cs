using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawBitmap.MainClass
{
    class Tips
    {
        public string name;
        public string author;
        public string date;
        public string text;
        public string biaoqian;
        public string quanxian;
        public string kind;
        public Tips()
        {
        }
        public Tips(string name,string author,string date,string text,string biaoqian,string quanxian,string kind)
        {
            this.name = name;
            this.author = author;
            this.date = date;
            this.text = text;
            this.biaoqian = biaoqian;
            this.quanxian = quanxian;
            this.kind = kind;
        }
    }
}
