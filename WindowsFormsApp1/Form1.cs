using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Net.Http;
using Yaskawa.IEC61131.RMI.HttpUtil;

namespace WindowsFormsApp1
{
    public partial class FileSender : Form
    {
        private IUserFileUtility utility_;
        public FileSender()
        {
            InitializeComponent();
            ControllerFolder.Items.Clear();
            ControllerFolder.Items.AddRange(new string[] { UserDirectories.FLASH_DRIVE_PARAMETERS, UserDirectories.FLASH_USER_DATA, UserDirectories.FLASH_CAM_DATA, UserDirectories.RAMDISK_USER_DATA, UserDirectories.RAMDISK_CAM_DATA });
            ControllerFolder.SelectedIndex = 0;
            utility_ = new UserFileUtility();
        }

        private string FormatFolderFile(string folderName, string fileName, string description)
        {
            return string.Format("{2} : {0} : {1}", folderName, fileName, description);
        }

        private void SaveFile(string filePath, string contents)
        {
            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(contents);
            writer.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select File";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "All Files (*.*)|*.*|Gcode File (*.gcode)|*gcode";
            openFileDialog1.Multiselect = false;
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName !="")
            { textBox1.Text= openFileDialog1.FileName;}
            else
            { textBox1.Text= "Secim Yapilmadi!";}

        }

        private void button1_Click(object sender, EventArgs e)
        {
        
            string caption = FormatFolderFile(ControllerFolder.Text, textBox1.Text, button1.Text);
            try
            {
                string error = null;
                int errorCode = utility_.SendFileToUserDirectory(textBox2.Text, textBox1.Text , ControllerFolder.Text, out error);

                if (errorCode == 0)
                {
                    MessageBox.Show("Successful", caption);
                }
                else
                {
                    MessageBox.Show("Error code [" + errorCode + "]\n" + error, caption);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, caption);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
