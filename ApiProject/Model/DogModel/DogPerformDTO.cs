using MongoDB.Bson.Serialization.Attributes;

namespace ApiProject.Model.DogModel
{
    public class DogPerformDTO
    {
        public int Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }

        public string Breed { get; set; }

        public string Wool { get; set; }
        public int Age { get; set; }

        public DateTime DateBirth { get; set; }

        public string Sex { get; set; }

        public int Growth { get; set; }

        public int Weight { get; set; }

        public int KSY { get; set; }

        public string Namenursery { get; set; }

        public int? Chip { get; set; }

        public string Photo { get; set; }
        public bool? Puppies { get; set; } = false;


        public int UserId { get; set; }


        public int? Mother_Id { get; set; }
        public int? Father_Id { get; set; }

        public string? UserName { get; set; }
        public string? UserSurname { get; set; }

    }
}
