using Shoping.Presentation.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Shoping.Presentation.Control
{
    /// <summary>
    /// Interaction logic for SettingUserControl.xaml
    /// </summary>
    public partial class SettingUserControl : System.Windows.Controls.UserControl
    {
        public delegate void NewItemsPerPage(int size);
        public event NewItemsPerPage NewItemsPerPageUpdating;
        public SettingViewModel SettingViewModel { get; set; }
        //public int ItemsPerPage { get; set; }

        public SettingUserControl()
        {
            InitializeComponent();
            comboBox.Items.Add(4);
            comboBox.Items.Add(8);
            comboBox.Items.Add(12);
            comboBox.SelectedIndex = 0;
            SettingViewModel = new SettingViewModel(App.iSettingBusiness);
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = comboBox.SelectedIndex;
            int pageSize = selectedIndex != -1 ?(int)comboBox.Items[selectedIndex] : 1;
            NewItemsPerPageUpdating?.Invoke(pageSize);
            ItemsPerPage.itemsPerPage = pageSize;
            
        }

        private async void BtnBackup_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var res = await SettingViewModel.Backup(dialog.SelectedPath);
                    if (res)
                    {
                        System.Windows.MessageBox.Show("Backup thành công");
                    }
                }
            }
        }

        private async void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true; // Allow multiple file selection
            openFileDialog.Filter = "BSON Files (*.bson)|*.bson"; // Filter files to show only BSON files

            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Get the selected file names
                var selectedFiles = openFileDialog.FileNames.ToList();
                var res = await SettingViewModel.Restore(selectedFiles);
                if (res)
                {
                    System.Windows.MessageBox.Show("Restore thành công");
                }
                // Process selected files
            }
        }
    }
}
