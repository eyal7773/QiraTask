using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Maps
{
    public sealed class InvoiceMap : ClassMap<Invoice>
    {
        public InvoiceMap()
        {
            Map(m => m.CreatedAt).TypeConverter<DateTimeConverter>();
            Map(m => m.Id);
            Map(m => m.Status).TypeConverter<GEnumConverter<InvoiceStatuses>>();
            Map(m => m.Amount);
            Map(m => m.PaymentMethod).TypeConverter<GEnumConverter<PaymentMethods>>();
            Map(m => m.UpdatedAt).TypeConverter<DateTimeConverter>().Optional();
        }
    }
    
    public class DateTimeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (!DateTimeOffset.TryParse(text,out _))
            {
                return null;
            }
            return DateTimeOffset.Parse(text);
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value is null)
            {
                return null;
            }
            return ((DateTimeOffset)value).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    public class GEnumConverter<T> : DefaultTypeConverter where T : struct
    {

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            int parsedValue;
            if (Int32.TryParse(text, out parsedValue))
            {
                return (T)(object)parsedValue;
            }
            return base.ConvertFromString(text, row, memberMapData);

            
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            T result;
            if (Enum.TryParse<T>(value.ToString(), out result))
            {
                return (Convert.ToInt32(result)).ToString();
            }
            return base.ConvertToString(value, row, memberMapData);
        }
    }    



}
