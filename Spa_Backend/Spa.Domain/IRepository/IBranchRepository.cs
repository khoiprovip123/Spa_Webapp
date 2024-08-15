using Spa.Domain.Entities;

namespace Spa.Domain.IRepository
{
    public interface IBranchRepository
    {
        Task<List<Branch>> GetAllBranches();
        Task<Branch> GetBranchByID(long? branchID);
        Task<string> GetBranchNameByID(long? branchID);
        Task<Branch> CreateBranch(Branch branchDTO);
        Task<bool> UpdateBranch(Branch branchDTO);
        Task<bool> DeleteBranch(long? id);
    }
}
