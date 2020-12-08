using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for MemberScreen.xaml
    /// </summary>
    public partial class MemberScreen : UserControl
    {
        ObservableCollection<MemberModel> memberList;
        public MemberScreen()
        {
            InitializeComponent();
        }

        private void MemberScreen_Loaded(object sender, RoutedEventArgs e)
        {
            memberList = new ObservableCollection<MemberModel>(DatabaseAccess.LoadMember());
            MemberListView.ItemsSource = memberList;
        }

        private void AddMemberBtn_Click(object sender, RoutedEventArgs e)
        {
            //Handle exception

            //Save member
            MemberModel member = new MemberModel { MemberName = MemberNameTxt.Text, MemberTel = MemberTelTxt.Text, MemberAddr = MemberAddrTxt.Text };
            DatabaseAccess.SaveMember(member);
            //Update view
            memberList = new ObservableCollection<MemberModel>(DatabaseAccess.LoadMember());
            MemberListView.ItemsSource = memberList;
        }
    }
}
