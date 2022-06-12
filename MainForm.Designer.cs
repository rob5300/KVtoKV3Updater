namespace KVSurfaceUpdater
{
    partial class MainForm
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
            this.General = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.surfacePage = new System.Windows.Forms.TabPage();
            this.soundPage = new System.Windows.Forms.TabPage();
            this.convertButton = new System.Windows.Forms.Button();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.General.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // General
            // 
            this.General.AutoSize = true;
            this.General.Controls.Add(this.button1);
            this.General.Controls.Add(this.textBox1);
            this.General.Controls.Add(this.label1);
            this.General.Dock = System.Windows.Forms.DockStyle.Top;
            this.General.Location = new System.Drawing.Point(0, 0);
            this.General.Name = "General";
            this.General.Size = new System.Drawing.Size(464, 64);
            this.General.TabIndex = 0;
            this.General.TabStop = false;
            this.General.Text = "General";
            this.General.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Folder";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(89, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.PlaceholderText = "Path here";
            this.textBox1.Size = new System.Drawing.Size(288, 23);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(383, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.surfacePage);
            this.tabControl1.Controls.Add(this.soundPage);
            this.tabControl1.Location = new System.Drawing.Point(8, 70);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(450, 250);
            this.tabControl1.TabIndex = 1;
            // 
            // surfacePage
            // 
            this.surfacePage.Location = new System.Drawing.Point(4, 24);
            this.surfacePage.Name = "surfacePage";
            this.surfacePage.Padding = new System.Windows.Forms.Padding(3);
            this.surfacePage.Size = new System.Drawing.Size(442, 222);
            this.surfacePage.TabIndex = 0;
            this.surfacePage.Text = "Surfaces";
            this.surfacePage.UseVisualStyleBackColor = true;
            // 
            // soundPage
            // 
            this.soundPage.Location = new System.Drawing.Point(4, 24);
            this.soundPage.Name = "soundPage";
            this.soundPage.Padding = new System.Windows.Forms.Padding(3);
            this.soundPage.Size = new System.Drawing.Size(352, 245);
            this.soundPage.TabIndex = 1;
            this.soundPage.Text = "Sounds";
            this.soundPage.UseVisualStyleBackColor = true;
            // 
            // convertButton
            // 
            this.convertButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.convertButton.Location = new System.Drawing.Point(0, 352);
            this.convertButton.Name = "convertButton";
            this.convertButton.Size = new System.Drawing.Size(464, 46);
            this.convertButton.TabIndex = 2;
            this.convertButton.Text = "Convert";
            this.convertButton.UseVisualStyleBackColor = true;
            this.convertButton.Click += new System.EventHandler(this.convertButton_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.progressBar1);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 326);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(464, 26);
            this.bottomPanel.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(3, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(458, 19);
            this.progressBar1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(464, 398);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.General);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.convertButton);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.General.ResumeLayout(false);
            this.General.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox General;
        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private TabControl tabControl1;
        private TabPage surfacePage;
        private TabPage soundPage;
        private Button convertButton;
        private Panel bottomPanel;
        private ProgressBar progressBar1;
    }
}