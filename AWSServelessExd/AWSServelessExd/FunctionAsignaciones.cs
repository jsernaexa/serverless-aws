using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations.APIGateway;
using DogServices.Models;
using Amazon.Lambda.Annotations;
using System.Text.Json;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace DogServices;

public class FunctionAsignaciones
{
    private readonly AmazonSQSClient _sqsClient;
    private readonly DynamoDBContext _dynamoDbContext;

    public FunctionAsignaciones()
    {
        var credentials = new BasicAWSCredentials("AKIAXRFIHH2TJDBPOX6Z", "fZQALZscyKcaCcvG3V971qcCmHYJOHYxVmFPyhtU");
        _sqsClient = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast1);
        _dynamoDbContext = new DynamoDBContext(new AmazonDynamoDBClient());
    }

    [LambdaFunction(Role = "@AsignacionesApiLambdaExecutionRole")]
    [HttpApi(LambdaHttpMethod.Post, "/paseo/aleatorio")]
    public async Task PasearAleatorio([FromBody] Perro perro, ILambdaContext context)
    {
        context.Logger.LogInformation($"Validando perro: {JsonSerializer.Serialize(perro)}");
        var perroDb = await _dynamoDbContext.LoadAsync<Perro>(hashKey: perro.Raza, rangeKey: perro.Nombre);
        if(perroDb == null )
        {
            throw new Exception("El perro no existe!!!");
        }

        context.Logger.LogInformation($"Buscando Paseador para: {JsonSerializer.Serialize(perro)}");
        var paseadores = await _dynamoDbContext.QueryAsync<Paseador>(perro.Ciudad).GetRemainingAsync();
        context.Logger.LogInformation($"Paseadores disponibles en la ciudad de {perro.Ciudad} => {JsonSerializer.Serialize(paseadores)}");
        var request = new SendMessageRequest()
        {
            QueueUrl = "https://sqs.us-east-1.amazonaws.com/517896355494/Paseo-request",
            MessageBody = JsonSerializer.Serialize(new Asignacion { Paseador = paseadores.FirstOrDefault(), Perro = perro })
        };
        
        var resp = await _sqsClient.SendMessageAsync(request);
        context.Logger.LogInformation($"Message sent to sqs {resp.MessageId}");
    }
}
