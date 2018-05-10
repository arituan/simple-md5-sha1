using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace FirstHashApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            int typeData = cbxDataFormat.SelectedIndex;
            byte[] data = null;

            tbMD5.Text = string.Empty;
            tbSHA.Text = string.Empty;

            if (tbData.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Data empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (checkMD5.Checked == false && checkSHA.Checked == false)
            {
                MessageBox.Show("Hash function does not choose", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (typeData == 0)
            {
                if (!File.Exists(tbData.Text.Trim()))
                {
                    MessageBox.Show("File does not exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    data = File.ReadAllBytes(tbData.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else if (typeData == 1)
            {
                try
                {
                    data = Encoding.UTF8.GetBytes(tbData.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else if (typeData == 2)
            {
                
                try
                {
                    data = ConvertHexStringToByteArray(tbData.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (checkMD5.Checked == true)
            {
                try
                {
                    tbMD5.Text = GetMd5Hash(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            if (checkSHA.Checked == true)
            {
                try
                {
                    tbSHA.Text = GetSHA1Hash(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }


        }

        private byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] HexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }

        private string GetMd5Hash(byte[] input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(input);

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        private string GetSHA1Hash(byte[] input)
        {
            using (SHA1 SHA1Hash = SHA1.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = SHA1Hash.ComputeHash(input);

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbxDataFormat.SelectedIndex = 0;
        }

        private void cbxDataFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbData.Text = string.Empty;
            if (cbxDataFormat.SelectedIndex != 0)
            {
                btnOpenFile.Hide();
                tbData.Size = new Size(246, 20);
            }
            else
            {
                btnOpenFile.Show();
                tbData.Size = new Size(216, 20);
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            fileDialog.ShowDialog();
            if (fileDialog.FileName.Trim() != string.Empty)
            {
                tbData.Text = fileDialog.FileName.Trim();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cbxDataFormat.SelectedIndex = 0;
            tbMD5.Text = string.Empty;
            tbSHA.Text = string.Empty;
            checkMD5.Checked = false;
            checkSHA.Checked = false;
            tbData.Text = string.Empty;
        }

        private void tbData_TextChanged(object sender, EventArgs e)
        {
            tbMD5.Text = string.Empty;
            tbSHA.Text = string.Empty;
        }
    }
}
