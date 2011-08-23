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

namespace WorldMapControlRadSL
{
    public class QueryResultItem
    {
        private string _Title;  // CountryName
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

        public QueryResultItem()
        {
        }

        public QueryResultItem(string title, string countrynameabbrev, string datafield1, string datafield2, string datafield3, string dashboardlink, string clickable)
        {
            this._Title = title;
            this._CountryNameAbbreviation = countrynameabbrev;
            this._DataField1 = datafield1;
            this._DataField2 = datafield2;
            this._DataField3 = datafield3;
            this._DashboardLink = dashboardlink;
            this._Clickable = clickable;
        }
    }

    public partial class MainPage : UserControl
    {
        private const string _data1 = "Data Field 1";
        private const string _data2 = "Data Field 2";
        private const string _data3 = "Data Field 3";
        private const string _country = "Country";
        private List<QueryResultItem> myListset = new List<QueryResultItem>();
        private List<FrameworkElement> items;

        public MainPage()
        {
            InitializeComponent();

            LoadInitParams();

            string webserviceUri = @"http://brown-moss/MoonSilverlight/_vti_bin/lists.asmx";
            string sharepointListGuid = @"B7BF0524-9756-4958-A110-E60E533EDEC1";
            XElement query = new XElement("Query");
            XElement queryOptions = new XElement("QueryOptions");
            XElement viewFields = new XElement("ViewFields");

            BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
            EndpointAddress endpointAddress = new EndpointAddress(webserviceUri);

            basicHttpBinding.MaxBufferSize = 2147483647;
            basicHttpBinding.MaxReceivedMessageSize = 2147483647;

            SpProxy.ListsSoapClient proxy = new SpProxy.ListsSoapClient(basicHttpBinding, endpointAddress);
            proxy.GetListItemsCompleted += new EventHandler<SpProxy.GetListItemsCompletedEventArgs>(proxy_GetListItemsCompleted);
            proxy.GetListItemsAsync(sharepointListGuid, null, query, viewFields, null, queryOptions, null);

            this.RadMap1.InitializeCompleted +=new EventHandler(RadMap1_InitializeCompleted);
        }

        private void LoadInitParams()
        {
            string initParamName = "ListUrl";
            if (App.Current.Host.InitParams.ContainsKey(initParamName))
            {
                this.Mainpage_HyperlinkButton_LinkToData.NavigateUri = new Uri(App.Current.Host.InitParams[initParamName]);
            }
        }

        void proxy_GetListItemsCompleted(object sender, SpProxy.GetListItemsCompletedEventArgs e)
        {
            XNamespace ns = "#RowsetSchema";
            var query = from x in e.Result.Descendants()
                        where x.Name == ns + "row"
                        select new QueryResultItem
                        (
                            x.Attribute("ows_Title") == null ? string.Empty : x.Attribute("ows_Title").Value,
                            x.Attribute("ows_Country_x0020_Name_x0020_Abbrevi") == null ? string.Empty : x.Attribute("ows_Country_x0020_Name_x0020_Abbrevi").Value,
                            x.Attribute("ows_Data_x0020_Field_x0020_1") == null ? string.Empty : x.Attribute("ows_Data_x0020_Field_x0020_1").Value,
                            x.Attribute("ows_Data_x0020_Field_x0020_2") == null ? string.Empty : x.Attribute("ows_Data_x0020_Field_x0020_2").Value,
                            x.Attribute("ows_Data_x0020_Field_x0020_3") == null ? string.Empty : x.Attribute("ows_Data_x0020_Field_x0020_3").Value,
                            x.Attribute("ows_Dashboard_x0020_Link") == null ? string.Empty : x.Attribute("ows_Dashboard_x0020_Link").Value,
                            x.Attribute("ows_Clickable") == null ? string.Empty : x.Attribute("ows_Clickable").Value
                        );

            if (myListset == null)
            {
                myListset = new List<QueryResultItem>();
            }

            foreach (QueryResultItem aItem in query)
            {
                QueryResultItem nItem = new QueryResultItem();
                nItem.Title = aItem.Title;
                nItem.CountryNameAbbreviation = aItem.CountryNameAbbreviation;
                nItem.DataField1 = aItem.DataField1;
                nItem.DataField2 = aItem.DataField2;
                nItem.DataField3 = aItem.DataField3;
                nItem.Clickable = aItem.Clickable;
                nItem.DashboardLink = aItem.DashboardLink;
                myListset.Add(nItem);
            }

            BindTooptipData();
        }

        private void BindTooptipData()
        {
            foreach (MapShape shape in this.items)
                {
                    ExtendedData extendedData = shape.ExtendedData;
                    if (extendedData != null)
                    {
                        string countryName = (string)shape.ExtendedData.GetValue("CNTRY_NAME");
                        var obj = myListset.Select(item => item.Title == countryName);

                        var listItemOfCountry = from x in myListset
                                                where x.Title == countryName
                                                select x;
                        
                        if (listItemOfCountry.Count() == 0)
                        {
                        }
                        else
                        {                            
                            shape.Fill = new SolidColorBrush
                            (
                                Color.FromArgb(255, Convert.ToByte("B2", 16), Convert.ToByte("CC", 16), Convert.ToByte("ED", 16))  // light blue (sea color)
                            );
                            //MapShapeFill msFill = new MapShapeFill();
                            //msFill.
                            //shape.HighlightFill = msFill;
                                
                            //    new SolidColorBrush
                            //(
                            //    Color.FromArgb(255, Convert.ToByte("B2", 16), Convert.ToByte("CC", 16), Convert.ToByte("ED", 16))   //Color.FromArgb(255, Convert.ToByte("7C", 16), Convert.ToByte("94", 16), Convert.ToByte("AE", 16))
                            //);

                            QueryResultItem item = (QueryResultItem)listItemOfCountry.First();
                            
                            // Add tooltip data bind
                            ExtendedPropertySet propertySet = new ExtendedPropertySet();
                            propertySet.RegisterProperty(_data1, _data1, typeof(string), string.Empty);
                            propertySet.RegisterProperty(_data2, _data2, typeof(string), string.Empty);
                            propertySet.RegisterProperty(_data3, _data3, typeof(string), string.Empty);
                            propertySet.RegisterProperty(_country, _country, typeof(string), string.Empty);

                            // Set Data1 Field
                            ExtendedData extData = new ExtendedData(propertySet);
                            extData.SetValue(_data1, item.DataField1);
                            extData.SetValue(_data2, item.DataField2);
                            extData.SetValue(_data3, item.DataField3);
                            // Set Country Field
                            extData.SetValue(_country, countryName);                            
                            shape.ExtendedData = extData;                            

                            ToolTip tooltip = new ToolTip();
                            tooltip.FontFamily = new FontFamily("Arial Regular");
                            tooltip.FontSize = 11.5;

                            String convertParam = string.Format("{{{0}}}", _country);
                            if (item.DataField1 != string.Empty)
                            {
                                convertParam = string.Format("{0}\r\n{{{1}}}", convertParam, _data1);
                            }
                            if (item.DataField2 != string.Empty)
                            {
                                convertParam = string.Format("{0}\r\n{{{1}}}", convertParam, _data2);
                            }
                            if (item.DataField3 != string.Empty)
                            {
                                convertParam = string.Format("{0}\r\n{{{1}}}", convertParam, _data3);
                            }

                            Binding tooltipBinding = new Binding()
                            {
                                Converter = new ExtendedDataConverter(),
                                ConverterParameter = convertParam,
                                Source = shape.ExtendedData
                            }; 
                            tooltip.SetBinding(ToolTip.ContentProperty, tooltipBinding);
                            ToolTipService.SetToolTip(shape, tooltip);
                        }
                    }
            }
        }

        void RadMap1_InitializeCompleted(object sender, EventArgs e)
        {
            this.InformationLayer.Reader.PreviewReadCompleted += new PreviewReadShapesCompletedEventHandler(MapShapeReader_PreviewReadCompleted);
        }

        private void MapShapeReader_PreviewReadCompleted(object sender, Telerik.Windows.Controls.Map.PreviewReadShapesCompletedEventArgs eventArgs)
        {
            if (eventArgs.Error == null)
            {
                this.items = eventArgs.Items;
            }
        }
    }
}
