using ApiProject.Services;
using ApiProject.Model.DogModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ApiProject.Context;
using ApiProject.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDogService _dogService;
        private readonly IGenericRepository<Dog> _genericRepository;

        public DogController(IDogService dogService, ApiDbContext context, IMapper mapper, IGenericRepository<Dog> genericRepository)
        {
            _dogService = dogService;
            _context = context;
            _mapper = mapper;
            _genericRepository = genericRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dog>>> GetDogs()
        {
            var dogs = await _genericRepository.GetAll();
            return Ok(dogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dog>> GetDogById(int id)
        {
            var dog = await _genericRepository.GetById(id);
            if (dog == null)
            {
                return NotFound();
            }
            return Ok(dog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDog(int id, DogDTO dogDTO)
        {
            var dog = await _genericRepository.GetById(id);
            if (dog == null)
            {
                return NotFound();
            }

            _mapper.Map(dogDTO, dog);
            _genericRepository.Update(dog);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<DogDTO>> PostDog(DogDTO dogDTO)
        {
            dogDTO.DateBirth = DateTime.SpecifyKind(dogDTO.DateBirth, DateTimeKind.Utc);

            var dog = _mapper.Map<Dog>(dogDTO);
            _genericRepository.Insert(dog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDogById), new { id = dog.Id }, _mapper.Map<DogDTO>(dog));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDog(int id)
        {
            var dog = await _genericRepository.GetById(id);
            if (dog == null)
            {
                return NotFound();
            }

            var puppies = await _context.Dogs.Where(d => d.Mother_Id == id || d.Father_Id == id).ToListAsync();

            foreach (var puppy in puppies)
            {
                if (puppy.Mother_Id == id)
                {
                    puppy.Mother_Id = null;
                }
                if (puppy.Father_Id == id)
                {
                    puppy.Father_Id = null;
                }
            }

            _context.Dogs.UpdateRange(puppies);
            _genericRepository.Delete(dog);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
            }

            return NoContent();
        }

        

        [HttpGet("GetDogByBreed/{breed}")]
        public async Task<ActionResult<List<DogPerformDTO>>> GetDogByBreed(string breed)
        {
            var dogsWithUsers = await _context.Dogs
                .Where(d => d.Breed == breed)
                .Join(_context.Users,
                      dog => dog.UserId,
                      user => user.Id,
                      (dog, user) => new { Dog = dog, User = user })
                .ToListAsync();

            var dogPerformDTOs = new List<DogPerformDTO>();

            foreach (var item in dogsWithUsers)
            {
                bool hasPuppies = await _context.Dogs.AnyAsync(d => d.Mother_Id == item.Dog.Id);

                var dogPerformDTO = new DogPerformDTO
                {
                    Id = item.Dog.Id,
                    Name = item.Dog.Name,
                    Breed = item.Dog.Breed,
                    Wool = item.Dog.Wool,
                    Age = item.Dog.Age,
                    DateBirth = item.Dog.DateBirth,
                    Sex = item.Dog.Sex,
                    Growth = item.Dog.Growth,
                    Weight = item.Dog.Weight,
                    KSY = item.Dog.KSY,
                    Namenursery = item.Dog.Namenursery,
                    Chip = item.Dog.Chip,
                    Photo = item.Dog.Photo,
                    Puppies = hasPuppies ? item.Dog.Puppies = true : null,
                    UserId = item.User.Id,
                    UserName = item.User.Name,
                    UserSurname = item.User.Surname
                };

                dogPerformDTOs.Add(dogPerformDTO);
            }

            if (dogPerformDTOs.Count == 0)
            {
                return NotFound();
            }

            return Ok(dogPerformDTOs);
        }

        [HttpGet("GetDogByName/{name}")]
        public async Task<ActionResult<List<DogSortInfo>>> GetDogByName(string name)
        {
            var nameList = _dogService.GetByName(name);

            if (nameList == null)
            {
                return NotFound();
            }

            return Ok(nameList);
        }

        private bool DogExists(int id)
        {
            return _context.Dogs.Any(e => e.Id == id);
        }
    }
}
