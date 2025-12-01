using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContactsBusinessLayer;
using CountriesBusinessLayer;

namespace ContactsWinFormSolution
{
    public partial class frmAddEditContact : Form
    {
        public frmAddEditContact(int ContactID)
        {
            InitializeComponent();

            _ContactID = ContactID;
            if (_ContactID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
            

        }
        enum enMode
        {
            AddNew = 0, Update = 1
        }
        private enMode _Mode = enMode.AddNew;
        int _ContactID;
        clsContact _Contact;

        private void _LoadData()
        {
            _FillCountriesInComboBox();
            cbCountry.SelectedIndex = 0;

            if (_Mode == enMode.AddNew)
            {
                this.Text = "Add New Contact";
                _Contact = new clsContact();
                return;
            }

            _Contact = clsContact.Find(_ContactID);

            if (_Contact == null )
            {
                MessageBox.Show("This form will be closed because no contact with ID = " + _ContactID);
                this.Close();
                return;
            }

            this.Text = "Edit Cotnact ID = " + _ContactID;
            lblContactID.Text = _ContactID.ToString();
            txtFirstName.Text = _Contact.FirstName;
            txtLastName.Text = _Contact.LastName;
            txtEmail.Text = _Contact.Email;
            txtPhone.Text = _Contact.Phone;
            txtAddress.Text = _Contact.Address;
            dtpDateOfBirth.Value = _Contact.DateOfBirth;

            if (_Contact.ImagePath != "")
            {
                pictureBox1.Load(_Contact.ImagePath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; // works -- sets the loaded image
                //pictureBox1.BackgroundImageLayout = ImageLayout.Zoom; // not working -- sets the back image 
            }
            btnRemove.Enabled = _Contact.ImagePath != "";
            cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.Find(_Contact.CountryID).CountryName);
        }

        private void _FillCountriesInComboBox()
        {
           DataTable dtCountries = clsCountry.GetAllCountrys();
            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void frmAddEditContact_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void btnSetImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FilePath = openFileDialog1.FileName;
                pictureBox1.Load(FilePath);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = null;
            btnRemove.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int CountryID = clsCountry.Find(_Contact.CountryID).ID;

            _Contact.FirstName = txtFirstName.Text;
            _Contact.LastName = txtLastName.Text;
            _Contact.Email = txtEmail.Text;
            _Contact.Address = txtAddress.Text;
            _Contact.DateOfBirth = dtpDateOfBirth.Value;
            _Contact.CountryID = CountryID;

            if (pictureBox1.ImageLocation != null)
            {
                _Contact.ImagePath = pictureBox1.ImageLocation;
            }
            else
            {
                _Contact.ImagePath = string.Empty;
            }

            if (_Contact.Save())
            {
                MessageBox.Show("Data saved successfully");
            }
            else
            {
                MessageBox.Show("Error: Failed saving data");
            }

            _Mode = enMode.Update;
            lblContactID.Text = _Contact.ID.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
