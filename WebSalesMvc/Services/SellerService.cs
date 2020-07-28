using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Data;
using WebSalesMvc.Models;

namespace WebSalesMvc.Services {
    public class SellerService {

        private readonly AppDbContext _context;

        public SellerService(AppDbContext context) {
            _context = context;
        }

        //Find all
        public async Task<List<Seller>> FindAllAsync() {
            return await _context.Seller.ToListAsync();
        }

        //Insert
        public async Task Insert(Seller obj) {
            _context.Seller.Add(obj);
            await _context.SaveChangesAsync();
        }

        //Find by Id
        public async Task<Seller> FindById(int? id) {
            return await _context.Seller.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
        }

        //Remove
        public async Task Remove(int id) {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            await _context.SaveChangesAsync();
        }

        //Update
        public async Task Update(Seller obj) {
            _context.Seller.Update(obj);
            await _context.SaveChangesAsync();
        }
    }
}
