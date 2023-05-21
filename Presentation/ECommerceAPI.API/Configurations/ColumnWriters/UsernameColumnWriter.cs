using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace ECommerceAPI.API.Configurations.ColumnWriters
{
    //during logging, it will create username column (if not exists) and save data in relation to the username
    public class UsernameColumnWriter : ColumnWriterBase
    {
        //dbType Column type 
        public UsernameColumnWriter() : base(NpgsqlDbType.Varchar)
        {
        }

        public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
        {
            //we need to get username
            //Properties during logging 
            var (username, value) = logEvent.Properties.FirstOrDefault(p => p.Key == "user_name");

            //If username value not null, return its value
            return value?.ToString() ?? null;

        }
    }
}
