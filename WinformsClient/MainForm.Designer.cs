namespace WinformsClient
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
            this.lstInterfaces = new System.Windows.Forms.ListBox();
            this.txtCode = new System.Windows.Forms.RichTextBox();
            this.formContainer = new System.Windows.Forms.SplitContainer();
            this.butClose = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.butFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.formContainer)).BeginInit();
            this.formContainer.Panel1.SuspendLayout();
            this.formContainer.Panel2.SuspendLayout();
            this.formContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstInterfaces
            // 
            this.lstInterfaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstInterfaces.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lstInterfaces.FormattingEnabled = true;
            this.lstInterfaces.IntegralHeight = false;
            this.lstInterfaces.ItemHeight = 16;
            this.lstInterfaces.Location = new System.Drawing.Point(0, 0);
            this.lstInterfaces.Name = "lstInterfaces";
            this.lstInterfaces.Size = new System.Drawing.Size(295, 472);
            this.lstInterfaces.TabIndex = 0;
            this.lstInterfaces.SelectedIndexChanged += new System.EventHandler(this.SelectedInterfaceChanged);
            // 
            // txtCode
            // 
            this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtCode.Location = new System.Drawing.Point(0, 0);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(590, 472);
            this.txtCode.TabIndex = 1;
            this.txtCode.Text = "";
            // 
            // formContainer
            // 
            this.formContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formContainer.Location = new System.Drawing.Point(3, 31);
            this.formContainer.Name = "formContainer";
            // 
            // formContainer.Panel1
            // 
            this.formContainer.Panel1.Controls.Add(this.lstInterfaces);
            // 
            // formContainer.Panel2
            // 
            this.formContainer.Panel2.Controls.Add(this.txtCode);
            this.formContainer.Size = new System.Drawing.Size(887, 472);
            this.formContainer.SplitterDistance = 295;
            this.formContainer.SplitterWidth = 2;
            this.formContainer.TabIndex = 2;
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butClose.Location = new System.Drawing.Point(815, 509);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(75, 23);
            this.butClose.TabIndex = 3;
            this.butClose.Text = "Close";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.CloseClick);
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFile.Location = new System.Drawing.Point(0, 5);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(142, 16);
            this.lblFile.TabIndex = 4;
            this.lblFile.Text = "Select .NET assembly";
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(148, 5);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(704, 20);
            this.txtFile.TabIndex = 5;
            // 
            // butFile
            // 
            this.butFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butFile.Location = new System.Drawing.Point(858, 5);
            this.butFile.Name = "butFile";
            this.butFile.Size = new System.Drawing.Size(32, 20);
            this.butFile.TabIndex = 6;
            this.butFile.Text = "...";
            this.butFile.UseVisualStyleBackColor = true;
            this.butFile.Click += new System.EventHandler(this.OpenFileClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 536);
            this.Controls.Add(this.butFile);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.formContainer);
            this.MinimumSize = new System.Drawing.Size(480, 320);
            this.Name = "MainForm";
            this.Text = "Interface Implementor";
            this.formContainer.Panel1.ResumeLayout(false);
            this.formContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.formContainer)).EndInit();
            this.formContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstInterfaces;
        private System.Windows.Forms.RichTextBox txtCode;
        private System.Windows.Forms.SplitContainer formContainer;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button butFile;
    }
}

