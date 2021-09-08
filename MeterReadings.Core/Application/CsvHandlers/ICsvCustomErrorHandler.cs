using System;
using CsvHelper.TypeConversion;

namespace MeterReadings.Core.Application.CsvHandlers
{
    public interface ICsvCustomErrorHandler : ITypeConverter
    {
        Exception GetLastError();
        string GetOffendingValue();
    }
}