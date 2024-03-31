using System.Data;
using System.Dynamic;
using System.IO;
using ExcelDataReader;
using Newtonsoft.Json;

namespace Shoping.Business.Helper
{
    public class ExcelHelper
    {
        public static List<TEntity> ReadAsList<TEntity>(MemoryStream memorystream)
        {
            var lstData = new List<TEntity>();
            var lstHeaders = new List<string>();
            var lstJSON = new List<Dictionary<string, object>>();
            using (var reader = ExcelReaderFactory.CreateReader(memorystream))
            {
                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true,
                    }
                });

                foreach(DataTable table in dataSet.Tables)
                {
                    foreach(DataColumn column in table.Columns)
                    {
                        lstHeaders.Add(column.ColumnName);
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        var dict = new Dictionary<string, object>();
                        var values = row.ItemArray;
                        for(int i = 0; i < lstHeaders.Count; i++)
                        {
                            var key = lstHeaders[i];
                            var value = values[i].ToString();
                            dict.Add(key, value);
                        }
                        lstJSON.Add(dict);
                    }
                }
            }
            lstData = JsonConvert.DeserializeObject<List<TEntity>>(JsonConvert.SerializeObject(lstJSON));
            return lstData;
        }
    }
}
