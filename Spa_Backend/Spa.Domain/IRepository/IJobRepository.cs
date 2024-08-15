using Spa.Domain.Entities;

namespace Spa.Domain.IRepository
{
    public interface IJobRepository
    {
        Task<List<JobType>> GetAllJobs();
        Task<List<JobType>> GetAllJobForPermissions();
        Task<JobType> GetJobTypeByID(long? jobTypeID);
        Task<JobType> CreateJobType(JobType jobDTO);
        Task<bool> UpdateJob(JobType jobDTO);
        Task<bool> DeleteJob(long? id);
    }
}
