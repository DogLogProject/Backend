using ApiProject.Context;
using ApiProject.GenericRepository;
using ApiProject.Model;
using ApiProject.Model.DogModel;
using ApiProject.Model.UserModel;
using ApiProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private IGenericRepository<User> _genericRepository = null;

        public UsersController(IUserService userService, ApiDbContext context, IMapper mapper)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
            this._genericRepository = new GenericRepository<User>(_context);
        }

      
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var cars = await _genericRepository.GetAll();

            return Ok(cars);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var car = await _genericRepository.GetById(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }


        // PUT: api/Cars/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO dogDTO)
        {
            var car = await _genericRepository.GetById(id);

            if (id != car.Id)
            {
                return BadRequest();
            }
            _mapper.Map(dogDTO, car);

            await _genericRepository.Update(car);


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Cars
        [HttpPost]
        public async Task<ActionResult<DogDTO>> PostUser(UserDTO dogDTO)
        {
            var driver = _mapper.Map<User>(dogDTO);

            await _genericRepository.Insert(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = driver.Id }, _mapper.Map<UserDTO>(driver));
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {

            _genericRepository.Delete(id);

            await _context.SaveChangesAsync();

            return NoContent();
        }

      
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
