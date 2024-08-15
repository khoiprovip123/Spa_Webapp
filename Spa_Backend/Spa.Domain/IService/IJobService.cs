using Spa.Domain.Entities;

namespace Spa.Domain.IService
{
    public interface IJobService
    {
        Task<List<JobType>> GetAllJobs();
        Task<List<JobType>> GetAllJobForPermissions();
        Task<JobType> GetJobTypeByID(long? jobTypeID);
        Task<JobType> CreateJobType(JobType jobDTO);
        Task UpdateJob(JobType jobDTO);
        Task DeleteJob(long? id);
    }
}
