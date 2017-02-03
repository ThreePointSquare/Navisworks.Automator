using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;


namespace Gensler_NWC_Automator
{
    public class NWC_ViewModel : ViewModelBase
    {
        public NWC_ViewModel()
        {
        }

        #region Methods

        public void SetNormalUIDisplay()
        {
        }

        //create an instance of the process so we can track it.
        private Process process_NWC = new Process();

        //Method to start a process and pass commandline to the process for tracking
        private bool NWCProcess()
        {
            //Check if the program is installed
            string NWCTaskRunner = @"\FiletoolsTaskRunner.exe";
            string NWCTaskRunnerProgram = NWC_ProgramPath + NWCTaskRunner;

            //string NWCFileListText = @"C:\Temp\NWC_FileList.txt";
            //Build the arguments including the double quotes
            string NWC_quote = "\"";
            string NWCFileListText = FolderLocation + "\\NWC_Automator.txt"; //This didn't needed the double quotes
            string NWC_list = NWC_quote + NWCFileListText + NWC_quote;
            string NWC_NWF = NWC_quote + FolderLocation + "\\NWC_Automator.nwf" + NWC_quote;
            string NWC_log = NWC_quote + FolderLocation + "\\NWC_Automator.Log" + NWC_quote;
            string NWC_lang = " /lang en-US";
            //string NWC_arguments = @" /i ""C:\Temp\NWC_FileList.txt"" /of ""C:\Temp\conference.nwf"" /log ""C:\Temp\events.log"" /lang fr-fr";
            string NWC_arguments = " /i " + NWC_list + " /of " + NWC_NWF + " /log " + NWC_log + NWC_lang;


            //Process the TaskRunner
            try
            {
                //Write the text file before passing it to the arguments
                if (FileHandlingForRevitFiles.WriteTXT(MyFileNames, NWCFileListText))
                {
                    if (File.Exists(NWCTaskRunnerProgram) == true)
                    {
                        process_NWC.StartInfo.UseShellExecute = false;
                        process_NWC.StartInfo.RedirectStandardOutput = true;
                        process_NWC.StartInfo.RedirectStandardError = true;
                        process_NWC.StartInfo.FileName = NWCTaskRunnerProgram;
                        process_NWC.StartInfo.Arguments = NWC_arguments;
                        process_NWC.EnableRaisingEvents = true;
                        process_NWC.Exited += new EventHandler(process_Exited);

                        process_NWC.Start();
                        StreamReader reader = process_NWC.StandardOutput;
                        //StreamReader errors = process_NWC.StandardError;
                        //string output = reader.ReadToEnd();
                        //string error = errors.ReadToEnd();
                        //Console.WriteLine(output);
                        //MessageBox.Show(error);
                        Application.Current.MainWindow.WindowState = WindowState.Minimized;
                        process_NWC.WaitForExit();
                        process_NWC.Close();
                    }
                    else
                    {
                        MessageBox.Show("The program doesn't exist. Please verify if Navisworks Manage 2015 is installed.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured \"{0}\" :" + "\n" + ex.Message, NWCTaskRunnerProgram);
                return false;
            }

            return true;
        }


        //Message Box after the process exited
        public void process_Exited(object sender, EventArgs e)
        {
            if (process_NWC.ExitCode == 0)
            {
                if (File.Exists(Path.Combine(FileHandlingForRevitFiles.MasterFolderLocation, "NWC_Automator.nwf")))
                {
                    File.Delete(Path.Combine(FileHandlingForRevitFiles.MasterFolderLocation, "NWC_Automator.nwf"));
                }

                bool result = this.MoveRenameFiles();

                if (result)
                {
                    // Add date time stamp
                    DateTime now = DateTime.Now;
                    MyFileNames.ProgramLastRanOn = now;


                    MessageBox.Show("The Automation process completed!", "NWC Automation Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("The Automation process did not completed! The NWC files may have been created but were not moved or renamed.", "NWC Automation Result", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("The Automation process is interupted!", "NWC Automation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (Application.Current.MainWindow.WindowState != WindowState.Normal)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }


        //Check if the Collection has duplicates in the Rename filename.
        private bool CheckIfRenameColumnHasDuplicates()
        {
            bool hasDuplicates = false;

            var set = new HashSet<string>();

            foreach (UnitFileRow element in MyFileNames.ListRowItem)
            {
                if (!set.Add(element.RenameUnitFileName.Trim()))
                {
                    hasDuplicates = true;
                    break;
                }
            }
            return hasDuplicates;
        }


        //Move or Rename the files as needed based on choices made
        private bool MoveRenameFiles()
        {
            foreach (UnitFileRow element in MyFileNames.ListRowItem)
            {
                if (element.UnitFileSelection == true)
                {
                    string myFileName = Path.GetDirectoryName(element.UnitFileName) + "\\" + Path.GetFileNameWithoutExtension(element.UnitFileName) + ".nwc";
                    string newFileName = Path.GetDirectoryName(element.UnitFileName) + "\\" + Path.GetFileNameWithoutExtension(element.UnitFileName) + ".nwc";
                    string cacheFolder = FolderLocation + "\\_NWC Cache";
                    FileInfo myFileNameInfo = new FileInfo(myFileName);
                    DirectoryInfo cacheFolderInfo = new DirectoryInfo(cacheFolder);

                    //Update the date on the files
                    DateTime now = DateTime.Now;
                    element.DateFileExported = now;

                    if (IsSelectedMoveFiles)
                    {
                        if (!cacheFolderInfo.Exists)//(!Directory.Exists(cacheFolder))
                        {
                            Directory.CreateDirectory(cacheFolder);
                        }

                        newFileName = cacheFolder + "\\" + Path.GetFileNameWithoutExtension(element.UnitFileName) + ".nwc";

                        if (IsSelectedRenameFiles)
                        {
                            newFileName = cacheFolder + "\\" + element.RenameUnitFileName + ".nwc";
                        }

                        if (myFileNameInfo.Exists)//(File.Exists(myFileName))
                        {
                            File.Copy(myFileName, newFileName, true);
                            File.Delete(myFileName);
                        }
                        //return true;
                    }

                    if (IsSelectedMoveFiles == false && IsSelectedRenameFiles == true)
                    {
                        if (myFileNameInfo.Exists)
                        {
                            newFileName = Path.GetDirectoryName(element.UnitFileName) + "\\" + element.RenameUnitFileName + ".nwc";
                            File.Copy(myFileName, newFileName, true);
                            File.Delete(myFileName);
                            //return true;
                        }
                    }
                }
            }
            return true;
        }


        #endregion Methods

        #region Properties / Fields

        //Properties for tracking different items

        private UnitFileRowList _myFileNames = new UnitFileRowList();
        private string _folderLocation = "";
        private int _theSelectedIndex = -1;
        private bool _IsSelectedRenameFiles = true;
        private bool _IsSelectedMoveFiles = true;
        private bool _IsExpanderOpen = false;
        private int _PanelZIndex = -1;
        private object _SelectedItems; //currently not used in the program
        private bool _programDidRan = false; //tracker to check if changes were made by the program

        private bool _initializerForPanelZIndex = true; //this is to intialize the panels to right ZIndex
        private string _nwc_ProgramPath = @"C:\Program Files\Autodesk\Navisworks Manage 2015";




        #region MyFileNames Property

        public UnitFileRowList MyFileNames
        {
            get { return _myFileNames; }
            set
            {
                if (_myFileNames != value)
                {
                    _myFileNames = value;
                    OnPropertyChanged("MyFileNames");
                }
            }
        }

        #endregion MyFileNames Property

        #region FolderLocation Property

        public string FolderLocation
        {
            get { return _folderLocation; }
            set
            {
                if (_folderLocation != value)
                {
                    _folderLocation = value;

                    OnPropertyChanged("FolderLocation");
                }
            }
        }

        #endregion FolderLocation Property

        #region SelectedIndex Property

        public int TheSelectedIndex
        {
            get { return _theSelectedIndex; }
            set
            {
                _theSelectedIndex = value;
                OnPropertyChanged("TheSelectedIndex");
            }
        }

        #endregion SelectedIndex Property

        #region Selected Items in the List Property

        public object SelectedItems
        {
            get { return _SelectedItems; }
            set
            {
                _SelectedItems = value;
                OnPropertyChanged("SelectedItems");
            }
        }

        #endregion Selected Items in the List Property

        #region Is Selected RenameFiles Property

        public bool IsSelectedRenameFiles
        {
            get
            {
                return _IsSelectedRenameFiles;
            }
            set
            {
                _IsSelectedRenameFiles = value;
                OnPropertyChanged("IsSelectedRenameFiles");
            }
        }

        #endregion Is Selected RenameFiles Property

        #region Is Selected MoveFiles Property

        public bool IsSelectedMoveFiles
        {
            get
            {
                return _IsSelectedMoveFiles;
                //if (FolderLocation == null)
                //{
                //    return false;
                //}
                //return !string.IsNullOrWhiteSpace(FolderLocation);
            }
            set
            {
                _IsSelectedMoveFiles = value;
                OnPropertyChanged("IsSelectedMoveFiles");
            }
        }

        #endregion Is Selected MoveFiles Property

        #region Is Expander Open Property

        public bool IsExpanderOpen
        {
            get
            {
                return _IsExpanderOpen;
            }
            set
            {
                PanelZIndex = -1;
                _IsExpanderOpen = value;
                OnPropertyChanged("IsExpanderOpen");
            }
        }

        #endregion Is Expander Open Property

        #region Panel ZIndex Property

        public int PanelZIndex
        {
            get
            {
                if (IsExpanderOpen == true || _initializerForPanelZIndex == true)
                {
                    _initializerForPanelZIndex = false;
                    return _PanelZIndex = -1;
                }
                else
                {
                    return _PanelZIndex = 1;
                }
                //return _PanelZIndex;
            }
            set
            {
                _PanelZIndex = value;
                OnPropertyChanged("PanelZIndex");
            }
        }

        #endregion Panel ZIndex Property

        #region NWC_ProgramPath Property

        public string NWC_ProgramPath
        {
            get { return _nwc_ProgramPath; }
            set
            {
                if (_nwc_ProgramPath != value)
                {
                    _nwc_ProgramPath = value;

                    OnPropertyChanged("NWC_ProgramPath");
                }
            }
        }

        #endregion NWC_ProgramPath Property

        #region ProgramDidRan Property

        public bool ProgramDidRan
        {
            get { return _programDidRan; }
            set
            {
                if (_programDidRan != value)
                {
                    _programDidRan = value;

                    OnPropertyChanged("ProgramDidRan");
                }
            }
        }

        #endregion ProgramDidRan Property



        #endregion Properties / Fields

        #region Button Commands

        #region Command AddFiles

        private RelayCommand _command_AddFiles;

        public RelayCommand Command_AddFiles
        {
            get
            {
                _command_AddFiles = new RelayCommand(
                    param => this._AddFiles(), param => this._CanAddFiles());

                return _command_AddFiles;
            }
        }

        private bool _CanAddFiles()
        {
            return true;
        }

        //Actual Command that should run
        private void _AddFiles()
        {
            if (MyFileNames.ListRowItem.Count == 0)
            {
                ObservableCollection<UnitFileRow> _newItems = new ObservableCollection<UnitFileRow>();

                _newItems = DataProcessing.FileRowProcessing();
                foreach (UnitFileRow element in _newItems)
                {
                    MyFileNames.ListRowItem.Add(element);
                }
            }
            else
            {
                ObservableCollection<UnitFileRow> _newItems = new ObservableCollection<UnitFileRow>();
                _newItems = DataProcessing.FileRowProcessing();

                foreach (UnitFileRow element in _newItems)
                {
                    if (MyFileNames.ListRowItem.Any(item => item.UnitFileName == element.UnitFileName)) { }
                    else { MyFileNames.ListRowItem.Add(element); }
                }
            }
        }

        #endregion Command AddFiles

        #region Command Remove Item from List

        private RelayCommand _command_RemoveItems;

        public RelayCommand Command_RemoveItems
        {
            get
            {
                _command_RemoveItems = new RelayCommand(
                    param => this._RemoveItems(), param => this._CanRemoveItems());

                return _command_RemoveItems;
            }
        }

        private bool _CanRemoveItems()
        {
            return true;
        }

        //Actual Command that should run
        private void _RemoveItems()
        {
            //check if the List has items and if yes remove it at the index
            if (MyFileNames.ListRowItem.Count != 0)
            {
                MyFileNames.ListRowItem.RemoveAt(TheSelectedIndex);
            }
            ProgramDidRan = true;
        }

        #endregion Command Remove Item from List

        #region Command LoadXMLFileList

        private RelayCommand _command_LoadXMLFileList;

        public RelayCommand Command_LoadXMLFileList
        {
            get
            {
                _command_LoadXMLFileList = new RelayCommand(
                    param => this._LoadXMLFileList(), param => this._CanLoadXMLFileList());
                return _command_LoadXMLFileList;
            }
        }

        private bool _CanLoadXMLFileList()
        {
            return true;
        }

        //Actual Command that should run
        private void _LoadXMLFileList()
        {
            // MyFileNames = DataProcessing.FileRowProcessing();
            MyFileNames = FileHandlingForRevitFiles.DeserializeFromXML(MyFileNames);
            FolderLocation = FileHandlingForRevitFiles.MasterFolderLocation;
            NWC_ProgramPath = MyFileNames.NavisworksPath;
            List<string> missingFiles = new List<string>();
            //check if the loaded files exists in the location
            foreach (UnitFileRow element in MyFileNames.ListRowItem)
            {
                if (!File.Exists(element.UnitFileName))
                {
                    missingFiles.Add(element.UnitFileName);
                }
            }
            if (missingFiles.Count > 0)
            {
                MessageBox.Show("The following files doesn't exists! \n" + String.Join("\n", missingFiles) + "\nUse Cleanup button to remove such files from the list.", "Missing File", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion Command LoadXMLFileList

        #region Command SaveXMLFileList

        private RelayCommand _command_SaveXMLFileList;

        public RelayCommand Command_SaveXMLFileList
        {
            get
            {
                _command_SaveXMLFileList = new RelayCommand(
                    param => this._SaveXMLFileList(), param => this._CanSaveXMLFileList());

                return _command_SaveXMLFileList;
            }
        }

        private bool _CanSaveXMLFileList()
        {
            if (MyFileNames.ListRowItem.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Actual Command that should run
        private void _SaveXMLFileList()
        {
            if (MyFileNames.ListRowItem.Count != 0)
            {
                MyFileNames.NavisworksPath = NWC_ProgramPath;
                //string filepath = @"myFileList.xml";
                FileHandlingForRevitFiles.SerializeToXML(MyFileNames);
                FolderLocation = FileHandlingForRevitFiles.MasterFolderLocation;
            }
        }

        #endregion Command SaveXMLFileList

        #region Command Locate Folder

        private RelayCommand _command_LocateFolder;

        public RelayCommand Command_LocateFolder
        {
            get
            {
                _command_LocateFolder = new RelayCommand(
                    param => this._LocateFolder(), param => this._CanLocateFolder());

                return _command_LocateFolder;
            }
        }

        private bool _CanLocateFolder()
        {
            return true;
        }

        //Actual Command that should run
        private void _LocateFolder()
        {
            FolderLocation = FileHandlingForRevitFiles.GetFolderPath();
        }

        #endregion Command Locate Folder

        #region Command Locate Program Folder

        private RelayCommand _command_NWC_ProgramPath;

        public RelayCommand Command_NWC_ProgramPath
        {
            get
            {
                _command_NWC_ProgramPath = new RelayCommand(
                    param => this._NWC_ProgramPath(), param => this._CanNWC_ProgramPath());

                return _command_NWC_ProgramPath;
            }
        }

        private bool _CanNWC_ProgramPath()
        {
            return true;
        }

        //Actual Command that should run
        private void _NWC_ProgramPath()
        {
            string _tempPath = FileHandlingForRevitFiles.GetFolderPath();

            if (_tempPath != "")
            {
                NWC_ProgramPath = _tempPath;
            }
        }

        #endregion Command Locate Program Folder

        #region Command Cleanup Files List

        private RelayCommand _command_CleanupList;

        public RelayCommand Command_CleanupList
        {
            get
            {
                _command_CleanupList = new RelayCommand(
                    param => this._CleanupList(), param => this._CanCleanupList());

                return _command_CleanupList;
            }
        }

        private bool _CanCleanupList()
        {
            if (MyFileNames.ListRowItem.Count != 0)
            {
                List<int> missingFilesIndexes = new List<int>();
                int itemIndex = 0;
                //check if the loaded files exists in the location
                foreach (UnitFileRow element in MyFileNames.ListRowItem)
                {
                    if (!File.Exists(element.UnitFileName))
                    {
                        missingFilesIndexes.Add(itemIndex);
                    }
                    itemIndex++;
                }
                if (missingFilesIndexes.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //Actual Command that should run
        private void _CleanupList()
        {
            //Command algorithm to move files after the process is complete.
            List<int> missingFilesIndexes = new List<int>();
            int itemIndex = 0;
            //check if the loaded files exists in the location
            foreach (UnitFileRow element in MyFileNames.ListRowItem)
            {
                if (!File.Exists(element.UnitFileName))
                {
                    missingFilesIndexes.Add(itemIndex);
                }
                itemIndex++;
            }
            if (missingFilesIndexes.Count > 0)
            {
                for (int i = missingFilesIndexes.Count - 1; i >= 0; i--)
                {
                    MyFileNames.ListRowItem.RemoveAt(missingFilesIndexes[i]);
                }
            }
        }

        #endregion Command Cleanup Files List

        #region Command CreateNWC

        private RelayCommand _command_CreateNWC;

        public RelayCommand Command_CreateNWC
        {
            get
            {
                _command_CreateNWC = new RelayCommand(
                    param => this._CreateNWC(), param => this._CanCreateNWC());

                return _command_CreateNWC;
            }
        }

        private bool _CanCreateNWC()
        {
            if (MyFileNames.ListRowItem.Count != 0 && Directory.Exists(FolderLocation) && _CanCleanupList() == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Actual Command that should run
        private void _CreateNWC()
        {
            if (this.CheckIfRenameColumnHasDuplicates() && IsSelectedRenameFiles == true)
            {
                MessageBox.Show("The New Filename values are not unique! Avoid duplicates!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                //Call the command for NWC commandline input
                //Create a text file to feed into NWC commandline
                //Check for the processes to see if it began
                //Check if the processes completed it task



                //http://help.autodesk.com/view/NAV/2015/ENU/?guid=GUID-974E2165-7403-4025-B0D0-F7EBC46AC592

                
                this.NWCProcess();
                //bool Result = this.MoveRenameFiles();

                //If process succeeds then do the following
                //Start moving files
                //Rename filenames if the rename option is selected.
            }
        }

        #endregion Command CreateNWC

        #endregion Button Commands
    }
}