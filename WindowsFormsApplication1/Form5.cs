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
    public partial class Form5 : Form
    {
        Image img;
        private System.Drawing.Printing.PrintDocument docToPrint =
        new System.Drawing.Printing.PrintDocument();
        public Form5()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "openFileDialog1") textBox1.Text = openFileDialog1.FileName;
            else return;
            if (openFileDialog1.FileName.Length == 0)
            {
                return;
            }
            else
            {
                try
                {
                    img = Image.FromFile(openFileDialog1.FileName);
                }
                catch (Exception eq)
                {
                    MessageBox.Show("Выбранный файл не распознаётся как изображение.");

                }
            }






        }


        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                printDialog1.Document = docToPrint;

                // DialogResult result = printDialog1.ShowDialog();
                printDocument1.Print();

                // If the result is OK then print the document.
                //  if (result == DialogResult.OK)
                //{
                //  docToPrint.Print();
                //}
            }
            catch
            {
                MessageBox.Show("Не найдены установленные принтеры");
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Insert code to render the page here.
            // This code will be called when the control is drawn.

            // The following code will render a simple
            // message on the printed document.
            string text = WindowsFormsApplication1.dataRef.textTaskRef;
            if (text == null)
            {
                text = "";

            }
            if (img == null)
            {
                MessageBox.Show("Не найден граф для печати.");
                return;

            }
            string printed = "";
            for (int i = 0; i < text.Length; i++)
            {
                printed += text[i];
                if (i % 85 > 60)
                {
                    if (text[i] == ' ')
                    {
                        printed += '\n';
                        for (int j = 0; j < 31; j++, i++)
                        {
                            if (i < text.Length) printed += text[i];
                        }
                    }

                }
            }
            printed += '\n';
            printed += "Автор работы: " + textBox2.Text;
            System.Drawing.Font printFont = new System.Drawing.Font
                ("Arial", 12, System.Drawing.FontStyle.Regular);
            double ImgHeig = img.Height * 600 / img.Width;
            if (ImgHeig < 700)
            {
                img = (Image)new Bitmap(img, new Size(700, img.Height));
            }
           // MessageBox.Show(ImgHeig.ToString());
            e.Graphics.DrawImage(img, new Point(10, 10));
            // Draw the content.
            e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(0, img.Height + 50), new Point(1000, img.Height + 50));
            e.Graphics.DrawString(printed, printFont,
                System.Drawing.Brushes.Black, 10, img.Height + 65);
        }
    }
}
