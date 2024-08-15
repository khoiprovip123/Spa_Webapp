using Spa.Domain.Entities;

namespace Spa.Domain.IRepository
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllPermissions();
        Task<List<Permission>> GetAllPermissionByJobTypeID(long? jobTypeID);
        Task<List<string>> GetAllPermissionNameByJobTypeID(long? jobTypeID);
        Task<bool> GetRolePermissionByID(long? jobTypeID, long perID);
        Task<RolePermission> CreateRolePermission(RolePermission rolePerDTO);
        Task<bool> DeleteRolePermission(long? JobTypeID, long PermissionID);
        Task<Permission> GetPermissionByName(string per);
        Task<Permission> CreatePermission(Permission perDTO);
        Task<bool> UpdatePermission(Permission perDTO);
        Task<bool> DeletePermission(long perID);
        Task<Permission> GetPermissionByPermissionID(long perID);
    }
}
