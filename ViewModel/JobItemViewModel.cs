using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectJobs.Model;
using ProjectJobs.Util;
using System.Runtime.Serialization;

namespace ProjectJobs.viewModel
{
    public class JobItemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 취업 
        /// </summary>
        private int _id;
        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
                NotifyPropertyChanged("Id");
            }
        }

        /// <summary>
        /// 취업 
        /// </summary>
        private int _groupCode;
        public int GroupCode
        {
            get
            {
                return this._groupCode;
            }
            set
            {
                this._groupCode = value;
                NotifyPropertyChanged("GroupCode");
            }
        }

        private string _url;
        public string Url
        {
            get
            {
                return this._url;
            }
            set
            {
                this._url = value;
                NotifyPropertyChanged("Url");
            }
        }

        private string _companyName;
        public string CompanyName
        {
            get
            {
                return this._companyName;
            }
            set
            {
                this._companyName = value;
                NotifyPropertyChanged("CompanyName");
            }
        }

        private int _active;
        public int Active
        {
            get
            {
                return this._active;
            }
            set
            {
                this._active = value;
                NotifyPropertyChanged("Active");
            }
        }

        private int _postingTimestamp;
        public int PostingTimestamp
        {
            get
            {
                return this._postingTimestamp;
            }
            set
            {
                this._postingTimestamp = value;
                NotifyPropertyChanged("PostingTimestamp");
            }
        }

        private int _modificationTimestamp;
        public int ModificationTimestamp
        {
            get
            {
                return this._modificationTimestamp;
            }
            set
            {
                this._modificationTimestamp = value;
                NotifyPropertyChanged("ModificationTimestamp");
            }
        }

        private int _openingTimestamp;
        public int OpeningTimestamp
        {
            get
            {
                return this._openingTimestamp;
            }
            set
            {
                this._openingTimestamp = value;
                NotifyPropertyChanged("OpeningTimestamp");
            }
        }

        private string _expirationDateTime;
        public string ExpirationDateTime
        {
            get
            {
                return this._expirationDateTime;
            }
        }

        private double _expirationTimestamp;
        public double ExpirationTimestamp
        {
            get
            {
                return this._expirationTimestamp;
            }
            set
            {
                this._expirationTimestamp = value;
                this._expirationDateTime = "마감일: ";

                if (this.ExpirationTimestamp == 1988118000)
                {
                    this._expirationDateTime += "채용시";
                }
                else
                {
                    string timeFormat = "yyyy-MMM-d";

                    this._expirationDateTime += DateUtil.getTimeStampToDateTime(this._expirationTimestamp).ToString(timeFormat);
                }
               
                NotifyPropertyChanged("ExpirationTimestamp");
                NotifyPropertyChanged("ExpirationDateTime");
            }
        }

        private string _position;
        public string Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
                NotifyPropertyChanged("Position");
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private string _location;
        public string Location
        {
            get
            {
                return this._location;
            }
            set
            {
                this._location = value;
                this._location = this._location.Replace(" &gt;", "");

                NotifyPropertyChanged("Location");
            }
        }

        private string _jobType;
        public string JobType
        {
            get
            {
                return this._jobType;
            }
            set
            {
                this._jobType = value;
                NotifyPropertyChanged("JobType");
            }
        }

        private string _industry;
        public string Industry
        {
            get
            {
                return this._industry;
            }
            set
            {
                this._industry = value;
                NotifyPropertyChanged("Industry");
            }
        }

        private string _jobCategory;
        public string JobCategory
        {
            get
            {
                return this._jobCategory;
            }
            set
            {
                this._jobCategory = value;
                NotifyPropertyChanged("JobCategory");
            }
        }

        private int _openQuantity;
        public int OpenQuantity
        {
            get
            {
                return this._openQuantity;
            }
            set
            {
                this._openQuantity = value;
                NotifyPropertyChanged("OpenQuantity");
            }
        }

        private string _experienceLevel;
        public string ExperienceLevel
        {
            get
            {
                return this._experienceLevel;
            }
            set
            {
                this._experienceLevel = value;
                NotifyPropertyChanged("ExperienceLevel");
            }
        }

        private string _requiredEducationLevel;
        public string RequiredEducationLevel
        {
            get
            {
                return this._requiredEducationLevel;
            }
            set
            {
                this._requiredEducationLevel = value;
                NotifyPropertyChanged("RequiredEducationLevel");
            }
        }

        private string _keyword;
        public string Keyword
        {
            get
            {
                return this._keyword;
            }
            set
            {
                this._keyword = value;
                this._keyword = this._keyword.Replace(",", ", ");
                NotifyPropertyChanged("Keyword");
            }
        }

        private string _salary;
        public string Salary
        {
            get
            {
                return this._salary;
            }
            set
            {
                this._salary = value;
                NotifyPropertyChanged("Salary");
            }
        }

        public void LoadData(Job job)
        {
            this.Id = job.Id;
            this.Url = job.Url;
            this.CompanyName = job.CompanyName;
            this.Active = job.Active;
            this.PostingTimestamp = job.PostingTimestamp;
            this.ModificationTimestamp = job.ModificationTimestamp;
            this.OpeningTimestamp = job.OpeningTimestamp;
            this.ExpirationTimestamp = job.ExpirationTimestamp;
            this.Title = job.Title;
            this.Location = job.Location;
            this.JobType = job.JobType;
            this.Industry = job.Industry;
            this.JobCategory = job.JobCategory;
            this.OpenQuantity = job.OpenQuantity;
            this.ExperienceLevel = job.ExperienceLevel;
            this.RequiredEducationLevel = job.RequiredEducationLevel;
            this.Keyword = job.Keyword;
            this.Salary = job.Salary;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
