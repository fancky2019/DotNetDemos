using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Common
{
    public class DataTableModels
    {
        /// <summary>
        /// 泛型实体集合转换成DataTable
        /// </summary>
        /// <param name="dataList">实体类列表</param>
        /// <returns></returns>
        public static DataTable FillDataTable<T>(List<T> dataList, DataTable dt)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return null;
            }

            if (dt == null)
            {
                throw new Exception("source DataTable is null");
            }
            Type type = typeof(T);
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (T model in dataList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model) ?? DBNull.Value;
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }
    }
}
