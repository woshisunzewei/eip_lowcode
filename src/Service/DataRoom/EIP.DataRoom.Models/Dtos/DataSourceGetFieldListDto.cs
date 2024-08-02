using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIP.DataRoom.Models.Dtos
{
    public class DataSourceGetFieldListInput
    {
        public int id { get; set; }

        public string tableName { get; set; }
    }

    public class DataSourceGetFieldListOutput
    {
        public string columnComment { get; set; }

        public string columnName { get; set; }

        public string columnType { get; set; }

        public string fieldDesc { get; set; }
    }
}
