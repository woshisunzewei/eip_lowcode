<template>
  <a-spin :spinning="spinning">
    <a-card size="small" :bodyStyle="{ height: height, overflow: 'auto' }">
      <a-form-model
        ref="form"
        :model="form"
        :rules="rules"
        :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol"
      >
        <a-tabs default-active-key="1">
          <a-tab-pane key="1" tab="登录配置">
            <a-form-model-item label="标题" prop="loginTitle">
              <a-input
                v-model="form.loginTitle"
                placeholder="请输入Title:头部显示"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="系统名" prop="loginSystemName">
              <a-input
                v-model="form.loginSystemName"
                placeholder="请输入系统名:如EIP权限工作流平台"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="备案号" prop="loginCopyright">
              <a-input
                v-model="form.loginCopyright"
                placeholder="请输入备案号:如Copyright @2023 蜀ICP备18012364号-1"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="登录方式" prop="loginType">
              <a-checkbox-group
                v-model="form.loginType"
                :options="loginTypeOptions"
                :default-value="['password']"
              />
            </a-form-model-item>

            <a-form-model-item label="选项">
              <a-tooltip>
                <template slot="title"> 显示验证码,增加系统安全性 </template>
                <a-checkbox v-model="form.loginCaptcha">
                  <a-space>显示验证码<a-icon type="question-circle" /></a-space
                ></a-checkbox>
              </a-tooltip>

              <a-checkbox v-model="form.loginParticles">粒子特效</a-checkbox>
              <a-tooltip>
                <template slot="title">
                  帐号只能在一处登录,若在另外一处已登录,将会强制跳回登录界面
                </template>
                <a-checkbox v-model="form.loginSso">
                  <a-space>单点登录<a-icon type="question-circle" /></a-space
                ></a-checkbox>
              </a-tooltip>
            </a-form-model-item>
            <a-form-model-item
              label="验证码"
              v-if="form.loginCaptcha"
              prop="loginCaptchaConfig"
            >
              <a-row>
                <a-col :span="4">
                  类型：<a-select
                    style="width: 150px"
                    v-model="form.loginCaptchaType"
                  >
                    <a-select-option value="0">默认</a-select-option>
                    <a-select-option value="1">中文汉字</a-select-option>
                    <a-select-option value="2">数字</a-select-option>
                    <a-select-option value="3"
                      >中文数字（小写）</a-select-option
                    >
                    <a-select-option value="4"
                      >中文数字（大写）</a-select-option
                    >
                    <a-select-option value="5">字母大小写</a-select-option>
                    <a-select-option value="6">字母小写</a-select-option>
                    <a-select-option value="7">字母大写</a-select-option>
                    <a-select-option value="8">字母数字小写</a-select-option>
                    <a-select-option value="9">字母数字大写</a-select-option>
                    <a-select-option value="10">阿拉伯数字运算</a-select-option>
                    <a-select-option value="11">中文数字运算</a-select-option>
                  </a-select>
                </a-col>
                <a-col :span="4">
                  验证码长度：
                  <a-input-number
                    :min="1"
                    v-model="form.loginCaptchaCodeLength"
                  ></a-input-number>
                </a-col>
                <a-col :span="4">
                  忽略大小写：
                  <a-switch v-model="form.loginCaptchaIgnoreCase" />
                </a-col>
                <a-col :span="4">
                  动画：
                  <a-switch v-model="form.loginCaptchaAnimation" />
                </a-col>
                <a-col :span="4">
                  宽度：<a-input-number
                    :min="1"
                    v-model="form.loginCaptchaWidth"
                  ></a-input-number>
                </a-col>
                <a-col :span="4">
                  高度：<a-input-number
                    :min="1"
                    v-model="form.loginCaptchaHeight"
                  ></a-input-number>
                </a-col>
              </a-row>
              <a-row>
                <a-col :span="4">
                  <a-tooltip>
                    <template slot="title">
                      中文使用kaiti，其他字符可根据喜好设置（可能部分转字符会出现绘制不出的情况）。
                      当验证码类型为“ARITHMETIC”时，不要使用“Ransom”字体。（运算符和等号绘制不出来）
                      <a
                        target="_blank"
                        href="https://captcha.sunseeyou.com/index.html"
                        >样式参考</a
                      >
                    </template>
                    字体：<a-select
                      style="width: 150px"
                      v-model="form.loginCaptchaFontFamily"
                    >
                      <a-select-option value="actionj">actionj</a-select-option>
                      <a-select-option value="epilog">epilog</a-select-option>
                      <a-select-option value="fresnel">fresnel</a-select-option>
                      <a-select-option value="headache"
                        >headache</a-select-option
                      >
                      <a-select-option value="lexo">lexo</a-select-option>
                      <a-select-option value="prefix">prefix</a-select-option>
                      <a-select-option value="progbot">progbot</a-select-option>
                      <a-select-option value="ransom">ransom</a-select-option>
                      <a-select-option value="robot">robot</a-select-option>
                      <a-select-option value="scandal">scandal</a-select-option>
                      <a-select-option value="kaiti">kaiti</a-select-option>
                    </a-select>
                  </a-tooltip>
                </a-col>
                <a-col :span="4">
                  字体大小：<a-input-number
                    :min="1"
                    v-model="form.loginCaptchaFontSize"
                  ></a-input-number>
                </a-col>
                <a-col :span="4">
                  过期秒数：
                  <a-input-number
                    :min="1"
                    v-model="form.loginCaptchaExpirySeconds"
                  ></a-input-number>
                </a-col>
                <a-col :span="4">
                  存储键前缀：
                  <a-input
                    style="width: 100px"
                    v-model="form.loginCaptchaStoreageKeyPrefix"
                  ></a-input>
                </a-col>

                <a-col :span="4">
                  <div class="flex align-start justify-start">
                    <span style="width: 100px"> 背景色：</span
                    ><eip-color
                      :value="form.loginCaptchaBackgroundColor"
                      @change="
                        (value) => {
                          form.loginCaptchaBackgroundColor = value;
                        }
                      "
                    ></eip-color>
                  </div>
                </a-col>
                <a-col :span="4">
                  <div class="flex align-start justify-start">
                    <span style="width: 100px"> 前景色：</span
                    ><eip-color
                      :value="form.loginCaptchaForegroundColors"
                      @change="
                        (value) => {
                          form.loginCaptchaForegroundColors = value;
                        }
                      "
                    ></eip-color>
                  </div>
                </a-col>
              </a-row>
              <a-row>
                <a-col :span="4">
                  干扰线数量：<a-input-number
                    :min="0"
                    v-model="form.loginCaptchaInterferenceLineCount"
                  ></a-input-number>
                </a-col>
                <a-col :span="4">
                  泡泡数量：<a-input-number
                    :min="0"
                    v-model="form.loginCaptchaBubbleCount"
                  ></a-input-number>
                </a-col>
                <a-col :span="4">
                  泡泡边沿厚度：<a-input-number
                    :min="0"
                    v-model="form.loginCaptchaBubbleThickness"
                  ></a-input-number>
                </a-col>
                <a-col :span="4">
                  泡泡最小半径：<a-input-number
                    :min="0"
                    v-model="form.loginCaptchaBubbleMinRadius"
                  ></a-input-number>
                </a-col>
                <a-col :span="4">
                  泡泡最大半径：<a-input-number
                    :min="0"
                    v-model="form.loginCaptchaBubbleMaxRadius"
                  ></a-input-number>
                </a-col>
                <a-col :span="4">
                  帧间隔：<a-input-number
                    :min="0"
                    v-model="form.loginCaptchaInterferenceLineCount"
                  ></a-input-number>
                </a-col>
              </a-row>
              <a-row>
                <a-col :span="4">
                  图片质量：<a-input-number
                    :min="1"
                    v-model="form.loginCaptchaQuality"
                  ></a-input-number>
                </a-col>
                <a-col :span="4">
                  文字粗体：<a-checkbox v-model="form.loginCaptchaTextBold"
                    >文字粗体
                  </a-checkbox>
                </a-col>
              </a-row>
            </a-form-model-item>

            <a-form-model-item label="Token有效" prop="loginTokenExpires">
              超过
              <a-input-number
                :min="1"
                v-model="form.loginTokenExpires"
              ></a-input-number>
              分钟后自动失效
            </a-form-model-item>
            <a-form-model-item label="登录验证" prop="loginErrorNumberLock">
              <a-space>
                <a-tooltip>
                  <template slot="title">
                    登录错误次数锁定,选择后,超过登录次数将锁定
                  </template>
                  <a-checkbox v-model="form.loginErrorNumberLock">
                    <a-space
                      >错误次数锁定<a-icon type="question-circle" /></a-space
                  ></a-checkbox>
                </a-tooltip>
                <a-tooltip v-if="form.loginErrorNumberLock">
                  <template slot="title"> 超过登录次数将锁定 </template>
                  ，超过
                  <a-input-number
                    :min="1"
                    v-model="form.loginErrorNumber"
                  ></a-input-number>
                  次将锁定
                </a-tooltip>
                <a-tooltip v-if="form.loginErrorNumberLock">
                  <template slot="title">
                    超过登录次数将锁定,锁定多少分钟后才能重新登录，若不填写则永久锁住，需要联系管理员删除锁定
                  </template>
                  ，锁定
                  <a-input-number
                    :min="1"
                    v-model="form.loginErrorNumberLockMinute"
                  ></a-input-number>
                  分钟才能重新登录
                </a-tooltip></a-space
              >
            </a-form-model-item>

            <a-form-model-item label="背景图" prop="loginBg">
              <a-upload-dragger
                :show-upload-list="false"
                name="file"
                accept=".png,.jpg,.jpeg,.gif"
                :multiple="false"
                :customRequest="uploadLoginBgRequest"
              >
                <img
                  v-if="form.loginBg"
                  :src="form.loginBg"
                  style="width: 100%; height: 130px"
                />
                <div v-else>
                  <p class="ant-upload-drag-icon">
                    <a-icon type="inbox" />
                  </p>
                  <p class="ant-upload-text">单击或拖动文件到此区域进行上传</p>
                </div>
              </a-upload-dragger>

              <a-button key="submit" @click="form.loginBg = ''" type="danger"
                ><a-icon type="delete" />清空</a-button
              >
            </a-form-model-item>

            <a-form-model-item label="提示信息" prop="loginTip">
              <eip-editor
                ref="editor"
                v-if="tinymce.show"
                v-model="form.loginTip"
                :height="tinymce.height"
              >
              </eip-editor>
            </a-form-model-item>
          </a-tab-pane>
          <a-tab-pane key="2" tab="系统配置">
            <a-form-model-item label="系统全称" prop="systemTitle">
              <a-input
                v-model="form.systemTitle"
                placeholder="请输入系统全称:如EIP权限工作流平台"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="系统简称" prop="systemShortName">
              <a-input
                v-model="form.systemShortName"
                placeholder="请输入系统简称:折叠后显示名称"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="最长未操时间" prop="systemMaxDoCheckTime">
              超过
              <a-input-number v-model="form.systemMaxDoCheckTime" />
              分钟后，系统将强制登录
            </a-form-model-item>

            <a-form-model-item label="密码安全">
              <span
                >最少
                <a-input-number v-model="form.systemPasswordLength" :min="1" />
                个字符串 </span
              >，
              <span
                >超过
                <a-input-number
                  v-model="form.systemPasswordCompelChangeDay"
                  :min="1"
                />
                天强制修改密码
              </span>
              <br />
              <a-checkbox v-model="form.systemPasswordHaveNumber">
                数字
              </a-checkbox>
              <span v-if="form.systemPasswordHaveNumber"
                >， 最少长度:
                <a-input-number
                  v-model="form.systemPasswordHaveNumberLength"
                  :min="1"
                />
                个字符串
              </span>
              <br />
              <a-checkbox v-model="form.systemPasswordHaveLetters">
                字母
              </a-checkbox>
              <span v-if="form.systemPasswordHaveLetters">
                ， 最少长度:
                <a-input-number
                  v-model="form.systemPasswordHaveLettersLength"
                  :min="1"
                />
                个字符串
              </span>
              <br />
              <a-checkbox v-model="form.systemPasswordHaveSpecialCharacters">
                特殊字符
              </a-checkbox>
              <span v-if="form.systemPasswordHaveSpecialCharacters">
                ， 最少长度:
                <a-input-number
                  v-model="form.systemPasswordHaveSpecialCharactersLength"
                  :min="1"
                />
                个字符串
              </span>
            </a-form-model-item>

            <a-form-model-item label="选项">
              <a-tooltip>
                <template slot="title">
                  除开登录界面将姓名作为数字水印
                </template>
                <a-checkbox v-model="form.systemWaterMark">
                  <a-space>水印<a-icon type="question-circle" /></a-space
                ></a-checkbox>
              </a-tooltip>
            </a-form-model-item>
            <a-form-model-item label="系统域名" prop="systemDomain">
              <a-input
                v-model="form.systemDomain"
                placeholder="请输入系统域名:http://demo.eipflow.com"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="系统接口地址" prop="systemDomainApi">
              <a-input
                v-model="form.systemDomainApi"
                placeholder="请输入系统接口地址:http://api.eipflow.com"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="系统Logo" prop="systemLogo">
              <a-upload
                name="avatar"
                list-type="picture-card"
                class="avatar-uploader"
                accept=".png,.jpg,.jpeg,.gif"
                :multiple="false"
                :show-upload-list="false"
                :customRequest="uploadSystemLogoRequest"
              >
                <img
                  style="width: 100px; height: 100px"
                  v-if="form.systemLogo"
                  :src="form.systemLogo"
                  alt="avatar"
                />
                <div v-else>
                  <a-icon :type="loading ? 'loading' : 'plus'" />
                </div>
              </a-upload>
              <a-button key="submit" @click="form.systemLogo = ''" type="danger"
                ><a-icon type="delete" />清空</a-button
              >
            </a-form-model-item>
          </a-tab-pane>
          <a-tab-pane key="3" tab="邮箱">
            <a-form-model-item label="SMTP服务器" prop="emailSmtpServer">
              <a-input
                v-model="form.emailSmtpServer"
                placeholder="请输入SMTP服务器"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="SMTP端口" prop="emailSmtpServerPort">
              <a-input
                v-model="form.emailSmtpServerPort"
                placeholder="请输入SMTP服务端口号:如25"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="SMTP-SSL" prop="emailSmtpSsl">
              <a-switch v-model="form.emailSmtpSsl" />
            </a-form-model-item>
            <a-form-model-item label="邮箱登录名" prop="emailLoginName">
              <a-input
                v-model="form.emailLoginName"
                placeholder="请输入邮箱登录名"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="邮箱密码" prop="emailLoginPassword">
              <a-input
                type="password"
                v-model="form.emailLoginPassword"
                placeholder="请输入邮箱密码"
                allow-clear
              />
            </a-form-model-item>
          </a-tab-pane>
          <a-tab-pane key="4" tab="三方帐号">
            <a-tabs default-active-key="1">
              <a-tab-pane key="1" tab="企业微信">
                <a-form-model-item
                  label="企业ID"
                  extra="https://work.weixin.qq.com/wework_admin/frame#profile"
                  prop="wechatWorkCorpId"
                >
                  <a-input
                    v-model="form.wechatWorkCorpId"
                    placeholder="请输入企业ID"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item
                  label="AgentId"
                  extra="https://work.weixin.qq.com/wework_admin/frame#apps 下建立应用后得到"
                  prop="wechatWorkAgentId"
                >
                  <a-input
                    v-model="form.wechatWorkAgentId"
                    placeholder="请输入AgentId"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item label="Secret" prop="wechatWorkCorpSecret">
                  <a-input-password
                    v-model="form.wechatWorkCorpSecret"
                    placeholder="请输入Secret"
                    allow-clear
                  />
                </a-form-model-item>

                <a-form-model-item label="Token" prop="wechatWorkCorpToken">
                  <a-input
                    v-model="form.wechatWorkCorpToken"
                    placeholder="请输入Token"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item
                  label="EncodingAESKey"
                  prop="wechatWorkCorpEncodingAESKey"
                >
                  <a-input
                    v-model="form.wechatWorkCorpEncodingAESKey"
                    placeholder="请输入EncodingAESKey"
                    allow-clear
                  />
                </a-form-model-item>
              </a-tab-pane>
              <a-tab-pane key="2" tab="微信公众号">
                <a-form-model-item
                  label="AppId"
                  extra="https://mp.weixin.qq.com/advanced/advanced?action=dev&t=advanced/dev&token=214649512&lang=zh_CN"
                  prop="wechatMpAppId"
                >
                  <a-input
                    v-model="form.wechatMpAppId"
                    placeholder="请输入AppId"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item label="AppSecret" prop="wechatMpAppSecret">
                  <a-input
                    v-model="form.wechatMpAppSecret"
                    placeholder="请输入AppSecret"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item label="Token" prop="wechatMpToken">
                  <a-input
                    v-model="form.wechatMpToken"
                    placeholder="请输入AppId"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item
                  label="EncodingAESKey"
                  prop="wechatMpEncodingAESKey"
                >
                  <a-input
                    v-model="form.wechatMpEncodingAESKey"
                    placeholder="请输入AppId"
                    allow-clear
                  />
                </a-form-model-item>
              </a-tab-pane>

              <a-tab-pane key="3" tab="微信小程序">
                <a-form-model-item
                  label="AppId"
                  extra="https://mp.weixin.qq.com/wxamp/devprofile/get_profile?token=1022523917&lang=zh_CN"
                  prop="wechatOpenAppId"
                >
                  <a-input
                    v-model="form.wechatOpenAppId"
                    placeholder="请输入AppId"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item label="AppSecret" prop="wechatOpenAppSecret">
                  <a-input
                    v-model="form.wechatOpenAppSecret"
                    placeholder="请输入AppSecret"
                    allow-clear
                  />
                </a-form-model-item>

                <a-form-model-item label="Token" prop="wechatOpenToken">
                  <a-input
                    v-model="form.wechatOpenToken"
                    placeholder="请输入Token"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item
                  label="EncodingAESKey"
                  prop="wechatOpenEncodingAESKey"
                >
                  <a-input
                    v-model="form.wechatOpenEncodingAESKey"
                    placeholder="请输入EncodingAESKey"
                    allow-clear
                  />
                </a-form-model-item>
              </a-tab-pane>

              <a-tab-pane key="4" tab="钉钉">
                <a-form-model-item
                  label="CorpId"
                  extra="https://open-dev.dingtalk.com/?spm=dd_developers.header.unLogin.openDevBtn#/"
                  prop="dingTalkCorpId"
                >
                  <a-input
                    v-model="form.dingTalkCorpId"
                    placeholder="请输入CorpId"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item label="ApiToken" prop="dingTalkApiToken">
                  <a-input
                    v-model="form.dingTalkApiToken"
                    placeholder="请输入ApiToken"
                    allow-clear
                  />
                </a-form-model-item>

                <a-form-model-item
                  label="AgentId"
                  extra="https://open-dev.dingtalk.com/fe/app#/corp/app 应用消息"
                  prop="dingTalkAgentId"
                >
                  <a-input
                    v-model="form.dingTalkAgentId"
                    placeholder="请输入AgentId"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item label="AppKey" prop="dingTalkAppKey">
                  <a-input
                    v-model="form.dingTalkAppKey"
                    placeholder="请输入AppKey"
                    allow-clear
                  />
                </a-form-model-item>
                <a-form-model-item label="AppSecret" prop="dingTalkAppSecret">
                  <a-input-password
                    v-model="form.dingTalkAppSecret"
                    placeholder="请输入AppSecret"
                    allow-clear
                  />
                </a-form-model-item>

                <a-form-model-item
                  label="EncodingAesKey"
                  extra="https://open-dev.dingtalk.com/fe/app#/corp/app 事件与回调"
                  prop="dingTalkEncodingAesKey"
                >
                  <a-input-password
                    v-model="form.dingTalkEncodingAesKey"
                    placeholder="请输入EncodingAesKey"
                    allow-clear
                  />
                </a-form-model-item>

                <a-form-model-item label="Token" prop="dingTalkToken">
                  <a-input-password
                    v-model="form.dingTalkToken"
                    placeholder="请输入Token"
                    allow-clear
                  />
                </a-form-model-item>
              </a-tab-pane>
            </a-tabs>
          </a-tab-pane>
          <a-tab-pane key="5" tab="附件设置">
            <a-form-model-item label="存储方式" prop="filesStorageType">
              <a-radio-group
                v-model="form.filesStorageType"
                name="filesStorageType"
              >
                <a-radio value="LocalStorage"> 本地存储 </a-radio>
                <a-radio value="AliYunOSS"> 阿里云OSS </a-radio>
                <a-radio value="QCloudOSS"> 腾讯云COS </a-radio>
                <a-radio value="QiNiuKoDo"> 七牛云KoDo </a-radio>
              </a-radio-group>
            </a-form-model-item>
            <a-form-model-item label="存储路径" prop="filesStoragePath">
              <a-input
                v-model="form.filesStoragePath"
                placeholder="请使用路径，如/upload/doc/，路径前后都要加/（七牛云为key/value结构不支持）"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="文件后缀类型" prop="文件后缀类型">
              <a-input
                v-model="form.filesStorageFileSuffix"
                placeholder="使用小写逗号分割"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="文件最大M（兆）" prop="文件最大M（兆）">
              <a-input-number
                style="width: 100%"
                v-model="form.filesStorageFileMaxSize"
                placeholder="一般10M即可，超过网络容易缓慢"
              />
            </a-form-model-item>
            <a-form-model-item
              label="绑定域名"
              prop="filesStorageBucketBindUrl"
            >
              <a-input
                v-model="form.filesStorageBucketBindUrl"
                placeholder="独立绑定的域名最好，也可以用云存储提供的多级域名"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item
              v-if="form.filesStorageType != 'LocalStorage'"
              label="云存储授权账户"
              prop="filesStorageAccessKeyId"
            >
              <a-input
                v-model="form.filesStorageAccessKeyId"
                placeholder="请输入云存储授权账户"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item
              v-if="form.filesStorageType != 'LocalStorage'"
              label="云存储授权密钥"
              prop="filesStorageAccessKeySecret"
            >
              <a-input
                type="password"
                v-model="form.filesStorageAccessKeySecret"
                placeholder="请输入云存储授权密钥"
                allow-clear
              />
            </a-form-model-item>

            <a-form-model-item
              v-if="form.filesStorageType == 'AliYunOSS'"
              label="阿里云节点"
              prop="filesStorageAliYunEndpoint"
            >
              <a-input
                v-model="form.filesStorageAliYunEndpoint"
                placeholder="请输入阿里云节点"
                allow-clear
              />
            </a-form-model-item>

            <a-form-model-item
              v-if="form.filesStorageType == 'AliYunOSS'"
              label="阿里云桶名称"
              prop="filesStorageAliYunBucketName"
            >
              <a-input
                v-model="form.filesStorageAliYunBucketName"
                placeholder="请输入阿里云桶名称"
                allow-clear
              />
            </a-form-model-item>

            <a-form-model-item
              v-if="form.filesStorageType == 'QCloudOSS'"
              label="腾讯云账户标识"
              prop="filesStorageTencentAccountId"
            >
              <a-input
                v-model="form.filesStorageTencentAccountId"
                placeholder="请输入腾讯云账户标识"
                allow-clear
              />
            </a-form-model-item>

            <a-form-model-item
              v-if="form.filesStorageType == 'QCloudOSS'"
              label="腾讯云桶地域"
              prop="filesStorageTencentCosRegion"
            >
              <a-input
                v-model="form.filesStorageTencentCosRegion"
                placeholder="请输入腾讯云桶地域"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item
              v-if="form.filesStorageType == 3"
              label="腾讯云桶名称"
              prop="filesStorageTencentBucketName"
            >
              <a-input
                v-model="form.filesStorageTencentBucketName"
                placeholder="请输入腾讯云桶名称"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item
              v-if="form.filesStorageType == 'QiNiuKoDo'"
              label="七牛云桶名称"
              prop="filesStorageQiNiuBucketName"
            >
              <a-input
                v-model="form.filesStorageQiNiuBucketName"
                placeholder="请输入七牛云桶名称"
                allow-clear
              />
            </a-form-model-item>
          </a-tab-pane>
          <a-tab-pane key="6" tab="其他">
            <a-divider orientation="left">代码生成</a-divider>
            <a-form-model-item
              label="代码生成地址"
              prop="systemCodeGenerationPath"
            >
              <a-input
                v-model="form.systemCodeGenerationPath"
                placeholder="请输入代码生成地址:G:\\代码\\EIP_Api_V2\\src"
                allow-clear
              />
            </a-form-model-item>
            <a-divider orientation="left">物流快递</a-divider>
            <a-form-model-item
              label="AppId"
              prop="expressAppId"
              extra="申请地址：https://www.showapi.com/apiGateway/view/64/20#tabs"
            >
              <a-input
                v-model="form.expressAppId"
                placeholder="请输入AppId"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="Secret" prop="expressSecret">
              <a-input
                type="password"
                v-model="form.expressSecret"
                placeholder="请输入Secret"
                allow-clear
              />
            </a-form-model-item>
          </a-tab-pane>
        </a-tabs>
      </a-form-model>
      <div class="flex justify-center align-center">
        <a-space>
          <a-button
            key="submit"
            @click="submit"
            type="primary"
            :loading="loading"
            ><a-icon type="save" />提交</a-button
          >
        </a-space>
      </div>
    </a-card>
  </a-spin>
</template>

<script>
import {
  save,
  query,
  uploadLoginBg,
  uploadSystemLogo,
} from "@/services/system/config/index";
export default {
  name: "edit",
  data() {
    return {
      height: window.innerHeight - 100 + "px",
      tinymce: {
        plugins: "preview fullscreen code",
        toolbar: "undo redo |fullscreen|code",
        height: 200,
        show: false,
        menubar: "",
      },
      loginTypeOptions: [
        { label: "帐号密码", value: "password" },
        { label: "短信验证码", value: "verificationCode" },
        { label: "钉钉扫码", value: "dingtalkScan" },
        { label: "企业微信扫码", value: "wxWorkScan" },
      ],
      config: {
        labelCol: { span: 2 },
        wrapperCol: { span: 22 },
      },

      form: {
        loginTitle: "",
        loginSystemName: "",
        loginType: ["password"],
        loginCaptcha: false,
        loginParticles: false,
        loginSso: false,
        loginErrorNumberLock: true,
        loginErrorNumber: 5,
        loginErrorNumberLockMinute: 5,
        loginTokenExpires: 10,
        loginBg: "",
        loginTip: "",
        loginCopyright: "",

        loginCaptchaType: 0,
        loginCaptchaCodeLength: 4,
        loginCaptchaExpirySeconds: 60,
        loginCaptchaStoreageKeyPrefix: null,
        loginCaptchaIgnoreCase: true,
        loginCaptchaAnimation: false,
        loginCaptchaFontSize: 32,
        loginCaptchaWidth: 100,
        loginCaptchaHeight: 40,
        loginCaptchaBubbleMinRadius: 5,
        loginCaptchaBubbleMaxRadius: 10,
        loginCaptchaBubbleCount: 3,
        loginCaptchaBubbleThickness: 1.0,
        loginCaptchaInterferenceLineCount: 3,
        loginCaptchaFontFamily: "kaiti",
        loginCaptchaFrameDelay: 15,
        loginCaptchaBackgroundColor: "#ffffff",
        loginCaptchaForegroundColors: "",
        loginCaptchaQuality: 100,
        loginCaptchaTextBold: false,

        systemTitle: "",
        systemShortName: "",
        systemLogo: "",
        systemWaterMark: "",

        systemPasswordLength: 4,
        systemPasswordCompelChangeDay: null,
        systemPasswordHaveNumber: false,
        systemPasswordHaveNumberLength: 4,
        systemPasswordHaveLetters: false,
        systemPasswordHaveLettersLength: 4,
        systemPasswordHaveSpecialCharacters: false,
        systemPasswordHaveSpecialCharactersLength: 4,

        systemMaxDoCheckTime: 10,
        systemDomain: "",
        systemDomainApi: "",
        systemCodeGenerationPath: "",

        emailSmtpServer: "",
        emailSmtpServerPort: "",
        emailSmtpSsl: true,
        emailLoginName: "",
        emailLoginPassword: "",

        wechatWorkCorpId: "",
        wechatWorkAgentId: "",
        wechatWorkCorpSecret: "",
        wechatWorkCorpToken: null,
        wechatWorkCorpEncodingAESKey: null,

        wechatMpAppId: "",
        wechatMpAppSecret: "",
        wechatMpToken: null,
        wechatMpEncodingAESKey: null,

        wechatOpenAppId: "",
        wechatOpenAppSecret: "",
        wechatOpenToken: null,
        wechatOpenEncodingAESKey: null,

        dingTalkCorpId: "",
        dingTalkApiToken: "",
        dingTalkAgentId: "",
        dingTalkAppKey: "",
        dingTalkAppSecret: "",
        dingTalkEncodingAesKey: "",
        dingTalkToken: "",

        filesStorageType: "LocalStorage",
        filesStorageFileSuffix:
          "gif,jpg,jpeg,png,bmp,xls,xlsx,doc,docx,pdf,mp4,WebM,Ogv",
        filesStorageFileMaxSize: "10",
        filesStoragePath: "",
        filesStorageBucketBindUrl: "",
        filesStorageAccessKeyId: "",
        filesStorageAccessKeySecret: "",
        filesStorageAliYunEndpoint: "",
        filesStorageAliYunBucketName: "",
        filesStorageTencentAccountId: "",
        filesStorageTencentCosRegion: "",
        filesStorageQiNiuBucketName: "",

        expressAppId: "",
        expressSecret: "",
      },

      rules: {},

      loading: false,
      spinning: false,
    };
  },

  mounted() {
    setTimeout(() => {
      this.tinymce.show = true;
    }, 10);

    this.find();
  },
  methods: {
    /**
     *
     */
    uploadLoginBgRequest(file) {
      let that = this;
      const formData = new FormData();
      formData.append("Files", file.file);
      that.$message.loading({
        content: "上传中...",
        duration: 0,
      });
      uploadLoginBg(formData)
        .then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.form.loginBg = result.data.url;
          } else {
            that.$message.error(result.msg);
          }
        })
        .catch(() => {
          that.$message.error("上传出错");
        });
    },

    /**
     *
     * @param {*} file
     */
    uploadSystemLogoRequest(file) {
      let that = this;
      const formData = new FormData();
      formData.append("Files", file.file);
      that.$message.loading({
        content: "上传中...",
        duration: 0,
      });
      uploadSystemLogo(formData)
        .then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.form.systemLogo = result.data.url;
          } else {
            that.$message.error(result.msg);
          }
        })
        .catch(() => {
          that.$message.error("上传出错");
        });
    },

    /**
     * 保存
     */
    submit() {
      let that = this;
      that.$message.loading({
        content: "保存中...",
        duration: 0,
      });
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          var configs = [];
          for (let key in that.form) {
            if (key == "loginType") {
              configs.push({
                key: "loginType",
                value: JSON.stringify(that.form.loginType),
              });
            } else {
              configs.push({ key: key, value: that.form[key] });
            }
          }
          save({
            Configs: configs,
          }).then(function (result) {
            that.$message.destroy();
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
            } else {
              that.$message.error(result.msg);
            }
            that.loading = false;
          });
        } else {
          return false;
        }
      });
    },

    /**
     * 根据Id查找
     */
    find() {
      let that = this;
      that.spinning = true;
      query().then(function (result) {
        if (
          result.code === that.eipResultCode.success &&
          result.data.length > 0
        ) {
          result.data.forEach((item) => {
            if (item.key == "loginType") {
              that.form["loginType"] = JSON.parse(
                item.value ? item.value : "[]"
              );
            } else {
              that.form[item.key] = item.value
                ? ["true", "false"].includes(item.value)
                  ? item.value == "true"
                  : item.value
                : undefined;
            }
          });
        }
        that.spinning = false;
      });
    },
  },
};
</script>

<style lang="less">
.edui-default {
  line-height: 20px !important;
}

.avatar-uploader > .ant-upload {
  width: 128px;
  height: 128px;
}

.ant-upload-select-picture-card i {
  font-size: 32px;
  color: #999;
}

.ant-upload-select-picture-card .ant-upload-text {
  margin-top: 8px;
  color: #666;
}
</style>
