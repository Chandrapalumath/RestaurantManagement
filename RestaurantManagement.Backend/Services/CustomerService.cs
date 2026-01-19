using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Customers;

namespace RestaurantManagement.Backend.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IOrderRepository _orderRepo;
        public CustomerService(ICustomerRepository customerRepo, IOrderRepository orderRepo)
        {
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
        }

        public async Task<CustomerResponseDto> CreateAsync(CustomerCreateRequestDto dto, Guid TableId)
        {
            var existing = await _customerRepo.GetByMobileAsync(dto.MobileNumber);
            if (existing != null)
            {
                await MapCustomerWithTable(TableId, existing.Id);
                return new CustomerResponseDto
                {
                    Id = existing.Id,
                    Name = existing.Name,
                    MobileNumber = existing.MobileNumber,
                    CreatedAt = existing.CreatedAt
                };
            }

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                MobileNumber = dto.MobileNumber.Trim()
            };
            await _customerRepo.AddAsync(customer);
            await _customerRepo.SaveChangesAsync();
            await MapCustomerWithTable(TableId, customer.Id);
            return new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                MobileNumber = customer.MobileNumber,
                CreatedAt = customer.CreatedAt
            };
        }

        public async Task<CustomerResponseDto> GetByIdAsync(Guid id)
        {
            var customer = await _customerRepo.GetByIdAsync(id)
                           ?? throw new NotFoundException("Customer not found.");

            return new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                MobileNumber = customer.MobileNumber,
                CreatedAt = customer.CreatedAt
            };
        }

        public async Task<List<CustomerResponseDto>> GetAllAsync()
        {
            var list = await _customerRepo.GetAllAsync();
            return list.Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                MobileNumber = c.MobileNumber,
                CreatedAt = c.CreatedAt
            }).ToList();
        }
        
        private async Task MapCustomerWithTable(Guid TableId, Guid customerId)
        {
            var orders = await _orderRepo.GetNotBilledOrders(TableId);
            foreach (var order in orders)
            {
                order.CustomerId = customerId;
                _orderRepo.Update(order);
            }
            await _orderRepo.SaveChangesAsync();
        }
    }
}