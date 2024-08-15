using Spa.Domain.Entities;

namespace Spa.Domain.IService
{
    public interface IBranchService
    {
        Task<List<Branch>> GetAllBranches();
        Task<Branch>GetBranchByID(long? branchID);
        Task<string> GetBranchNameByID(long? branchID);
        Task<Branch> CreateBranch(Branch branchDTO);
        Task UpdateBranch(Branch branchDTO);
        Task DeleteBranch(long? id);
    }
}
