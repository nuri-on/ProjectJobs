using ProjectJobs.Command;
using ProjectJobs.Config;
using ProjectJobs.Data;
using ProjectJobs.Manager;
using ProjectJobs.Model;
using ProjectJobs.Util;
using ProjectJobs.viewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// 그룹 정보 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234229에 나와 있습니다.

namespace ProjectJobs
{
    /// <summary>
    /// 그룹 내에 있는 항목의 미리 보기를 비롯해 단일 그룹에 대한 개요를 표시하는
    /// 페이지입니다.
    /// </summary>
    public sealed partial class GroupDetailPage : ProjectJobs.Common.LayoutAwarePage
    {
        private List<Job> _jobs = new List<Job>();
        private JobsViewModel _jobsViewModel;
        private JobsCommand _jobsCommand = new JobsCommand();

        private Boolean isJobViewed = false;

        public ObservableCollection<JobsViewModel> Groups { get; private set; }

        public GroupDetailPage()
        {
            this.InitializeComponent();

            this.Groups = new ObservableCollection<JobsViewModel>();
            // this._jobsCommand = new JobsCommand();
        }

        async Task<List<Job>> getJosbs(JobsCommand jobsCommand)
        {
            JobsManager jobsManager = new JobsManager();

            Task<List<Job>> getJobsData = jobsManager.getJobs(jobsCommand);

            List<Job> jobs = await getJobsData;

            return jobs;
        }

        /// <summary>
        /// 탐색 중 전달된 콘텐츠로 페이지를 채웁니다. 이전 세션의 페이지를
        /// 다시 만들 때 저장된 상태도 제공됩니다.
        /// </summary>
        /// <param name="navigationParameter">이 페이지가 처음 요청될 때
        /// <see cref="Frame.Navigate(Type, Object)"/>에 전달된 매개 변수 값입니다.
        /// </param>
        /// <param name="pageState">이전 세션 동안 이 페이지에 유지된
        /// 사전 상태입니다. 페이지를 처음 방문할 때는 이 값이 null입니다.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: 문제 도메인에 적합한 데이터 모델을 만들어 샘플 데이터를 바꿉니다.
            _jobsCommand.Start = 0;
            _jobsCommand.Count = JobsConfig.ITEMS_PER_GROUP_DETAIL;

            if (navigationParameter.GetType() == typeof(int))
            {
                _jobsCommand.jobCategoryCode = (int)navigationParameter;

                var getJobsTask = this.getJosbs(_jobsCommand);
                var getJobsAwaiter = getJobsTask.GetAwaiter();

                getJobsAwaiter.OnCompleted(() =>
                {
                    this._jobs = getJobsAwaiter.GetResult();
                    // this._jobs = getJobsAwaiter.GetResult();

                    this._jobsViewModel = new JobsViewModel();
                    this._jobsViewModel.LoadData(this._jobs);

                    this._jobsViewModel.Title = JobsConfig.JOBS_CATEGORIES_MAP[_jobsCommand.jobCategoryCode];
                    this._jobsViewModel.Code = _jobsCommand.jobCategoryCode;

                    this.Groups.Add(this._jobsViewModel);

                    this.DefaultViewModel["Groups"] = this.Groups;
                });
            }
            else
            {
                _jobsCommand.Start = 0;
                _jobsCommand.Count = JobsConfig.ITEMS_PER_GROUP_DETAIL;

                _jobsCommand.Keyword = (string)navigationParameter;

                var getJobsTask = this.getJosbs(_jobsCommand);
                var getJobsAwaiter = getJobsTask.GetAwaiter();

                getJobsAwaiter.OnCompleted(() =>
                {
                    this._jobs = getJobsAwaiter.GetResult();
                    // this._jobs = getJobsAwaiter.GetResult();

                    this._jobsViewModel = new JobsViewModel();
                    this._jobsViewModel.LoadData(this._jobs);

                    this._jobsViewModel.Title = _jobsCommand.Keyword;
            
                    this.Groups.Add(this._jobsViewModel);

                    this.DefaultViewModel["Groups"] = this.Groups;
                });
            }
            // var group = SampleDataSource.GetGroup((String)navigationParameter);
            // this.DefaultViewModel["Items"] = group.Items;
        }

        /// <summary>
        /// 그룹 내의 항목을 클릭할 때 호출됩니다.
        /// </summary>
        /// <param name="sender">클릭된 항목을 표시하는
        /// GridView(또는 응용 프로그램이 기본 뷰 상태인 경우 ListView)입니다.</param>
        /// <param name="e">클릭된 항목을 설명하는 이벤트 데이터입니다.</param>
        void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.isJobViewed == false)
            {
                var jobViewModel = ((JobViewModel)e.ClickedItem);
                var folderName = "pin.json";

                Job job = new Job();

                job.Id = jobViewModel.Id;
                job.GroupCode = jobViewModel.GroupCode;
                job.Url = jobViewModel.Url;
                job.CompanyName = jobViewModel.CompanyName;
                job.Active = jobViewModel.Active;
                job.PostingTimestamp = jobViewModel.PostingTimestamp;
                job.ModificationTimestamp = jobViewModel.ModificationTimestamp;
                job.OpeningTimestamp = jobViewModel.OpeningTimestamp;
                job.ExpirationTimestamp = jobViewModel.ExpirationTimestamp;
                job.Title = jobViewModel.Title;
                job.Location = jobViewModel.Location;
                job.JobType = jobViewModel.JobType;
                job.Industry = jobViewModel.Industry;
                job.JobCategory = jobViewModel.JobCategory;
                job.OpenQuantity = jobViewModel.OpenQuantity;
                job.ExperienceLevel = jobViewModel.ExperienceLevel;
                job.RequiredEducationLevel = jobViewModel.RequiredEducationLevel;
                job.Keyword = jobViewModel.Keyword;
                job.Salary = jobViewModel.Salary;

                StorageUtil.SaveJob(folderName, job);

                // StorageUtil.Data.Add(job);
                // StorageUtil.Save<Job>();

                // 해당하는 대상 페이지로 이동합니다. 필요한 정보를 탐색 매개 변수로
                // 전달하여 새 페이지를 구성합니다.
                // var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
                // this.Frame.Navigate(typeof(ItemDetailPage), itemId);

                JobItemViewModel jobItemViewModel = new JobItemViewModel();

                this.JobItem.DataContext = jobItemViewModel;

                jobItemViewModel.Id = jobViewModel.Id;
                jobItemViewModel.GroupCode = jobViewModel.GroupCode;
                jobItemViewModel.Url = jobViewModel.Url;
                jobItemViewModel.CompanyName = jobViewModel.CompanyName;
                jobItemViewModel.Active = jobViewModel.Active;
                jobItemViewModel.PostingTimestamp = jobViewModel.PostingTimestamp;
                jobItemViewModel.ModificationTimestamp = jobViewModel.ModificationTimestamp;
                jobItemViewModel.OpeningTimestamp = jobViewModel.OpeningTimestamp;
                jobItemViewModel.ExpirationTimestamp = jobViewModel.ExpirationTimestamp;
                jobItemViewModel.Title = jobViewModel.Title;
                jobItemViewModel.Location = jobViewModel.Location;
                jobItemViewModel.JobType = jobViewModel.JobType;
                jobItemViewModel.Industry = jobViewModel.Industry;
                jobItemViewModel.JobCategory = jobViewModel.JobCategory;
                jobItemViewModel.OpenQuantity = jobViewModel.OpenQuantity;
                jobItemViewModel.ExperienceLevel = jobViewModel.ExperienceLevel;
                jobItemViewModel.RequiredEducationLevel = jobViewModel.RequiredEducationLevel;
                jobItemViewModel.Keyword = jobViewModel.Keyword;
                jobItemViewModel.Salary = jobViewModel.Salary;

                // this.companyName.Text = jobViewModel.CompanyName;

                if (jobItemViewModel.Id == JobsConfig.GROUP_FAVORITE_CODE)
                {
                    this.buttonAddFavorite.Content = new BitmapImage(new Uri("Assets/delete.png"));
                    
                    
                }

                this.itemGridView.Opacity = 0.5;
                this.JobItem.Visibility = Visibility.Visible;

                this.addJobItem(JobsConfig.GROUP_PIN_CODE, job);
            }

            this.isJobViewed = true;
        }

        private void buttonJobItemClose_Click(object sender, RoutedEventArgs e)
        {
            this.isJobViewed = false;

            this.itemGridView.Opacity = 1;
            this.JobItem.Visibility = Visibility.Collapsed;
        }

        private async void buttonSetFavorite_Click(object sender, RoutedEventArgs e)
        {
            // TODO
            await Launcher.LaunchUriAsync(new Uri("http://www.danawa.com"));

            // 1. 현재 레이어 팝업(xaml 상의 이름은 JobItem)에서 취업 정보를 가져옴
            // 1.1. JobItem의 데이터 컨텍스트에서 취업 정보 추출
            JobItemViewModel jobItemViewModel = (JobItemViewModel)this.JobItem.DataContext; // Object 타입에서 jobItemViewModel로 강제 형변환

            // 2. job 모델 세팅
            var folderName = "favorite.json";

            Job job = new Job();

            // 2.1. jobItemViewModel에서 job 모델로 데이터 이전
            job.Id = jobItemViewModel.Id;
            job.GroupCode = jobItemViewModel.GroupCode;
            job.Url = jobItemViewModel.Url;
            job.CompanyName = jobItemViewModel.CompanyName;
            job.Active = jobItemViewModel.Active;
            job.PostingTimestamp = jobItemViewModel.PostingTimestamp;
            job.ModificationTimestamp = jobItemViewModel.ModificationTimestamp;
            job.OpeningTimestamp = jobItemViewModel.OpeningTimestamp;
            job.ExpirationTimestamp = jobItemViewModel.ExpirationTimestamp;
            job.Title = jobItemViewModel.Title;
            job.Location = jobItemViewModel.Location;
            job.JobType = jobItemViewModel.JobType;
            job.Industry = jobItemViewModel.Industry;
            job.JobCategory = jobItemViewModel.JobCategory;
            job.OpenQuantity = jobItemViewModel.OpenQuantity;
            job.ExperienceLevel = jobItemViewModel.ExperienceLevel;
            job.RequiredEducationLevel = jobItemViewModel.RequiredEducationLevel;
            job.Keyword = jobItemViewModel.Keyword;
            job.Salary = jobItemViewModel.Salary;

            // 3. 로컬 스토리지에 저장
            if (job.GroupCode >= 100)
            {
                StorageUtil.SaveJob(folderName, job);

                this.addJobItem(JobsConfig.GROUP_FAVORITE_CODE, job);

                var message = new MessageDialog("즐겨찾기에 추가되었습니다.");
                message.ShowAsync();
            }
            else if (job.GroupCode == JobsConfig.GROUP_PIN_CODE)
            {
                //
            }
            else if (job.GroupCode == JobsConfig.GROUP_FAVORITE_CODE)
            {
                StorageUtil.DeleteJob(folderName, job);

                this.removeJobItem(job.GroupCode, job.Id);

                var message = new MessageDialog("즐겨찾기에 삭제되었습니다.");
                message.ShowAsync();
            }
        }

        private void removeJobItem(int p1, int p2)
        {
            throw new NotImplementedException();
        }

        private void addJobItem(int groupCode, Job job)
        {
            Boolean isGroupExist = false;

            foreach (JobsViewModel jobsViewModel in this.Groups)
            {
                if (jobsViewModel.Code == groupCode)
                {
                    isGroupExist = true;
                }
            }

            if (isGroupExist == false)
            {
                this._jobsViewModel = new JobsViewModel();

                if (groupCode == JobsConfig.GROUP_PIN_CODE)
                {
                    this._jobsViewModel.Title = JobsConfig.GROUP_PIN_NAME;
                    this._jobsViewModel.Code = JobsConfig.GROUP_PIN_CODE;
                }
                else if (groupCode == JobsConfig.GROUP_FAVORITE_CODE)
                {
                    this._jobsViewModel.Title = JobsConfig.GROUP_FAVORITE_NAME;
                    this._jobsViewModel.Code = JobsConfig.GROUP_FAVORITE_CODE;
                }

                this.Groups.Add(this._jobsViewModel);

                //this.orderGroups();
            }

            foreach (JobsViewModel jobsViewModel in this.Groups)
            {
                if (jobsViewModel.Code == groupCode)
                {
                    ObservableCollection<JobViewModel> jobItems = jobsViewModel.JobsResource;
                    Boolean isJobItemExist = false;

                    for (int index = 0; index < jobItems.Count; index++)
                    {
                        if (jobItems[index].Id == job.Id)
                        {
                            isJobItemExist = true;

                            break;
                        }
                    }

                    if (isJobItemExist == false)
                    {
                        JobViewModel jobviewModel = new JobViewModel();

                        jobviewModel.Id = job.Id;
                        jobviewModel.GroupCode = groupCode;
                        jobviewModel.Url = job.Url;
                        jobviewModel.CompanyName = job.CompanyName;
                        jobviewModel.Active = job.Active;
                        jobviewModel.PostingTimestamp = job.PostingTimestamp;
                        jobviewModel.ModificationTimestamp = job.ModificationTimestamp;
                        jobviewModel.OpeningTimestamp = job.OpeningTimestamp;
                        jobviewModel.ExpirationTimestamp = job.ExpirationTimestamp;
                        jobviewModel.Title = job.Title;
                        jobviewModel.Location = job.Location;
                        jobviewModel.JobType = job.JobType;
                        jobviewModel.Industry = job.Industry;
                        jobviewModel.JobCategory = job.JobCategory;
                        jobviewModel.OpenQuantity = job.OpenQuantity;
                        jobviewModel.ExperienceLevel = job.ExperienceLevel;
                        jobviewModel.RequiredEducationLevel = job.RequiredEducationLevel;
                        jobviewModel.Keyword = job.Keyword;
                        jobviewModel.Salary = job.Salary;

                        jobItems.Add(jobviewModel);
                    }
                }
            }

            this.refreshGroups();
        }

        private void refreshGroups()
        {
            for (int index = 0; index < this.Groups.Count; index++)
            {
                if (this.Groups[index].JobsResource.Count == 0)
                {
                    this.Groups.Remove(this.Groups[index]);
                }
            }

            //this.orderGroups();
        }

        //private void orderGroups()
        //{
        //    this.groupedItemsViewSource.Source = new ObservableCollection<JobsViewModel>(from i in this.Groups orderby i.Code select i);
        //}

        private void comboboxOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string order = "";

            ComboBox combobox = sender as ComboBox;

            ComboBoxItem comboboxItem = (ComboBoxItem)combobox.SelectedItem;

            if (comboboxItem.Name.Equals("order") == false)
            {
                order = comboboxItem.Name;

                var getJobsTask = this.getJosbs(_jobsCommand);
                var getJobsAwaiter = getJobsTask.GetAwaiter();

                getJobsAwaiter.OnCompleted(() =>
                {
                    this._jobs = getJobsAwaiter.GetResult();
                    // this._jobs = getJobsAwaiter.GetResult();

                    this._jobsViewModel = new JobsViewModel();
                    this._jobsViewModel.LoadData(this._jobs);

                    this.Groups.Clear();

                    this._jobsViewModel.Title = JobsConfig.JOBS_CATEGORIES_MAP[_jobsCommand.jobCategoryCode];
                    
                    this.Groups.Add(this._jobsViewModel);

                    this.DefaultViewModel["Groups"] = this.Groups;
                });
            }

            this._jobsCommand.Order = order;
        }

        private void comboboxLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int location = 0;

            ComboBox combobox = sender as ComboBox;

            ComboBoxItem comboboxItem = (ComboBoxItem)combobox.SelectedItem;

            if (comboboxItem.Name.Equals("location") == false)
            {
                location = Int32.Parse(comboboxItem.Name.Replace("location_", ""));

                var getJobsTask = this.getJosbs(_jobsCommand);
                var getJobsAwaiter = getJobsTask.GetAwaiter();

                getJobsAwaiter.OnCompleted(() =>
                {
                    this._jobs = getJobsAwaiter.GetResult();
                    // this._jobs = getJobsAwaiter.GetResult();

                    this._jobsViewModel = new JobsViewModel();
                    this._jobsViewModel.LoadData(this._jobs);

                    this.Groups.Clear();

                    this._jobsViewModel.Title = JobsConfig.JOBS_CATEGORIES_MAP[_jobsCommand.jobCategoryCode];
                    
                    this.Groups.Add(this._jobsViewModel);

                    this.DefaultViewModel["Groups"] = this.Groups;
                });
            }

            this._jobsCommand.Location = location;
        }
    }
}

