using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для settings.xaml
    /// </summary>
    public partial class settings : Window
    {
        string conectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ксения\source\repos\WpfApp5\WpfApp5\Data1.mdf;Integrated Security=True";
        ObservableCollection<string> cex;
        ObservableCollection<string> type;
        ObservableCollection<string> machine;
        ObservableCollection<string> subtype;

        string str;
        string mach;
        string ts;
        string subt;
        int sh = -1;
        int typ = -1;

        public settings()
        {
            InitializeComponent();
            ShowShop();
            ShowType();
        }
        public void ShowShop()
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

                Shop.ItemsSource = cex;
            }
        } //показать цеха
        public void ShowType()
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

                Type.ItemsSource = type;


            }

        } //показать типы
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
                            machine.Add(reader.GetString(1));
                    }
                }

                reader.Close();

                Machine.ItemsSource = machine;
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
                            subtype.Add(reader.GetString(1));
                    }
                }

                reader.Close();

                SubType.ItemsSource = subtype;
            }


        } //показать подтипы при выборе типа

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

        private void Shop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            object obj = ((Selector)sender).SelectedItem;
            if (obj == null) return;
            str = (string)obj;
            int d = SearchId(str, 0);
            sh = d;
            ShowMachine(d);
        } //выбрать цех из списка
        private void Type_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            object obj = ((Selector)sender).SelectedItem;
            if (obj == null) return;
            ts = (string)obj;
            int d = SearchId(ts, 2);
            typ = d;
            ShowSubType(d);
        } //выбрать тип из списка

        private void Machine_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            object obj = ((Selector)sender).SelectedItem;
            if (obj == null) return;
            mach = (string)obj;
        } //выбрать установку из списка
        private void SubType_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            object obj = ((Selector)sender).SelectedItem;
            if (obj == null) return;
            subt = (string)obj;
        } //выбрать подтип из списка



        private void Add_Shop_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window is Add)
                    return;
            } //проверка открыто ли окно

            Add add = new Add("Shops(Shop)");
            add.Owner = this;
            add.Show();
            add.Closing += Add_Shop_Closing;
        } //добавить цех
        private void Add_Shop_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowShop();
        } //обновляет список цехов при закрытии окна добавления
        private void Delete_Shop_Click(object sender, RoutedEventArgs e)
        {
            bool bl = false;
            if (str == null) return;
            int d = SearchId(str, 0);
            using(SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT Id_Shop FROM Machine";
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (d == (int)reader.GetValue(0)) bl = true;
                    }
                }
            }

            if(!bl)
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "DELETE FROM Shops WHERE Shop=N'" + str + "'";
                SqlCommand command = new SqlCommand(sqlExp, connection);
                int n = command.ExecuteNonQuery();
                ShowShop();
            }
        } //удаляет выбранный цех


        private void Add_Machine_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window is Add)
                    return;
            } //проверка открыто ли окно

            Add add = new Add(true, "Выберите привязку к цеху:", "Machine(Machine_Name, Id_Shop)", 0);
            add.Owner = this;
            add.Show();
            add.Closing += Add_Machine_Closing;

        } //добавить установку
        private void Add_Machine_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowMachine(sh);
        } //обновляет список установок при закрытии окна добавления
        private void Delete_Machine_Click(object sender, RoutedEventArgs e)
        {
            if (mach == null) return;
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "DELETE FROM Machine WHERE Machine_Name=N'" + mach + "'";
                SqlCommand command = new SqlCommand(sqlExp, connection);
                int n = command.ExecuteNonQuery();
                ShowMachine(sh);
            }

        } //удаляет выбранную установку


        private void Add_Type_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window is Add)
                    return;
            } //проверка открыто ли окно

            Add add = new Add("Type_Sensor(Type_Sensor)");
            add.Owner = this;
            add.Show();
            add.Closing += Add_Type_Closing;
        } //добавить тип
        private void Add_Type_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowType();
        } //обновляет список типов при закрытии окна добавления
        private void Delete_Type_Click(object sender, RoutedEventArgs e)
        {
            bool bl = false;
            if (ts == null) return;
            int d = SearchId(ts, 2);
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "SELECT Id_TypeSensor FROM SubType_Sensor";
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (d == (int)reader.GetValue(0)) bl = true;
                    }
                }
            }

            if (!bl)
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "DELETE FROM Type_Sensor WHERE Type_Sensor=N'" + ts + "'";
                SqlCommand command = new SqlCommand(sqlExp, connection);
                int n = command.ExecuteNonQuery();
                ShowType();
            }

        } //удаляет выбранный тип


        private void Add_SubType_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window is Add)
                    return;
            } //проверка открыто ли окно

            Add add = new Add(true, "Выберите привязку к типу:", "SubType_Sensor(SubType_Name, Id_TypeSensor)", 2);
            add.Owner = this;
            add.Show();
            add.Closing += Add_SubType_Closing;
        }
        private void Add_SubType_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowSubType(typ);
        } //обновляет список установок при закрытии окна добавления
        private void Delete_SubType_Click(object sender, RoutedEventArgs e)
        {
            if (subt == null) return;
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                string sqlExp = "DELETE FROM SubType_Sensor WHERE SubType_Name=N'" + subt + "'";
                SqlCommand command = new SqlCommand(sqlExp, connection);
                int n = command.ExecuteNonQuery();
                ShowSubType(typ);
            }

        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
    }
}
