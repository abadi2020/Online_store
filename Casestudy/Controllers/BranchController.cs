using Microsoft.AspNetCore.Mvc;
using Casestudy.Models;
using Casestudy.Utils; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Casestudy.Controllers
{
    public class BranchController : Controller
    {
        AppDbContext _db;
        public BranchController(AppDbContext context)
        {
            _db = context;
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionVars.Message) != null)
            {
                ViewBag.Message = HttpContext.Session.GetString(SessionVars.Message);
            }
            return View();
        }
        [AllowAnonymous]
        [Route("[action]/{lat:double}/{lng:double}")]
        public IActionResult GetBranches(float lat, float lng)
        {
            BranchModel model = new BranchModel(_db);
            return Ok(model.GetThreeClosestBranches(lat, lng));
        }
    }
}