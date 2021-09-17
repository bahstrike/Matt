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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bitdepthSolid = new System.Windows.Forms.RadioButton();
            this.bitdepth1555 = new System.Windows.Forms.RadioButton();
            this.bitdepth565 = new System.Windows.Forms.RadioButton();
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
            this.bitdepth4444 = new System.Windows.Forms.RadioButton();
            this.originalNeedColormap = new System.Windows.Forms.Label();
            this.previewNeedColormap = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.colormapGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 182);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 256);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Original";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(286, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Preview";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(289, 182);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(256, 256);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bitdepthSolid);
            this.groupBox2.Controls.Add(this.bitdepth4444);
            this.groupBox2.Controls.Add(this.bitdepth1555);
            this.groupBox2.Controls.Add(this.bitdepth565);
            this.groupBox2.Controls.Add(this.bitdepth8);
            this.groupBox2.Location = new System.Drawing.Point(551, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 164);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Material Format";
            // 
            // bitdepthSolid
            // 
            this.bitdepthSolid.AutoSize = true;
            this.bitdepthSolid.Location = new System.Drawing.Point(6, 19);
            this.bitdepthSolid.Name = "bitdepthSolid";
            this.bitdepthSolid.Size = new System.Drawing.Size(136, 17);
            this.bitdepthSolid.TabIndex = 4;
            this.bitdepthSolid.Text = "Solid color w/ colormap";
            this.bitdepthSolid.UseVisualStyleBackColor = true;
            this.bitdepthSolid.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // bitdepth1555
            // 
            this.bitdepth1555.AutoSize = true;
            this.bitdepth1555.Location = new System.Drawing.Point(6, 88);
            this.bitdepth1555.Name = "bitdepth1555";
            this.bitdepth1555.Size = new System.Drawing.Size(108, 17);
            this.bitdepth1555.TabIndex = 2;
            this.bitdepth1555.Text = "16-bit 1555ARGB";
            this.bitdepth1555.UseVisualStyleBackColor = true;
            this.bitdepth1555.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // bitdepth565
            // 
            this.bitdepth565.AutoSize = true;
            this.bitdepth565.Checked = true;
            this.bitdepth565.Location = new System.Drawing.Point(6, 65);
            this.bitdepth565.Name = "bitdepth565";
            this.bitdepth565.Size = new System.Drawing.Size(95, 17);
            this.bitdepth565.TabIndex = 1;
            this.bitdepth565.TabStop = true;
            this.bitdepth565.Text = "16-bit 565RGB";
            this.bitdepth565.UseVisualStyleBackColor = true;
            this.bitdepth565.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // bitdepth8
            // 
            this.bitdepth8.AutoSize = true;
            this.bitdepth8.Location = new System.Drawing.Point(6, 42);
            this.bitdepth8.Name = "bitdepth8";
            this.bitdepth8.Size = new System.Drawing.Size(107, 17);
            this.bitdepth8.TabIndex = 0;
            this.bitdepth8.Text = "8-bit w/ colormap";
            this.bitdepth8.UseVisualStyleBackColor = true;
            this.bitdepth8.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // colormapGroup
            // 
            this.colormapGroup.BackColor = System.Drawing.SystemColors.Control;
            this.colormapGroup.Controls.Add(this.pictureBox3);
            this.colormapGroup.Controls.Add(this.label4);
            this.colormapGroup.Controls.Add(this.button2);
            this.colormapGroup.Controls.Add(this.cmpOrGobPath);
            this.colormapGroup.Controls.Add(this.label3);
            this.colormapGroup.Controls.Add(this.gobColormap);
            this.colormapGroup.Location = new System.Drawing.Point(551, 182);
            this.colormapGroup.Name = "colormapGroup";
            this.colormapGroup.Size = new System.Drawing.Size(237, 256);
            this.colormapGroup.TabIndex = 7;
            this.colormapGroup.TabStop = false;
            this.colormapGroup.Text = "Colormap";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(6, 113);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(225, 137);
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "GOB Colormap";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(156, 30);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cmpOrGobPath
            // 
            this.cmpOrGobPath.Location = new System.Drawing.Point(6, 32);
            this.cmpOrGobPath.Name = "cmpOrGobPath";
            this.cmpOrGobPath.Size = new System.Drawing.Size(144, 20);
            this.cmpOrGobPath.TabIndex = 2;
            this.cmpOrGobPath.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "*.cmp OR *.gob";
            // 
            // gobColormap
            // 
            this.gobColormap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gobColormap.Enabled = false;
            this.gobColormap.FormattingEnabled = true;
            this.gobColormap.Location = new System.Drawing.Point(6, 86);
            this.gobColormap.Name = "gobColormap";
            this.gobColormap.Size = new System.Drawing.Size(144, 21);
            this.gobColormap.TabIndex = 0;
            this.gobColormap.SelectedIndexChanged += new System.EventHandler(this.gobColormap_SelectedIndexChanged);
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(12, 12);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 8;
            this.openButton.Text = "Open...";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 41);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Save...";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // originalKeepColormap
            // 
            this.originalKeepColormap.Location = new System.Drawing.Point(12, 140);
            this.originalKeepColormap.Name = "originalKeepColormap";
            this.originalKeepColormap.Size = new System.Drawing.Size(136, 23);
            this.originalKeepColormap.TabIndex = 10;
            this.originalKeepColormap.Text = "Keep Current Colormap";
            this.originalKeepColormap.UseVisualStyleBackColor = true;
            this.originalKeepColormap.Click += new System.EventHandler(this.originalKeepColormap_Click);
            // 
            // bitdepth4444
            // 
            this.bitdepth4444.AutoSize = true;
            this.bitdepth4444.Location = new System.Drawing.Point(6, 111);
            this.bitdepth4444.Name = "bitdepth4444";
            this.bitdepth4444.Size = new System.Drawing.Size(159, 17);
            this.bitdepth4444.TabIndex = 3;
            this.bitdepth4444.Text = "16-bit 4444ARGB (Indy only)";
            this.bitdepth4444.UseVisualStyleBackColor = true;
            this.bitdepth4444.CheckedChanged += new System.EventHandler(this.format_CheckedChanged);
            // 
            // originalNeedColormap
            // 
            this.originalNeedColormap.AutoSize = true;
            this.originalNeedColormap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originalNeedColormap.ForeColor = System.Drawing.Color.DarkOrange;
            this.originalNeedColormap.Location = new System.Drawing.Point(155, 166);
            this.originalNeedColormap.Name = "originalNeedColormap";
            this.originalNeedColormap.Size = new System.Drawing.Size(113, 13);
            this.originalNeedColormap.TabIndex = 11;
            this.originalNeedColormap.Text = "NEED COLORMAP";
            this.originalNeedColormap.Visible = false;
            // 
            // previewNeedColormap
            // 
            this.previewNeedColormap.AutoSize = true;
            this.previewNeedColormap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewNeedColormap.ForeColor = System.Drawing.Color.DarkOrange;
            this.previewNeedColormap.Location = new System.Drawing.Point(432, 166);
            this.previewNeedColormap.Name = "previewNeedColormap";
            this.previewNeedColormap.Size = new System.Drawing.Size(113, 13);
            this.previewNeedColormap.TabIndex = 12;
            this.previewNeedColormap.Text = "NEED COLORMAP";
            this.previewNeedColormap.Visible = false;
            // 
            // Matt
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.previewNeedColormap);
            this.Controls.Add(this.originalNeedColormap);
            this.Controls.Add(this.originalKeepColormap);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.colormapGroup);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Matt";
            this.Text = "Matt - BAH 2021";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Matt_FormClosing);
            this.Load += new System.EventHandler(this.Matt_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Matt_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Matt_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.colormapGroup.ResumeLayout(false);
            this.colormapGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton bitdepth1555;
        private System.Windows.Forms.RadioButton bitdepth565;
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
        private System.Windows.Forms.RadioButton bitdepth4444;
        private System.Windows.Forms.Label originalNeedColormap;
        private System.Windows.Forms.Label previewNeedColormap;
    }
}

