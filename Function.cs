using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

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
    public string HandleSQSEvent(SQSEvent sqsEvent, ILambdaContext context)
    {
        Console.WriteLine("ConnectionString aws lambda mysql");

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

    private static async Task<string> GetSecretAsync()
    {
        string secretName = "connectionString";
        string region = "us-east-1";

        IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
        };

        GetSecretValueResponse response;

        response = await client.GetSecretValueAsync(request);

        return response.SecretString;
    }
}