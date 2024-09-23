using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ApiProject.Model.UserModel;
using System.ComponentModel.DataAnnotations;

namespace ApiProject.Model.DogModel
{
    public class DogSortInfo
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Breed { get; set; }
        public string Wool { get; set; }
        public int Age { get; set; }
        public int KSY { get; set; }
        public int? Chip { get; set; }
        public string Diplomas { get; set; }
        public string Exterior { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateBirth { get; set; }

        public UserDTO Owner { get; set; }
        public List<DogSortInfo> Puppies { get; set; } = new List<DogSortInfo>();

        public List<DogPhoto> Photos { get; set; } = new List<DogPhoto>();



    }
}
