using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        string conectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ксения\source\repos\WpfApp5\WpfApp5\Data1.mdf;Integrated Security=True";
        ObservableCollection<string> cex;
        private string name;
        private bool bl;
        string tp;
        int bip;
        string sql;

        public Add(string ng)
        {
            InitializeComponent();
            tp = ng;
        }
        public Add(bool vis, string cim, string ng, int bt)
        {
            InitializeComponent();
            bl = vis;
            tp = ng;
            bip = bt;
            Bind_Name.Visibility = Visibility.Visible;
            Bind.Text = cim;
            Show_Shops(bt);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            name = textBox.Text;
            int Idsh;

            if (Check_Name(name) || name.LastIndexOf(" ") != -1 || name.IndexOf(" ") == 0 || name.Length >= 30)
            {
                return;
            }

            if (bl)
            {
                if (Bind_Name.SelectedIndex == -1) return;
                Idsh = SearchId(Bind_Name.SelectedItem.ToString(), bip); // вернуть Id элемента из таблицы
                sql = "INSERT INTO " + tp + " VALUES (N'" + name + "', " + Idsh + ")";
            }
            else
            {
                sql = "INSERT INTO " + tp + " VALUES (N'" + name + "')";
            }

            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = sql;
                SqlCommand command = new SqlCommand(sqlExp, connection);
                command.ExecuteNonQuery();
            }
            this.Close();
        }
        private bool Check_Name(string itr)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM Shops";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExp, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                DataTable dt = dataSet.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    var cells = row.ItemArray;
                    if ((string)cells[1] == itr)
                    {
                        return true;
                    }
                }

            }
            return false;
        } //проверяет есть ли такая строка в списке
        private void Show_Shops(int bt)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                switch (bt)
                {
                    case 0:
                        sql = "SELECT * FROM Shops";
                        break;
                    case 2:
                        sql = "SELECT * FROM Type_Sensor";
                        break;
                }

                connection.Open();
                string sqlExp = sql;
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    cex = new ObservableCollection<string> { };

                    while (reader.Read())
                    {
                        cex.Add(reader.GetString(1));
                    }
                }

                reader.Close();

                Bind_Name.ItemsSource = cex;
            }

        } //показать цеха

        public int SearchId(string itr, int bt)
        {
            int d = -1;
            string sql = "";

            switch (bt)
            {
                case 0:
                    sql = "SELECT * FROM Shops";
                    break;
                case 2:
                    sql = "SELECT * FROM Type_Sensor";
                    break;
            }

            if (sql == null) return 0;

            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = sql;
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExp, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                DataTable dt = dataSet.Tables[0];


                foreach (DataRow row in dt.Rows)
                {
                    var cells = row.ItemArray;
                    if ((string)cells[1] == itr)
                    {
                        d = (int)cells[0];
                    }
                }
            }

            return d;
        } // вернуть Id элемента из таблицы

    }
}
