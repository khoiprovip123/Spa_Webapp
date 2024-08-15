using Microsoft.Extensions.Configuration;
using Spa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Spa.Domain.IRepository;

namespace Spa.Infrastructure
{
    public class BranchRepository : IBranchRepository
    {
        private static readonly List<Branch> _branch = new();
        private readonly IConfiguration _config;
        private readonly SpaDbContext _spaDbContext;
        
        public BranchRepository(SpaDbContext spaDbContext, IConfiguration config)
        {
            _config = config;
            _spaDbContext = spaDbContext;
        }
        
        public async Task<List<Branch>> GetAllBranches()
        {
            var brans = await _spaDbContext.Branches.ToListAsync();
            if (brans is null)
            {
                return null;
            }

            var branDTOs = brans.Select(bran => new Branch
            {
                BranchID = bran.BranchID,
                BranchName = bran.BranchName,
                BranchPhone = bran.BranchPhone,
                BranchAddress = bran.BranchAddress,
            }).OrderBy(b => b.BranchID).ToList();

            return branDTOs;
        }
        
        public async Task<string> GetBranchNameByID(long? branchID)
        {
            var Branch = await _spaDbContext.Branches.FindAsync(branchID);
            return Branch.BranchName;
        }

        public async Task<Branch> GetBranchByID(long? branchID)
        {
            var Branch = await _spaDbContext.Branches.FindAsync(branchID);
            return Branch;
        }

            public async Task<Branch> CreateBranch(Branch branchDTO)
        {
            var newBranch = new Branch()
            {
                BranchName = branchDTO.BranchName,
                BranchAddress = branchDTO.BranchAddress,
                BranchPhone = branchDTO.BranchPhone,
            };
            await _spaDbContext.Branches.AddAsync(newBranch);
            await _spaDbContext.SaveChangesAsync();
            return newBranch;
        }

        public async Task<bool> UpdateBranch(Branch branchDTO)
        {
            var newUpdate = new Branch
            {
                BranchID= branchDTO.BranchID,
                BranchName = branchDTO.BranchName,
                BranchAddress= branchDTO.BranchAddress,
                BranchPhone = branchDTO.BranchPhone,
            };
            var branchUpdate = await _spaDbContext.Branches.FirstOrDefaultAsync(b => b.BranchID == newUpdate.BranchID);
            if (branchUpdate is null) return false;
            {
                branchUpdate.BranchName = newUpdate.BranchName;
                branchUpdate.BranchAddress = newUpdate.BranchAddress;
                branchUpdate.BranchPhone = newUpdate.BranchPhone;
            }
            _spaDbContext.Branches.Update(branchUpdate);
            _spaDbContext.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteBranch(long? BranchID)
        {
                var branch = await _spaDbContext.Branches.FirstOrDefaultAsync(a => a.BranchID == BranchID);
                if (branch is null)
                {
                    return false;
                }
                _spaDbContext.Branches.Remove(branch);
                _spaDbContext.SaveChanges();
                return true;
        }
    }
}
