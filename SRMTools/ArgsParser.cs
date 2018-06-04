using System;
using System.Collections.Generic;

namespace SRMTools
{
    class ArgsParser
    {
        private static String options;
        private static List<ArgItem> ArgList;

        public static String[] GetArgArray()
        {
            return options.Split(',');            
        }

        public static void Options(string v)
        {
            options = v;
            ArgList = new List<ArgItem>();
            String[] args = options.Split(',');
            foreach(var item in args)
            {
                String[] split = item.Split('|');
                ArgList.Add(new ArgItem(split[0], split[1]));
            }
        }

        public static bool HasValidArgs()
        {
            foreach(var item in ArgList)
            {
                if(item.value != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static void Parse(string[] args)
        {
            foreach(var arg in args)
            {
                ArgList.ForEach(delegate (ArgItem item)
                {
                    if ((arg.IndexOf(item.fullName) > -1) && ((arg.Length - item.shortName.Length) > 0))
                    {
                        item.value = arg.Substring(item.fullName.Length);
                        return;
                    }
                    if ((arg.IndexOf(item.shortName) > -1) && ((arg.Length - item.shortName.Length) > 0))
                    {
                        item.value = arg.Substring(item.shortName.Length);
                        return;
                    }
                });                
            }
        }

        private static object GetValue(string v)
        {
            foreach(var item in ArgList)
            {
                if(item.shortName == v)
                {
                    return item.value;
                }
            }
            return null;
        }

        public static String GetValueString(string v)
        {
            var val = GetValue(v);
            if (val != null)
            {
                return (String)val;
            }
            return "";
        }
    }

    class ArgItem
    {
        public String shortName;
        public String fullName;
        public Object value;

        public ArgItem(String ShortName, String FullName, Object Value = null)
        {
            shortName = ShortName;
            fullName = FullName;
            value = Value;
        }
    }
}