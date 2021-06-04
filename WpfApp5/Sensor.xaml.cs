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
using WpfApp5;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для Sensor.xaml
    /// </summary>
    public partial class Sensor : Window
    {
        string conectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ксения\source\repos\WpfApp5\WpfApp5\Data1.mdf;Integrated Security=True";
        int Idd;
        string photo;
        string txt = "Имя: ";
        string name;
        string volt = "Напряжение: ";
        string size = "Размер: ";
        string connect = "Разъем: ";
        int exi;
        int sensorId;

        public Sensor(Sensors sensor)
        {
            InitializeComponent();
            sensorId = sensor.Id;
            OpenSensor(sensorId);
            this.Unloaded += Close_Sensor;
        }

        private void Close_Sensor(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// показать всю информацию о датчике
        /// </summary>
        /// <param name="Id"></param>
        public void OpenSensor(int Id)
        {
            Idd = Id;
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM Sensor";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExp, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                DataTable dt = dataSet.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    var cells = row.ItemArray;

                    if(Id == (int)cells[0])
                    {
                        txt += (string)cells[5];
                        name = (string)cells[5];
                        photo = (string)cells[6];
                        volt += (string)cells[8];
                        size += (string)cells[9];
                        connect += (string)cells[11];
                        exi = (int)cells[10];
                    }


                }
                Name.Text = txt;
                Volt.Text = volt;
                Size.Text = size;
                Connector.Text = connect;
                Existence.Text = exi.ToString();
                Uri uri = new Uri(photo, UriKind.Absolute);
                ImageSource imgSource = new BitmapImage(uri);
                Photo.Source = imgSource;
            }

        }

        /// <summary>
        /// обновить количество
        /// </summary>
        /// <param name="Id"></param>
        public void UpdateExi(int Id)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM Sensor";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExp, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                DataTable dt = dataSet.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    var cells = row.ItemArray;

                    if (Id == (int)cells[0])
                    {
                        exi = (int)cells[10];
                    }


                }
                Existence.Text = exi.ToString();
            }

        }


        /// <summary>
        /// убавить количество
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Less_Click(object sender, RoutedEventArgs e)
        {
            if (exi == 0) return;
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                exi--;
                string sqlExp = String.Format("UPDATE Sensor SET Existence={1} WHERE Name_Sensor='{0}'", name, exi);
                SqlCommand command = new SqlCommand(sqlExp, connection);
                command.ExecuteNonQuery();
                UpdateExi(Idd);
            }

        }

        /// <summary>
        /// прибавить количество
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void More_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                exi++;
                string sqlExp = String.Format("UPDATE Sensor SET Existence={1} WHERE Name_Sensor='{0}'", name, exi);
                SqlCommand command = new SqlCommand(sqlExp, connection);
                command.ExecuteNonQuery();
                UpdateExi(Idd);
            }

        }


    }
}
