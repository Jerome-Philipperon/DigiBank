using DAL;
using System;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BankContext bc = new BankContext();
            bc.Initialization(true);
            Console.WriteLine("Yolo ça marche");
            Console.WriteLine(bc.Managers.Find(1).LastName);
        }
    }
}
