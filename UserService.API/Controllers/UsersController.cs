using Infrastructure.Library.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Library.Models;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetUsers()
        {
            var patients = _context.Users.ToList();
            return Ok(patients);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var patient = _context.Users.Find(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        // POST: api/users
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email ;

            _context.SaveChanges();
            return Ok(user);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult UserPatient(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
