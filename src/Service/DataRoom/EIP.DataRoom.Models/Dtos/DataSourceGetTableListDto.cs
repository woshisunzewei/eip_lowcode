namespace EIP.DataRoom.Models.Dtos
{
    public class DataSourceGetTableListInput
    {
        public int id { get; set; }
    }

    public class DataSourceGetTableListOutput
    {
        public string name { get; set; }

        public int status { get; set; }
    }
}
