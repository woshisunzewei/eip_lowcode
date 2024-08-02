using EIP.Common.Models.Paging;
using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.DataSource
{
    public class SystemDataSourceConfigOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid DataBaseId { get; set; }
        public string Type { get; set; }

        public SystemDataSourceConfigOutputTable Table { get; set; }

        public SystemDataSourceConfigOutputProc Proc { get; set; }

        public SystemDataSourceConfigOutputView View { get; set; }

        public SystemDataSourceConfigOutputInterface Interface { get; set; }

        public List<SystemDataSourceConfigOutputColumns> Columns { get; set; }


        public List<SystemDataSourceConfigOutputInParams> InParams { get; set; }


        public List<SystemDataSourceConfigOutputOutParams> OutParams { get; set; }

        public Guid? DataMenuId { get; set; }

        public bool NoRuleGetData { get; set; }

        public Filter Filters { get; set; }
    }
    public class SystemDataSourceConfigOutputTable
    {
        public string Name { get; set; }
    }
    public class SystemDataSourceConfigOutputProc
    {
        public string Name { get; set; }
    }
    public class SystemDataSourceConfigOutputView
    {
        public string Name { get; set; }
    }
    public class SystemDataSourceConfigOutputInterface
    {
        public string Type { get; set; }

        public string Url { get; set; }
    }

    public class SystemDataSourceConfigOutputColumns
    {
    }

    public class SystemDataSourceConfigOutputInParams
    {
        public string Title { get; set; }

        public string Field { get; set; }
    }

    public class SystemDataSourceConfigOutputOutParams
    {
        public string Title { get; set; }

        public string Field { get; set; }

        public string Alias { get; set; }
    }
    
}
