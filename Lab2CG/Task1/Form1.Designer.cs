namespace Task1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();

            menuStrip1.Items.AddRange( new ToolStripItem[] { fileToolStripMenuItem } );
            fileToolStripMenuItem.DropDownItems.AddRange( new ToolStripItem[] { openToolStripMenuItem } );

            fileToolStripMenuItem.Text = "File";
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;

            Controls.Add( menuStrip1 );
            MainMenuStrip = menuStrip1;
            Text = "Image Viewer";
            DoubleBuffered = true;
            WindowState = FormWindowState.Normal;

            ClientSize = new Size( 800, 600 );
        }
    }
}
