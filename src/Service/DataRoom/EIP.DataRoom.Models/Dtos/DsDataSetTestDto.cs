using EIP.Common.Models.Paging;

namespace EIP.DataRoom.Models.Dtos
{
    public class DsDataSetTestInput : QueryParam
    {
        public string dataSetType { get; set; }

        public int dataSourceId { get; set; }

        public string script { get; set; }

    }
    public class scriptOutput
    {
        public List<string> fieldInfo { get; set; }

        public string tableName { get; set; }
    }
    public class DsDataSetTestOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<DsDataSetTestStructureOutput> structure { get; set; } = new List<DsDataSetTestStructureOutput>();

        /// <summary>
        /// 
        /// </summary>
        public List<string> tableNameList { get; set; } = new List<string>();

    }

    public class DsDataSetTestStructureOutput
    {
        public string fieldName { get; set; }

        public string fieldType { get; set; }

        public string fieldDesc { get; set; }
    }
}
