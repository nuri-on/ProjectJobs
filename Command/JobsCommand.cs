using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectJobs.Command
{
    class JobsCommand
    {
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
            }
        }

        private int _jobCategoryCode;
        public int jobCategoryCode
        {
            get
            {
                return this._jobCategoryCode;
            }
            set
            {
                this._jobCategoryCode = value;
            }
        }

        private int _start;
        public int Start
        {
            get
            {
                return this._start;
            }
            set
            {
                this._start = value;
            }
        }

        private int _count;
        public int Count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
            }
        }

        private string _order = "";
        public string Order
        {
            get
            {
                return this._order;
            }
            set
            {
                this._order = value;
            }
        }

        private int _location;
        public int Location
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
    }
}
