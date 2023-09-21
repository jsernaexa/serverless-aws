using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogServices.Models
{
    public class Perro
    {
        public string? Id { get; set; }
        public string? Nombre { get; set; }
        [DynamoDBHashKey]
        public string? Raza { get; set; }
        public int Edad { get; set; }
        public string? Observaciones { get; set; }
        public string? Direccion { get; set;}
        public string? Ciudad { get; set; }
    }
}
