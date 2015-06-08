using System.Runtime.Serialization;

namespace ProjectJobs.Model
{
    [KnownType(typeof(ProjectJobs.Model.Job))]
    [DataContractAttribute]
    public class Job
    {
        /// <summary>
        /// 취업 
        /// </summary>
        private int _id;
        [DataMember()]
        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        private int _groupCode;
        [DataMember()]
        public int GroupCode
        {
            get
            {
                return this._groupCode;
            }
            set
            {
                this._groupCode = value;
            }
        }

        private string _url;
        [DataMember()]
        public string Url
        {
            get
            {
                return this._url;
            }
            set
            {
                this._url = value;
            }
        }

        private string _companyName;
        [DataMember()]
        public string CompanyName
        {
            get
            {
                return this._companyName;
            }
            set
            {
                this._companyName = value;
            }
        }

        private int _active;
        [DataMember()]
        public int Active
        {
            get
            {
                return this._active;
            }
            set
            {
                this._active = value;
            }
        }

        private int _postingTimestamp;
        [DataMember()]
        public int PostingTimestamp
        {
            get
            {
                return this._postingTimestamp;
            }
            set
            {
                this._postingTimestamp = value;
            }
        }

        private int _modificationTimestamp;
        [DataMember()]
        public int ModificationTimestamp
        {
            get
            {
                return this._modificationTimestamp;
            }
            set
            {
                this._modificationTimestamp = value;
            }
        }

        private int _openingTimestamp;
        [DataMember()]
        public int OpeningTimestamp
        {
            get
            {
                return this._openingTimestamp;
            }
            set
            {
                this._openingTimestamp = value;
            }
        }

        private double _expirationTimestamp;
        [DataMember()]
        public double ExpirationTimestamp
        {
            get
            {
                return this._expirationTimestamp;
            }
            set
            {
                this._expirationTimestamp = value;
            }
        }

        private string _title;
        [DataMember()]
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        private string _location;
        [DataMember()]
        public string Location
        {
            get
            {
                return this._location;
            }
            set
            {
                this._location = value;
            }
        }

        private string _jobType;
        [DataMember()]
        public string JobType
        {
            get
            {
                return this._jobType;
            }
            set
            {
                this._jobType = value;
            }
        }

        private string _industry;
        [DataMember()]
        public string Industry
        {
            get
            {
                return this._industry;
            }
            set
            {
                this._industry = value;
            }
        }

        private string _jobCategory;
        [DataMember()]
        public string JobCategory
        {
            get
            {
                return this._jobCategory;
            }
            set
            {
                this._jobCategory = value;
            }
        }

        private int _openQuantity;
        [DataMember()]
        public int OpenQuantity
        {
            get
            {
                return this._openQuantity;
            }
            set
            {
                this._openQuantity = value;
            }
        }

        private string _experienceLevel;
        [DataMember()]
        public string ExperienceLevel
        {
            get
            {
                return this._experienceLevel;
            }
            set
            {
                this._experienceLevel = value;
            }
        }

        private string _requiredEducationLevel;
        [DataMember()]
        public string RequiredEducationLevel
        {
            get
            {
                return this._requiredEducationLevel;
            }
            set
            {
                this._requiredEducationLevel = value;
            }
        }

        private string _keyword;
        [DataMember()]
        public string Keyword
        {
            get
            {
                return this._keyword;
            }
            set
            {
                this._keyword = value;
            }
        }

        private string _salary;
        [DataMember()]
        public string Salary
        {
            get
            {
                return this._salary;
            }
            set
            {
                this._salary = value;
            }
        }
    }
}
