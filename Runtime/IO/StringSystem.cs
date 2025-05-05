using System.Text;

namespace Stereome
{
    public struct StringSystem
    {
        public static string Append(params object[] obj)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < obj.Length; i++)
            {
                sb.Append(obj[i]);
            }
            return sb.ToString();
        }
        public static string AppendFormat(string format, params object[] obj)
        {
            return new StringBuilder().AppendFormat(format, obj).ToString();
        }
    }
}
