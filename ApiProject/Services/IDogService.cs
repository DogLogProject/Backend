using ApiProject.Model;
using ApiProject.Model.DogModel;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Services
{
    public interface IDogService
    {
        Task<List<Dog>> GetByBreed(string name);
        Task<List<Dog>> GetByWool(string name);
        Task<List<Dog>> GetByAge(int age);
        Task<bool> HasChildren(int dogId);
        Task<List<Dog>> GetByName(string name);
    
    }
}
