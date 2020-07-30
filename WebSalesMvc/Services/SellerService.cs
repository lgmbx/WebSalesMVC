using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services.Exceptions;

namespace WebSalesMvc.Services {
    public class SellerService {

        private readonly AppDbContext _context;

        public SellerService(AppDbContext context) {
            _context = context;
        }

        //Find all
        public async Task<List<Seller>> FindAllAsync() {
            return await _context.Seller.Include(x => x.Department).ToListAsync();
        }

        //Insert
        public async Task InsertAsync(Seller obj) {
            _context.Seller.Add(obj);
            await _context.SaveChangesAsync();
        }

        //Find by Id
        public async Task<Seller> FindByIdAsync(int? id) {
            return await _context.Seller.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
        }

        //Remove
        public async Task RemoveAsync(int id) {
            try {
                var obj = _context.Seller.Find(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                throw new IntegrityException("Seller with linked sale cannot be removed");
            }
           
        }

        //Update
        public async Task UpdateAsync(Seller obj) {
            if (!_context.Seller.Any(x => x.Id == obj.Id)) {
                throw new NotFoundException("Not Found");
            }
            try {
                _context.Seller.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e) {
                throw new DbConcurrencyException(e.Message);
            }
           
        }
    }
}
