using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ubiety.Dns.Core;

namespace AnAnForZ
{
    public partial class Data1 : Form
    {
        public Data1()
        {
            InitializeComponent();
        }

        private void Data1_Load(object sender, EventArgs e)
        {
            createImage();
        }

        private void createImage()
        {
            int height = 480, width = 700;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            try
            {
                g.Clear(Color.White);  //清除图片背景色
                //样式
                Font font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
                Font font1 = new System.Drawing.Font("宋体", 20, FontStyle.Regular);
                Font font2 = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                //线性矩形
                LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height), Color.Blue, Color.Blue, 1.0f, true);

                g.FillRectangle(Brushes.AliceBlue, 0, 0, width, height);
                //指定画笔
                Brush brush1 = new SolidBrush(Color.Blue);
                Brush brush2 = new SolidBrush(Color.SaddleBrown);

                g.DrawString("串口通信数据折线图", font1, brush1, new PointF(85, 30));

                //矩形的边框线
                g.DrawRectangle(new Pen(Color.Blue), 0, 0, bitmap.Width - 1, bitmap.Height - 1);

                //绘制线条
                Pen thePen = new Pen(brush, 1);
                Pen pen = new Pen(Color.Blue, 3);

                //绘制纵向线条
                int a = 60;
                for(int i = 0; i < 8; i++)
                {
                    g.DrawLine(thePen, a, 80, a, 340);
                    a = a + 80;
                }
                a = 60;g.DrawLine(pen, a, 82, a, 340);

                //绘制横向线条
                int y = 106;
                for(int i = 0; i < 10; i++)
                {
                    g.DrawLine(thePen, 60, y, 620, y);
                    y = y + 26;
                }
                g.DrawLine(pen, 60, y - 26, 620, y - 26);

                //x轴
                String[] n = { "第一期", "第二期", "第三期", "第四期", "上半年", "下半年", "全年统计" };
                a = 45;
                for(int i = 0; i < 7; i++)
                {
                    g.DrawString(n[i].ToString(), font, Brushes.Blue, a, 348);
                    a = a + 77;
                }

                //y轴
                String[] m = { "220人", " 200人", " 175人", "150人", " 125人", " 100人", " 75人", " 50人"," 25人"};
                y = 100;
                for (int i = 0; i < 9; i++)
                {
                    g.DrawString(m[i].ToString(), font, Brushes.Red, 10, y); //设置文字内容及输出位置
                    y = y + 26;
                }

                //数据呈现区

                int[] count1 = new int[7];
                int[] count2 = new int[7];
                count1[0] = 15;count1[1] = 24;count1[2] = 26;count1[3] = 40;count1[4] = 20;
                count1[5] = 30;count1[6] = 34;
                count2[0] = 23;count2[1] = 40;count2[2] = 35;count2[3] = 50;count2[4] = 45;
                count2[5] = 33;count2[6] = 44;

                //显示折线
                Pen gPen = new Pen(Color.Red, 2);
                Font font3 = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                SolidBrush mybrush = new SolidBrush(Color.Red);
                Point[] points1 = new Point[7];
                points1[0].X = 60; points1[0].Y = 340 - count1[0]; //从106纵坐标开始, 到(0, 0)坐标时
                points1[1].X = 140; points1[1].Y = 340 - count1[1];
                points1[2].X = 220; points1[2].Y = 340 - count1[2];
                points1[3].X = 300; points1[3].Y = 340 - count1[3];

                points1[4].X = 380; points1[4].Y = 340 - count1[4];
                points1[5].X = 460; points1[5].Y = 340 - count1[5];

                points1[6].X = 540; points1[6].Y = 340 - count1[6];
                g.DrawLines(gPen, points1); //绘制折线

                //绘制数字
                //绘制数字
                g.DrawString(count1[0].ToString(), font3, Brushes.Red, 58, points1[0].Y - 20);
                g.DrawString(count1[1].ToString(), font3, Brushes.Red, 138, points1[1].Y - 20);
                g.DrawString(count1[2].ToString(), font3, Brushes.Red, 218, points1[2].Y - 20);
                g.DrawString(count1[3].ToString(), font3, Brushes.Red, 298, points1[3].Y - 20);
                g.DrawString(count1[4].ToString(), font3, Brushes.Red, 378, points1[4].Y - 20);
                g.DrawString(count1[5].ToString(), font3, Brushes.Red, 458, points1[5].Y - 20);
                g.DrawString(count1[6].ToString(), font3, Brushes.Red, 538, points1[6].Y - 20);

                Pen mypen3 = new Pen(Color.Green, 2);
                Point[] points2 = new Point[7];
                points2[0].X = 60; points2[0].Y = 340 - count2[0];
                points2[1].X = 140; points2[1].Y = 340 - count2[1];
                points2[2].X = 220; points2[2].Y = 340 - count2[2];
                points2[3].X = 300; points2[3].Y = 340 - count2[3];

                points2[4].X = 380; points2[4].Y = 340 - count2[4];
                points2[5].X = 460; points2[5].Y = 340 - count2[5];

                points2[6].X = 540; points2[6].Y = 340 - count2[6];
                g.DrawLines(mypen3, points2); //绘制折线

                //绘制通过人数
                g.DrawString(count2[0].ToString(), font3, Brushes.Green, 61, points2[0].Y - 15);
                g.DrawString(count2[1].ToString(), font3, Brushes.Green, 131, points2[1].Y - 15);
                g.DrawString(count2[2].ToString(), font3, Brushes.Green, 221, points2[2].Y - 15);
                g.DrawString(count2[3].ToString(), font3, Brushes.Green, 301, points2[3].Y - 15);

                g.DrawString(count2[4].ToString(), font3, Brushes.Green, 381, points2[4].Y - 15);
                g.DrawString(count2[5].ToString(), font3, Brushes.Green, 461, points2[5].Y - 15);

                g.DrawString(count2[6].ToString(), font3, Brushes.Green, 541, points2[6].Y - 15);

                //绘制标识
                g.DrawRectangle(new Pen(Brushes.Red), 180, 390, 250, 50); //绘制范围框
                g.FillRectangle(Brushes.Red, 270, 402, 20, 10); //绘制小矩形
                g.DrawString("报名人数", font2, Brushes.Red, 292, 400);

                g.FillRectangle(Brushes.Green, 270, 422, 20, 10);
                g.DrawString("通过人数", font2, Brushes.Green, 292, 420);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
               
/*                Response.ClearContent();
                Response.ContentType = "image/Jpeg";
                Response.BinaryWrite(ms.ToArray());*/
            }
            finally
            {
                g.Dispose();
                bitmap.Dispose();
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {

            string connString = "server=localhost;port=3306;user=root;password=root;database=o3o;Charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
                MessageBox.Show("连接成功！", "测试结果");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
