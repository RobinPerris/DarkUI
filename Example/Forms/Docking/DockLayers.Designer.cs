using DarkUI.Controls;
using DarkUI.Docking;

namespace Example
{
    partial class DockLayers
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
            this.lstLayers = new DarkUI.Controls.DarkListView();
            this.cmbList = new DarkUI.Controls.DarkDropdownList();
            this.SuspendLayout();
            // 
            // lstLayers
            // 
            this.lstLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLayers.HideScrollBars = false;
            this.lstLayers.Location = new System.Drawing.Point(0, 51);
            this.lstLayers.Name = "lstLayers";
            this.lstLayers.ShowIcons = true;
            this.lstLayers.Size = new System.Drawing.Size(280, 399);
            this.lstLayers.TabIndex = 0;
            this.lstLayers.Text = "darkListView1";
            // 
            // cmbList
            // 
            this.cmbList.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbList.Location = new System.Drawing.Point(0, 25);
            this.cmbList.Name = "cmbList";
            this.cmbList.ShowBorder = false;
            this.cmbList.Size = new System.Drawing.Size(280, 26);
            this.cmbList.TabIndex = 1;
            this.cmbList.Text = "darkDropdownList1";
            // 
            // DockLayers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstLayers);
            this.Controls.Add(this.cmbList);
            this.DefaultDockArea = DarkUI.Docking.DarkDockArea.Right;
            this.DockText = "Layers";
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::Example.Icons.Collection_16xLG;
            this.Name = "DockLayers";
            this.SerializationKey = "DockLayers";
            this.Size = new System.Drawing.Size(280, 450);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkListView lstLayers;
        private DarkDropdownList cmbList;
    }
}
