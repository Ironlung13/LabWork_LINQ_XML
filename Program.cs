using System;
using System.Xml.Linq;

namespace LabWork_LINQ_XML
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Choose task:");
            Console.WriteLine("1: LinqXML6");
            Console.WriteLine("2: LinqXML16");
            Console.WriteLine("3: LinqXML26");
            Console.WriteLine("4: LinqXML46");
        TaskChoice:
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    XMLTasks.XMLTask1_6();
                    break;
                case "2":
                    Console.Clear();
                    XMLTasks.XMLTasks2_16();
                    break;
                case "3":
                    Console.Clear();
                    XMLTasks.XMLTasks3_26();
                    break;
                case "4":
                    Console.Clear();
                    XMLTasks.XMLTasks4_46();
                    break;
                default:
                    goto TaskChoice;
            }
            Console.WriteLine("To exit program, enter \"q\"");
            Console.WriteLine("To restart, enter anything else.");
            switch (Console.ReadLine())
            {
                case "q":
                case "Q":
                    return;
                default:
                    Console.Clear();
                    Main();
                    break;
            }
        }
    }
}
