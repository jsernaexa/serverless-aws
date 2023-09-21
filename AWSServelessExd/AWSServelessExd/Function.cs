using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations.APIGateway;
using DogServices.Models;
using Amazon.Lambda.Annotations;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DogServices;

public class Function
{
    private readonly DynamoDBContext _dynamoDbContext;

    public Function()
    {
        _dynamoDbContext = new DynamoDBContext(new AmazonDynamoDBClient());
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Post, "/perro")]
    public Task PostOrder([FromBody]Perro perro, ILambdaContext context)
    {
        context.Logger.LogInformation($"Received: {JsonSerializer.Serialize(perro)}");
        return Task.CompletedTask;
    }
}
