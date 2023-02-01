using MarketPhone.WEB.Data;
using MarketPhone.WEB.Models;
using MarketPhone.WEB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MarketPhone.WEB.Controllers
{
    public class HomeController : Controller
    {
        private PhoneDBContext _dbMarket;
        public HomeController(PhoneDBContext dbMarket)
        {
            _dbMarket = dbMarket;
        }

        public IActionResult Index()
        {
            return View(_dbMarket.Phones.ToList());
        }

        [HttpGet]
        public IActionResult MakeOrder(int? id)
        {
            if (id == null)
                return NotFound();
            Phone phone = _dbMarket.Phones.Find(id);
            if (phone == null)
                return NotFound();

            OrderViewModel orderModel = new OrderViewModel { PhoneId = phone.Id };
            return View(orderModel);
        }

        [HttpPost]
        public IActionResult MakeOrder(OrderViewModel orderModel)
        {
            if (ModelState.IsValid)
            {
                Phone phone = _dbMarket.Phones.Find(orderModel.PhoneId);
                if (phone == null)
                    return NotFound();

                decimal sum = phone.Price;

                Order order = new Order
                {
                    PhoneId = phone.Id,
                    PhoneNumber = orderModel.PhoneNumber,
                    Address = orderModel.Address,
                    Date = DateTime.Now,
                    Sum = sum
                };
                _dbMarket.Orders.Add(order);
                _dbMarket.SaveChanges();
                return Content("<h2>Ваш заказ успешно оформлен</h2>");
            }
            return View(orderModel);
        }

        protected override void Dispose(bool disposing)
        {
            _dbMarket.Dispose();
            base.Dispose(disposing);
        }

    }
}