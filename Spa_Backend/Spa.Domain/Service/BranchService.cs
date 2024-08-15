using Microsoft.AspNetCore.Identity;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class BranchService:IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<List<Branch>> GetAllBranches()
        {
            var branches = await _branchRepository.GetAllBranches();
            return branches;
        }

        public async Task<Branch> GetBranchByID(long? branchID)
        {
            var branch = await _branchRepository.GetBranchByID(branchID);
            return branch;
        }
        public async Task<string> GetBranchNameByID(long? branchID)
        {
            string branch = await _branchRepository.GetBranchNameByID(branchID);
            return branch;
        }

        public async Task<Branch> CreateBranch(Branch branchDTO)
        {
            var newBranch = await _branchRepository.CreateBranch(branchDTO);
            return newBranch;
        }
        public async Task UpdateBranch(Branch branchDTO)
        {
            await _branchRepository.UpdateBranch(branchDTO);
        }

        public async Task DeleteBranch(long? id)
        {
            await _branchRepository.DeleteBranch(id);
        }

    }
}
