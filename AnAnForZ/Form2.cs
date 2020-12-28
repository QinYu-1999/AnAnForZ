using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AnAnForZ
{
    public partial class Form2 : Form
    {
        /*线性列表用于图表显示*/
        private List<int> List_ia = new List<int>();    //电流ia
        Random random = new Random();
        public Form2()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int num1 = random.Next(0, 99);   //随机数
            int num2 = random.Next(0, 99);
            int num3 = random.Next(0, 99);
            int num4 = random.Next(0, 99);

            List_ia.Add(num1); ListRemove(List_ia);
            DrawChart(List_ia);
        }

        private void DrawChart(List<int> List_ia)
        {
            chart1.Series[0].Points.Clear();    //清除所有点

            for (int i = 0; i < List_ia.Count; i++)
            {
                chart1.Series[0].Points.AddXY(i + 1, List_ia[i]);   //添加点
            }
        }

        //当大于80时移除list头部
        private void ListRemove(List<int> list)
        {
            if (list.Count >= 80)
            {
                list.RemoveAt(0);
            }

        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if (button_start.Text == "开始")
            {
                timer1.Start();
                button_start.Text = "暂停";
            }
            else
            {
                timer1.Stop();
                button_start.Text = "开始";
            }

        }


        private void chart1_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {

            HitTestResult myTestResult = chart1.HitTest(e.X, e.Y, ChartElementType.DataPoint);//获取命中测试的结果
            if (myTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = myTestResult.PointIndex;
                DataPoint dp = myTestResult.Series.Points[i];
                string XValue = dp.XValue.ToString();//获取数据点的X值
                string YValue = dp.YValues[0].ToString();//获取数据点的Y值
                e.Text = "道址:" + XValue + "\r\n数值" + YValue;
            }

        }
    }
}
