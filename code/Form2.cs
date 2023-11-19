using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pkglab3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            openFileDialog.Filter = "Файлы изображений (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif";
        }
        
        OpenFileDialog openFileDialog = new OpenFileDialog();
        string selectedFilePath;
        Bitmap bitmapImage;
        double thresholdValue, maxValue;
        int blockSize = 11;
        double C = 2;
        int operation;
        int constanta;

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = openFileDialog.FileName;
                bitmapImage = new Bitmap(selectedFilePath);
                Double.TryParse(numericUpDown1.Value.ToString(), out C);
                int.TryParse(numericUpDown2.Value.ToString(), out blockSize);
                int.TryParse(numericUpDown5.Value.ToString(), out constanta);
                Double.TryParse(numericUpDown4.Value.ToString(), out thresholdValue);
                Double.TryParse(numericUpDown3.Value.ToString(), out maxValue);
                
                    String str = comboBox1.Text.ToString();
                if (str == "Сложение")
                    operation = 1; 
                else if (str == "Умножение")
                    operation = 2;
                else if (str == "Логарифмическое")
                    operation = 3;
                else if (str == "Степенное")
                    operation = 4;
                else if (str == "Умножение на константу")
                    operation = 5;
                else if (str == "Добавление константы")
                    operation = 6;
                else
                    operation = 7;
                Form1 f = new Form1(bitmapImage, thresholdValue, maxValue, blockSize, C, operation, constanta);
                f.ShowDialog();
            }
           
        }
    }
}
