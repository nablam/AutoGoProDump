using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GittedGoProDump
{
    class Program
    {
        static int mili = 1000;
        static void Main(string[] args)
        {
            int secInterval = 8;
            Console.WriteLine("hello world");
            RestClientManager rcm = new RestClientManager();
           // for (int i = 0; i < 15; i++)
                for (; ; )
                {

                Console.WriteLine("Sleep for " + +secInterval + " seconds.");
                Thread.Sleep(secInterval*mili);
                rcm.RunQuerySync();
                Console.WriteLine("ready for next xeck in " +secInterval );

            }
            Console.WriteLine("ready for zzzzzzzzzz");



        }
    }
}
