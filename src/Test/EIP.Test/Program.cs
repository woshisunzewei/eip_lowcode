using Dapper;
using Dm;
using EIP.Base.Models.Entities.System;
using Kdbndp;
using System.Data;

string sql = "select * from  System_UserInfo where Code=:Code ";
var data = KdbndpConnectionFactory.GetConn().QueryFirstOrDefault<SystemUserInfo>(sql, new Test
{
Code = "Admin"
});
public class KdbndpConnectionFactory
{
    static string sqlConnString = "Server=localhost;PORT=5237;USER=SYSDBA;PASSWORD=SYSDBA;SCHEMA=eip;database=eip";
    public static IDbConnection GetConn()
    {
        return new KdbndpConnection(sqlConnString);
    }
}

public class Test
{
    public string Code { get; set; }
}





//using Dapper;
//using Dm;
//using EIP.Base.Models.Entities.System;
//using System.Data;

//string sql = "select * from  System_UserInfo where Code=:Code ";
//var data = DmConnectionFactory.GetConn().QueryFirstOrDefault<SystemUserInfo>(sql, new Test
//{
//    Code="Admin"
//});
//public class DmConnectionFactory
//{
//    static string sqlConnString = "Server=localhost;PORT=5237;USER=SYSDBA;PASSWORD=SYSDBA;SCHEMA=eip;database=eip";
//    public static IDbConnection GetConn()
//    {
//        return new DmConnection(sqlConnString);
//    }
//}

//public class Test          
//{
//    public string Code { get; set; }
//}

