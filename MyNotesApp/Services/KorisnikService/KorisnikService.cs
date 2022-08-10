using System.Security.Claims;

namespace MyNotesApp.Services.KorisnikService
{
    public class KorisnikService : IKorisnikService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KorisnikService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetSomeName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            }
            return result;
        }
    }
}
