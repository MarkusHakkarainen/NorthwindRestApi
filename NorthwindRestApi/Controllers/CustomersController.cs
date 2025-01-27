using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using System.Linq.Expressions;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        NorthwindOriginalContext db = new NorthwindOriginalContext();

        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            try
            {
                var asiakkaat = db.Customers.ToList();
                return Ok(asiakkaat);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe. Lue lisää:" + ex.InnerException);
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetOneCustomersById(String id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);
                if (asiakas != null)
                {
                    return Ok(asiakas);
                }
                else
                {
                    //return BadRequest("Asiakasta id:llä " + id + "ei löydy");
                    return NotFound($"Asiakasta id:llä {id} ei löydy");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + ex);
            }
        }

        [HttpPost]
        public ActionResult AddNew([FromBody] Customer cust)
        {
            try 
            {
                db.Customers.Add(cust);
                db.SaveChanges();
                return Ok($"Lisättiin uusi asiakas {cust.CompanyName} from {cust.City}");
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe.Lue lisää: " + ex.InnerException);
            }
        }

    }
}
