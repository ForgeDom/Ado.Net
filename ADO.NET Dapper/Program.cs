using System.Diagnostics;
using System.Text;
using ADO.NET_Dapper.Methods;
using ADO.NET_Dapper.Requests;
using Data.Source.RemoteDB;
using Microsoft.Data.SqlClient;

namespace ADO.NET_Dapper;

class Program
{
    static void Main(string[] args)
    {
        Console.InputEncoding = Encoding.Unicode;
        Console.OutputEncoding = Encoding.Unicode;
        try
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=VegetablesAndFruits;Trusted_Connection=True;";
            SqlDataProvider dataProvider = new SqlDataProvider(connectionString);
            Console.Write("Connection is open: ");
            Console.WriteLine(dataProvider.IsConnectionOpen());
            /*Console.WriteLine("Execute query...");
            dataProvider.VoidExecute("CREATE DATABASE VegetablesAndFruits");
            Console.WriteLine("Query executed successfully.");*/
            Console.WriteLine("Execute query...");
            dataProvider.VoidExecute("USE VegetablesAndFruits");
            var requests = new Requests.Requests();

            while (true)
            {
                Console.WriteLine("Choose query: ");
                Console.WriteLine("1. All Data");
                Console.WriteLine("2. All Names");
                Console.WriteLine("3. All Colors");
                Console.WriteLine("4. Max Calories");
                Console.WriteLine("5. Min Calories");
                Console.WriteLine("6. Avg Calories");
                Console.WriteLine("7. All Items By Color");
                Console.WriteLine("8. Count Items By Each Color");
                Console.WriteLine("9. Count Items Below Exact Amount Calories");
                Console.WriteLine("10. Count Items Above Exact Amount Calories");
                Console.WriteLine("11. Count Items Within Calories Range");
                Console.WriteLine("12. Count Yellow or Red Items");
                Console.WriteLine("0. Exit");
                Console.Write("Your choice: ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int choice) || choice < 0 || choice > 12)
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }

                if (input == "0")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }
                
                ExactRequest requestType = choice switch
                {
                    1 => ExactRequest.All,
                    2 => ExactRequest.AllNames,
                    3 => ExactRequest.AllColors,
                    4 => ExactRequest.MaxCalories,
                    5 => ExactRequest.MinCalories,
                    6 => ExactRequest.AvgCalories,
                    7 => ExactRequest.AllItemsByColor,
                    8 => ExactRequest.ItemsCountByEachColor,
                    9 => ExactRequest.ItemsBelowCalories,
                    10 => ExactRequest.ItemsAboveCalories,
                    11 => ExactRequest.ItemsWithinCaloriesRange,
                    12 => ExactRequest.YellowOrRedItems,
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                string? query = requests.GetRequest(requestType);
                
                if (choice == 2 || choice == 3) 
                {
                    var result = dataProvider.ReaderExecute(query);
                
                    foreach (var item in result.Values)
                    {
                        foreach (var value in item)
                        {
                            Console.WriteLine(value);
                            
                        }
                    }
                }
                else if (choice == 1) 
                {
                    var result = dataProvider.ReaderExecute(query);
                
                    foreach (var item in result)
                    {
                        Console.Write($"[{item.Key}]:");
                        foreach (var value in item.Value)
                        {
                            Console.Write($" {value} ");
                        }
                        Console.WriteLine();
                    }
                }
                else if (choice == 7) 
                {
                    Console.Write("Enter color: ");
                    string? color = Console.ReadLine();
                    if (string.IsNullOrEmpty(color))
                    {
                        Console.WriteLine("Color cannot be empty. Please try again.");
                        continue;
                    }
                
                    var methodProcessing = new MethodProcessing(dataProvider);
                    methodProcessing.ProcessAllItemsByColor(color);
                    continue;
                }
                else if (choice == 8)
                {
                    var result = dataProvider.ReaderExecute(query);

                    foreach (var item in result)
                    {
                        if(int.TryParse(item.Value[0], out int count))
                        {
                            Console.WriteLine($"Color: {item.Key} - Count: {count}");
                        }
                        else
                        {
                            Console.WriteLine($"Error parsing count for color: {item.Key}");
                        }
                    }
                }
                
                else if (choice == 9)
                {
                    Console.Write("Enter maximum calories: ");
                    if (int.TryParse(Console.ReadLine(), out int maxCalories))
                    {
                        var methodProcessing = new MethodProcessing(dataProvider);
                        methodProcessing.ProcessItemsBelowCalories(maxCalories);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }
                }
                else if (choice == 10)
                {
                    Console.Write("Enter minimum calories: ");
                    if (int.TryParse(Console.ReadLine(), out int minCalories))
                    {
                        var methodProcessing = new MethodProcessing(dataProvider);
                        methodProcessing.ProcessItemsAboveCalories(minCalories);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }
                }
                else if (choice == 11)
                {
                    Console.Write("Enter minimum calories: ");
                    if (int.TryParse(Console.ReadLine(), out int minCalories))
                    {
                        Console.Write("Enter maximum calories: ");
                        if (int.TryParse(Console.ReadLine(), out int maxCalories))
                        {
                            var methodProcessing = new MethodProcessing(dataProvider);
                            methodProcessing.ProcessItemsWithinCaloriesRange(minCalories, maxCalories);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }
                }
                else if (choice == 12)
                {
                    var methodProcessing = new MethodProcessing(dataProvider);
                    methodProcessing.ProcessYellowOrRedItems();
                }
                else 
                {
                    object scalarResult = dataProvider.ScalarExecute(query);
                    Console.WriteLine($"Result: {scalarResult}");
                }
            }
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exception: {e.Message}");
        }
        Console.ReadKey();
    }
}