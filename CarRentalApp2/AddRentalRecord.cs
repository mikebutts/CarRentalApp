using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp2
{ 

    public partial class AddRentalRecord : Form
    {

        private ManageRentalRecords _parentForm;
        private MainWindow mainWindow;
        private readonly CarRentalEntities1 _db;
        bool isEditMode;
    public AddRentalRecord(ManageRentalRecords parentForm)
        {
            isEditMode = false;
            InitializeComponent();
            _parentForm = parentForm;
            _db = new CarRentalEntities1();
           
        }
        public AddRentalRecord(CarRentalRecord record, ManageRentalRecords parentForm)
        {
            isEditMode = true;
            InitializeComponent();
            _parentForm = parentForm;
            lblTitle.Text = "Edit Vehicle ";
            _db = new CarRentalEntities1();
            PopulateFields(record);

        }
        private void PopulateFields(CarRentalRecord record)
        {
            lblId.Text = record.id.ToString();
            txtName.Text = record.CustomerName;
            dtpDepart.Value = (DateTime)record.DateDepart;
            dtpArrive.Value = (DateTime)record.DateArrive;
            txtCost.Text = record.Cost.ToString();
        
            LoadCarDataIntoComboBox(record.CarTypeId);




        }
        public AddRentalRecord(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
                string customerName = txtName.Text;
                var dateDepart = dtpDepart.Value;
                var dateArrive = dtpArrive.Value;
                var isvalid = true;
                string errorMessage = "";

                var carType = cbCarType.SelectedItem?.ToString();

                double cost = 0;
                cost = Convert.ToDouble(txtCost.Text);

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isvalid = false;
                    errorMessage += "Please enter missing data \n\r";
                }

                if (dateDepart > dateArrive)
                {
                    isvalid = false;
                    errorMessage += "Invalid Date selection \n\r";
                }

                if (isvalid)
                { 
                    if(isEditMode)
                    {
                        var id = int.Parse(lblId.Text);
                        var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                        record.CustomerName = customerName;
                        record.DateDepart = dateDepart;
                        record.DateArrive = dateArrive;
                        record.Cost = (decimal)cost;
                        record.CarTypeId = (int)cbCarType.SelectedValue;
                        
                        _db.SaveChanges();
                    Close();

                    }
                    else
                    {

                        var rentalRecord = new CarRentalRecord();
                        rentalRecord.CustomerName = customerName;
                        rentalRecord.DateDepart = dateDepart;
                        rentalRecord.DateArrive = dateArrive;
                        rentalRecord.Cost = (decimal)cost;
                        rentalRecord.CarTypeId = (int)cbCarType.SelectedValue;

                        _db.CarRentalRecords.Add(rentalRecord);
                        _db.SaveChanges();

                        MessageBox.Show($"Successfully added rental for {customerName}");
                        Close();

                    }

            }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!isEditMode)
            {
                LoadCarDataIntoComboBox();
            } 
          
        }


        private void LoadCarDataIntoComboBox(int? selectedCarId = null)
        {
            // Retrieve the car data from the database
            var cars = _db.TypesOfCars
                .Select(q => new { Id = q.Id, Car = q.Year + " " + q.Make + " " + q.Model })
                .ToList();

            // Set the DisplayMember and ValueMember for the ComboBox
            cbCarType.DisplayMember = "Car";   // Display the "Car" property
            cbCarType.ValueMember = "Id";      // Use "Id" as the value
            cbCarType.DataSource = cars;       // Set the data source to the list of cars

            // Preselect the car if an Id is provided
            if (selectedCarId.HasValue)
            {
                cbCarType.SelectedValue = selectedCarId.Value; // Set the selected value to the car Id
            }
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Call the RefreshForm method in Form1
            _parentForm.RefreshForm();
            base.OnFormClosed(e);
        }

    }
}
