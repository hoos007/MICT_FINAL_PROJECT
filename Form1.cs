using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MICT_Final_Project
{
    public partial class Form1 : Form
    {
        private static int setImgCount;
        private static List<String> names = new List<String>();
        public Form1()
        {
            InitializeComponent();
        }

        private String openFile(int type)
        {
            openFileDialog1.Title = "영상 파일 열기";
            openFileDialog1.Filter = " All Files(*.*) |*.*| Bitmap File(*.bmp)| *.bmp | Jpeg File(*.jpg) | *.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strFilename = openFileDialog1.FileName;
                HopFieldProcess.setImage(Image.FromFile(strFilename),type);
                return strFilename;
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String name;
            name = openFile(0);

            if (name != null)
            {
                HopFieldProcess.wplus();
                setImgCount++;
                textBox1.AppendText(Convert.ToString(setImgCount) + "번 이미지 학습 : " + Path.GetFileNameWithoutExtension(name) + "\r\n");
                names.Add(Path.GetFileNameWithoutExtension(name));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFile(1);
            HopFieldProcess.process();
            
            textBox1.AppendText("\r\n연산완료");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = HopFieldProcess.testImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int result;
            result = HopFieldProcess.resultCheck();
            
            textBox1.Text = names[result];
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = HopFieldProcess.image[result];
        }
    }
}
