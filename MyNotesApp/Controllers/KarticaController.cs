using Microsoft.AspNetCore.Mvc;
using MyNotesApp.Data;

namespace MyNotesApp.Controllers
{
    public class KarticaController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
      
        private static List<Kartica> kartice = new List<Kartica>
        { };



        public KarticaController(DataContext context, IConfiguration configuration, IUserService userService)
        {

            _context = context;
            _configuration = configuration;
            _userService = userService;
        }
        [HttpPut("Dodavanje")]
        public string AzurirajKarticu(int brojKartice, int iznos)
        {
            var JednaKartica = _context.SveKartice.SingleOrDefault(x => x.BrojKartice == brojKartice);
            if(JednaKartica != null)
            {
                JednaKartica.UkupnoStanje += iznos;

                _context.SveKartice.Update(JednaKartica);

                _context.SaveChanges();
                return "Uspesno dopunjen iznos!";
            }
            else
            {
                return "Broj ove kartice ne postoji!";
            }
            
        }
        [HttpPut("Skidanje")]
        public string IzvrsiKupovinu(int brojKartice, int iznos)
        {
            var JednaKartica = _context.SveKartice.SingleOrDefault(x => x.BrojKartice == brojKartice);
            if (JednaKartica != null)
            {
                if(JednaKartica.UkupnoStanje >= iznos)
                {
                    JednaKartica.UkupnoStanje -= iznos;

                    JednaKartica.UkupnoTransakcija += 1;
                    _context.SveKartice.Update(JednaKartica);

                    _context.SaveChanges();
                    return "Uspesna kupovina!";
                }
                else
                {
                    return "Nedovoljno para na kartici!";
                }
               
            }
            else 
            {
                return "Broj ove kartice ne postoji!";
            }
        }
    }
}
