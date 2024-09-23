using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ApiProject.Model.DogModel
{
    public class DogDTO
    {

        [BsonElement("Name")]
        public string Name { get; set; }

        public string Breed { get; set; }

        public string Wool { get; set; }
        public int Age { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateBirth { get; set; }

        public string Sex { get; set; }

        public int Growth { get; set; }

        public int Weight { get; set; }

        public int KSY { get; set; }

        public string Namenursery { get; set; }

        public int? Chip { get; set; }

        public string Photo { get; set; }
        public string Diplomas { get; set; }
        public string Exterior { get; set; }
        public int UserId { get; set; }
        public int? Mother_Id { get; set; }
        public int? Father_Id { get; set; }       
    }
}
