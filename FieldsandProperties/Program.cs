using System;

namespace FieldsandProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person();
            person.Name = "Tom";
            Console.WriteLine(person.Name);
        }
    }

    class Person
    {
        public string Name;
    }
}
