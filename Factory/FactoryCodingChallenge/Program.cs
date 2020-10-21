using System;

namespace Factory
{
    class Program
    {
        //factory coding challenge on Udemy Design Patterns Course
        static void Main(string[] args)
        {
            var personFactory = new PersonFactory();
            var Sarah = personFactory.CreatePerson("Sarah");
            var Jordan = personFactory.CreatePerson("Jordan");
            var Elizabeth = personFactory.CreatePerson("Elizabeth");

          
            Console.WriteLine(Sarah);
            Console.WriteLine(Jordan);
            Console.WriteLine(Elizabeth);

        }
    }
}
