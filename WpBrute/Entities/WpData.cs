using System.ComponentModel;

namespace WpBrute.Entities
{
    public class WpData : INotifyPropertyChanged
    {
        public const int STATUS_NEW = 1;
        public const int STATUS_NEEDS_ACTION = 2;
        public const int STATUS_SUCCESS = 3;
        public const int STATUS_FAILED = 4;

        private string url;
        private string password;
        private string login;
        private string status;
        private int statusCode;

        public int StatusCode
        {
            get => statusCode;
            set
            {
                if (value == 1)
                {
                    Status = "Новый";
                }
                else if (value == 2)
                {
                    Status = "Требует  действия";
                }
                else if (value == 3)
                {
                    status = "Данные подошли";
                }
                else
                {
                    status = "Данные не подошли";
                }
                OnPropertyChanged("Status");
                statusCode = value;
            }
        }
        public string Url
        {
            get => url;
            set
            {
                url = value;
                OnPropertyChanged("Url");
            }
        }

        public string Password
        {
            get => password; set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Status
        {
            get => status;

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
