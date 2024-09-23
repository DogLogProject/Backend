using ApiProject.Context;
using ApiProject.Model;
using ApiProject.Model.DogModel;
using ApiProject.Model.UserModel;
using ApiProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogDetailsController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IDogDetailsService _dogDetails;


        public DogDetailsController(ApiDbContext context, IDogDetailsService dogDetails)
        {
            _dogDetails = dogDetails;
            _context = context;
        }

        [HttpGet("with-children/{id}")]
        public async Task<ActionResult<DogSortInfo>> GetDogWithChildren(int id)
        {
            var dogWithChildren = await _context.Dogs
                .Where(d => d.Id == id)
                .Select(dog => new DogSortInfo
                {
                    Name = dog.Name,
                    Puppies = _context.Dogs
                        .Where(child => child.Mother_Id == dog.Id || child.Father_Id == dog.Id)
                        .Select(child => new DogSortInfo
                        {
                            Id = child.Id,
                            Photo = child.Photo,
                            Name = child.Name,
                            Sex = child.Sex,
                            Wool = child.Wool,
                            Breed = child.Breed,
                            Age = child.Age
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (dogWithChildren == null)
            {
                return NotFound();
            }

            return Ok(dogWithChildren);
        }

        [HttpGet("album-dog/{id}")]
        public async Task<ActionResult<DogSortInfo>> GetDogWithPhoto(int id)
        {
            var dogWithPhoto = await _context.Dogs
                .Where(d => d.Id == id)
                .Select(dog => new DogSortInfo
                {
                    Name = dog.Name,
                    Photos = _context.DogPhotos
                .Where(photo => photo.DogId == dog.Id)
                .Select(photo => new DogPhoto
                {
                    Id = photo.Id,
                    Photo = photo.Photo,
                    DogId = photo.DogId
                }).ToList()
                })
                .FirstOrDefaultAsync();

            if (dogWithPhoto == null)
            {
                return NotFound();
            }

            return Ok(dogWithPhoto);
        }

        [HttpGet("users-with-dogs/{id}")]
        public async Task<ActionResult<DogSortInfo>> GetUserWithDogs(int id)
        {
            var userWithDogs = await _context.Users
                .Where(u => u.Id == id)
                .Select(user => new UserDTO
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Phone = user.Phone,
                    Location = user.Location,
                    Namenursery = user.Namenursery,
                    Photo = user.Photo,

                    Dogs = _context.Dogs
                        .Where(d => d.UserId == user.Id)
                        .Select(dog => new DogSortInfo
                        {
                            Id = dog.Id,
                            Photo = dog.Photo,
                            Name = dog.Name,
                            Sex = dog.Sex,
                            Wool = dog.Wool,
                            Breed = dog.Breed,
                            Age = dog.Age,
                            Chip = dog.Chip,
                            Diplomas = dog.Diplomas,
                            Exterior = dog.Exterior,
                            DateBirth = dog.DateBirth,

                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (userWithDogs == null)
            {
                return NotFound();
            }

            return Ok(userWithDogs);
        }

        [HttpGet("dog-user/{id}")]
        public async Task<ActionResult<DogSortInfo>> GetDogWithUser(int id)
        {
            var dogWithOwner = await _context.Dogs
                .Where(d => d.Id == id)
                .Select(dog => new DogSortInfo
                {
                    Name = dog.Name,
                    Photo = dog.Photo,
                    Owner = _context.Users
                        .Where(u => u.Id == dog.UserId)
                        .Select(user => new UserDTO
                        {
                            Name = user.Name,
                            Surname = user.Surname,
                            Email = user.Email,
                            Phone = user.Phone,
                            Location = user.Location,
                            Namenursery = user.Namenursery,
                            Photo = user.Photo,
                            Dogs = _context.Dogs
                                .Where(d => d.UserId == user.Id)
                                .Select(d => new DogSortInfo
                                {
                                    Id = d.Id,
                                    Photo = d.Photo,
                                    Name = d.Name,
                                    Sex = d.Sex,
                                    Wool = d.Wool,
                                    Breed = d.Breed,
                                    Age = d.Age
                                })
                                .ToList()
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (dogWithOwner == null)
            {
                return NotFound();
            }

            return Ok(dogWithOwner);
        }

        [HttpGet("dog-with-ancestors/{id}")]
        public async Task<ActionResult<DogTree>> GetDogWithAncestors(int id)
        {
            var dogWithAncestors = await _dogDetails.GetDogWithAncestorsAsync(id);

            if (dogWithAncestors == null)
            {
                return NotFound();
            }

            return Ok(dogWithAncestors);
        }



    }
}

