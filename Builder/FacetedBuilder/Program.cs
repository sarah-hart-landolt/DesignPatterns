using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace FacetedBuilder
{
    public class Person
    {
        // address
        public string StreetAddress, Postcode, City;

        // employment
        public string CompanyName, Position;

        public int AnnualIncome;

        public override string ToString()
        {
            return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(Postcode)}: {Postcode}, {nameof(City)}: {City}, {nameof(CompanyName)}: {CompanyName}, {nameof(Position)}: {Position}, {nameof(AnnualIncome)}: {AnnualIncome}";
        }
    }

    public class PersonBuilder // facade (a component which hides a lot of information behind it)
    {
        // the object we're going to build
        protected Person person = new Person(); // this is a reference!

        // we then have two more buiders for address and job within the person builder
        public PersonAddressBuilder Lives => new PersonAddressBuilder(person);
        public PersonJobBuilder Works => new PersonJobBuilder(person);


        //how we can get access to printing a "Person" without exposing another bit of the API 
        //however, he says that it's not a great programming approach
        public static implicit operator Person(PersonBuilder pb)
        {
            return pb.person;
        }
    }

    // this builder inhertis from PersonBuilder
    public class PersonJobBuilder : PersonBuilder
    {
        // constructor takes an argument of person
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        // Method : At
        public PersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        // Method AsA
        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }
        // method Earning 
        public PersonJobBuilder Earning(int annualIncome)
        {
            person.AnnualIncome = annualIncome;
            return this;
        }
    }
    // another builder inheriting from Person Builder
    public class PersonAddressBuilder : PersonBuilder
    {
        // might not work with a value type!
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string streetAddress)
        {
            person.StreetAddress = streetAddress;
            return this;
        }

        public PersonAddressBuilder WithPostcode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }

    }

    public class Program
    {
        static void Main(string[] args)
        {
            // calls a new Person Builder 
            var pb = new PersonBuilder();
            //uses Person instead of var to be able to access the "implicit operator Person: to be able to Consolewriteline"
            Person person = pb
                // implementing the address builder
              .Lives
                .At("123 London Road")
                .In("London")
                .WithPostcode("SW12BC")
                //implementing the job builder
              .Works
                .At("Fabrikam")
                .AsA("Engineer")
                .Earning(123000);

            WriteLine(person);
        }
    }
}
