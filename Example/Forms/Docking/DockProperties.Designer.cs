using DarkUI.Config;

namespace Example
{
    partial class DockProperties
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.darkCheckBox1 = new DarkUI.Controls.DarkCheckBox();
            this.darkCheckBox2 = new DarkUI.Controls.DarkCheckBox();
            this.darkCheckBox3 = new DarkUI.Controls.DarkCheckBox();
            this.darkRadioButton1 = new DarkUI.Controls.DarkRadioButton();
            this.darkRadioButton2 = new DarkUI.Controls.DarkRadioButton();
            this.darkRadioButton3 = new DarkUI.Controls.DarkRadioButton();
            this.pnlMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 25);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMain.Size = new System.Drawing.Size(280, 425);
            this.pnlMain.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.darkRadioButton3);
            this.panel1.Controls.Add(this.darkRadioButton2);
            this.panel1.Controls.Add(this.darkRadioButton1);
            this.panel1.Controls.Add(this.darkCheckBox3);
            this.panel1.Controls.Add(this.darkCheckBox2);
            this.panel1.Controls.Add(this.darkCheckBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.panel1.Size = new System.Drawing.Size(260, 152);
            this.panel1.TabIndex = 0;
            // 
            // darkCheckBox1
            // 
            this.darkCheckBox1.AutoSize = true;
            this.darkCheckBox1.Location = new System.Drawing.Point(0, 0);
            this.darkCheckBox1.Name = "darkCheckBox1";
            this.darkCheckBox1.Size = new System.Drawing.Size(78, 19);
            this.darkCheckBox1.TabIndex = 1;
            this.darkCheckBox1.Text = "Checkbox";
            // 
            // darkCheckBox2
            // 
            this.darkCheckBox2.AutoSize = true;
            this.darkCheckBox2.Enabled = false;
            this.darkCheckBox2.Location = new System.Drawing.Point(0, 25);
            this.darkCheckBox2.Name = "darkCheckBox2";
            this.darkCheckBox2.Size = new System.Drawing.Size(124, 19);
            this.darkCheckBox2.TabIndex = 2;
            this.darkCheckBox2.Text = "Disabled checkbox";
            // 
            // darkCheckBox3
            // 
            this.darkCheckBox3.AutoSize = true;
            this.darkCheckBox3.Checked = true;
            this.darkCheckBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.darkCheckBox3.Enabled = false;
            this.darkCheckBox3.Location = new System.Drawing.Point(0, 50);
            this.darkCheckBox3.Name = "darkCheckBox3";
            this.darkCheckBox3.Size = new System.Drawing.Size(171, 19);
            this.darkCheckBox3.TabIndex = 3;
            this.darkCheckBox3.Text = "Disabled checked checkbox";
            // 
            // darkRadioButton1
            // 
            this.darkRadioButton1.AutoSize = true;
            this.darkRadioButton1.Location = new System.Drawing.Point(0, 75);
            this.darkRadioButton1.Name = "darkRadioButton1";
            this.darkRadioButton1.Size = new System.Drawing.Size(91, 19);
            this.darkRadioButton1.TabIndex = 4;
            this.darkRadioButton1.TabStop = true;
            this.darkRadioButton1.Text = "Radiobutton";
            // 
            // darkRadioButton2
            // 
            this.darkRadioButton2.AutoSize = true;
            this.darkRadioButton2.Location = new System.Drawing.Point(0, 100);
            this.darkRadioButton2.Name = "darkRadioButton2";
            this.darkRadioButton2.Size = new System.Drawing.Size(91, 19);
            this.darkRadioButton2.TabIndex = 5;
            this.darkRadioButton2.TabStop = true;
            this.darkRadioButton2.Text = "Radiobutton";
            // 
            // darkRadioButton3
            // 
            this.darkRadioButton3.AutoSize = true;
            this.darkRadioButton3.Enabled = false;
            this.darkRadioButton3.Location = new System.Drawing.Point(0, 125);
            this.darkRadioButton3.Name = "darkRadioButton3";
            this.darkRadioButton3.Size = new System.Drawing.Size(136, 19);
            this.darkRadioButton3.TabIndex = 6;
            this.darkRadioButton3.TabStop = true;
            this.darkRadioButton3.Text = "Disabled radiobutton";
            // 
            // DockProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.DockArea = DarkUI.Config.DarkDockArea.Right;
            this.DockText = "Properties";
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::Example.Icons.properties_16xLG;
            this.Name = "DockProperties";
            this.Size = new System.Drawing.Size(280, 450);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel panel1;
        private DarkUI.Controls.DarkCheckBox darkCheckBox1;
        private DarkUI.Controls.DarkCheckBox darkCheckBox2;
        private DarkUI.Controls.DarkCheckBox darkCheckBox3;
        private DarkUI.Controls.DarkRadioButton darkRadioButton3;
        private DarkUI.Controls.DarkRadioButton darkRadioButton2;
        private DarkUI.Controls.DarkRadioButton darkRadioButton1;
    }
}
