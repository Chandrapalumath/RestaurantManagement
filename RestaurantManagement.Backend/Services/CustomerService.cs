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
        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<CustomerResponseDto> CreateAsync(CustomerCreateRequestDto dto)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                MobileNumber = dto.MobileNumber.Trim(),
                CreatedAt = DateTime.UtcNow
            };
            await _customerRepo.AddAsync(customer);
            await _customerRepo.SaveChangesAsync();
            return new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                MobileNumber = customer.MobileNumber
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
                MobileNumber = customer.MobileNumber
            };
        }

        public async Task<List<CustomerResponseDto>> GetAllAsync()
        {
            var list = await _customerRepo.GetAllAsync();
            return list.Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                MobileNumber = c.MobileNumber
            }).ToList();
        }
        
        public async Task<List<CustomerResponseDto>> GetByMobileNumberAsync(string mobile)
        {
            var customers = await _customerRepo.GetByMobileAsync(mobile.Trim());
            //if (!customers.Any()) throw new NotFoundException("No user found with this mobile number");
            return customers.Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                MobileNumber = c.MobileNumber
            }).ToList();
        }
    }
}