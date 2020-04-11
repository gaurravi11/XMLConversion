using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using ClosedXML.Excel;
using ExcelDataReader;

namespace XMLtoExcel
{
    public partial class Dashboard : Form
    {
        string[] files;
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void XMLtoDatagrid(string FolderName)
        {
            try
            {
                /*This Dataset is the main dataset to be extracted as excel*/
                DataSet MainXMLData = new DataSet();
                if (files == null || files.Length < 1)
                {
                    MessageBox.Show("Please Select Folder of XML Files");
                    return;
                }
                int rows = 0;
                string FileName = "";
                foreach (var path in files)
                {
                    FileName = Path.GetFileNameWithoutExtension(path);
                    var xDocument = XDocument.Parse(File.ReadAllText(path, System.Text.Encoding.UTF8));
                    StringReader sr = new StringReader(xDocument.ToString());
                    DataSet XMLData = new DataSet();
                    //XMLData.ReadXml(path, XmlReadMode.InferSchema);
                    XMLData.ReadXml(sr);
                    if (rows == 0)
                    {
                        /*Cloning the Dataset with what we get it from the XMLs*/
                        MainXMLData = XMLData.Clone();

                        foreach (DataTable dt in MainXMLData.Tables)
                        {
                            DataColumn Col = dt.Columns.Add("FileKaNo");
                            Col.SetOrdinal(0);
                        }

                        //dataGridView1.Columns.Add("FileName", "FileName");
                        //foreach (DataColumn dc in XMLData.Tables[0].Columns)
                        //{
                        //    dataGridView1.Columns.Add(dc.ColumnName, dc.ColumnName);
                        //}
                    }
                    else
                    {
                        var result = XMLData.Tables.Cast<DataTable>()
                                                .Where(x => !MainXMLData.Tables.Cast<DataTable>().Any(y => y.TableName == x.TableName)).ToList();

                        foreach (var list in result)
                        {
                            MainXMLData.Tables.Add(list.TableName);
                        }

                        foreach (DataTable dt in MainXMLData.Tables)
                        {
                            DataColumnCollection columns = dt.Columns;
                            if (!columns.Contains("FileKaNo"))
                            {
                                DataColumn Col = dt.Columns.Add("FileKaNo");
                                Col.SetOrdinal(0);
                            }
                        }
                    }

                    foreach (DataTable dt in MainXMLData.Tables)
                    {
                        if (XMLData.Tables.Contains(dt.TableName))
                        {
                            DataTable dataTable = XMLData.Tables[dt.TableName];
                            if (rows != 0)
                            {
                                var result = dataTable.Columns.Cast<DataColumn>()
                                                    .Where(x => !dt.Columns.Cast<DataColumn>().Any(y => y.ColumnName == x.ColumnName)).ToList();

                                foreach (var list in result)
                                {
                                    dt.Columns.Add(list.ColumnName);
                                }
                            }

                            foreach (DataRow dr in dataTable.Rows)
                            {
                                DataRow dataRow = dt.NewRow();
                                dataRow["FileKaNo"] = FileName;
                                foreach (DataColumn dc in dataTable.Columns)
                                {
                                    dataRow[dc.ColumnName] = dr[dc.ColumnName] == null ? "" : dr[dc.ColumnName];
                                }
                                dt.Rows.Add(dataRow);
                            }
                        }
                        rows++;
                    }
                    #region Datagridview
                    //if (XMLData.Tables[0].TableName == "Address")
                    //{
                    //    if (rows != 0)
                    //    {
                    //        var result = XMLData.Tables[0].Columns.Cast<DataColumn>()
                    //                            .Where(x => !MainXMLData.Tables[0].Columns.Cast<DataColumn>().Any(y => y.ColumnName == x.ColumnName)).ToList();

                    //        foreach (var list in result)
                    //        {
                    //            MainXMLData.Tables[0].Columns.Add(list.ColumnName);
                    //            //dataGridView1.Columns.Add(list.ColumnName, list.ColumnName);
                    //        }
                    //    }
                    //    columns = MainXMLData.Tables[0].Columns.Count;
                    //    foreach (DataRow dr in XMLData.Tables[0].Rows)
                    //    {
                    //        DataRow dataRow = MainXMLData.Tables[0].NewRow();
                    //        dataRow["FileName"] = FileName;

                    //        //dataGridView1.Rows.Add();
                    //        //dataGridView1.Rows[rows].Cells["FileName"].Value = FileName;
                    //        foreach (DataColumn dc in XMLData.Tables[0].Columns)
                    //        {
                    //            //dataGridView1.Rows[rows].Cells[dc.ColumnName].Value = dr[dc.ColumnName];
                    //            dataRow[dc.ColumnName] = dr[dc.ColumnName];
                    //        }
                    //        MainXMLData.Tables[0].Rows.Add(dataRow);
                    //        rows++;
                    //    }
                    //}
                    #endregion
                }
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.Description = "Select Folder For Output Excel File";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(MainXMLData);
                        wb.SaveAs(fd.SelectedPath + @"\" + FolderName + ".xlsx");

                        MessageBox.Show("Excel Created Successfully", "XML DATA");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ExcelToDataSet(string FilePath)
        {
            FileStream stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader;

            if (Path.GetExtension(FilePath) == "xls")
            {
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            //DataSet excelData = excelReader.AsDataSet();

            //4.DataSet - The result of each spreadsheet will be created in the result.Tables with first column as header
            var excelData = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            if (excelData != null)
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.Description = "Select Folder For Output XML Files";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    var listofFiles = excelData.Tables[0].AsEnumerable().Select(x => x.Field<string>("FileKaNo")).Distinct().ToList();
                    DataSet dataSet;
                    foreach (var item in listofFiles)
                    {
                        DataTable dataTable = new DataTable();
                        dataSet = new DataSet("CGXML");
                        int i = 0;
                        foreach (DataTable tables in excelData.Tables)
                        {
                            //DataTable dt = new DataTable(tables.TableName);
                            var dt = tables.AsEnumerable().Where(x => x.Field<string>("FileKaNo") == item).Select(x => x);
                            if (dt.Count() > 0)
                            {
                                dataSet.Tables.Add(dt.CopyToDataTable());
                                dataSet.Tables[i].TableName = tables.TableName;
                                i++;
                            }
                            //dataSet.Tables[tables.TableName].Rows.Add(tables.AsEnumerable().Where(x => x.Field<string>("Ravi") == item).Select(x => x).CopyToDataTable().Select());
                            //DataRow[] dataRows = tables.Select(x => x.Field<string>("Ravi") == item).Select(x => x).CopyToDataTable();
                            //dataSet.Tables[tables.TableName].Rows.Add(dataRows);
                        }

                        foreach (DataTable dt in dataSet.Tables)
                        {
                            dt.Columns.Remove("FileKaNo");
                            
                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    foreach (DataColumn c in dt.Columns)
                            //    {
                            //        if (row.IsNull(c))
                            //        {
                            //            //row[c] = string.Empty;
                            //            row[c] = "";
                            //        }
                            //    }
                            //}
                            //dt.AcceptChanges();
                        }
                        //dataSet.WriteXml(fd.SelectedPath + @"\" + item + ".cgxml");

                        //System.IO.StreamWriter xmlSW = new System.IO.StreamWriter(fd.SelectedPath + @"\" + item + ".cgxml");
                        //dataSet.WriteXml(xmlSW);
                        //////xmlSW.Write(DatasetToString(dataSet));
                        //xmlSW.Close();
                        DatasetToXML(dataSet, fd.SelectedPath, item);
                    }
                    MessageBox.Show("XMLs Created Successfully", "XML DATA");
                }
            }
        }


        void DatasetToXML(DataSet dataSet, string path, string name)
        {
            XmlDocument myxml = new XmlDocument();
            myxml.LoadXml("<?xml version='1.0' ?>" +
                "<CGXML>" +
                "</CGXML>");
            //XmlElement myxmlCGXML = myxml.CreateElement("CGXML");
            int i = 1;
            foreach (DataTable dt in dataSet.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    XmlElement myxmlrecord = myxml.CreateElement(dt.TableName);
                    XmlElement myxmlfield;
                    foreach (DataColumn col in dt.Columns)
                    {
                        myxmlfield = myxml.CreateElement(col.ColumnName);
                        if (dr[col] == null)
                            myxmlfield.IsEmpty = true;
                        else
                            myxmlfield.InnerText = dr[col] == null ? "" : dr[col].ToString();
                        myxmlrecord.AppendChild(myxmlfield);
                    }
                    myxml.ChildNodes[i].AppendChild(myxmlrecord);
                }
                //i++;
            }
            myxml.Save(path + @"\" + name + ".cgxml");
        }

        StringWriter DatasetToString(DataSet dataSet)
        {
            StringWriter sw = new StringWriter();
            try
            {
                //ds.WriteXml(sw, XmlWriteMode.IgnoreSchema);
                foreach (DataTable dt in dataSet.Tables)
                {
                    sw.Write(@"<CGXML>");
                    foreach (DataRow row in dt.Rows)
                    {
                        sw.Write(@"<" + dt.TableName + ">");
                        foreach (DataColumn col in dt.Columns)
                        {
                            sw.Write(@"<" + XmlConvert.EncodeName(col.ColumnName) + @">");
                            sw.Write(row[col]);
                            sw.Write(@"</" + XmlConvert.EncodeName(col.ColumnName) + @">");
                        }
                        sw.Write(@"</" + dt.TableName + ">");
                    }
                    sw.Write(@"</CGXML" + dt.TableName + ">");
                }
                return sw;
            }
            catch (Exception ex)
            {
                return sw;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (rdb1.Checked)
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.Description = "Select Folder For XML Files to Convert into Excel";
                DialogResult result = fd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    files = Directory.GetFiles(fd.SelectedPath)
                                              .Where(p => p.ToLower().EndsWith(".cgxml"))
                                              .ToArray();
                }

                XMLtoDatagrid(Path.GetFileName(fd.SelectedPath));
            }
            else
            {
                 
            }
        }
    }
}
