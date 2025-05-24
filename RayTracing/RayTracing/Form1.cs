using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace RayTracing
{
    public partial class Form1: Form
    {
        View view;
        public Form1()
        {
            InitializeComponent();
            view = new View();

        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            view.InitShaders();


        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            view.buffers(glControl1);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (glControl1.ClientSize.Height == 0)
            {
                glControl1.ClientSize = new Size(glControl1.ClientSize.Width, 1);
            }

            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            view.setAspect((float)glControl1.Width / glControl1.Height);
            glControl1.Invalidate();
        }
    }
}
