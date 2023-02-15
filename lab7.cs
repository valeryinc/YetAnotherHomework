using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Program
{
    public static void Main()
    {
        CookBook cookBook = new(@"C:\Users\vtat5\Desktop\cookbook.txt");

        foreach(var i in cookBook.dishes)
        {
            Console.WriteLine(i.Key);
            foreach(var k in i.Value)
            {
                Console.WriteLine($"{k.name} | {k.qty} | {k.measure}");
            }
            Console.WriteLine();
        }

        var shopList = CookBook.GetShopListByDishes(cookBook, new() { "Фахитос" }, 3);
        foreach(var j in shopList)
        {
            Console.WriteLine($"{j.Key}: {j.Value.qty} {j.Value.measure}");
        }

        Task3.GetSolution(@"C:\Users\vtat5\Desktop\niggers");
    }
}

public class CookBook
{
    public Dictionary<string, List<Ingredient>> dishes = new();

    public CookBook(string path) => DishParser(path);

    public Dictionary<string, List<Ingredient>> DishParser(string path)
    {
        using (var reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;
                string dishName = line;
                int ingredientCount = int.Parse(reader.ReadLine());
                var ingredients = new List<Ingredient>();

                for (int i = 0; i < ingredientCount; i++)
                {
                    string[] ingredientLine = reader.ReadLine().Split('|');
                    ingredients.Add(new Ingredient() { name = ingredientLine[0].Trim(), qty = int.Parse(ingredientLine[1].Trim()), measure = ingredientLine[2].Trim() });
                }

                dishes.Add(dishName, ingredients);
            }
        }
        return dishes;
    }

    public static Dictionary<string, Ingredient> GetShopListByDishes(CookBook cookBook, List<string> dishes, int personCount)
    {
        Dictionary<string, Ingredient> shopList = new Dictionary<string, Ingredient>();

        foreach (var dish in dishes)
        {
            foreach (var ingredient in cookBook.dishes[dish])
            {
                if (shopList.ContainsKey(ingredient.name))
                    shopList[ingredient.name].qty += ingredient.qty * personCount;
                else
                    shopList.Add(ingredient.name, new() { name = ingredient.name, qty = ingredient.qty * personCount, measure = ingredient.measure });
            }
        }

        return shopList;
    }

    public class Ingredient
    {
        public string name;
        public int qty;
        public string measure;
    }
}

public class Task3
{
    public static void GetSolution(string directoryPath)
    {
        File.Delete(Path.Combine(directoryPath, "output.txt"));
        string[] paths = Directory.GetFiles(directoryPath);
        Dictionary<string, int> linesCounts = new();
        for(int i = 0; i < paths.Length; ++i)
            linesCounts.Add(paths[i], File.ReadLines(paths[i]).Count());
        var sortedLinesCounts = linesCounts.OrderBy(x => x.Value);

        using (var writer = new StreamWriter(Path.Combine(directoryPath, "output.txt")))
        {
            foreach (var kvp in sortedLinesCounts)
            {
                writer.WriteLine(kvp.Key[(kvp.Key.LastIndexOf('\\') + 1)..]);
                writer.WriteLine(kvp.Value);

                using (var reader = new StreamReader(kvp.Key))
                {
                    while (!reader.EndOfStream)
                    {
                        writer.WriteLine(reader.ReadLine());
                    }
                }
            }
        }
    }
}
