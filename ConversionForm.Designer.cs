namespace KVSurfaceUpdater
{
    partial class ConversionForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InputPanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.newFilePrefixPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.filePrefixBox = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.newFilePrefixPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // InputPanel
            // 
            this.InputPanel.AutoSize = true;
            this.InputPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.InputPanel.Location = new System.Drawing.Point(3, 3);
            this.InputPanel.Name = "InputPanel";
            this.InputPanel.Size = new System.Drawing.Size(0, 0);
            this.InputPanel.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.newFilePrefixPanel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(334, 40);
            this.flowLayoutPanel1.TabIndex = 5;
            this.flowLayoutPanel1.WrapContents = false;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // newFilePrefixPanel
            // 
            this.newFilePrefixPanel.Controls.Add(this.filePrefixBox);
            this.newFilePrefixPanel.Controls.Add(this.label1);
            this.newFilePrefixPanel.Location = new System.Drawing.Point(3, 3);
            this.newFilePrefixPanel.Name = "newFilePrefixPanel";
            this.newFilePrefixPanel.Size = new System.Drawing.Size(328, 34);
            this.newFilePrefixPanel.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "New File Prefix";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // filePrefixBox
            // 
            this.filePrefixBox.Location = new System.Drawing.Point(94, 3);
            this.filePrefixBox.Name = "filePrefixBox";
            this.filePrefixBox.Size = new System.Drawing.Size(231, 23);
            this.filePrefixBox.TabIndex = 1;
            // 
            // ConversionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.InputPanel);
            this.Name = "ConversionForm";
            this.Size = new System.Drawing.Size(334, 40);
            this.Load += new System.EventHandler(this.ConversionForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.newFilePrefixPanel.ResumeLayout(false);
            this.newFilePrefixPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Panel InputPanel;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel newFilePrefixPanel;
        private TextBox filePrefixBox;
        private Label label1;
    }
}
