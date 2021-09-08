using CsvHelper;
using CsvHelper.Configuration;
using System;
using CsvHelper.TypeConversion;

namespace MeterReadings.Core.Application.CsvHandlers
{
    public class CsvCustomErrorHandler<T> : ICsvCustomErrorHandler
    {
        private Exception _conversionError;
        private string _offendingValue;

        public Exception GetLastError()
        {
            return _conversionError;
        }

        public string GetOffendingValue()
        {
            return _offendingValue;
        }

        object ITypeConverter.ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            _conversionError = null;
            _offendingValue = null;
            try
            {
                return (T) Convert.ChangeType(text, typeof(T));
            }
            catch (Exception localConversionError)
            {
                _conversionError = localConversionError;
            }

            return default(T);
        }

        string ITypeConverter.ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return Convert.ToString(value);
        }
    }
}
