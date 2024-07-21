//using Microsoft.EntityFrameworkCore;
//using Station.Data;
//using Station.Models;

//public class BillRepository : IBillRepository
//{
//    private readonly ApplicationDbContext _dbContext;

//    public BillRepository(ApplicationDbContext dbContext)
//    {
//        _dbContext = dbContext;
//    }

//    public async Task<Bill> GetBillAsync(int id)
//    {
//        return await _dbContext.Bills
//            .Include(b => b.Subscriber)
//            .Include(b => b.Meter.Readings)
//            .FirstOrDefaultAsync(b => b.BillID == id);
//    }

//    public async Task<IEnumerable<Bill>> GetAllBillsForSubscriberAsync(int subscriberId)
//    {
//        return await _dbContext.Bills
//            .Where(b => b.SubscriberID == subscriberId)
//            .Include(b => b.Subscriber)
//            .Include(b => b.Meter.Readings)
//            .ToListAsync();
//    }

//    public async Task<Bill> CreateBillAsync(Bill bill)
//    {
//        await _dbContext.Bills.AddAsync(bill);
//        await _dbContext.SaveChangesAsync();
//        return bill;
//    }

//    public async Task UpdateBillAsync(Bill? bill)
//    {
//        _dbContext.Bills.Update(bill);
//        await _dbContext.SaveChangesAsync();
//    }

//    public async Task DeleteBillAsync(int id)
//    {
//        var bill = await GetBillAsync(id);
//        _dbContext.Bills.Remove(bill);
//        await _dbContext.SaveChangesAsync();
//    }
//}