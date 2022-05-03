using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace MEDCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string word1 = this.textBox1.Text;
            string word2 = this.textBox2.Text;

            DataGridView grid = (DataGridView)this.panel1.Controls[0];
            if (grid.ColumnCount > 0)
            {
                clearGrid();
            }
            word1 = word1.Trim();
            word1 = word1.ToLower();
            word2 = word2.Trim();
            word2 = word2.ToLower();

            if (word1 == "" || word2 == "")
            {
                System.Windows.Forms.MessageBox.Show("Empty blanks or wrong format!!\nplease try again!", "Input Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearTexts();
            }
            else
            {
                this.sw.Start();
                int x = word1.Length;
                int y = word2.Length;
                grid.Columns.Add("col0", "#");
                grid.Rows.Add();
                grid.Columns[0].Width = 40;
                grid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                grid.Rows[0].HeaderCell.Value = "#";
                grid.Rows[0].Cells[0].Value = 0;

                MedCell[,] medCells = new MedCell[x + 1,y+1];
                medCells[0, 0] = new MedCell(0, null, "");
                
                for(int i = 0; i < x; i++)
                {
                    grid.Rows.Add();
                    grid.Rows[i + 1].HeaderCell.Value = word1[i]+"";
                    grid.Rows[i + 1].Cells[0].Value = i + 1;
                    medCells[i + 1, 0] = new MedCell(i+1, medCells[i,0], "Delete");
                }
                for(int j = 0; j < y; j++)
                {
                    grid.Columns.Add("col" + (j + 1), word2[j]+"");
                    grid.Columns[j+1].Width = 40;
                    grid.Columns[j+1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    grid.Rows[0].Cells[j + 1].Value = j + 1;
                    medCells[0, j+1] = new MedCell(j+1, medCells[0, j], "Insertion");
                }

                MedCell result = calculateMED(medCells);
                int insertion = 0, delete = 0, replace = 0, copy = 0;
                grid.Rows[x].Cells[y].Style.BackColor = Color.Chocolate;
                while (result.parent != null)
                {
                    if (result.method == "Insertion")
                    {
                        insertion++;
                        y--;
                    }
                    else if (result.method == "Delete")
                    {
                        delete++;
                        x--;
                    }
                    else if (result.method == "Replace")
                    {
                        replace++;
                        x--;
                        y--;
                    }
                    else if (result.method == "Copy")
                    {
                        copy++;
                        x--;
                        y--;
                    }
                    grid.Rows[x].Cells[y].Style.BackColor = Color.Chocolate;
                    result = result.parent;
                }
                this.textBox3.Text = word1 + " changed to " + word2 + " with " + insertion + " Insertions " + delete + " Deletes " + replace + " Replaces" 
                    + Environment.NewLine + "( " + copy + " Copies )" + "   MED DISTANCE : " + (insertion+delete+replace);
                this.sw.Stop();
                this.textBox3.AppendText("    Running Time: " + this.sw.Elapsed.TotalMilliseconds + " Milliseconds");
                this.sw.Reset();
                grid.ClearSelection();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearTexts();
            clearGrid();
        }

        private void clearGrid()
        {
            DataGridView grid = (DataGridView)this.panel1.Controls[0];
            grid.Rows.Clear();
            grid.Columns.Clear();
        }

        private void clearTexts()
        {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
        }

        private MedCell calculateMED(MedCell[,] cells)
        {
            DataGridView grid = (DataGridView)this.panel1.Controls[0];
            for (int i = 1; i < cells.GetLength(0); i++)
            {
                for(int j = 1; j < cells.GetLength(1); j++)
                {
                    if(grid.Columns[j].HeaderText == grid.Rows[i].HeaderCell.Value.ToString())
                        cells[i, j] = new MedCell(cells[i - 1, j - 1].value, cells[i - 1, j - 1], "Copy");
                    else
                    {
                        if(cells[i-1,j-1].value <= cells[i-1,j].value && cells[i-1,j-1].value <= cells[i,j-1].value)
                            cells[i, j] = new MedCell(cells[i - 1, j - 1].value+1, cells[i - 1, j - 1], "Replace");
    
                        else if (cells[i - 1, j].value <= cells[i - 1, j - 1].value && cells[i - 1, j].value <= cells[i, j - 1].value)
                            cells[i,j] = new MedCell(cells[i - 1, j].value + 1, cells[i - 1, j], "Delete");
 
                        else if (cells[i, j - 1].value <= cells[i-1,j-1].value && cells[i, j - 1].value <= cells[i-1,j].value)
                            cells[i, j] = new MedCell(cells[i, j-1].value + 1, cells[i, j-1], "Insertion");                   
                    }
                    grid.Rows[i].Cells[j].Value = cells[i, j].value;
                }
            }
            return cells[cells.GetLength(0)-1, cells.GetLength(1)-1];
        }
    }
}
