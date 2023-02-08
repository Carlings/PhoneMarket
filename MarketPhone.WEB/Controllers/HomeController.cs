using MarketPhone.DAL.Data;
using MarketPhone.DAL.Entities;
using MarketPhone.WEB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MarketPhone.BLL.Interfaces;
using AutoMapper;
using MarketPhone.BLL.DTO;
using System.ComponentModel.DataAnnotations;
using StackExchange.Redis;

namespace MarketPhone.WEB.Controllers
{
    public class HomeController : Controller
    {
        IOrderService orderService;
        public HomeController(IOrderService serv)
        {
            orderService = serv;
        }

        public IActionResult Index()
        {
            IEnumerable<PhoneDTO> phoneDtos = orderService.GetPhones();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhoneDTO, PhoneViewModel>()).CreateMapper();
            var phones = mapper.Map<IEnumerable<PhoneDTO>, List<PhoneViewModel>>(phoneDtos);
            return View(phones);
        }

        [HttpGet]
        public IActionResult MakeOrder(int? id)
        {
            try
            {
                PhoneDTO phone = orderService.GetPhone(id);
                var order = new OrderViewModel { PhoneId = phone.Id };

                return View(order);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult MakeOrder(OrderViewModel orderModel)
        {
            try
            {
                var orderDto = new OrderDTO { PhoneId = orderModel.PhoneId, Address = orderModel.Address, PhoneNumber = orderModel.PhoneNumber };
                orderService.MakeOrder(orderDto);
                return Content("<h2>Ваш заказ успешно оформлен</h2>");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Source, ex.Message);
            }
            return View(orderModel);
        }

        protected override void Dispose(bool disposing)
        {
            orderService.Dispose();
            base.Dispose(disposing);
        }

    }
}