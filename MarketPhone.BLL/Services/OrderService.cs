using AutoMapper;
using MarketPhone.BLL.BusinessModels;
using MarketPhone.BLL.DTO;
using MarketPhone.BLL.Interfaces;
using MarketPhone.DAL.Data;
using MarketPhone.DAL.Entities;
using MarketPhone.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPhone.BLL.Services
{
    internal class OrderService : IOrderService
    {
        private IRepository<Phone> phoneRepository;
        private IRepository<Order> orderRepository;
        public OrderService(IRepository<Phone> phoneRepository, IRepository<Order> orderRepository)
        {
            this.phoneRepository = phoneRepository;
            this.orderRepository = orderRepository;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PhoneDTO GetPhone(int? id)
        {
            if (id == null)
                throw new ValidationException("Не установлено id телефона");
            var phone = phoneRepository.Get(id.Value);
            if (phone == null)
                throw new ValidationException("Телефон не найден");

            return new PhoneDTO { Company = phone.Company, Id = phone.Id, Name = phone.Name, Price = phone.Price };
        }

        public IEnumerable<PhoneDTO> GetPhones()
        {
            // применяем автомаппер для проекции одной коллекции на другую
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Phone, PhoneDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Phone>, List<PhoneDTO>>(phoneRepository.GetAll());
        }

        public void MakeOrder(OrderDTO orderDto)
        {
            Phone phone = phoneRepository.Get(orderDto.PhoneId);

            if (phone == null)
                throw new ValidationException("Телефон не найден");

            decimal sum = new Discount(0.1m).GetDiscountedPrice(phone.Price);
            Order order = new Order
            {
                Date = DateTime.Now,
                Address = orderDto.Address,
                PhoneId = phone.Id,
                Sum = sum,
                PhoneNumber = orderDto.PhoneNumber
            };
            orderRepository.Create(order);
        }
    }
}
