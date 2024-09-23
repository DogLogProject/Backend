using ApiProject.Context;
using ApiProject.Model.DogModel;
using ApiProject.Model.UserModel;
using ApiProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class DogDetailsService : IDogDetailsService
{
    private readonly ApiDbContext _context;
    private readonly IMemoryCache _cache;

    public DogDetailsService(ApiDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<DogTree> GetDogWithAncestorsAsync(int id, int maxDepth = 4, int currentDepth = 0)
    {
        if (currentDepth > maxDepth)
        {
            return null;
        }

        var dog = await _context.Dogs
            .Where(d => d.Id == id)
            .Select(d => new DogTree
            {
                Id = d.Id,
                Name = d.Name,
                Photo = d.Photo,
                Mother_Id = d.Mother_Id,
                Father_Id = d.Father_Id
            })
            .FirstOrDefaultAsync();

        if (dog != null)
        {
            dog.Mother = dog.Mother_Id.HasValue ? await GetDogWithAncestorsAsync(dog.Mother_Id.Value, maxDepth, currentDepth + 1) : null;
            dog.Father = dog.Father_Id.HasValue ? await GetDogWithAncestorsAsync(dog.Father_Id.Value, maxDepth, currentDepth + 1) : null;
        }
        _cache.Set(id, dog, TimeSpan.FromMinutes(10));
        return dog;
    }

    public async Task<DogSortInfo> GetDogWithChildren(int id)
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
        return dogWithChildren;
    }

    public async Task<DogSortInfo> GetDogWithPhoto(int id)
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
        return dogWithPhoto;
    }

    public async Task<DogSortInfo> GetDogWithUser(int id)
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

        return dogWithOwner;
    }

    public async Task<DogTree> GetUserWithDogs(int id)
    {
        var dogWithParents = await _context.Dogs
                .Where(d => d.Id == id)
                .Select(dog => new DogTree
                {
                    Id = dog.Id,
                    Name = dog.Name,
                    Photo = dog.Photo,
                    Mother_Id = dog.Mother_Id,
                    Father_Id = dog.Father_Id,
                    Mother = _context.Dogs
                        .Where(m => m.Id == dog.Mother_Id)
                        .Select(m => new DogTree
                        {
                            Id = m.Id,
                            Name = m.Name,
                            Photo = m.Photo
                        })
                        .FirstOrDefault(),
                    Father = _context.Dogs
                        .Where(f => f.Id == dog.Father_Id)
                        .Select(f => new DogTree
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Photo = f.Photo
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();
        return dogWithParents;
    }
}
