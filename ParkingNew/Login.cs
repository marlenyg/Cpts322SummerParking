using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingNew
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = new User();
            if (user.UserName == textBox2.Text && user.Password == textBox1.Text)
            {
                this.Hide();
                Parking engine = new Parking();
                engine.ShowDialog();
                this.Close();

            }
            else
            {
               Console.WriteLine($"Wrong Username or Password");
                textBox2.Text = textBox1.Text = "";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
