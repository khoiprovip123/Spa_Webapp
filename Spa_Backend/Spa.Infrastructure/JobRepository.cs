using Microsoft.Extensions.Configuration;
using Spa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Spa.Domain.IRepository;

namespace Spa.Infrastructure
{
    public class JobRepository : IJobRepository
    {
        private static readonly List<JobType> _job = new();
        private readonly IConfiguration _config;
        private readonly SpaDbContext _spaDbContext;
        
        public JobRepository(SpaDbContext spaDbContext, IConfiguration config)
        {
            _config = config;
            _spaDbContext = spaDbContext;
        }
        
        public async Task<List<JobType>> GetAllJobs()
        {
            var jobs = await _spaDbContext.JobTypes.ToListAsync();
            if (jobs is null)
            {
                return null;
            }

            var jobDTOs = jobs.Select(job => new JobType
            {
                JobTypeID = job.JobTypeID,
                JobTypeName = job.JobTypeName
            })  .Where(j => j.JobTypeName != "Admin")
                .OrderBy(j => j.JobTypeID).ToList();

            return jobDTOs;
        }

        public async Task<List<JobType>> GetAllJobForPermissions()
        {
            var jobs = await _spaDbContext.JobTypes.ToListAsync();
            if (jobs is null)
            {
                return null;
            }

            var jobDTOs = jobs.Select(job => new JobType
            {
                JobTypeID = job.JobTypeID,
                JobTypeName = job.JobTypeName
            }).OrderByDescending(j => j.JobTypeID).ToList();

            return jobDTOs;
        }

        public async Task<string> GetJobTypeNameByID(long? JobTypeId)
        {
            var Role = await _spaDbContext.JobTypes.FindAsync(JobTypeId);
            return Role.JobTypeName;
        }
        public async Task<JobType> GetJobTypeByID(long? JobTypeId)
        {
            var job = await _spaDbContext.JobTypes.FindAsync(JobTypeId);
            return job;
        }
        public async Task<JobType> CreateJobType(JobType jobDTO)
        {
            var newJob = new JobType()
            {
                JobTypeName = jobDTO.JobTypeName
            };
            await _spaDbContext.JobTypes.AddAsync(newJob);
            await _spaDbContext.SaveChangesAsync();
            return newJob;
        }

        public async Task<bool> UpdateJob(JobType jobDTO)
        {
            var newUpdate = new JobType
            {
                JobTypeID = jobDTO.JobTypeID,
                JobTypeName = jobDTO.JobTypeName,
            };
            var jobUpdate = await _spaDbContext.JobTypes.FirstOrDefaultAsync(b => b.JobTypeID == newUpdate.JobTypeID);
            if (jobUpdate is null) return false;
            {
                jobUpdate.JobTypeName = newUpdate.JobTypeName;
            }
            _spaDbContext.JobTypes.Update(jobUpdate);
            _spaDbContext.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteJob(long? JobTypeID)
        {
            var job = await _spaDbContext.JobTypes.FirstOrDefaultAsync(a => a.JobTypeID == JobTypeID);
            if (job is null)
            {
                return false;
            }
            _spaDbContext.JobTypes.Remove(job);
            _spaDbContext.SaveChanges();
            return true;
        }
    }
}
