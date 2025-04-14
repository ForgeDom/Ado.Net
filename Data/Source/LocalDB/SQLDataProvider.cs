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
    
    public SqlDataReader ReaderExecute(string query)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    return command.ExecuteReader();
                }
            }
        }
        catch (SqlException e)
        {
            throw new Exception(e.Message);
        }
    }
}