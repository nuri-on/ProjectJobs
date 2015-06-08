using ProjectJobs.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
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

// 표 형태 응용 프로그램 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234226에 나와 있습니다.

namespace ProjectJobs
{
    /// <summary>
    /// 기본 응용 프로그램 클래스를 보완하는 응용 프로그램별 동작을 제공합니다.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Singleton 응용 프로그램 개체를 초기화합니다. 이것은 실행되는 작성 코드의 첫 번째
        /// 줄이며 따라서 main() 또는 WinMain()과 논리적으로 동일합니다.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// 최종 사용자가 응용 프로그램을 정상적으로 시작할 때 호출됩니다. 다른 진입점은
        /// 특정 파일을 열거나, 검색 결과를 표시하는 등 응용 프로그램을 시작할 때
        /// 사용됩니다.
        /// </summary>
        /// <param name="args">시작 요청 및 프로세스에 대한 정보입니다.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 창에 콘텐츠가 이미 있는 경우 앱 초기화를 반복하지 말고,
            // 창이 활성화되어 있는지 확인하십시오.
            
            if (rootFrame == null)
            {
                // 탐색 컨텍스트로 사용할 프레임을 만들고 첫 페이지로 이동합니다.
                rootFrame = new Frame();
                //프레임을 SuspensionManager 키에 연결                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // 해당하는 경우에만 저장된 세션 상태를 복원합니다.
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //상태를 복원하는 중에 오류가 발생했습니다.
                        //상태가 없는 것으로 가정하고 계속합니다.
                    }
                }

                // 현재 창에 프레임 넣기
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // 탐색 스택이 복원되지 않으면 첫 번째 페이지로 돌아가고
                // 필요한 정보를 탐색 매개 변수로 전달하여 새 페이지를
                // 구성합니다.
                if (!rootFrame.Navigate(typeof(GroupedItemsPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // 현재 창이 활성 창인지 확인
            Window.Current.Activate();
        }

        /// <summary>
        /// 응용 프로그램 실행이 일시 중지된 경우 호출됩니다. 응용 프로그램이 종료될지
        /// 또는 메모리 콘텐츠를 변경하지 않고 다시 시작할지 여부를 결정하지 않은 채
        /// 응용 프로그램 상태가 저장됩니다.
        /// </summary>
        /// <param name="sender">일시 중지된 요청의 소스입니다.</param>
        /// <param name="e">일시 중지된 요청에 대한 세부 정보입니다.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        /// <summary>
        /// 검색 결과를 표시하기 위해 응용 프로그램이 활성화될 때 호출됩니다.
        /// </summary>
        /// <param name="args">활성화 요청에 대한 정보입니다.</param>
        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            // TODO: Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted 등록
            // 응용 프로그램이 이미 실행 중인 경우 OnWindowCreated에서 이벤트의 검색 속도를 높입니다.

            // 창에서 아직 프레임 탐색을 사용하고 있지 않은 경우 고유한 프레임을 삽입합니다.
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // 응용 프로그램이 최상위 프레임을 포함하지 않는 경우 이는 응용 프로그램의 
            // 초기 시작일 수 있습니다. 일반적으로 이 메서드 및 App.xaml.cs의 OnLaunched는 
            // 공용 메서드를 호출할 수 있습니다.
            if (frame == null)
            {
                // 탐색 상황에 맞게 사용되는 프레임을 만들어 이를
                // SuspensionManager 키에 연결합니다.
                frame = new Frame();
                ProjectJobs.Common.SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // 해당하는 경우에만 저장된 세션 상태를 복원합니다.
                    try
                    {
                        await ProjectJobs.Common.SuspensionManager.RestoreAsync();
                    }
                    catch (ProjectJobs.Common.SuspensionManagerException)
                    {
                        //잘못된 복원 상태가 있습니다.
                        //상태가 없다고 가정하고 계속합니다.
                    }
                }
            }

            frame.Navigate(typeof(GroupDetailPage), args.QueryText);
            /*Window.Current.Content = frame;

            // 현재 윈도우가 활성인지 확인합니다.
            Window.Current.Activate();*/
        }
    }
}
