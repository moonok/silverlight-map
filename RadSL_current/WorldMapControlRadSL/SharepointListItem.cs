using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WorldMapControlRadSL
{
    public class SharepointListItem
    {
        // CountryName
        private string _Title;
        public string Title
        {
            get { return this._Title; }
            set { this._Title = value; }
        }

        private string _CountryNameAbbreviation;
        public string CountryNameAbbreviation
        {
            get { return this._CountryNameAbbreviation; }
            set { this._CountryNameAbbreviation = value; }
        }

        private string _DataField1;
        public string DataField1
        {
            get { return this._DataField1; }
            set { this._DataField1 = value; }
        }

        private string _DataField2;
        public string DataField2
        {
            get { return this._DataField2; }
            set { this._DataField2 = value; }
        }

        private string _DataField3;
        public string DataField3
        {
            get { return this._DataField3; }
            set { this._DataField3 = value; }
        }

        private string _DataField4;
        public string DataField4
        {
            get { return this._DataField4; }
            set { this._DataField4 = value; }
        }

        private string _DataField5;
        public string DataField5
        {
            get { return this._DataField5; }
            set { this._DataField5 = value; }
        }

        private string _DashboardLink;
        public string DashboardLink
        {
            get { return this._DashboardLink; }
            set { this._DashboardLink = value; }
        }

        private string _Clickable;
        public string Clickable
        {
            get { return this._Clickable; }
            set { this._Clickable = value; }
        }

        public SharepointListItem()
        {
        }

        public SharepointListItem(string title, string countrynameabbrev, string datafield1, string datafield2, string datafield3, string datafield4, string datafield5, string dashboardlink, string clickable)
        {
            this._Title = title;
            this._CountryNameAbbreviation = countrynameabbrev;
            this._DataField1 = datafield1;
            this._DataField2 = datafield2;
            this._DataField3 = datafield3;
            this._DataField4 = datafield4;
            this._DataField5 = datafield5;
            this._DashboardLink = dashboardlink;
            this._Clickable = clickable;
        }
    }
}
