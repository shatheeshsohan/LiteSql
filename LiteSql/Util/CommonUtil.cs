using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteSql.Util
{
    public static class CommonUtil
    {
        public static string NVL(this string? self, string defaultVal) 
        {
            return !string.IsNullOrEmpty(self) ? self : defaultVal;
        }

        public static int getObjKeyIndex(List<string> columnsList) 
        {
            return columnsList.FindIndex(a => a == "obj_key");
        }

        public static void Move(this List<string> list, string item, int newIndex)
        {
            if (item != null)
            {
                var oldIndex = list.IndexOf(item);
                if (oldIndex > -1)
                {
                    list.RemoveAt(oldIndex);

                    if (newIndex > oldIndex) newIndex--;
                    // the actual index could have shifted due to the removal

                    list.Insert(newIndex, item);
                }
            }

        }
    }
}
