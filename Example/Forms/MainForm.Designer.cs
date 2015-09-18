namespace Example
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
            this.btnConfirm = new DarkUI.DarkButton();
            this.btnCancel = new DarkUI.DarkButton();
            this.btnDisabled = new DarkUI.DarkButton();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(12, 12);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Padding = new System.Windows.Forms.Padding(5);
            this.btnConfirm.Size = new System.Drawing.Size(95, 27);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "Confirm";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(113, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(5);
            this.btnCancel.Size = new System.Drawing.Size(95, 27);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // btnDisabled
            // 
            this.btnDisabled.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDisabled.Enabled = false;
            this.btnDisabled.Location = new System.Drawing.Point(214, 12);
            this.btnDisabled.Name = "btnDisabled";
            this.btnDisabled.Padding = new System.Windows.Forms.Padding(5);
            this.btnDisabled.Size = new System.Drawing.Size(95, 27);
            this.btnDisabled.TabIndex = 2;
            this.btnDisabled.Text = "Disabled";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(331, 302);
            this.Controls.Add(this.btnDisabled);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dark UI - Example";
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.DarkButton btnConfirm;
        private DarkUI.DarkButton btnCancel;
        private DarkUI.DarkButton btnDisabled;
    }
}

