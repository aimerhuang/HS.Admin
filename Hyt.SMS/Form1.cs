using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hyt.SMS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mobiles = tbxMobiles.Text.Split('\n');
            var message = tbxMessage.Text.Trim();
            foreach (var m in mobiles)
            {
                Console.WriteLine(m.Trim());
                var smsSender=Extra.SMS.SmsProviderFactory.CreateProvider();
                var result=  smsSender.Send(m.Trim(), message+"【品胜商城】",null);
                Console.WriteLine(result.Status);
            }
            MessageBox.Show(string.Format("{0} mobiles sent.",mobiles.Length));

        }

 

       
    }
}
