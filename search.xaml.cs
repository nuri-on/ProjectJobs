using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 계약 검색 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234240에 나와 있습니다.

namespace ProjectJobs
{
    // TODO: 매니페스트를 편집하여 검색을 사용하도록 설정합니다.
    //
    // 패키지 매니페스트는 자동으로 업데이트할 수 없습니다. 패키지 매니페스트
    // 파일을 열고 검색 설정 활성화가 지원되는지 확인하십시오.
    /// <summary>
    /// 이 페이지에서는 이 응용 프로그램을 대상으로 전역 검색을 수행한 경우의 검색 결과를 표시합니다.
    /// </summary>
    public sealed partial class search : ProjectJobs.Common.LayoutAwarePage
    {

        public search()
        {
            this.InitializeComponent();
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
            var queryText = navigationParameter as String;

            // TODO: 응용 프로그램별 검색 논리입니다. 검색 프로세스는
            //       사용자가 선택할 수 있는 결과 범주 목록을 만듭니다.
            //
            //       filterList.Add(new Filter("<filter name>", <result count>));
            //
            //       활성 상태에서 시작하려면 첫 번째 필터(일반적으로 "모두")만 세 번째 인수로 true를
            //       전달해야 합니다. 활성 필터의 결과가 아래의
            //       Filter_SelectionChanged에 제공됩니다.

            var filterList = new List<Filter>();
            filterList.Add(new Filter("All", 0, true));

            // 뷰 모델을 통해 결과를 전달합니다.
            this.DefaultViewModel["QueryText"] = '\u201c' + queryText + '\u201d';
            this.DefaultViewModel["Filters"] = filterList;
            this.DefaultViewModel["ShowFilters"] = filterList.Count > 1;
        }

        /// <summary>
        /// 기본 뷰 상태에서 ComboBox를 사용하여 필터를 선택할 때 호출됩니다.
        /// </summary>
        /// <param name="sender">ComboBox 인스턴스입니다.</param>
        /// <param name="e">선택한 필터가 변경된 방법을 설명하는 이벤트 데이터입니다.</param>
        void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 선택된 필터가 무엇인지 결정합니다.
            var selectedFilter = e.AddedItems.FirstOrDefault() as Filter;
            if (selectedFilter != null)
            {
                // 해당하는 필터 개체에 결과를 미러링하여
                // 기본 뷰 상태가 아닐 때 사용되는 RadioButton 표현에 변경이 반영될 수 있게 합니다.
                selectedFilter.Active = true;

                // TODO: this.DefaultViewModel["Results"]을 바인딩 가능한 이미지, 제목, 부제목 및 설명 속성이 있는
                //       항목의 컬렉션으로 설정하여 활성 필터의 변경에 응답합니다.

                // 결과가 있는지 확인합니다.
                object results;
                ICollection resultsCollection;
                if (this.DefaultViewModel.TryGetValue("Results", out results) &&
                    (resultsCollection = results as ICollection) != null &&
                    resultsCollection.Count != 0)
                {
                    VisualStateManager.GoToState(this, "ResultsFound", true);
                    return;
                }
            }

            // 검색 결과가 없을 경우 알림 텍스트를 표시합니다.
            VisualStateManager.GoToState(this, "NoResultsFound", true);
        }

        /// <summary>
        /// 기본 뷰가 아닌 경우 RadioButton을 사용하여 필터를 선택할 때 호출됩니다.
        /// </summary>
        /// <param name="sender">선택한 RadioButton 인스턴스입니다.</param>
        /// <param name="e">RadioButton을 선택한 방법을 설명하는 이벤트 데이터입니다.</param>
        void Filter_Checked(object sender, RoutedEventArgs e)
        {
            // 기본 뷰 상태일 때 변경이 반영되도록 해당하는 ComboBox에서 사용된 CollectionViewSource에
            // 변경을 미러링합니다.
            if (filtersViewSource.View != null)
            {
                var filter = (sender as FrameworkElement).DataContext;
                filtersViewSource.View.MoveCurrentTo(filter);
            }
        }

        /// <summary>
        /// 검색 결과를 볼 때 사용할 수 있는 필터 중 하나를 설명하는 뷰 모델입니다.
        /// </summary>
        private sealed class Filter : ProjectJobs.Common.BindableBase
        {
            private String _name;
            private int _count;
            private bool _active;

            public Filter(String name, int count, bool active = false)
            {
                this.Name = name;
                this.Count = count;
                this.Active = active;
            }

            public override String ToString()
            {
                return Description;
            }

            public String Name
            {
                get { return _name; }
                set { if (this.SetProperty(ref _name, value)) this.OnPropertyChanged("Description"); }
            }

            public int Count
            {
                get { return _count; }
                set { if (this.SetProperty(ref _count, value)) this.OnPropertyChanged("Description"); }
            }

            public bool Active
            {
                get { return _active; }
                set { this.SetProperty(ref _active, value); }
            }

            public String Description
            {
                get { return String.Format("{0} ({1})", _name, _count); }
            }
        }
    }
}
