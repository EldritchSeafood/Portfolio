using System;
using CaseStudyAPI.DAL;
using CaseStudyAPI.DAL.DAO;
using CaseStudyAPI.DAL.DomainClasses;
using CaseStudyAPI.APIHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CaseStudyAPI.Helpers;

namespace CaseStudyAPI.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase {
        AppDbContext _ctx;
        public OrderController(AppDbContext context) {  // injected here
            _ctx = context;
        }
        [HttpPost]
        [Produces("application/json")]
        public ActionResult<string> Index(OrderHelper helper) {
            string retVal = "";
            try {
                CustomerDAO cDao = new CustomerDAO(_ctx);
                Customer orderOwner = cDao.GetByEmail(helper.email);
                OrderDAO oDao = new OrderDAO(_ctx);
                int orderId = oDao.AddOrder(orderOwner.Id, helper.selections);
                if (orderId > 0) {
                    retVal = "Order " + orderId + " Created! Goods Backordered!";
                }
                else {
                    retVal = "Order not saved";
                }
            }
            catch (Exception ex) {
                retVal = "Order not saved " + ex.Message;
            }
            return retVal;
        }

        [HttpGet("{email}")]
        public ActionResult<List<Order>> List(string email) {
            List<Order> orders = new List<Order>();
            CustomerDAO oDao = new CustomerDAO(_ctx);
            Customer orderOwner = oDao.GetByEmail(email);
            OrderDAO tDao = new OrderDAO(_ctx);
            orders = tDao.GetAll(orderOwner.Id);
            return orders;
        }

        [HttpGet("{orderid}/{email}")]
        public ActionResult<List<OrderDetailsHelper>> GetOrderDetails(int orderid, string email) {
            OrderDAO dao = new OrderDAO(_ctx);
            return dao.GetOrderDetails(orderid, email);
        }


    }
}
