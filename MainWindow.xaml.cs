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

namespace TestWPFENTITY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        hospitalEntities hp = new hospitalEntities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeTable();
        }

        private void InitializeTable()
        {
            // READ
            // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/
            dgPatientTable.ItemsSource = null;
            // LINQ
            var query = from patient in hp.patients
                        select
                        new
                        {
                            patient.ID,
                            patient.name,
                            patient.phone,
                            patient.email,
                            patient.address,
                            patient.postalZip
                        };
            dgPatientTable.ItemsSource = query.ToList();
        }

        //Create
        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            using (hp)
            {
                patient Patient = new patient()
                {
                    name = tbName.Text,
                    phone = tbPhone.Text,
                    email = tbEmail.Text,
                    address = tbAddress.Text,
                    postalZip = tbPostalZip.Text
                };
                //tábla
                hp.patients.Add(Patient);
                hp.SaveChanges();
                tbName.Text = "";
                tbPhone.Text = "";
                tbEmail.Text = "";
                tbAddress.Text = "";
                tbPostalZip.Text = "";
                InitializeTable();
            }
        }

        int patientId = 0;
        private void dgPatientTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator
                                .ContainerFromIndex(dataGrid.SelectedIndex);
            DataGridCell id;
            try
            {
                id = (DataGridCell)dataGrid.Columns[0].GetCellContent(row).Parent;
            }
            catch
            {
                return;
            }
            lblID.Content = id.Content;

            //lblID.Content = ((TextBlock)id.Content).Text;
            try
            {
                patientId = int.Parse(((TextBlock)id.Content).Text);
            }
            catch
            {
                patientId = 0;
                return;
            }
            DataGridCell name = (DataGridCell)dataGrid.Columns[1].GetCellContent(row).Parent;
            tbNameUpdate.Text = ((TextBlock)name.Content).Text;
            DataGridCell phone = (DataGridCell)dataGrid.Columns[2].GetCellContent(row).Parent;
            tbPhoneUpdate.Text = ((TextBlock)phone.Content).Text;
            DataGridCell email = (DataGridCell)dataGrid.Columns[3].GetCellContent(row).Parent;
            tbEmailUpdate.Text = ((TextBlock)email.Content).Text;
            DataGridCell address = (DataGridCell)dataGrid.Columns[4].GetCellContent(row).Parent;
            tbAddressUpdate.Text = ((TextBlock)address.Content).Text;
            DataGridCell postalZip = (DataGridCell)dataGrid.Columns[5].GetCellContent(row).Parent;
            tbPostalzipUpdate.Text = ((TextBlock)postalZip.Content).Text;
        }

        private void btnUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            if (patientId > 0)
            {
                using (hp)
                {
                    patient patient = hp.patients.Find(patientId);
                    patient.name = tbNameUpdate.Text;
                    patient.phone = tbPhoneUpdate.Text;
                    patient.email = tbEmailUpdate.Text;
                    patient.address = tbAddressUpdate.Text;
                    patient.postalZip = tbPostalzipUpdate.Text;
                    hp.SaveChanges();
                }
                MessageBox.Show("Sikeres módosítás!");
                InitializeTable();

                /*{
                    MainWindow mv = new MainWindow();
                    mv.Show();
                    this.Close();
                }*/
            }
        }
    }
}
