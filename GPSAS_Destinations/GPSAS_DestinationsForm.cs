using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPSAS_Destinations
{
    public partial class GPSAS_DestinationsForm : Form
    {
        #region GPSAS_DestinationsForm Variables  

        // Variables
        private List<String> selectedFiles;
        private Thread workerThread;
        private String workingDir;

        // lock objects
        private Object _lockWorkindDir = new Object();

        // Thread safe variables
        private String OutputDirectory
        {
            get
            {
                lock (_lockWorkindDir)
                {
                    return workingDir;
                }
            }
            set
            {
                lock (_lockWorkindDir)
                {
                    workingDir = value;
                }
            }
        }

        // Delegates
        private delegate void SetProgressBarMaxCallback(ProgressBar p, int max);
        private delegate void SetProgressBarCallback(ProgressBar p, int percetDone);
        private delegate void SetProgressBarParseCallback(int percetDone);
        private delegate void SetStatusTextCallback(String text);
        private delegate void SetButtonEnabledCallback(Button b, Boolean enabled);
        private delegate void SetGroupBoxEnabledCallback(GroupBox gb, Boolean enabled);

        #endregion

        #region GPSAS_DestinationsForm Callbacks  

        // Callbacks
        public void SetProgressBarMax(ProgressBar p, int max)
        {
            if (p.InvokeRequired)
            {
                SetProgressBarMaxCallback d = new SetProgressBarMaxCallback(UpdateProgressBar);
                this.Invoke(d, new Object[] { p, max });
            }
            else
            {
                p.Maximum = max;
            }
        }
        public void UpdateProgressBar(ProgressBar p, int percetDone)
        {
            if (p.InvokeRequired)
            {
                SetProgressBarCallback d = new SetProgressBarCallback(UpdateProgressBar);
                this.Invoke(d, new Object[] { p, percetDone });
            }
            else
            {
                p.Value = percetDone;
            }
        }
        public void UpdateProgressBarParse(int percetDone)
        {
            if (progressBar_parse.InvokeRequired)
            {
                SetProgressBarParseCallback d = new SetProgressBarParseCallback(UpdateProgressBarParse);
                this.Invoke(d, new Object[] { percetDone });
            }
            else
            {
                progressBar_parse.Value = percetDone;
            }
        }
        public void SetStatusText(String text)
        {
            if (label_status.InvokeRequired)
            {
                SetStatusTextCallback d = new SetStatusTextCallback(SetStatusText);
                this.Invoke(d, new Object[] { text });
            }
            else
            {
                label_status.Text = text;
            }
        }
        public void SetGroupBoxEnabled(GroupBox gb, Boolean enabled)
        {
            if (gb.InvokeRequired)
            {
                SetGroupBoxEnabledCallback d = new SetGroupBoxEnabledCallback(SetGroupBoxEnabled);
                this.Invoke(d, new Object[] { gb, enabled });
            }
            else
            {
                gb.Enabled = enabled;
            }
        }
        public void SetButtonEnabled(Button b, Boolean enabled)
        {
            if (b.InvokeRequired)
            {
                SetButtonEnabledCallback d = new SetButtonEnabledCallback(SetButtonEnabled);
                this.Invoke(d, new Object[] { b, enabled });
            }
            else
            {
                b.Enabled = enabled;
            }
        }

        #endregion 

        #region GPSAS_DestinationsForm Init and Form Control
        /// <summary>
        /// Constuctor.
        /// </summary>
        public GPSAS_DestinationsForm()
        {
            InitializeComponent();
            InitializeOpenFileDialog();

            loadOutputDirectory();
            loadSettings();
        }

        /// <summary>
        /// Initializes the Open File dialog component
        /// </summary>
        private void InitializeOpenFileDialog()
        {
            this.openFileDialog.Filter =
                "Excel Files *.xlsx|*.XLSX";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.Title = "Select Files";
        }

        /// <summary>
        /// Called when parsing is started to disable form components
        /// </summary>
        private void parsingStarted()
        {
            SetGroupBoxEnabled(groupBox_params, false);
            SetButtonEnabled(button_save_dir, false);
            SetButtonEnabled(button_open, false);
            SetButtonEnabled(buttonSelectFiles, false);
            SetButtonEnabled(button_stop, true);
            UpdateProgressBar(progressBar_parse, 0);
            UpdateProgressBar(progressBar_overall, 0);

        }

        /// <summary>
        /// Called when parsing is stopped to enable form components
        /// </summary>
        private void parsingStopped()
        {
            // Save logs when stopped
            Logger.Save();
            // Re-enable GUI
            SetGroupBoxEnabled(groupBox_params, true);
            SetButtonEnabled(buttonSelectFiles, true);
            SetButtonEnabled(button_save_dir, true);
            SetButtonEnabled(button_open, true);
            SetButtonEnabled(button_stop, false);
            UpdateProgressBar(progressBar_parse, 0);
            UpdateProgressBar(progressBar_overall, 0);
            this.SetStatusText("Waiting for user input");
        }

        public void updateFileListView()
        {
            this.listBoxSelectedFiles.Items.Clear();

            foreach (String file in selectedFiles)
                this.listBoxSelectedFiles.Items.Add(file.Substring(file.LastIndexOf('\\')));

            // Enable parse tool if files are selected
            if (selectedFiles.Count > 0)
                this.button_open.Enabled = true;
            else
                this.button_open.Enabled = false;
        }

        #endregion  

        #region GPSAS_DestinationsForm Settings 

        /// <summary>
        /// Loads and sets the value for the output directory.
        /// </summary>
        private void loadOutputDirectory()
        {
            try
            {
                String value = Properties.Settings.Default.saveDir;
                // Return if no directory saved
                if (String.IsNullOrEmpty(value))
                    return;
                // Verify saved value is valid
                if (!isDirectoryValid(value))
                    throw new Exception();
                // Set text box value
                textBox_out.Text = value;
                OutputDirectory = value;
            }
            catch {  MessageBox.Show("Unable to load saved output dorectory.", "Error"); }
        }

        /// <summary>
        /// Loads the users saved settings.
        /// </summary>
        private void loadSettings()
        {
            try
            {
                // Check radio button
                if (Properties.Settings.Default.Haversine)
                    radioButton_haversine.Checked = true;
                else
                    radioButton_dist.Checked = true;
                // Set numeric up downs
                numericUpDown_dist.Value = Properties.Settings.Default.DeltaDist;
                numericUpDown_time.Value = Properties.Settings.Default.DeltaTime;
                numericUpDown_pts.Value = Properties.Settings.Default.MinPoints;
            }
            catch {  MessageBox.Show("Unable to load saved parameters.", "Error"); }
        }

        /// <summary>
        /// Saves the users parameters.
        /// </summary>
        private void saveSettings()
        {
            Properties.Settings.Default.Haversine = radioButton_haversine.Checked;
            Properties.Settings.Default.DeltaDist = numericUpDown_dist.Value;
            Properties.Settings.Default.DeltaTime = numericUpDown_time.Value;
            Properties.Settings.Default.MinPoints = numericUpDown_pts.Value;
            Properties.Settings.Default.Save();
            MessageBox.Show("Parameters saved.", "Notice");
        }

        /// <summary>
        /// Sets the output directory.
        /// </summary>
        private void setSaveDirectory()
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                string path = diag.SelectedPath;
                if (!isDirectoryValid(path))
                {
                    textBox_out.Text = String.Empty;
                    MessageBox.Show("The provided save directory was not valid. Please choose again.", "Error");
                    return;
                }
                textBox_out.Text = path;
                Properties.Settings.Default.saveDir = textBox_out.Text;
                Properties.Settings.Default.Save();
                OutputDirectory = path;
            }
        }

        #endregion

        #region GPSAS_DestinationsForm Utility 

        /// <summary>
        /// Checks if the provided directory is valid.
        /// </summary>
        /// <param name="dir">The directory of interest.</param>
        /// <returns>True if valid, false otherwise.</returns>
        private Boolean isDirectoryValid(String dir)
        {
            if (Directory.Exists(dir))
                return true;
            return false;
        }

        #endregion  

        #region GPSAS_DestinationsForm ClusterComputer  

        /// <summary>
        /// Updates the variables for the cluster computer
        /// </summary>
        private void setClusterComputerValues()
        {
            // Set distance formula
            if (radioButton_haversine.Checked)
                ClusterComputer.HaversineOn = true;
            else
                ClusterComputer.HaversineOn = false;
            // Set parameters
            ClusterComputer.DELTA_DIST_THRESHOLD = (Double)numericUpDown_dist.Value;
            ClusterComputer.DELTA_TIME_THRESHOLD = (Double)numericUpDown_time.Value;
            ClusterComputer.MINPTS = (int)numericUpDown_pts.Value;
        }

        /// <summary>
        /// Shows a dialog for a user to select a file, attempts to read file, starts parser if able.
        /// </summary>
        private void startClusterComputer()
        {
            setClusterComputerValues();
            parsingStarted();
            int numFiles = selectedFiles.Count * 2;
            progressBar_overall.Maximum = numFiles;
            // Start worker thread
            workerThread = new Thread(new ThreadStart(_workerThread));
            workerThread.Start();
        }

        /// <summary>
        /// Displays an async message box so that the program may continue
        /// </summary>
        /// <param name="message"></param>
        private void showAsyncMessagebox(String message, String title)
        {
            Thread myThread = new Thread(delegate ()
            {
                MessageBox.Show(message, title);
            });
            myThread.Start();
        }

        /// <summary>
        /// Aborts the cluter computer process.
        /// </summary>
        private void stopClusterComputer()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to stop the parser?", "Confirm Stop", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    workerThread.Abort();
                    parsingStopped();
                }
                catch  { }
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        /// <summary>
        /// Asyc process that is spawned on worker thread.
        /// </summary>
        private void _workerThread()
        {
            int currentCount = 0;

            String workingDirectory = OutputDirectory + "\\Results_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            Directory.CreateDirectory(workingDirectory);

            // Set logger
            Logger.Initialize(workingDirectory);

            // Read data form and cluster data from each selected file
            foreach (String file in selectedFiles)
            {
                progressBar_overall.Value = currentCount;
                String fileName = file.Substring(file.LastIndexOf('\\')).Replace("\\","");
                Boolean loaded = false;
                try
                {
                    Logger.Log("Reading data from file: " + fileName);
                    // Attempt to read excel data
                    ExcelManager.ReadData(this, file);
                    loaded = true;
                }
                catch (Exception ex)
                {
                    Logger.Log("Unable to read data from file: " + fileName);
                    Logger.Log(ex.ToString());
                    Logger.Log(ex.StackTrace);
                    if (ex is IOException)
                        showAsyncMessagebox("Unable to open file. Make sure the file is not open: " + fileName, "Error");
                    else if (ex is ExcelManager.ExcelParseExceptin)
                        showAsyncMessagebox("Unable to find the necessary columns in the provided data file: " + fileName, "Error");
                    else
                        showAsyncMessagebox("Unable to load data from file: " + fileName, "Error");
                }
                currentCount++;
                UpdateProgressBar(progressBar_overall, currentCount);

                // If the data was properly loaded
                if (loaded)
                {
                    Logger.Log("Computing clusters for: " + fileName);
                    ClusterComputer.Start(this, workingDirectory, fileName);
                }
                currentCount++;
                UpdateProgressBar(progressBar_overall, currentCount);
            }
            SetButtonEnabled(button_stop, false);
            MessageBox.Show("All files parsed.", "Info");
            parsingStopped();
        }

        #endregion 

        #region GPSAS_DestinationsForm Form Events  

        /// <summary>
        /// On Open button click. Starts cluster computer.
        /// </summary>
        private void button_open_Click(object sender, EventArgs e)
        {
            // Verify that an output directory has been set.
            if (String.IsNullOrEmpty(textBox_out.Text))
            {
                MessageBox.Show("Please select a directory to save results to.", "Notice");
                return;
            }

            // Verify that the provided directory is valid
            if (!isDirectoryValid(textBox_out.Text))
            {
                textBox_out.Text = String.Empty;
                MessageBox.Show("The provided save directory was not valid. Please choose again.", "Error");
                return;
            }

            // Verify that files are selected
            if (selectedFiles.Count <= 0)
            {
                MessageBox.Show("You must select at least one file to parse.", "Error");
                return;
            }

            // Start the cluster computations
            startClusterComputer();
        }

        private void button_save_dir_Click(object sender, EventArgs e)
        {
            setSaveDirectory();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            setClusterComputerValues();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            setClusterComputerValues();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            setClusterComputerValues();
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            stopClusterComputer();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            setClusterComputerValues();
        }

        private void radioButton_haversine_CheckedChanged(object sender, EventArgs e)
        {
            setClusterComputerValues();
        }

        private void button_saveparams_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void buttonSelectFiles_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                selectedFiles = new List<String>();
                // Read the files
                foreach (String file in openFileDialog.FileNames)
                {
                    try
                    {
                        selectedFiles.Add(file);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to laod file: " + file.Substring(file.LastIndexOf('\\')) + ". You may not have permission to read the file, or it may be corrupt.\n\nReported error: " + ex.Message, "Error");
                    }
                }
                updateFileListView();
            }
        }

        #endregion

    }
}
