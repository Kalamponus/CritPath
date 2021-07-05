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

namespace Dialog
{
    public partial class Form1 : Form
    {
        string dataPath;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            var location = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.InitialDirectory = location;
            openFileDialog1.Filter = "csv files (*.csv)|*.csv";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileNames.Length > 1)
                    MessageBox.Show("Нельзя загрузить больше одного файла!");
                else if (openFileDialog1.FileNames.Length == 1)
                {
                    dataPath = openFileDialog1.FileNames[0];
                }
                else
                    MessageBox.Show("Выберите файл!");
            }
        }

        private void buttonMakeSolutionAndSave_Click(object sender, EventArgs e)
        {
            Stream myStream;
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    string path = saveFileDialog1.FileName;
                    if (path != null && path != "")
                    {
                        //CriticalPath cp = new CriticalPath(dataPath, path);
                        //cp.CalculateCriticalPath();
                        //MinimalPath mp = new MinimalPath(dataPath, path);
                        //mp.CalculateMinimalPath();
                        JohnsonMethod jm = new JohnsonMethod(dataPath, path);
                        jm.Calculate();
                        MessageBox.Show("Решение сохранено!");
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}
