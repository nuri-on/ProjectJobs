using ProjectJobs.Command;
using ProjectJobs.Config;
using ProjectJobs.Data;
using ProjectJobs.Manager;
using ProjectJobs.Model;
using ProjectJobs.viewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ProjectJobs.Util;
using Windows.UI.Popups;
using Windows.UI.Notifications;
using ProjectJobs.NotificationExtensions;

using System.Linq;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;


// 그룹화된 항목 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234231에 나와 있습니다.
namespace ProjectJobs
{

    
    /// <summary>
    /// 그룹화된 항목 컬렉션을 표시하는 페이지입니다.
    /// </summary>
    public sealed partial class GroupedItemsPage : ProjectJobs.Common.LayoutAwarePage
    {
        
        private List<int> _GROUPS = new List<int>() { 404, 108, 113, 114 };

        private List<Job> _jobs = new List<Job>();
        private List<Job> _jobsFromLocalStorage = new List<Job>();

        private JobsViewModel _jobsViewModel;

        public ObservableCollection<JobsViewModel> Groups { get; private set; }
        public ObservableCollection<JobsViewModel> RawGroups { get; private set; }

        private Boolean isJobViewed = false;

        public GroupedItemsPage()
        {
            this.InitializeComponent();

            this.Groups = new ObservableCollection<JobsViewModel>();
            this.RawGroups = new ObservableCollection<JobsViewModel>();

            // this.itemGridView.DataContext = this._jobsViewModel;
        }

        async Task<List<Job>> getJobs(JobsCommand jobsCommand)
        {
            JobsManager jobsManager = new JobsManager();

            Task<List<Job>> getJobsData = jobsManager.getJobs(jobsCommand);

            List<Job> jobs = await getJobsData;

            return jobs;
        }
        
        async void getJobsFromLocalStorage()
        {
            var getJobsFromLocalStorageTask = StorageUtil.Restore("pin.json");
            var getJobsFromLocalStorageAwaiter = getJobsFromLocalStorageTask.GetAwaiter();

            getJobsFromLocalStorageAwaiter.OnCompleted(() => 
            {
                try
                {
                    List<Job> jobs = getJobsFromLocalStorageAwaiter.GetResult();

                    if (jobs.Count > 0)
                    {
                        // MessageDialog messageDialog = new MessageDialog(jobs.Count.ToString());
                        //messageDialog.ShowAsync();

                        this._jobsViewModel = new JobsViewModel();
                        this._jobsViewModel.Title = JobsConfig.GROUP_PIN_NAME;
                        this._jobsViewModel.Code = JobsConfig.GROUP_PIN_CODE;
                        this._jobsViewModel.LoadData(jobs);

                        this.Groups.Add(this._jobsViewModel);
                    }
                }
                catch
                {
                    // TODO
                }
            });
        }
        

        async void getFavoriteJobsFromLocalStorage()
        {
            var getJobsFromLocalStorageTask = StorageUtil.Restore("favorite.json");
            var getJobsFromLocalStorageAwaiter = getJobsFromLocalStorageTask.GetAwaiter();

            getJobsFromLocalStorageAwaiter.OnCompleted(() =>
            {
                try
                {
                    List<Job> jobs = getJobsFromLocalStorageAwaiter.GetResult();

                    if (jobs.Count > 0)
                    {
                        // MessageDialog messageDialog = new MessageDialog(jobs.Count.ToString());
                        //messageDialog.ShowAsync();

                        this._jobsViewModel = new JobsViewModel();
                        this._jobsViewModel.Title = JobsConfig.GROUP_FAVORITE_NAME;
                        this._jobsViewModel.Code = JobsConfig.GROUP_FAVORITE_CODE;

                        this._jobsViewModel.LoadData(jobs);

                        this.Groups.Add(this._jobsViewModel);
                    }
                }
                catch
                {
                    // TODO
                }
            });
        }

        async Task<Job> getCurrentJobFromLocalStorage()
        {
            Task<List<Job>> getJobsFromLocalStorageTask = StorageUtil.Restore("pin.json");

            List<Job> jobs = await getJobsFromLocalStorageTask;

            if (jobs.Count > 0)
            {
                return jobs[jobs.Count - 1];
            }
            else
            {
                return null;
            }
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
            JobsCommand jobsCommand = new JobsCommand();

            jobsCommand.Start = 0;
            jobsCommand.Count = JobsConfig.ITEMS_PER_GROUP_MAIN;

            foreach (int jobCategoryCode in this._GROUPS)
            {
                jobsCommand.jobCategoryCode = jobCategoryCode;

                var getJobsTask = this.getJobs(jobsCommand);
                var getJobsAwaiter = getJobsTask.GetAwaiter();

                getJobsAwaiter.OnCompleted(() =>
                {
                    this._jobs = getJobsAwaiter.GetResult();
                    // this._jobs = getJobsAwaiter.GetResult();

                    this._jobsViewModel = new JobsViewModel();
                    this._jobsViewModel.Title = JobsConfig.JOBS_CATEGORIES_MAP[jobCategoryCode];
                    this._jobsViewModel.Code = jobCategoryCode;
                    
                    this._jobsViewModel.LoadData(this._jobs);

                    this.RawGroups.Add(this._jobsViewModel);

                    if (RawGroups.Count == this._GROUPS.Count)
                    {
                        this.arrangeGroups();

                        this.DefaultViewModel["Groups"] = this.Groups;

                        // 라이브 타일 설정
                        this.setLiveTile();
                    }
                });
            }

            // 불러오기
            this.getJobsFromLocalStorage();
            this.getFavoriteJobsFromLocalStorage();


        }

        private void setLiveTile() 
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            List<JobViewModel> jobsViewModel = new List<JobViewModel>();

            for (int index = 0; index < this.Groups.Count; index++)
            {
                if (this.Groups[index].Code >= 100)
                {
                    jobsViewModel.Add(this.Groups[index].JobsResource[0]);
                }
            }

            /*
            Random rand = new Random();

            int groupsIndex = rand.Next(3);
            int jobsResourceIndex = rand.Next(3);

            while (this.Groups[groupsIndex].JobsResource[jobsResourceIndex].Title != null)
            {
                jobsViewModel.Add(this.Groups[groupsIndex].JobsResource[jobsResourceIndex]);

                if (jobsViewModel.Count == 20)
                    break;

                groupsIndex = rand.Next(3);
                jobsResourceIndex = rand.Next(3);
            }
            */
            
            foreach (JobViewModel jobViewModel in jobsViewModel)
            {
                // 정사각형 라이브 타일
                var squareTile = new TileSquarePeekImageAndText04();

                squareTile.TextBodyWrap.Text = jobViewModel.Title;
                squareTile.Image.Alt = jobViewModel.CompanyName;
                squareTile.Image.Src = "";

                // 큰사각형 라이브 타일
                var wideTile = new TileWideSmallImageAndText03 { SquareContent = squareTile };

                wideTile.TextBodyWrap.Text = jobViewModel.Title;
                wideTile.Image.Alt = jobViewModel.CompanyName;
                wideTile.Image.Src = "";

                // 알림 설정
                var notification = wideTile.CreateNotification();
                notification.Tag = jobViewModel.Id.ToString();

                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            }
        }

        private void arrangeGroups()
        {
            foreach (int groupCode in this._GROUPS)
            {
                JobsViewModel jobsViewModel = this.getGroupByCode(groupCode);

                if (jobsViewModel != null)
                {
                    this.Groups.Add(jobsViewModel);
                }
            }
        }

        private JobsViewModel getGroupByCode(int code)
        {
            foreach (JobsViewModel jobsViewModel in this.RawGroups)
            {
                if (jobsViewModel.Code == code)
                {
                    return jobsViewModel;
                }
            }

            return null;
        }

        /// <summary>
        /// 그룹 머리글을 클릭할 때 호출됩니다.
        /// </summary>
        /// <param name="sender">선택한 그룹의 그룹 머리글로 사용되는 단추입니다.</param>
        /// <param name="e">클릭이 시작된 방법을 설명하는 이벤트 데이터입니다.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // 단추 인스턴스가 나타내는 그룹을 결정합니다.
            var group = (sender as FrameworkElement).DataContext;

            // 해당하는 대상 페이지로 이동합니다. 필요한 정보를 탐색 매개 변수로
            // 전달하여 새 페이지를 구성합니다.
            this.Frame.Navigate(typeof(GroupDetailPage), ((JobsViewModel)group).Code);
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

                if (jobViewModel.GroupCode == JobsConfig.GROUP_FAVORITE_CODE)
                {
                    this.buttonAddFavorite.Content = new Image()
                    {
                        Source = new BitmapImage(new Uri(this.BaseUri, "Assets/delete.png"))
                    };
                }
                else
                {
                    this.buttonAddFavorite.Content = new Image()
                    {
                        Source = new BitmapImage(new Uri(this.BaseUri, "assets/favs.addto.png"))
                    };
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

        private void buttonSetFavorite_Click(object sender, RoutedEventArgs e)
        {
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

                this.orderGroups();
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

        private void removeJobItem(int groupCode, int itemCode)
        {
            foreach (JobsViewModel jobsViewModel in this.Groups)
            {
                if (jobsViewModel.Code == groupCode)
                {
                    ObservableCollection<JobViewModel> jobItems = jobsViewModel.JobsResource;

                    for (int index = 0; index < jobItems.Count; index++)
                    {
                        if (jobItems[index].Id == itemCode)
                        {
                            jobItems.Remove(jobItems[index]);
                        }
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

            this.orderGroups();
        }

        private void orderGroups()
        {
            this.groupedItemsViewSource.Source = new ObservableCollection<JobsViewModel>(from i in this.Groups orderby i.Code select i);
        }

        private void buttonAddFavorite_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }
    }
}
 