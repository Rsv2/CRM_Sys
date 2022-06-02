using System.Windows.Controls;

namespace AdaptHostURL
{
    /// <summary>
    /// Логика взаимодействия для HoistUI.xaml
    /// </summary>
    public partial class HostUI : UserControl
    {
        public HostUI()
        {
            DataContext = new ViewModel();
            InitializeComponent();
        }
    }
}
