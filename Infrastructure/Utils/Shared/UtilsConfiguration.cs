using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infraction.Backend.Image.Service.Infrastructure.Utils.Shared
{
    public static class UtilsConfiguration
    {
        public static DateTime DateLocal = DateTimeNowLocal();

        public static DateTime DateTimeNowLocal()
        {
            var utcDateTime = DateTime.UtcNow;

            string nzTimeZoneKey = "Pacific Standard Time (Mexico)";

            TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(nzTimeZoneKey);

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, nzTimeZone);
        }

        public static DateTime GetDateTimeNowByTimeZone(string nzTimeZoneKey)
        {
            var utcDateTime = DateTime.UtcNow;

            TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(nzTimeZoneKey);

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, nzTimeZone);
        }

        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            string description = string.Empty;

            Type type = enumerationValue.GetType();

            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
            }

            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }

        public static string GetName<T>(this T enumerationValue) where T : struct
        {
            string? description = string.Empty;

            Type type = enumerationValue.GetType();

            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
            }

            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DefaultValueAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DefaultValueAttribute)attrs[0]).Value.ToString();
                }
            }

            return description;
        }
    }
}