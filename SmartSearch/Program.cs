using System;

namespace SmartSearch
{
    class Program
    {
        static readonly string[] _fruits = new string[]{
            "Apples", "Apricots", "Avocados",
            "Bananas", "Boysenberries", "Blueberries", "Bing Cherry", "Blackberries",
            "Cherries", "Cantaloupe", "Crab apples", "Clementine", "Cucumbers",
            "Melons", "Pears", "Grapes", "Strawberries", 
        };
        static void Main()
        {
            SmartDictionary<string> sd = new(m => m, _fruits);
            bool finished = false;
            while(!finished)
            {
                Console.WriteLine("Enter your search:");
                string search = Console.ReadLine();

                Console.WriteLine();
                foreach (string fruit in sd.Search(search, 5))
                {
                    Console.WriteLine(fruit);
                }
                Console.WriteLine("Finished? (y/n)");
                finished = Console.ReadKey().KeyChar == 'y';
            }
        }
    }
}
