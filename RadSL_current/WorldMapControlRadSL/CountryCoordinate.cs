using System;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls.Map;
using System.Windows.Data;
using System.Linq;
using System.Xml.Linq;
using System.ServiceModel;
using System.Collections.Generic;
using System.Windows;
using System.IO;

namespace WorldMapControlRadSL
{
    public class Country
    {
        private string _CountryName;
        public string CountryName
        {
            get { return this._CountryName; }
            set { this._CountryName = value; }
        }

        private double _Longitude;
        public double Longitude
        {
            get { return this._Longitude; }
            set { this._Longitude = value; }
        }

        private double _Latitude;
        public double Latitude
        {
            get { return this._Latitude; }
            set { this._Latitude = value; }
        }
    }

    public class CountryCoordinate
    {
        static public List<Country> countryList;

        static public void LoadCountryCoordinate()
        {
            Stream fileInfo = typeof(CountryCoordinate).Assembly.GetManifestResourceStream("WorldMapControlRadSL.country_coordinate.csv");

            if (fileInfo == null)
            {
                return;
            }

            if (countryList == null)
            {
                countryList = new List<Country>();
            }
            using (StreamReader fileReader = new StreamReader(fileInfo))
            {
                string line = string.Empty;
                string[] row;
                while ((line = fileReader.ReadLine()) != null)
                {
                    row = line.Split(',');
                    Country nCountry = new Country();
                    nCountry.CountryName = (row[0] == null) ? string.Empty : row[0];
                    nCountry.Latitude =  Convert.ToDouble((row[1] == null) ? string.Empty : row[1]);
                    nCountry.Longitude = Convert.ToDouble((row[2] == null) ? string.Empty : row[2]);

                    countryList.Add(nCountry);
                }
            }
        }
    }
}
