using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Metrics;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TesteLambdaMysql;

public class Function
{
    /// <summary>
    /// A simple function that takes a RDS event
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        Console.WriteLine($"Beginning to process {sqsEvent.Records.Count} records...");

        foreach (var record in sqsEvent.Records)
        {
            Console.WriteLine($"Message ID: {record.MessageId}");
            Console.WriteLine($"Event Source: {record.EventSource}");

            Console.WriteLine($"Record Body:");
            Console.WriteLine(record.Body);

            SetInDatabase(record.Body);
        }

        Console.WriteLine("Processing complete.");
        return $"Processed {sqsEvent.Records.Count} records.";
    }

    private void SetInDatabase(string body)
    {
        string configDB = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        using var _connection = new MySqlConnection(configDB);

        if (_connection.State == ConnectionState.Closed)
            _connection.Open();

        using var cmd = new MySqlCommand("INSERT INTO TESTE(objeto) VALUES(?objeto) ", _connection);
        cmd.Parameters.Add("?objeto", MySqlDbType.VarChar).Value = body;

        cmd.ExecuteNonQuery();

        using var cmd2 = new MySqlCommand("SELECT * FROM TESTE", _connection);
        using var reader = cmd2.ExecuteReader();

        Console.WriteLine("Log: reader.FieldCount: " + reader.FieldCount);

        while (reader.Read())
        {
            Console.WriteLine($"Objeto = {reader["objeto"]}");
        }
    }
}
