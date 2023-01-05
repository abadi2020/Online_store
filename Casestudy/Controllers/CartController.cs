using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using Casestudy.Utils;
using Casestudy.Models;
using System.Collections.Generic;

namespace Casestudy.Controllers
{
    public class CartController : Controller
    {
        AppDbContext _db;
        public CartController(AppDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ClearCart() // clear out current cart
        {
            HttpContext.Session.Remove("cart");
            HttpContext.Session.Set<String>("Message", "Cart Cleared");
            return Redirect("/Home");
        }

        public ActionResult AddOrder()
        {
            OrderModel model = new OrderModel(_db);
            int retVal = -1;
            string retMessage = "";
            string message = "";
            try
            {
                Dictionary<string, object> trayItems = HttpContext.Session.Get<Dictionary<string, object>>(SessionVars.Order);

                retVal = model.AddOrder(trayItems, HttpContext.Session.GetString(SessionVars.User), ref message);
                if (retVal > 0) // Tray Added
                {
                    retMessage = "Order " + retVal + " Placed!" + message;
                }
                else // problem
                {
                    retMessage = "Order not added, try again later";
                }
            }
            catch (Exception ex) // big problem
            {
                retMessage = "Order was not created, try again later! - " + ex.Message;
            }
            HttpContext.Session.Remove(SessionVars.Order); // clear out current tray once persisted
            HttpContext.Session.SetString(SessionVars.Message, retMessage);
            return Redirect("/Home");
        }

        public IActionResult List()
        {
            // they can't list orders if they're not logged on
            if (HttpContext.Session.GetString(SessionVars.User) == null)
            {
                return Redirect("/Login");
            }
            return View("Orders");
        }

        [Route("[action]")]
        public IActionResult getOrders()
        {
            OrderModel model = new OrderModel(_db);
            return Ok(model.GetAll(HttpContext.Session.GetString(SessionVars.User)));
        }

        [Route("[action]/{oid:int}")]
        public IActionResult GetOrderDetails(int oid)
        {
            OrderModel model = new OrderModel(_db);
            return Ok(model.GetOrderDetails(oid, HttpContext.Session.GetString(SessionVars.User)));
        }


    }
}
    
