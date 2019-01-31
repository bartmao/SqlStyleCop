namespace SqlStyleCop
{
    partial class MainWindow
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
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtSql = new System.Windows.Forms.RichTextBox();
            this.txtLogger = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pgbHandling = new System.Windows.Forms.ProgressBar();
            this.logPanel = new System.Windows.Forms.Panel();
            this.chkMsg = new System.Windows.Forms.CheckBox();
            this.chkWarn = new System.Windows.Forms.CheckBox();
            this.chkErr = new System.Windows.Forms.CheckBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.dgvStyleCops = new System.Windows.Forms.DataGridView();
            this.TypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbSql = new System.Windows.Forms.TabControl();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExportLog = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.logPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStyleCops)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(6, 368);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(184, 149);
            this.txtDescription.TabIndex = 13;
            // 
            // txtSql
            // 
            this.txtSql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSql.Location = new System.Drawing.Point(196, 26);
            this.txtSql.Name = "txtSql";
            this.txtSql.Size = new System.Drawing.Size(542, 336);
            this.txtSql.TabIndex = 11;
            this.txtSql.Text = "";
            this.txtSql.WordWrap = false;
            this.txtSql.TextChanged += new System.EventHandler(this.txtSql_TextChanged);
            // 
            // txtLogger
            // 
            this.txtLogger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogger.Location = new System.Drawing.Point(196, 385);
            this.txtLogger.Name = "txtLogger";
            this.txtLogger.ReadOnly = true;
            this.txtLogger.Size = new System.Drawing.Size(542, 132);
            this.txtLogger.TabIndex = 10;
            this.txtLogger.Text = "";
            this.txtLogger.Click += new System.EventHandler(this.txtLogger_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Controls.Add(this.btnExportLog);
            this.panel1.Controls.Add(this.pgbHandling);
            this.panel1.Controls.Add(this.logPanel);
            this.panel1.Controls.Add(this.dgvStyleCops);
            this.panel1.Controls.Add(this.txtLogger);
            this.panel1.Controls.Add(this.txtSql);
            this.panel1.Controls.Add(this.tbSql);
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Controls.Add(this.btnImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(747, 561);
            this.panel1.TabIndex = 14;
            // 
            // pgbHandling
            // 
            this.pgbHandling.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgbHandling.Location = new System.Drawing.Point(479, 530);
            this.pgbHandling.Name = "pgbHandling";
            this.pgbHandling.Size = new System.Drawing.Size(259, 23);
            this.pgbHandling.TabIndex = 22;
            // 
            // logPanel
            // 
            this.logPanel.Controls.Add(this.chkMsg);
            this.logPanel.Controls.Add(this.chkWarn);
            this.logPanel.Controls.Add(this.chkErr);
            this.logPanel.Controls.Add(this.chkAll);
            this.logPanel.Location = new System.Drawing.Point(196, 362);
            this.logPanel.Name = "logPanel";
            this.logPanel.Size = new System.Drawing.Size(542, 19);
            this.logPanel.TabIndex = 21;
            // 
            // chkMsg
            // 
            this.chkMsg.AutoSize = true;
            this.chkMsg.Checked = true;
            this.chkMsg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMsg.Location = new System.Drawing.Point(163, 0);
            this.chkMsg.Name = "chkMsg";
            this.chkMsg.Size = new System.Drawing.Size(69, 17);
            this.chkMsg.TabIndex = 3;
            this.chkMsg.Tag = "1";
            this.chkMsg.Text = "Message";
            this.chkMsg.UseVisualStyleBackColor = true;
            this.chkMsg.Click += new System.EventHandler(this.chkAll_Click);
            // 
            // chkWarn
            // 
            this.chkWarn.AutoSize = true;
            this.chkWarn.Checked = true;
            this.chkWarn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWarn.Location = new System.Drawing.Point(101, 0);
            this.chkWarn.Name = "chkWarn";
            this.chkWarn.Size = new System.Drawing.Size(66, 17);
            this.chkWarn.TabIndex = 2;
            this.chkWarn.Tag = "2";
            this.chkWarn.Text = "Warning";
            this.chkWarn.UseVisualStyleBackColor = true;
            this.chkWarn.Click += new System.EventHandler(this.chkAll_Click);
            // 
            // chkErr
            // 
            this.chkErr.AutoSize = true;
            this.chkErr.Checked = true;
            this.chkErr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkErr.Location = new System.Drawing.Point(47, 0);
            this.chkErr.Name = "chkErr";
            this.chkErr.Size = new System.Drawing.Size(48, 17);
            this.chkErr.TabIndex = 1;
            this.chkErr.Tag = "4";
            this.chkErr.Text = "Error";
            this.chkErr.UseVisualStyleBackColor = true;
            this.chkErr.Click += new System.EventHandler(this.chkAll_Click);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Checked = true;
            this.chkAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAll.Location = new System.Drawing.Point(4, 0);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(37, 17);
            this.chkAll.TabIndex = 0;
            this.chkAll.Tag = "7";
            this.chkAll.Text = "All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.Click += new System.EventHandler(this.chkAll_Click);
            // 
            // dgvStyleCops
            // 
            this.dgvStyleCops.AllowUserToAddRows = false;
            this.dgvStyleCops.AllowUserToDeleteRows = false;
            this.dgvStyleCops.AllowUserToResizeColumns = false;
            this.dgvStyleCops.AllowUserToResizeRows = false;
            this.dgvStyleCops.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStyleCops.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStyleCops.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TypeName,
            this.IsSelected,
            this.Desc});
            this.dgvStyleCops.Location = new System.Drawing.Point(6, 3);
            this.dgvStyleCops.MultiSelect = false;
            this.dgvStyleCops.Name = "dgvStyleCops";
            this.dgvStyleCops.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStyleCops.Size = new System.Drawing.Size(184, 359);
            this.dgvStyleCops.TabIndex = 20;
            this.dgvStyleCops.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStyleCops_CellClick);
            // 
            // TypeName
            // 
            this.TypeName.HeaderText = "Cop Name";
            this.TypeName.Name = "TypeName";
            this.TypeName.ReadOnly = true;
            this.TypeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TypeName.Width = 130;
            // 
            // IsSelected
            // 
            this.IsSelected.HeaderText = "Select";
            this.IsSelected.Name = "IsSelected";
            this.IsSelected.Width = 50;
            // 
            // Desc
            // 
            this.Desc.HeaderText = "Desc";
            this.Desc.Name = "Desc";
            this.Desc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Desc.Visible = false;
            // 
            // tbSql
            // 
            this.tbSql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSql.Location = new System.Drawing.Point(196, 2);
            this.tbSql.Name = "tbSql";
            this.tbSql.SelectedIndex = 0;
            this.tbSql.Size = new System.Drawing.Size(542, 24);
            this.tbSql.TabIndex = 15;
            this.tbSql.SelectedIndexChanged += new System.EventHandler(this.tbSql_SelectedIndexChanged);
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRun.Location = new System.Drawing.Point(121, 523);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(91, 30);
            this.btnRun.TabIndex = 16;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.Location = new System.Drawing.Point(6, 523);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(109, 30);
            this.btnImport.TabIndex = 19;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExportLog
            // 
            this.btnExportLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportLog.Location = new System.Drawing.Point(218, 523);
            this.btnExportLog.Name = "btnExportLog";
            this.btnExportLog.Size = new System.Drawing.Size(91, 30);
            this.btnExportLog.TabIndex = 23;
            this.btnExportLog.Text = "Export Log";
            this.btnExportLog.UseVisualStyleBackColor = true;
            this.btnExportLog.Click += new System.EventHandler(this.btnExportLog_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 561);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(763, 599);
            this.Name = "MainWindow";
            this.Text = "SQL Style Cop";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.logPanel.ResumeLayout(false);
            this.logPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStyleCops)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.RichTextBox txtSql;
        private System.Windows.Forms.RichTextBox txtLogger;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tbSql;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.DataGridView dgvStyleCops;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Desc;
        private System.Windows.Forms.Panel logPanel;
        private System.Windows.Forms.CheckBox chkMsg;
        private System.Windows.Forms.CheckBox chkWarn;
        private System.Windows.Forms.CheckBox chkErr;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.ProgressBar pgbHandling;
        private System.Windows.Forms.Button btnExportLog;
    }
}

