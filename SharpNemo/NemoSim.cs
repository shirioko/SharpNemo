using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SharpNemo
{
    public partial class NemoSim : Form
    {
        public int A, AV, Amplitude, Deviation, Delay = 0;
        const int DIRECTION_LEFT = -5;
        const int DIRECTION_RIGHT = 5;

        public NemoSim()
        {
            InitializeComponent();
        }

        private void Run()
        {
            while (true)
            {
                int _a = this.A;

                //process ends
                if (this.A <= ((this.Amplitude * -1) + this.Deviation))
                    this.AV = DIRECTION_RIGHT;
                else if (this.A >= (this.Amplitude + this.Deviation))
                    this.AV = DIRECTION_LEFT;

                if(!(this.Amplitude == 0 && this.A == this.Deviation))
                {
                    this.A += this.AV;
                }

                if (_a != this.A)
                {
                    this.SendData();
                }

                Thread.Sleep(this.Delay);
            }
        }

        delegate void DataDelegate();

        protected void SendData()
        {
            if (this.label1.InvokeRequired)
            {
                DataDelegate dd = new DataDelegate(SendData);
                this.Invoke(dd);
            }
            else
            {
                this.label1.Text = this.A.ToString();
                this.Redraw();
            }
        }

        protected System.Drawing.Graphics graphObj;

        protected void Redraw()
        {
            this.graphObj.Clear(System.Drawing.Color.White);
            Pen myPen = new Pen(System.Drawing.Color.Red, 5);
            this.graphObj.DrawLine(myPen, 60, 120, 60 + this.A, 20);
        }

        private void NemoSim_Load(object sender, EventArgs e)
        {
            this.Delay = 100;
            this.Amplitude = 10;
            this.Deviation = 0;

            this.AV = DIRECTION_LEFT;

            this.textBox1.Text = this.Delay.ToString();
            this.textBox2.Text = this.Amplitude.ToString();
            this.textBox3.Text = this.Deviation.ToString();

            Thread t = new Thread(new ThreadStart(Run));
            t.IsBackground = true;
            t.Start();
            this.graphObj = this.CreateGraphics();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Delay = Int32.Parse(this.textBox1.Text);
            this.Amplitude = Int32.Parse(this.textBox2.Text);
            this.Deviation = Int32.Parse(this.textBox3.Text);
        }
    }
}
