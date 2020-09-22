using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        
        
        public Form1()
        {
            InitializeComponent();

            DateTime dt1 = DateTime.Today;
            dateTimePicker1.Value = dt1.AddMonths(-2);

            dateTimePicker2.Value = DateTime.Today;
        }
        
        private void chart1_Click(object sender, EventArgs e)
        {
            
        }

        private void CallOnAll()
        {
            string[] xyValues;
            DateTime x;
            double y = 0;
            DateTime fromDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            int tradesCount = 0;
            List<double> trades = new List<double>();
            List<DateTime> dateList = new List<DateTime>();
            SortedList<DateTime, List<double>> sortedDictionary = new SortedList<DateTime, List<double>>(new DuplicateKeyComparer<DateTime>());

            string path = textBox2.Text;

            //***atidaro visus csv exportuotus failus pasirinktame kataloge ir istraukia data, profit losss ir treidu skaiciu ***
            //***Naudojama DuplicateKeyComparer klase dublikatams sujungti "data stucte" SortedList tinka ir Dictionary

            foreach (string txtName in Directory.GetFiles(path, "*.csv"))
            {
                using (StreamReader file = new StreamReader(txtName))
                {
                    string line = file.ReadLine();
                    dateList.Clear();
                    trades.Clear();

                    while (line != null)

                    {

                        xyValues = line.Split(',');
                        //MessageBox.Show(xyValues[1]);
                        string dateModify = xyValues[0];
                        string date = dateModify.Remove(8, 4);
                        x = DateTime.ParseExact(date, string.Format("yyyyMMdd"), null);
                        y = double.Parse(xyValues[1]);
                        double trade = double.Parse(xyValues[6]);
                        trades.Add(trade);
                        dateList.Add(x);
                        sortedDictionary.Add(x, new List<double> { y, trade });
                        line = file.ReadLine();
                        y = 0;
                        
                    }

                    //***Skaiciuoja treidus nuo pasirinktos datos, nes jie sudetiniai***
                    
                    fromDate = dateTimePicker1.Value;
                    endDate = dateTimePicker2.Value;


                    foreach (DateTime item in dateList)
                    {
                        if (item >fromDate && item < endDate)
                        {

                            int index = dateList.IndexOf(item);
                            //MessageBox.Show(index.ToString());
                            if (index == 0)
                            {
                                tradesCount++;
                            } else
                            if (trades[index] != trades[index-1])
                            {
                                tradesCount++;
                            }

                        }
  
                    }

                }
            }


            //***Calculates chart from specified date***
          
            //fromDate = dateTimePicker1.Value;
            //endDate = dateTimePicker2.Value;
            
            foreach (var item in sortedDictionary)
            {
                if (item.Key > fromDate && item.Key < endDate)
                {
                    //MessageBox.Show(item.Value[0].ToString());

                    y = y + (double)(item.Value[0]);
                    chart1.Series["Series1"].Points.AddXY(item.Key, y);
 
                }
                else if (sortedDictionary.Keys.Last() < fromDate)
                {
                    MessageBox.Show("Data is older than selected From Date. Please select earlier date.");
                    break;
                }
  
            }

            //***Emty folder message***
            if (tradesCount == 0)
            {
                MessageBox.Show("Folder is empty of exported csv files or there is no trades in a specified period.");
            }


            //***Show trades on Chart***
            textBox1.Text = tradesCount.ToString() + " Trades";

            int netProfit = (int)y;
            textBoxES.Text = netProfit.ToString();

            //***Chart properties***

            chart1.Series["Series1"].XValueType = ChartValueType.DateTime;
                chart1.Series["Series1"].ChartType = SeriesChartType.Line;
                chart1.Series["Series1"].Color = Color.Blue;
                




        }

        private void CallOnES()
        {
            string[] xyValues;
            DateTime x;
            double y = 0;
            DateTime fromDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            int tradesCount = 0;
            List<double> trades = new List<double>();
            List<DateTime> dateList = new List<DateTime>();
            SortedList<DateTime, List<double>> sortedDictionary = new SortedList<DateTime, List<double>>(new DuplicateKeyComparer<DateTime>());

            string path = textBox2.Text;

            //***atidaro visus csv exportuotus failus pasirinktame kataloge ir istraukia data, profit losss ir treidu skaiciu ***
            //***Naudojama DuplicateKeyComparer klase dublikatams sujungti "data stucte" SortedList tinka ir Dictionary

            foreach (string txtName in Directory.GetFiles(path, "*ES*.csv"))
            {
                using (StreamReader file = new StreamReader(txtName))
                {
                    string line = file.ReadLine();
                    dateList.Clear();
                    trades.Clear();

                    while (line != null)

                    {

                        xyValues = line.Split(',');
                        //MessageBox.Show(xyValues[1]);
                        string dateModify = xyValues[0];
                        string date = dateModify.Remove(8, 4);
                        x = DateTime.ParseExact(date, string.Format("yyyyMMdd"), null);
                        y = double.Parse(xyValues[1]);
                        double trade = double.Parse(xyValues[6]);
                        trades.Add(trade);
                        dateList.Add(x);
                        sortedDictionary.Add(x, new List<double> { y, trade });
                        line = file.ReadLine();
                        y = 0;

                    }

                    //***Skaiciuoja treidus nuo pasirinktos datos, nes jie sudetiniai***

                    fromDate = dateTimePicker1.Value;
                    endDate = dateTimePicker2.Value;


                    foreach (DateTime item in dateList)
                    {
                        if (item > fromDate && item < endDate)
                        {

                            int index = dateList.IndexOf(item);
                            //MessageBox.Show(index.ToString());
                            if (index == 0)
                            {
                                tradesCount++;
                            }
                            else
                            if (trades[index] != trades[index - 1])
                            {
                                tradesCount++;
                            }

                        }

                    }

                }
            }


            //***Calculates chart from specified date***

            //fromDate = dateTimePicker1.Value;
            //endDate = dateTimePicker2.Value;

            foreach (var item in sortedDictionary)
            {
                if (item.Key > fromDate && item.Key < endDate)
                {
                    //MessageBox.Show(item.Value[0].ToString());

                    y = y + (double)(item.Value[0]);
                   

                }
                else if (sortedDictionary.Keys.Last() < fromDate)
                {
                    MessageBox.Show("Data is older than selected From Date. Please select earlier date.");
                    break;
                }

            }

            //***Emty folder message***
            if (tradesCount == 0)
            {
                MessageBox.Show("Folder for ES is empty of exported csv files or there is no trades in a specified period.");
            }


           //ShowNetProfit
          
            int netProfit = (int)y;
            textBox3.Text = netProfit.ToString();


            //***Chart properties***

            chart1.Series["Series1"].XValueType = ChartValueType.DateTime;
            chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            chart1.Series["Series1"].Color = Color.Blue;





        }

        private void CallOnEMD()
        {
            string[] xyValues;
            DateTime x;
            double y = 0;
            DateTime fromDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            int tradesCount = 0;
            List<double> trades = new List<double>();
            List<DateTime> dateList = new List<DateTime>();
            SortedList<DateTime, List<double>> sortedDictionary = new SortedList<DateTime, List<double>>(new DuplicateKeyComparer<DateTime>());

            string path = textBox2.Text;

            //***atidaro visus csv exportuotus failus pasirinktame kataloge ir istraukia data, profit losss ir treidu skaiciu ***
            //***Naudojama DuplicateKeyComparer klase dublikatams sujungti "data stucte" SortedList tinka ir Dictionary

            foreach (string txtName in Directory.GetFiles(path, "*EMD*.csv"))
            {
                using (StreamReader file = new StreamReader(txtName))
                {
                    string line = file.ReadLine();
                    dateList.Clear();
                    trades.Clear();

                    while (line != null)

                    {

                        xyValues = line.Split(',');
                        //MessageBox.Show(xyValues[1]);
                        string dateModify = xyValues[0];
                        string date = dateModify.Remove(8, 4);
                        x = DateTime.ParseExact(date, string.Format("yyyyMMdd"), null);
                        y = double.Parse(xyValues[1]);
                        double trade = double.Parse(xyValues[6]);
                        trades.Add(trade);
                        dateList.Add(x);
                        sortedDictionary.Add(x, new List<double> { y, trade });
                        line = file.ReadLine();
                        y = 0;

                    }

                    //***Skaiciuoja treidus nuo pasirinktos datos, nes jie sudetiniai***

                    fromDate = dateTimePicker1.Value;
                    endDate = dateTimePicker2.Value;


                    foreach (DateTime item in dateList)
                    {
                        if (item > fromDate && item < endDate)
                        {

                            int index = dateList.IndexOf(item);
                            //MessageBox.Show(index.ToString());
                            if (index == 0)
                            {
                                tradesCount++;
                            }
                            else
                            if (trades[index] != trades[index - 1])
                            {
                                tradesCount++;
                            }

                        }

                    }

                }
            }


            //***Calculates chart from specified date***

            //fromDate = dateTimePicker1.Value;
            //endDate = dateTimePicker2.Value;

            foreach (var item in sortedDictionary)
            {
                if (item.Key > fromDate && item.Key < endDate)
                {
                    //MessageBox.Show(item.Value[0].ToString());

                    y = y + (double)(item.Value[0]);


                }
                else if (sortedDictionary.Keys.Last() < fromDate)
                {
                    MessageBox.Show("Data is older than selected From Date. Please select earlier date.");
                    break;
                }

            }

            //***Emty folder message***
            if (tradesCount == 0)
            {
                MessageBox.Show("Folder for EMD is empty of exported csv files or there is no trades in a specified period.");
            }


            //ShowNetProfit

            int netProfit = (int)y;
            textBox5.Text = netProfit.ToString();


            //***Chart properties***

            chart1.Series["Series1"].XValueType = ChartValueType.DateTime;
            chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            chart1.Series["Series1"].Color = Color.Blue;





        }

        private void CallOnNQ()
        {
            string[] xyValues;
            DateTime x;
            double y = 0;
            DateTime fromDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            int tradesCount = 0;
            List<double> trades = new List<double>();
            List<DateTime> dateList = new List<DateTime>();
            SortedList<DateTime, List<double>> sortedDictionary = new SortedList<DateTime, List<double>>(new DuplicateKeyComparer<DateTime>());

            string path = textBox2.Text;

            //***atidaro visus csv exportuotus failus pasirinktame kataloge ir istraukia data, profit losss ir treidu skaiciu ***
            //***Naudojama DuplicateKeyComparer klase dublikatams sujungti "data stucte" SortedList tinka ir Dictionary

            foreach (string txtName in Directory.GetFiles(path, "*NQ*.csv"))
            {
                using (StreamReader file = new StreamReader(txtName))
                {
                    string line = file.ReadLine();
                    dateList.Clear();
                    trades.Clear();

                    while (line != null)

                    {

                        xyValues = line.Split(',');
                        //MessageBox.Show(xyValues[1]);
                        string dateModify = xyValues[0];
                        string date = dateModify.Remove(8, 4);
                        x = DateTime.ParseExact(date, string.Format("yyyyMMdd"), null);
                        y = double.Parse(xyValues[1]);
                        double trade = double.Parse(xyValues[6]);
                        trades.Add(trade);
                        dateList.Add(x);
                        sortedDictionary.Add(x, new List<double> { y, trade });
                        line = file.ReadLine();
                        y = 0;

                    }

                    //***Skaiciuoja treidus nuo pasirinktos datos, nes jie sudetiniai***

                    fromDate = dateTimePicker1.Value;
                    endDate = dateTimePicker2.Value;


                    foreach (DateTime item in dateList)
                    {
                        if (item > fromDate && item < endDate)
                        {

                            int index = dateList.IndexOf(item);
                            //MessageBox.Show(index.ToString());
                            if (index == 0)
                            {
                                tradesCount++;
                            }
                            else
                            if (trades[index] != trades[index - 1])
                            {
                                tradesCount++;
                            }

                        }

                    }

                }
            }


            //***Calculates chart from specified date***

            //fromDate = dateTimePicker1.Value;
            //endDate = dateTimePicker2.Value;

            foreach (var item in sortedDictionary)
            {
                if (item.Key > fromDate && item.Key < endDate)
                {
                    //MessageBox.Show(item.Value[0].ToString());

                    y = y + (double)(item.Value[0]);


                }
                else if (sortedDictionary.Keys.Last() < fromDate)
                {
                    MessageBox.Show("Data is older than selected From Date. Please select earlier date.");
                    break;
                }

            }

            //***Emty folder message***
            if (tradesCount == 0)
            {
                MessageBox.Show("Folder for NQ is empty of exported csv files or there is no trades in a specified period.");
            }


            //ShowNetProfit

            int netProfit = (int)y;
            textBox4.Text = netProfit.ToString();


            //***Chart properties***

            chart1.Series["Series1"].XValueType = ChartValueType.DateTime;
            chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            chart1.Series["Series1"].Color = Color.Blue;





        }

        private void CallOnEC()
        {
            string[] xyValues;
            DateTime x;
            double y = 0;
            DateTime fromDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            int tradesCount = 0;
            List<double> trades = new List<double>();
            List<DateTime> dateList = new List<DateTime>();
            SortedList<DateTime, List<double>> sortedDictionary = new SortedList<DateTime, List<double>>(new DuplicateKeyComparer<DateTime>());

            string path = textBox2.Text;

            //***atidaro visus csv exportuotus failus pasirinktame kataloge ir istraukia data, profit losss ir treidu skaiciu ***
            //***Naudojama DuplicateKeyComparer klase dublikatams sujungti "data stucte" SortedList tinka ir Dictionary

            foreach (string txtName in Directory.GetFiles(path, "*EC*.csv"))
            {
                using (StreamReader file = new StreamReader(txtName))
                {
                    string line = file.ReadLine();
                    dateList.Clear();
                    trades.Clear();

                    while (line != null)

                    {

                        xyValues = line.Split(',');
                        //MessageBox.Show(xyValues[1]);
                        string dateModify = xyValues[0];
                        string date = dateModify.Remove(8, 4);
                        x = DateTime.ParseExact(date, string.Format("yyyyMMdd"), null);
                        y = double.Parse(xyValues[1]);
                        double trade = double.Parse(xyValues[6]);
                        trades.Add(trade);
                        dateList.Add(x);
                        sortedDictionary.Add(x, new List<double> { y, trade });
                        line = file.ReadLine();
                        y = 0;

                    }

                    //***Skaiciuoja treidus nuo pasirinktos datos, nes jie sudetiniai***

                    fromDate = dateTimePicker1.Value;
                    endDate = dateTimePicker2.Value;


                    foreach (DateTime item in dateList)
                    {
                        if (item > fromDate && item < endDate)
                        {

                            int index = dateList.IndexOf(item);
                            //MessageBox.Show(index.ToString());
                            if (index == 0)
                            {
                                tradesCount++;
                            }
                            else
                            if (trades[index] != trades[index - 1])
                            {
                                tradesCount++;
                            }

                        }

                    }

                }
            }


            //***Calculates chart from specified date***

            //fromDate = dateTimePicker1.Value;
            //endDate = dateTimePicker2.Value;

            foreach (var item in sortedDictionary)
            {
                if (item.Key > fromDate && item.Key < endDate)
                {
                    //MessageBox.Show(item.Value[0].ToString());

                    y = y + (double)(item.Value[0]);


                }
                else if (sortedDictionary.Keys.Last() < fromDate)
                {
                    MessageBox.Show("Data is older than selected From Date. Please select earlier date.");
                    break;
                }

            }

            //***Emty folder message***
            if (tradesCount == 0)
            {
                MessageBox.Show("Folder for EC is empty of exported csv files or there is no trades in a specified period.");
            }


            //ShowNetProfit

            int netProfit = (int)y;
            textBox6.Text = netProfit.ToString();


            //***Chart properties***

            chart1.Series["Series1"].XValueType = ChartValueType.DateTime;
            chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            chart1.Series["Series1"].Color = Color.Blue;





        }

        private void CallOnKC()
        {
            string[] xyValues;
            DateTime x;
            double y = 0;
            DateTime fromDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            int tradesCount = 0;
            List<double> trades = new List<double>();
            List<DateTime> dateList = new List<DateTime>();
            SortedList<DateTime, List<double>> sortedDictionary = new SortedList<DateTime, List<double>>(new DuplicateKeyComparer<DateTime>());

            string path = textBox2.Text;

            //***atidaro visus csv exportuotus failus pasirinktame kataloge ir istraukia data, profit losss ir treidu skaiciu ***
            //***Naudojama DuplicateKeyComparer klase dublikatams sujungti "data stucte" SortedList tinka ir Dictionary

            foreach (string txtName in Directory.GetFiles(path, "*KC*.csv"))
            {
                using (StreamReader file = new StreamReader(txtName))
                {
                    string line = file.ReadLine();
                    dateList.Clear();
                    trades.Clear();

                    while (line != null)

                    {

                        xyValues = line.Split(',');
                        //MessageBox.Show(xyValues[1]);
                        string dateModify = xyValues[0];
                        string date = dateModify.Remove(8, 4);
                        x = DateTime.ParseExact(date, string.Format("yyyyMMdd"), null);
                        y = double.Parse(xyValues[1]);
                        double trade = double.Parse(xyValues[6]);
                        trades.Add(trade);
                        dateList.Add(x);
                        sortedDictionary.Add(x, new List<double> { y, trade });
                        line = file.ReadLine();
                        y = 0;

                    }

                    //***Skaiciuoja treidus nuo pasirinktos datos, nes jie sudetiniai***

                    fromDate = dateTimePicker1.Value;
                    endDate = dateTimePicker2.Value;


                    foreach (DateTime item in dateList)
                    {
                        if (item > fromDate && item < endDate)
                        {

                            int index = dateList.IndexOf(item);
                            //MessageBox.Show(index.ToString());
                            if (index == 0)
                            {
                                tradesCount++;
                            }
                            else
                            if (trades[index] != trades[index - 1])
                            {
                                tradesCount++;
                            }

                        }

                    }

                }
            }


            //***Calculates chart from specified date***

            //fromDate = dateTimePicker1.Value;
            //endDate = dateTimePicker2.Value;

            foreach (var item in sortedDictionary)
            {
                if (item.Key > fromDate && item.Key < endDate)
                {
                    //MessageBox.Show(item.Value[0].ToString());

                    y = y + (double)(item.Value[0]);


                }
                else if (sortedDictionary.Keys.Last() < fromDate)
                {
                    MessageBox.Show("Data is older than selected From Date. Please select earlier date.");
                    break;
                }

            }

            //***Emty folder message***
            if (tradesCount == 0)
            {
                MessageBox.Show("Folder for KC is empty of exported csv files or there is no trades in a specified period.");
            }


            //ShowNetProfit

            int netProfit = (int)y;
            textBox7.Text = netProfit.ToString();


            //***Chart properties***

            chart1.Series["Series1"].XValueType = ChartValueType.DateTime;
            chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            chart1.Series["Series1"].Color = Color.Blue;





        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string path = textBox2.Text;

            if (path == string.Empty)
            {
                MessageBox.Show("Select folder to continue");
                return;
            }


            CallOnAll();
            CallOnES();
            CallOnEMD();
            CallOnNQ();
            CallOnEC();
            CallOnKC();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lab_X_Axis.AutoSize = false;
            lab_Y_Axis.AutoSize = false;
            lab_X_Axis.Text = "";
            lab_Y_Axis.Text = "";
            lab_X_Axis.Size = new Size(1, 690);
            lab_Y_Axis.Size = new Size(950, 1);

            label1.Text = "From Date";
            label2.Text = "Folder Path";
            label4.Text = "ES";

            string defaultPath = @"C:\\Temp\" + DateTime.Now.ToString("yyyyMMdd");
            textBox2.Text = defaultPath;
            

        }

         private void lab_X_Axis_Click(object sender, EventArgs e)
        {

        }

        private void lab_Y_Axis_Click(object sender, EventArgs e)
        {

        }

        private void chart1_MouseMove_1(object sender, MouseEventArgs e)

        {
           /* lab_X_Axis.Location = new Point((e.X), 25);
            lab_Y_Axis.Location = new Point(107, e.Y);

            if (e.X <= 107 || e.Y >= 690 || e.Y <= 25 || e.X >= 1020)
            {
                lab_X_Axis.Visible = false;
                lab_Y_Axis.Visible = false;
                lab_X_Axis_Curr.Visible = false;

            }
            else
            {
                lab_X_Axis.Visible = true;
                lab_Y_Axis.Visible = true;
                lab_X_Axis_Curr.Visible = true;
            }*/

            try
            {
                double yValue = chart1.ChartAreas[0].AxisY2.PixelPositionToValue(e.Y);
                double xValue = chart1.ChartAreas[0].AxisX2.PixelPositionToValue(e.X);

                
                string xValueFromDoubleToDate = DateTime.FromOADate(xValue).ToString("MM/dd/yyyy");

                lab_X_Axis_Curr.Text = (Math.Round(yValue).ToString("000 000") + " $" + ",     " + xValueFromDoubleToDate);

                lab_X_Axis_Curr.Location = new Point(e.X + 20, e.Y - 20);
            }

            catch
            {

            }

            finally
            {

            }
        }

        
        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }

            

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy/MM/dd";
            

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.CustomFormat = "yyyy/MM/dd";
            

        }



        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            textBox1.Clear();
            textBoxES.Clear();
            //Form1 frm1 = new Form1();

            //frm1.Show();
            //frm1.Close();
        }

        private void SaveImage_Click(object sender, EventArgs e)
        {
            string path = null;
            string name = DateTime.Now.ToString("yyyyMMdd_HHmm");
            
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }

            
            chart1.SaveImage(path + @"\" + name + ".jpg", ChartImageFormat.Jpeg);
            MessageBox.Show("Image saved.");
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
