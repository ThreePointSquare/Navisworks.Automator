   M S B A M L   `   `   `  ����\  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35P<clr-namespace:Microsoft.Win32;assembly=PresentationFrameworkMicrosoft.Win32  5   	   P LGensler_NWC_Automator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:Gensler_NWC_AutomatorGensler_NWC_Automator 5   	   &    Gensler_NWC_Automator.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      5   	   NWin32<clr-namespace:Microsoft.Win32;assembly=PresentationFramework      5   	   @d2http://schemas.microsoft.com/expression/blend/2008      5   	   5local#clr-namespace:Gensler_NWC_Automator      5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006      5   	     � Title$  Gensler NWC Automator��5   	   $
��1220��5	   	   $	��750��5
   	   $	��725��5   	   $	��750��5   	   $
��1980��5   	   $	��750��5   	   $��Segoe UI Light)�5   	   $��14(�5   	    � Topmost .  � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5      )  #Gensler_NWC_Automator.NWC_ViewModel   NWC_ViewModelDataSource��%   &	          5   
   6   5      .��5      �  � DataContext 6   ��  �� Source# [   � RowDefinitions 5   
   �� 5      �� 5      $	��461 �6   �� 5      �� 5      $	��186 �6   �� 5      $��13 �6    � ColumnDefinitions 5   
   �� 5       $	��133 �6   �� 5!      .+�5#   
   �� $��1��6   $��4��5$      $��10,10,30,10q�5%      $$��Resources/NWCAutomatorLogo.png��5&      $
��Fill=�5'      �� 5(   
   $��1��6   $��1��5)      $��	0,0,10,10q�5*      $��Files to Process��5+      .��5,      � .+�5-      �� -   5         ��Name$ btn_LoadFileList��5-      $��88��5.      $��32��5/      $��	29,10,0,0q�50      $
��Left=�51      $	��Top=�52      $
��Load��55      ��53      �� *Command_LoadXMLFileList+	 �� Mode$	 OneWay=�55      �� 56      -   5        $ btn_SaveFileList��56      $��88��57      $��32��58      $��
134,10,0,0q�59      $
��Left=�5:      $	��Top=�5;      $
��Save��5>      ��5<      �� *Command_SaveXMLFileList+$	 OneWay=�5>      �� 5?      -   5        $ btn_AddFileList��5?      $��88��5@      $��32��5A      $��
237,10,0,0q�5B      $
��Left=�5C      $	��Top=�5D      $��	Add Files��5G      ��5E      �� *Command_AddFiles+$	 OneWay=�5G      �� 5I      -   5        
 ��Name$
 LV_FileNames��5I      $��0,62,0,0q�5J      ��5K      ��  �� Path$ MyFileNames.ListRowItem� �� SelectedIndex 5L      �� *TheSelectedIndex+��5M      �� 5O       �� 
TargetType" ��6    .�5P      ��  �� Property � �� �� Value$ Stretch=�6K   �� 5Q       �@�ContextMenu 5R   "   �� 5S   &    62   �� # [  .@�5T   *   m� $��Remove��6h    m� Command 63   �� *Command_RemoveItems+$	 OneWay=�6h   5U   '   5W      �� 5X       � ��$ Stretch=�6K   �� 5Y       � ��
 ���� 6;   �� 5Z       � ��$ Stretch=�6K   �� 5[       � �� .  6:   �� 5\       �@��
IsSelected 6;   �� *SelectedItems+5]       �� View 5`      �� 5b      -   5        GV_FileNames�� �� 
IsSelected .  .P�5c      ��  �� Width$ 32��6-   $�� ��68    �� CellTemplate 5d   "   �� 5e   &    �� DataType'  !Gensler_NWC_Automator.UnitFileRow"  63   .R�5f   *   �� ��.   d� 	IsChecked 63   �� $ UnitFileSelection�$	 TwoWay=�6m   5g   '   5i      �� 5j      $
 Auto��6-   $��Revit Filenames��6:    5k   "   �� 5l   &   "  63   .R�5m   *   ��  �� TextAlignment$
 Left=�6W   ��64   �� $ UnitFileName�6W   5n   '   5p      �� 5q      $	 350��6-   $��New Filename��69    5r   "   �� 5s   &   "  63   .R�5t   *   �� $��tbox_inputTextBox��62    �� AutoWordSelection . 
�������|�.  �� IsInputMethodEnabled . ��.  �� TextAlignment$
 Left=�5}   2   ��5z   2   �� $ RenameUnitFileName�$	 TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5}   2   5~   '   5�      �� 5�      $	 350��6-   $��Last Modified��69    5�   "   �� 5�   &   "  63   .R�5�   *   �� $
 Left=�6[   ��64   �� $ DateFileExported�6[   5�   '   5�      5�      5�      �� 5�      -   5        $ btn_CleanupList��5�      $��88��5�      $��32��5�      $��
404,10,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Cleanup List��5�      ��5�      �� *Command_CleanupList+$	 OneWay=�$ PropertyChanged=�5�      �� 5�      $��800,25,10,368q�6   $��Segoe WP Black)�6e   $��18(�6�   $��Right=�6�   ��6/   �� $" MyFileNames.ProgramLastRanOn�6�   �� 5�      $��800,0,10,395q�6   $��Program Last ran on: ��6.   $��Right=�6N   5�      5�      �� 5�   
   -   5         ��Name$ lblSelected��5�      $��1��5�      $��2��5�      $��1��5�      $
��1069��5�      $��33��5�      $��	0,454,0,0q�5�      $
��Left=�5�      $	��Top=�5�      ��5�      �� *TheSelectedIndex+�� 5�   
   $��3��6   $��2��5�      $��1��5�      $��	0,1,10,10q�5�      $��Process NWC��5�      .��5�      � .+�5�      � .+�5�      �� -   5        $ btn_LoadList��5�      $	��213��5�      $��88��5�      $��	0,62,10,0q�5�      $��Right=�5�      $	��Top=�5�      $��
Create NWC��5�      ��5�      �� *Command_CreateNWC+$	 OneWay=�$ PropertyChanged=�5�      �� 5�      -	   5        $ btn_BrowseFolder��5�      $��40��5�      $��40��5�      $��	0,10,10,0q�5�      $��Right=�5�      $	��Top=�5�      $��. . .��5�      ��5�      �� *Command_LocateFolder+$	 OneWay=�5�      �� 5�      $	��253��6   $��22��5�      $��	10,62,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $(��"Simplify NWC Filenames by Renaming��5�       5�      �� *IsSelectedRenameFiles+$	 TwoWay=�{�5�      �� $ FolderLocation.Length��� 5�      -
   5        $ Label_FolderLocation��5�      $��10,10,903,112q�5�      $��Work Folder Location :��5�      �� 5�      -   5         ��Name$ FileNameTextBox��5�      $��40��5�      $��159,10,60,112q�5�      
�������
������� �� MaxLines$ 1��5�       � ToolTip$8 2NWC Files will be moved into this Folder location.��5�      ��5�      �� *FolderLocation+$ PropertyChanged=�5�      �� 5�      $��3��6   $��0��5�      $��108,125,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Label��5�      �� 5�      $	��253��6   $��22��5�      $��	10,89,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $#��Move NWC files to work folder��5�       5�      �� *IsSelectedMoveFiles+$	 TwoWay=�{�5�      �� *FolderLocation.Length+5�      �� 5�        K� ZIndex  6   �� *PanelZIndex+.+�5�      9� ! �� Left$! 6��6   
" �� Top$" 90��5�      $	��800��5�      $��68��5�      # 9� ExpandDirection$# Up=�5�      $1��+Folder where Navisworks Manage is installed��5�      $ 9� 
IsExpanded$ 5�      �� *IsExpanderOpen+$	 TwoWay=�$ PropertyChanged=���5�      � 5�      $��84��6#   ��5�   "   �� 5�   &   % �� Opacity$% 1Q�66   & �� Color$& 	#FFE0E0E0��6B   .+�5�   "   � .+�5�   &   �� $��40��6-   $��40��5�   -   $��0,2,1,42q�5�   -   $��Right=�5�   -   $��. . .��5�   -   ��5�   -   �� *Command_NWC_ProgramPath+$	 OneWay=�5�   -   5�   #   � 5�   "   .+�5�   &   �� $��40��6.   $��
22,2,51,42q�5�   .   
�������
���������5  .   �� *NWC_ProgramPath+$	 TwoWay=�$ PropertyChanged=�5  #   5     5     5     5	     5
     5     5     