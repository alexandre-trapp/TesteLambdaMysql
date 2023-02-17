using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

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
        }

        Console.WriteLine("Processing complete.");
        return $"Processed {sqsEvent.Records.Count} records.";
    }
}
