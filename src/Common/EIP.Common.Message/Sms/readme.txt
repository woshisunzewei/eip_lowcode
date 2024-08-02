﻿
            // 请根据实际 appid 和 appkey 进行开发，以下只作为演示 sdk 使用
            // appid,appkey,templId申请方式可参考接入指南 https://www.qcloud.com/document/product/382/3785#5-.E7.9F.AD.E4.BF.A1.E5.86.85.E5.AE.B9.E9.85.8D.E7.BD.AE
            int sdkappid = 1400155442;
            string appkey = "42be634358e490ff7be0e64afdf42737";
            string phoneNumber1 = "18349249218";
            int tmplId = 178003;
            try
            {
                TencentCloudSmsSingleSender singleSender = new TencentCloudSmsSingleSender(sdkappid, appkey);
                var singleResult = singleSender.Send(0, "86", phoneNumber1, "测试短信，普通单发，深圳，小明，上学。", "", "");
                Console.WriteLine(singleResult);

                //List<string> templParams = new List<string>();
                //templParams.Add("指定模板单发");
                //templParams.Add("深圳");
                //templParams.Add("小明");
                //// 指定模板单发
                //// 假设短信模板内容为：测试短信，{1}，{2}，{3}，上学。
                //singleResult = singleSender.SendWithParam("86", phoneNumber1, tmplId, templParams, "", "", "");
                //Console.WriteLine(singleResult);

                //TencentCloudSmsMultiSender multiSender = new TencentCloudSmsMultiSender(sdkappid, appkey);
                //List<string> phoneNumbers = new List<string> {phoneNumber1};
                ////phoneNumbers.Add(phoneNumber2);
                ////phoneNumbers.Add(phoneNumber3);

                //// 普通群发
                //// 下面是 3 个假设的号码
                //var multiResult = multiSender.Send(0, "86", phoneNumbers, "测试短信，普通群发，深圳，小明，上学。", "", "");
                //Console.WriteLine(multiResult);

                //// 指定模板群发
                //// 假设短信模板内容为：测试短信，{1}，{2}，{3}，上学。
                //templParams.Clear();
                //templParams.Add("指定模板群发");
                //templParams.Add("深圳");
                //templParams.Add("小明");
                //multiResult = multiSender.SendWithParam("86", phoneNumbers, tmplId, templParams, "", "", "");
                //Console.WriteLine(multiResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }