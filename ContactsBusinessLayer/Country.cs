using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountriesDataAccessLayer;

namespace CountriesBusinessLayer
{
    public class clsCountry
    {

        public int ID { set; get; }
        public string CountryName { set; get; }
        public string Code { set; get; }
        public string PhoneCode { set; get; }

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsCountry()
        {
            this.ID = -1;
            this.CountryName = string.Empty;
            this.Code = string.Empty;
            this.PhoneCode = string.Empty;

            Mode = enMode.AddNew;
        }
        private clsCountry(int ID, string CountryName, string Code, string PhoneCode)
        {
            this.ID = ID;
            this.CountryName = CountryName;
            this.Code = Code;
            this.PhoneCode = PhoneCode;

            Mode = enMode.Update;
        }

        public static clsCountry Find(int ID)
        {
            string CountryName = string.Empty;
            string Code = string.Empty;
            string PhoneCode = string.Empty;

            if (clsCountryDataAcess.GetCountryInfoByID(ID, ref CountryName, ref Code, ref PhoneCode))
            {
                return new clsCountry(ID, CountryName, Code, PhoneCode);
            }
            return null;
        }
        public static clsCountry Find(string CountryName)
        {
            int ID = 0;
            string Code = string.Empty, PhoneCode = string.Empty;

            if (clsCountryDataAcess.GetCountryInfoByCountryName(CountryName, ref ID, ref Code, ref PhoneCode))
            {
                return new clsCountry(ID, CountryName, Code, PhoneCode);
            }
            return null;
        }
        private bool _AddNewCountry()
        {
            this.ID = clsCountryDataAcess.AddNewCountry(CountryName, Code, PhoneCode);
            return ID != -1;
        }
        private bool _UpdateCountry()
        {
            return clsCountryDataAcess.UpdateCountry(ID, CountryName, Code, PhoneCode);
        }
        public static bool DeleteCountry(int ID)
        {

            return clsCountryDataAcess.DeleteCountry(ID);


        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCountry())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    _UpdateCountry();
                    break;
            }

            return false;
        }
        public static DataTable GetAllCountrys()
        {
            return clsCountryDataAcess.GetAllCountries();
        }
        public static bool isCountryExist(int ID)
        {
            return clsCountryDataAcess.isCountryExist(ID);
        }
        public static bool isCountryExist(string CountryName)
        {
            return clsCountryDataAcess.isCountryExist(CountryName);
        }
    }
}
