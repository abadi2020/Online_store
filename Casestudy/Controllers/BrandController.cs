
using Microsoft.AspNetCore.Mvc;
using Casestudy.Models;
using System.Collections.Generic;
using Casestudy.Utils;
using System;

namespace Casestudy.Controllers
{
    public class BrandController : Controller
    {
        AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index(BrandViewModel vm)
        {
            // only build the catalogue once
            if (HttpContext.Session.Get<List<Brand>>(SessionVars.Brands) == null)
            {
                // no session information so let's go to the database
                try
                {
                    BrandModel catModel = new BrandModel(_db);
                    // now load the Brands
                    List<Brand> Brands = catModel.GetAll();
                    HttpContext.Session.Set<List<Brand>>(SessionVars.Brands, Brands);
                    vm.SetBrands(Brands);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Catalogue Problem - " + ex.Message;
                }
            }
            else
            {
                // no need to go back to the database as information is already in the session
                vm.SetBrands(HttpContext.Session.Get<List<Brand>>(SessionVars.Brands));
                ProductModel productModel = new ProductModel(_db);
                if(vm.Id != null)
                vm.Products = productModel.GetAllByBrand(int.Parse(vm.Id));
            }
            return View(vm);
        }

        public IActionResult SelectBrand(BrandViewModel vm)
        {
            BrandModel branModel = new BrandModel(_db);
            ProductModel proModel = new ProductModel(_db);
            List<Product> products = proModel.GetAllByBrand(vm.BrandId);
            List<ProductViewModel> vms = new List<ProductViewModel>();
            if (products.Count > 0)
            {
                foreach (Product product in products)
                {
                    ProductViewModel pvm = new ProductViewModel();
                    pvm.Qty = 0;
                    pvm.BrandId = product.BrandId;
                    pvm.BrandName = branModel.GetName(product.BrandId);
                    pvm.ProductName = product.ProductName;
                    pvm.Description = product.Description;
                    pvm.Id = product.Id;
                    pvm.GraphicName = product.GraphicName;
                    pvm.CostPrice = product.CostPrice;
                    pvm.MSRB = product.MSRB;
                    pvm.QtyOnHanad = product.QtyOnHanad;
                    pvm.QtyOnBackOrder = product.QtyOnBackOrder;

                    vms.Add(pvm);
                }
                ProductViewModel[] myList = vms.ToArray();
                HttpContext.Session.Set<ProductViewModel[]>(SessionVars.productList, myList);
            }
            vm.SetBrands(HttpContext.Session.Get<List<Brand>>(SessionVars.Brands));
            return View("Index", vm); // need the original Index View here
        }

        [HttpPost]
        public ActionResult SelectProduct(BrandViewModel vm)
        {
            Dictionary<string, object> cart;
            if (HttpContext.Session.Get<Dictionary<string, Object>>(SessionVars.Order) == null)
            {
                cart = new Dictionary<string, object>();
            }
            else
            {
                cart = HttpContext.Session.Get<Dictionary<string, object>>(SessionVars.Order);
            }
            ProductViewModel[] productList = HttpContext.Session.Get<ProductViewModel[]>(SessionVars.productList);
            String retMsg = "";
           
            foreach (ProductViewModel product in productList)
            {
                if (product.Id == vm.Id )
                {
                    if (vm.Qty > 0) // update only selected product
                    {
                        product.Qty = vm.Qty;
                        retMsg = vm.Qty + " - product(s) Added!";
                        cart[product.Id] = product;
                    }
                    else
                    {
                        product.Qty = 0;
                        cart.Remove(product.Id);
                        retMsg = "product(s) Removed!";
                    }
                    vm.BrandId = product.BrandId;
                    break;
                }
            }

            ViewBag.AddMessage = retMsg;
            HttpContext.Session.Set<Dictionary<string, Object>>(SessionVars.Order, cart);
            vm.SetBrands(HttpContext.Session.Get<List<Brand>>(SessionVars.Brands));
            return PartialView("AddToOrderPartial");
        }

    }
}