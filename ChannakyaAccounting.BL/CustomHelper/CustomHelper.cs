using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.BL.CustomHelper
{
        public enum EDeno
        {
            [Description("DenoIn")]
            DenoIn = 1,
            [Description("DenoOut")]
            DenoOut = 2,
        }
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());
            System.ComponentModel.DescriptionAttribute[] attributes =
                  (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(System.ComponentModel.DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }


}
