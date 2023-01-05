using System;
using System.Collections.Generic;
using System.Linq;
namespace Casestudy.Models
{
    public class DataUtility
    {
        private AppDbContext _db;
        dynamic objectJson; // an element that is typed as dynamic is assumed to support any operation
        public DataUtility(AppDbContext context)
        {
            _db = context;
        }
        public bool loadProductsToDb(string stringJson)
        {
           
            bool brandsLoaded = false;
            bool productsLoaded = false;
            try
            {
                objectJson = Newtonsoft.Json.JsonConvert.DeserializeObject(stringJson);
                brandsLoaded = loadBrands();
                productsLoaded = loadProducts();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return brandsLoaded && productsLoaded;
        }
        private bool loadBrands()
        {
            bool loadedBrands = false;
            try
            {
                // clear out the old rows
                _db.Brands.RemoveRange(_db.Brands);
                _db.SaveChanges();
                List<String> allBrands = new List<String>();
                foreach (var node in objectJson)
                {
                    allBrands.Add(Convert.ToString(node["Brand"]));
                }
                // distinct will remove duplicates before we insert them into the db
                IEnumerable<String> Brands = allBrands.Distinct<String>();
                foreach (string braName in Brands)
                {
                    Brand bra = new Brand();
                    bra.Name = braName;
                    _db.Brands.Add(bra);
                    _db.SaveChanges();
                }
                loadedBrands = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedBrands;
        }
        private bool loadProducts()
        {
            bool loadedItems = false;
            try
            {
                List<Brand> Brands = _db.Brands.ToList();
                // clear out the old
                _db.Products.RemoveRange(_db.Products);
                _db.SaveChanges();
                foreach (var node in objectJson)
                {
                    Product item = new Product();
                    item.ProductName = Convert.ToString(node["ProductName"]);
                    item.GraphicName = Convert.ToString(node["GraphicName"]);
                    item.Id = Convert.ToString(node["Id"]);
                    item.CostPrice = Convert.ToDecimal(node["CostPrice"].Value);
                    item.MSRB = Convert.ToDecimal(node["MSRB"].Value);
                    item.QtyOnHanad = Convert.ToInt32(node["QtyOnHanad"].Value);
                    item.QtyOnBackOrder = Convert.ToInt32(node["QtyOnBackOrder"].Value);
                    item.Description = Convert.ToString(node["Description"]);
                    string bra = Convert.ToString(node["Brand"].Value);
                    // add the FK here
                    foreach (Brand brand in Brands)
                    {
                        if (brand.Name == bra)
                        {
                            item.Brand = brand;
                        }
                            
                    }
                    _db.Products.Add(item);
                    _db.SaveChanges();
                }
                loadedItems = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedItems;
        }
    }
}
