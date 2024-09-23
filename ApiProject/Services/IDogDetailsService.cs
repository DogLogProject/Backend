using ApiProject.Model.DogModel;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Services
{
    public interface IDogDetailsService
    {
        Task<DogTree> GetDogWithAncestorsAsync(int id, int maxDepth = 3, int currentDepth = 0);
        Task<DogTree> GetUserWithDogs(int id);
        Task<DogSortInfo> GetDogWithPhoto(int id);
        Task<DogSortInfo> GetDogWithChildren(int id);
        Task<DogSortInfo> GetDogWithUser(int id);
    }
}
