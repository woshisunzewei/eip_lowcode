using EIP.Common.Models.Resx;
using EIP.Common.Core.Util;

namespace EIP.Common.Repository
{
    public static class RepositoryUtil
    {
        /// <summary>
        /// 获取参数符号
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static string GetSymbol()
        {
            switch (ConfigurationUtil.GetDbConnectionType())
            {
                case ResourceDataBaseType.Dm:
                case ResourceDataBaseType.Kingbase:
                    return ":";
                default:
                    return "@";
            }
        }
    }
}
