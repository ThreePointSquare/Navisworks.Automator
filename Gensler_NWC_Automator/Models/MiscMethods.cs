using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gensler_NWC_Automator
{
    public class MiscMethods : NWC_ViewModel
    {
        public MiscMethods()
        { }
        
        public void ColorizeList(ListView ListName)
        {
            DateTime checkDate = new DateTime();
            TimeSpan difference;
            checkDate = MyFileNames.ProgramLastRanOn;
            foreach (var element in MyFileNames.ListRowItem)
            {
                difference = checkDate - element.DateFileExported;
                if (difference.Hours < 4)
                {
                    //set the color to green
                }
            }
        }
        
    }   
}