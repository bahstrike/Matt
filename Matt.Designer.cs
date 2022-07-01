namespace Matt
{
    partial class Matt
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.abgr32 = new System.Windows.Forms.RadioButton();
            this.bgr24 = new System.Windows.Forms.RadioButton();
            this.bitdepthSolid = new System.Windows.Forms.RadioButton();
            this.abgr4444 = new System.Windows.Forms.RadioButton();
            this.abgr1555 = new System.Windows.Forms.RadioButton();
            this.bgr565 = new System.Windows.Forms.RadioButton();
            this.bitdepth8 = new System.Windows.Forms.RadioButton();
            this.colormapGroup = new System.Windows.Forms.GroupBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.cmpOrGobPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gobColormap = new System.Windows.Forms.ComboBox();
            this.openButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.originalKeepColormap = new System.Windows.Forms.Button();
            this.originalNeedColormap = new System.Windows.Forms.Label();
            this.previewNeedColormap = new System.Windows.Forms.Label();
            this.fillTransparent = new System.Windows.Forms.CheckBox();
            this.logList = new System.Windows.Forms.ListBox();
            this.autoselectFormat = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new MattControls.PictureBoxMaterial();
            this.pictureBox2 = new MattControls.PictureBoxMaterial();
            this.groupBox2.SuspendLayout();
            this.colormapGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Original";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Preview";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.abgr32);
            this.groupBox2.Controls.Add(this.bgr24);
            this.groupBox2.Controls.Add(this.bitdepthSolid);
            this.groupBox2.Controls.Add(this.abgr4444);
            this.groupBox2.Controls.Add(this.abgr1555);
            this.groupBox2.Controls.Add(this.bgr565);
            this.groupBox2.Controls.Add(this.bitdepth8);
            this.groupBox2.Location = new System.Drawing.Point(738, 13);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(316, 217);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Material Format";
            // 
            // abgr32
            // 
            this.abgr32.AutoSize = true;
            this.abgr32.Location = new System.Drawing.Point(8, 193);
            this.abgr32.Margin = new System.Windows.Forms.Padding(4);
            this.abgr32.Name = "abgr32";
            this.abgr32.Size = new System.Drawing.Size(60, 20);
            this.abgr32.TabIndex = 6;
            this.abgr32.Text = "32-bit";
            this.abgr32.UseVisualStyleBackColor = true;
            this.abgr32.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // bgr24
            // 
            this.bgr24.AutoSize = true;
            this.bgr24.Location = new System.Drawing.Point(8, 165);
            this.bgr24.Margin = new System.Windows.Forms.Padding(4);
            this.bgr24.Name = "bgr24";
            this.bgr24.Size = new System.Drawing.Size(60, 20);
            this.bgr24.TabIndex = 5;
            this.bgr24.Text = "24-bit";
            this.bgr24.UseVisualStyleBackColor = true;
            this.bgr24.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // bitdepthSolid
            // 
            this.bitdepthSolid.AutoSize = true;
            this.bitdepthSolid.Location = new System.Drawing.Point(8, 23);
            this.bitdepthSolid.Margin = new System.Windows.Forms.Padding(4);
            this.bitdepthSolid.Name = "bitdepthSolid";
            this.bitdepthSolid.Size = new System.Drawing.Size(168, 20);
            this.bitdepthSolid.TabIndex = 4;
            this.bitdepthSolid.Text = "Solid color w/ colormap";
            this.bitdepthSolid.UseVisualStyleBackColor = true;
            this.bitdepthSolid.Visible = false;
            this.bitdepthSolid.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // abgr4444
            // 
            this.abgr4444.AutoSize = true;
            this.abgr4444.Location = new System.Drawing.Point(8, 137);
            this.abgr4444.Margin = new System.Windows.Forms.Padding(4);
            this.abgr4444.Name = "abgr4444";
            this.abgr4444.Size = new System.Drawing.Size(129, 20);
            this.abgr4444.TabIndex = 3;
            this.abgr4444.Text = "16-bit 4444ABGR";
            this.abgr4444.UseVisualStyleBackColor = true;
            this.abgr4444.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // abgr1555
            // 
            this.abgr1555.AutoSize = true;
            this.abgr1555.Location = new System.Drawing.Point(8, 108);
            this.abgr1555.Margin = new System.Windows.Forms.Padding(4);
            this.abgr1555.Name = "abgr1555";
            this.abgr1555.Size = new System.Drawing.Size(129, 20);
            this.abgr1555.TabIndex = 2;
            this.abgr1555.Text = "16-bit 1555ABGR";
            this.abgr1555.UseVisualStyleBackColor = true;
            this.abgr1555.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // bgr565
            // 
            this.bgr565.AutoSize = true;
            this.bgr565.Checked = true;
            this.bgr565.Location = new System.Drawing.Point(8, 80);
            this.bgr565.Margin = new System.Windows.Forms.Padding(4);
            this.bgr565.Name = "bgr565";
            this.bgr565.Size = new System.Drawing.Size(113, 20);
            this.bgr565.TabIndex = 1;
            this.bgr565.TabStop = true;
            this.bgr565.Text = "16-bit 565BGR";
            this.bgr565.UseVisualStyleBackColor = true;
            this.bgr565.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // bitdepth8
            // 
            this.bitdepth8.AutoSize = true;
            this.bitdepth8.Location = new System.Drawing.Point(8, 52);
            this.bitdepth8.Margin = new System.Windows.Forms.Padding(4);
            this.bitdepth8.Name = "bitdepth8";
            this.bitdepth8.Size = new System.Drawing.Size(129, 20);
            this.bitdepth8.TabIndex = 0;
            this.bitdepth8.Text = "8-bit w/ colormap";
            this.bitdepth8.UseVisualStyleBackColor = true;
            this.bitdepth8.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // colormapGroup
            // 
            this.colormapGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.colormapGroup.BackColor = System.Drawing.SystemColors.Control;
            this.colormapGroup.Controls.Add(this.pictureBox3);
            this.colormapGroup.Controls.Add(this.label4);
            this.colormapGroup.Controls.Add(this.button2);
            this.colormapGroup.Controls.Add(this.cmpOrGobPath);
            this.colormapGroup.Controls.Add(this.label3);
            this.colormapGroup.Controls.Add(this.gobColormap);
            this.colormapGroup.Location = new System.Drawing.Point(735, 238);
            this.colormapGroup.Margin = new System.Windows.Forms.Padding(4);
            this.colormapGroup.Name = "colormapGroup";
            this.colormapGroup.Padding = new System.Windows.Forms.Padding(4);
            this.colormapGroup.Size = new System.Drawing.Size(316, 313);
            this.colormapGroup.TabIndex = 7;
            this.colormapGroup.TabStop = false;
            this.colormapGroup.Text = "Colormap";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(8, 130);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(301, 176);
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Paint += new System.Windows.Forms.PaintEventHandler(this.colormap_Paint);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 79);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "GOB Colormap";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(208, 37);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 26);
            this.button2.TabIndex = 3;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cmpOrGobPath
            // 
            this.cmpOrGobPath.Location = new System.Drawing.Point(8, 39);
            this.cmpOrGobPath.Margin = new System.Windows.Forms.Padding(4);
            this.cmpOrGobPath.Name = "cmpOrGobPath";
            this.cmpOrGobPath.Size = new System.Drawing.Size(191, 22);
            this.cmpOrGobPath.TabIndex = 2;
            this.cmpOrGobPath.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "*.cmp OR *.gob";
            // 
            // gobColormap
            // 
            this.gobColormap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gobColormap.Enabled = false;
            this.gobColormap.FormattingEnabled = true;
            this.gobColormap.Location = new System.Drawing.Point(8, 99);
            this.gobColormap.Margin = new System.Windows.Forms.Padding(4);
            this.gobColormap.Name = "gobColormap";
            this.gobColormap.Size = new System.Drawing.Size(191, 24);
            this.gobColormap.TabIndex = 0;
            this.gobColormap.SelectedIndexChanged += new System.EventHandler(this.gobColormap_SelectedIndexChanged);
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(16, 15);
            this.openButton.Margin = new System.Windows.Forms.Padding(4);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(100, 28);
            this.openButton.TabIndex = 8;
            this.openButton.Text = "Open...";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(16, 50);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 28);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Save...";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // originalKeepColormap
            // 
            this.originalKeepColormap.Location = new System.Drawing.Point(16, 172);
            this.originalKeepColormap.Margin = new System.Windows.Forms.Padding(4);
            this.originalKeepColormap.Name = "originalKeepColormap";
            this.originalKeepColormap.Size = new System.Drawing.Size(181, 28);
            this.originalKeepColormap.TabIndex = 10;
            this.originalKeepColormap.Text = "Keep Current Colormap";
            this.originalKeepColormap.UseVisualStyleBackColor = true;
            this.originalKeepColormap.Click += new System.EventHandler(this.originalKeepColormap_Click);
            // 
            // originalNeedColormap
            // 
            this.originalNeedColormap.AutoSize = true;
            this.originalNeedColormap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originalNeedColormap.ForeColor = System.Drawing.Color.DarkOrange;
            this.originalNeedColormap.Location = new System.Drawing.Point(193, 5);
            this.originalNeedColormap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.originalNeedColormap.Name = "originalNeedColormap";
            this.originalNeedColormap.Size = new System.Drawing.Size(141, 17);
            this.originalNeedColormap.TabIndex = 11;
            this.originalNeedColormap.Text = "NEED COLORMAP";
            this.originalNeedColormap.Visible = false;
            // 
            // previewNeedColormap
            // 
            this.previewNeedColormap.AutoSize = true;
            this.previewNeedColormap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewNeedColormap.ForeColor = System.Drawing.Color.DarkOrange;
            this.previewNeedColormap.Location = new System.Drawing.Point(203, 5);
            this.previewNeedColormap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.previewNeedColormap.Name = "previewNeedColormap";
            this.previewNeedColormap.Size = new System.Drawing.Size(141, 17);
            this.previewNeedColormap.TabIndex = 12;
            this.previewNeedColormap.Text = "NEED COLORMAP";
            this.previewNeedColormap.Visible = false;
            // 
            // fillTransparent
            // 
            this.fillTransparent.AutoSize = true;
            this.fillTransparent.Checked = true;
            this.fillTransparent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fillTransparent.Location = new System.Drawing.Point(449, 22);
            this.fillTransparent.Margin = new System.Windows.Forms.Padding(4);
            this.fillTransparent.Name = "fillTransparent";
            this.fillTransparent.Size = new System.Drawing.Size(195, 20);
            this.fillTransparent.TabIndex = 13;
            this.fillTransparent.Text = "Show transparent as fuchsia";
            this.fillTransparent.UseVisualStyleBackColor = true;
            this.fillTransparent.CheckedChanged += new System.EventHandler(this.fillTransparent_CheckedChanged);
            // 
            // logList
            // 
            this.logList.FormattingEnabled = true;
            this.logList.ItemHeight = 16;
            this.logList.Location = new System.Drawing.Point(152, 15);
            this.logList.Margin = new System.Windows.Forms.Padding(4);
            this.logList.Name = "logList";
            this.logList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.logList.Size = new System.Drawing.Size(288, 148);
            this.logList.TabIndex = 14;
            // 
            // autoselectFormat
            // 
            this.autoselectFormat.AutoSize = true;
            this.autoselectFormat.Checked = true;
            this.autoselectFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoselectFormat.Location = new System.Drawing.Point(449, 50);
            this.autoselectFormat.Margin = new System.Windows.Forms.Padding(4);
            this.autoselectFormat.Name = "autoselectFormat";
            this.autoselectFormat.Size = new System.Drawing.Size(264, 20);
            this.autoselectFormat.TabIndex = 15;
            this.autoselectFormat.Text = "Autoselect format based on input image";
            this.autoselectFormat.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(16, 208);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel1.Controls.Add(this.originalNeedColormap);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox2);
            this.splitContainer1.Panel2.Controls.Add(this.previewNeedColormap);
            this.splitContainer1.Size = new System.Drawing.Size(711, 343);
            this.splitContainer1.SplitterDistance = 354;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 16;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(6, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(345, 310);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(6, 30);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(342, 310);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // Matt
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 566);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.autoselectFormat);
            this.Controls.Add(this.logList);
            this.Controls.Add(this.fillTransparent);
            this.Controls.Add(this.originalKeepColormap);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.colormapGroup);
            this.Controls.Add(this.groupBox2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Matt";
            this.Text = "Matt - BAH 2021";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Matt_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Matt_FormClosed);
            this.Load += new System.EventHandler(this.Matt_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Matt_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Matt_DragEnter);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.colormapGroup.ResumeLayout(false);
            this.colormapGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MattControls.PictureBoxMaterial pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private MattControls.PictureBoxMaterial pictureBox2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton abgr1555;
        private System.Windows.Forms.RadioButton bgr565;
        private System.Windows.Forms.RadioButton bitdepth8;
        private System.Windows.Forms.GroupBox colormapGroup;
        private System.Windows.Forms.ComboBox gobColormap;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox cmpOrGobPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.RadioButton bitdepthSolid;
        private System.Windows.Forms.Button originalKeepColormap;
        private System.Windows.Forms.RadioButton abgr4444;
        private System.Windows.Forms.Label originalNeedColormap;
        private System.Windows.Forms.Label previewNeedColormap;
        private System.Windows.Forms.CheckBox fillTransparent;
        public System.Windows.Forms.ListBox logList;
        private System.Windows.Forms.CheckBox autoselectFormat;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RadioButton abgr32;
        private System.Windows.Forms.RadioButton bgr24;
    }
}

