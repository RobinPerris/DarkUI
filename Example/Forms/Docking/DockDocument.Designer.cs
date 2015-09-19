namespace Example
{
    partial class DockDocument
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
            this.txtDocument = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtDocument
            // 
            this.txtDocument.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.txtDocument.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDocument.ForeColor = System.Drawing.Color.Gainsboro;
            this.txtDocument.Location = new System.Drawing.Point(0, 0);
            this.txtDocument.Multiline = true;
            this.txtDocument.Name = "txtDocument";
            this.txtDocument.Size = new System.Drawing.Size(175, 173);
            this.txtDocument.TabIndex = 1;
            this.txtDocument.Text = "This is some example text";
            // 
            // DockDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDocument);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DockDocument";
            this.Size = new System.Drawing.Size(175, 173);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDocument;
    }
}
