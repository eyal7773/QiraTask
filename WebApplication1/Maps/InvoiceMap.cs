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
            Map(m => m.Status);
            Map(m => m.Amount);
            Map(m => m.PaymentMethod);
            /****
             * 
             * עצרתי כאן
             * צריך לתרגם את האינמים למספרים וכו'
             * ואחר כך לסיים פוסט
             * ופוט
             * ואז לבדוק וולידציות
             * ולבדוק שגיאות
             * ולעשות אינטרפייס
             * ולבדוק שהקוד תקין
             * 
             * */
        }
    }
    
    public class DateTimeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return DateTimeOffset.Parse(text);
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return ((DateTimeOffset)value).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    

}
