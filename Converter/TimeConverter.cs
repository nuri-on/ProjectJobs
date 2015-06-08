using ProjectJobs.Util;
using System;
using Windows.UI.Xaml.Data;
using System.Globalization;

namespace ProjectJobs.Converter
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string orginString = value.ToString();
            string resultString;

            switch (parameter as string)
            {
                case "expire":
                    resultString = "마감일: ";

                    if (orginString == "1988118000")
                    {
                        resultString += "채용시";
                    }
                    else
                    {
                        string timeFormat = "yyyy-MMM-d";

                        resultString += DateUtil.getTimeStampToDateTime(Double.Parse(orginString)).ToString(timeFormat);
                    }
                    break;

                default:
                    return value;
            }

            return resultString;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
