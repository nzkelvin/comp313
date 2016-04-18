using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var angle = Math.PI * 180 / 180.0;
            var x = (float)Math.Cos(angle) * 1;
            var y = (float)Math.Sin(angle) * 1;
            Console.WriteLine("x: {0}; y {1}; angle: {2}", x, y, angle);
            Console.Read();
        }
    }
}
