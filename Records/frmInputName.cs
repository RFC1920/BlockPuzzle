using System;
using System.Windows.Forms;

namespace WinTetris.Records
{
    public partial class frmInputName : Form
    {
        public frmInputName()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > Constants.MaxLengthForName)
            {
                string message = String.Format(Constants.MessageAbouteIncorrectNameLength, Constants.MaxLengthForName);
                MessageBox.Show(message);
                DialogResult = DialogResult.None;
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void frmInputName_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DialogResult == DialogResult.None)
            {
                e.Cancel = true;
            }
        }

        private void frmInputName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnOk.PerformClick();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
