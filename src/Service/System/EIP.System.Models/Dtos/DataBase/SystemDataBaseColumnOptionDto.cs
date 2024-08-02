using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.DataBase
{
    public class SystemDataBaseColumnOptionSelect
    {
        public bool dynamic { get; set; }

        public SystemDataBaseColumnOptionSelectDynamicConfig dynamicConfig { get; set; }

        public List<SystemDataBaseColumnOptionSelectOptions> options { get; set; }
    }

    public class SystemDataBaseColumnOptionSelectDynamicConfig
    {
        public SystemDataBaseColumnOptionSelectDynamicConfigSource source { get; set; }
    }

    public class SystemDataBaseColumnOptionSelectDynamicConfigSource
    {
        public string type { get; set; }
    }

    public class SystemDataBaseColumnOptionSelectOptions
    {
        public string value { get; set; }

        public string label { get; set; }
    }


    public class SystemDataBaseColumnOptionCorrelationRecord
    {
        public SystemDataBaseColumnOptionCorrelationRecordConfig dynamicConfig { get; set; }

        public List<SystemDataBaseColumnOptionCorrelationRecordConfigColumns> columns { get; set; }

    }

    public class SystemDataBaseColumnOptionCorrelationRecordConfig
    {
        public Guid? table { get; set; }

    }
    public class SystemDataBaseColumnOptionCorrelationRecordConfigColumns
    {
        public string name { get; set; }

        public bool isShow { get; set; }
    }

    public class SystemDataBaseColumnOptionCorrelationRecordDataOutput
    {
        public Guid? RelationId { get; set; }


    }


    public class SystemDataBaseColumnOptionSerialNo
    {
        public bool dynamic { get; set; }

        public SystemDataBaseColumnOptionSelectDynamicConfig dynamicConfig { get; set; }

        public List<SystemDataBaseColumnOptionSelectOptions> options { get; set; }
    }
}
