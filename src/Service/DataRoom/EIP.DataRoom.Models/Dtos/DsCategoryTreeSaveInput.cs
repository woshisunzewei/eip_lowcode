namespace EIP.DataRoom.Models.Dtos
{
    public class DsCategoryTreeSaveInput
    {
        public int? id { get; set; }

        public string name { get; set; }

        public int parentId { get; set; }

        public string type { get; set; }
    }
}
