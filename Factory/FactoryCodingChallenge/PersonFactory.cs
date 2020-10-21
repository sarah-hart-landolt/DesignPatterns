using System;
using System.Collections.Generic;
using System.Text;

namespace Factory
{
    public class PersonFactory
    {
        private static int _id = 0;
        public Person CreatePerson(string name)
        {
            return new Person
            {
                Id = _id++,
                Name = name
            };
        }
    }
}
