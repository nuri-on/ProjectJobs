using ProjectJobs.Command;
using ProjectJobs.Config;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectJobs.Dao
{
    class JobsDao
    {
        public async Task<string> getJobs(JobsCommand jobsCommand)
        {
            string jobsContent = await this.GetJobsContent(jobsCommand);

            return jobsContent;
        }

        private async Task<string> GetJobsContent(JobsCommand jobsCommand)
        {
            string resourceUrl = JobsConfig.JOBS_RESOURCE_URL;

            if (jobsCommand.Keyword != null)
            {
                resourceUrl += "keyword=" + jobsCommand.Keyword + "&";
            }
            
            if(jobsCommand.jobCategoryCode > 0)
            {
                resourceUrl += "job_category=" + jobsCommand.jobCategoryCode + "&";
            }
            
            if (jobsCommand.Start > 0)
            {
                resourceUrl += "start=" + jobsCommand.Start + "&";
            }
            
            if (jobsCommand.Count > 0)
            {
                resourceUrl += "count=" + jobsCommand.Count + "&";
            }

            if ((jobsCommand.Order != null) && (jobsCommand.Order.Length > 0))
            { 
                resourceUrl += "sort=" + jobsCommand.Order + "&";
            }

            if (jobsCommand.Location > 0)
            {
                resourceUrl += "loc_cd=" + jobsCommand.Location + "&";
            }

            resourceUrl = resourceUrl.Substring(0, resourceUrl.Length - 1);

            HttpClient httpClient = new HttpClient();

            Task<string> getStringAsync = null;

            try
            {
                getStringAsync = httpClient.GetStringAsync(resourceUrl);
            }
            catch(Exception exception)
            {
                // TODO: 예외 처리하기!
            }

            return await getStringAsync;
        }
    }
}
