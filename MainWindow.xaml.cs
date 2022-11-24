using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace TestWPFENTITY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly hospitalEntities hp = new hospitalEntities();
        private readonly ObservableCollection<patient> patients = new ObservableCollection<patient>();

        public MainWindow()
        {
            InitializeComponent();
            dgPatientTable.ItemsSource = patients;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeTable();
        }

        private void InitializeTable()
        {
            // READ
            // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/
            // LINQ

            patients.Clear();
            hp.patients.ToList().ForEach(p => patients.Add(p));
        }

        //Create
        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            using (hp)
            {
                patient Patient = (patient)addPanel.DataContext;

                //tábla
                hp.patients.Add(Patient);
                hp.SaveChanges();

                addPanel.DataContext = new patient();

                InitializeTable();
            }
        }

        int patientId = 0;

        private void btnUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            hp.SaveChanges();
            MessageBox.Show("Sikeres módosítás!");
            InitializeTable();
        }
    }
}
