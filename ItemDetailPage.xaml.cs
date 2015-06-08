using ProjectJobs.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 항목 정보 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234232에 나와 있습니다.

namespace ProjectJobs
{
    /// <summary>
    /// 그룹 내의 단일 항목에 대한 정보를 표시하며 같은 그룹에 속한 다른 항목으로
    /// 전환하는 제스처도 허용하는 페이지입니다.
    /// </summary>
    public sealed partial class ItemDetailPage : ProjectJobs.Common.LayoutAwarePage
    {
        public ItemDetailPage()
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
            // 저장된 페이지 상태에서 표시할 초기 항목을 재정의하도록 허용합니다.
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            // TODO: 문제 도메인에 적합한 데이터 모델을 만들어 샘플 데이터를 바꿉니다.
            var item = SampleDataSource.GetItem((String)navigationParameter);
            this.DefaultViewModel["Group"] = item.Group;
            this.DefaultViewModel["Items"] = item.Group.Items;
            this.flipView.SelectedItem = item;
        }

        /// <summary>
        /// 응용 프로그램이 일시 중지되거나 탐색 캐시에서 페이지가 삭제된 경우
        /// 이 페이지와 관련된 상태를 유지합니다. 값은
        /// <see cref="SuspensionManager.SessionState"/>의 serialization 요구 사항을 만족해야 합니다.
        /// </summary>
        /// <param name="pageState">serializable 상태로 채워질 빈 사전입니다.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (SampleDataItem)this.flipView.SelectedItem;
            pageState["SelectedItem"] = selectedItem.UniqueId;
        }
    }
}
