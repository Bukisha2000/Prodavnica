using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNotesApp.Data;
using BCrypt.Net;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace MyNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private static User user = new User();
        private static List<User> users = new List<User>
        { };



        public UserController(DataContext context, IConfiguration configuration, IUserService userService)
        {

            _context = context;
            _configuration = configuration;
            _userService = userService;
        }
        [HttpGet("me"), Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            var role = User.FindFirstValue(ClaimTypes.Role);
            return Ok(new { userName, role });

        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_context.Users.ToList());
        }

        [HttpPost("register")]
        public String SetUser([FromBody] User user)
        {
            var SingleUser = _context.Users.SingleOrDefault(x => x.UserName == user.UserName);

            if (SingleUser != null)
            {
                return "Korisnik sa ovim imenom vec postoji!";
            }

            String hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPassword);
            user.UserPassword = hashedPassword;
            user.Role = "Worker";
            _context.Users.Add(user);

            _context.SaveChanges();
            return "Uspesan register!";
        }
        private string CreateToken(User user)
        {
          
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),

                 new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        [HttpGet("login")]
        public string GetUserByName(String name, String passFrontend)
        {


            var SingleUser = _context.Users.Where(User => User.UserName.Equals(name)).AsEnumerable().ToList();

            if (SingleUser.Count == 0)
            {
                return "Ne postoji korisnik sa ovim imenom!";
            }

            if (BCrypt.Net.BCrypt.Verify(passFrontend, SingleUser[0].UserPassword))
            {
                HttpContext.Session.SetString("user:", "The Doctor");
                String token = CreateToken(SingleUser[0]);

                 var refreshToken = GenerateRefreshToken();

                SetRefreshToken(refreshToken, SingleUser[0]);

                _context.Users.Update(SingleUser[0]);

                _context.SaveChanges(); 

                return token;
                
                
            }
            else
            {
                return "Neispravna Sifra!";
            }




        }
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddMinutes(30),
                Created = DateTime.Now
            };
            return refreshToken;
        }
        private void SetRefreshToken(RefreshToken newRefreshToken,User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            _context.Users.Update(user);

            _context.SaveChanges();

        }
        [HttpPost("refresh-Token")]
        public ActionResult<string> RefreshToken()
        {
            
            var refreshToken = Request.Cookies["refreshToken"];
            var SingleUser = _context.Users.Where(User => User.RefreshToken.Equals(refreshToken)).AsEnumerable().ToList();
            if (SingleUser.Count == 0)
            {
                return Unauthorized("Invalid refresh token!");
            }
            else if (SingleUser[0].TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired..");
            }
           
            string token = CreateToken(SingleUser[0]);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, SingleUser[0]);

            _context.Users.Update(SingleUser[0]);

            _context.SaveChanges();
            return Ok(token);

        } 

        [HttpGet("see"), Authorize(Roles = "Admin")]
        public String GetSee()
        {
            return "Uspeo si";
        }
    }
}
