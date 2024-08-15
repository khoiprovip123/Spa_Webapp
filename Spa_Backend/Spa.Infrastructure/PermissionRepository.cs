using Microsoft.Extensions.Configuration;
using Spa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Spa.Domain.IRepository;

namespace Spa.Infrastructure
{
    public class PermissionRepository : IPermissionRepository
    {
        private static readonly List<Permission> _per = new();
        private static readonly List<RolePermission> _rolePer = new();

        private readonly IConfiguration _config;
        private readonly SpaDbContext _spaDbContext;

        public PermissionRepository(SpaDbContext spaDbContext, IConfiguration config)
        {
            _config = config;
            _spaDbContext = spaDbContext;
        }

        public async Task<List<Permission>> GetAllPermissions()
        {
            var pers = await _spaDbContext.Permissions.ToListAsync();
            if (pers is null)
            {
                return null;
            }
            var perDTOs = pers.Select(per => new Permission
            {
                PermissionID = per.PermissionID,
                PermissionName = per.PermissionName
            }).OrderBy(j => j.PermissionID).ToList();
            return perDTOs;
        }
        public async Task<List<RolePermission>> GetAllRolePermissions()
        {
            var rolePers = await _spaDbContext.RolePermissions.ToListAsync();
            if (rolePers is null)
            {
                return null;
            }
            var rolePerDTOs = rolePers.Select(per => new RolePermission
            {
                PermissionID = per.PermissionID,
                JobTypeID = per.JobTypeID
            }).OrderBy(j => j.JobTypeID).ToList();
            return rolePerDTOs;
        }

        public async Task<bool>GetRolePermissionByID(long? jobTypeID, long perID)
        {
            var rolePernission = await _spaDbContext.RolePermissions
                .FirstOrDefaultAsync(rp => rp.JobTypeID == jobTypeID && rp.PermissionID == perID);
            if (rolePernission is null)
                return false;
            return true;
        }

        public async Task<RolePermission> CreateRolePermission(RolePermission rolePerDTO)
        {
            var newRolePer = new RolePermission()
            {
                JobTypeID = rolePerDTO.JobTypeID,
                PermissionID = rolePerDTO.PermissionID,
            };
            await _spaDbContext.RolePermissions.AddAsync(newRolePer);
            await _spaDbContext.SaveChangesAsync();
            return newRolePer;
        }

        public async Task<bool> DeleteRolePermission(long? JobTypeID, long PermissionID)
        {
            var rolePernission = await _spaDbContext.RolePermissions
                .FirstOrDefaultAsync(rp => rp.JobTypeID == JobTypeID && rp.PermissionID == PermissionID);
            if (rolePernission is null)
            {
                return false;
            }
            _spaDbContext.RolePermissions.Remove(rolePernission);
            _spaDbContext.SaveChanges();
            return true;
        }

        public async Task<List<string>> GetAllPermissionNameByJobTypeID(long? jobTypeID)
        {
            var permissionNames = await _spaDbContext.Permissions
                .Where(p => _spaDbContext.RolePermissions.Any(rp => rp.JobTypeID == jobTypeID && rp.PermissionID == p.PermissionID))
                .Select(p => p.PermissionName)
                .ToListAsync();
            return permissionNames;
        }
        public async Task<List<Permission>> GetAllPermissionByJobTypeID(long? jobTypeID)
        {
            {
                var permissions = await _spaDbContext.Permissions
                    .Where(p => _spaDbContext.RolePermissions.Any(rp => rp.JobTypeID == jobTypeID && rp.PermissionID == p.PermissionID))
                    .ToListAsync();
                return permissions;
            }
        }
        public async Task<Permission> GetPermissionByName(string per)
        {
            var Permission = await _spaDbContext.Permissions.FirstOrDefaultAsync(p => p.PermissionName == per);
            return Permission;
        }

        public async Task<Permission> GetPermissionByPermissionID(long perID)
        {
            var Permission = await _spaDbContext.Permissions.FirstOrDefaultAsync(p => p.PermissionID == perID);
            return Permission;
        }

        public async Task<Permission> CreatePermission(Permission perDTO)
        {
            var newPer = new Permission()
            {
                PermissionName = perDTO.PermissionName,
            };
            await _spaDbContext.Permissions.AddAsync(newPer);
            await _spaDbContext.SaveChangesAsync();
            return newPer;
        }

        public async Task<bool> UpdatePermission(Permission perDTO)
        {
            var newUpdate = new Permission
            {
                PermissionName = perDTO.PermissionName,
            };
            var perUpdate = await _spaDbContext.Permissions.FirstOrDefaultAsync(b => b.PermissionID == perDTO.PermissionID);
            if (perUpdate is null) return false;
            {
                perUpdate.PermissionName = newUpdate.PermissionName;
            }
            _spaDbContext.Permissions.Update(perUpdate);
            _spaDbContext.SaveChanges();
            return true;
        }

        public async Task<bool> DeletePermission(long perID)
        {
            var rolePermissionsToDelete = await _spaDbContext.RolePermissions.Where(rp => rp.PermissionID == perID).ToListAsync();
            _spaDbContext.RolePermissions.RemoveRange(rolePermissionsToDelete);
            await _spaDbContext.SaveChangesAsync();
            // Xóa Permission
            var permission = await _spaDbContext.Permissions.FirstOrDefaultAsync(p => p.PermissionID == perID);
            if (permission is null)
            {
                return false;
            }
            _spaDbContext.Permissions.Remove(permission);
            await _spaDbContext.SaveChangesAsync();
            return true;
        }
    }
}
