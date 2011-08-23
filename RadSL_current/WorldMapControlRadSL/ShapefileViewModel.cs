using System;
using Telerik.Windows.Controls;

namespace WorldMapControlRadSL
{
    public class ShapefileViewModel : ViewModelBase
    {
#if SILVERLIGHT
        protected const string ShapeRelativeUriFormat = "DataSources/Geospatial/{0}.{1}";
#else
        protected const string ShapeRelativeUriFormat = "/Map;component/Resources/{0}.{1}";
#endif
        protected const string ShapeExtension = "shp";
        protected const string DbfExtension = "dbf";

        private string _region;
        private Uri _shapefileSourceUri;
        private Uri _shapefileDataSourceUri;

        public ShapefileViewModel()
        {
            this.PropertyChanged += this.ShapefileViewModelPropertyChanged;
        }

        public string Region
        {
            get
            {
                return this._region;
            }
            set
            {
                if (this._region != value)
                {
                    this._region = value;
                    this.OnPropertyChanged("Region");
                }
            }
        }

        public Uri ShapefileSourceUri
        {
            get
            {
                return this._shapefileSourceUri;
            }
            set
            {
                if (this._shapefileSourceUri != value)
                {
                    this._shapefileSourceUri = value;
                    this.OnPropertyChanged("ShapefileSourceUri");
                }
            }
        }

        public Uri ShapefileDataSourceUri
        {
            get
            {
                return this._shapefileDataSourceUri;
            }
            set
            {
                if (this._shapefileDataSourceUri != value)
                {
                    this._shapefileDataSourceUri = value;
                    this.OnPropertyChanged("ShapefileDataSourceUri");
                }
            }
        }

        private void ShapefileViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Region")
            {
                this.ShapefileSourceUri = new Uri(string.Format(ShapeRelativeUriFormat, this.Region, ShapeExtension), UriKind.Relative);
                this.ShapefileDataSourceUri = new Uri(string.Format(ShapeRelativeUriFormat, this.Region, DbfExtension), UriKind.Relative);
            }
        }
    }
}
