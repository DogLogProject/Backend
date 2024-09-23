using ApiProject.Context;
using ApiProject.GenericRepository;
using ApiProject.Model.DogModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogAlbumController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IGenericRepository<DogPhoto> _genericRepository;

        public DogAlbumController(ApiDbContext context)
        {
            _context = context;
            _genericRepository = new GenericRepository<DogPhoto>(_context);
        }

        // GET: api/DogPhotos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DogPhoto>>> GetDogPhotos()
        {
            var dogPhotos = await _genericRepository.GetAll();
            return Ok(dogPhotos);
        }

        // GET: api/DogPhotos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DogPhoto>> GetDogPhoto(int id)
        {
            var dogPhoto = await _genericRepository.GetById(id);
            if (dogPhoto == null)
            {
                return NotFound();
            }
            return Ok(dogPhoto);
        }

        // PUT: api/DogPhotos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDogPhoto(int id, DogPhoto dogPhoto)
        {
            if (id != dogPhoto.Id)
            {
                return BadRequest();
            }

            if (!DogPhotoExists(id))
            {
                return NotFound();
            }

            try
            {
                _genericRepository.Update(dogPhoto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/DogPhotos
        [HttpPost]
        public async Task<ActionResult<DogPhoto>> PostDogPhoto(DogPhoto dogPhoto)
        {
            await _genericRepository.Insert(dogPhoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDogPhoto), new { id = dogPhoto.Id }, dogPhoto);
        }

        // DELETE: api/DogPhotos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDogPhoto(int id)
        {
            var dogPhoto = await _genericRepository.GetById(id);
            if (dogPhoto == null)
            {
                return NotFound();
            }

            await _genericRepository.Delete(id);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DogPhotoExists(int id)
        {
            return _context.DogPhotos.Any(e => e.Id == id);
        }
    }
}
