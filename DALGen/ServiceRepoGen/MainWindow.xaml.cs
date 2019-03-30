using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;
using System.Configuration;
using NokelServices.DALGenLib.Configuration;
using NokelServices.DALGenLib;

namespace ServiceRepoGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker worker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
            LoadSettingsSection();
        }

        private void LoadSettingsSection()
        {
            DALSettingsSection dss = null;

            var config = ConfigurationManager.OpenExeConfiguration(null);
            dss = (DALSettingsSection)config.Sections["dalSettings"];

            Console.WriteLine("Looking for templates in this folder: " + dss.BaseTemplateFolder);

            DALSettings settings = new DALSettings(dss);
            this.DataContext = settings;
            ConfigureWorkerThread();
            lblStatus.Text = "Ready";
        }

        private void ConfigureWorkerThread()
        {
            if (this.worker != null)
            {
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool success = Boolean.Parse(e.Result.ToString());

            if (success)
            {
                lblStatus.Text = "Files Generated successfully.";
            }
            else
            {
                lblStatus.Text = "Error generating files. Please check log for details.";
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DALSettingsSection sect = e.Argument as DALSettingsSection;

            DALSettings settings = new DALSettings(sect);
            ProcessDAL pd = new ProcessDAL(settings);

            e.Result = pd.GenerateAllFiles();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            lblStatus.Text = "Specify new config";
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".config";
            dlg.Filter = "Config files (*.config)|*.config";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                ExeConfigurationFileMap fm = new ExeConfigurationFileMap();
                fm.ExeConfigFilename = filename;
                var conf = ConfigurationManager.OpenMappedExeConfiguration(fm, ConfigurationUserLevel.None);

                DALSettingsSection dss = (DALSettingsSection)conf.Sections["dalSettings"];

                DALSettings settings = new DALSettings(dss);

                this.DataContext = settings;

                lblStatus.Text = "New Config Loaded";
                return;
            }
            lblStatus.Text = "Ready";

        }

        private void btnGen_Click(object sender, RoutedEventArgs e)
        {
            DALSettingsSection runSettings = new DALSettingsSection();

            runSettings.AddServiceExtensions = this.cbAddServiceExtensions.IsChecked.Value;
            runSettings.BaseOutputFolder = this.txtBaseOutputFolder.Text;
            runSettings.BaseTemplateFolder = this.txtBaseTempate.Text;
            runSettings.ConnectionString = this.txtConnString.Text;
            runSettings.ContextName = this.txtContextName.Text;
            runSettings.MakeObjectsPlural = this.cbPlural.IsChecked.Value;
            runSettings.ModelNameSpace = this.txtModelNamespace.Text;
            runSettings.PluralizeCollections = this.cbPluralCollections.IsChecked.Value;
            runSettings.RepositoryNameSpace = this.txtRepoNamespace.Text;
            runSettings.RepositoryTemplateClassFile = this.txtRepoClass.Text;
            runSettings.RepositoryTemplateFolder = this.txtRepoTemplate.Text;
            runSettings.RepositoryTemplateInterfaceFile = this.txtRepoInterface.Text;
            runSettings.ServiceExtensionTemplateFolder = this.txtSvcExtensionFolder.Text;
            runSettings.ServiceNameSpace = this.txtServiceNamespace.Text;
            runSettings.ServiceTemplateClassFile = this.txtServiceClass.Text;
            runSettings.ServiceTemplateFolder = this.txtServiceTemplate.Text;
            runSettings.ServiceTemplateInterfaceFile = this.txtServiceInterface.Text;
            runSettings.CreateOutputFolders = this.cbCreateOutputFolders.IsChecked.Value;

            if (worker != null)
            {
                worker.RunWorkerAsync(runSettings);
                lblStatus.Text = "Generating files in the background.";
            }
            else
            {
                lblStatus.Text = "Critical Error!  Files not generated.";
            }
        }

        private void Help_Click_1(object sender, RoutedEventArgs e)
        {

        }

    }
}
