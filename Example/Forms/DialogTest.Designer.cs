namespace Example
{
    partial class DialogTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogTest));
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTreeView = new DarkUI.DarkSectionPanel();
            this.treeTest = new DarkUI.DarkTreeView();
            this.pnlListView = new DarkUI.DarkSectionPanel();
            this.lstTest = new DarkUI.DarkListView();
            this.pnlMessageBox = new DarkUI.DarkSectionPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMessageBox = new DarkUI.DarkButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDialog = new DarkUI.DarkButton();
            this.pnlMain.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.pnlTreeView.SuspendLayout();
            this.pnlListView.SuspendLayout();
            this.pnlMessageBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tblMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(10, 15, 10, 5);
            this.pnlMain.Size = new System.Drawing.Size(708, 410);
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
            this.tblMain.Location = new System.Drawing.Point(10, 15);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 1;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Size = new System.Drawing.Size(688, 390);
            this.tblMain.TabIndex = 0;
            // 
            // pnlTreeView
            // 
            this.pnlTreeView.Controls.Add(this.treeTest);
            this.pnlTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTreeView.Location = new System.Drawing.Point(463, 0);
            this.pnlTreeView.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.pnlTreeView.Name = "pnlTreeView";
            this.pnlTreeView.SectionHeader = "Tree view test";
            this.pnlTreeView.Size = new System.Drawing.Size(220, 390);
            this.pnlTreeView.TabIndex = 14;
            // 
            // treeTest
            // 
            this.treeTest.AllowMoveNodes = true;
            this.treeTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeTest.Location = new System.Drawing.Point(1, 25);
            this.treeTest.MultiSelect = true;
            this.treeTest.Name = "treeTest";
            this.treeTest.ShowIcons = true;
            this.treeTest.Size = new System.Drawing.Size(218, 364);
            this.treeTest.TabIndex = 0;
            this.treeTest.Text = "darkTreeView1";
            // 
            // pnlListView
            // 
            this.pnlListView.Controls.Add(this.lstTest);
            this.pnlListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlListView.Location = new System.Drawing.Point(234, 0);
            this.pnlListView.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.pnlListView.Name = "pnlListView";
            this.pnlListView.SectionHeader = "List view test";
            this.pnlListView.Size = new System.Drawing.Size(219, 390);
            this.pnlListView.TabIndex = 13;
            // 
            // lstTest
            // 
            this.lstTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTest.Location = new System.Drawing.Point(1, 25);
            this.lstTest.MultiSelect = true;
            this.lstTest.Name = "lstTest";
            this.lstTest.Size = new System.Drawing.Size(217, 364);
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
            this.pnlMessageBox.SectionHeader = "Section test";
            this.pnlMessageBox.Size = new System.Drawing.Size(219, 390);
            this.pnlMessageBox.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnMessageBox);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 25);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(217, 364);
            this.panel1.TabIndex = 0;
            // 
            // btnMessageBox
            // 
            this.btnMessageBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMessageBox.Location = new System.Drawing.Point(10, 50);
            this.btnMessageBox.Name = "btnMessageBox";
            this.btnMessageBox.Padding = new System.Windows.Forms.Padding(5);
            this.btnMessageBox.Size = new System.Drawing.Size(197, 30);
            this.btnMessageBox.TabIndex = 6;
            this.btnMessageBox.Text = "Message Box";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnDialog);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(197, 40);
            this.panel2.TabIndex = 5;
            // 
            // btnDialog
            // 
            this.btnDialog.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDialog.Location = new System.Drawing.Point(0, 0);
            this.btnDialog.Name = "btnDialog";
            this.btnDialog.Padding = new System.Windows.Forms.Padding(5);
            this.btnDialog.Size = new System.Drawing.Size(197, 30);
            this.btnDialog.TabIndex = 4;
            this.btnDialog.Text = "Dialog";
            // 
            // DialogTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 455);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test dialog please ignore";
            this.Controls.SetChildIndex(this.pnlMain, 0);
            this.pnlMain.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.pnlTreeView.ResumeLayout(false);
            this.pnlListView.ResumeLayout(false);
            this.pnlMessageBox.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private DarkUI.DarkSectionPanel pnlTreeView;
        private DarkUI.DarkTreeView treeTest;
        private DarkUI.DarkSectionPanel pnlListView;
        private DarkUI.DarkListView lstTest;
        private DarkUI.DarkSectionPanel pnlMessageBox;
        private System.Windows.Forms.Panel panel1;
        private DarkUI.DarkButton btnMessageBox;
        private System.Windows.Forms.Panel panel2;
        private DarkUI.DarkButton btnDialog;
    }
}