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

public class FunctionPerros
{
    private readonly DynamoDBContext _dynamoDbContext;

    public FunctionPerros()
    {
        _dynamoDbContext = new DynamoDBContext(new AmazonDynamoDBClient());
    }

    [LambdaFunction(Role = "@PerrosApiLambdaExecutionRole")]
    [HttpApi(LambdaHttpMethod.Post, "/perro")]
    public async Task PostPerro([FromBody]Perro perro, ILambdaContext context)
    {
        context.Logger.LogInformation($"Recibiendo: {JsonSerializer.Serialize(perro)}");
        await _dynamoDbContext.SaveAsync(perro);
        context.Logger.LogInformation($"Perro guardado");
    }

    [LambdaFunction(Role = "@PerrosApiLambdaExecutionRole")]
    [HttpApi(LambdaHttpMethod.Get, "/perro/{raza}/{nombre}")]
    public async Task<Perro> GetPerro(string raza, string nombre, ILambdaContext context)
    {
        return await _dynamoDbContext.LoadAsync<Perro>(hashKey: raza, rangeKey: nombre);
    }

    [LambdaFunction(Role = "@PerrosApiLambdaExecutionRole")]
    [HttpApi(LambdaHttpMethod.Get, "/perro/{raza}")]
    public async Task<List<Perro>> GetPerros(string raza, ILambdaContext context)
    {
        return await _dynamoDbContext.QueryAsync<Perro>(raza).GetRemainingAsync();
    }

    [LambdaFunction(Role = "@PerrosApiLambdaExecutionRole")]
    [HttpApi(LambdaHttpMethod.Delete, "/perro/{raza}/{nombre}")]
    public async Task DeletePerro(string raza, string nombre, ILambdaContext context)
    {
        await _dynamoDbContext.DeleteAsync<Perro>(hashKey: raza, rangeKey: nombre);
    }
}
