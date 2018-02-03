using System;

namespace StaticMainMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int a = 4;
            byte b = 6;
            
            Testing.Function();

            Console.WriteLine(a.Equals(b));
        }
    }

    class Testing
    {
        public static void Function() 
        {
            Console.WriteLine("Jeremy Lin");
        }
    }
}
