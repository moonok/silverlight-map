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
    public partial class MainPage : UserControl
    {
        private const string _data1 = "Data Field 1";
        private const string _data2 = "Data Field 2";
        private const string _data3 = "Data Field 3";
        private const string _data4 = "Data Field 4";
        private const string _data5 = "Data Field 5";
        private const string _country = "Country";
        private List<SharepointListItem> myListset = new List<SharepointListItem>();
        private List<FrameworkElement> items;
        private string _webserviceUri = string.Empty;        // @"http://brown-moss/MoonSilverlight/_vti_bin/lists.asmx";
        private string _sharepointListGuid = string.Empty;   // @"B7BF0524-9756-4958-A110-E60E533EDEC1";

        public MainPage()
        {
            InitializeComponent();

            LoadInitParams();
            CountryCoordinate.LoadCountryCoordinate();
            LoadSharepointListData();
        }

        private void LoadSharepointListData()
        {
            XElement query = new XElement("Query");
            XElement queryOptions = new XElement("QueryOptions");
            XElement viewFields = new XElement("ViewFields");

            BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
            EndpointAddress endpointAddress = new EndpointAddress(_webserviceUri);

            basicHttpBinding.MaxBufferSize = 2147483647;
            basicHttpBinding.MaxReceivedMessageSize = 2147483647;

            SpProxy.ListsSoapClient proxy = new SpProxy.ListsSoapClient(basicHttpBinding, endpointAddress);
            proxy.GetListItemsCompleted += new EventHandler<SpProxy.GetListItemsCompletedEventArgs>(proxy_GetListItemsCompleted);
            proxy.GetListItemsAsync(_sharepointListGuid, null, query, viewFields, null, queryOptions, null);

            this.RadMap1.InitializeCompleted += new EventHandler(RadMap1_InitializeCompleted);
        }

        private void LoadInitParams()
        {
            //string initParam1 = "LinkToDataUrl";
            //string initParam2 = "ListGuid";
            //string initParam3 = "SiteUrl";
            //if (App.Current.Host.InitParams.ContainsKey(initParam1))
            //{
            //    this.Mainpage_HyperlinkButton_LinkToData.NavigateUri = new Uri(App.Current.Host.InitParams[initParam1]);
            //}
            //if (App.Current.Host.InitParams.ContainsKey(initParam2))
            //{
            //    this._sharepointListGuid = App.Current.Host.InitParams[initParam2];
            //}
            //if (App.Current.Host.InitParams.ContainsKey(initParam3))
            //{
            //    this._webserviceUri = string.Format("{0}_vti_bin/lists.asmx", App.Current.Host.InitParams[initParam3]);
            //}

            this.Mainpage_HyperlinkButton_LinkToData.NavigateUri = new Uri("http://edweb/iebmrp/DocCenter/Lists/MapList/AllItems.aspx");
            this._sharepointListGuid = @"3DB1C790-08E9-4615-AFBA-9C2D36E12CF3";
            this._webserviceUri = @"http://edweb/iebmrp/DocCenter/_vti_bin/lists.asmx";
        }

        void proxy_GetListItemsCompleted(object sender, SpProxy.GetListItemsCompletedEventArgs e)
        {
            XNamespace ns = "#RowsetSchema";
            var query = from x in e.Result.Descendants()
                        where x.Name == ns + "row"
                        select new SharepointListItem
                        (
                            x.Attribute("ows_Title") == null ? string.Empty : x.Attribute("ows_Title").Value,
                            x.Attribute("ows_Country_x0020_Name_x0020_Abbrevi") == null ? string.Empty : x.Attribute("ows_Country_x0020_Name_x0020_Abbrevi").Value,
                            x.Attribute("ows_Data_x0020_Field_x0020_1") == null ? string.Empty : x.Attribute("ows_Data_x0020_Field_x0020_1").Value,
                            x.Attribute("ows_Data_x0020_Field_x0020_2") == null ? string.Empty : x.Attribute("ows_Data_x0020_Field_x0020_2").Value,
                            x.Attribute("ows_Data_x0020_Field_x0020_3") == null ? string.Empty : x.Attribute("ows_Data_x0020_Field_x0020_3").Value,
                            x.Attribute("ows_Data_x0020_Field_x0020_4") == null ? string.Empty : x.Attribute("ows_Data_x0020_Field_x0020_4").Value,
                            x.Attribute("ows_Data_x0020_Field_x0020_5") == null ? string.Empty : x.Attribute("ows_Data_x0020_Field_x0020_5").Value,
                            x.Attribute("ows_Dashboard_x0020_Link") == null ? string.Empty : x.Attribute("ows_Dashboard_x0020_Link").Value,
                            x.Attribute("ows_Clickable") == null ? string.Empty : x.Attribute("ows_Clickable").Value
                        );

            if (myListset == null)
            {
                myListset = new List<SharepointListItem>();
            }

            foreach (SharepointListItem aItem in query)
            {
                SharepointListItem nItem = new SharepointListItem();
                nItem.Title = aItem.Title;
                nItem.CountryNameAbbreviation = aItem.CountryNameAbbreviation;
                nItem.DataField1 = aItem.DataField1;
                nItem.DataField2 = aItem.DataField2;
                nItem.DataField3 = aItem.DataField3;
                nItem.DataField4 = aItem.DataField4;
                nItem.DataField5 = aItem.DataField5;
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
                    string countrynameabbrev = (string)shape.ExtendedData.GetValue("ISO_2DIGIT");
                        
                    var listItems = from x in myListset
                                            where x.Title == countryName
                                            select x;
                        
                    // Add country name abbreviation
                    HyperlinkButton hbutton = new HyperlinkButton();                    
                    hbutton.Foreground = new SolidColorBrush(Colors.Black);
                    hbutton.Content = countrynameabbrev;
                    hbutton.FontSize = 10;
                    hbutton.IsEnabled = false;
                    var obj = from x in CountryCoordinate.countryList
                                where x.CountryName == countrynameabbrev
                                select x;
                    if (obj.Count() != 0)
                    {
                        Country country = (Country)obj.First();
                        hbutton.SetValue(MapLayer.LocationProperty, new Location(country.Latitude, country.Longitude)); 
                        this.InformationLayer.Items.Add(hbutton);
                    }

                    if (listItems.Count() == 0)
                    {
                    }
                    else
                    {
                        shape.Fill = new SolidColorBrush
                        (
                            Color.FromArgb(255, Convert.ToByte("B2", 16), Convert.ToByte("CC", 16), Convert.ToByte("ED", 16))  // light blue (sea color)
                        );
                        shape.HighlightFill.Fill = new SolidColorBrush
                        (
                            Color.FromArgb(255, Convert.ToByte("B2", 16), Convert.ToByte("CC", 16), Convert.ToByte("ED", 16))   //Color.FromArgb(255, Convert.ToByte("7C", 16), Convert.ToByte("94", 16), Convert.ToByte("AE", 16))
                        );

                        SharepointListItem item = (SharepointListItem)listItems.First();

                        // Add clickable item a styling and an uri
                        if(item.Clickable != "0")
                        {
                            shape.HighlightFill.Fill = new SolidColorBrush
                            (
                                Color.FromArgb(255, Convert.ToByte("8F", 16), Convert.ToByte("A3", 16), Convert.ToByte("BC", 16))   //Color.FromArgb(255, Convert.ToByte("7C", 16), Convert.ToByte("94", 16), Convert.ToByte("AE", 16))
                            );
                            hbutton.IsEnabled = true;
                            hbutton.NavigateUri = new Uri(item.DashboardLink);
                        }
                            
                        // Add tooltip data bind
                        ExtendedPropertySet propertySet = new ExtendedPropertySet();
                        propertySet.RegisterProperty(_data1, _data1, typeof(string), string.Empty);
                        propertySet.RegisterProperty(_data2, _data2, typeof(string), string.Empty);
                        propertySet.RegisterProperty(_data3, _data3, typeof(string), string.Empty);
                        propertySet.RegisterProperty(_data4, _data4, typeof(string), string.Empty);
                        propertySet.RegisterProperty(_data5, _data5, typeof(string), string.Empty);
                        propertySet.RegisterProperty(_country, _country, typeof(string), string.Empty);

                        // Set Data Fields
                        ExtendedData extData = new ExtendedData(propertySet);
                        extData.SetValue(_data1, item.DataField1);
                        extData.SetValue(_data2, item.DataField2);
                        extData.SetValue(_data3, item.DataField3);
                        extData.SetValue(_data4, item.DataField4);
                        extData.SetValue(_data5, item.DataField5);
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
                        if (item.DataField4 != string.Empty)
                        {
                            convertParam = string.Format("{0}\r\n{{{1}}}", convertParam, _data4);
                        }
                        if (item.DataField5 != string.Empty)
                        {
                            convertParam = string.Format("{0}\r\n{{{1}}}", convertParam, _data5);
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
