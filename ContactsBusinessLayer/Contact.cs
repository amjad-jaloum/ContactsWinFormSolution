using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsDataAccessLayer;

namespace ContactsBusinessLayer
{
    public class clsContact
    {
        public int ID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        public DateTime DateOfBirth { set; get; }

        public string ImagePath { set; get; }

        public int CountryID { set; get; }

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsContact() {
            this.ID = -1;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Email = string.Empty;
            this.Phone = string.Empty;
            this.Address = string.Empty;
            this.DateOfBirth = DateTime.Now;
            this.ImagePath = string.Empty;
            this.CountryID = -1;
            Mode = enMode.AddNew;
        }
        private clsContact(int ID, string firstName, string lastName, string email, string phone, string address, DateTime dateOfBirth, string imagePath, int countryID)
        {
            this.ID = ID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
            DateOfBirth = dateOfBirth;
            ImagePath = imagePath;
            CountryID = countryID;

            Mode = enMode.Update;
        }

        public static clsContact Find(int ID)
        {
            string FirtsName = string.Empty;
            string LastName = string.Empty;
            string Email = string.Empty;
            string Phone = string.Empty;
            string Address = string.Empty;
            DateTime DateOfBirth = DateTime.Now;
            string ImagePath = string.Empty;
            int CountryID = -1;
            if (clsContactDataAccess.GetContactInfoByID(ID, ref FirtsName, ref LastName,
                ref Email, ref Phone, ref Address, ref DateOfBirth, ref CountryID, ref ImagePath))
            {
                return new clsContact(ID, FirtsName, LastName, Email, Phone, Address, DateOfBirth, ImagePath, CountryID);
            }
            return null;
        }
        private bool _AddNewContact()
        {
            this.ID = clsContactDataAccess.AddNewContact(FirstName,LastName, Email, Phone, Address, DateOfBirth,CountryID,ImagePath);
            return ID != -1;
        }
        private bool _UpdateContact()
        {
            return clsContactDataAccess.UpdateContact(ID, FirstName, LastName, Email, Phone, Address, DateOfBirth, CountryID, ImagePath);
        }
        public static bool DeleteContact(int ID)
        {

            return clsContactDataAccess.DeleteContact(ID);


        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewContact())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    _UpdateContact();
                    return true;
            }

            return false;
        }

        public static DataTable GetAllContacts()
        {
            return clsContactDataAccess.GetAllContacts();
        }
        public static bool isContactExist(int ID)
        {
            return clsContactDataAccess.isContactExist(ID);
        }

    }
}
