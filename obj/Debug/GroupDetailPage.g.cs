﻿

#pragma checksum "C:\Users\Nuri\Documents\Visual Studio 2012\Projects\JobsProject\ProjectJobs\ProjectJobs\GroupDetailPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FF0735385A91AE979286494A9398E347"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectJobs
{
    partial class GroupDetailPage : global::ProjectJobs.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 55 "..\..\GroupDetailPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.GridView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 208 "..\..\GroupDetailPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.GridView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 239 "..\..\GroupDetailPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 242 "..\..\GroupDetailPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.comboboxOrder_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 268 "..\..\GroupDetailPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.comboboxLocation_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 106 "..\..\GroupDetailPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.buttonSetFavorite_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 109 "..\..\GroupDetailPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.buttonJobItemClose_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


