using ProjectJobs.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ProjectJobs.Config;
using System.ComponentModel;
using System;

namespace ProjectJobs.viewModel
{
    public class JobsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<JobViewModel> JobsResource { get; private set; }
        public ObservableCollection<JobViewModel> JobsRawResource { get; private set; }

        private int _code;
        public int Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
                NotifyPropertyChanged("Code");
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

        public JobsViewModel()
        {
            this.JobsResource = new ObservableCollection<JobViewModel>();
            this.JobsRawResource = new ObservableCollection<JobViewModel>();

            this.JobsRawResource.CollectionChanged += JobsRawResource_CollectionChanged;
        }

        public void LoadData(List<Job> jobs)
        {
            foreach (Job job in jobs)
            {
                this.JobsRawResource.Add(new JobViewModel()
                {
                    Id = job.Id,
                    GroupCode = this.Code,
                    Url = job.Url,
                    CompanyName = job.CompanyName,
                    Active = job.Active,
                    PostingTimestamp = job.PostingTimestamp,
                    ModificationTimestamp = job.ModificationTimestamp,
                    OpeningTimestamp = job.OpeningTimestamp,
                    ExpirationTimestamp = job.ExpirationTimestamp,
                    Title = job.Title,
                    Location = job.Location,
                    JobType = job.JobType,
                    Industry = job.Industry,
                    JobCategory = job.JobCategory,
                    OpenQuantity = job.OpenQuantity,
                    ExperienceLevel = job.ExperienceLevel,
                    RequiredEducationLevel = job.RequiredEducationLevel,
                    Keyword = job.Keyword,
                    Salary = job.Salary
                });
            }
        }

        private void JobsRawResource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // 두 가지 이유를 위해 GroupedItemsPage에서 바인딩할 전체 항목 컬렉션의
            // 하위 집합을 제공합니다. GridView는 대용량 항목 컬렉션을 가상화하지 않고
            // 많은 항목으로 구성된 그룹에서 검색할 때의 사용자 경험을
            // 향상시킵니다.
            //
            // 1, 2, 3, 4 또는 6행이 표시된 표 형태 창 열에 채워지므로
            // 최대 12항목이 표시됩니다.
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < JobsConfig.ITEMS_PER_GROUP_DETAIL)
                    {
                        this.JobsResource.Insert(e.NewStartingIndex, this.JobsRawResource[e.NewStartingIndex]);
                        if (this.JobsResource.Count > JobsConfig.ITEMS_PER_GROUP_DETAIL)
                        {
                            this.JobsResource.RemoveAt(JobsConfig.ITEMS_PER_GROUP_DETAIL);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < JobsConfig.ITEMS_PER_GROUP_DETAIL && e.NewStartingIndex < JobsConfig.ITEMS_PER_GROUP_DETAIL)
                    {
                        this.JobsResource.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < JobsConfig.ITEMS_PER_GROUP_DETAIL)
                    {
                        this.JobsResource.RemoveAt(e.OldStartingIndex);
                        this.JobsResource.Add(this.JobsRawResource[11]);
                    }
                    else if (e.NewStartingIndex < JobsConfig.ITEMS_PER_GROUP_DETAIL)
                    {
                        this.JobsResource.Insert(e.NewStartingIndex, this.JobsRawResource[e.NewStartingIndex]);
                        this.JobsResource.RemoveAt(JobsConfig.ITEMS_PER_GROUP_DETAIL);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < JobsConfig.ITEMS_PER_GROUP_DETAIL)
                    {
                        this.JobsResource.RemoveAt(e.OldStartingIndex);
                        if (this.JobsResource.Count >= JobsConfig.ITEMS_PER_GROUP_DETAIL)
                        {
                            this.JobsResource.Add(this.JobsRawResource[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < JobsConfig.ITEMS_PER_GROUP_DETAIL)
                    {
                        this.JobsResource[e.OldStartingIndex] = this.JobsRawResource[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.JobsResource.Clear();
                    while (this.JobsResource.Count < this.JobsRawResource.Count && this.JobsResource.Count < JobsConfig.ITEMS_PER_GROUP_DETAIL)
                    {
                        this.JobsResource.Add(this.JobsRawResource[this.JobsResource.Count]);
                    }
                    break;
            }
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
