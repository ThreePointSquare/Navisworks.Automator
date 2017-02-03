#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.WindowsAPICodePack.Dialogs;

#endregion Using

namespace Gensler_NWC_Automator
{
    public static class FileHandlingForRevitFiles
    {
        //master folder location for work ground
        public static string MasterFolderLocation { get; set; }

        #region Select RVT files

        //Method to get all Files for processing
        public static List<string> GetAllRevitFilesForProcessing()
        {
            //Array to store Revit Filenames
            List<string> oc_AllRvtFiles = new List<string>();
            //Microsoft method to create open file dialog and filter to get all Revit files.
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".rvt";
            dlg.Filter = "Revit Files (.rvt)|*.rvt";
            dlg.Multiselect = true;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                foreach (string fileName in dlg.FileNames)
                {
                    oc_AllRvtFiles.Add(fileName);
                }
            }
            return oc_AllRvtFiles;
        }

        #endregion Select RVT files

        #region Select Folder Path

        //Get Folder path
        public static string GetFolderPath()
        {
            var dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = true;
            dialog.NavigateToShortcut = true;
            string dirToProcess = @"";

            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                dirToProcess = Directory.Exists(dialog.FileName) ? dialog.FileName : Path.GetDirectoryName(dialog.FileName);
            }
            return dirToProcess;
        }

        #endregion Select Folder Path

        #region Write XML

        //Convert Data into XML
        static public void SerializeToXML(UnitFileRowList XMLdata)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UnitFileRowList));

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.AddExtension = true;
            dlg.Filter = "XML File | *.xml";
            dlg.Title = "Save the XML data for future retrieval.";
            //dlg.CheckFileExists = true;

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string custFilename = dlg.FileName;
                FileStream stream = null;
                stream = new FileStream(custFilename, FileMode.Create, FileAccess.Write);
                serializer.Serialize(stream, XMLdata);
                stream.Close();
                MasterFolderLocation = Path.GetDirectoryName(dlg.FileName);
            }
        }

        #endregion Write XML

        #region Read XML

        //Retrieve Data from XML
        static public UnitFileRowList DeserializeFromXML(UnitFileRowList myOriginalFileList)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(UnitFileRowList));
            UnitFileRowList myFilesFromXML = new UnitFileRowList();

            //Microsoft method to create open file dialog and filter to get all Revit files.
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "XML File | *.xml";
            dlg.Multiselect = false;

            string myFileName = "";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                myFileName = dlg.FileName;
            }
            if (myFileName != "")
            {
                TextReader textReader = new StreamReader(myFileName);

                myFilesFromXML = (UnitFileRowList)deserializer.Deserialize(textReader);
                textReader.Close();

                MasterFolderLocation = Path.GetDirectoryName(dlg.FileName);
                return myFilesFromXML;
            }

            return myOriginalFileList;
        }

        #endregion Read XML

        #region Write TXT

        static public bool WriteTXT(UnitFileRowList _myFileNames, string _filepath)
        {
            //string _filepath = @"C:\Temp\NWC_FileList.txt";
            StreamWriter SaveTextFile = new StreamWriter(_filepath);
            foreach (UnitFileRow element in _myFileNames.ListRowItem)
            {
                if (element.UnitFileSelection == true && File.Exists(element.UnitFileName))
                {
                    SaveTextFile.WriteLine(element.UnitFileName);
                }
            }
            SaveTextFile.Close();
            return true;
        }

        #endregion Write TXT
    }
}