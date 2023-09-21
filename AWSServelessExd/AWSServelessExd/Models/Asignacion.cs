using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogServices.Models
{
    public class Asignacion
    {
        public Paseador? Paseador { get; set; }
        public List<Perro>? Perros { get; set; }
    }
}
