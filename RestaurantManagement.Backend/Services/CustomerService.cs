using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Customers;
using System.Collections.Generic;

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
            var customers = await _customerRepo.GetByMobileAsync(dto.MobileNumber);
            if (customers != null)
            {
                var existing = customers.FirstOrDefault();
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
        
        public async Task<List<CustomerResponseDto>> GetByMobileNumberAsync(string mobile)
        {
            var customers = await _customerRepo.GetByMobileAsync(mobile);
            if (!customers.Any()) throw new NotFoundException("No user found with this mobile number");
            return customers.Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                MobileNumber = c.MobileNumber,
                CreatedAt = c.CreatedAt
            }).ToList();
        }
    }
}