// See https://aka.ms/new-console-template for more information
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;

Console.WriteLine("Hello, World!");

AmazonSQSClient _sqsClient;
var credentials = new BasicAWSCredentials("", "");
_sqsClient = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast1);

var request = new SendMessageRequest()
{
    QueueUrl = "https://sqs.us-east-1.amazonaws.com/517896355494/Paseo-request",
    MessageBody = "HOLA JONATHAN!!!!!!!!!!!!!!!!"
};

var resp = await _sqsClient.SendMessageAsync(request);

Console.ReadLine();
