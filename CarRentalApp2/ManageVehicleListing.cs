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
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalEntities1 _db;
        public ManageVehicleListing()
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            //var cars = _db.TypesOfCars.ToList();    --selects all columns

            var cars = _db.TypesOfCars.Select(q => new {ID = q.Id,  Name = q.Make,Model = q.Model, VIN = q.VIN, Year = q.Year, LicensePlateNumber = q.LicensePlateNumber}).ToList();
            dgvVehicle.DataSource = cars;
            dgvVehicle.Columns[0].HeaderText = "ID";
            dgvVehicle.Columns[1].HeaderText = "NAME";

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditVehicle addEditVehicle = new AddEditVehicle(this);
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //get IS of the ROW
            var id = (int)dgvVehicle.SelectedRows[0].Cells["ID"].Value;
            //query for the car
            var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);
            //launch addeditvehicle with our car
            var addEditVehicle = new AddEditVehicle(car, this);
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //get IS of the ROW
            var id = (int)dgvVehicle.SelectedRows[0].Cells["ID"].Value;
            //query for the car
            var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);

            _db.TypesOfCars.Remove(car);
            _db.SaveChanges();

            dgvVehicle.Refresh();

        }
        public void RefreshForm()
        {
            // Add code to refresh the form (e.g., refresh data in a grid or reset fields)

          
            MessageBox.Show("Refreshing?");
            // Select a custom model collection of cars from database
            var cars = _db.TypesOfCars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlateNumber = q.LicensePlateNumber,
                    q.Id
                })
                .ToList();
            dgvVehicle.DataSource = cars;
            dgvVehicle.Columns[4].HeaderText = "License Plate Number";
            //Hide the column for ID. Changed from the hard coded column value to the name, 
            // to make it more dynamic. 
            dgvVehicle.Columns["Id"].Visible = false;
        }
    }
}
