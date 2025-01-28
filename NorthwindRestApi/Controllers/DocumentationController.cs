using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        NorthwindOriginalContext db = new NorthwindOriginalContext();
        private const string ValidKeyCode = "12345"; // Kovakoodattu avainkoodi

        [HttpGet("{keycode}")]
        public ActionResult GetDocumentation(string keycode)
        {
            try
            {
                // Tarkistetaan avainkoodi
                if (keycode != ValidKeyCode)
                {
                    return BadRequest(new
                    {
                        Date = DateTime.Now,
                        Message = "Wrong keycode!"
                    });
                }

                // Haetaan dokumentaatio tietokannasta
                var documentation = db.Documentations.ToList();

                if (documentation == null || !documentation.Any())
                {
                    return NotFound(new
                    {
                        Date = DateTime.Now,
                        Message = "Documentation empty"
                    });
                }

                return Ok(documentation);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
