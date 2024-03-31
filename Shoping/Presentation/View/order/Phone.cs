using System.ComponentModel;

namespace Shoping.Presentation.View.order
{
    public class Phone : INotifyPropertyChanged
    {
        private string _name;
        private int _price;
        private string _manufacturer;
        private string _avatar;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        public int Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Price"));
                }
            }
        }
        public string Avatar
        {
            get
            {
                return _avatar;
            }
            set
            {
                _avatar = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Avatar"));
                }
            }
        }
        public string Manufacturer
        {
            get
            {
                return _manufacturer;
            }
            set
            {
                _manufacturer = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Manufacturer"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
