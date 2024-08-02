namespace EIP.System.Models.Dtos.Dictionary
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDictionaryFindByParentIdInput
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HaveSelf { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
    }
}
