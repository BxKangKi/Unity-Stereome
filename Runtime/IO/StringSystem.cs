using System.Text;

namespace Stereome
{
    public readonly struct StringSystem
    {
        // Static readonly string builder for save memeory and prevent allocated garbages.
        // If you call Clear() method, earlier contents are going to all removed.
        private static readonly StringBuilder sb = new StringBuilder();
        public static void Clear()
        {
            sb.Clear();
        }

        /// <summary>
        /// Append object(string) to current String System.
        /// </summary>
        /// <param name="str"></param>
        public static void Append(object str)
        {
            sb.Append(str);
        }

        public static string Append(object a, object b)
        {
            sb.Clear();
            sb.Append(a);
            sb.Append(b);
            return Value();
        }


        public static string Append(params object[] objects)
        {
            sb.Clear();
            for (int i = 0; i < objects.Length; i++)
            {
                sb.Append(objects[i]);
            }
            return Value();
        }

        public static string Value()
        {
            return sb.ToString();
        }
    }
}
