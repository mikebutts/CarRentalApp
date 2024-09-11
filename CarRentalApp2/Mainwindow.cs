﻿using System;
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
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var addRentalRecord = new AddRentalRecord(this);
            addRentalRecord.MdiParent = this;
            addRentalRecord.Show();
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var vehicleListing = new ManageVehicleListing();
            vehicleListing.MdiParent = this;
            vehicleListing.Show();
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var manageRentalRecords = new ManageRentalRecords();
            manageRentalRecords.MdiParent = this;
            manageRentalRecords.Show();
        }
    }
}
