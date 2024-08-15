using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
namespace Spa.Domain.Service
{
    public class JobService:IJobService
    {
        private readonly IJobRepository _jobRepository;
        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<List<JobType>> GetAllJobs()
        {
            var jobs = await _jobRepository.GetAllJobs();
            return jobs;
        }
        public async Task<List<JobType>> GetAllJobForPermissions()
        {
            var jobs = await _jobRepository.GetAllJobForPermissions();
            return jobs;
        }

        public async Task<JobType> GetJobTypeByID(long? jobTypeID)
        {
            var job = await _jobRepository.GetJobTypeByID(jobTypeID);
            return job;
        }

        public async Task<JobType> CreateJobType(JobType jobDTO)
        {
            var newJob = await _jobRepository.CreateJobType(jobDTO);
            return newJob;
        }
        public async Task UpdateJob(JobType jobDTO)
        {
            await _jobRepository.UpdateJob(jobDTO);
        }

        public async Task DeleteJob(long? id)
        {
            await _jobRepository.DeleteJob(id);
        }
    }
}
