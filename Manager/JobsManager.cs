using ProjectJobs.Command;
using ProjectJobs.Dao;
using ProjectJobs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectJobs.Manager
{
    class JobsManager
    {
        public async Task<List<Job>> getJobs(JobsCommand jobsCommand)
        {
            string getJobsData = await this.GetJobsData(jobsCommand);

            List<Job> jobs = new List<Job>();

            try
            {
                XDocument jobsXDocument = XDocument.Parse(getJobsData);

                var jobsData = from data in jobsXDocument.Descendants("job") select data;

                foreach (XElement jobXElement in jobsData)
                {
                    Job job = new Job();

                    job.Id = (int)jobXElement.Element("id");
                    job.Url = (string)jobXElement.Element("url");
                    job.CompanyName = (string)jobXElement.Element("company").Element("name");
                    job.Active = (int)jobXElement.Element("active");
                    job.PostingTimestamp = (int)jobXElement.Element("posting-timestamp");
                    job.ModificationTimestamp = (int)jobXElement.Element("modification-timestamp");
                    job.OpeningTimestamp = (int)jobXElement.Element("opening-timestamp");
                    job.ExpirationTimestamp = (int)jobXElement.Element("expiration-timestamp");
                    job.Title = (string)jobXElement.Element("position").Element("title");
                    job.Location = (string)jobXElement.Element("position").Element("location");
                    job.JobType = (string)jobXElement.Element("position").Element("job-type");
                    job.Industry = (string)jobXElement.Element("position").Element("industry");
                    job.JobCategory = (string)jobXElement.Element("position").Element("job-category");
                    job.OpenQuantity = (int)jobXElement.Element("position").Element("open-quantity");
                    job.ExperienceLevel = (string)jobXElement.Element("position").Element("experience-level");
                    job.RequiredEducationLevel = (string)jobXElement.Element("position").Element("required-education-level");
                    job.Keyword = (string)jobXElement.Element("keyword");
                    job.Salary = (string)jobXElement.Element("salary");

                    jobs.Add(job);
                }
            }

            catch (Exception exception)
            {
                // TODO: 예외 처리하기!
            }

            return jobs;
        }

        private async Task<string> GetJobsData(JobsCommand jobsCommand)
        {
            JobsDao jobsDao = new JobsDao();

            Task<string> getJobsContent = null;

            try
            {
                getJobsContent = jobsDao.getJobs(jobsCommand);
            }
            catch (Exception exception)
            {
                // TODO: 예외 처리하기!
            }

            return await getJobsContent;
        }
    }
}
