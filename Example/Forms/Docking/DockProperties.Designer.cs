using DarkUI.Config;
using DarkUI.Docking;

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
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbList = new DarkUI.Controls.DarkDropdownList();
            this.darkTitle3 = new DarkUI.Controls.DarkTitle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.darkRadioButton3 = new DarkUI.Controls.DarkRadioButton();
            this.darkRadioButton2 = new DarkUI.Controls.DarkRadioButton();
            this.darkRadioButton1 = new DarkUI.Controls.DarkRadioButton();
            this.darkTitle1 = new DarkUI.Controls.DarkTitle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.darkCheckBox3 = new DarkUI.Controls.DarkCheckBox();
            this.darkCheckBox2 = new DarkUI.Controls.DarkCheckBox();
            this.darkCheckBox1 = new DarkUI.Controls.DarkCheckBox();
            this.darkTitle2 = new DarkUI.Controls.DarkTitle();
            this.darkScrollBar1 = new DarkUI.Controls.DarkScrollBar();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.cmbList);
            this.panel3.Controls.Add(this.darkTitle3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 196);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel3.Size = new System.Drawing.Size(250, 62);
            this.panel3.TabIndex = 3;
            // 
            // cmbList
            // 
            this.cmbList.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbList.Location = new System.Drawing.Point(0, 26);
            this.cmbList.Name = "cmbList";
            this.cmbList.Size = new System.Drawing.Size(250, 26);
            this.cmbList.TabIndex = 8;
            this.cmbList.Text = "darkDropdownList1";
            // 
            // darkTitle3
            // 
            this.darkTitle3.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle3.Location = new System.Drawing.Point(0, 0);
            this.darkTitle3.Name = "darkTitle3";
            this.darkTitle3.Size = new System.Drawing.Size(250, 26);
            this.darkTitle3.TabIndex = 7;
            this.darkTitle3.Text = "Lists";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.darkRadioButton3);
            this.panel1.Controls.Add(this.darkRadioButton2);
            this.panel1.Controls.Add(this.darkRadioButton1);
            this.panel1.Controls.Add(this.darkTitle1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 103);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel1.Size = new System.Drawing.Size(250, 93);
            this.panel1.TabIndex = 2;
            // 
            // darkRadioButton3
            // 
            this.darkRadioButton3.AutoSize = true;
            this.darkRadioButton3.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkRadioButton3.Enabled = false;
            this.darkRadioButton3.Location = new System.Drawing.Point(0, 64);
            this.darkRadioButton3.Name = "darkRadioButton3";
            this.darkRadioButton3.Size = new System.Drawing.Size(250, 19);
            this.darkRadioButton3.TabIndex = 6;
            this.darkRadioButton3.TabStop = true;
            this.darkRadioButton3.Text = "Disabled radiobutton";
            // 
            // darkRadioButton2
            // 
            this.darkRadioButton2.AutoSize = true;
            this.darkRadioButton2.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkRadioButton2.Location = new System.Drawing.Point(0, 45);
            this.darkRadioButton2.Name = "darkRadioButton2";
            this.darkRadioButton2.Size = new System.Drawing.Size(250, 19);
            this.darkRadioButton2.TabIndex = 5;
            this.darkRadioButton2.TabStop = true;
            this.darkRadioButton2.Text = "Radiobutton";
            // 
            // darkRadioButton1
            // 
            this.darkRadioButton1.AutoSize = true;
            this.darkRadioButton1.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkRadioButton1.Location = new System.Drawing.Point(0, 26);
            this.darkRadioButton1.Name = "darkRadioButton1";
            this.darkRadioButton1.Size = new System.Drawing.Size(250, 19);
            this.darkRadioButton1.TabIndex = 4;
            this.darkRadioButton1.TabStop = true;
            this.darkRadioButton1.Text = "Radiobutton";
            // 
            // darkTitle1
            // 
            this.darkTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle1.Location = new System.Drawing.Point(0, 0);
            this.darkTitle1.Name = "darkTitle1";
            this.darkTitle1.Size = new System.Drawing.Size(250, 26);
            this.darkTitle1.TabIndex = 7;
            this.darkTitle1.Text = "Radio buttons";
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.darkCheckBox3);
            this.panel2.Controls.Add(this.darkCheckBox2);
            this.panel2.Controls.Add(this.darkCheckBox1);
            this.panel2.Controls.Add(this.darkTitle2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 10);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel2.Size = new System.Drawing.Size(250, 93);
            this.panel2.TabIndex = 1;
            // 
            // darkCheckBox3
            // 
            this.darkCheckBox3.AutoSize = true;
            this.darkCheckBox3.Checked = true;
            this.darkCheckBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.darkCheckBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkCheckBox3.Enabled = false;
            this.darkCheckBox3.Location = new System.Drawing.Point(0, 64);
            this.darkCheckBox3.Name = "darkCheckBox3";
            this.darkCheckBox3.Size = new System.Drawing.Size(250, 19);
            this.darkCheckBox3.TabIndex = 6;
            this.darkCheckBox3.Text = "Disabled checked checkbox";
            // 
            // darkCheckBox2
            // 
            this.darkCheckBox2.AutoSize = true;
            this.darkCheckBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkCheckBox2.Enabled = false;
            this.darkCheckBox2.Location = new System.Drawing.Point(0, 45);
            this.darkCheckBox2.Name = "darkCheckBox2";
            this.darkCheckBox2.Size = new System.Drawing.Size(250, 19);
            this.darkCheckBox2.TabIndex = 5;
            this.darkCheckBox2.Text = "Disabled checkbox";
            // 
            // darkCheckBox1
            // 
            this.darkCheckBox1.AutoSize = true;
            this.darkCheckBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkCheckBox1.Location = new System.Drawing.Point(0, 26);
            this.darkCheckBox1.Name = "darkCheckBox1";
            this.darkCheckBox1.Size = new System.Drawing.Size(250, 19);
            this.darkCheckBox1.TabIndex = 4;
            this.darkCheckBox1.Text = "Checkbox";
            // 
            // darkTitle2
            // 
            this.darkTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle2.Location = new System.Drawing.Point(0, 0);
            this.darkTitle2.Name = "darkTitle2";
            this.darkTitle2.Size = new System.Drawing.Size(250, 26);
            this.darkTitle2.TabIndex = 8;
            this.darkTitle2.Text = "Check boxes";
            // 
            // darkScrollBar1
            // 
            this.darkScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.darkScrollBar1.Enabled = false;
            this.darkScrollBar1.Location = new System.Drawing.Point(265, 25);
            this.darkScrollBar1.Maximum = 5;
            this.darkScrollBar1.Minimum = 1;
            this.darkScrollBar1.Name = "darkScrollBar1";
            this.darkScrollBar1.Size = new System.Drawing.Size(15, 425);
            this.darkScrollBar1.TabIndex = 1;
            this.darkScrollBar1.Text = "darkScrollBar1";
            // 
            // DockProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.darkScrollBar1);
            this.DefaultDockArea = DarkUI.Docking.DarkDockArea.Right;
            this.DockText = "Properties";
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::Example.Icons.properties_16xLG;
            this.Name = "DockProperties";
            this.SerializationKey = "DockProperties";
            this.Size = new System.Drawing.Size(280, 450);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DarkUI.Controls.DarkRadioButton darkRadioButton3;
        private DarkUI.Controls.DarkRadioButton darkRadioButton2;
        private DarkUI.Controls.DarkRadioButton darkRadioButton1;
        private DarkUI.Controls.DarkTitle darkTitle1;
        private System.Windows.Forms.Panel panel2;
        private DarkUI.Controls.DarkCheckBox darkCheckBox3;
        private DarkUI.Controls.DarkCheckBox darkCheckBox2;
        private DarkUI.Controls.DarkCheckBox darkCheckBox1;
        private DarkUI.Controls.DarkTitle darkTitle2;
        private DarkUI.Controls.DarkScrollBar darkScrollBar1;
        private System.Windows.Forms.Panel panel3;
        private DarkUI.Controls.DarkTitle darkTitle3;
        private DarkUI.Controls.DarkDropdownList cmbList;
    }
}
