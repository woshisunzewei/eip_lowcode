//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Google.ProtocolBuffers;
//using com.gexin.rp.sdk.dto;
//using com.igetui.api.openservice;
//using com.igetui.api.openservice.igetui;
//using com.igetui.api.openservice.igetui.template;
//using com.igetui.api.openservice.payload;
//using com.igetui.api.openservice.igetui.sms;
//using System.Net;
//using com.igetui.api.openservice.igetui.template.style;

///**
// * 
// * 说明：
// *      此工程是一个测试工程，所用的相关.dll文件，都已经存在protobuffer文件里，需要加载到References里。
// *      工程中还有用到一个System.Web.Extensions文件，这个文件是用到Framework里V4.0版本的，
// *      一般路径如下：C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0，
// *      或如下路径：C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.5没有的也可以在protobuffer
// *      文件夹里找到。如再有问题，请直接联系技术客服，谢谢！
// *      GetuiServerApiSDK：此.dll文件为个推C#版本的SDK文件
// *      Google.ProtocolBuffers：此.dll文件为Google的数据交换格式文件
// *  注：
// *      新增一个连接超时时间设置，通过在环境变量--用户变量中增加名为：GETUI_TIMEOUT 的变量（修改环境变量，
//       电脑重启后才能生效），值则是超时时间，如不设定，则默认为20秒。
// **/

//namespace GetuiServerApiSDKDemo
//{
//    public class demo
//    {
//        //参数设置 <-----参数需要重新设置----->
//        //http的域名
//        private static String HOST = "http://sdk.open.api.igexin.com/apiex.htm";

//        //https的域名
//        //private static String HOST = "https://api.getui.com/apiex.htm";

//        //定义常量, appId、appKey、masterSecret 采用本文档 "第二步 获取访问凭证 "中获得的应用配置
//        private static String APPID = "LLNstWgyGm8UM2SsherlU3";
//        private static String APPKEY = "j4l9rfhgwT79B9EH1Wvai8";
//        private static String MASTERSECRET = "ItQSEF1Hgw7V0q95csarI1";
//        private static String CLIENTID = "a7f536a051965dec65ae7e1b59956f0d";
//        private static String CLIENTID1 = "e605a0db5ce3cca9b76b012978064940";
//        private static String CLIENTID2 = "e605a0db5ce3cca9b76b012978064940";
//        private static String GroupName = "app推送";
//        private static String Badge = "50";  
//        private static String TASKID = "OSA-0903_bWHwhpFPEC7i5nZwHmc6d";
//        private static String ALIAS = "请输入别名";
//        private static string PN = "13550347892";
//        static void Main(string[] args)
//        {
//            try
//            {
//                //toList接口每个用户状态返回是否开启，可选
//                Console.OutputEncoding = Encoding.GetEncoding("UTF-8");
//            Environment.SetEnvironmentVariable("needDetails", "true");

//                //下为消息推送的四种方式，单独使用时，请注释掉另外三种方法
//                //getContentIdDemo();
//                //pushTagMessageDemo();
//                //pushMessageToSingleToSMS();
//                //getLast24HoursOnlineUserStatisticsDemo();
//                //queryUserCountDemo();
//                //getUserCountByTagsDemo();
//                //restoreCidListFromBlkDemo();
//                //addCidListToBlkDemo();
//                //getPushActionResultByTaskidsDemo();
//                //对单个用户的推送
//                PushMessageToSingle();
//                //对指定列表用户推送
//                //PushMessageToList();
//                //对指定应用群推送
//                //pushMessageToApp();
//                //批量单推
//                //pushMessageToSingleBatch();
//                //多标签推送
//                //pushTagMessage();
//                //System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match("ADDR=1234;NAME=ZHANG;PHONE=6789", "NAME=(.+);");
//                //Console.Write(match.Success);
//                //getPushResultByTaskidListDemo();
//                //getPushResultByGroupNameDemo();
//                //// pushTagMessageRetryDemo();
//                //getScheduleTaskDemo();
//                //delScheduleTaskDemo();
//                //queryCidPnDemo();
//                //stopSendSmsDemo();

//                //getUserStatus();
//                //setBadgeForCIDDemo();
//                //setBadgeForDeviceTokenDemo();
//                //bindCidPnDemo();
//                //unbindCidPnDemo();
//                //queryAppPushDataByDate();
//                //queryAppUserDataByDate();

//                //setTag();
//                //stopTask();
//                //getPushResultTest();
//                //getUserTagsTest();
//                //getPersonaTagsDemo();

//                //bindAlias();
//                //queryClientId();
//                //queryAlias();
//                //aliasUnBind();
//                //bindAliasBatch();
//                //unBindAliasAll();
//            }
//            catch (Exception e)
//            {
//                System.Console.WriteLine(" --------------------------------- throw errors ---------------------------------");
//                System.Console.WriteLine(e.Message);
//                System.Console.WriteLine(" -----------------------------------------------------------------");
//                System.Console.WriteLine(e.StackTrace);
//                System.Console.WriteLine(" -----------------------------------------------------------------");
//            }
//        }
//        private static void unBindAliasAll()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.unBindAliasAll(APPID, ALIAS);
//            System.Console.Write(res);
//        }

//        private static void bindAliasBatch()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            com.igetui.api.openservice.igetui.Target target1 = new com.igetui.api.openservice.igetui.Target();
//            target1.appId = APPID;
//            target1.clientId = CLIENTID;
//            target1.alias = ALIAS;
//            com.igetui.api.openservice.igetui.Target target2 = new com.igetui.api.openservice.igetui.Target();
//            target2.alias = ALIAS;
//            List<com.igetui.api.openservice.igetui.Target> targets = new List<com.igetui.api.openservice.igetui.Target>();
//            targets.Add(target1);
//            targets.Add(target2);
//            string res = push.bindAlias(APPID, targets);
//            System.Console.Write(res);
//        }
//        private static void aliasUnBind()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.unBindAlias(APPID, ALIAS, CLIENTID);
//            System.Console.Write(res);
//        }
//        private static void queryAlias()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.queryAlias(APPID, CLIENTID);
//            System.Console.Write(res);
//        }
//        private static void queryClientId()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.queryClientId(APPID, ALIAS);
//            System.Console.Write(res);
//        }
//        private static void bindAlias()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.bindAlias(APPID, ALIAS, CLIENTID);
//            System.Console.Write(res);
//        }
//        private static void getPersonaTagsDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.getPersonaTags(APPID);
//            System.Console.Write(res);
//        }
//        private static void getUserTagsTest()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.getUserTags(APPID, CLIENTID);
//            System.Console.Write(res);
//        }
//        private static void getPushResultTest()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.getPushResult(TASKID);
//            System.Console.Write(res);
//        }
//        private static void stopTask()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            bool res = push.stop(TASKID);
//            System.Console.Write(res);
//        }
//        private static void setTag()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            List<string> tagList = new List<string>();
//            tagList.Add("标签1");
//            tagList.Add("标签2");
//            string res = push.setClientTag(APPID, CLIENTID, tagList);
//            System.Console.Write(res);
//        }
//        private static void queryAppUserDataByDate()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.queryAppUserDataByDate(APPID, "20150910");
//            System.Console.Write(res);
//        }
//        private static void queryAppPushDataByDate()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.queryAppPushDataByDate(APPID, "20150910");
//            System.Console.Write(res);
//        }
//        private static void unbindCidPnDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            List<string> list = new List<string>();
//            list.Add(CLIENTID);
//            string res = push.unbindCidPn(APPID, list);
//            System.Console.Write(res);
//        }
//        private static void setBadgeForDeviceTokenDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            List<string> deviceTokenList = new List<string>();
//            deviceTokenList.Add("60e6721af9a5fc87b97253f6188d6424ba2cfd8bdae11b1c9a0ab0e90a50ac48");
//            string res = push.setBadgeForDeviceToken(Badge, APPID, deviceTokenList);
//            System.Console.Write(res);
//        }
//        private static void setBadgeForCIDDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            List<string> cidList = new List<string>();
//            cidList.Add("c1c91984172baf930e808d088be06b7b");
//            string res = push.setBadgeForCID(Badge, APPID, cidList);
//            System.Console.Write(res);
//        }
//        private static void getUserStatus()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.getClientIdStatus(APPID, CLIENTID);
//            System.Console.Write(res);
//        }
//        private static void stopSendSmsDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.stopSendSms(APPID, TASKID);
//            System.Console.Write(res);
//        }
//        private static void queryCidPnDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            List<string> cidList = new List<string>();
//            cidList.Add(CLIENTID);
//            string res = push.queryCidPn(APPID, cidList);
//            System.Console.Write(res);
//        }
//        private static void getScheduleTaskDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.getScheduleTask(TASKID, APPID);
//            System.Console.Write(res);
//        }

//        private static void delScheduleTaskDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.delScheduleTask(TASKID, APPID);
//            System.Console.Write(res);
//        }

//        private static void getPushResultByGroupNameDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string res = push.getPushResultByGroupName(APPID, GroupName);
//            System.Console.Write(res);
//        }
//        private static void getPushResultByTaskidListDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            List<string> taskIdList = new List<string>();
//            taskIdList.Add("OSA-0904_cc2QS6fcsqAUx5ovIxbHH6");
//            string res = push.getPushResultByTaskidList(taskIdList);
//            Console.Write(res);
//        }
//        private static void getPushActionResultByTaskidsDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            List<string> taskIdList = new List<string>();
//            taskIdList.Add("OSA-0904_oQlwc8ZPcb9GSTpYbJWkH3");
//            taskIdList.Add("OSA-0904_cBXBc7nDVm9lKxQ3Ybb7M3");
//            taskIdList.Add("OSA-0904_cc2QS6fcsqAUx5ovIxbHH6");
//            List<string> actionIdList = new List<string>();
//            actionIdList.Add("90005");
//            actionIdList.Add("90006");
//            string res = push.getPushActionResultByTaskids(taskIdList, actionIdList);
//            Console.Write(res);
//        }
//        private static void addCidListToBlkDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            List<string> cidList = new List<string>();
//            cidList.Add("adf1e2252b3df06bee9a9a0a9adb57eb");
//            cidList.Add("");
//            string res = push.addCidListToBlk(APPID, cidList);
//            Console.Write(res);
//        }
//        private static void restoreCidListFromBlkDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            List<string> cidList = new List<string>();
//            cidList.Add("adf1e2252b3df06bee9a9a0a9adb57eb");
//            cidList.Add("5089d7fd12d6ce1e787c038532889712");
//            string res = push.restoreCidListFromBlk(APPID, cidList);
//            Console.Write(res);
//        }
//        private static void getUserCountByTagsDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            List<string> tagList = new List<string>();
//            tagList.Add("18-20");
//            tagList.Add("5555");
//            string res = push.getUserCountByTags(APPID, tagList);
//            Console.Write(res);
//        }

//        public static void queryUserCountDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            AppConditions app = new AppConditions();
//            List<string> phonelist = new List<string>();
//            phonelist.Add("ANDROID");
//            List<string> provinceList = new List<string>();
//            provinceList.Add("浙江省");
//            List<string> tagList = new List<string>();
//            tagList.Add("发生大幅度三");
//            app.addCondition(AppConditions.PHONE_TYPE, phonelist);
//            app.addCondition(AppConditions.REGION, provinceList);
//            app.addCondition(AppConditions.TAG, tagList);

//            string res = push.queryUserCount(APPID, app);
//            Console.Write(res);
//        }
//        private static void getLast24HoursOnlineUserStatisticsDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string appid = "LLNstWgyGm8UM2SsherlU3";
//            string res = push.getLast24HoursOnlineUserStatistics(appid);
//            Console.Write(res);
//        }
//        private static void getContentIdDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            string contentId = "OSS-1030_3d86cfa3efcff1a5453325adfebf0d06";
//            bool res = push.cancelContentId(contentId);
//            System.Diagnostics.Debug.WriteLine(res);
//            Console.Write(res);
//        }
//        private static void pushMessageToSingleToSMS()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            TransmissionTemplate template = TransmissionTemplateDemo();
//            com.igetui.api.openservice.igetui.sms.SmsInfo smsMessage = new com.igetui.api.openservice.igetui.sms.SmsInfo();
//            Dictionary<string, string> smscount = new Dictionary<string, string>();
//            smscount.Add("url", "http://www.baidu.com");
//            smsMessage.Payload = "1234";
//            smsMessage.SmsContent = smscount;
//            smsMessage.SmsTemplateId = "d0d4c3315e49e2868c7fd235379d32ed";
//            smsMessage.IsApplink = false;
//            smsMessage.Url = "www.bai";
//            smsMessage.OfflineSendtime = 1000;
//            template.setSmsInfo(smsMessage);
//            SingleMessage message = new SingleMessage();
//            message.IsOffline = true;
//            //  离线有效时间
//            message.OfflineExpireTime = 1000 * 3600 * 12;
//            message.Data = template;
//            com.igetui.api.openservice.igetui.Target target = new com.igetui.api.openservice.igetui.Target();
//            target.appId = APPID;
//            target.clientId = CLIENTID;
//            string ret = push.pushMessageToSingle(message, target);
//            Console.Write(ret + "pushsms");

//        }
//        private static void pushTagMessageDemo()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            LinkTemplate template = LinkTemplateDemo();
//            TagMessage app = new TagMessage();
//            app.IsOffline = false;
//            app.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
//            app.Data = template;
//            List<string> appidlist = new List<string>();
//            appidlist.Add(APPID);
//            app.AppIdList = appidlist;
//            app.Speed = 1;
//            app.Tag = "111";
//            string res = push.pushTagMessage(app, "123545");

//            Console.Write(res+"PushTagMessage");
//        }

//        private static void PushMessageToSingle()
//        {

//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

//            //消息模版：TransmissionTemplate:透传模板

//           // TransmissionTemplate template = TransmissionTemplateDemo();
//            LinkTemplate template = LinkTemplateDemo();

//            // 单推消息模型
//            SingleMessage message = new SingleMessage();
//            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
//            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
//            message.Data = template;
//            //判断是否客户端是否wifi环境下推送，2为4G/3G/2G，1为在WIFI环境下，0为不限制环境
//            //message.PushNetWorkType = 1;  

//            com.igetui.api.openservice.igetui.Target target = new com.igetui.api.openservice.igetui.Target();
//            target.appId = APPID;
//            target.clientId = CLIENTID;
//            //target.alias = ALIAS;
//            try
//            {
//                String pushResult = push.pushMessageToSingle(message, target);

//                System.Console.WriteLine("-----------------------------------------------");
//                System.Console.WriteLine(pushResult);

//                System.Console.WriteLine("-----------------------------------------------");
//            }
//            catch (RequestException e)
//            {
//                String requestId = e.RequestId;
//                //发送失败后的重发
//                String pushResult = push.pushMessageToSingle(message, target, requestId);
//                System.Console.WriteLine("-----------------------------------------------");
//                System.Console.WriteLine(pushResult);
//                System.Console.WriteLine("-----------------------------------------------");
//            }
//        }

//        //PushMessageToList接口测试代码
//        private static void PushMessageToList()
//        {
//            // 推送主类（方式1，不可与方式2共存）
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            // 推送主类（方式2，不可与方式1共存）此方式可通过获取服务端地址列表判断最快域名后进行消息推送，每10分钟检查一次最快域名
//            //IGtPush push = new IGtPush("",APPKEY,MASTERSECRET);
//            ListMessage message = new ListMessage();

//            NotificationTemplate template = NotificationTemplateDemo();
//            // 用户当前不在线时，是否离线存储,可选
//            message.IsOffline = true;
//            // 离线有效时间，单位为毫秒，可选
//            message.OfflineExpireTime = 1000 * 3600 * 12;
//            message.Data = template;
//            //message.PushNetWorkType = 0;        //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为不限制网络环境。
//            //设置接收者
//            List<com.igetui.api.openservice.igetui.Target> targetList = new List<com.igetui.api.openservice.igetui.Target>();
//            com.igetui.api.openservice.igetui.Target target1 = new com.igetui.api.openservice.igetui.Target();
//            target1.appId = APPID;
//            target1.clientId = CLIENTID;

//            // 如需要，可以设置多个接收者
//            //com.igetui.api.openservice.igetui.Target target2 = new com.igetui.api.openservice.igetui.Target();
//            //target2.AppId = APPID;
//            //target2.ClientId = "ddf730f6cabfa02ebabf06e0c7fc8da0";

//            targetList.Add(target1);
//            //targetList.Add(target2);

//            String contentId = push.getContentId(message);
//            String pushResult = push.pushMessageToList(contentId, targetList);
//            System.Console.WriteLine("-----------------------------------------------");
//           System.Console.WriteLine(pushResult);
//            System.Console.WriteLine("-----------------------------------------------");
//        }


//        //pushMessageToApp接口测试代码
//        private static void pushMessageToApp()
//        {
//            // 推送主类（方式1，不可与方式2共存）
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            // 推送主类（方式2，不可与方式1共存）此方式可通过获取服务端地址列表判断最快域名后进行消息推送，每10分钟检查一次最快域名
//            //IGtPush push = new IGtPush("",APPKEY,MASTERSECRET);

//            AppMessage message = new AppMessage();

//            // 设置群推接口的推送速度，单位为条/秒，仅对pushMessageToApp（对指定应用群推接口）有效
//            message.Speed = 100;

//            TransmissionTemplate template = TransmissionTemplateDemo();

//            // 用户当前不在线时，是否离线存储,可选
//            message.IsOffline = false;
//            // 离线有效时间，单位为毫秒，可选  
//            message.OfflineExpireTime = 1000 * 3600 * 12;
//            message.Data = template;
//            //message.PushNetWorkType = 0;        //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为不限制网络环境。
//            List<String> appIdList = new List<string>();
//            appIdList.Add(APPID);

//            //通知接收者的手机操作系统类型
//            List<String> phoneTypeList = new List<string>();
//            //phoneTypeList.Add("ANDROID");
//            //phoneTypeList.Add("IOS");
//            //通知接收者所在省份
//            List<String> provinceList = new List<string>();
//            //provinceList.Add("浙江");
//            //provinceList.Add("上海");
//            //provinceList.Add("北京");

//            List<String> tagList = new List<string>();
//            //tagList.Add("开心");

//            message.AppIdList = appIdList;
//            message.PhoneTypeList = phoneTypeList;
//            message.ProvinceList = provinceList;
//            message.TagList = tagList;


//            String pushResult = push.pushMessageToApp(message);
//            System.Console.WriteLine("-----------------------------------------------");
//            System.Console.WriteLine(pushResult);
//        }

//        public static void pushMessageToSingleBatch()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            IBatch batch = new BatchImpl(APPKEY, push);
//            //消息模版：TransmissionTemplate:透传模板
//            TransmissionTemplate templateTrans = TransmissionTemplateDemo();
//            com.igetui.api.openservice.igetui.sms.SmsInfo smsinfo = new com.igetui.api.openservice.igetui.sms.SmsInfo();
//            smsinfo.SmsTemplateId = "";
//            Dictionary<string, string> smscount = new Dictionary<string, string>{
//                {"code1", "123"},
//                {"time", "5"}
//            };
//            smsinfo.SmsContent = smscount;
//            smsinfo.SmsTemplateId = "1a0ad952756f4c679ca67f008bf37b5e";
//            smsinfo.OfflineSendtime = 1000L;
//            templateTrans.setSmsInfo(smsinfo);
//            // 单推消息模型
//            SingleMessage messageTrans = new SingleMessage();
//            messageTrans.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
//            messageTrans.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
//            messageTrans.Data = templateTrans;
//            //判断是否客户端是否wifi环境下推送，2为4G/3G/2G，1为在WIFI环境下，0为不限制环境
//            //messageTrans.PushNetWorkType = 1;  

//            com.igetui.api.openservice.igetui.Target targetTrans = new com.igetui.api.openservice.igetui.Target();
//            targetTrans.appId = APPID;
//            targetTrans.clientId = CLIENTID1;
//            batch.add(messageTrans, targetTrans);

//            NotificationTemplate templateNoti = NotificationTemplateDemo();

//            // 单推消息模型
//            SingleMessage messageNoti = new SingleMessage();
//            messageNoti.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
//            messageNoti.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
//            messageNoti.Data = templateNoti;
//            //判断是否客户端是否wifi环境下推送，2为4G/3G/2G，1为在WIFI环境下，0为不限制环境
//            //messageNoti.PushNetWorkType = 1;  

//            com.igetui.api.openservice.igetui.Target targetNoti = new com.igetui.api.openservice.igetui.Target();
//            targetNoti.appId = APPID;
//            targetNoti.clientId = CLIENTID2;
//            batch.add(messageNoti, targetNoti);
//            try
//            {
//                String ret = batch.submit();
//                Console.Out.WriteLine(ret);
//            }
//            catch (Exception e)
//            {
//                batch.retry();
//                Console.WriteLine(e);
//            }
//        }

//        static void pushTagMessage()
//        {
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            TagMessage message = TagMessageDemo();
//            System.Console.WriteLine(push.pushTagMessage(message));
//        }

//        public static TagMessage TagMessageDemo()
//        {
//            TagMessage message = new TagMessage();
//            List<String> appIdList = new List<string>();
//            appIdList.Add(APPID);
//            NotificationTemplate template = NotificationTemplateDemo();

//            message.AppIdList = appIdList;
//            message.Data = template;
//            message.Speed = 1;
//            message.Tag = "111";
//            return message;
//        }


//        //通知透传模板动作内容
//        public static NotificationTemplate NotificationTemplateDemo()
//        {
//            NotificationTemplate template = new NotificationTemplate();
//            template.AppId = APPID;
//            template.AppKey = APPKEY;
//            //通知栏标题
//            template.Title = "请填写通知标题";
//            //通知栏内容     
//            template.Text = "请填写通知内容";
//            //通知栏显示本地图片
//            template.Logo = "";
//            //通知栏显示网络图标
//            template.LogoURL = "";
//            //应用启动类型，1：强制应用启动  2：等待应用启动
//            template.TransmissionType = 1;
//            //透传内容  
//            template.TransmissionContent = "请填写透传内容";
//            //接收到消息是否响铃，true：响铃 false：不响铃   
//            template.IsRing = true;
//            //接收到消息是否震动，true：震动 false：不震动   
//            template.IsVibrate = true;
//            //接收到消息是否可清除，true：可清除 false：不可清除    
//            template.IsClearable = true;
//            //设置通知定时展示时间，结束时间与开始时间相差需大于6分钟，消息推送后，客户端将在指定时间差内展示消息（误差6分钟）
//            String begin = "2015-03-06 14:36:10";
//            String end = "2015-03-06 14:46:20";
//            template.setDuration(begin, end);

//            return template;
//        }

//        //透传模板动作内容
//        protected internal static TransmissionTemplate TransmissionTemplateDemo()
//        {
//            TransmissionTemplate template = new TransmissionTemplate();
//            template.AppId = APPID;
//            template.AppKey = APPKEY;
//            //应用启动类型，1：强制应用启动 2：等待应用启动
//            template.TransmissionType = 1;
//            //透传内容  
//            template.TransmissionContent = "透传内容";
//            com.igetui.api.openservice.igetui.template.notify.Notify notify = new com.igetui.api.openservice.igetui.template.notify.Notify();
//            notify.Title = "titleee";
//            notify.Content = "contentee";
//            notify.Intent = "intent:#Intent;mm;end";
//            //notify.Payload='payloadtest';
//            notify.Type = NotifyInfo.Types.Type._payload;
//            template.set3rdNotifyInfo(notify);
//            APNPayload apnpayload = new APNPayload();
//            apnpayload.Badge = 4;
//            apnpayload.Sound = "com.gexin.ios.silence";
//            apnpayload.addCustomMsg("payload", "啦啦啦啦啦啦");
//            apnpayload.ContentAvailable = 1;
//            apnpayload.Category = "ACTIONABLE";
//            apnpayload.VoicePlayType = 2; 
//            apnpayload.VoicePlayMessage = "支付宝到账一千万元整";
//            DictionaryAlertMsg alertMsg = new DictionaryAlertMsg();
//            alertMsg.Body = "body";
//            alertMsg.ActionLocKey = "actionLockey";
//            alertMsg.LocKey = "lockey";
//            List<string> locargs = new List<string>();
//            locargs.Add("locArgs");
//            alertMsg.LocArgs = locargs;
//            alertMsg.LaunchImage = "launchImage";
//            // IOS8.2以上版本支持;
//            alertMsg.Title = "Title";
//            List<string> TitleLocArg = new List<string>();
//            TitleLocArg.Add("TitleLocArg");
//            alertMsg.TitleLocArgs = TitleLocArg;
//            alertMsg.TitleLocKey = "TitleLocKey";
//            apnpayload.AlertMsg = alertMsg;
//            //Style0 style = new Style0();
//            //style.Title = "123123";
//            //style.Text = "1231";
//            //style.Logo = "";
//            //style.IsClearable = true;
//            //style.IsRing = true;
//            //style.IsVibrate = true;
//            //设置通知定时展示时间，结束时间与开始时间相差需大于6分钟，消息推送后，客户端将在指定时间差内展示消息（误差6分钟）
//            //String begin = "2015-03-06 14:36:10";
//            //String end = "2015-03-06 14:46:20";
//            //template.setDuration(begin, end);
//            //VoIPPayload voIPPayload = new VoIPPayload();
//            //voIPPayload.voIPPayload = "getui";
//            template.setAPNInfo(apnpayload);
//            return template;
//        }

//        //网页模板内容
//        public static LinkTemplate LinkTemplateDemo()
//        {
//            LinkTemplate template = new LinkTemplate();
//            template.AppId = APPID;
//            template.AppKey = APPKEY;
//            //通知栏标题
//            template.Title = "C#测试";
//            //通知栏内容 
//            template.Text = "C#测试";
//            //通知栏显示本地图片 
//            template.Logo = "";
//            //通知栏显示网络图标，如无法读取，则显示本地默认图标，可为空
//            template.LogoURL = "";
//            //打开的链接地址    
//            template.Url = "http://www.baidu.com";
//            //接收到消息是否响铃，true：响铃 false：不响铃   
//            template.IsRing = false;
//            //接收到消息是否震动，true：震动 false：不震动   
//            template.IsVibrate = false;
//            //接收到消息是否可清除，true：可清除 false：不可清除
//            template.IsClearable = false;
//            return template;
//        }

//        //通知栏弹框下载模板
//        public static NotyPopLoadTemplate NotyPopLoadTemplateDemo()
//        {
//            NotyPopLoadTemplate template = new NotyPopLoadTemplate();
//            template.AppId = APPID;
//            template.AppKey = APPKEY;
//            //通知栏标题
//            template.NotyTitle = "请填写通知标题";
//            //通知栏内容
//            template.NotyContent = "请填写通知内容";
//            //通知栏显示本地图片
//            template.NotyIcon = "icon.png";
//            //通知栏显示网络图标
//            template.LogoURL = "http://www-igexin.qiniudn.com/wp-content/uploads/2013/08/logo_getui1.png";
//            //弹框显示标题
//            template.PopTitle = "弹框标题";
//            //弹框显示内容    
//            template.PopContent = "弹框内容";
//            //弹框显示图片    
//            template.PopImage = "";
//            //弹框左边按钮显示文本    
//            template.PopButton1 = "下载";
//            //弹框右边按钮显示文本    
//            template.PopButton2 = "取消";
//            //通知栏显示下载标题
//            template.LoadTitle = "下载标题";
//            //通知栏显示下载图标,可为空 
//            template.LoadIcon = "file://push.png";
//            //下载地址，不可为空
//            template.LoadUrl = "http://www.appchina.com/market/d/425201/cop.baidu_0/com.gexin.im.apk ";
//            //应用安装完成后，是否自动启动
//            template.IsActived = true;
//            //下载应用完成后，是否弹出安装界面，true：弹出安装界面，false：手动点击弹出安装界面 
//            template.IsAutoInstall = true;
//            //接收到消息是否响铃，true：响铃 false：不响铃
//            template.IsBelled = true;
//            //接收到消息是否震动，true：震动 false：不震动   
//            template.IsVibrationed = true;
//            //接收到消息是否可清除，true：可清除 false：不可清除    
//            template.IsCleared = true;
//            return template;
//        }
//        //private static void pushTagMessageRetryDemo()
//        //{
//        //    IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//        //    NotificationTemplate template = new NotificationTemplate();
//        //    template.AppId = "s3olXQVg1d5mwSaZ2Oz642";
//        //    template.AppKey = "YZkTKbz91l84HyM0TGlb38";
//        //    template.TransmissionType = 2;
//        //    template.TransmissionContent = "测试离线";
//        //    TagMessage message = new TagMessage();
//        //    message.IsOffline = false;
//        //    message.OfflineExpireTime = 10 * 60 * 1000;
//        //    message.Data = template;
//        //    List<string> appidlist = new List<string>();
//        //    appidlist.Add(APPID);
//        //    message.AppIdList = appidlist;
//        //    message.Speed = 1;
//        //    string res = push.pushTagMessageRetry(message);
//        //    System.Console.Write(res);
//        //}
//        private static void bindCidPnDemo()
//        {
//            Encoding encoding = new UTF8Encoding(true);
//            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
//            Dictionary<string, String> dict = new Dictionary<string, String>();
//            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
//            byte[] data = md5Hasher.ComputeHash(encoding.GetBytes(PN));
//            StringBuilder sBuilder = new StringBuilder();
//            for (int i = 0; i < data.Length; i++)
//            {
//                sBuilder.Append(data[i].ToString("x2"));
//            }
//            dict.Add(CLIENTID, sBuilder.ToString());
//            string res = push.bindCidPn(APPID,dict);
//            System.Console.Write(res);
//        }
//    }
//}
