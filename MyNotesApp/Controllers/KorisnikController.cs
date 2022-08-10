using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyNotesApp.Data;
using MyNotesApp.Services.KorisnikService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private static List<Korisnik> prodavnicas = new List<Korisnik>
        { };
        private static Prodavnica prod = new Prodavnica();
        private readonly IKorisnikService _korisnikService;
        public KorisnikController(DataContext context, IConfiguration configuration, IKorisnikService korisnikService)
        {
            _korisnikService = korisnikService;
            _context = context;
            _configuration = configuration;

        }

        [HttpGet]
        public ActionResult GetKorisnici()
        {
            return Ok(_context.Korisnici.ToList());
        }
        [HttpGet("me"),Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _korisnikService.GetSomeName();
            var serNumber = User.FindFirstValue(ClaimTypes.SerialNumber);
            return Ok(new { userName, serNumber });

        }
        [HttpPost("register")]
        public String SetKorisnik([FromBody] Korisnik korisnik)
        {
            var Korisnici = _context.Korisnici.SingleOrDefault(x => x.KorisnikIme == korisnik.KorisnikIme);

            if (Korisnici != null)
            {
                return "Korisnik sa ovim imenom vec postoji!";
            }

            String hashedPassword = BCrypt.Net.BCrypt.HashPassword(korisnik.KorisnikSifra);
            korisnik.KorisnikSifra = hashedPassword;
           
            _context.Korisnici.Add(korisnik);

            _context.SaveChanges();
            return "Uspesan register!";
        }
        private string CreateToken(Korisnik korisnik)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, korisnik.KorisnikIme),
                new Claim(ClaimTypes.SerialNumber,korisnik.KorisnikKartica.ToString())

                 
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
        public string GetKorisnikPoImenu(String KorisnikIme, String korSifra)
        {


            var JedanKorisnik = _context.Korisnici.Where(x => x.KorisnikIme.Equals(KorisnikIme)).AsEnumerable().ToList();

            if (JedanKorisnik.Count == 0)
            {
                return "Ne postoji korisnik sa ovim imenom!";
            }

            if (BCrypt.Net.BCrypt.Verify(korSifra, JedanKorisnik[0].KorisnikSifra))
            {
                HttpContext.Session.SetString("user:", "The Doctor");
                String token = CreateToken(JedanKorisnik[0]);

               

                _context.Korisnici.Update(JedanKorisnik[0]);
                _context.SaveChanges();

                return token;


            }
            else
            {
                return "Neispravna Sifra!";
            }




        }
    }
}
