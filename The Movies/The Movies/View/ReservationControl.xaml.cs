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
using The_Movies.Repository;
using The_Movies.ViewModel;

namespace The_Movies.View
{
    /// <summary>
    /// Interaction logic for ReservationControl.xaml
    /// </summary>
    public partial class ReservationControl : UserControl
    {
        public ReservationControl()
        {
            InitializeComponent();
            IMovieProgramRepo movieProgramRepo = new IMovieProgramFileRepo("movie_programs.txt");
            ICustomerProgramRepo customerRepo = new CustomerProgramFileRepo("customer.txt");
            IReservationProgramRepo reserveRepo = new ReservationProgramFileRepo("reservations.txt");
            ScheduleControlViewModel vm = new ScheduleControlViewModel(movieProgramRepo, customerRepo, reserveRepo);
            DataContext = vm;
        }
    }
}
