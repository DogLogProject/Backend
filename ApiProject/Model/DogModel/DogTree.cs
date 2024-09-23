using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProject.Model.DogModel
{
    public class DogTree
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }

        public int? Mother_Id { get; set; }
        public int? Father_Id { get; set; }

        [ForeignKey("Mother_Id")]
        public DogTree? Mother { get; set; }

        [ForeignKey("Father_Id")]
        public DogTree? Father { get; set; }
    }
}
