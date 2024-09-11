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

    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities1 _db;
        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
        }

        private AddRentalRecord _addRentalRecordForm;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            {
                if (_addRentalRecordForm == null || _addRentalRecordForm.IsDisposed)
                {
                    // Create and open the form if it is not already open
                    _addRentalRecordForm = new AddRentalRecord(this);
                    _addRentalRecordForm.MdiParent = this.MdiParent;
                    _addRentalRecordForm.Show();
                }
                else
                {
                    // If it's already open, bring it to the front
                    _addRentalRecordForm.BringToFront();
                }
                //AddRentalRecord addRentalRecord = new AddRentalRecord(this);
                //addRentalRecord.MdiParent = this.MdiParent;
                //addRentalRecord.Show();
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //get ID of the ROW
            var id = (int)dgvRecords.SelectedRows[0].Cells["ID"].Value;
            //query for the record
            var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

            //launch addRentalRecord with our record
            var addRentalRecord = new AddRentalRecord(record, this);
            addRentalRecord.MdiParent = this.MdiParent;
            addRentalRecord.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //get Id of the ROW
                var id = (int)dgvRecords.SelectedRows[0].Cells["ID"].Value;
                //query for the record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                DialogResult dr = MessageBox.Show("Are You Sure You Want To Delete This Record?",
                    "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    _db.CarRentalRecords.Remove(record);
                    _db.SaveChanges();

                    RefreshForm();
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
  
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            RefreshForm();

        }
        public void RefreshForm()
        {
            dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            var records = _db.CarRentalRecords.Select(q => new
            {
                Id = q.id,
                CustomerName = q.CustomerName,
                DepartDate = q.DateDepart,
                Return = q.DateArrive,
                Cost = q.Cost
                    ,
                Car = q.TypesOfCar.Year + " " + q.TypesOfCar.Make + " " + q.TypesOfCar.Model
            }).ToList();
            dgvRecords.DataSource = records;
            dgvRecords.Columns["Id"].Visible = false;
        }
    }
}
