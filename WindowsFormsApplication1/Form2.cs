using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Glee.Drawing;
namespace WindowsApplication
{
    public partial class Form2 : Form
    {
        ToolTip toolTip1 = new ToolTip();
        public Form2()
        {
            this.Load += new EventHandler(Form1_Load);
            InitializeComponent();

            this.toolTip1.Active = true;
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to       toolTip1.ShowAlways = true;


        }

        void Form1_Load(object sender, EventArgs e)
        {
            //  gViewer.RemoveToolbar();
            gViewer.SelectionChanged +=
              new EventHandler(gViewer_SelectionChanged);

          //  gViewer.MouseClick += new MouseEventHandler(gViewer_MouseClick);
        }

      /* void gViewer_MouseClick(object sender, MouseEventArgs e)
        {
           
        }*/


        object selectedObjectAttr;
        object selectedObject;
        void gViewer_SelectionChanged(object sender, EventArgs e)
        {

            if (selectedObject != null)
            {
                if (selectedObject is Edge)
                    (selectedObject as Edge).Attr = selectedObjectAttr as EdgeAttr;
                else if (selectedObject is Node)
                    (selectedObject as Node).Attr = selectedObjectAttr as NodeAttr;

                selectedObject = null;
            }

            if (gViewer.SelectedObject == null)
            {
                //label1.Text = "No object under the mouse";
                this.gViewer.SetToolTip(toolTip1, "");

            }
            else
            {
                selectedObject = gViewer.SelectedObject;

                if (selectedObject is Edge)
                {
                    selectedObjectAttr = (gViewer.SelectedObject as Edge).Attr.Clone();
                    (gViewer.SelectedObject as Edge).Attr.Color = Microsoft.Glee.Drawing.Color.Magenta;
                    (gViewer.SelectedObject as Edge).Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Magenta;
                    Edge edge = gViewer.SelectedObject as Edge;




                    //here you can use e.Attr.Id or e.UserData to get back to you data
                    this.gViewer.SetToolTip(this.toolTip1, String.Format("edge from {0} {1}", edge.Source, edge.Target));

                }
                else if (selectedObject is Node)
                {

                    selectedObjectAttr = (gViewer.SelectedObject as Node).Attr.Clone();
                    (selectedObject as Node).Attr.Color = Microsoft.Glee.Drawing.Color.Magenta;
                    (selectedObject as Node).Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Magenta;
                    //here you can use e.Attr.Id to get back to your data
                    this.gViewer.SetToolTip(toolTip1, String.Format("node {0}", (selectedObject as Node).Attr.Label));
                }
                //label1.Text = selectedObject.ToString();
            }
            gViewer.Invalidate();
        }




        private void button1_Click(object sender, EventArgs e)
        {//this is abstract.dot of GraphViz
            Graph g = new Graph("graph");
            WindowsFormsApplication1.dataRef.grafRef = g;
            g.GraphAttr.NodeAttr.Padding = 3;
            WindowsFormsApplication1.dataRef.gvRef = gViewer;
            Edge edge;
            CheckState state = checkBox1.CheckState;
            System.Windows.Forms.DataGridView curTable=WindowsFormsApplication1.dataRef.gridRef;
            for (int i = 0; i < curTable.ColumnCount; i++)
            {
                for (int j = 0; j < curTable.RowCount; j++)
                {

                    if (curTable[i, j].Value == null) continue;  ///Индексы могут быть перевёрнуты, нул может быть пустой строкой
                    if (curTable[i, j].Value.ToString().Length > 2)
                    {
                        edge = (Edge)g.AddEdge("Q" + (i + 1), "Q" + ((curTable[i, j]).Value.ToString().Substring(2).Trim() == "0" ? "Stop" : (curTable[i, j]).Value.ToString().Substring(2).Trim()));
                        if(("Q" + (i + 1) =="Q" + ((curTable[i, j]).Value.ToString().Substring(2).Trim())) && (state== CheckState.Unchecked) )
                        {
                            if((j+1)%2==0)edge.Attr.Color=Microsoft.Glee.Drawing.Color.Gray;
                            else edge.Attr.Color = Microsoft.Glee.Drawing.Color.Brown;
                        }
                        edge.Attr.Label = curTable.Rows[j].HeaderCell.Value.ToString() + "/" + curTable[i, j].Value.ToString()[0] + curTable[i, j].Value.ToString()[1];
                    }
                }
            }


           
            //layout the graph and draw it
            gViewer.Graph = g;
         //   this.propertyGrid1.SelectedObject = g;
        }

        private static void CreateSourceNode(Node a)
        {
            a.Attr.Shape = Microsoft.Glee.Drawing.Shape.Box;
            a.Attr.XRad = 3;
            a.Attr.YRad = 3;
            a.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.Cyan;
            a.Attr.LineWidth = 2;
        }

        private void CreateTargetNode(Node a)
        {
            a.Attr.Shape = Microsoft.Glee.Drawing.Shape.DoubleCircle;
            a.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightGray;

            a.Attr.LabelMargin = -4;
        }


        private void recalculateLayoutButton_Click(object sender, EventArgs e)
        {
            // this.gViewer.Graph = this.propertyGrid1.SelectedObject as Microsoft.Glee.Drawing.Graph;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowsFormsApplication1.Form5 frm = new WindowsFormsApplication1.Form5();
            frm.Show();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            WindowsFormsApplication1.dataRef.gvRef = null;
        }

    }
}
