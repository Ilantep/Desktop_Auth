using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Desktop_Auth
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Input_Number.IsEnabled = true;
            Input_Password.IsEnabled = false;
            EnterBtn.IsEnabled = false;
            RefreshBtn.IsEnabled= false;
            CodeBox.IsEnabled = false;
            Input_Number.Focus();
        }

        string code;
        DispatcherTimer timer = new DispatcherTimer();

        //Генератор рандомного кода
        public void GenerCode()
        {
            code = null;
            Random rnd = new Random();
            string[] Masive = new string[] { "Q","W","E","R","T","Y","Y","U","I","O","P","A","S","D","F","G","H","J","K","L","Z","X","C","V","B","N","M",
                                             "q","w","e","r","t","y","u","i","o","p","a","s","d","f","g","h","h","j","k","l","z","x","c","v","b","n","m",
                                             "1","2","3","4","5","6","7","8","9","0","!","@","#","$","%","^","&","*","(",")","_","-","=","+",";",":",",",
                                             ".","/","[","{","}","]","|"};

            for (int i = 0; i < 8; i++)
                code += Masive[rnd.Next(Masive.Length)];
            if (MessageBox.Show(code.ToString(), "Code", MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                timer.Interval = TimeSpan.FromSeconds(10);
                timer.Tick += Timer_Tick;
                timer.Start();
                EnterBtn.IsEnabled = true;
                RefreshBtn.IsEnabled = true;
                CodeBox.IsEnabled = true;
            }
        }

        // Вывод окна с окночанием времени
        void Timer_Tick(object sender, EventArgs e)
        {
            code = null;
            MessageBox.Show("Время вышло. Код сброшен");
            timer.Stop();
        }

        //Обновляет код
        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            GenerCode();
            CodeBox.Focus();
        }

        //Очищает все поля, блокирует все поля кроме Номер
        private void DeliteBtn_Click(object sender, RoutedEventArgs e)
        {
            Input_Number.Clear();
            Input_Password.Clear();
            CodeBox.Clear();
            Input_Number.IsEnabled = true;
            Input_Password.IsEnabled = false;
            EnterBtn.IsEnabled = false;
            RefreshBtn.IsEnabled = false;
            CodeBox.IsEnabled = false;
            Input_Number.Focus();
        }

        //После нажатия на кнопку входа, проверяет и выводит ФИО и Роль
        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (code==CodeBox.Text)
            {
                if (Input_Number.Text == "1518")
                {
                    MessageBox.Show("Вы Макаров Овидий Вячеславович, ваша роль Специалист ТП (выездной инженер)");
                }
                if (Input_Number.Text == "1526")
                {
                    MessageBox.Show("Вы Сидоров Арсений Богуславович, ваша роль Директор по развитию");
                }
                if (Input_Number.Text == "1527")
                {
                    MessageBox.Show("Вы Самсонов Егор Тимофеевич, ваша роль Технический департамент");
                }
                if (Input_Number.Text == "1531")
                {
                    MessageBox.Show("Вы Симонова Сильвия Валерьевна, ваша роль Бухгалтер");
                }
                if (Input_Number.Text == "1534")
                {
                    MessageBox.Show("Вы Афанасьева Дарина Львовна, ваша роль Менеджер по работе с клиентами");
                }
                if (Input_Number.Text == "1535")
                {
                    MessageBox.Show("Вы Шубина Мелитта Тарасовна, ваша роль Менеджер по работе с клиентами");
                }
            }
        }

        //Проверка на наличие пользователя
        private void Input_Number_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using (var db = new ProjectEntities())
                {
                    var Users = db.Users.AsNoTracking().FirstOrDefault(u => u.Number == Input_Number.Text);
                    if(Users == null)
                    {
                        MessageBox.Show("Пользователь не найден");
                    }
                    else
                    {
                        Input_Number.IsEnabled = false;
                        Input_Password.IsEnabled = true;
                        Input_Password.Focus();
                    }
                }

            }
        }

        //Проверка на правильность пароля
        private void Input_Password_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using (var db = new ProjectEntities())
                {
                    var Password = db.Users.AsNoTracking().FirstOrDefault(u => u.Number == Input_Number.Text & (u.Password == Input_Password.Password));
                    if (Password == null)
                    {
                        MessageBox.Show("Неверный пароль");
                    }
                    else
                    {
                        Input_Password.IsEnabled = false;
                        CodeBox.IsEnabled = true;
                        RefreshBtn.IsEnabled = true;
                        GenerCode();
                        CodeBox.Focus();
                    }
                }
            }
        }
    }
}
