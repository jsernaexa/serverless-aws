using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations.APIGateway;
using DogServices.Models;
using Amazon.Lambda.Annotations;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;

namespace DogServices;

public class FunctionAsignaciones
{
    private readonly DynamoDBContext _dynamoDbContext;

    public FunctionAsignaciones()
    {
        _dynamoDbContext = new DynamoDBContext(new AmazonDynamoDBClient());
    }

    [LambdaFunction(Role = "@AsignacionesApiLambdaExecutionRole")]
    [HttpApi(LambdaHttpMethod.Post, "/paseo/aleatorio")]
    public async Task PasearAleatorio([FromBody] Perro perro, ILambdaContext context)
    {
        context.Logger.LogInformation($"Validando perro: {JsonSerializer.Serialize(perro)}");
        var perroDb = await _dynamoDbContext.LoadAsync<Perro>(hashKey: perro.Raza, rangeKey: perro.Nombre);
        if(perroDb != null )
        {
            context.Logger.LogInformation($"Buscando Paseador para: {JsonSerializer.Serialize(perro)}");
            var paseadores = await _dynamoDbContext.QueryAsync<Paseador>(perro.Ciudad).GetRemainingAsync();
            context.Logger.LogInformation($"Paseadores disponibles en la ciudad de {perro.Ciudad} => {JsonSerializer.Serialize(paseadores)}");
        }

        throw new Exception("El perro no existe!!!");
    }
}
