using CaseStudyAPI.APIHelpers;
using CaseStudyAPI.Helpers;
using CaseStudyAPI.DAL.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseStudyAPI.DAL.DAO {
    public class OrderDAO {
        private AppDbContext _db;
        public OrderDAO(AppDbContext ctx) {
            _db = ctx;
        }
        public int AddOrder(int userid, OrderSelectionHelper[] selections) {
            int orderId = -1;
            using (_db) {
                // we need a transaction as multiple entities involved
                using (var _trans = _db.Database.BeginTransaction()) {
                    try {
                        Order order = new Order();
                        order.UserId = userid;
                        order.OrderDate = System.DateTime.Now;
                        order.OrderAmount = 0;

                        ProductDAO pDao = new ProductDAO(_db);
                        Product product = new Product();

                        // calculate total cost and add it to the order, then lower quantitys in product table and add backorders if necessary
                        foreach (OrderSelectionHelper selection in selections) {
                            order.OrderAmount += (selection.item.MSRP + (selection.item.MSRP * 13/100)) * selection.Qty;
                        }
                        _db.Orders.Add(order);
                        _db.SaveChanges();
                        // then add each item to the orderlineitem table
                        foreach (OrderSelectionHelper selection in selections) {
                            OrderLineItem oLItem = new OrderLineItem();
                            oLItem.QtyOrdered = selection.Qty;
                            oLItem.ProductId = selection.item.Id;
                            oLItem.OrderId = order.Id;
                            oLItem.SellingPrice = selection.item.MSRP;

                            // modify the product stock while filling out QtySold and QtyBackordered
                            product = pDao.GetById(selection.item.Id);
                            if (product.QuantityOnHand < selection.Qty) {
                                oLItem.QtySold = product.QuantityOnHand;
                                product.QuantityOnBackOrder = product.QuantityOnBackOrder + selection.Qty - product.QuantityOnHand;
                                oLItem.QtyBackOrdered = selection.Qty - product.QuantityOnHand;
                                product.QuantityOnHand = 0;
                            } else {
                                product.QuantityOnHand -= selection.Qty;
                                oLItem.QtySold = selection.Qty;
                                oLItem.QtyBackOrdered = 0;
                            }

                            _db.Product.Update(product);
                            _db.OrderLineItems.Add(oLItem);
                            _db.SaveChanges();
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
        } // end AddOrder

        public List<Order> GetAll(int id) {
            return _db.Orders.Where(order => order.UserId == id).ToList<Order>();
        }

        public List<OrderDetailsHelper> GetOrderDetails(int oid, string email) {
            Customer customer = _db.Customers.FirstOrDefault(customer => customer.Email == email);
            List<OrderDetailsHelper> allDetails = new List<OrderDetailsHelper>();
            // LINQ way of doing INNER JOINS
            var results = from o in _db.Orders
                          join oli in _db.OrderLineItems on o.Id equals oli.OrderId
                          join p in _db.Product on oli.ProductId equals p.Id
                          where (o.UserId == customer.Id && o.Id == oid)
                          select new OrderDetailsHelper {
                              UserId = o.UserId,
                              OrderId = o.Id,
                              OrderDate = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt"),
                              ProductName = p.ProductName,
                              MSRP = p.MSRP,
                              ExtPrice = p.MSRP * oli.QtyOrdered,
                              QtyOrdered = oli.QtyOrdered,
                              QtySold = oli.QtySold,
                              QtyBackOrdered = oli.QtyBackOrdered,
                              OrderAmount = o.OrderAmount
                          };
            allDetails = results.ToList<OrderDetailsHelper>();
            return allDetails;
        }

    }
}