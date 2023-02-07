using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrayonDiary
{
    class Student
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public int Math { get; set; }
        public int Literature { get; set; }
        public int Biology { get; set; }
        public int PE { get; set; }
        public int IT { get; set; }

        public Student(string line)
        {
            string[] part = line.Split(';');
            Name = part[0];
            Class = part[1];
            Math = int.Parse(part[2]);
            Literature= (int)int.Parse(part[3]);
            Biology= (int)int.Parse(part[4]);
            PE = (int)int.Parse(part[5]);
            IT= (int)int.Parse(part[6]);
        }
    }
}
