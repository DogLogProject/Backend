using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using ApiProject.Model.DogModel;
using System.ComponentModel.DataAnnotations;

namespace ApiProject.Model
{
    public class User
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; } 
        public string Password { get; set; }
        public string Phone { get; set; }
        public string? Location { get; set; } 
        public string? Namenursery { get; set; }
        public string? Photo { get; set; }

       

    }
}
