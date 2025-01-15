
using HumanTalentApp.Areas.Identity.Models;
using HumanTalentApp.Data;
using EntityModelsHumanTalentApp.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Policy;

namespace HumanTalentApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles ="ADMN")]
    public class UserController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AppDbContext _context; 
        private string? passwordEncrypted;

        public UserController(AppDbContext context, IConfiguration configuration) 
        { 
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

            passwordEncrypted = Utilities.EncryptPassword(model.Password);
            var user = await _context.AdmnUsers.FirstOrDefaultAsync(u => u.UserName == model.UserName && u.Password == passwordEncrypted);
            if (user == null)
            {
                throw new Exception("Usuario Invalido");
            }
            var getRole = await _context.TAdmnRoles.FirstOrDefaultAsync(x => x.IdeRole == user.IdeRole);
            if (user == null)
            {
                return Unauthorized();
            }
            var token = GenerateJwtToken(user, getRole.Code);
            return Ok(new { Token = token });
        }
        private string GenerateJwtToken(AdmnUser user, string role)
        {
            var userClaims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Sid, user.IdeUser.ToString()),
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        public IActionResult GetUser(string email)
        {
            var result = _context.AdmnUsers.Where(x => x.Email == email);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetUserById(Guid id)
        {
            var result = _context.AdmnUsers.Where(x => x.IdeUser == id);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var result = from u in _context.AdmnUsers
                         join r in _context.TAdmnRoles on u.IdeRole equals r.IdeRole
                         select new
                         {
                             dni = u.Dni,
                             firstName = u.FirstName,
                             lastName = u.LastName,
                             userName = u.UserName,
                             email = u.Email,
                             role = r.Name,
                             ideUser= u.IdeUser
                         };
            return Ok(result);
        }

        [HttpDelete("{UserId?}")]
        public IActionResult DeleteUser(Guid UserId)
        {
            try
            {
                var users = _context.AdmnUsers.Where(x => x.IdeUser == UserId).FirstOrDefault();
                if (users == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Registro no existe",
                        result = ""
                    });
                }

                _context.AdmnUsers.Remove(users);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro eliminado",
                    Data = users
                });
            }
            catch (Exception e)
            {
                return StatusCode(400, new
                {
                    error = e.Message
                });
            }
        }

        [HttpPut]
        public IActionResult UpdateUsers([FromBody] AdmnUser optData)
        {
            try
            {
                var data = optData ?? throw new Exception("The value is null");

                var users = _context.AdmnUsers.Where(x => x.IdeUser == optData.IdeUser).FirstOrDefault();
                if (users == null)
                {
                    return StatusCode(400, new
                    {
                        message = "Registro no existe",
                        result = ""
                    });
                }
                users.Dni = optData.Dni;
                users.Email = optData.Email;
                users.FirstName = optData.FirstName;
                users.IdeRole = optData.IdeRole;
                users.IdeUser = optData.IdeUser;
                users.UserName = optData.UserName;
                users.LastName = optData.LastName;
                if (!optData.Password.IsNullOrEmpty())
                {
                    users.Password = Utilities.EncryptPassword(optData.Password);
                }
                _context.AdmnUsers.Update(users);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro actualizado",
                    Data = optData
                });
            }
            catch (Exception e)
            {
                return StatusCode(400, new
                {
                    error = e.Message
                });
            }
        }
        [HttpPost]
        public IActionResult Register([FromBody] AdmnUser optData)
        {
            try
            {
                var data = optData ?? throw new Exception("The value is null");
                var passwordHash = Utilities.EncryptPassword(optData.Password);
                var getUserName = optData.Email.Split("@");
                optData.Password = passwordHash;
                optData.UserName = getUserName[0];
                _context.AdmnUsers.Add(optData);
                _context.SaveChanges();
                return StatusCode(200, new
                {
                    Message = "Registro creado",
                    Data = optData
                });
            }
            catch (Exception e)
            {
                return StatusCode(400, new
                {
                    error = e.Message
                });
            }
        }
        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Identity/Authentication/Login");
        }

    }
}