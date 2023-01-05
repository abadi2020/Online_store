using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Casestudy.Models;
using Microsoft.EntityFrameworkCore;

namespace Casestudy.Controllers
{
    public class DataController : Controller
    {
         AppDbContext _ctx;
        public async Task<IActionResult> Index()
        {
            DataUtility util = new DataUtility(_ctx);
            string msg = "";
            var json =  GetProductsFromJson();
            try
            {
                msg = (util.loadProductsToDb(json)) ? "tables loaded" : "problem loading tables";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            ViewData["msg"] = msg;
            return View();
        }

        private string GetProductsFromJson()
        {
            string file = "wwwroot/Products.json";
            StreamReader r = new StreamReader(file);

            var result = r.ReadToEnd();

            return result;
        }

        public DataController(AppDbContext context)
        {
            _ctx = context;
        }


    }
}