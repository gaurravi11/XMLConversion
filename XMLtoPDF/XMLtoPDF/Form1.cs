using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace XMLtoPDF
{
    public partial class Form1 : Form
    {
        string[] files;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            DialogResult result = fd.ShowDialog();

            if (result == DialogResult.OK)
            {
                files = Directory.GetFiles(fd.SelectedPath)
                                          .Where(p => p.ToLower().EndsWith(".cgxml"))
                                          .ToArray();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (files == null || files.Length < 1)
            {
                MessageBox.Show("Please Select Folder of XML Files");
                return;
            }
            Viewer frm = new Viewer(files, 1);
            frm.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string xmlFile_Admin = @"Data\Admin.xml";
            string xmlFile_County = @"Data\County.xml";
            string xmlFile_Dictionary = @"Data\Dictionary.xml";
            string xmlFile_Locality = @"Data\Locality.xml";
            string xmlFile_RightType = @"Data\RightType.xml";
            string xmlFile_ValidariCP = @"Data\ValidariCP.xml";
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(xmlFile_Admin, XmlReadMode.InferSchema);
            GlobalTables.Admin = dataSet.Tables[0];

            //DataRow[] dt = GlobalTables.Admin.Select("SIRUTA='179141'");

            dataSet.ReadXml(xmlFile_County, XmlReadMode.InferSchema);
            GlobalTables.County = dataSet.Tables[1];


            dataSet.ReadXml(xmlFile_Dictionary, XmlReadMode.InferSchema);
            GlobalTables.Dictionary = dataSet.Tables[2];

            dataSet.ReadXml(xmlFile_Locality, XmlReadMode.InferSchema);
            GlobalTables.Locality = dataSet.Tables[3];

            dataSet.ReadXml(xmlFile_RightType, XmlReadMode.InferSchema);
            GlobalTables.RightType = dataSet.Tables[4];

            dataSet.ReadXml(xmlFile_ValidariCP, XmlReadMode.InferSchema);
            GlobalTables.Validari_Fluxuri = dataSet.Tables[5];
            GlobalTables.Validari = dataSet.Tables[6];
            GlobalTables.Differente_Suprafete = dataSet.Tables[7];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (files == null || files.Length < 1)
            {
                MessageBox.Show("Please Select Folder of XML Files");
                return;
            }

            Viewer frm = new Viewer(files, 2);
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (files == null || files.Length < 1)
            {
                MessageBox.Show("Please Select Folder of XML Files");
                return;
            }

            Viewer frm = new Viewer(files, 3);
            frm.Show();
        }
    }
}
