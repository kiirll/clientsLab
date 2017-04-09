using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        public byte st = 0;
        
        public XmlReader xmlClients;
        public XmlReader xmlMasters;
        public XmlReader xmlHoldings;

        public DataSet dsClient;
        public DataSet dsMaster;
        public DataSet dsHolding;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //xmlClients = XmlReader.Create("client.xml", new XmlReaderSettings());
                //xmlClients = XmlReader.("client.xml", new XmlReaderSettings());
                //helllo v2.1 kirill
                xmlClients = XmlReader.Create(new StringReader(GET("http://localhost/docxml.php?file=client")));
               
                dsClient = new DataSet();
                dsClient.ReadXml(xmlClients);
                dataGridView1.DataSource = dsClient.Tables[0];

                string[] namecol = new string [16]{ "Фамилия", "Имя", "Акт", "Адрес", "Город", "Штат",
                    "zip код", "Телефон", "Дата открытия", "ss номер", "Картинка",
                    "Дата рождения", "Риск", "Профессия", "Статус", "Интересы"};
                for (int i= 0; i < 16; i++)     dataGridView1.Columns[i].HeaderText = namecol[i];

                dataGridView1.Columns.Remove(dataGridView1.Columns[16]);
                viewHold();
                st = 1;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form about = new About();
            about.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            try
            {
                string img = dataGridView1.CurrentRow.Cells[10].Value.ToString();
                Bitmap image1 = new Bitmap(img);
                pictureBox1.Image = image1;
            }
            catch (Exception ex)  {    }

            try
            {
                if (st == 1)
                {
                    filt(2, "ACCT_NBR");
                }
                else if (st == 2)
                {
                    filt(0, "SYMBOL");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   
            }

        } 
        private void filt(int numcol, string namecol)
        {
            string act = dataGridView1.CurrentRow.Cells[numcol].Value.ToString();
            DataTable firstTable = dsHolding.Tables[0];
            DataView view1 = new DataView(firstTable);
            BindingSource bs = new BindingSource();
            bs.DataSource = view1;
            bs.Filter = String.Format(namecol+" LIKE '{0}'", act);
            dataGridView2.DataSource = bs;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                st = 2;
                xmlMasters = XmlReader.Create(new StringReader(GET("http://localhost/docxml.php?file=masters")));
                dsMaster = new DataSet();
                dsMaster.ReadXml(xmlMasters);
                dataGridView1.DataSource = dsMaster.Tables[0];
                string[] namecol = new string[4] { "Символ", "Со Имя", "Обмен","Курс_Цена" };
                for (int i = 0; i < 4 ; i++) dataGridView2.Columns[i].HeaderText = namecol[i];
                viewHold();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private string GET(string Url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            string output ="";
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                output+=stream.ReadToEnd();
            }
            return output;
        }

        private void viewHold (){
            try
            {
                xmlHoldings = XmlReader.Create(new StringReader(GET("http://localhost/docxml.php?file=holdings")));
                dsHolding = new DataSet();
                dsHolding.ReadXml(xmlHoldings);
                dataGridView2.DataSource = dsHolding.Tables[0];
                string[] namecol = new string[5] { "Акт", "Символ", "Акции", "целевая цена", "целевая дата" };
                for (int i = 0; i < 6; i++) dataGridView2.Columns[i].HeaderText = namecol[i];
            }
            catch (Exception ex)
            { }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void helpContextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpNavigator navigator = HelpNavigator.TopicId;
            Help.ShowHelp (this, @"C:\Users\Кирилл\Documents\Visual Studio 2015\Projects\WindowsFormsApplication1\WindowsFormsApplication1\bin\Debug\help.chm", navigator);
        }
    }
}
