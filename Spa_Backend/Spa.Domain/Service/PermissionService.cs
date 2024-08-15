using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
namespace Spa.Domain.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _perRepository;
        private readonly IJobRepository _jobRepository;
        public PermissionService(IPermissionRepository perRepository, IJobRepository jobRepository)
        {
            _perRepository = perRepository;
            _jobRepository = jobRepository;
        }

        public async Task<List<Permission>> GetAllPermissions()
        {
            var jobs = await _perRepository.GetAllPermissions();
            return jobs;
        }
        public async Task<List<string>> GetAllPermissionNameByJobTypeID(long? jobTypeID)
        {
            var permissionNames= await _perRepository.GetAllPermissionNameByJobTypeID(jobTypeID);
            return permissionNames;
        }
        public async Task<List<Permission>> GetAllPermissionByJobTypeID(long? jobTypeID)
        {
            var permissions = await _perRepository.GetAllPermissionByJobTypeID(jobTypeID);
            return permissions;
        }

        public async Task<bool> GetRolePermissionByID(long? jobTypeID, long perID)
        {
            var per = await _perRepository.GetRolePermissionByID(jobTypeID, perID);
            return per;
        }

        public async Task<RolePermission> CreateRolePermission(RolePermission rolePerDTO)
        {
            var checkJobID = await _jobRepository.GetJobTypeByID(rolePerDTO.JobTypeID);
            if (checkJobID is null)
                return null;
            var checkPer = await _perRepository.GetPermissionByPermissionID(rolePerDTO.PermissionID);
            if (checkPer is null)
                return null;
            var rolePer = await _perRepository.GetRolePermissionByID(rolePerDTO.JobTypeID, rolePerDTO.PermissionID);
            if (rolePer is true)
                throw new Exception("Exist");
            var newRolePer =  await _perRepository.CreateRolePermission(rolePerDTO);
            return newRolePer;
        }

        public async Task<bool> DeleteRolePermission(long? JobTypeID, long PermissionID)
        {
            bool deleteRolePer = await _perRepository.DeleteRolePermission(JobTypeID,PermissionID);
            return deleteRolePer;
        }

        public async Task<Permission> GetPermissionByName(string per)
        {
            var permission = await _perRepository.GetPermissionByName(per);
            if(permission is null)
                throw new Exception("Not Exist");
            return permission;
        }

        public async Task<Permission> GetPermissionByPermissionID(long perID)
        {
            var permission = await _perRepository.GetPermissionByPermissionID(perID);
            if (permission is null)
                throw new Exception("Not Exist");
            return permission;
        }

        public async Task<Permission> CreatePermission(Permission perDTO)
        {
            var oldPer = await _perRepository.GetPermissionByName(perDTO.PermissionName);
            if (oldPer is not null)
                return null;
            var newPer = await _perRepository.CreatePermission(perDTO);
            return newPer;
        }

        public async Task<bool> UpdatePermission(Permission perDTO)
        {
            bool per = await _perRepository.UpdatePermission(perDTO);
            return per;
        }

        public async Task<bool> DeletePermission(long perID)
        {
            bool per = await _perRepository.DeletePermission(perID);
            return per;
        }
    }
}
