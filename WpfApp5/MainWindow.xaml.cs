using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;

namespace WpfApp5
{
    public class Sensors
    {
        public int Id { get; set; }
        public int Id_Machine { get; set; }
        public int Id_TSensor { get; set; }
        public int Id_SubType { get; set; }
        public string Name_Sensor { get; set; }
        public string Photo_Sensor { get; set; }
        public string Existence { get; set; }
    }
    public partial class MainWindow : Window
    {
        string conectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ксения\source\repos\WpfApp5\WpfApp5\Data1.mdf;Integrated Security=True";

        ObservableCollection<string> cex;
        ObservableCollection<string> type;
        ObservableCollection<Sensors> sensor;
        List<int> MachineSensorId = new List<int> { };
        List<int> MachineSensorId_Machine = new List<int> { };

        Sensors sensors;

        byte bt = 0;
        byte br = 0;

        string[] hg = new string[5];
        string[] hgLB2 = new string[3];
        public string heading { get; set; }
        DataTable dt;

        int IdShop;
        int IdMachine;
        int IdTypeSensor;
        int IdSubType;

        /// <summary>
        /// метод запускается первым и показывает цеха
        /// </summary>
        private void Start_and_Shop()
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM Shops";
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

                LB.ItemsSource = cex;

                heading = "ВСЕ";
                TB.Text = heading;
                heading = null;

            }

        } 

        /// <summary>
        /// метод запускается первым и показывает типы датчиков
        /// </summary>
        private void Start_and_TypeSensor()
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM Type_Sensor";
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    type = new ObservableCollection<string> { };

                    while (reader.Read())
                    {
                        type.Add(reader.GetString(1));
                    }
                }


                reader.Close();

                LB2.ItemsSource = type;


            }

        } 

        /// <summary>
        /// показывает установки при выборе цеха
        /// </summary>
        /// <param name="d"></param>
        /// <param name="itr"></param>
        private void Show_Machine(int d, string itr)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM [Machine]";
                bt = 1;
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    cex = new ObservableCollection<string> { ".." };

                    while (reader.Read())
                    {
                        if (d == (int)reader.GetValue(2))
                        {
                            cex.Add(reader.GetString(1));
                        }
                    }
                }
                reader.Close();
                LB.ItemsSource = cex;

                //heading = itr;
                //TB.Text = heading;

            }
        } 

        /// <summary>
        /// показывает подтипы при выборе типа в LB2
        /// </summary>
        /// <param name="d"></param>
        private void Show_SubType_Sensor_LB2(int d)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM [SubType_Sensor]";
                br = 1;
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    type = new ObservableCollection<string> { ".." };

                    while (reader.Read())
                    {
                        if (d == (int)reader.GetValue(2))
                        {
                            type.Add(reader.GetString(1));
                        }
                    }
                }
                reader.Close();
                LB2.ItemsSource = type;

            }
        } 

        /// <summary>
        /// показывает подипы при выборе типа в Lb
        /// </summary>
        /// <param name="d"></param>
        /// <param name="itr"></param>
        private void Show_SubType_Sensor_LB(int d, string itr)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM [SubType_Sensor]";
                bt = 3;
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    cex = new ObservableCollection<string> { ".." };

                    while (reader.Read())
                    {
                        if (d == (int)reader.GetValue(2))
                        {
                            cex.Add(reader.GetString(1));
                        }
                    }
                }
                reader.Close();
                LB.ItemsSource = cex;

                //heading += "       ";
                //heading += itr;
                //TB.Text = heading;

            }
        }

        /// <summary>
        /// показать тип датчика при выборе установки
        /// </summary>
        /// <param name="d"></param>
        /// <param name="itr"></param>
        private void Show_TypeSensor(int d, string itr)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM [Type_Sensor]";
                bt = 2;
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    cex = new ObservableCollection<string> { ".." };

                    while (reader.Read())
                    {
                        cex.Add(reader.GetString(1));
                    }
                }
                reader.Close();
                LB.ItemsSource = cex;

                //heading += itr;
                //TB.Text = heading;
            }
        }

        /// <summary>
        /// показать датчики в SensorsLB
        /// </summary>
        private void Show_Sensor()
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM Sensor";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExp, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                sensor = new ObservableCollection<Sensors> { };

                dt = dataSet.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    sensor.Add(GetSensors(row));
                }

                SensorsLB.ItemsSource = sensor;
            }
        }

        /// <summary>
        /// обновить dt
        /// </summary>
        private void Update_dt()
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT * FROM Sensor";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExp, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                dt = dataSet.Tables[0];
            }

        }

        /// <summary>
        /// показать датчики в SensorsLB
        /// </summary>
        /// <param name="d"></param>
        /// <param name="bt"></param>
        private void Show_Sensor(int d, int bt)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                sensor = new ObservableCollection<Sensors> { };

                foreach (DataRow row in dt.Rows)
                {
                    if(GetSensors(row, d, bt)!=null)
                        sensor.Add(GetSensors(row, d, bt));
                }
                MachineSensorId = new List<int> { };

            }

            SensorsLB.ItemsSource = sensor;
        }

        /// <summary>
        /// //показать датчики в SensorsLB2
        /// </summary>
        /// <param name="d"></param>
        /// <param name="bt"></param>
        private void Show_SensorLB2(int d, int bt)
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                sensor = new ObservableCollection<Sensors> { };

                foreach (DataRow row in dt.Rows)
                {
                    if (GetSensorsLB2(row, d, bt) != null)
                        sensor.Add(GetSensorsLB2(row, d, bt));
                }

            }

            SensorsLB.ItemsSource = sensor;
        } 


        public Sensors GetSensors(DataRow row)
        {
            sensors = new Sensors();

            var cells = row.ItemArray;
            sensors.Id = (int)cells[0];
            sensors.Name_Sensor = (string)cells[5];
            sensors.Photo_Sensor = (string)cells[6];
            if ((int)cells[10] == 0) sensors.Existence = "Нет в наличии!";
            else sensors.Existence = "В наличии " + (int)cells[10] + " шт.";

            return sensors;
        }
        public Sensors GetSensors(DataRow row, int d, int bt)
        {
            sensors = new Sensors();
            var cells = row.ItemArray;

            switch (bt)
            {
                case 0:
                    sensors.Id = (int)cells[0];
                    sensors.Name_Sensor = (string)cells[5];
                    sensors.Photo_Sensor = (string)cells[6];
                    if ((int)cells[10] == 0) sensors.Existence = "Нет в наличии!";
                    else sensors.Existence = "В наличии " + (int)cells[10] + " шт.";
                    break;
                case 1:

                    //new

                    using (SqlConnection connection = new SqlConnection(conectionString))
                    {
                        connection.Open();
                        string sqlExp = "SELECT * FROM Machine_Sensor WHERE MachaneId = " + d;
                        SqlCommand command = new SqlCommand(sqlExp, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MachineSensorId.Add((int)reader.GetValue(1));
                            }
                        }
                    }

                    if (MachineSensorId.BinarySearch((int)cells[0]) > -1)
                    {
                        sensors.Id = (int)cells[0];
                        sensors.Name_Sensor = (string)cells[5];
                        sensors.Photo_Sensor = (string)cells[6];
                        if ((int)cells[10] == 0) sensors.Existence = "Нет в наличии!";
                        else sensors.Existence = "В наличии " + (int)cells[10] + " шт.";
                    }
                    else
                    {
                        sensors = null;
                    }
                    break;


                //new

                //if (d == (int)cells[2] && IdShop == (int)cells[1])
                //{
                //    sensors.Id = (int)cells[0];
                //    sensors.Name_Sensor = (string)cells[5];
                //    sensors.Photo_Sensor = (string)cells[6];
                //}
                //else 
                //{
                //    sensors = null;
                //}
                //break;

                case 2:
                    using (SqlConnection connection = new SqlConnection(conectionString))
                    {
                        connection.Open();
                        string sqlExp = "SELECT * FROM Machine_Sensor WHERE MachaneId = " + IdMachine;
                        SqlCommand command = new SqlCommand(sqlExp, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MachineSensorId.Add((int)reader.GetValue(1));
                            }
                        }
                    }
                    if (d == (int)cells[3] && MachineSensorId.BinarySearch((int)cells[0]) > -1)
                    {
                        sensors.Id = (int)cells[0];
                        sensors.Name_Sensor = (string)cells[5];
                        sensors.Photo_Sensor = (string)cells[6];
                        if ((int)cells[10] == 0) sensors.Existence = "Нет в наличии!";
                        else sensors.Existence = "В наличии " + (int)cells[10] + " шт.";
                    }
                    else
                    {
                        sensors = null;
                    }
                    break;

                case 3:
                    using (SqlConnection connection = new SqlConnection(conectionString))
                    {
                        connection.Open();
                        string sqlExp = "SELECT * FROM Machine_Sensor WHERE MachaneId = " + IdMachine;
                        SqlCommand command = new SqlCommand(sqlExp, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MachineSensorId.Add((int)reader.GetValue(1));
                            }
                        }
                    }
                    if (d == (int)cells[4] && MachineSensorId.BinarySearch((int)cells[0]) > -1 && IdTypeSensor == (int)cells[3])
                    {
                        sensors.Id = (int)cells[0];
                        sensors.Name_Sensor = (string)cells[5];
                        sensors.Photo_Sensor = (string)cells[6];
                        if ((int)cells[10] == 0) sensors.Existence = "Нет в наличии!";
                        else sensors.Existence = "В наличии " + (int)cells[10] + " шт.";
                    }
                    else
                    {
                        sensors = null;
                    }
                    break;
            }

            return sensors;
        }
        public Sensors GetSensorsLB2(DataRow row, int d, int br)
        {
            sensors = new Sensors();
            var cells = row.ItemArray;

            switch (br)
            {
                case 0:
                    if (d == (int)cells[3])
                    {
                        sensors.Id = (int)cells[0];
                        sensors.Name_Sensor = (string)cells[5];
                        sensors.Photo_Sensor = (string)cells[6];
                        if ((int)cells[10] == 0) sensors.Existence = "Нет в наличии!";
                        else sensors.Existence = "В наличии " + (int)cells[10] + " шт.";
                    }
                    else
                    {
                        sensors = null;
                    }
                    break;
                case 1:
                    if (d == (int)cells[4] && IdTypeSensor == (int)cells[3])
                    {
                        sensors.Id = (int)cells[0];
                        sensors.Name_Sensor = (string)cells[5];
                        sensors.Photo_Sensor = (string)cells[6];
                        if ((int)cells[10] == 0) sensors.Existence = "Нет в наличии!";
                        else sensors.Existence = "В наличии " + (int)cells[10] + " шт.";
                    }
                    else
                    {
                        sensors = null;
                    }
                    break;
            }

            return sensors;
        }

        public void Select_SubType(string itr)
        {
            //heading = tr;
            //heading += "       ";
            //itr.Replace("\n", " ");
            //heading += itr;
            //TB.Text = heading;

        }
        public void Heading(string itr, int bt)
        {
            if (itr == "..")
                hg[bt] = "";
            else
            {
                itr = itr.Replace("\n", " ");
                itr = itr.Replace("\r", "");
                hg[bt] = itr;
            }
            for (int i=0; i<=bt; i++)
            {
                heading += hg[i];
                heading += "      ";
            }
            TB.Text = heading;
            heading = null;
        } 
        public void HeadingLB2(string itr, int bt)
        {
            itr = itr.Replace("\n", " ");
            itr = itr.Replace("\r", "");
            hgLB2[bt] = itr;

            for (int i = 0; i <= bt; i++)
            {
                heading += hgLB2[i];
                heading += "      ";
            }
            TB.Text = heading;
            heading = null;
        }

        public int SearchId(string itr, int bt)
        {
            int d = -1;
            string sql = "";

            switch (bt)
            {
                case 0: sql = "SELECT * FROM Shops";
                    break;
                case 1: sql = "SELECT * FROM [Machine]";
                    break;
                case 2: sql = "SELECT * FROM [Type_Sensor]";
                    break;
                case 3: sql = "SELECT * FROM [SubType_Sensor]";
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
                    if (bt == 1)
                    {
                        if ((string)cells[1] == itr && IdShop == (int)cells[2])
                        {
                            d = (int)cells[0];
                        }

                    }
                    else
                    if ((string)cells[1] == itr)
                    {
                        d = (int)cells[0];
                    }
                }
            }


            return d;
        } // вернуть Id элемента из таблицы
        public int SearchIdLB2(string itr, int br)
        {
            int d = -1;
            string sql = "";

            switch (br)
            {
                case 0:
                    sql = "SELECT * FROM Type_Sensor";
                    break;
                case 1:
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

        }
        public MainWindow()
        {
            InitializeComponent();

            Start_and_Shop();
            Start_and_TypeSensor();
            Show_Sensor();
        }


        /// <summary>
        /// обработка выбора LB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            object obj = ((Selector)sender).SelectedItem;

            if (obj == null) //проверка выбрано ли что-то
            {
                return;
            }

            string itr = ((Selector)sender).SelectedItem.ToString();


            if (itr == "..")
            {
                switch (bt)
                {
                    case 1:
                        Start_and_Shop();
                        Show_Sensor();
                        bt = 0;
                        Heading("ВСЕ", bt);
                        break;
                    case 2:
                        Show_Machine(IdShop, itr);
                        Show_Sensor(IdShop, --bt);
                        bt++;
                        Heading(itr, bt);
                        break;
                    case 3:
                        Show_TypeSensor(IdMachine, itr);
                        Show_Sensor(IdMachine, --bt);
                        bt++;
                        Heading(itr, bt);
                        break;
                }
                //Start_and_Shop();
                //Show_Sensor();
                //bt = 0;
                return;
            }

            int d = SearchId(itr, bt); //получить Id выбраного элемента

            if (d >= 0)
            {
                Update_dt(); //обновить dt
                Show_Sensor(d, bt);

                switch (bt)
                {
                    case 0:
                        Heading(itr, bt); //записывает заголовок
                        Show_Machine(d, itr); //показывает список установок
                        IdShop = d;
                        break;
                    case 1:
                        Heading(itr, bt); //записывает заголовок
                        Show_TypeSensor(d, itr); //показывает список типов
                        IdMachine = d;
                        break;
                    case 2:
                        Heading(itr, bt); //записывает заголовок
                        Show_SubType_Sensor_LB(d, itr); //показывает список подтипов
                        IdTypeSensor = d;
                        break;
                    case 3:
                        Heading(itr, bt); //записывает заголовок
                        Select_SubType(itr); //показывает датчики
                        IdSubType = d;
                        break;
                }
            }


        }

        /// <summary>
        /// обработка выбора в LB2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LB2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            object obj = ((Selector)sender).SelectedItem;

            if (obj == null) //проверка выбрано ли что-то
            {
                return;
            }

            string itr = ((Selector)sender).SelectedItem.ToString(); //получить выделенную позицию в таблице

            if (itr == "..")
            {
                heading = "ВСЕ";
                TB.Text = heading;
                heading = null;
                Start_and_TypeSensor();
                Show_Sensor();
                br = 0;
                return;
            }

            int d = SearchIdLB2(itr, br); //получить Id выбраного элемента

            if (d >= 0)
            {
                Update_dt(); //обновить dt
                Show_SensorLB2(d, br);

                switch (br)
                {
                    case 0:
                        HeadingLB2(itr, br);
                        Show_SubType_Sensor_LB2(d);
                        IdTypeSensor = d;
                        break;
                    case 1:
                        HeadingLB2(itr, br);
                        IdSubType = d;
                        break;
                }

            }

        }


        /// <summary>
        /// открытие окна Sensor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SensorsLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach(Window window in App.Current.Windows)
            {
                if (window is Sensor)
                    return;
            } //проверка открыто ли окно

            sensors = ((Sensors)((Selector)sender).SelectedItem);
            if (sensors == null) return;

            Sensor sens = new Sensor(sensors);
            sens.Owner = this;
            sens.Show();
        } 


        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

    }
}
