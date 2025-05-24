using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace RayTracing
{
    class View
    {
        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;

        int attribute_vpos;
        int uniform_pos;
        Vector3 campos;
        int uniform_aspect;
        double aspect;
        float uniform_maxRayDepth;

        int vbo_position;


        public void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
        public void InitShaders()
        {
            BasicProgramID = GL.CreateProgram(); // создание объекта программы  
            loadShader("..\\..\\raytracing.vert", ShaderType.VertexShader, BasicProgramID,
                    out BasicVertexShader);
            loadShader("..\\..\\raytracing.frag", ShaderType.FragmentShader, BasicProgramID,
            out BasicFragmentShader);
            GL.LinkProgram(BasicProgramID); 
            // Проверяем успех компоновки 
            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status); 
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));


        }
        public void buffers(GLControl gl)
        {
            Vector3[] vertdata = new Vector3[] {
                new Vector3(-1f, -1f, 0f),
                new Vector3( 1f, -1f, 0f),
                new Vector3( 1f,  1f, 0f),
                new Vector3(-1f,  1f, 0f) };

            attribute_vpos = GL.GetAttribLocation(BasicProgramID, "vPosition");
            uniform_pos = GL.GetUniformLocation(BasicProgramID, "campos");
            uniform_aspect = GL.GetUniformLocation(BasicProgramID, "aspect");


            GL.GenBuffers(1, out vbo_position);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length *
            Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);

            attribute_vpos = GL.GetAttribLocation(BasicProgramID, "vPosition");
            GL.EnableVertexAttribArray(attribute_vpos); // Включаем атрибут!

            campos = new Vector3(0, 0, 3);
            aspect = (float)gl.Width / gl.Height;




            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.Uniform3(uniform_pos, campos);
            GL.Uniform1(uniform_aspect, aspect);
            GL.UseProgram(BasicProgramID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


            GL.DrawArrays(PrimitiveType.Quads, 0, 4);
            GL.DisableVertexAttribArray(attribute_vpos);

            gl.SwapBuffers();
        }

        public void setAspect(double aspect)
        {
            this.aspect = aspect;
        }
    }
}
