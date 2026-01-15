using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Models.Enums;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Billing;

namespace RestaurantManagement.Backend.Services
{
    public class BillingService : IBillingService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IBillingRepository _billingRepo;
        private readonly ISettingsRepository _settingsRepo;

        public BillingService(IOrderRepository orderRepo, IBillingRepository billingRepo, ISettingsRepository settingsRepo)
        {
            _orderRepo = orderRepo;
            _billingRepo = billingRepo;
            _settingsRepo = settingsRepo;
        }

        //public async Task<BillResponseDto> GenerateBillAsync(int customerId, int waiterId)
        //{
        //    var order = await _orderRepo.GetLatestCompletedOrderByCustomerAsync(customerId);
        //    if (order == null)
        //        throw new Exception("No completed order found for this customer.");

        //    var existingBill = await _billingRepo.FindAsync(b => b.OrderId == order.Id);
        //    if (existingBill.Any())
        //        throw new Exception("Bill already generated for this order.");

        //    var settings = await _settingsRepo.GetSettingsAsync();
        //    if (settings == null)
        //        throw new Exception("Admin settings not configured.");

        //    var subTotal = order.Items.Sum(i => (i.UnitPrice*i.Quantity));

        //    var discountPercent = settings.DiscountPercent;
        //    var taxPercent = settings.TaxPercent;

        //    var discountAmount = (subTotal * discountPercent) / 100m;
        //    var taxable = subTotal - discountAmount;
        //    var taxAmount = (taxable * taxPercent) / 100m;
        //    var grandTotal = taxable + taxAmount;

        //    var bill = new Bill
        //    {
        //        CustomerId = customerId,
        //        OrderId = order.Id,
        //        GeneratedByWaiterId = waiterId,
        //        SubTotal = subTotal,
        //        DiscountPercent = discountPercent,
        //        DiscountAmount = discountAmount,
        //        TaxPercent = taxPercent,
        //        TaxAmount = taxAmount,
        //        GrandTotal = grandTotal,
        //        IsPaymentDone = false,
        //        GeneratedAt = DateTime.UtcNow
        //    };

        //    await _billingRepo.AddAsync(bill);
        //    await _billingRepo.SaveChangesAsync();

        //    return MapBillToDto(bill);
        //}

        public async Task<BillResponseDto> GenerateBillByOrderIdAsync(int orderId, int waiterId)
        {
            var order = await _orderRepo.GetOrderWithItemsAsync(orderId)
                        ?? throw new Exception("Order not found.");

            if (order.Status != OrderStatus.Completed)
                throw new Exception("Only completed orders can be billed.");

            // prevent duplicate bill for same order
            var existing = await _billingRepo.FindAsync(b => b.OrderId == order.Id);
            if (existing.Any())
                throw new Exception("Bill already generated for this order.");

            var settings = await _settingsRepo.GetSettingsAsync()
                           ?? throw new Exception("Admin settings not configured.");

            var subTotal = order.Items.Sum(i => (i.Quantity * i.UnitPrice));

            var discountPercent = settings.DiscountPercent;
            var taxPercent = settings.TaxPercent;

            var discountAmount = (subTotal * discountPercent) / 100m;
            var taxable = subTotal - discountAmount;
            var taxAmount = (taxable * taxPercent) / 100m;
            var grandTotal = taxable + taxAmount;

            var bill = new Bill
            {
                CustomerId = order.CustomerId,
                OrderId = order.Id,
                GeneratedByWaiterId = waiterId,
                SubTotal = subTotal,
                DiscountPercent = discountPercent,
                DiscountAmount = discountAmount,
                TaxPercent = taxPercent,
                TaxAmount = taxAmount,
                GrandTotal = grandTotal,
                IsPaymentDone = false,
                GeneratedAt = DateTime.UtcNow
            };

            await _billingRepo.AddAsync(bill);
            await _billingRepo.SaveChangesAsync();

            return MapBillToDto(bill);
        }


        public async Task MarkPaymentDoneAsync(int billId, int waiterId)
        {
            var bill = await _billingRepo.GetByIdAsync(billId)
                       ?? throw new Exception("Bill not found.");

            if (bill.GeneratedByWaiterId != waiterId)
                throw new Exception("You can update payment only for your own bill.");

            bill.IsPaymentDone = true;
            _billingRepo.Update(bill);
            await _billingRepo.SaveChangesAsync();
        }

        public async Task<BillResponseDto> GetBillByIdAsync(int billId, int? waiterId, bool isAdmin)
        {
            var bill = await _billingRepo.GetBillDetailsAsync(billId)
                       ?? throw new Exception("Bill not found.");

            if (!isAdmin && waiterId.HasValue && bill.GeneratedByWaiterId != waiterId.Value)
                throw new Exception("You can only view bills generated by you.");

            return MapBillToDto(bill);
        }

        public async Task<List<BillResponseDto>> GetBillsByCustomerIdAsync(int customerId, int? waiterId, bool isAdmin)
        {
            var bills = await _billingRepo.GetBillsByCustomerIdAsync(customerId);

            if (!isAdmin && waiterId.HasValue)
                bills = bills.Where(b => b.GeneratedByWaiterId == waiterId.Value).ToList();

            return bills.Select(MapBillToDto).ToList();
        }

        public async Task<List<BillResponseDto>> GetAllBillsAsync()
        {
            var bills = await _billingRepo.GetAllBillsAsync();
            return bills.Select(MapBillToDto).ToList();
        }

        private static BillResponseDto MapBillToDto(Bill bill)
        {
            return new BillResponseDto
            {
                BillId = bill.Id,
                CustomerId = bill.CustomerId,
                OrderId = bill.OrderId,
                SubTotal = bill.SubTotal,
                DiscountPercent = bill.DiscountPercent,
                DiscountAmount = bill.DiscountAmount,
                TaxPercent = bill.TaxPercent,
                TaxAmount = bill.TaxAmount,
                GrandTotal = bill.GrandTotal,
                IsPaymentDone = bill.IsPaymentDone,
                GeneratedAt = bill.GeneratedAt
            };
        }
    }

}