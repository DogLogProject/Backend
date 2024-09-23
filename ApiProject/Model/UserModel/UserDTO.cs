using ApiProject.Model.DogModel;

namespace ApiProject.Model.UserModel
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? Namenursery { get; set; }
        public string? Photo { get; set; }
        public List<DogSortInfo>? Dogs { get; set; } = new List<DogSortInfo>();


    }
}
