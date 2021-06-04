using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp5;

namespace Store
{
    /// <summary>
    /// Логика взаимодействия для Add_Sensor.xaml
    /// </summary>
    public partial class Add_Sensor : Window
    {
        private string conectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ксения\source\repos\WpfApp5\WpfApp5\Data1.mdf;Integrated Security=True";
        public List<string> photostring = new List<string> { };
        private string sql;
        private ObservableCollection<string> cex;
        private ObservableCollection<string> machine;
        private ObservableCollection<string> subtype;
        private ObservableCollection<int> addust = new ObservableCollection<int> { };
        ObservableCollection<string> nmust = new ObservableCollection<string> { };
        private int i = 0;
        private int idtype = -1;
        private int idsubtype = -1;

        public Add_Sensor()
        {
            InitializeComponent();
            Show_Shops(0);
            Show_Shops(2);
            this.Unloaded += AddMach;
        }

        private void AddMach(object sender, RoutedEventArgs e)
        {
            int idmax;

            using(SqlConnection connection = new SqlConnection(conectionString))
            {
                string sql = "SELECT MAX(Id) FROM Sensor";

                connection.Open();
                string sqlExp = sql;
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    idmax = (int)reader.GetValue(0);

                }
                else return;
            }

            using(SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();

                for (int i=0; i < addust.Count; i++)
                {
                    string sqlExp = "INSERT INTO Machine_Sensor (MachaneId, SensorId) VALUES (" + addust[i].ToString() + ", "+idmax.ToString()+")";
                    SqlCommand command = new SqlCommand(sqlExp, connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        private string GetStringPhoto()
        {
            FileInfo fileInfo;
            string str;
            string it = "";
            OpenFileDialog open = new OpenFileDialog();
            open.ShowDialog();
            str = open.FileName;
            if (str != "")
            {
                fileInfo = new FileInfo(str);
                it = fileInfo.Name; //имя файла
                it = it.Replace(fileInfo.Extension, "");
                FileInfo fl = new FileInfo(@"C:\Users\Ксения\source\repos\WpfApp5\WpfApp5\image\" + it + fileInfo.Extension);
                for(int i=0; fl.Exists; i++)
                {
                    it = it.Remove(it.Length - 1);
                    it += i.ToString();
                    fl = new FileInfo(@"C:\Users\Ксения\source\repos\WpfApp5\WpfApp5\image\" + it + fileInfo.Extension);
                }

                fileInfo.CopyTo(@"C:\Users\Ксения\source\repos\WpfApp5\WpfApp5\image\" + it + fileInfo.Extension, true);
                it = @"C:\Users\Ксения\source\repos\WpfApp5\WpfApp5\image\" + it + fileInfo.Extension;
                photostring.Add(it);
            }
            return it;
        }

        private void photo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string str = GetStringPhoto();
            if (str != "")
            {
                Uri uri = new Uri(str, UriKind.Absolute);
                ImageSource imgSource = new BitmapImage(uri);
                photo.Source = imgSource;
            }

        }

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

                switch (bt)
                {
                    case 0:
                        Ceh.ItemsSource = cex;
                        break;
                    case 2:
                        Type.ItemsSource = cex;
                        break;
                }
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
                case 1:
                    sql = "SELECT * FROM Machine";
                    break;
                case 2:
                    sql = "SELECT * FROM Type_Sensor";
                    break;
                case 3:
                    sql = "SELECT * FROM SubType_Sensor";
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
        public int SearchMachine(string itr, int bt)
        {
            int d = -1;
            string sql = "";

            sql = "SELECT * FROM Machine WHERE Id_Shop = " + bt.ToString();

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
        public void ShowMachine(int d)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM Machine";
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    machine = new ObservableCollection<string> { };

                    while (reader.Read())
                    {
                        if (d == (int)reader.GetValue(2))
                        {
                            string st = reader.GetString(1);
                            st = st.Replace("\n", " ");
                            st = st.Replace("\r", "");
                            machine.Add(st);
                        }
                        
                    }
                }

                reader.Close();

                Ust.ItemsSource = machine;
            }

        } //показать установки при выборе цеха 
        public void ShowSubType(int d)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM SubType_sensor";
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    subtype = new ObservableCollection<string> { };

                    while (reader.Read())
                    {
                        if (d == (int)reader.GetValue(2))
                        {
                            string st = reader.GetString(1);
                            st = st.Replace("\n", " ");
                            st = st.Replace("\r", "");
                            subtype.Add(st);
                        }
                    }
                }

                reader.Close();

                SubType.ItemsSource = subtype;
            }


        } //показать подтипы при выборе типа


        private void Сex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Selector)sender).SelectedItem == null) return;
            ShowMachine(SearchId(((Selector)sender).SelectedItem.ToString(), 0));

        } //выбор цеха
        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Selector)sender).SelectedItem == null) return;
            idtype = SearchId(((Selector)sender).SelectedItem.ToString(), 2);
            ShowSubType(idtype);
        } //выбор типа

        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((Ust.SelectedItem == null) || (Ceh.SelectedItem == null)) return;

            int i = SearchMachine(Ust.SelectedItem.ToString(), SearchId(Ceh.SelectedItem.ToString(), 0));
            if (addust.IndexOf(i) == -1)
                addust.Add(i);
            else return;

            string str = (Ust.SelectedItem.ToString() + ": " + Ceh.SelectedItem.ToString() + "  ");
            nmust.Add(str);
            PerUst.ItemsSource = nmust;


            Ceh.ItemsSource = null;
            Ust.ItemsSource = null;
            Show_Shops(0);
        } //кнопка привязать к уст.

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            object ob = PerUst.SelectedItem;
            int i = nmust.IndexOf((string)ob);
            nmust.Remove((string)ob);
            addust.RemoveAt(i);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WpfApp5.Menu menu = new WpfApp5.Menu(photostring);
            menu.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (idtype == -1 || PerUst.ItemsSource == null || wrName.Text == "" || wrVolt.Text == "" || wrSize.Text == "" || wrConnect.Text == "" || idsubtype == -1 || wrExistence.Text == "")
            {
                System.Windows.MessageBox.Show("Пожалуйста, заполните все пункты", "Не сохранено",MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            int leaght = photostring.Count;

            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "INSERT INTO Sensor (Name_Sensor, Voltage, Size, Connector, Id_TSensor, Id_SubType, Photo_Sensoor, Existence) VALUES (N'" +wrName.Text+"', N'"+wrVolt.Text+"', N'"+wrSize.Text+"', N'"+wrConnect.Text+"', '"+idtype.ToString()+"', '"+idsubtype.ToString()+"', N'"+photostring[leaght-1]+"', "+wrExistence.Text+" )";
                SqlCommand command = new SqlCommand(sqlExp, connection);
                command.ExecuteNonQuery();
            }
            WpfApp5.Menu menu = new WpfApp5.Menu();
            menu.Show();
            this.Close();
        }

        private void SubType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Selector)sender).SelectedItem == null) return;
            idsubtype = SearchId(((Selector)sender).SelectedItem.ToString(), 3);
        }

    }
}
