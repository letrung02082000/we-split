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
using System.Windows.Shapes;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for AddMemberWindow.xaml
    /// </summary>
    public partial class AddMemberWindow : Window
    {
        public delegate void AddMember(MemberModel member);
        public event AddMember EventHandler;
        public ObservableCollection<MemberModel> MemberList;
        public AddMemberWindow()
        {
            InitializeComponent();
        }

        private void AddMemberWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MemberList = new ObservableCollection<MemberModel>(DatabaseAccess.LoadMember());
            MemberListView.ItemsSource = MemberList;
        }

        private void MemberListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MemberModel member = MemberList[MemberListView.SelectedIndex];
            member.OldMember = true;
            if (EventHandler != null)
            {
                EventHandler(member);
                this.Close();
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddMemberBtn_Click(object sender, RoutedEventArgs e)
        {
            if((NameTextBox.Text.Trim() == "") || (AddrTextBox.Text.Trim() == "") || (TelTextBox.Text.Trim() == ""))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các thông tin.");
                return;
            }

            if (EventHandler != null)
            {
                MemberModel member = new MemberModel { MemberName = NameTextBox.Text, MemberTel = TelTextBox.Text, MemberAddr = AddrTextBox.Text, OldMember=false };
                EventHandler(member);
                this.Close();
            }
        }
    }
}
