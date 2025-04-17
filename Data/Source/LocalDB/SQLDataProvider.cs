using System.Data;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.SqlClient;

namespace Data.Source.RemoteDB;

public class SqlDataProvider
{
    public string? ConnectionString { get; set; }

    public SqlDataProvider(string connectionString)
    {
        this.ConnectionString = connectionString;
    }
    
    public bool IsConnectionOpen()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            return connection.State == ConnectionState.Open;
        }
    }

    public void VoidExecute(string query)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException e)
        {
            throw new Exception(e.Message);
        }
    }
    
    public int ScalarExecute(string query)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }
        catch (SqlException e)
        {
            throw new Exception(e.Message);
        }
    }
    
    public Dictionary<string, List<string?>> ReaderExecute(string query)
    {
        if (query is null)
        {
            throw new Exception("Query is null");
        }
        var result = new Dictionary<string, List<string?>>();
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
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
            throw new Exception(e.Message);
        }
        return result;
    }
}