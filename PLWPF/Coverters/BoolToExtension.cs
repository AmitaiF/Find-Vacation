using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BE;

namespace PLWPF
{
    public class BoolToExtension : IValueConverter
    {
        public object Convert(object value,Type targetType,object parameter,CultureInfo culture)
        {

            // Since they are connected Two way mode in the begining the got they get the deafult value of Unintrested so the got all false(not 'v' in checkbox)
            //but when we put manually 'v' in the checkbox he set the value in the class (using the convertback function) But he set in the ui (target) the 'v' so he enter here 
            //with the value of what he set that's mean "Possible" so that he will put 'v' in check box can not use this func and the deafult value but will not auto maticlly reset after press on add button
            if ((Extension)value == Extension.Possible)
                return true;
            return false;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((bool)value)
            {
                return Extension.Possible;
            }
            else
            {
                return Extension.Unintrested;
            }
            
        }
    }
}










































































