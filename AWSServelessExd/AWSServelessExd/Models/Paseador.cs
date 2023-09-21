﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogServices.Models
{
    public class Paseador
    {
        public string? Nombre { get; set; }
        public int Edad { get; set; }

        [DynamoDBHashKey]
        public string? Ciudad { get; set; }
        public string? Id { get; set; }
    }
}
