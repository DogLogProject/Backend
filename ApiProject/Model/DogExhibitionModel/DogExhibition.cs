using ApiProject.Model.DogModel;
using System.ComponentModel.DataAnnotations;

namespace ApiProject.Model.DogExhibitionModel
{
    public class DogExhibition
    {
        public int Id { get; set; }
        public int DogId { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]       
        public DateTime DateBirth { get; set; }
        public string Venue { get; set; }
        public string expertJudge { get; set; }
        public string evaluationExterior { get; set; }
        public int totalPoints { get; set; }
        public string Class {  get; set; }
        public List<DogPhoto> Photos { get; set; } = new List<DogPhoto>();

    }
}
