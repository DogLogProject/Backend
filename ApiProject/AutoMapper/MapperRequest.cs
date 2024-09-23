using AutoMapper;
using ApiProject.Model.DogModel;
using ApiProject.Model.UserModel;
using ApiProject.Model;

namespace ApiProject.AutoMapper
{
    public class InitializeAutomapper : Profile
    {
        public InitializeAutomapper()
        {
            CreateMap<Dog, DogDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
