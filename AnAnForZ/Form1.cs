using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AnAnForZ
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;   //串口
        private Thread threadPort;     //接收串口数据线程
        private Boolean isOpen;       //判断串口是否打开
        private List<Double> list = new List<Double>();   //图表数据显示
        private Boolean startData;   //图形数据获取

        /**
         * 窗体线程的委托函数，解决窗体单线程的交互问题
         */
        private delegate void AddListCallback(String str);
        private delegate void AddLableText(String str);
        private delegate void AddRedText();
        private delegate void ClearText();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String[] portName = SerialPort.GetPortNames();
            for (int a = 0; a < portName.Count(); a++)
            {
                port1.Items.Add(portName[a]);
            }
            button1.Enabled = true;
            seeData.Visible = false;
            button2.Enabled = false;
            label4.Text = "0 v";
            label6.Text = "暂无人进入";
            for (int i = 0; i < 5; i++) { chart1.Series[0].Points.AddXY(i + 1, 0); }

        }

        private void portS_Click(object sender, EventArgs e)
        {
            String[] portName = SerialPort.GetPortNames();
            for (int a = 0; a < portName.Count(); a++)
            {
                port1.Items.Add(portName[a]);
            }
        }

        //串口打开
        public Boolean portOpen(String portName, Int32 baund)
        {
            serialPort = new SerialPort(portName);
            serialPort.BaudRate = baund;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Parity = Parity.None;
            serialPort.Encoding = System.Text.Encoding.ASCII;
            serialPort.Open();
            return serialPort.IsOpen;
        }

        //接收线程
        private void readData()
        {
            Byte[] portData = new Byte[30];
            while (isOpen)
            {
                try
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        Thread.Sleep(1000);
                        int readCount = serialPort.Read(portData, 0, portData.Length);
                        dataHandle(portData, readCount);
                    }
                } catch (Exception e)
                {
                    MessageBox.Show("出错：n串口开始关闭", e.Message);
                    isOpen = false;
                    clearDataText();
                    break;
                }
                Thread.Sleep(1000);
            }
            serialPort.Close();
            serialPort = null;
            clearDataText();
            MessageBox.Show("串口关闭成功！");
        }

        private void addSee(String str)
        {
            if (seeData.InvokeRequired) { AddListCallback d = new AddListCallback(addSee); this.Invoke(d, str); }
            else { seeData.Items.Add(str); seeData.SelectedIndex = seeData.Items.Count - 1; seeData.ClearSelected(); }
        }

        //byte转十六进制字符
        public String byteToS(Byte[] data, int start, int length)
        {
            StringBuilder str = new StringBuilder();
            for (int i = start; i < start + length; i++)
            {
                str.Append(data[i].ToString("X2"));
            }
            return str.ToString();
        }

        //数据处理
        private void dataHandle(Byte[] portData, int count)
        {
            addSee(String.Format("串口返回数据为:{0}", byteToS(portData, 0, count)));
            string A1 = byteToS(portData, 0, 1);
            if (A1 == "47")
            {
                //String B1 = byteToS(portData, 0, count);  //接收到的是气体节点的数据，将该数据进行封装
                gavDataHandle(portData, count);
            }
            else if (A1 == "D3")
            {
                redDataHandle();
            }
            else
            {
                clearDataText();
            }

        }
        //检测不到数据
        private void clearDataText()
        {
            if (label6.InvokeRequired)
            {
                ClearText d = new ClearText(clearDataText);
                this.Invoke(d);
            }
            else
            {
                label6.Text = "";
                label6.Text = "暂无人进入";
                label4.Text = "";
                label4.Text = "0 v";
            }

        }


        //发送数据
        public void sendData(Byte[] data) {

            if (!isOpen) { MessageBox.Show("未连接串口，请先进行连接"); };
            try {
                serialPort.Write(data, 0, data.Length);

            }
            catch(Exception ex) {
                MessageBox.Show("数据发送失败，错误信息为：{0}", ex.Message);
            }
        
        }

        //气体数据处理
        public void gavDataHandle(Byte[] portData,int count) 
        {
            int v1 = Convert.ToInt32(byteToS(portData, 4, 1),16); String a1 = Char.ConvertFromUtf32(v1);
            int v2 = Convert.ToInt32(byteToS(portData, 5, 1),16);String a2 = Char.ConvertFromUtf32(v2);
            int v3 = Convert.ToInt32(byteToS(portData, 6, 1),16);String a3 = Char.ConvertFromUtf32(v3);
            int v4 = Convert.ToInt32(byteToS(portData, 7, 1),16);String a4 = Char.ConvertFromUtf32(v4);
            int v5 = Convert.ToInt32(byteToS(portData, 8, 1),16); String a5 = Char.ConvertFromUtf32(v5);
            String A = a1 + a2 + a3 + a4 + a5;
            String B = a1 + a2 + a3 + a4;
            gavLableText(A);
            Double b = Convert.ToDouble(B);
            //int c = (int)(b * 10);
            //当按钮开启
            if (startData)
            {
                //将当前气体添加进数组
                charDataAdd(b);
            }

        }

        private void redDataHandle()
        {
            if (label6.InvokeRequired)
            {
                AddRedText d = new AddRedText(redDataHandle);
                this.Invoke(d);
            }
            else {
                label6.Text = "";
                label6.Text = "有人进入";
            }

        }
        private void gavLableText(String str)
        {
            if (label4.InvokeRequired)
            {
                AddLableText d = new AddLableText(gavLableText);
                this.Invoke(d, str);
            }
            else {
                label4.Text = "";
                label4.Text = str;
            }
        }

        //图形数据的添加
        private void charDataAdd(Double c)
        {
            list.Add(c);
            listCountTest(list);
        }

        private void drawChar()
        {
            chart1.Series[0].Points.Clear(); // 清除所有点
            for(int i =0;i<list.Count;i++)
            {
                chart1.Series[0].Points.AddXY(i, list[i]);  //添加点
            }
        
        }

        //当数据大于15时，移除数据的头部
        private void listCountTest(List<Double> list) 
        {
            if (list.Count >= 15)
            {
                list.RemoveAt(0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!isOpen)
            {
                if(port1.SelectedIndex < 0)
                {
                    MessageBox.Show("请先选择串口!");
                    port1.Focus();
                    return;
                }
                else if(baundR.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择波特率！");
                    baundR.Focus();
                    return;
                }
                else
                {
                    String portName = port1.Text.Trim();
                    Int32 baundData = Int32.Parse(baundR.Text.Trim());
                    try
                    {
                        Boolean portS = portOpen(portName, baundData);
                        if (portS)
                        {
                            isOpen = true;
                            threadPort = new Thread(new ThreadStart(readData));
                            threadPort.IsBackground = true;       //设置线程后台运行
                            threadPort.Start();
                            button1.Enabled = false;
                            button2.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("串口打开未成功.");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("打开串口按钮未生效");
                    }
                }
            }
            else
            {
                MessageBox.Show("串口已处于打开状态！请勿重复操作!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!isOpen)
            {
                MessageBox.Show("未打开串口，请先打开串口");
                return;
            }
            Byte[] data = new Byte[] { 0x4A, 0x30, 0x0B, 0x01, 0x70, 0x55 };
            sendData(data);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!isOpen) { MessageBox.Show("未打开串口，请先打开串口");return; }
            Byte[] data = new Byte[] { 0x4A, 0x30, 0x0B, 0x00, 0x71, 0x55 };
            sendData(data);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!isOpen) { MessageBox.Show("未打开串口，请先打开串口"); return; }
            Byte[] data = new Byte[] { 0x4A, 0x40, 0x0A, 0x01, 0x01, 0x55 };
            sendData(data);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!isOpen) { MessageBox.Show("未打开串口，请先打开串口"); return; }
            Byte[] data = new Byte[] { 0x4A, 0x40, 0x0A, 0x02, 0x02, 0x55 };
            sendData(data);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!isOpen) { MessageBox.Show("未打开串口，请先打开串口"); return; }
            Byte[] data = new Byte[] { 0x4A, 0x40, 0x0A, 0x00, 0x00, 0x55 };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isOpen = false;
            button2.Enabled = false;
            button1.Enabled = true;
        }

        private void chart1_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            HitTestResult myTestResult = chart1.HitTest(e.X, e.Y, ChartElementType.DataPoint);//获取命中测试的结果
            if (myTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = myTestResult.PointIndex;
                DataPoint dp = myTestResult.Series.Points[i];
                string YValue = dp.YValues[0].ToString();//获取数据点的Y值
                e.Text = "\r该点数值:" + YValue;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!isOpen) { MessageBox.Show("请先打开串口"); }
            else {
                if (dataStart.Text == "开始获取")
                {
                    startData = true;
                    timer1.Start();
                    dataStart.Text = "暂停获取";
                }
                else
                {
                    startData = false;
                    timer1.Stop();
                    dataStart.Text = "开始获取";
                }
            }
        }
        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            drawChar();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear(); // 清除所有点
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本软件需配合相应的下位机使用，功能按钮可以对相应的模块进行控制;\n" +
                "气体的浓度可以以图形的方式呈现，最大呈现15个数据，之后再增加会清除头节点；\n" +
                "结合红外模块可检测是否有人进入。");
        }
    }
}
