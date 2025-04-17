using Data.Source.RemoteDB;
using ADO.NET_Dapper.Requests;
using Microsoft.Data.SqlClient;

namespace ADO.NET_Dapper.Methods;

public class MethodProcessing
{

    private SqlDataProvider _dataProvider;

    public void ProcessAllItemsByColor(string color)
    {
        var request = new Requests.Requests();
        string query = request.GetRequest(ExactRequest.AllItemsByColor);
    
        var result = new Dictionary<string, List<string?>>();
        try
        {
            using (var connection = new SqlConnection(_dataProvider.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    // Добавляем параметр @color
                    command.Parameters.AddWithValue("@color", color);
    
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        List<string?> values = new List<string?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            values.Add(reader[i].ToString());
                        }
    
                        string key = reader[0]?.ToString() ?? "null";
                        if (!result.ContainsKey(key))
                        {
                            result.Add(key, values);
                        }
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine($"SQL Exception: {e.Message}");
            return;
        }
    
        if (result == null || result.Count == 0)
        {
            Console.WriteLine("No items found for the specified color.");
            return;
        }
    
        foreach (var item in result)
        {
            Console.WriteLine($"[{item.Key}]");
            foreach (var value in item.Value)
            {
                Console.WriteLine($" {value} ");
            }
            Console.WriteLine();
        }
    }
    
    public void ProcessItemsBelowCalories(int calories)
    {
        var request = new Requests.Requests();
        string query = request.GetRequest(ExactRequest.ItemsBelowCalories).Replace("{calories}", calories.ToString());
    
        var result = _dataProvider.ReaderExecute(query);
    
        foreach (var item in result)
        {
            Console.WriteLine($"[{item.Key}]: {string.Join(", ", item.Value)}");
        }
    }
    
    public void ProcessItemsAboveCalories(int calories)
    {
        var request = new Requests.Requests();
        string query = request.GetRequest(ExactRequest.ItemsAboveCalories).Replace("{calories}", calories.ToString());
    
        var result = _dataProvider.ReaderExecute(query);
    
        foreach (var item in result)
        {
            Console.WriteLine($"[{item.Key}]: {string.Join(", ", item.Value)}");
        }
    }
    
    public void ProcessItemsWithinCaloriesRange(int minCalories, int maxCalories)
    {
        var request = new Requests.Requests();
        string query = request.GetRequest(ExactRequest.ItemsWithinCaloriesRange);
    
        var result = new Dictionary<string, List<string?>>();
        try
        {
            using (var connection = new SqlConnection(_dataProvider.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@minCalories", minCalories);
                    command.Parameters.AddWithValue("@maxCalories", maxCalories);
    
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        List<string?> values = new List<string?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            values.Add(reader[i].ToString());
                        }
    
                        string key = reader[0]?.ToString() ?? "null";
                        if (!result.ContainsKey(key))
                        {
                            result.Add(key, values);
                        }
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine($"SQL Exception: {e.Message}");
            return;
        }
    
        foreach (var item in result)
        {
            Console.WriteLine($"[{item.Key}]: {string.Join(", ", item.Value)}");
        }
    }
    
    public void ProcessYellowOrRedItems()
    {
        var request = new Requests.Requests();
        string query = request.GetRequest(ExactRequest.YellowOrRedItems);
    
        var result = _dataProvider.ReaderExecute(query);
    
        foreach (var item in result)
        {
            Console.WriteLine($"[{item.Key}]: {string.Join(", ", item.Value)}");
        }
    }
    public MethodProcessing(SqlDataProvider dataProvider)
    {
        this._dataProvider = dataProvider;
    }

    ~MethodProcessing()
    { 
        
    }
}
