using ApiProject.Context;
using ApiProject.Model.DogModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProject.Services
{
    public class DogService : IDogService
    {
        private readonly ApiDbContext _context;

        public DogService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Dog>> GetByBreed(string breed)
        {
            return await _context.Dogs.Where(dog => dog.Breed == breed).ToListAsync();
        }

        public async Task<List<Dog>> GetByWool(string wool)
        {
            return await _context.Dogs.Where(dog => dog.Wool == wool).ToListAsync();
        }

        public async Task<List<Dog>> GetByAge(int age)
        {
            return await _context.Dogs.Where(dog => dog.Age == age).ToListAsync();
        }

        public async Task<bool> HasChildren(int dogId)
        {
            return _context.Dogs.Any(d => d.Mother_Id == dogId || d.Father_Id == dogId);
        }

        public async Task<List<Dog>> GetByName(string name)
        {
            return await _context.Dogs.Where(dog => dog.Name == name).ToListAsync();
        }

    }
}
