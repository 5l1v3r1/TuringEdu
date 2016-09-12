using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string Alph = WindowsFormsApplication1.dataAlph.Alph;
            this.Location  =new Point( WindowsFormsApplication1.dataAlph.X-50, WindowsFormsApplication1.dataAlph.Y-50);

            for (int i = 0; i <Alph.Length; i++)
            {
                var col = new DataGridViewTextBoxColumn();
                this.dataGridView1.Columns.Add(col);
                this.dataGridView1.Columns[i].Width = 20;
                this.dataGridView1.Columns[i].Resizable = 0;
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                if (i == 0) { this.dataGridView1.Rows.Add(); }
                if (Alph[i] == ' ') { this.dataGridView1[i, 0].Value = "_"; }
                else { this.dataGridView1[i, 0].Value = Alph[i].ToString(); }
            }
           /* dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;*/

        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            WindowsFormsApplication1.dataAlph.index = e.ColumnIndex;
            this.Close();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            WindowsFormsApplication1.dataAlph.index = e.ColumnIndex;
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            WindowsFormsApplication1.dataAlph.index = e.ColumnIndex;
            this.Close();
        }
    }
}
