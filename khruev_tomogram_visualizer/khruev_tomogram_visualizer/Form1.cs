using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;



namespace khruev_tomogram_visualizer
{

    
    public partial class Form1: Form
    {
        Bin bin;
        View view;
        bool needReload = false;
        bool loaded = false;
        bool rbutton1 = false;
        bool rbutton2 = false;
        bool rbutton3 = false;
        int currentLayer = 0;

        int FrameCount;
        DateTime NextFPSupdate = DateTime.Now.AddSeconds(1);

        void displayFPS()
        {
            if(DateTime.Now >= NextFPSupdate)
            {
                this.Text = String.Format("CT Visualizer (fps={0})",FrameCount);
                NextFPSupdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }
        public Form1()
        {
            InitializeComponent();
            bin = new Bin();
            view = new View();
            trackBarMin.Value = 0;
            trackBarWidth.Value = 2000;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = dialog.FileName;
                bin.readBIN(str);
                trackBar1.Maximum = Bin.Z-1;
                view.SetupView(glControl1.Width, glControl1.Height);

                trackBarMin.Value = 0;
                trackBarWidth.Value = 2000;
                view.SetTransferFunction(0, 2000);

                loaded = true;
                glControl1.Invalidate();
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                if (rbutton1)
                {
                    view.DrawQuads(currentLayer);
                }
                else if (rbutton3)
                {
                    view.DrawQuadStrip(currentLayer);
                }
                else
                {
                    if (needReload)
                    {
                        view.GenerateTextureImage(currentLayer);
                        view.Load2DTexture();
                        needReload = false;
                    }
                    view.DrawTexture();
                }
                glControl1.SwapBuffers();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            needReload = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            rbutton1 = radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            rbutton2 = radioButton2.Checked;
        }



        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            rbutton3 = radioButton3.Checked;
        }
        private void trackBarMin_Scroll(object sender, EventArgs e)
        {
            view.SetTransferFunction(trackBarMin.Value, trackBarWidth.Value);
            labelMin.Text = $"Min: {trackBarMin.Value}";
            needReload = true;
            glControl1.Invalidate();
        }

        private void trackBarWidth_Scroll(object sender, EventArgs e)
        {
            view.SetTransferFunction(trackBarMin.Value, trackBarWidth.Value);
            labelWidth.Text = $"Width: {trackBarWidth.Value}";
            needReload = true;
            glControl1.Invalidate();
        }

        private void labelMin_Click(object sender, EventArgs e)
        {

        }
    }
}
