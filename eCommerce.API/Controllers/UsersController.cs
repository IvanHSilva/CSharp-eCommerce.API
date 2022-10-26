using eCommerce.API.Models;
using eCommerce.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private IUserRepository _repository;

        public UsersController() {
            _repository = new UserRepository();
        }

        [HttpGet]
        public IActionResult Get() {
            return Ok(_repository.GetUsers()); //HTTP 200 (Ok) - 300 (Redir) - 400 (Error)
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            OkObjectResult user = Ok(_repository.GetUser(id));
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Insert([FromBody]User user) {
            _repository.InsertUser(user);
            return Ok(user);
        }

        [HttpPut]
        public IActionResult Update([FromBody]User user) {
            _repository.UpdateUser(user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            _repository.DeleteUser(id);
            return Ok();
        }
    }
}