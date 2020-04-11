using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace XMLtoPDF
{
    public partial class Viewer : Form
    {
        Data.CGXML cgxml1 = null;

        string[] files = null;
        public Viewer(string[] file, int rType)
        {
            InitializeComponent();

            //try
            //{
                files = file;
                if (rType == 1)
                    CreateFISA();
                else if (rType == 2)
                    Report1();
                else if (rType == 3)
                    Report2();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error");
            //}
        }

        public bool IsValidDouble(string str)
        {
            try
            {
                Convert.ToDouble(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CreateFISA()
        {
            DataTable dt = new DataTable("Others");
            Int32 Sno = 0, Sno1 = 0;
            dt.Columns.Add("LandPlotNo");
            dt.Columns.Add("ParcelNo");
            dt.Columns.Add("MeaseuredArea");
            dt.Columns.Add("PaperLBNo");
            dt.Columns.Add("Enclosed");
            dt.Columns.Add("CoArea");
            dt.Columns.Add("LNotes");
            dt.Columns.Add("BuildingNo");
            dt.Columns.Add("BDestination");
            dt.Columns.Add("LevelSNo");
            dt.Columns.Add("IsLegal");
            dt.Columns.Add("IUNo");
            dt.Columns.Add("bMeaseuredArea");
            dt.Columns.Add("BNotes");
            dt.Columns.Add("CADGENNO");
            dt.Columns.Add("SNo");
            dt.Columns.Add("Name");
            dt.Columns.Add("DeedDetails");
            dt.Columns.Add("DeedNo");
            dt.Columns.Add("DeedDate");
            dt.Columns.Add("Code");
            dt.Columns.Add("Observation");
            dt.Columns.Add("SNo1");
            dt.Columns.Add("Name1");
            dt.Columns.Add("DeedDetails1");
            dt.Columns.Add("DeedNo1");
            dt.Columns.Add("DeedDate1");
            dt.Columns.Add("Observation1");
            dt.Columns.Add("Code1");
            dt.Columns.Add("Sector");
            dt.Columns.Add("City");
            dt.Columns.Add("Notari");
            dt.Columns.Add("NotariDeedType");
            dt.Columns.Add("NotariDeedDetails");
            dt.Columns.Add("NotariAuthority");
            dt.Columns.Add("Constructiecondominiu");
            dt.Columns.Add("Nrbloc");
            dt.Columns.Add("nrtop");
            dt.Columns.Add("LMArea");

            DataTable dt_Build = new DataTable("FISA_Building");
            dt_Build.Columns.Add("BuildingNo");
            dt_Build.Columns.Add("BDestination");
            dt_Build.Columns.Add("LevelSNo");
            dt_Build.Columns.Add("IsLegal");
            dt_Build.Columns.Add("IUNo");
            dt_Build.Columns.Add("bMeaseuredArea");
            dt_Build.Columns.Add("BNotes");
            dt_Build.Columns.Add("CADGENNO");
            dt_Build.Columns.Add("Constructiecondominiu");
            dt_Build.Columns.Add("Nrbloc");
            dt_Build.Columns.Add("LCADGENNO");

            DataTable dt_Person1 = new DataTable("Person");
            dt_Person1.Columns.Add("SNo");
            dt_Person1.Columns.Add("Name");
            dt_Person1.Columns.Add("DeedDetails");
            dt_Person1.Columns.Add("DeedNo");
            dt_Person1.Columns.Add("DeedDate");
            dt_Person1.Columns.Add("Code");
            dt_Person1.Columns.Add("Observation");
            dt_Person1.Columns.Add("CADGENNO");

            DataTable dt_Person2 = new DataTable("Person1");
            dt_Person2.Columns.Add("SNo");
            dt_Person2.Columns.Add("Name");
            dt_Person2.Columns.Add("DeedDetails");
            dt_Person2.Columns.Add("DeedNo");
            dt_Person2.Columns.Add("DeedDate");
            dt_Person2.Columns.Add("Code");
            dt_Person2.Columns.Add("Observation");
            dt_Person2.Columns.Add("CADGENNO");

            DataTable dt_Notari = new DataTable("Notari");
            dt_Notari.Columns.Add("SNo");
            dt_Notari.Columns.Add("Notari");
            dt_Notari.Columns.Add("NotariDeedType");
            dt_Notari.Columns.Add("NotariDeedDetails");
            dt_Notari.Columns.Add("NotariAuthority");
            dt_Notari.Columns.Add("CADGENNO");

            foreach (var path in files)
            {
                Sno = 0;
                Sno1 = 0;

                var xDocument = XDocument.Parse(File.ReadAllText(path, System.Text.Encoding.UTF8));
                StringReader sr = new StringReader(xDocument.ToString());
                //DataSet XMLData = new DataSet();
                //XMLData.ReadXml(sr);

                cgxml1 = new Data.CGXML();
                //cgxml1.ReadXml(path, XmlReadMode.InferSchema);
                cgxml1.ReadXml(sr);
                string Siruta = "";

                string Bnotes = "";
                foreach (DataRow dr1 in cgxml1.Building.Rows)
                {
                    Sno++;
                    Bnotes += " C" + Sno.ToString() + ": " + dr1["Notes"].ToString() + " ";
                }
                Sno = 0;
                foreach (DataRow dr1 in cgxml1.Building.Rows)
                {
                    DataRow dr = dt_Build.NewRow();
                    dr["BuildingNo"] = dr1["BUILDNO"].ToString();
                    dr["BDestination"] = dr1["BUILDINGDESTINATION"].ToString();
                    dr["LevelSNo"] = dr1["LEVELSNO"].ToString();
                    dr["IsLegal"] = Convert.ToInt16(dr1["ISLEGAL"]) == 1 ? "DA" : "NU";
                    dr["IUNo"] = dr1["IUNO"].ToString() == "" ? "0" : dr1["IUNO"].ToString();
                    dr["bMeaseuredArea"] = dr1["MEASUREDAREA"].ToString();
                    dr["BNotes"] = Bnotes;
                    dr["Nrbloc"] = cgxml1.Building.Columns.Contains("Block") ? dr1["Block"].ToString() : "";
                    dr["Constructiecondominiu"] = IsValidDouble(dr1["IUNO"].ToString()) ? (Convert.ToInt32(dr1["IUNO"].ToString()) > 0 ? "DA" : "NU") : "NU";
                    dr["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                    dt_Build.Rows.Add(dr);
                    dt_Build.AcceptChanges();
                }

                foreach (DataRow dr in cgxml1.Registration.Rows)
                {
                    if (dr["REGISTRATIONTYPE"].ToString() == "NOTATION")
                    {
                        DataRow dr1 = dt_Notari.NewRow();
                        dr1["Notari"] = dr["NOTES"].ToString();
                        dr1["NotariDeedType"] = GlobalTables.Dictionary.Select("DICTIONARYITEMCODE = '" + cgxml1.Deed.Select("DeedId = '" + dr["DEEDID"].ToString() + "'")[0]["DeedType"].ToString() + "'")[0]["DICTIONARYITEMNAME"].ToString();
                        dr1["NotariDeedDetails"] = cgxml1.Deed.Select("Deedid = '" + dr["DEEDID"].ToString() + "'")[0]["DEEDNUMBER"].ToString() + " / " + Convert.ToDateTime(cgxml1.Deed.Select("Deedid = '" + dr["DEEDID"].ToString() + "'")[0]["DEEDDate"].ToString()).ToString("dd.MM.yyyy");
                        dr1["NotariAuthority"] = cgxml1.Deed.Select("Deedid = '" + dr["DEEDID"].ToString() + "'")[0]["Authority"].ToString();
                        dr1["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                        dt_Notari.Rows.Add(dr1);
                        dt_Notari.AcceptChanges();
                    }
                }
                foreach (DataRow dr1 in cgxml1.Parcel.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["City"] = GlobalTables.Admin.Select("ADMINISTRATIVEUNITID = '" + cgxml1.Address.Select("ADDRESSID = '" + cgxml1.Land[0]["AddressId"].ToString() + "'")[0]["SIRSUP"].ToString() + "'")[0]["Name"].ToString();
                    dr["nrtop"] = cgxml1.Land.Columns.Contains("TOPONO") ? cgxml1.Land.Rows[0]["TOPONO"].ToString() : "";
                    dr["LandPlotNo"] = dr1["LandPlotno"].ToString();
                    dr["ParcelNo"] = dr1["Parcelno"].ToString();
                    dr["MeaseuredArea"] = dr1["MeasuredArea"].ToString();
                    dr["PaperLBNo"] = dr1["PaperLBNo"].ToString();
                    dr["Enclosed"] = Convert.ToBoolean(cgxml1.Land.Rows[0]["ENCLOSED"]) ? "I" : "N";
                    dr["CoArea"] = Convert.ToBoolean(cgxml1.Land.Rows[0]["CoArea"]) ? "CO" : "NCO";
                    dr["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                    dr["LNotes"] = (dr1["Notes"].ToString() == "") ? "" : "1A : " + dr1["Notes"].ToString();
                    dr["Notari"] = "-";
                    dr["Sector"] = cgxml1.Land.Rows[0]["CADSector"].ToString();
                    dr["LMArea"] = cgxml1.Land.Rows[0]["MeasuredArea"].ToString();
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }


                foreach (DataRow dr1 in cgxml1.Person.Rows)
                {

                    string code = "";
                    if (dr1["IdCode"].ToString().Length > 0 && dr1["IdCode"].ToString().Substring(0, 6) != "999999" && Convert.ToInt32(dr1["ISPHYSICAL"]) == 1)
                    {
                        string Year = "19" + dr1["IdCode"].ToString().Substring(1, 2);
                        string Month = dr1["IdCode"].ToString().Substring(3, 2);
                        string Day = dr1["IdCode"].ToString().Substring(5, 2);

                        code = Day + "." + Month + "." + Year;
                    }
                    else
                    {
                        code = dr1["IdCode"].ToString();
                    }

                    string Deedid = "";
                    if (cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["LBPARTNO"].ToString() == "2")
                    {
                        Sno++;
                        Deedid = cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["DeedId"].ToString();
                        DataRow dr_p = dt_Person1.NewRow();
                        dr_p["Sno"] = Sno;
                        dr_p["Name"] = dr1["LastName"] + (dr1["FatherInitial"].ToString() == "" ? " " : " " + dr1["FatherInitial"].ToString() + " ") + dr1["FirstName"];
                        dr_p["DeedDetails"] = cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDNUMBER"].ToString() + "/" + Convert.ToDateTime(cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDDate"].ToString()).ToString("dd.MM.yyyy");
                        dr_p["DeedNo"] = cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDNUMBER"].ToString();
                        dr_p["DeedDate"] = Convert.ToDateTime(cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDDate"].ToString()).ToString("dd/MM/yyyy");
                        dr_p["Observation"] = dr1["Notes"].ToString();
                        dr_p["Code"] = dr1["IdCode"].ToString();
                        dr_p["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                        dt_Person1.Rows.Add(dr_p);
                        dt_Person1.AcceptChanges();
                    }
                    if (cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["LBPARTNO"].ToString() == "3")
                    {
                        Sno1++;
                        Deedid = cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["DeedId"].ToString();
                        DataRow dr_p = dt_Person2.NewRow();
                        dr_p["Sno"] = Sno1;
                        dr_p["Name"] = dr1["LastName"] + (dr1["FatherInitial"].ToString() == "" ? " " : " " + dr1["FatherInitial"].ToString() + " ") + dr1["FirstName"];
                        dr_p["DeedDetails"] = cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDNUMBER"].ToString() + "/" + Convert.ToDateTime(cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDDate"].ToString()).ToString("dd.MM.yyyy");
                        dr_p["DeedNo"] = cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDNUMBER"].ToString();
                        dr_p["DeedDate"] = Convert.ToDateTime(cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDDate"].ToString()).ToString("dd/MM/yyyy");
                        dr_p["Observation"] = dr1["Notes"].ToString();
                        dr_p["Code"] = dr1["IdCode"].ToString();
                        dr_p["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                        dt_Person2.Rows.Add(dr_p);
                        dt_Person2.AcceptChanges();
                    }
                }
            }

            Reports.FISA cryRpt = new XMLtoPDF.Reports.FISA();
            //cryRpt.Load(Application.StartupPath + @"\Reports\CrystalReport2.rpt");

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.Tables.Add(dt_Build);
            ds.Tables.Add(dt_Person1);
            ds.Tables.Add(dt_Person2);
            ds.Tables.Add(dt_Notari);
            cryRpt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cryRpt;
            crystalReportViewer1.Refresh();
        }

        private void Report1()
        {
            DataTable dt = new DataTable();
            Int32 Sno = 0, Sno1 = 0;
            dt.Columns.Add("Sno");
            dt.Columns.Add("Name");
            dt.Columns.Add("Data");
            dt.Columns.Add("Identifier");
            dt.Columns.Add("Plansa");
            dt.Columns.Add("Sector");
            dt.Columns.Add("Address");
            dt.Columns.Add("proprietate");
            dt.Columns.Add("Suprafaţa");
            dt.Columns.Add("Observaţii");
            dt.Columns.Add("City");
            dt.Columns.Add("County");

            foreach (var path in files)
            {
                cgxml1 = new Data.CGXML();
                cgxml1.ReadXml(path, XmlReadMode.InferSchema);

                string Identifier = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                string[] Identifier2 = new string[cgxml1.Building.Rows.Count];
                if (cgxml1.Building.Rows.Count > 0)
                    for (int i = 0; i < cgxml1.Building.Rows.Count; i++)
                    {
                        Identifier2[i] = cgxml1.Building.Rows[i]["CADGENNO"].ToString();
                    }
                string Plansa = "";
                string Sector = cgxml1.Land.Rows[0]["CADSector"].ToString();

                //String Siruta = cgxml1.Address.Select("INTRAVILAN = True")[0]["Siruta"].ToString();
                //DataRow[] dr1 = GlobalTables.Admin.Select(@"SIRUTA = '" + Siruta + "'");
                //string city = dr1[0]["Name"].ToString();
                //string Coutyid = dr1[0]["CountyId"].ToString();
                //DataRow[] dr2 = GlobalTables.County.Select("CountyId = '" + Coutyid + "'");
                //string CountryName = dr2[0]["Name"].ToString();
                //string Address = "UAT " + city + ", Jud. " + CountryName + " " + cgxml1.Parcel.Rows[0]["LandplotNo"].ToString() + "-" + cgxml1.Parcel.Rows[0]["ParcelNo"].ToString();
                string CountryName="";
                string city = "";
                string Siruta = "";
                string Address = "";
                string proprietate = cgxml1.Land.Rows[0]["MeasuredArea"].ToString();
                string[] NAMES = new string[cgxml1.Person.Rows.Count];
                int namescount = 0;
                foreach (DataRow dr in cgxml1.Person.Rows)
                {
                    DataRow[] dr1 = null;
                    #region old
                    //if (GlobalTables.Admin.Select("SIRUTA='" + Siruta + "'").Count() > 0)
                    //{
                    //    city = GlobalTables.Admin.Select(@"SIRUTA = '" + Siruta + "'")[0]["Name"].ToString();
                    //    CountryName = (GlobalTables.County.Select("CountyId = '" + GlobalTables.Admin.Select(@"SIRUTA = '" + Siruta + "'")[0]["CountyId"].ToString() + "'"))[0]["Name"].ToString();
                    //}
                    //else if (GlobalTables.Locality.Select("SIRUTA='" + Siruta + "'").Count() > 0)
                    //{
                    //    city = GlobalTables.Locality.Select("SIRUTA='" + Siruta + "'")[0]["Name"].ToString();
                    //    CountryName = (GlobalTables.Locality.Select("LOCALITYID = '" + GlobalTables.Locality.Select(@"SIRUTA = '" + Siruta + "'")[0]["LOCALITYID"].ToString() + "'"))[0]["Name"].ToString();
                    //}
                    //else
                    //{
                    //    city = "";
                    //    CountryName = "";
                    //}
                    #endregion
                    CountryName = "";
                    city = "";
                    Siruta = Address = "";
                    Siruta = cgxml1.Address.Select("ADDRESSID = '" + dr["ADDRESSID"].ToString() + "'")[0]["Siruta"].ToString();
                    city = GlobalTables.Admin.Select("ADMINISTRATIVEUNITID = '" + cgxml1.Address.Select("ADDRESSID = '" + cgxml1.Land[0]["AddressId"].ToString() + "'")[0]["SIRSUP"].ToString() + "'")[0]["Name"].ToString();
                    CountryName = GlobalTables.County.Select("COUNTYID = '" + GlobalTables.Admin.Select("ADMINISTRATIVEUNITID = '" + cgxml1.Address.Select("ADDRESSID = '" + cgxml1.Land[0]["AddressId"].ToString() + "'")[0]["SIRSUP"].ToString() + "'")[0]["COUNTYID"].ToString() + "'")[0]["Name"].ToString();
                    Address = "UAT " + city + ", Jud. " + CountryName + " " + cgxml1.Parcel.Rows[0]["LandplotNo"].ToString() + "-" + cgxml1.Parcel.Rows[0]["ParcelNo"].ToString();

                    string code = "";
                    if (dr["IdCode"].ToString().Length > 0 && dr["IdCode"].ToString().Substring(0, 6) != "999999" && Convert.ToInt32(dr["ISPHYSICAL"]) == 1)
                    {
                        string Year = "19" + dr["IdCode"].ToString().Substring(1, 2);
                        string Month = dr["IdCode"].ToString().Substring(3, 2);
                        string Day = dr["IdCode"].ToString().Substring(5, 2);

                        code = Day + "." + Month + "." + Year;
                    }
                    else if (dr["IdCode"].ToString().Substring(0, 6) == "999999")
                    {
                        code = "";
                    }
                    else
                    {
                        code = dr["IdCode"].ToString();
                    }


                    //if (dr["REGISTRATIONID"].ToString() == "1")
                    if(cgxml1.Registration.Select("REGISTRATIONID = '" + dr["REGISTRATIONID"].ToString() + "'")[0]["LBPARTNO"].ToString() == "2")
                    {
                        DataRow dr5 = dt.NewRow();
                        if (NAMES.Contains(dr["LastName"].ToString() + " " + dr["FirstName"].ToString()))
                        {
                            if (cgxml1.Building.Rows.Count > 0)
                            {
                                for (int i = 0; i < cgxml1.Building.Rows.Count; i++)
                                {
                                    Sno++;
                                    dr5 = dt.NewRow();
                                    dr5["Sno"] = Sno;
                                    dr5["Name"] = dr["LastName"] + " " + dr["FirstName"];
                                    //dr5["Data"] = (Convert.ToInt32(dr["ADDRESSID"]) > 1 && Convert.ToInt32(dr["ADDRESSID"]) < 8) ? dr["IDCODE"] : "";
                                    dr5["Data"] = code;
                                    dr5["Plansa"] = "";
                                    dr5["Identifier"] = cgxml1.Building.Rows[i]["CADGENNO"].ToString();
                                    dr5["Sector"] = Sector;
                                    dr5["Address"] = Address;
                                    dr5["proprietate"] = cgxml1.Building.Rows[i]["LEGALAREA"].ToString();
                                    dr5["Suprafaţa"] = "";
                                    dr5["Observaţii"] = dr["NOTES"];
                                    dr5["City"] = "UAT " + city;
                                    dr5["County"] = CountryName;
                                    dt.Rows.Add(dr5);
                                }
                            }
                        }
                        else
                        {
                            NAMES[namescount] = dr["LastName"].ToString() + " " + dr["FirstName"].ToString();
                            namescount++;

                            Sno++;
                            dr5["Sno"] = Sno;
                            dr5["Name"] = dr["LastName"] + " " + dr["FirstName"];
                            //dr5["Data"] = (Convert.ToInt32(dr["ADDRESSID"]) > 1 && Convert.ToInt32(dr["ADDRESSID"]) < 8) ? dr["IDCODE"] : "";
                            dr5["Data"] = code;
                            dr5["Plansa"] = "";
                            dr5["Identifier"] = Identifier;
                            dr5["Sector"] = Sector;
                            dr5["Address"] = Address;
                            dr5["proprietate"] = proprietate;
                            dr5["Suprafaţa"] = "";
                            dr5["Observaţii"] = dr["NOTES"];
                            dr5["City"] = "UAT " + city;
                            dr5["County"] = CountryName;
                            dt.Rows.Add(dr5);
                        }
                        dt.AcceptChanges();
                    }
                }
            }
            Reports.Report1 cryRpt = new XMLtoPDF.Reports.Report1();
            //cryRpt.Load(Application.StartupPath + @"\Reports\CrystalReport2.rpt");
            cryRpt.SetDataSource(dt);
            crystalReportViewer1.ReportSource = cryRpt;
            crystalReportViewer1.Refresh();
        }

        private void Report2()
        {
            DataTable dt = new DataTable("Table2");
            Int32 Sno = 0, Sno1 = 0;
            dt.Columns.Add("PaperCadNo");
            dt.Columns.Add("Inravile");
            dt.Columns.Add("LandPlotNo");
            dt.Columns.Add("ParcelNo");
            dt.Columns.Add("MeaseuredArea");
            dt.Columns.Add("PaperLBNo");
            dt.Columns.Add("Enclosed");
            dt.Columns.Add("CoArea");
            dt.Columns.Add("BuildingNo");
            dt.Columns.Add("BDestination");
            dt.Columns.Add("LevelSNo");
            dt.Columns.Add("IsLegal");
            dt.Columns.Add("IUNo");
            dt.Columns.Add("bMeaseuredArea");
            dt.Columns.Add("BNotes");
            dt.Columns.Add("CADGENNO");
            dt.Columns.Add("SNo");
            dt.Columns.Add("Name");
            dt.Columns.Add("FirstName");
            dt.Columns.Add("FatherInitial");
            dt.Columns.Add("LastName");
            dt.Columns.Add("Address");
            dt.Columns.Add("DeedDetails");
            dt.Columns.Add("DeedNo");
            dt.Columns.Add("DeedDate");
            dt.Columns.Add("Code");
            dt.Columns.Add("Observation");
            dt.Columns.Add("SNo1");
            dt.Columns.Add("Name1");
            dt.Columns.Add("FirstName1");
            dt.Columns.Add("FatherInitial1");
            dt.Columns.Add("LastName1");
            dt.Columns.Add("Address1");
            dt.Columns.Add("DeedDetails1");
            dt.Columns.Add("DeedNo1");
            dt.Columns.Add("DeedDate1");
            dt.Columns.Add("Code1");
            dt.Columns.Add("Observation1");
            dt.Columns.Add("UseCategory");
            dt.Columns.Add("ActualQuota");
            dt.Columns.Add("Title");
            dt.Columns.Add("DeedType");
            dt.Columns.Add("Authority");
            dt.Columns.Add("Teren");
            dt.Columns.Add("ActualQuota1");
            dt.Columns.Add("Title1");
            dt.Columns.Add("DeedType1");
            dt.Columns.Add("Authority1");
            dt.Columns.Add("Teren1");
            dt.Columns.Add("RightType");
            dt.Columns.Add("Sector");
            dt.Columns.Add("Notari");
            dt.Columns.Add("NotariDeedType");
            dt.Columns.Add("NotariDeedDetails");
            dt.Columns.Add("NotariAuthority");
            dt.Columns.Add("ParcelNotes");
            dt.Columns.Add("UAT");
            dt.Columns.Add("CO");
            dt.Columns.Add("NrTop");
            dt.Columns.Add("NrCF");
            dt.Columns.Add("Cotaparte");
            dt.Columns.Add("Valoare");
            dt.Columns.Add("Tipmoneda");
            dt.Columns.Add("Imprejmuit");
            dt.Columns.Add("contestat");

            DataTable dt_Notari = new DataTable("Report2_Notari");
            dt_Notari.Columns.Add("Notari");
            dt_Notari.Columns.Add("NotariDeedType");
            dt_Notari.Columns.Add("NotariDeedDetails");
            dt_Notari.Columns.Add("NotariAuthority");
            dt_Notari.Columns.Add("Imprejmuit");
            dt_Notari.Columns.Add("contestat");
            dt_Notari.Columns.Add("ParcelNotes");
            dt_Notari.Columns.Add("CADGENNO");

            DataTable dt_Parcel = new DataTable("Report2_Parcel");
            dt_Parcel.Columns.Add("PaperCadNo");
            dt_Parcel.Columns.Add("Inravile");
            dt_Parcel.Columns.Add("LandPlotNo");
            dt_Parcel.Columns.Add("ParcelNo");
            dt_Parcel.Columns.Add("MeaseuredArea");
            dt_Parcel.Columns.Add("PaperLBNo");
            dt_Parcel.Columns.Add("BuildingNo");
            dt_Parcel.Columns.Add("BDestination");
            dt_Parcel.Columns.Add("LevelSNo");
            dt_Parcel.Columns.Add("IsLegal");
            dt_Parcel.Columns.Add("bMeaseuredArea");
            dt_Parcel.Columns.Add("BNotes");
            dt_Parcel.Columns.Add("CADGENNO");
            dt_Parcel.Columns.Add("NrTop");
            dt_Parcel.Columns.Add("NrCF");
            dt_Parcel.Columns.Add("UseCategory");
            dt_Parcel.Columns.Add("Address");

            DataTable dt_Building = new DataTable("Report2_Building");
            dt_Building.Columns.Add("BuildingNo");
            dt_Building.Columns.Add("BDestination");
            dt_Building.Columns.Add("LevelSNo");
            dt_Building.Columns.Add("IsLegal");
            dt_Building.Columns.Add("bMeaseuredArea");
            dt_Building.Columns.Add("BNotes");
            dt_Building.Columns.Add("CADGENNO");
            dt_Building.Columns.Add("NrCF");

            foreach (var path in files)
            {
                Sno = 0;
                Sno1 = 0;
                cgxml1 = new Data.CGXML();
                cgxml1.ReadXml(path, XmlReadMode.InferSchema);


                String Siruta = "";
                string Address = "Loc. ";

                #region Notari
                if (cgxml1.Registration.Select("REGISTRATIONTYPE = 'NOTATION'").Count() > 0)
                {
                    foreach (DataRow dr1 in cgxml1.Registration.Rows)
                    {
                        if (dr1["REGISTRATIONTYPE"].ToString() == "NOTATION")
                        {
                            DataRow dr = dt_Notari.NewRow();
                            dr["Notari"] = dr1["NOTES"].ToString();
                            dr["NotariDeedType"] = GlobalTables.Dictionary.Select("DICTIONARYITEMCODE = '" + cgxml1.Deed.Select("DeedId = '" + dr1["DeedId"].ToString() + "'")[0]["DeedType"].ToString() + "'")[0]["DICTIONARYITEMNAME"].ToString();
                            dr["NotariDeedDetails"] = cgxml1.Deed.Select("Deedid = '" + dr1["DeedId"].ToString() + "'")[0]["DEEDNUMBER"].ToString() + " / " + Convert.ToDateTime(cgxml1.Deed.Select("Deedid = '" + dr1["DeedId"].ToString() + "'")[0]["DEEDDate"].ToString()).ToString("dd.MM.yyyy");
                            dr["NotariAuthority"] = cgxml1.Deed.Select("Deedid = '" + dr1["DeedId"].ToString() + "'")[0]["Authority"].ToString();
                            dr["Imprejmuit"] = Convert.ToInt32(cgxml1.Land.Rows[0]["ENCLOSED"]) == 0 ? "Neîmprejmuit" : "Împrejmuit";
                            dr["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                            dr["ParcelNotes"] = cgxml1.Parcel.Rows[0]["Notes"].ToString();
                            dt_Notari.Rows.Add(dr);
                            dt_Notari.AcceptChanges();
                        }
                    }
                }
                else
                {
                    DataRow dr = dt_Notari.NewRow();
                    dr["Notari"] = "-";
                    dr["NotariDeedType"] = "-";
                    dr["NotariDeedDetails"] = "-";
                    dr["NotariAuthority"] = "-";
                    dr["Imprejmuit"] = Convert.ToInt32(cgxml1.Land.Rows[0]["ENCLOSED"]) == 0 ? "Neîmprejmuit" : "Împrejmuit";
                    dr["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                    dr["ParcelNotes"] = cgxml1.Parcel.Rows[0]["Notes"].ToString();
                    dt_Notari.Rows.Add(dr);
                    dt_Notari.AcceptChanges();
                }
                #endregion

                #region Parcel
                int count_Parcel = cgxml1.Parcel.Rows.Count;
                int count_Building = cgxml1.Building.Rows.Count;
                int i = 0;
                if (count_Parcel >= count_Building)
                {
                    foreach (DataRow dr1 in cgxml1.Parcel.Rows)
                    {
                        DataRow dr = dt_Parcel.NewRow();
                        string CountryName = "";
                        string city = "";
                        Siruta = Address = "";
                        //if (cgxml1.Parcel.Rows[0]["LandplotNo"].ToString() + "-" + cgxml1.Parcel.Rows[0]["ParcelNo"].ToString() == "-")
                        //{
                            Siruta = cgxml1.Address.Select("ADDRESSID = '" + cgxml1.Land.Rows[0]["ADDRESSID"].ToString() + "'")[0]["Siruta"].ToString();
                            city = GlobalTables.Admin.Select("ADMINISTRATIVEUNITID = '" + cgxml1.Address.Select("ADDRESSID = '" + cgxml1.Land[0]["AddressId"].ToString() + "'")[0]["SIRSUP"].ToString() + "'")[0]["Name"].ToString();
                            CountryName = GlobalTables.County.Select("COUNTYID = '" + GlobalTables.Admin.Select("ADMINISTRATIVEUNITID = '" + cgxml1.Address.Select("ADDRESSID = '" + cgxml1.Land[0]["AddressId"].ToString() + "'")[0]["SIRSUP"].ToString() + "'")[0]["COUNTYID"].ToString() + "'")[0]["Name"].ToString();
                            Address = "Loc " + city + " UAT " + city + ", Jud. " + CountryName + " " + cgxml1.Parcel.Rows[0]["LandplotNo"].ToString() + "-" + cgxml1.Parcel.Rows[0]["ParcelNo"].ToString();
                        //}
                        //else
                        //{
                        //    Address = cgxml1.Parcel.Rows[0]["LandplotNo"].ToString() + "-" + cgxml1.Parcel.Rows[0]["ParcelNo"].ToString();
                        //}
                        dr["PaperCadNo"] = cgxml1.Land.Rows[0]["PaperCadNo"].ToString();
                        dr["LandPlotNo"] = dr1["LandPlotno"].ToString();
                        dr["ParcelNo"] = dr1["Parcelno"].ToString();
                        dr["MeaseuredArea"] = cgxml1.Land.Rows[0]["MeasuredArea"].ToString();
                        dr["PaperLBNo"] = cgxml1.Land.Rows[0]["E2IDENTIFIER"].ToString();
                        dr["Inravile"] = Convert.ToBoolean(dr1["INTRAVILAN"]) ? "I" : "E";
                        dr["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                        dr["UseCATEGORY"] = dr1["UseCATEGORY"].ToString();
                        dr["NrTop"] = cgxml1.Land.Columns.Contains("TOPONO") ? cgxml1.Land.Rows[0]["TOPONO"].ToString() : "";
                        dr["NrCF"] = cgxml1.Land.Rows[0]["PaperLBNo"].ToString();
                        dr["Address"] = Address;
                        if (count_Building > 0)
                        {
                            if (i <= count_Building - 1)
                            {
                                dr["BuildingNo"] = cgxml1.Building.Rows[i]["BUILDNO"].ToString();
                                dr["BDestination"] = cgxml1.Building.Rows[i]["BUILDINGDESTINATION"].ToString();
                                dr["LevelSNo"] = cgxml1.Building.Rows[i]["LEVELSNO"].ToString();
                                dr["IsLegal"] = Convert.ToInt32(cgxml1.Building.Rows[i]["ISLEGAL"]) == 1 ? "DA" : "NU";
                                dr["bMeaseuredArea"] = cgxml1.Building.Rows[i]["MEASUREDAREA"].ToString();
                                dr["BNotes"] = cgxml1.Building.Rows[i]["Notes"].ToString();
                            }
                        }
                        dt_Parcel.Rows.Add(dr);
                        dt_Parcel.AcceptChanges();
                        i++;
                    }
                }
                else
                {
                    foreach (DataRow dr1 in cgxml1.Building.Rows)
                    {
                        DataRow dr = dt_Parcel.NewRow();
                        string CountryName = "";
                        string city = "";
                        Siruta = Address = "";
                        //if (cgxml1.Parcel.Rows[0]["LandplotNo"].ToString() + "-" + cgxml1.Parcel.Rows[0]["ParcelNo"].ToString() == "-")
                        //{
                            Siruta = cgxml1.Address.Select("ADDRESSID = '" + cgxml1.Land.Rows[0]["ADDRESSID"].ToString() + "'")[0]["Siruta"].ToString();
                            city = GlobalTables.Admin.Select("ADMINISTRATIVEUNITID = '" + cgxml1.Address.Select("ADDRESSID = '" + cgxml1.Land[0]["AddressId"].ToString() + "'")[0]["SIRSUP"].ToString() + "'")[0]["Name"].ToString();
                            CountryName = GlobalTables.County.Select("COUNTYID = '" + GlobalTables.Admin.Select("ADMINISTRATIVEUNITID = '" + cgxml1.Address.Select("ADDRESSID = '" + cgxml1.Land[0]["AddressId"].ToString() + "'")[0]["SIRSUP"].ToString() + "'")[0]["COUNTYID"].ToString() + "'")[0]["Name"].ToString();
                            Address = "Loc " + city + "UAT " + city + ", Jud. " + CountryName + " " + cgxml1.Parcel.Rows[0]["LandplotNo"].ToString() + "-" + cgxml1.Parcel.Rows[0]["ParcelNo"].ToString();
                        //}
                        //else
                        //{
                        //    Address = cgxml1.Parcel.Rows[0]["LandplotNo"].ToString() + "-" + cgxml1.Parcel.Rows[0]["ParcelNo"].ToString();
                        //}
                        dr["BuildingNo"] = dr1["BUILDNO"].ToString();
                        dr["BDestination"] = dr1["BUILDINGDESTINATION"].ToString();
                        dr["LevelSNo"] = dr1["LEVELSNO"].ToString();
                        dr["IsLegal"] = Convert.ToInt32(dr1["ISLEGAL"]) == 1 ? "DA" : "NU";
                        dr["bMeaseuredArea"] = dr1["MeasuredArea"].ToString();
                        dr["BNotes"] = dr1["Notes"].ToString();
                        dr["NrCF"] = cgxml1.Land.Rows[0]["PaperLBNo"].ToString();
                        dr["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                        dr["Address"] = Address;
                        dr["NrTop"] = cgxml1.Land.Columns.Contains("TOPONO") ? cgxml1.Land.Rows[0]["TOPONO"].ToString() : "";
                        dr["PaperCadNo"] = cgxml1.Land.Rows[0]["PaperCadNo"].ToString();
                        dr["MeaseuredArea"] = cgxml1.Land.Rows[0]["MeasuredArea"].ToString();
                        dr["PaperLBNo"] = cgxml1.Land.Rows[0]["E2IDENTIFIER"].ToString();
                        if (count_Parcel > 0)
                        {
                            if (i <= count_Parcel - 1)
                            {
                                dr["LandPlotNo"] = cgxml1.Parcel.Rows[i]["LandPlotno"].ToString();
                                dr["ParcelNo"] = cgxml1.Parcel.Rows[i]["Parcelno"].ToString();
                                dr["Inravile"] = Convert.ToBoolean(cgxml1.Parcel.Rows[i]["INTRAVILAN"]) ? "I" : "E";
                                dr["UseCATEGORY"] = cgxml1.Parcel.Rows[i]["UseCATEGORY"].ToString();
                            }
                        }
                        dt_Parcel.Rows.Add(dr);
                        dt_Parcel.AcceptChanges();
                        i++;
                    }
                }
                #endregion

                string BuildingCadgeNo = "";

                #region Building
                foreach (DataRow dr1 in cgxml1.Building.Rows)
                {
                    DataRow dr = dt_Building.NewRow();
                    dr["NrCF"] = cgxml1.Land.Rows[0]["E2IDENTIFIER"].ToString();
                    dr["BuildingNo"] = dr1["BUILDNO"].ToString();
                    dr["BDestination"] = dr1["BUILDINGDESTINATION"].ToString();
                    dr["LevelSNo"] = dr1["LEVELSNO"].ToString();
                    dr["IsLegal"] = Convert.ToInt32(dr1["ISLEGAL"]) == 1 ? "DA" : "NU";
                    dr["bMeaseuredArea"] = dr1["MEASUREDAREA"].ToString();
                    dr["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                    dt_Building.Rows.Add(dr);
                    dt_Building.AcceptChanges();

                    BuildingCadgeNo += dr1["CADGENNO"].ToString() + ", ";
                }
                #endregion

                string[] NAMES = new string[cgxml1.Person.Rows.Count];
                int namescount = 0;
                foreach (DataRow dr1 in cgxml1.Person.Rows)
                {
                    
                    Siruta = "";
                    Address = "Loc. ";
                    DataRow dr = dt.NewRow();
                    dr["PaperCadNo"] = cgxml1.Land.Rows[0]["PaperCadNo"].ToString();
                    dr["Sector"] = cgxml1.Land.Rows[0]["CADSECTOR"].ToString();
                    dr["LandPlotNo"] = cgxml1.Parcel.Rows[0]["LandPlotno"].ToString();
                    dr["ParcelNo"] = cgxml1.Parcel.Rows[0]["Parcelno"].ToString();
                    dr["MeaseuredArea"] = cgxml1.Parcel.Rows[0]["MeasuredArea"].ToString();
                    dr["PaperLBNo"] = cgxml1.Parcel.Rows[0]["PaperLBNo"].ToString();
                    dr["Enclosed"] = Convert.ToBoolean(cgxml1.Land.Rows[0]["ENCLOSED"]) ? "I" : "N";
                    dr["CoArea"] = Convert.ToBoolean(cgxml1.Land.Rows[0]["CoArea"]) ? "CO" : "NCO";
                    dr["Inravile"] = Convert.ToBoolean(cgxml1.Parcel.Rows[0]["INTRAVILAN"]) ? "I" : "E";
                    dr["CADGENNO"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                    dr["UseCATEGORY"] = cgxml1.Parcel.Rows[0]["UseCATEGORY"].ToString();
                    dr["ParcelNotes"] = cgxml1.Parcel.Rows[0]["Notes"].ToString();

                    dr["NrTop"] = cgxml1.Land.Columns.Contains("TOPONO") ? cgxml1.Land.Rows[0]["TOPONO"].ToString() : "";
                    dr["NrCF"] =  cgxml1.Land.Rows[0]["E2IDENTIFIER"].ToString();

                    if (cgxml1.BuildingCommonParts.Rows.Count > 0)
                    {
                        dr["Cotaparte"] = cgxml1.BuildingCommonParts.Columns.Contains("COMMONPARTTYPE") ? cgxml1.BuildingCommonParts.Rows[0]["COMMONPARTTYPE"].ToString() : "";
                    }
                    else
                    {
                        dr["Cotaparte"] = "";
                    }
                    dr["Valoare"] = cgxml1.Registration.Columns.Contains("VALUEAMOUNT") ? cgxml1.Registration.Rows[0]["VALUEAMOUNT"].ToString() : "";
                    dr["Tipmoneda"] = cgxml1.Registration.Columns.Contains("VALUECURRENCY") ? cgxml1.Registration.Rows[0]["VALUECURRENCY"].ToString() : "";
                    dr["Imprejmuit"] = Convert.ToInt32(cgxml1.Land.Rows[0]["ENCLOSED"]) == 0 ? "Neîmprejmuit" : "Împrejmuit";

                    if (cgxml1.ContestedxEntity.Rows.Count > 0)
                    {
                        dr["contestat"] = cgxml1.ContestedxEntity.Columns.Contains("CONTESTEDXENTITYID") ? cgxml1.ContestedxEntity.Rows[0]["CONTESTEDXENTITYID"].ToString() : "";
                    }
                    else
                    {
                        dr["contestat"] = "";
                    }

                    #region Notari not in use
                    if (cgxml1.Registration.Select("REGISTRATIONTYPE = 'NOTATION'").Count() > 0)
                    {
                        //string Id = cgxml1.Registration.Select("REGISTRATIONTYPE = 'NOTATION'")[0]["RegistrationId"].ToString();
                        string Id = cgxml1.Registration.Select("REGISTRATIONTYPE = 'NOTATION'")[0]["DeedId"].ToString();

                        dr["Notari"] = cgxml1.Registration.Select("REGISTRATIONTYPE = 'NOTATION'")[0]["NOTES"].ToString();
                        dr["NotariDeedType"] = GlobalTables.Dictionary.Select("DICTIONARYITEMCODE = '" + cgxml1.Deed.Select("DeedId = '" + Id + "'")[0]["DeedType"].ToString() + "'")[0]["DICTIONARYITEMNAME"].ToString();
                        dr["NotariDeedDetails"] = cgxml1.Deed.Select("Deedid = '" + Id + "'")[0]["DEEDNUMBER"].ToString() + " / " + Convert.ToDateTime(cgxml1.Deed.Select("Deedid = '" + Id + "'")[0]["DEEDDate"].ToString()).ToString("dd.MM.yyyy");
                        dr["NotariAuthority"] = cgxml1.Deed.Select("Deedid = '" + Id + "'")[0]["Authority"].ToString();
                    }
                    else
                    {
                        dr["Notari"] = "-";
                        dr["NotariDeedType"] = "-";
                        dr["NotariDeedDetails"] = "-";
                        dr["NotariAuthority"] = "-";
                    }
                    #endregion

                    Siruta = cgxml1.Address.Select("ADDRESSID = '" + dr1["ADDRESSID"].ToString() + "'")[0]["Siruta"].ToString();
                    string ADDId = GlobalTables.Locality.Select("SIRUTA='" + Siruta + "'")[0]["ADMINISTRATIVEUNITID"].ToString();
                    Address = Address + GlobalTables.Locality.Select("SIRUTA='" + Siruta + "'")[0]["Name"].ToString() + " Jud. " + GlobalTables.County.Select("COUNTYID = '" + GlobalTables.Admin.Select("ADMINISTRATIVEUNITID='" + ADDId + "'")[0]["COUNTYID"].ToString() + "'")[0]["Name"].ToString();

                    dr["UAT"] = GlobalTables.Admin.Select("ADMINISTRATIVEUNITID = '" + cgxml1.Address.Select("ADDRESSID = '" + cgxml1.Land[0]["AddressId"].ToString() + "'")[0]["SIRSUP"].ToString() + "'")[0]["Name"].ToString();
                    dr["CO"] = (Convert.ToInt16(cgxml1.Land[0]["COAREA"]) == 1) ? "CO" : "NCO";
                    string code = "";
                    if (dr1["IdCode"].ToString().Length > 0 && dr1["IdCode"].ToString().Substring(0,6) != "999999" && Convert.ToInt32(dr1["ISPHYSICAL"]) == 1)
                    {
                        string Year = "19" + dr1["IdCode"].ToString().Substring(1, 2);
                        string Month = dr1["IdCode"].ToString().Substring(3, 2);
                        string Day = dr1["IdCode"].ToString().Substring(5, 2);

                        code = Day + "." + Month + "." + Year;
                    }
                    else
                    {
                        code = dr1["IdCode"].ToString();
                    }
                    //if (dr1["IdCode"].ToString() != "" && dr1["IdCode"].ToString().Length > 6 && dr1["IdCode"].ToString().Substring(0, 6) != "999999")
                    //{
                    //    string Year = "19" + dr1["IdCode"].ToString().Substring(1, 2);
                    //    string Month = dr1["IdCode"].ToString().Substring(3, 2);
                    //    string Day = dr1["IdCode"].ToString().Substring(5, 2);

                    //    code = Day + "." + Month + "." + Year;
                    //}
                    //else if (dr1["IdCode"].ToString().Substring(0, 6) != "999999")
                    //   code = dr1["IdCode"].ToString();

                    if (cgxml1.Building.Rows.Count > 0)
                    {
                        dr["BuildingNo"] = cgxml1.Building.Rows[0]["BUILDNO"].ToString();
                        dr["BDestination"] = cgxml1.Building.Rows[0]["BUILDINGDESTINATION"].ToString();
                        dr["LevelSNo"] = cgxml1.Building.Rows[0]["LEVELSNO"].ToString();
                        dr["IsLegal"] = Convert.ToInt32(cgxml1.Building.Rows[0]["ISLEGAL"]) == 1 ? "DA" : "NU";
                        dr["IUNo"] = cgxml1.Building.Rows[0]["IUNO"].ToString();
                        dr["bMeaseuredArea"] = cgxml1.Building.Rows[0]["MEASUREDAREA"].ToString();
                        dr["BNotes"] = cgxml1.Building.Rows[0]["Notes"].ToString();
                        
                    }

                    string Deedid = "";
                    if (cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["LBPARTNO"].ToString() == "2")
                    {

                        Sno++;
                        dr["Sno"] = Sno;
                        Deedid = cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["DeedId"].ToString();
                        dr["Name"] = dr1["LastName"] + (dr1["FatherInitial"].ToString() == "" ? " " : " " + dr1["FatherInitial"].ToString() + " ") + dr1["FirstName"];
                        dr["FirstName"] = dr1["FirstName"];
                        dr["FatherInitial"] = dr1["FatherInitial"];
                        dr["LastName"] = dr1["LastName"];
                        dr["DeedDetails"] = cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDNUMBER"].ToString() + " / " + Convert.ToDateTime(cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDDate"].ToString()).ToString("dd.MM.yyyy");
                        dr["DeedNo"] = cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDNUMBER"].ToString();
                        dr["DeedType"] = GlobalTables.Dictionary.Select("DICTIONARYITEMCODE = '" + cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DeedType"].ToString() + "'")[0]["DICTIONARYITEMNAME"].ToString();
                        dr["DeedDate"] = Convert.ToDateTime(cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDDate"].ToString()).ToString("dd/MM/yyyy");
                        dr["Authority"] = cgxml1.Deed.Select("Deedid = " + Deedid)[0]["Authority"].ToString();
                        dr["Observation"] = dr1["Notes"].ToString();
                        dr["ActualQuota"] = cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["ActualQuota"].ToString();
                        dr["Title"] = cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["Title"].ToString();
                        
                        dr["Code"] = code;
                        dr["Address"] = Address;

                        if (NAMES.Contains(dr1["LastName"].ToString() + (dr1["FatherInitial"].ToString() == "" ? " " : " " + dr1["FatherInitial"].ToString() + " ") + dr1["FirstName"].ToString()))
                        {
                            dr["Teren"] = BuildingCadgeNo;
                        }
                        else
                        {
                            NAMES[namescount] = dr1["LastName"] + (dr1["FatherInitial"].ToString() == "" ? " " : " " + dr1["FatherInitial"].ToString() + " ") + dr1["FirstName"];
                            dr["Teren"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                            namescount++;
                        }

                        //DataRow[] dr123 = cgxml1.Person.Select("REGISTRATIONID = 2");
                        //if (dr123.Count() == 0)
                        //{
                        //    dr["FirstName1"] = '-';
                        //    dr["LastName1"] = '-';
                        //}
                    }
                    if (cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["LBPARTNO"].ToString() == "3")
                    {
                        Sno1++;
                        dr["Sno1"] = Sno1;
                        Deedid = cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["DeedId"].ToString();
                        dr["Name1"] = dr1["LastName"] + (dr1["FatherInitial"].ToString() == "" ? " " : " " + dr1["FatherInitial"].ToString() + " ") + dr1["FirstName"];
                        dr["FirstName1"] = dr1["FirstName"];
                        dr["FatherInitial1"] = dr1["FatherInitial"];
                        dr["LastName1"] = dr1["LastName"];
                        if (cgxml1.Deed.Select("Deedid = " + Deedid).Count() > 0)
                        {
                            dr["DeedDetails1"] = cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDNUMBER"].ToString() + "/" + Convert.ToDateTime(cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDDate"].ToString()).ToString("dd.MM.yyyy");
                            dr["DeedNo1"] = cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDNUMBER"].ToString();
                            dr["DeedDate1"] = Convert.ToDateTime(cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DEEDDate"].ToString()).ToString("dd/MM/yyyy");
                            dr["DeedType1"] = GlobalTables.Dictionary.Select("DICTIONARYITEMCODE = '" + cgxml1.Deed.Select("Deedid = " + Deedid)[0]["DeedType"].ToString() + "'")[0]["DICTIONARYITEMNAME"].ToString();
                            dr["Authority1"] = cgxml1.Deed.Select("Deedid = " + Deedid)[0]["Authority"].ToString();

                        }
                        //dr["DeedDetails1"] = cgxml1.Deed.Select("Deedid = 2")[0]["DEEDNUMBER"].ToString() + " / " + Convert.ToDateTime(cgxml1.Deed.Select("Deedid = 2")[0]["DEEDDate"].ToString()).ToString("dd.MM.yyyy");
                        //dr["DeedNo1"] = cgxml1.Deed.Select("Deedid = 2")[0]["DEEDNUMBER"].ToString();
                        //dr["DeedDate1"] = Convert.ToDateTime(cgxml1.Deed.Select("Deedid = 2")[0]["DEEDDate"].ToString()).ToString("dd/MM/yyyy");
                        //dr["DeedType1"] = GlobalTables.Dictionary.Select("DICTIONARYITEMCODE = '" + cgxml1.Deed.Select("Deedid = 2")[0]["DeedType"].ToString() + "'")[0]["DICTIONARYITEMNAME"].ToString();
                        dr["Code1"] = code;
                        dr["ActualQuota1"] = cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["ActualQuota"].ToString();
                        dr["Title1"] = cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["Title"].ToString();
                        dr["RightType"] = cgxml1.Registration.Select("REGISTRATIONID = '" + dr1["REGISTRATIONID"].ToString() + "'")[0]["RightType"].ToString();
                        dr["Teren1"] = cgxml1.Land.Rows[0]["CADGENNO"].ToString();
                        dr["Address1"] = Address;
                    }
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }
            Reports.Report2 cryRpt = new XMLtoPDF.Reports.Report2();
            //cryRpt.Load(Application.StartupPath + @"\Reports\CrystalReport2.rpt");
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.Tables.Add(dt_Notari);
            ds.Tables.Add(dt_Parcel);
            ds.Tables.Add(dt_Building);
            cryRpt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cryRpt;
            crystalReportViewer1.Refresh();
        }

        private void Viewer_Load(object sender, EventArgs e)
        {

        }
    }
}
