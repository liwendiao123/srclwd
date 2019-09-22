using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Senparc.Core.Utility
{
  public  class DataTableListUtil
    {
        /// <summary>
        /// list to datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ListToDt<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new
            DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }


        public static class DtToList<T> where T : new()
        {
            /// <summary>
            /// datatable to list
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public static List<T> ConvertToModel(DataTable dt)
            {

                List<T> ts = new List<T>();// 定义集合
                Type type = typeof(T); // 获得此模型的类型
                string tempName = "";
                foreach (DataRow dr in dt.Rows)
                {
                    T t = new T();
                    PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;
                        if (dt.Columns.Contains(tempName))
                        {
                            if (!pi.CanWrite) continue;
                            object value = dr[tempName];
                            if (value != DBNull.Value)
                                pi.SetValue(t, value, null);
                        }
                    }
                    ts.Add(t);
                }
                return ts;
            }
        }
    }
}
