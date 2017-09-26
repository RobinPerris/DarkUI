using DarkUI.Controls;
using DarkUI.Docking;

namespace Example
{
    partial class DockConsole
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
            this.lstConsole = new DarkUI.Controls.DarkListView();
            this.darkTreeView1 = new DarkUI.Controls.DarkTreeView();
            this.SuspendLayout();
            // 
            // lstConsole
            // 
            this.lstConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstConsole.Location = new System.Drawing.Point(0, 25);
            this.lstConsole.MultiSelect = true;
            this.lstConsole.Name = "lstConsole";
            this.lstConsole.Size = new System.Drawing.Size(200, 175);
            this.lstConsole.TabIndex = 0;
            this.lstConsole.Text = "darkListView1";
            // 
            // darkTreeView1
            // 
            this.darkTreeView1.Location = new System.Drawing.Point(415, 117);
            this.darkTreeView1.MaxDragChange = 20;
            this.darkTreeView1.Name = "darkTreeView1";
            this.darkTreeView1.Size = new System.Drawing.Size(75, 23);
            this.darkTreeView1.TabIndex = 1;
            this.darkTreeView1.Text = "darkTreeView1";
            // 
            // DockConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.darkTreeView1);
            this.Controls.Add(this.lstConsole);
            this.DefaultDockArea = DarkUI.Docking.DarkDockArea.Bottom;
            this.DockText = "Console";
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::Example.Icons.Console;
            this.Name = "DockConsole";
            this.SerializationKey = "DockConsole";
            this.Size = new System.Drawing.Size(200, 200);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkListView lstConsole;
        private DarkTreeView darkTreeView1;
    }
}
