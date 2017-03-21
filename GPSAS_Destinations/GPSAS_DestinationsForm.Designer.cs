namespace GPSAS_Destinations
{
    partial class GPSAS_DestinationsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GPSAS_DestinationsForm));
            this.button_open = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox_params = new System.Windows.Forms.GroupBox();
            this.button_saveparams = new System.Windows.Forms.Button();
            this.groupBox_dist = new System.Windows.Forms.GroupBox();
            this.radioButton_dist = new System.Windows.Forms.RadioButton();
            this.radioButton_haversine = new System.Windows.Forms.RadioButton();
            this.numericUpDown_pts = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown_time = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown_dist = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_out = new System.Windows.Forms.TextBox();
            this.button_save_dir = new System.Windows.Forms.Button();
            this.progressBar_parse = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.button_stop = new System.Windows.Forms.Button();
            this.listBoxSelectedFiles = new System.Windows.Forms.ListBox();
            this.buttonSelectFiles = new System.Windows.Forms.Button();
            this.progressBar_overall = new System.Windows.Forms.ProgressBar();
            this.groupBox_params.SuspendLayout();
            this.groupBox_dist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_dist)).BeginInit();
            this.SuspendLayout();
            // 
            // button_open
            // 
            this.button_open.Enabled = false;
            this.button_open.Location = new System.Drawing.Point(229, 247);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(177, 23);
            this.button_open.TabIndex = 0;
            this.button_open.Text = "Parse Selected Files";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // groupBox_params
            // 
            this.groupBox_params.Controls.Add(this.button_saveparams);
            this.groupBox_params.Controls.Add(this.groupBox_dist);
            this.groupBox_params.Controls.Add(this.numericUpDown_pts);
            this.groupBox_params.Controls.Add(this.label5);
            this.groupBox_params.Controls.Add(this.numericUpDown_time);
            this.groupBox_params.Controls.Add(this.label3);
            this.groupBox_params.Controls.Add(this.label4);
            this.groupBox_params.Controls.Add(this.numericUpDown_dist);
            this.groupBox_params.Controls.Add(this.label2);
            this.groupBox_params.Controls.Add(this.label1);
            this.groupBox_params.Location = new System.Drawing.Point(229, 12);
            this.groupBox_params.Name = "groupBox_params";
            this.groupBox_params.Size = new System.Drawing.Size(245, 203);
            this.groupBox_params.TabIndex = 1;
            this.groupBox_params.TabStop = false;
            this.groupBox_params.Text = "Parameters";
            // 
            // button_saveparams
            // 
            this.button_saveparams.Location = new System.Drawing.Point(64, 168);
            this.button_saveparams.Name = "button_saveparams";
            this.button_saveparams.Size = new System.Drawing.Size(119, 23);
            this.button_saveparams.TabIndex = 16;
            this.button_saveparams.Text = "Save Parameters";
            this.button_saveparams.UseVisualStyleBackColor = true;
            this.button_saveparams.Click += new System.EventHandler(this.button_saveparams_Click);
            // 
            // groupBox_dist
            // 
            this.groupBox_dist.Controls.Add(this.radioButton_dist);
            this.groupBox_dist.Controls.Add(this.radioButton_haversine);
            this.groupBox_dist.Location = new System.Drawing.Point(9, 102);
            this.groupBox_dist.Name = "groupBox_dist";
            this.groupBox_dist.Size = new System.Drawing.Size(224, 60);
            this.groupBox_dist.TabIndex = 13;
            this.groupBox_dist.TabStop = false;
            this.groupBox_dist.Text = "Distance Calculation";
            // 
            // radioButton_dist
            // 
            this.radioButton_dist.AutoSize = true;
            this.radioButton_dist.Location = new System.Drawing.Point(7, 39);
            this.radioButton_dist.Name = "radioButton_dist";
            this.radioButton_dist.Size = new System.Drawing.Size(107, 17);
            this.radioButton_dist.TabIndex = 14;
            this.radioButton_dist.Text = "Distance Formula";
            this.radioButton_dist.UseVisualStyleBackColor = true;
            this.radioButton_dist.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton_haversine
            // 
            this.radioButton_haversine.AutoSize = true;
            this.radioButton_haversine.Checked = true;
            this.radioButton_haversine.Location = new System.Drawing.Point(7, 19);
            this.radioButton_haversine.Name = "radioButton_haversine";
            this.radioButton_haversine.Size = new System.Drawing.Size(113, 17);
            this.radioButton_haversine.TabIndex = 13;
            this.radioButton_haversine.TabStop = true;
            this.radioButton_haversine.Text = "Haversine Formula";
            this.radioButton_haversine.UseVisualStyleBackColor = true;
            this.radioButton_haversine.CheckedChanged += new System.EventHandler(this.radioButton_haversine_CheckedChanged);
            // 
            // numericUpDown_pts
            // 
            this.numericUpDown_pts.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.numericUpDown_pts.Location = new System.Drawing.Point(99, 76);
            this.numericUpDown_pts.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDown_pts.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_pts.Name = "numericUpDown_pts";
            this.numericUpDown_pts.Size = new System.Drawing.Size(89, 20);
            this.numericUpDown_pts.TabIndex = 12;
            this.numericUpDown_pts.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_pts.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // label5
            // 
            this.label5.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Min Points";
            // 
            // numericUpDown_time
            // 
            this.numericUpDown_time.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.numericUpDown_time.Location = new System.Drawing.Point(99, 51);
            this.numericUpDown_time.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDown_time.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_time.Name = "numericUpDown_time";
            this.numericUpDown_time.Size = new System.Drawing.Size(89, 20);
            this.numericUpDown_time.TabIndex = 9;
            this.numericUpDown_time.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_time.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label3
            // 
            this.label3.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(194, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Minutes";
            // 
            // label4
            // 
            this.label4.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Max Interval Time";
            // 
            // numericUpDown_dist
            // 
            this.numericUpDown_dist.Location = new System.Drawing.Point(99, 25);
            this.numericUpDown_dist.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown_dist.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_dist.Name = "numericUpDown_dist";
            this.numericUpDown_dist.Size = new System.Drawing.Size(89, 20);
            this.numericUpDown_dist.TabIndex = 6;
            this.numericUpDown_dist.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_dist.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Meters";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Max Loc. Radius";
            // 
            // textBox_out
            // 
            this.textBox_out.Enabled = false;
            this.textBox_out.Location = new System.Drawing.Point(293, 220);
            this.textBox_out.Name = "textBox_out";
            this.textBox_out.ReadOnly = true;
            this.textBox_out.Size = new System.Drawing.Size(181, 20);
            this.textBox_out.TabIndex = 2;
            // 
            // button_save_dir
            // 
            this.button_save_dir.Location = new System.Drawing.Point(229, 218);
            this.button_save_dir.Name = "button_save_dir";
            this.button_save_dir.Size = new System.Drawing.Size(58, 23);
            this.button_save_dir.TabIndex = 3;
            this.button_save_dir.Text = "Save To";
            this.button_save_dir.UseVisualStyleBackColor = true;
            this.button_save_dir.Click += new System.EventHandler(this.button_save_dir_Click);
            // 
            // progressBar_parse
            // 
            this.progressBar_parse.Location = new System.Drawing.Point(12, 303);
            this.progressBar_parse.Name = "progressBar_parse";
            this.progressBar_parse.Size = new System.Drawing.Size(462, 10);
            this.progressBar_parse.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(229, 281);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Status: ";
            // 
            // label_status
            // 
            this.label_status.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.label_status.AutoSize = true;
            this.label_status.Location = new System.Drawing.Point(278, 281);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(107, 13);
            this.label_status.TabIndex = 14;
            this.label_status.Text = "Waiting for user input";
            // 
            // button_stop
            // 
            this.button_stop.Enabled = false;
            this.button_stop.Location = new System.Drawing.Point(412, 246);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(59, 23);
            this.button_stop.TabIndex = 15;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // listBoxSelectedFiles
            // 
            this.listBoxSelectedFiles.FormattingEnabled = true;
            this.listBoxSelectedFiles.Location = new System.Drawing.Point(12, 10);
            this.listBoxSelectedFiles.Name = "listBoxSelectedFiles";
            this.listBoxSelectedFiles.Size = new System.Drawing.Size(211, 264);
            this.listBoxSelectedFiles.TabIndex = 16;
            // 
            // buttonSelectFiles
            // 
            this.buttonSelectFiles.Location = new System.Drawing.Point(12, 276);
            this.buttonSelectFiles.Name = "buttonSelectFiles";
            this.buttonSelectFiles.Size = new System.Drawing.Size(211, 22);
            this.buttonSelectFiles.TabIndex = 17;
            this.buttonSelectFiles.Text = "Select Files";
            this.buttonSelectFiles.UseVisualStyleBackColor = true;
            this.buttonSelectFiles.Click += new System.EventHandler(this.buttonSelectFiles_Click);
            // 
            // progressBar_overall
            // 
            this.progressBar_overall.Location = new System.Drawing.Point(12, 316);
            this.progressBar_overall.Name = "progressBar_overall";
            this.progressBar_overall.Size = new System.Drawing.Size(462, 10);
            this.progressBar_overall.TabIndex = 18;
            // 
            // GPSAS_DestinationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 334);
            this.Controls.Add(this.progressBar_overall);
            this.Controls.Add(this.buttonSelectFiles);
            this.Controls.Add(this.listBoxSelectedFiles);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.progressBar_parse);
            this.Controls.Add(this.button_save_dir);
            this.Controls.Add(this.textBox_out);
            this.Controls.Add(this.groupBox_params);
            this.Controls.Add(this.button_open);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(506, 372);
            this.MinimumSize = new System.Drawing.Size(506, 372);
            this.Name = "GPSAS_DestinationsForm";
            this.Text = "GPSAS_Destinations";
            this.groupBox_params.ResumeLayout(false);
            this.groupBox_params.PerformLayout();
            this.groupBox_dist.ResumeLayout(false);
            this.groupBox_dist.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_dist)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox_params;
        private System.Windows.Forms.NumericUpDown numericUpDown_time;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown_dist;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_pts;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_out;
        private System.Windows.Forms.Button button_save_dir;
        private System.Windows.Forms.ProgressBar progressBar_parse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox_dist;
        private System.Windows.Forms.RadioButton radioButton_dist;
        private System.Windows.Forms.RadioButton radioButton_haversine;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_saveparams;
        private System.Windows.Forms.ListBox listBoxSelectedFiles;
        private System.Windows.Forms.Button buttonSelectFiles;
        private System.Windows.Forms.ProgressBar progressBar_overall;
    }
}

