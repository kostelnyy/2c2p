using CCPP.Core.Domain;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;

namespace CCPP.Core.FileParsers.Csv
{
    public class PaymentTransactionCsvMap : ClassMap<PaymentTranstaction>
    {
        public PaymentTransactionCsvMap()
        {
            Map(m => m.Id).Index(0);
            Map(m => m.Amount).Index(1);
            Map(m => m.Currency).Index(2);
            Map(m => m.TransactionDate)
                .ConvertUsing(r => DateTime.ParseExact(r.GetField(3), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
            Map(m => m.Status).Index(4).TypeConverter<StatusConverter>();
        }
    }

    internal class StatusConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text switch
            {
                "Approved" => PaymentTranstactionStatus.Approved,
                "Failed"   => PaymentTranstactionStatus.Rejected,
                "Finished" => PaymentTranstactionStatus.Done,
                _          => throw new ArgumentException($"cant parse status {text}")
            };
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }
    }
}
