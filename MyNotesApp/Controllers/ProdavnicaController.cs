using Microsoft.AspNetCore.Mvc;
using MyNotesApp.Data;

namespace MyNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdavnicaController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private static List<Prodavnica> prodavnicas = new List<Prodavnica>
        { };
        private static Prodavnica prod = new Prodavnica();
        public ProdavnicaController(DataContext context, IConfiguration configuration)
        {

            _context = context;
            _configuration = configuration;
           
        }
        [HttpGet]
        public ActionResult GetProizvodi()
        {
          
            return Ok(_context.Prodavnice.ToList());
        }
        [HttpPost]
        public ActionResult SetProizvodi([FromBody] Prodavnica prodavnica)
        {
            _context.Prodavnice.Add(prodavnica);

            _context.SaveChanges();
            return Ok();

        }
        [HttpGet("byID")]
        public IEnumerable<Prodavnica> GetByID([FromQuery] int[] idProd)
        {
            
            var SviProizvodi =new List<Prodavnica>();
            
            foreach (var item in idProd)
            {
                var Jedan = _context.Prodavnice.FirstOrDefault(a => a.id == item);
                if(Jedan != null)
                {
                    SviProizvodi.Add(Jedan);
                }
            }

            return SviProizvodi;

        }

        [HttpPut("kupovina")]
        public string GetChangeProducts([FromQuery] int[] idProd, [FromQuery] int[] kolicina)
        {
            int brojac = 0;
            foreach (var item in idProd)
            {
                var Jedan = _context.Prodavnice.FirstOrDefault(a => a.id == item);
                Jedan.ProductQuantity -= kolicina[brojac];
                brojac++;
                _context.Prodavnice.Update(Jedan);
                _context.SaveChanges();
            }
            return "Uspesno izmenjeni proizvodi!";
        }
    }
}
