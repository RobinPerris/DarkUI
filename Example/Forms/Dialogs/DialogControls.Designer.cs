using DarkUI.Controls;

namespace Example
{
    partial class DialogControls
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogControls));
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTreeView = new DarkUI.Controls.DarkSectionPanel();
            this.treeTest = new DarkUI.Controls.DarkTreeView();
            this.pnlListView = new DarkUI.Controls.DarkSectionPanel();
            this.lstTest = new DarkUI.Controls.DarkListView();
            this.pnlMessageBox = new DarkUI.Controls.DarkSectionPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.darkComboBox1 = new DarkUI.Controls.DarkComboBox();
            this.darkTitle4 = new DarkUI.Controls.DarkTitle();
            this.panel6 = new System.Windows.Forms.Panel();
            this.darkNumericUpDown1 = new DarkUI.Controls.DarkNumericUpDown();
            this.darkTitle5 = new DarkUI.Controls.DarkTitle();
            this.panel5 = new System.Windows.Forms.Panel();
            this.darkRadioButton3 = new DarkUI.Controls.DarkRadioButton();
            this.darkRadioButton2 = new DarkUI.Controls.DarkRadioButton();
            this.darkRadioButton1 = new DarkUI.Controls.DarkRadioButton();
            this.darkTitle3 = new DarkUI.Controls.DarkTitle();
            this.panel4 = new System.Windows.Forms.Panel();
            this.darkCheckBox2 = new DarkUI.Controls.DarkCheckBox();
            this.darkCheckBox1 = new DarkUI.Controls.DarkCheckBox();
            this.darkTitle2 = new DarkUI.Controls.DarkTitle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnMessageBox = new DarkUI.Controls.DarkButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDialog = new DarkUI.Controls.DarkButton();
            this.darkTitle1 = new DarkUI.Controls.DarkTitle();
            this.darkGroupBox1 = new DarkUI.Controls.DarkGroupBox();
            this.darkCheckBox3 = new DarkUI.Controls.DarkCheckBox();
            this.darkRadioButton4 = new DarkUI.Controls.DarkRadioButton();
            this.pnlMain.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.pnlTreeView.SuspendLayout();
            this.pnlListView.SuspendLayout();
            this.pnlMessageBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.darkNumericUpDown1)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.darkGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tblMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.pnlMain.Size = new System.Drawing.Size(708, 528);
            this.pnlMain.TabIndex = 2;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 3;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblMain.Controls.Add(this.pnlTreeView, 0, 0);
            this.tblMain.Controls.Add(this.pnlListView, 0, 0);
            this.tblMain.Controls.Add(this.pnlMessageBox, 0, 0);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(5, 10);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 1;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Size = new System.Drawing.Size(698, 518);
            this.tblMain.TabIndex = 0;
            // 
            // pnlTreeView
            // 
            this.pnlTreeView.Controls.Add(this.treeTest);
            this.pnlTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTreeView.Location = new System.Drawing.Point(469, 0);
            this.pnlTreeView.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.pnlTreeView.Name = "pnlTreeView";
            this.pnlTreeView.SectionHeader = "Tree view test";
            this.pnlTreeView.Size = new System.Drawing.Size(224, 518);
            this.pnlTreeView.TabIndex = 14;
            // 
            // treeTest
            // 
            this.treeTest.AllowMoveNodes = true;
            this.treeTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeTest.Location = new System.Drawing.Point(1, 25);
            this.treeTest.MaxDragChange = 20;
            this.treeTest.MultiSelect = true;
            this.treeTest.Name = "treeTest";
            this.treeTest.ShowIcons = true;
            this.treeTest.Size = new System.Drawing.Size(222, 492);
            this.treeTest.TabIndex = 0;
            this.treeTest.Text = "darkTreeView1";
            // 
            // pnlListView
            // 
            this.pnlListView.Controls.Add(this.lstTest);
            this.pnlListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlListView.Location = new System.Drawing.Point(237, 0);
            this.pnlListView.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.pnlListView.Name = "pnlListView";
            this.pnlListView.SectionHeader = "List view test";
            this.pnlListView.Size = new System.Drawing.Size(222, 518);
            this.pnlListView.TabIndex = 13;
            // 
            // lstTest
            // 
            this.lstTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTest.Location = new System.Drawing.Point(1, 25);
            this.lstTest.MultiSelect = true;
            this.lstTest.Name = "lstTest";
            this.lstTest.Size = new System.Drawing.Size(220, 492);
            this.lstTest.TabIndex = 7;
            this.lstTest.Text = "darkListView1";
            // 
            // pnlMessageBox
            // 
            this.pnlMessageBox.Controls.Add(this.panel1);
            this.pnlMessageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMessageBox.Location = new System.Drawing.Point(5, 0);
            this.pnlMessageBox.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.pnlMessageBox.Name = "pnlMessageBox";
            this.pnlMessageBox.SectionHeader = "Controls test";
            this.pnlMessageBox.Size = new System.Drawing.Size(222, 518);
            this.pnlMessageBox.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.darkGroupBox1);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 25);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(220, 492);
            this.panel1.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.darkComboBox1);
            this.panel7.Controls.Add(this.darkTitle4);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(10, 349);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(200, 63);
            this.panel7.TabIndex = 23;
            // 
            // darkComboBox1
            // 
            this.darkComboBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.darkComboBox1.FormattingEnabled = true;
            this.darkComboBox1.Items.AddRange(new object[] {
            "Item 1",
            "Item 2",
            "This is a really long item in the collection to check out how text is clipped",
            "Item 4"});
            this.darkComboBox1.Location = new System.Drawing.Point(0, 26);
            this.darkComboBox1.Name = "darkComboBox1";
            this.darkComboBox1.Size = new System.Drawing.Size(200, 24);
            this.darkComboBox1.TabIndex = 20;
            // 
            // darkTitle4
            // 
            this.darkTitle4.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle4.Location = new System.Drawing.Point(0, 0);
            this.darkTitle4.Name = "darkTitle4";
            this.darkTitle4.Size = new System.Drawing.Size(200, 26);
            this.darkTitle4.TabIndex = 21;
            this.darkTitle4.Text = "ComboBox";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.darkNumericUpDown1);
            this.panel6.Controls.Add(this.darkTitle5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(10, 285);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(200, 64);
            this.panel6.TabIndex = 22;
            // 
            // darkNumericUpDown1
            // 
            this.darkNumericUpDown1.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkNumericUpDown1.Location = new System.Drawing.Point(0, 26);
            this.darkNumericUpDown1.Name = "darkNumericUpDown1";
            this.darkNumericUpDown1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.darkNumericUpDown1.Size = new System.Drawing.Size(200, 23);
            this.darkNumericUpDown1.TabIndex = 24;
            // 
            // darkTitle5
            // 
            this.darkTitle5.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle5.Location = new System.Drawing.Point(0, 0);
            this.darkTitle5.Name = "darkTitle5";
            this.darkTitle5.Size = new System.Drawing.Size(200, 26);
            this.darkTitle5.TabIndex = 23;
            this.darkTitle5.Text = "Numeric Up/Down";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.darkRadioButton3);
            this.panel5.Controls.Add(this.darkRadioButton2);
            this.panel5.Controls.Add(this.darkRadioButton1);
            this.panel5.Controls.Add(this.darkTitle3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(10, 185);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(200, 100);
            this.panel5.TabIndex = 12;
            // 
            // darkRadioButton3
            // 
            this.darkRadioButton3.AutoSize = true;
            this.darkRadioButton3.Checked = true;
            this.darkRadioButton3.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkRadioButton3.Enabled = false;
            this.darkRadioButton3.Location = new System.Drawing.Point(0, 64);
            this.darkRadioButton3.Name = "darkRadioButton3";
            this.darkRadioButton3.Size = new System.Drawing.Size(200, 19);
            this.darkRadioButton3.TabIndex = 4;
            this.darkRadioButton3.TabStop = true;
            this.darkRadioButton3.Text = "Disabled radio button";
            // 
            // darkRadioButton2
            // 
            this.darkRadioButton2.AutoSize = true;
            this.darkRadioButton2.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkRadioButton2.Location = new System.Drawing.Point(0, 45);
            this.darkRadioButton2.Name = "darkRadioButton2";
            this.darkRadioButton2.Size = new System.Drawing.Size(200, 19);
            this.darkRadioButton2.TabIndex = 3;
            this.darkRadioButton2.Text = "Radio button";
            // 
            // darkRadioButton1
            // 
            this.darkRadioButton1.AutoSize = true;
            this.darkRadioButton1.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkRadioButton1.Location = new System.Drawing.Point(0, 26);
            this.darkRadioButton1.Name = "darkRadioButton1";
            this.darkRadioButton1.Size = new System.Drawing.Size(200, 19);
            this.darkRadioButton1.TabIndex = 2;
            this.darkRadioButton1.Text = "Radio button";
            // 
            // darkTitle3
            // 
            this.darkTitle3.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle3.Location = new System.Drawing.Point(0, 0);
            this.darkTitle3.Name = "darkTitle3";
            this.darkTitle3.Size = new System.Drawing.Size(200, 26);
            this.darkTitle3.TabIndex = 16;
            this.darkTitle3.Text = "Radio buttons";
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.Controls.Add(this.darkCheckBox2);
            this.panel4.Controls.Add(this.darkCheckBox1);
            this.panel4.Controls.Add(this.darkTitle2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(10, 111);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel4.Size = new System.Drawing.Size(200, 74);
            this.panel4.TabIndex = 11;
            // 
            // darkCheckBox2
            // 
            this.darkCheckBox2.AutoSize = true;
            this.darkCheckBox2.Checked = true;
            this.darkCheckBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.darkCheckBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkCheckBox2.Enabled = false;
            this.darkCheckBox2.Location = new System.Drawing.Point(0, 45);
            this.darkCheckBox2.Name = "darkCheckBox2";
            this.darkCheckBox2.Size = new System.Drawing.Size(200, 19);
            this.darkCheckBox2.TabIndex = 13;
            this.darkCheckBox2.Text = "Disabled checkbox";
            // 
            // darkCheckBox1
            // 
            this.darkCheckBox1.AutoSize = true;
            this.darkCheckBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkCheckBox1.Location = new System.Drawing.Point(0, 26);
            this.darkCheckBox1.Name = "darkCheckBox1";
            this.darkCheckBox1.Size = new System.Drawing.Size(200, 19);
            this.darkCheckBox1.TabIndex = 12;
            this.darkCheckBox1.Text = "Enabled checkbox";
            // 
            // darkTitle2
            // 
            this.darkTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle2.Location = new System.Drawing.Point(0, 0);
            this.darkTitle2.Name = "darkTitle2";
            this.darkTitle2.Size = new System.Drawing.Size(200, 26);
            this.darkTitle2.TabIndex = 15;
            this.darkTitle2.Text = "Check boxes";
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.btnMessageBox);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 71);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel3.Size = new System.Drawing.Size(200, 40);
            this.panel3.TabIndex = 10;
            // 
            // btnMessageBox
            // 
            this.btnMessageBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMessageBox.Location = new System.Drawing.Point(0, 0);
            this.btnMessageBox.Name = "btnMessageBox";
            this.btnMessageBox.Padding = new System.Windows.Forms.Padding(5);
            this.btnMessageBox.Size = new System.Drawing.Size(200, 30);
            this.btnMessageBox.TabIndex = 12;
            this.btnMessageBox.Text = "Message Box";
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.btnDialog);
            this.panel2.Controls.Add(this.darkTitle1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 10);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.panel2.Size = new System.Drawing.Size(200, 61);
            this.panel2.TabIndex = 5;
            // 
            // btnDialog
            // 
            this.btnDialog.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDialog.Location = new System.Drawing.Point(0, 26);
            this.btnDialog.Name = "btnDialog";
            this.btnDialog.Padding = new System.Windows.Forms.Padding(5);
            this.btnDialog.Size = new System.Drawing.Size(200, 30);
            this.btnDialog.TabIndex = 4;
            this.btnDialog.Text = "Dialog";
            // 
            // darkTitle1
            // 
            this.darkTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle1.Location = new System.Drawing.Point(0, 0);
            this.darkTitle1.Name = "darkTitle1";
            this.darkTitle1.Size = new System.Drawing.Size(200, 26);
            this.darkTitle1.TabIndex = 14;
            this.darkTitle1.Text = "Dialogs";
            // 
            // darkGroupBox1
            // 
            this.darkGroupBox1.AutoSize = true;
            this.darkGroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox1.Controls.Add(this.darkRadioButton4);
            this.darkGroupBox1.Controls.Add(this.darkCheckBox3);
            this.darkGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkGroupBox1.Location = new System.Drawing.Point(10, 412);
            this.darkGroupBox1.Name = "darkGroupBox1";
            this.darkGroupBox1.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.darkGroupBox1.Size = new System.Drawing.Size(200, 69);
            this.darkGroupBox1.TabIndex = 24;
            this.darkGroupBox1.TabStop = false;
            this.darkGroupBox1.Text = "GroupBox";
            // 
            // darkCheckBox3
            // 
            this.darkCheckBox3.AutoSize = true;
            this.darkCheckBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkCheckBox3.Location = new System.Drawing.Point(10, 21);
            this.darkCheckBox3.Name = "darkCheckBox3";
            this.darkCheckBox3.Size = new System.Drawing.Size(180, 19);
            this.darkCheckBox3.TabIndex = 0;
            this.darkCheckBox3.Text = "Checkbox";
            // 
            // darkRadioButton4
            // 
            this.darkRadioButton4.AutoSize = true;
            this.darkRadioButton4.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkRadioButton4.Location = new System.Drawing.Point(10, 40);
            this.darkRadioButton4.Name = "darkRadioButton4";
            this.darkRadioButton4.Size = new System.Drawing.Size(180, 19);
            this.darkRadioButton4.TabIndex = 1;
            this.darkRadioButton4.TabStop = true;
            this.darkRadioButton4.Text = "Radio button";
            // 
            // DialogControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 573);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogControls";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Controls";
            this.Controls.SetChildIndex(this.pnlMain, 0);
            this.pnlMain.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.pnlTreeView.ResumeLayout(false);
            this.pnlListView.ResumeLayout(false);
            this.pnlMessageBox.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.darkNumericUpDown1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.darkGroupBox1.ResumeLayout(false);
            this.darkGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private DarkSectionPanel pnlTreeView;
        private DarkTreeView treeTest;
        private DarkSectionPanel pnlListView;
        private DarkListView lstTest;
        private DarkSectionPanel pnlMessageBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private DarkButton btnDialog;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private DarkButton btnMessageBox;
        private DarkCheckBox darkCheckBox2;
        private DarkCheckBox darkCheckBox1;
        private System.Windows.Forms.Panel panel5;
        private DarkRadioButton darkRadioButton2;
        private DarkRadioButton darkRadioButton1;
        private DarkRadioButton darkRadioButton3;
        private DarkTitle darkTitle1;
        private DarkTitle darkTitle2;
        private DarkTitle darkTitle3;
        private System.Windows.Forms.Panel panel7;
        private DarkComboBox darkComboBox1;
        private DarkTitle darkTitle4;
        private System.Windows.Forms.Panel panel6;
        private DarkNumericUpDown darkNumericUpDown1;
        private DarkTitle darkTitle5;
        private DarkGroupBox darkGroupBox1;
        private DarkRadioButton darkRadioButton4;
        private DarkCheckBox darkCheckBox3;
    }
}