#region Using

using System.Collections.ObjectModel;
using System.IO;
using System;

#endregion Using

namespace Gensler_NWC_Automator
{
    #region DataProcessing

    public class DataProcessing //: NWC_ViewModel
    {
        //void Selected(object sender, RoutedEventArgs e)
        //    {
        //        ListView cmd = (ListView)sender;
        //        string selectedItem = (string)(((System.Data.DataRowView)(cmd.SelectedItem)).Row[0]);
        //    }

        #region FileRowProcessing

        public static ObservableCollection<UnitFileRow> FileRowProcessing()
        {
            ObservableCollection<UnitFileRow> _fileList = new ObservableCollection<UnitFileRow>();
            foreach (string fileName in FileHandlingForRevitFiles.GetAllRevitFilesForProcessing())
            {
                bool isSelected = true;
                string name = fileName;
                //string rename = "<Give a new name>";
                string rename = Path.GetFileNameWithoutExtension(fileName);
                DateTime dateRan = DateTime.Now;
                
                //string rename = name.Split(name)
                _fileList.Add(new UnitFileRow(isSelected, name, rename, dateRan));
            }
            return _fileList;
        }

        #endregion FileRowProcessing
    }

    #endregion DataProcessing
}