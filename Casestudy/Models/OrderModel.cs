using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
namespace Casestudy.Models
{
    public class OrderModel
    {
        private AppDbContext _db;
        public OrderModel(AppDbContext ctx)
        {
            _db = ctx;
        }
        public int AddOrder(Dictionary<string, object> items, string user, ref string message)
        {
            int orderId = -1;
            using (_db)
            {
                // we need a transaction as multiple entities involved
                using (var _trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        Order order = new Order();
                        order.UserId = user;
                        order.OrderDate = System.DateTime.Now;

                        _db.Orders.Add(order);
                        _db.SaveChanges();

                        decimal Total = 0;
                        const decimal TAX = 0.13M;



                        // then add each item to the trayitems table
                        foreach (var key in items.Keys)
                        {
                            ProductViewModel product = JsonConvert.DeserializeObject<ProductViewModel>(Convert.ToString(items[key]));
                            if (product.Qty > 0)
                            {

                                Total += product.Qty * product.MSRB;


                                OrderLineItem olItem = new OrderLineItem();
                                olItem.ProductId = product.Id;
                                olItem.OrderId = order.Id;
                                olItem.SellingPrice = product.MSRB;

                                ProductModel pM = new ProductModel(_db);
                                Product newP = pM.GetById(product.Id);

                                if (product.Qty <= product.QtyOnHanad)
                                {


                                    newP.QtyOnHanad -= product.Qty;
                                    olItem.QtySold = product.Qty;
                                    olItem.QtyOrdered = product.Qty;
                                    olItem.QtyBackOrdered = 0;


                                }
                                else
                                {

                                    newP.QtyOnBackOrder += product.Qty - product.QtyOnHanad;
                                    newP.QtyOnHanad = 0;
                                    olItem.QtyOrdered = product.Qty;
                                    olItem.QtySold = product.QtyOnHanad;
                                    olItem.QtyBackOrdered = product.Qty - product.QtyOnHanad;
                                    message = " Some goods were backordered!";
                                }


                                order.OrderAmount = (TAX * Total) + Total;
                                _db.OrderLineItems.Add(olItem);
                                _db.SaveChanges();
                            }
                        }
                        // test trans by uncommenting out these 3 lines
                        //int x = 1;
                        //int y = 0;
                        //x = x / y;
                        _trans.Commit();
                        orderId = order.Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _trans.Rollback();
                    }
                }
            }
            return orderId;
        }

        public List<Order> GetAll(string userId)
        {
            List<Order> allOrders = _db.Orders.Where(order => order.UserId == userId).ToList<Order>();
            return allOrders;
        }

        public List<OrderViewModel> GetOrderDetails(int oId, string uId)
        {
            List<OrderViewModel> allDetails = new List<OrderViewModel>();
            // LINQ way of doing INNER JOINS
            List< OrderLineItem> orderItems = _db.OrderLineItems.Where(order => order.OrderId == oId).ToList<OrderLineItem>();
            decimal sub = 0;
            foreach (OrderLineItem item in orderItems)
            {
                sub += item.SellingPrice*item.QtyOrdered;
            }

            var results = from o in _db.Set<Order>()
                          join oli in _db.Set<OrderLineItem>() on o.Id equals oli.OrderId
                          join pr in _db.Set<Product>() on oli.ProductId equals pr.Id
                          where (o.UserId == uId && o.Id == oId)
                          select new OrderViewModel
                          {
                              // user and product details
                              UserId = uId,
                              ProductName = pr.ProductName,

                              // order line item details
                              QtyOrdered = oli.QtyOrdered,
                              QtySold = oli.QtySold,
                              QtyBackOrdered = oli.QtyBackOrdered,
                              SellingPrice = string.Format("{0:C2}", oli.SellingPrice),
                              ProductId = oli.ProductId,
                              Extended = string.Format("{0:C2}", oli.QtyOrdered * oli.SellingPrice),

                              // order details
                              SubTotal = string.Format("{0:C2}", sub),
                              Tax = string.Format("{0:C2}", sub * (decimal)0.13),
                              OrderId = o.Id,
                              OrderAmount = string.Format("{0:C2}", o.OrderAmount),
                              OrderDate = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")

                          };
            allDetails = results.ToList<OrderViewModel>();
            return allDetails;
        }


    }      
}