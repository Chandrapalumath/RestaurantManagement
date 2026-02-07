using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Customers;
using RestaurantManagement.Dtos.Pagination;

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

        public async Task<PagedResult<CustomerResponseDto>> GetAllAsync(int page, int pageSize, string? search)
        {
            var list = await _customerRepo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
                list = list.Where(c => c.Name.ToLower().Contains(search.ToLower())).ToList();

            var total = list.Count;

            var paged = list
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    MobileNumber = c.MobileNumber
                }).ToList();

            return new PagedResult<CustomerResponseDto>
            {
                Items = paged,
                TotalCount = total
            };
        }


        public async Task<List<CustomerResponseDto>> GetByMobileNumberAsync(string mobile)
        {
            var customers = await _customerRepo.GetByMobileAsync(mobile.Trim());
            return customers.Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                MobileNumber = c.MobileNumber
            }).ToList();
        }
    }
}