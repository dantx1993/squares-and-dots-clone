using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace ThePattern.Json
{
    public static class JsonExtension
    {
        public static string ToJson(this object data)
        {
            JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(Convert.ToDouble(obj)));
            string jsonData = JsonMapper.ToJson(data);
            return jsonData;
        }

        public static string ToJsonFormat(this object data)
        {
            string jsonData = ToJson(data);
            return FormatJson(jsonData);
        }

        public static T ToObject<T>(this string jsonData)
        {
            JsonMapper.RegisterImporter<double, float>(input => Convert.ToSingle(input));
            T data = JsonMapper.ToObject<T>(jsonData);
            return data;
        }

        private static string FormatJson(string str)
        {
            int count = 0;
            bool flag1 = false;
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < str.Length; ++index)
            {
                char ch = str[index];
                switch (ch)
                {
                    case '"':
                        sb.Append(ch);
                        bool flag2 = false;
                        int num = index;
                        while (num > 0 && str[--num] == '\\')
                            flag2 = !flag2;
                        if (!flag2)
                        {
                            flag1 = !flag1;
                            break;
                        }
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!flag1)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, count).ForEach<int>(item => sb.Append("    "));
                            break;
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!flag1)
                        {
                            sb.Append(" ");
                            break;
                        }
                        break;
                    case '[':
                    case '{':
                        sb.Append(ch);
                        if (!flag1)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++count).ForEach<int>(item => sb.Append("    "));
                            break;
                        }
                        break;
                    case ']':
                    case '}':
                        if (!flag1)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --count).ForEach<int>(item => sb.Append("    "));
                        }
                        sb.Append(ch);
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (T obj in ie)
                action(obj);
        }
    }
}