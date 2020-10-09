using System;
using System.Collections.Generic;
using static System.Console;

namespace Open_Closed
{
    public enum Color
    {
        Red,
        Green,
        Blue
    }

    public enum Size
    {
        Small,
        Medium,
        Large,
        Yuge
    }

    public enum Price
    {
        Cheap,
        Affordable,
        Expensive,
        Ridiculous
    }


    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;
        public Price Price;


        public Product(string name, Color color, Size size, Price price)
        {
            Name = name ??
                throw new ArgumentNullException(paramName: nameof(name));
            Color = color;
            Size = size;
            Price = price; 

        }
    }

    public class ProductFilter
    {
        // let's suppose we don't want ad-hoc queries on products
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
                if (p.Color == color)
                    yield return p;
        }

        public static IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
                if (p.Size == size)
                    yield return p;
        }

        public static IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var p in products)
                if (p.Size == size && p.Color == color)
                    yield return p;
        } // state space explosion
        // 3 criteria = 7 methods

        // OCP = open for extension but closed for modification
    }

    // we introduce two new interfaces that are open for extension

    // use the specification pattern: create interface 
    public interface ISpecification<T>
    {
        bool IsSatisfied(Product p);
    }

    // set up filter interface pulling in ISpecficiation pattern
    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    // create a class of ColorSpecification that uses ISpecification
    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        // set up constructor
        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        // specifices the method inherited from ISpecficiation 
        public bool IsSatisfied(Product p)
        {
            return p.Color == color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }

        public bool IsSatisfied(Product p)
        {
            return p.Size == size;
        }
    }
    //my added bit
    public class PriceSpecification : ISpecification<Product>
    {
        private Price price;

        public PriceSpecification(Price price)
        {
            this.price = price;
        }

        public bool IsSatisfied(Product p)
        {
            return p.Price== price;
        }
    }

    // combinator
    // making a possibility to filter by 2 Specifications instead of just color or size. Inherits  ISpecification interface
    // i edited to add 3 specifications
    public class AndSpecification<T> : ISpecification<T>
    {
        // making 2 specificaitons 
        private ISpecification<T> first,
        second, third; 

        // constructor of this class
        public AndSpecification(ISpecification<T> first, ISpecification<T> second, ISpecification<T> third)
        {
            this.first = first ??
                throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ??
                throw new ArgumentNullException(paramName: nameof(second));
            this.third = third ??
                throw new ArgumentNullException(paramName: nameof(second));
        }

        // from ISpecification, returning the product if the first and second spec are correct
        public bool IsSatisfied(Product p)
        {
            return first.IsSatisfied(p) && second.IsSatisfied(p) && third.IsSatisfied(p);
        }
    }

    // creating class of Filter that doesn't violate open/closed principle. Inherits from IFilter interface 
    //(which takes argument Product items and Ispecification interface  
    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var i in items)
                if (spec.IsSatisfied(i))
                    yield return i;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small, Price.Cheap);
            var tree = new Product("Tree", Color.Green, Size.Large, Price.Affordable);
            var house = new Product("House", Color.Blue, Size.Large, Price.Expensive);
            var car = new Product("Car", Color.Red, Size.Large, Price.Expensive);


            Product[] products = { apple, tree, house, car};

            var pf = new ProductFilter();
            WriteLine("Green products (old):");
            foreach (var p in pf.FilterByColor(products, Color.Green))
                WriteLine($" - {p.Name} is green");

            // ^^ BEFORE

            // vv AFTER
            // using the better filter// filter takes array of products and color specification as green as argument. 
            // loop over each product and return the Name of product that matches that specific color Green
            var bf = new BetterFilter();
            WriteLine("Green products (new):");
            foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
                WriteLine($" - {p.Name} is green");

            WriteLine("Large products");
            foreach (var p in bf.Filter(products, new SizeSpecification(Size.Large)))
                WriteLine($" - {p.Name} is large");

            // using the 
            WriteLine("Large blue, expensive items");
            foreach (var p in bf.Filter(products,
                    new AndSpecification<Product>(new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large), new PriceSpecification(Price.Expensive))))
            {
                WriteLine($" - {p.Name} is big, blue and expensive");
            }
        }
    }
}