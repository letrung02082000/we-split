﻿using ModelLibrary;
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
    }
}
