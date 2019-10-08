using System;
using DAL;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BankContext bc = new BankContext();
            bc.Initialization(true);

            Console.WriteLine("YOLO ça a marché !");
            Console.WriteLine(bc.Clients.FirstOrDefault(c => c.LastName == "Roidebretagne").FirstName);
        }
    }
}
