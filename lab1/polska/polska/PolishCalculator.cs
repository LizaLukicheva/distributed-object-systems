using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polska
{
    class PolishCalculator
    {
        static void Main(string[] args)
        {
            while (true)
            {
                System.Console.Write("Enter expression: ");
                string readedString = System.Console.ReadLine();
                var p = new PolishString(readedString);
                System.Console.WriteLine("Polish expression: ", p.getPolska() + " = " + Convert.ToString(p.calculatePolska()));
            }
        }
    }
}
