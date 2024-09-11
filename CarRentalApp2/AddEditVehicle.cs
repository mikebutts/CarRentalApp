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
    public partial class AddEditVehicle : Form
    {
        private ManageVehicleListing _parentForm;
        private bool isEditMode;
        private readonly CarRentalEntities1 _db;
        public AddEditVehicle(ManageVehicleListing parentForm)
        {
            isEditMode = false;
            _parentForm = parentForm;
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            _db = new CarRentalEntities1();
        }
        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing parentForm)
        {
            isEditMode = true;
            InitializeComponent();
            _parentForm = parentForm;
            lblTitle.Text = "Edit Vehicle ";
            PopulateFields(carToEdit);
            _db = new CarRentalEntities1();
        }

        private void PopulateFields(TypesOfCar car)
        {   
            lblId.Text = car.Id.ToString();
            txtMake.Text = car.Make;
            txtModel.Text = car.Model;
            txtVIN.Text = car.VIN;
            txtYear.Text = car.Year.ToString();
            txtLPN.Text = car.LicensePlateNumber;


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                var id = int.Parse(lblId.Text);
                var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id    );

                car.Model = txtModel.Text;
                car.Make = txtMake.Text;
                car.VIN = txtVIN.Text;  
                car.Year = int.Parse(txtYear.Text);
                car.LicensePlateNumber = txtLPN.Text;

                _db.SaveChanges();
                this.Close();

            }
            else
            {
                var newCar = new TypesOfCar { 
                    LicensePlateNumber = txtLPN.Text,
                    Make = txtMake.Text,
                    Model = txtModel.Text,
                    VIN = txtVIN.Text,
                    Year = int.Parse(txtYear.Text),
                
                };

                _db.TypesOfCars.Add(newCar);
                _db.SaveChanges();
                this.Close();
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Call the RefreshForm method in Form1
            _parentForm.RefreshForm();
            base.OnFormClosed(e);
        }
    }
}
