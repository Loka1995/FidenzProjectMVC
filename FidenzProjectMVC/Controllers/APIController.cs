using FidenzProjectMVC.Common.Interfaces;
using FidenzProjectMVC.Models;
using FidenzProjectMVC.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FidenzProjectMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public APIController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateUser(string id, [FromBody] EditUserDto editUserDto)
        {
            if (editUserDto == null || id != editUserDto.Id)
            {
                return BadRequest();
            }

            var user = _unitOfWork.User.GetUserByIdAsync(id).Result;

            user.Name = editUserDto.Name == null || editUserDto.Name == "" || editUserDto.Name == "string" ? user.Name : editUserDto.Name;
            user.Email = editUserDto.Email == null || editUserDto.Email == "" || editUserDto.Email == "string" ? user.Email : editUserDto.Email;
            user.Phone = editUserDto.Phone == null || editUserDto.Phone == "" || editUserDto.Phone == "string" ? user.Phone : editUserDto.Phone;

            _unitOfWork.User.UpdateUser(user);
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("{id}", Name = "GetDistance")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDistance(string id, double Latitude, double Longitude)
        {
            if (id == null || Latitude == null || Longitude == null)
            {
                return BadRequest();
            }

            var distance = await _unitOfWork.User.CalculateDistanceAsync(id, Latitude, Longitude);
            return Ok(distance + "Km");
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<User>>> Search(string word)
        {
            var result = await _unitOfWork.User.SearchUsersAsync(word);
            if (result.Any())
            {
                return Ok(result);
            }
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("groupedbyzipcode")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UsersByZipCodeDto>>> GetUsersGroupedByZipCode()
        {
            var result = await _unitOfWork.User.GetUsersGroupedByZipCodeAsync();
            if (result.Any())
            {
                return Ok(result);
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid login request");
            }
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                // User authenticated successfully
                var roles = await _userManager.GetRolesAsync(user);

                // Use the JwtTokenGenerator to create a JWT token
                var token = _jwtTokenGenerator.GenerateJwtToken(user.UserName, roles.FirstOrDefault());

                return Ok(token);
            }

            return Unauthorized("Invalid email or password");
        }
    }
}
