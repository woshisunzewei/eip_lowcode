在原有的自身的方法上添加相关扩展方法


Json动态输出需要的属性
//正常情况下转换对象为Json字符串
 //var json = JsonConvert.SerializeObject(user);
 
//输出的结果为：
//{"UserId":8888,"Username":"jason","Password":"jasonsong","Realname":"成长的小猪"}
 
//如果我们不希望展示 Password 密码属性，可以使用排除选项 PropertyOption.Exclude
//var json = JsonConvert.SerializeObject(user, new JsonSerializerSettings {
//    ContractResolver = new PropertyDisplayResolver(PropertyOption.Exclude, "Password")
//});
 
//输出的结果为：
//{"UserId":8888,"Username":"jason","Realname":"成长的小猪"}
 
//如果我们只想输出用户名 Username 和 Realname 属性，可以使用包含选项 PropertyOption.Include
var json = JsonConvert.SerializeObject(user, new JsonSerializerSettings {
      ContractResolver = new PropertyDisplayResolver(PropertyOption.Include, "Username", "Realname")
});
 
//输出的结果为：
//{"Username":"jason","Realname":"成长的小猪"}
