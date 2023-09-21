using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations.APIGateway;
using DogServices.Models;
using Amazon.Lambda.Annotations;
using System.Text.Json;

namespace DogServices;

public class FunctionPaseadores
{
    private readonly DynamoDBContext _dynamoDbContext;

    public FunctionPaseadores()
    {
        _dynamoDbContext = new DynamoDBContext(new AmazonDynamoDBClient());
    }

    [LambdaFunction(Role = "@PaseadoresApiLambdaExecutionRole")]
    [HttpApi(LambdaHttpMethod.Post, "/paseador")]
    public async Task PostPaseador([FromBody] Paseador paseador, ILambdaContext context)
    {
        context.Logger.LogInformation($"Recibiendo: {JsonSerializer.Serialize(paseador)}");
        await _dynamoDbContext.SaveAsync(paseador);
        context.Logger.LogInformation($"Paseador guardado");
    }

    [LambdaFunction(Role = "@PaseadoresApiLambdaExecutionRole")]
    [HttpApi(LambdaHttpMethod.Get, "/paseador/{ciudad}/{nombre}")]
    public async Task<Paseador> GetPaseador(string ciudad, string nombre, ILambdaContext context)
    {
        return await _dynamoDbContext.LoadAsync<Paseador>(hashKey: ciudad, rangeKey: nombre);
    }

    [LambdaFunction(Role = "@PaseadoresApiLambdaExecutionRole")]
    [HttpApi(LambdaHttpMethod.Get, "/paseador/{ciudad}")]
    public async Task<List<Paseador>> GetPaseadores(string ciudad, ILambdaContext context)
    {
        return await _dynamoDbContext.QueryAsync<Paseador>(ciudad).GetRemainingAsync();
    }

    [LambdaFunction(Role = "@PaseadoresApiLambdaExecutionRole")]
    [HttpApi(LambdaHttpMethod.Delete, "/paseador/{ciudad}/{nombre}")]
    public async Task DeletePaseador(string ciudad, string nombre, ILambdaContext context)
    {
        await _dynamoDbContext.DeleteAsync<Paseador>(hashKey: ciudad, rangeKey: nombre);
    }
}
