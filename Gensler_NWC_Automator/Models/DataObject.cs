#region Using

using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

#endregion Using

namespace Gensler_NWC_Automator
{
    #region UnitFileRow

    [Serializable()]
    [System.Xml.Serialization.XmlInclude(typeof(UnitFileRow))]
    public class UnitFileRow : ViewModelBase
    {
        //Private Fields
        private string _unitFileName;
        private string _renameUnitFileName;
        private bool _unitFileSelection;
        private DateTime _dateFileExported;

        //Properties
        [XmlElement("Filename")]
        public string UnitFileName
        {
            get { return _unitFileName; }
            set { _unitFileName = value; OnPropertyChanged("UnitFileName"); }
        }

        [XmlElement("NewFilename")]
        public string RenameUnitFileName
        {
            get { return _renameUnitFileName; }
            set { _renameUnitFileName = value; OnPropertyChanged("RenameUnitFileName"); }
        }

        [XmlElement("FileSelected")]
        public bool UnitFileSelection
        {
            get { return _unitFileSelection; }
            set { _unitFileSelection = value; OnPropertyChanged("UnitFileSelection"); }
        }

        [XmlElement("DateFileExported")]
        public DateTime DateFileExported
        {
            get { return _dateFileExported; }
            set { _dateFileExported = value; OnPropertyChanged("DateFileExported") ;}
        }

        //Implementation of the Class
        public UnitFileRow(bool UFS, string UFN, string RUFN, DateTime DFE)
        {
            this._unitFileSelection = UFS; 
            this._unitFileName = UFN; 
            this._renameUnitFileName = RUFN; 
            this._dateFileExported = DFE;
        }

        public UnitFileRow()
        {
        }
    }

    #endregion UnitFileRow

    #region UnitFileRowList

    [Serializable()]
    public class UnitFileRowList : ViewModelBase
    {
        public UnitFileRowList()
        {
            ListRowItem = new ObservableCollection<UnitFileRow>();
        }

        private ObservableCollection<UnitFileRow> _listRowItem;

        [XmlArray("Filerow")]
        public ObservableCollection<UnitFileRow> ListRowItem
        {
            get { return _listRowItem; }
            set { _listRowItem = value; OnPropertyChanged("ListRowItem"); }
        }

        [XmlElement("LastRunDate")]
        private DateTime _programLastRanOn;
        public DateTime ProgramLastRanOn
        {
            get { return _programLastRanOn; }
            set { _programLastRanOn = value; OnPropertyChanged("ProgramLastRanOn"); }
        }

        [XmlElement("HomeFolderLocation")]
        private string _homeFolderLocation;
        public string HomeFolderLocation
        {
            get { return _homeFolderLocation; }
            set { _homeFolderLocation = value; OnPropertyChanged("HomeFolderLocation"); }
        }

        [XmlElement("NavisworksPath")]
        private string _NavisworksPath;
        public string NavisworksPath
        {
            get { return _NavisworksPath; }
            set { _NavisworksPath = value; OnPropertyChanged("NavisworksPath"); }
        }

    }

    #endregion UnitFileRowList
}