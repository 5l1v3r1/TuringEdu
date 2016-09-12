using Microsoft.Glee.Drawing;
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
    public partial class Form1 : Form
    {
        //DataGridView Save = new DataGridView();
        int NofStates = 1, CurSizeAlph;
        string CurrentAlphabet;
        char[] TapeTur = new char[1200];
       
        char[] SaveTapeTur = new char[1200];
        char[] SaveTurCurrent = new char[31];
        int saveCurrentIndex;
        
        int NearestIndex;
        int shift = 400;
        int CurIndex;
        int CurState = 1;
        int PastState = 0;
        int speed = 500;
        int pause = 0;
        int SYS = 0;
        int stepbstep = 0;
        bool allowkeymouse=false;

        


        public class CurCell
        {
            public char smb;

            public char action;
            public int stateNext;


            public void ReadFromTable(string str)
            {
                //Form1 frm = new Form1();
                //string str;

                //str = frm.dataTable[col,row].Value.ToString();
                if (str[0] != '_') { smb = str[0]; }
                else { smb = ' '; }
                action = str[1];
                int leng = str.Length - 2;

                stateNext = Convert.ToInt32(str.Substring(2, leng));


            }


        }
        public Form1()
        {
            try
            {
                InitializeComponent();
                txtDesc.Text = "";
                WindowsFormsApplication1.dataRef.gridRef = dataTable;
                CurrentAlphabet = txtAlph.Text + ' ';
                CurSizeAlph = CurrentAlphabet.Length;
           
                
                for (int i = 0; i < CurSizeAlph; i++)
                {
                    var row = new DataGridViewRow();
                    if (CurrentAlphabet[i] != ' ') { row.HeaderCell.Value = CurrentAlphabet[i].ToString(); }
                    else { row.HeaderCell.Value = "_"; }
                    this.dataTable.Rows.Insert(i, row);
                    //  this.dataTable.Rows[1].HeaderCell.Value
                }

                for (int i = -15; i < 16; i++)
                {
                    var col = new DataGridViewTextBoxColumn();
                    this.dataMachine.Columns.Add(col);
                    this.dataMachine.Columns[i + 15].HeaderText = i.ToString();
                    this.dataMachine.Columns[i + 15].Width = 25;
                    this.dataMachine.Columns[i + 15].Resizable = 0;
                    this.dataMachine.Columns[i + 15].SortMode = DataGridViewColumnSortMode.NotSortable;

                }

                this.dataMachine.Rows.Add();
                this.dataMachine[15, 0].Style.BackColor = System.Drawing.Color.Khaki;

                CurIndex = shift + 100;
                for (int i = 0; i < 1100; i++)
                {
                    TapeTur[i] = ' ';
                }
                this.dataMachine.AllowUserToResizeColumns = false;
                this.dataMachine.AllowUserToResizeRows = false;
                this.dataMachine.AllowUserToOrderColumns = false;
                CurIndex = shift;
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "InitializeError");
                //   throw;
            }

        }



        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "ExitError");
                //   throw;
            }
        }

        private void refresh_columns()
        {
            try
            {
                for (int i = 0; i < NofStates; i++)
                    this.dataTable.Columns[i].HeaderText = "Q" + (i + 1).ToString();
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "RefreshColumnsError");
                //   throw;
            }
            // dataTable.DefaultCellStyle.for
        }
        private void PasteFwd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataTable.CurrentCell == null) { return; }

                var col = new DataGridViewTextBoxColumn();
                //col.HeaderText = "Q" + (this.dataTable.CurrentCell.ColumnIndex+1).ToString();
                col.Width = 50;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataTable.Columns.Insert(this.dataTable.CurrentCell.ColumnIndex + 1, col);
                NofStates += 1;
                refresh_columns();
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "PasteForwardError");
                //   throw;
            }
        }

        private void PasteBhd_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(this.dataTable.CurrentCell.ColumnIndex.ToString());
                if (this.dataTable.CurrentCell == null) { return; }

                var col = new DataGridViewTextBoxColumn();
                col.SortMode = DataGridViewColumnSortMode.NotSortable; 
                //col.HeaderText = "Q" + (this.dataTable.CurrentCell.ColumnIndex-1).ToString();
                col.Width = 50;
                if (this.dataTable.CurrentCell.ColumnIndex != 0)
                { this.dataTable.Columns.Insert(this.dataTable.CurrentCell.ColumnIndex - 1, col); }
                else { this.dataTable.Columns.Insert(this.dataTable.CurrentCell.ColumnIndex, col); }
                NofStates += 1;
                refresh_columns();
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "PasteBehindError");
                //   throw;
            }
        }

        private void DeleteCol_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.dataTable.CurrentCell == null)||(NofStates==1)) { return; }
                this.dataTable.Columns.RemoveAt(this.dataTable.CurrentCell.ColumnIndex);
                NofStates -= 1;
                refresh_columns();
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "DeleteDataColumnsError");
                //   throw;
            }
        }

        private void txtAlph_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (SYS == 1) { return; }
                int NewSizeAlph;
                string NewAlphabet;
                NewAlphabet = txtAlph.Text + ' ';
                NewSizeAlph = NewAlphabet.Length;

                if (NewSizeAlph > CurSizeAlph)
                {
                    if (NewSizeAlph - 1 != CurSizeAlph) { txtAlph.Text = CurrentAlphabet.Substring(0, CurSizeAlph - 1); MessageBox.Show("Вставляйте символы по одному"); return; }
                    int index = -1;
                    for (int i = 0; i < (CurSizeAlph); i++)
                    {
                        if (NewAlphabet[i] != CurrentAlphabet[i]) { index = i; break; }
                    }
                    if (index == -1) { index = NewSizeAlph - 1; }

                    if ((CurrentAlphabet.Contains(NewAlphabet[index])) || (NewAlphabet[index] == ' ')) { txtAlph.Text = CurrentAlphabet.Substring(0, CurSizeAlph - 1); txtAlph.SelectionStart = index; return; }
                    var row = new DataGridViewRow();
                    row.HeaderCell.Value = NewAlphabet[index].ToString();
                    this.dataTable.Rows.Insert(index, row);
                    CurrentAlphabet = NewAlphabet;
                    CurSizeAlph = NewSizeAlph;
                }
                else if (NewSizeAlph < CurSizeAlph)
                {
                    if (NewSizeAlph + 1 != CurSizeAlph) { txtAlph.Text = CurrentAlphabet.Substring(0, CurSizeAlph - 1); ; MessageBox.Show("Удаляйте символы по одному"); return; }
                    int index = -1;
                    for (int i = 0; i < (NewSizeAlph); i++)
                    {
                        if (NewAlphabet[i] != CurrentAlphabet[i]) { index = i; break; }

                    }
                    if (index == -1) { index = CurSizeAlph - 1; }
                    this.dataTable.Rows.RemoveAt(index);

                    CurrentAlphabet = NewAlphabet;
                    CurSizeAlph = NewSizeAlph;
                }
            }

            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "EditAlphabetError");
                //   throw;
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //dataMachine.Columns.Clear();
                SYS = 1;
                speed = 500;
                string desc = " ", table = " ";
                char[] mas = new char[50000];
                byte[] bt = new byte[50000];
                // byte[] bt = new byte[20000];
                //char[] openning[0]=' ';
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {


                    //  System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName, Encoding.Default);
                    int i = 0;

                    // txtDesc.Text = System.IO.File.ReadAllText(openFileDialog1.FileName,Encoding.ASCII);

                    bt = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                    int count = bt[0], index = 0;
                    if (bt[1] != 0) { count += 256 * bt[1]; }
                    label1.Text = bt[0].ToString() + " " + bt[1].ToString();
                    desc = System.Text.Encoding.Default.GetString(bt, 4, count);

                    txtDesc.Text = desc;

                    /* txtDesc.Text = " ";
                     int k = bt.Length;
                        for (int j = 0; j < k; j++)
                     {
                         txtDesc.Text += bt[j].ToString() + " ";
                     }*/

                    index = count + 8;
                    count = bt[index];
                    if (bt[index + 1] != 0) { count += 256 * bt[index + 1]; }
                    // label1.Text = count.ToString();
                    index += 4;
                    table = System.Text.Encoding.Default.GetString(bt, index, count);
                    int states = 0;
                    dataTable.Columns.Clear();
                    while (table[i] != '\n')
                    {
                        if (table[i] == '\t') { states += 1; var col = new DataGridViewTextBoxColumn(); col.SortMode = DataGridViewColumnSortMode.NotSortable; col.Width = 50; dataTable.Columns.Add(col); col.HeaderText = "Q" + states.ToString(); }
                        i++;
                    }
                    i++;
                    NofStates = states;
                    //   label1.Text = states.ToString();
                    CurrentAlphabet = "";
                    txtAlph.Text = "";
                    CurSizeAlph = 0;
                    int rl = 0;
                    while (i < table.Length)
                    {
                        int sl = 0;
                        char ch = table[i];
                        txtAlph.Text += ch.ToString();
                        var row = new DataGridViewRow();
                        if (ch == ' ') { row.HeaderCell.Value = "_"; }
                        else { row.HeaderCell.Value = ch.ToString(); }

                        this.dataTable.Rows.Insert(rl, row);
                        CurrentAlphabet += ch.ToString();
                        CurSizeAlph++;


                        i++;
                        while (table[i] != '\n')
                        {
                            int l = i + 1;
                            if (table[i] == '\t') { while ((table[l] != '\t')) { if ((table[l] == '\n')) { break; } l++; } sl++; }
                            if (i != l - 1) { dataTable[sl - 1, rl].Value = table.Substring(i + 1, l - i - 1); }
                            i = l;
                            //MessageBox.Show("lalsl");
                            // else { dataTable[sl, rl].Value = ""; }
                        }
                        i++;
                        rl++;

                    }
                    string com = "";
                    index += i;
                    //  richTextBox1.Text = bt[index].ToString();
                    count = bt[index];
                    if (bt[index + 1] != 0) { count += 256 * bt[index + 1]; }
                    index += 4;
                    richTextBox1.Text = System.Text.Encoding.Default.GetString(bt, index, count);

                    //openning = System.Text.Encoding.Default.GetString(bt,5,bt.Length-5);

                    // mas = System.Text.Encoding.UTF8.GetString(bt).ToCharArray();
                    /*while (sr.EndOfStream != true){
                   
                   i++;
                     }*/



                    // sr.Close();
                }


                /*    txtDesc.Text = " ";
                    int k = bt.Length;
                    for (int j = 0; j < k; j++)
                    {
                        txtDesc.Text += bt[j].ToString()+" " ;
                    }*/


                //txtDesc.Text = desc;            }

                SYS = 0;
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(),"OpenError");
                //   throw;
            }
        }

        private void dataTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string entered;
                string declaredP = "<>.";
                int col, row;
                col = e.ColumnIndex;
                row = e.RowIndex;
                if (this.dataTable[col, row].Value == null) { return; }

                entered = this.dataTable[col, row].Value.ToString();

                if (entered.Length == 1)
                {
                    if (!((entered[0] == declaredP[0]) || (entered[0] == declaredP[1]) || (entered[0] == declaredP[2]))) { MessageBox.Show("Отсутствует команда перехода"); dataTable[col, row].Value = ""; return; }
                    else
                    {
                        if (CurrentAlphabet[row] == ' ') { entered = "_" + entered; this.dataTable[col, row].Value = entered; }
                        else { entered = CurrentAlphabet[row] + entered; this.dataTable[col, row].Value = entered; }
                    }
                }


                if (entered.Length == 2)
                {
                    if (declaredP.Contains(entered[0]))
                    {
                        if (CurrentAlphabet[row] == ' ') { entered = "_" + entered; this.dataTable[col, row].Value = entered; }
                        else { entered = CurrentAlphabet[row] + entered; this.dataTable[col, row].Value = entered; }
                    }
                    else { entered += (col + 1).ToString(); this.dataTable[col, row].Value = entered; }
                }

                CurCell cl = new CurCell();
                cl.ReadFromTable(entered);
                //  if (entered[0] == '_') {  }

                if (declaredP.Contains(entered[0]))
                {
                    if (NofStates >= cl.stateNext)
                    {
                        char smb;
                        if (CurrentAlphabet[row] == ' ') { smb = '_'; }
                        else { smb = CurrentAlphabet[row]; }
                        this.dataTable[col, row].Value = smb.ToString() + entered;
                    }

                    else { MessageBox.Show("Состояния с номером \"" + cl.stateNext.ToString() + "\" не существует"); this.dataTable[col, row].Value = ""; }
                }

                else if (declaredP.Contains(cl.action))
                {
                    if ((CurrentAlphabet.Contains(cl.smb)) || (cl.smb == '_'))
                    {
                        if (NofStates >= cl.stateNext) { }
                        else { MessageBox.Show("Состояния с номером \"" + cl.stateNext.ToString() + "\" не существует"); this.dataTable[col, row].Value = ""; }
                    }
                    else { MessageBox.Show("Данный символ \"" + cl.smb + "\" не содержится в алфавите"); this.dataTable[col, row].Value = ""; }
                }

                else { MessageBox.Show("Отсутствуют управляющие символы < > . "); this.dataTable[col, row].Value = ""; }


                /*for (int i = 0; i < this.dataTable[col, row].Value.ToString().Length; i++)
                    for (int j = 0; j < declaredP.Length; j++)
                    { 
                
                    }


                        if (this.dataTable[col, row].Value.ToString().Length == 2)
                        {


                        }
                */
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "CellEditError");
                //   throw;
            }
        }

        private void построитьГрафToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WindowsApplication.Form2 frm = new WindowsApplication.Form2();
                frm.Show();
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "ShowGraphError");
                //   throw;
            }
        }

        private void dataMachine_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataMachine[e.ColumnIndex, e.RowIndex].Value == null) { return; }

                //if (this.dataMachine[e.ColumnIndex, e.RowIndex].Value.ToString() == "/help") { MessageBox.Show("Тут есть полезные примеры: http://kpolyakov.narod.ru/prog/turing.htm"); this.dataMachine[e.ColumnIndex, e.RowIndex].Value = null; return; }
               
                string edit = dataMachine[e.ColumnIndex, e.RowIndex].Value.ToString();
                if (edit.Length > 1) { MessageBox.Show("Необходимо ввести только один символ"); dataMachine[e.ColumnIndex, e.RowIndex].Value = ' '; return; }
                if (CurrentAlphabet.Contains(edit[0]))
                {
                    int ind = Convert.ToInt32(dataMachine.Columns[e.ColumnIndex].HeaderText) + shift;
                    TapeTur[ind] = edit[0];
                    label1.Text = ind.ToString() + " " + TapeTur[ind];
                }
                else if (edit[0] == '_') { dataMachine[e.ColumnIndex, e.RowIndex].Value = ' '; }
                else { MessageBox.Show("Символ отсутствует в алфавите"); dataMachine[e.ColumnIndex, e.RowIndex].Value = ' '; return; }
                //if(dataMachine[e.ColumnIndex,e.RowIndex].Value.ToString().Contains)
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "TuringTapeEditError");
                //   throw;
            }
        }

        private void StepRigth_Click(object sender, EventArgs e)
        {
            RunStepRigth();
        }

        private void RunStepRigth()
        {
            try
            {
                for (int i = -15; i < 16; i++)
                {
                    int check = Convert.ToInt32(this.dataMachine.Columns[i + 15].HeaderText);
                    if (check > 399) { MessageBox.Show("ЛЕНТА ЗАКОНЧИЛАСЬ"); return; }
                    this.dataMachine.Columns[i + 15].HeaderText = (check + 1).ToString();
                    this.dataMachine[i + 15, 0].Value = TapeTur[check + shift + 1];
                }
                CurIndex = Convert.ToInt32(this.dataMachine.Columns[15].HeaderText) + shift;
                // label1.Text = CurIndex.ToString() + " " + TapeTur[CurIndex];
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "StepRigthError");
                //   throw;
            }
        }
        private void StepLeft_Click(object sender, EventArgs e)
        {
            RunStepLeft();
        }

        private void RunStepLeft()
        {
            try
            {
                for (int i = -15; i < 16; i++)
                {
                    int check = Convert.ToInt32(this.dataMachine.Columns[i + 15].HeaderText);
                    if (check < -399) { MessageBox.Show("ЛЕНТА ЗАКОНЧИЛАСЬ"); return; }
                    this.dataMachine.Columns[i + 15].HeaderText = (check - 1).ToString();
                    this.dataMachine[i + 15, 0].Value = TapeTur[check + shift - 1];
                }
                CurIndex = Convert.ToInt32(this.dataMachine.Columns[15].HeaderText) + shift;
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "StepLeftError");
                //   throw;
            }
            //label1.Text = CurIndex.ToString() + " " + TapeTur[CurIndex];
        }

        private void Run_Click(object sender, EventArgs e)
        {
            //int CurSmb,i=0;
            try
            {
                toolStripButton3.Enabled = false;
                stopbtn.Enabled = true;
                pausebtn.Enabled = true;
                timer1.Interval = speed;
                timer1.Enabled = true;
                
                Run.Enabled = false;

                dataMachine.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightGreen;
                dataMachine.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                dataTable.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightGreen;
                dataTable.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "RunClickError");
                //   throw;
            }
                
                
                /* while (CurState != 0)
             {
               
                    
             }*/
            // CurState = 1;
        }


        private void txtDesc_TextChanged(object sender, EventArgs e)
        {
          //  WindowsFormsApplication1.dataRef.textTaskRef = txtDesc.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        Edge pastEdge = null;
        Microsoft.Glee.Drawing.Color pastColor;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Interval = speed;
                CurCell cr = new CurCell();


                int CurSmb, i = 0;
                //     label1.Text = CurIndex.ToString() + " " + TapeTur[CurIndex];    
                while (TapeTur[CurIndex] != CurrentAlphabet[i])
                { i++; }


                CurSmb = i;



                //MessageBox.Show("ALSDL");
                dataTable.CurrentCell = dataTable[CurState - 1, CurSmb];

                if (dataTable.CurrentCell.Value == null) { stop(); MessageBox.Show("Отсутствует команда в ячейке состояния " + CurState.ToString() + " для символа " + TapeTur[CurIndex]); return; }
                cr.ReadFromTable(dataTable.CurrentCell.Value.ToString());
                //string str = dataTable.CurrentCell.Value.ToString();
                label1.Text += cr.smb + " " + cr.action + " " + cr.stateNext.ToString() + " ";
                dataMachine[15, 0].Value = cr.smb.ToString();
                TapeTur[CurIndex] = cr.smb;
                if (cr.action == '>') { /*StepRigth_Click(sender, e);*/  RunStepRigth(); }
                else if (cr.action == '<') { /* StepLeft_Click(sender, e);*/RunStepLeft(); }
                else if (cr.action == '.') { }

                if (WindowsFormsApplication1.dataRef.gvRef != null)
                {
                    if (PastState != 0)
                    {
                        WindowsFormsApplication1.dataRef.grafRef.FindNode("Q" + PastState.ToString()).Attr.Fillcolor = Microsoft.Glee.Drawing.Color.White;

                        if (pastEdge != null)
                        {
                            pastEdge.Attr.Color = pastColor;
                            pastEdge.Attr.LineWidth = 1;

                        }
                        WindowsFormsApplication1.dataRef.gvRef.Graph = WindowsFormsApplication1.dataRef.grafRef;
                    }
                    PastState = CurState;

                    Node temp = WindowsFormsApplication1.dataRef.grafRef.FindNode("Q" + CurState.ToString());
                    temp.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightGreen;


                    List<Edge> curS = temp.OutEdges.ToList();

                    List<Edge> curSL = temp.SelfEdges.ToList();
                    //MessageBox.Show((dataTable[0, CurSmb].Value.ToString()[0] + "/" + (cr.smb==' ' ? "_" : cr.smb.ToString() ) + cr.action));
                    foreach (Edge element in curS)
                    {
                        if (element.Attr.Label.Trim() == ((txtAlph.Text[CurSmb] == ' ' ? '_' : txtAlph.Text[CurSmb]) + "/" + (cr.smb == ' ' ? "_" : cr.smb.ToString()) + cr.action))
                        {

                            pastColor = element.Attr.Color;
                            element.Attr.Color = Microsoft.Glee.Drawing.Color.Green;
                            element.Attr.LineWidth = 4;
                            pastEdge = element;

                            break;
                        }
                    }
                    foreach (Edge element in curSL)
                    {
                        if (element.Attr.Label.Trim() == ((txtAlph.Text[CurSmb] == ' ' ? '_' : txtAlph.Text[CurSmb]) + "/" + (cr.smb == ' ' ? "_" : cr.smb.ToString()) + cr.action))
                        {
                            pastColor = element.Attr.Color;
                            element.Attr.Color = Microsoft.Glee.Drawing.Color.Green;
                            element.Attr.LineWidth = 4;
                            pastEdge = element;
                            break;
                        }

                    }
                    WindowsFormsApplication1.dataRef.gvRef.Graph = WindowsFormsApplication1.dataRef.grafRef;
                }

                CurState = cr.stateNext;
                i = 0;
                // System.Threading.Thread.Sleep(1000);
                //if (stepbstep == 1) { pausebtn_Click(null, null); }

                if (CurState == 0)
                {

                    stop();
                    stepbstep = 0;
                    //    MessageBox.Show("Выполнение завершено");
                    dataMachine.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
                }


            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "RunningError");
                //   throw;
            }


        }

        private void speed4_Click(object sender, EventArgs e)
        {
            speed = 30;
        }

        private void speed2_Click(object sender, EventArgs e)
        {
            speed = 250;
        }

        private void speed1_Click(object sender, EventArgs e)
        {
            speed = 500;
        }

        private void speed05_Click(object sender, EventArgs e)
        {
            speed = 1000;
        }

        private void speed025_Click(object sender, EventArgs e)
        {
            speed = 2000;
        }

        private void выполнитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run_Click(sender, e);
        }

        private void шагВпередToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CurCell cr = new CurCell();


                int CurSmb, i = 0;
                //     label1.Text = CurIndex.ToString() + " " + TapeTur[CurIndex];    
                while (TapeTur[CurIndex] != CurrentAlphabet[i])
                { i++; }


                CurSmb = i;



                //MessageBox.Show("ALSDL");
                dataTable.CurrentCell = dataTable[CurState - 1, CurSmb];

                if (dataTable.CurrentCell.Value == null) { stop(); MessageBox.Show("Отсутствует команда в ячейке состояния " + CurState.ToString() + " для символа " + TapeTur[CurIndex]); return; }
                cr.ReadFromTable(dataTable.CurrentCell.Value.ToString());
                //string str = dataTable.CurrentCell.Value.ToString();
                label1.Text += cr.smb + " " + cr.action + " " + cr.stateNext.ToString() + " ";
                dataMachine[15, 0].Value = cr.smb.ToString();
                TapeTur[CurIndex] = cr.smb;
                if (cr.action == '>') { /*StepRigth_Click(sender, e);*/  RunStepRigth(); }
                else if (cr.action == '<') { /* StepLeft_Click(sender, e);*/RunStepLeft(); }
                else if (cr.action == '.') { }

                if (WindowsFormsApplication1.dataRef.gvRef != null)
                {
                    if (PastState != 0)
                    {
                        WindowsFormsApplication1.dataRef.grafRef.FindNode("Q" + PastState.ToString()).Attr.Fillcolor = Microsoft.Glee.Drawing.Color.White;

                        if (pastEdge != null)
                        {
                            pastEdge.Attr.Color = pastColor;
                            pastEdge.Attr.LineWidth = 1;

                        }
                        WindowsFormsApplication1.dataRef.gvRef.Graph = WindowsFormsApplication1.dataRef.grafRef;
                    }
                    PastState = CurState;

                    Node temp = WindowsFormsApplication1.dataRef.grafRef.FindNode("Q" + CurState.ToString());
                    temp.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightGreen;


                    List<Edge> curS = temp.OutEdges.ToList();

                    List<Edge> curSL = temp.SelfEdges.ToList();
                    //MessageBox.Show((dataTable[0, CurSmb].Value.ToString()[0] + "/" + (cr.smb==' ' ? "_" : cr.smb.ToString() ) + cr.action));
                    foreach (Edge element in curS)
                    {
                        if (element.Attr.Label.Trim() == ((txtAlph.Text[CurSmb] == ' ' ? '_' : txtAlph.Text[CurSmb]) + "/" + (cr.smb == ' ' ? "_" : cr.smb.ToString()) + cr.action))
                        {

                            pastColor = element.Attr.Color;
                            element.Attr.Color = Microsoft.Glee.Drawing.Color.Green;
                            element.Attr.LineWidth = 4;
                            pastEdge = element;

                            break;
                        }
                    }
                    foreach (Edge element in curSL)
                    {
                        if (element.Attr.Label.Trim() == ((txtAlph.Text[CurSmb] == ' ' ? '_' : txtAlph.Text[CurSmb]) + "/" + (cr.smb == ' ' ? "_" : cr.smb.ToString()) + cr.action))
                        {
                            pastColor = element.Attr.Color;
                            element.Attr.Color = Microsoft.Glee.Drawing.Color.Green;
                            element.Attr.LineWidth = 4;
                            pastEdge = element;
                            break;
                        }

                    }
                    WindowsFormsApplication1.dataRef.gvRef.Graph = WindowsFormsApplication1.dataRef.grafRef;
                }

                CurState = cr.stateNext;
                i = 0;
                // System.Threading.Thread.Sleep(1000);
                //if (stepbstep == 1) { pausebtn_Click(null, null); }

                if (CurState == 0)
                {

                    stop();
                    stepbstep = 0;
                    //    MessageBox.Show("Выполнение завершено");
                    dataMachine.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
                }
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "StepByStepError");
                //   throw;
            }
        }

        private void стопToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stop();
        }

        private void stopbtn_Click(object sender, EventArgs e)
        {
            stop();
        }

        private void stop()
        {
            try
            {
                timer1.Enabled = false;
                Run.Enabled = true;
                stopbtn.Enabled = false;
                pausebtn.Enabled = false;
                toolStripButton3.Enabled = true;
                CurState = 1;

                MessageBox.Show("Выполнение остановлено");
                dataMachine.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
                dataMachine.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dataTable.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
                dataTable.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                stepbstep = 0;
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "StopError");
                //   throw;
            }
        }

        private void pausebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (pause == 0)
                {
                    timer1.Enabled = false; pause = 1; Run.Enabled = true; stopbtn.Enabled = false;
                    pausebtn.Enabled = false; toolStripButton3.Enabled = true ; dataMachine.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
                    dataMachine.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }
                else
                {
                    timer1.Enabled = true; stopbtn.Enabled = true; toolStripButton3.Enabled = false;
                    pausebtn.Enabled = true; pause = 0; dataMachine.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightGreen;
                    dataMachine.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                }
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "PauseError");
                //   throw;
            }
        }

        private void dataTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] bt = new byte[20000];
                int index = 0;
                byte count = 0;
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != "")
                {
                    //  System.IO.StreamReader 
                    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                    int intcount;
                    string wrk = txtDesc.Text;
                    wrk = wrk.Replace("\n", "\r\n");
                    intcount = wrk.Length;
                    if (intcount < 256) { bt[0] = Convert.ToByte(intcount); }
                    else { bt[1] = Convert.ToByte(intcount / 255); bt[0] = Convert.ToByte((intcount % 255) - bt[1]); }
                    label1.Text = bt[0].ToString() + " " + bt[1].ToString();
                    index = 4;
                    for (int i = 0; i < 4; i++)
                    {
                        fs.WriteByte(bt[i]);
                    }
                    bt = System.Text.Encoding.Default.GetBytes(wrk.ToCharArray());


                    /* txtDesc.Text = "";
                    for (int i = 0; i < bt.Length; i++) { 
                        txtDesc.Text += bt[i].ToString()+" ";
                    }*/

                    for (int i = 0; i < bt.Length; i++)
                    {
                        fs.WriteByte(bt[i]);
                        bt[i] = 0;
                    }

                    fs.WriteByte(Convert.ToByte(NofStates + 1));
                    wrk = "";
                    int j = 0;
                    for (int i = 0; i < NofStates; i++)
                    {
                        wrk += "\tQ" + (i + 1).ToString();
                        j += 3;
                    }

                    wrk += "\r\n";
                    j++;
                    // txtDesc.Text += j.ToString()+" \n";
                    for (int i = 0; i < CurSizeAlph; i++)
                    {
                        if (dataTable.Rows[i].HeaderCell.Value.ToString() != "_") { wrk += dataTable.Rows[i].HeaderCell.Value.ToString(); }
                        else { wrk += " "; }
                        j += 1;
                        for (int k = 0; k < NofStates; k++)
                        {
                            if ((dataTable[k, i].Value != null) && (dataTable[k, i].Value.ToString().Length > 2)) { wrk += "\t" + dataTable[k, i].Value.ToString().Substring(0, 3); j += 4; }
                            else { wrk += "\t"; j++; }
                            // txtDesc.Text += j.ToString()+" ";
                        }

                        wrk += "\r\n";
                        j++;
                        // txtDesc.Text += j.ToString()+" \n";
                    }
                    //wrk = wrk.Replace("\n", "\r\n");
                    j += CurSizeAlph + 1;
                    if (j < 256) { bt[3] = Convert.ToByte(j); }
                    { bt[4] = Convert.ToByte(j / 255); bt[3] = Convert.ToByte((j % 255)); }

                    for (int i = 0; i < 7; i++)
                    {
                        fs.WriteByte(bt[i]);
                        bt[i] = 0;
                    }

                    bt = System.Text.Encoding.Default.GetBytes(wrk.ToCharArray());


                    for (int i = 0; i < bt.Length; i++)
                    {
                        fs.WriteByte(bt[i]);
                        bt[i] = 0;
                    }

                    wrk = richTextBox1.Text;
                    wrk = wrk.Replace("\n", "\r\n");
                    intcount = wrk.Length;
                    if (intcount < 256) { bt[0] = Convert.ToByte(intcount); }
                    else { bt[1] = Convert.ToByte(intcount / 255); bt[0] = Convert.ToByte((intcount % 255) - bt[1]); }

                    // label1.Text = bt[0].ToString() + " " + bt[1].ToString();
                    index = 4;
                    for (int i = 0; i < 4; i++)
                    {
                        fs.WriteByte(bt[i]);
                    }
                    //wrk += "d   c       ";
                    bt = System.Text.Encoding.Default.GetBytes(wrk.ToCharArray());

                    for (int i = 0; i < bt.Length; i++)
                    {
                        fs.WriteByte(bt[i]);
                        bt[i] = 0;
                    }


                    fs.WriteByte(100);

                    fs.WriteByte(0);
                    fs.WriteByte(0);
                    fs.WriteByte(0);

                    fs.WriteByte(99);

                    fs.WriteByte(0);
                    fs.WriteByte(0);
                    fs.WriteByte(0);
                    fs.WriteByte(0);
                    fs.WriteByte(0);
                    fs.WriteByte(0);
                    fs.WriteByte(0);

                    fs.Close();
                }

            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "SaveError");
                //   throw;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                CurCell cr = new CurCell();


                int CurSmb, i = 0;
                //     label1.Text = CurIndex.ToString() + " " + TapeTur[CurIndex];    
                while (TapeTur[CurIndex] != CurrentAlphabet[i])
                { i++; }


                CurSmb = i;



                //MessageBox.Show("ALSDL");
                dataTable.CurrentCell = dataTable[CurState - 1, CurSmb];

                if (dataTable.CurrentCell.Value == null) { stop(); MessageBox.Show("Отсутствует команда в ячейке состояния " + CurState.ToString() + " для символа " + TapeTur[CurIndex]); return; }
                cr.ReadFromTable(dataTable.CurrentCell.Value.ToString());
                //string str = dataTable.CurrentCell.Value.ToString();
                label1.Text += cr.smb + " " + cr.action + " " + cr.stateNext.ToString() + " ";
                dataMachine[15, 0].Value = cr.smb.ToString();
                TapeTur[CurIndex] = cr.smb;
                if (cr.action == '>') { /*StepRigth_Click(sender, e);*/  RunStepRigth(); }
                else if (cr.action == '<') { /* StepLeft_Click(sender, e);*/RunStepLeft(); }
                else if (cr.action == '.') { }

                if (WindowsFormsApplication1.dataRef.gvRef != null)
                {
                    if (PastState != 0)
                    {
                        WindowsFormsApplication1.dataRef.grafRef.FindNode("Q" + PastState.ToString()).Attr.Fillcolor = Microsoft.Glee.Drawing.Color.White;

                        if (pastEdge != null)
                        {
                            pastEdge.Attr.Color = pastColor;
                            pastEdge.Attr.LineWidth = 1;

                        }
                        WindowsFormsApplication1.dataRef.gvRef.Graph = WindowsFormsApplication1.dataRef.grafRef;
                    }
                    PastState = CurState;

                    Node temp = WindowsFormsApplication1.dataRef.grafRef.FindNode("Q" + CurState.ToString());
                    temp.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightGreen;


                    List<Edge> curS = temp.OutEdges.ToList();

                    List<Edge> curSL = temp.SelfEdges.ToList();
                    //MessageBox.Show((dataTable[0, CurSmb].Value.ToString()[0] + "/" + (cr.smb==' ' ? "_" : cr.smb.ToString() ) + cr.action));
                    foreach (Edge element in curS)
                    {
                        if (element.Attr.Label.Trim() == ((txtAlph.Text[CurSmb] == ' ' ? '_' : txtAlph.Text[CurSmb]) + "/" + (cr.smb == ' ' ? "_" : cr.smb.ToString()) + cr.action))
                        {

                            pastColor = element.Attr.Color;
                            element.Attr.Color = Microsoft.Glee.Drawing.Color.Green;
                            element.Attr.LineWidth = 4;
                            pastEdge = element;

                            break;
                        }
                    }
                    foreach (Edge element in curSL)
                    {
                        if (element.Attr.Label.Trim() == ((txtAlph.Text[CurSmb] == ' ' ? '_' : txtAlph.Text[CurSmb]) + "/" + (cr.smb == ' ' ? "_" : cr.smb.ToString()) + cr.action))
                        {
                            pastColor = element.Attr.Color;
                            element.Attr.Color = Microsoft.Glee.Drawing.Color.Green;
                            element.Attr.LineWidth = 4;
                            pastEdge = element;
                            break;
                        }

                    }
                    WindowsFormsApplication1.dataRef.gvRef.Graph = WindowsFormsApplication1.dataRef.grafRef;
                }

                CurState = cr.stateNext;
                i = 0;
                // System.Threading.Thread.Sleep(1000);
                //if (stepbstep == 1) { pausebtn_Click(null, null); }

                if (CurState == 0)
                {

                    stop();
                    stepbstep = 0;
                    //    MessageBox.Show("Выполнение завершено");
                    dataMachine.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
                }
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "StepByStepError");
                //   throw;
            }
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {
                SYS = 1;
                dataMachine.Columns.Clear();
                dataTable.Columns.Clear();

                NofStates = 1;
                shift = 400;

                CurState = 1;
                PastState = 0;
                speed = 500;
                pause = 0;

                stepbstep = 0;

                txtDesc.Text = "";
                richTextBox1.Text = "";
                txtAlph.Text = "01";
                CurrentAlphabet = txtAlph.Text + ' ';
                CurSizeAlph = CurrentAlphabet.Length;


                var colm = new DataGridViewTextBoxColumn();
                colm.Width = 50;
                colm.SortMode = DataGridViewColumnSortMode.NotSortable;
                colm.HeaderText = "Q1";
                dataTable.Columns.Add(colm);



                for (int i = 0; i < CurSizeAlph; i++)
                {
                    var row = new DataGridViewRow();
                    if (CurrentAlphabet[i] != ' ') { row.HeaderCell.Value = CurrentAlphabet[i].ToString(); }
                    else { row.HeaderCell.Value = "_"; }
                    this.dataTable.Rows.Insert(i, row);
                    //  this.dataTable.Rows[1].HeaderCell.Value
                }

                for (int i = -15; i < 16; i++)
                {
                    var col = new DataGridViewTextBoxColumn();
                    this.dataMachine.Columns.Add(col);
                    this.dataMachine.Columns[i + 15].HeaderText = i.ToString();
                    this.dataMachine.Columns[i + 15].Width = 25;
                    this.dataMachine.Columns[i + 15].Resizable = 0;

                }

                this.dataMachine.Rows.Add();
                this.dataMachine[15, 0].Style.BackColor = System.Drawing.Color.Khaki;

                CurIndex = shift + 100;
                for (int i = 0; i < 1100; i++)
                {
                    TapeTur[i] = ' ';
                }
                this.dataMachine.AllowUserToResizeColumns = false;
                this.dataMachine.AllowUserToResizeRows = false;
                this.dataMachine.AllowUserToOrderColumns = false;
                CurIndex = shift;
                SYS = 0;
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "CreateNewError");
                //   throw;
            }
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton7_Click(sender, e);
        }

        private void авторыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void сохранитьЛентуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int f = 0;
                saveCurrentIndex = CurIndex;
                for (int i = -15; i < 16; i++)
                {
                    if (this.dataMachine[i + 15, 0].Value == null) { SaveTurCurrent[i + 15] = ' '; }
                    else { SaveTurCurrent[i + 15] = this.dataMachine[i + 15, 0].Value.ToString()[0]; }
                }

                for (int i = 0; i < 1100; i++)
                {
                    SaveTapeTur[i] = TapeTur[i];
                    if ((f == 0) && (TapeTur[i] != ' ')) { f = 1; NearestIndex = i; }
                }
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "SaveTapeError");
                //   throw;
            }
        }

        private void восстановитьЛентуToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // CurIndex = saveCurrentIndex;
            try
            {
                if (CurIndex < saveCurrentIndex) { while (CurIndex != saveCurrentIndex) RunStepRigth(); }
                else if (CurIndex > saveCurrentIndex) { while (CurIndex != saveCurrentIndex) RunStepLeft(); }

                for (int i = -15; i < 16; i++)
                {
                    this.dataMachine[i + 15, 0].Value = SaveTurCurrent[i + 15];
                }

                for (int i = 0; i < 1100; i++)
                {
                    TapeTur[i] = SaveTapeTur[i];
                }
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "LoadTapeError");
                //   throw;
            }
        }

        private void dataMachine_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (allowkeymouse )
                {
                    WindowsFormsApplication1.dataAlph.Alph = CurrentAlphabet;
                    WindowsFormsApplication1.dataAlph.X = Cursor.Position.X;
                    WindowsFormsApplication1.dataAlph.Y = Cursor.Position.Y;
                    WindowsFormsApplication1.dataAlph.index = -1;

                    int c = e.ColumnIndex, r = e.RowIndex;
                    WindowsFormsApplication1.Form3 frm = new Form3();
                    //this.Enabled = false;
                    frm.ShowDialog();
                    
                    if (WindowsFormsApplication1.dataAlph.index != -1)
                    {
                        char str = CurrentAlphabet[WindowsFormsApplication1.dataAlph.index];
                        this.dataMachine[c, r].Value = str;
                        int ind = Convert.ToInt32(dataMachine.Columns[e.ColumnIndex].HeaderText) + shift;
                        TapeTur[ind] = str;
                    }
                }
            }
            catch (Exception ee)
            {
                // Extract some information from this exception, and then 
                // throw it to the parent method.
                // if (ee.Source != null)
                MessageBox.Show(ee.ToString(), "AddWithMouseError");
                //   throw;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataMachine.ReadOnly = false;
            allowkeymouse = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataMachine.ReadOnly = true;
            allowkeymouse = true;
        }

        private void авторыToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            WindowsFormsApplication1.Form4 frm = new Form4();
            frm.ShowDialog();

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            dataMachine.ReadOnly = false;
            allowkeymouse = true;
        }

        private void dataMachine_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                try
                {
                    if (allowkeymouse)
                    {
                        WindowsFormsApplication1.dataAlph.Alph = CurrentAlphabet;
                        WindowsFormsApplication1.dataAlph.X = Cursor.Position.X;
                        WindowsFormsApplication1.dataAlph.Y = Cursor.Position.Y;
                        WindowsFormsApplication1.dataAlph.index = -1;

                        int c = e.ColumnIndex, r = e.RowIndex;
                        WindowsFormsApplication1.Form3 frm = new Form3();
                        //this.Enabled = false;
                        frm.ShowDialog();

                        if (WindowsFormsApplication1.dataAlph.index != -1)
                        {
                            char str = CurrentAlphabet[WindowsFormsApplication1.dataAlph.index];
                            this.dataMachine[c, r].Value = str;
                            int ind = Convert.ToInt32(dataMachine.Columns[e.ColumnIndex].HeaderText) + shift;
                            TapeTur[ind] = str;
                        }
                    }
                }
                catch (Exception ee)
                {
                    // Extract some information from this exception, and then 
                    // throw it to the parent method.
                    // if (ee.Source != null)
                    MessageBox.Show(ee.ToString(), "AddWithMouseError");
                    //   throw;
                }
            }
        }
    }
}
