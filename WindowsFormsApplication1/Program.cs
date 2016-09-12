using Microsoft.Glee.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    //ewqe//
    static class dataRef
    {
        static public  System.Windows.Forms.DataGridView gridRef;
        static public Graph grafRef;
        static public Microsoft.Glee.GraphViewerGdi.GViewer gvRef;
        static public string textTaskRef;

    }
    static class dataAlph
    {
        static public string Alph;
        static public int X;
        static public int Y;
        static public int index;
       
    }
}
