<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-form-model-item label="短信配置" prop="smsConfigId">
          <a-select placeholder="请选择短信配置" allow-clear v-model="form.smsConfigId">
            <a-select-option v-for="(item, index) in smsConfigIds" :key="index" :value="item.smsConfigId"> {{ item.name
              }}
            </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="模板名称" prop="name">
          <a-input v-model="form.name" placeholder="请输入模板名称" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="系统代码" prop="code">
          <a-input v-model="form.code" placeholder="请输入系统代码,系统内使用根据此代码获取供应商提供的代码" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="模板代码" prop="templateId">
          <a-input v-model="form.templateId" placeholder="请输入短信代码,运营商平台审核后提供" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="签名" prop="sign">
          <a-input v-model="form.sign" placeholder="请输入签名" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="模板内容" prop="template">
          <a-input v-model="form.template" placeholder="请输入模板内容" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="短信类型" prop="type">
          <a-select placeholder="请选择短信类型" allow-clear v-model="form.type">
            <a-select-option :value="0"> 普通短信 </a-select-option>
            <a-select-option :value="1"> 营销短信 </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="国家码" prop="nationCode">
          <a-select placeholder="请选择国家码" allow-clear v-model="form.nationCode">
            <a-select-option value="86">中国</a-select-option>
            <a-select-option value="244">安哥拉</a-select-option>
            <a-select-option value="93">阿富汗</a-select-option>
            <a-select-option value="355">阿尔巴尼亚</a-select-option>
            <a-select-option value="213">阿尔及利亚</a-select-option>
            <a-select-option value="376">安道尔共和国</a-select-option>
            <a-select-option value="1264">安圭拉岛</a-select-option>
            <a-select-option value="1268">安提瓜和巴布达</a-select-option>
            <a-select-option value="54">阿根廷</a-select-option>
            <a-select-option value="374">亚美尼亚</a-select-option>
            <a-select-option value="247">阿森松</a-select-option>
            <a-select-option value="61">澳大利亚</a-select-option>
            <a-select-option value="43">奥地利</a-select-option>
            <a-select-option value="994">阿塞拜疆</a-select-option>
            <a-select-option value="1242">巴哈马</a-select-option>
            <a-select-option value="973">巴林</a-select-option>
            <a-select-option value="880">孟加拉国</a-select-option>
            <a-select-option value="1246">巴巴多斯</a-select-option>
            <a-select-option value="375">白俄罗斯</a-select-option>
            <a-select-option value="32">比利时</a-select-option>
            <a-select-option value="501">伯利兹</a-select-option>
            <a-select-option value="229">贝宁</a-select-option>
            <a-select-option value="1441">百慕大群岛</a-select-option>
            <a-select-option value="591">玻利维亚</a-select-option>
            <a-select-option value="267">博茨瓦纳</a-select-option>
            <a-select-option value="55">巴西</a-select-option>
            <a-select-option value="673">文莱</a-select-option>
            <a-select-option value="359">保加利亚</a-select-option>
            <a-select-option value="226">布基纳法索</a-select-option>
            <a-select-option value="95">缅甸</a-select-option>
            <a-select-option value="257">布隆迪</a-select-option>
            <a-select-option value="237">喀麦隆</a-select-option>
            <a-select-option value="1">加拿大</a-select-option>
            <a-select-option value="1345">开曼群岛</a-select-option>
            <a-select-option value="236">中非共和国</a-select-option>
            <a-select-option value="235">乍得</a-select-option>
            <a-select-option value="56">智利</a-select-option>
            <a-select-option value="57">哥伦比亚</a-select-option>
            <a-select-option value="242">刚果</a-select-option>
            <a-select-option value="682">库克群岛</a-select-option>
            <a-select-option value="506">哥斯达黎加</a-select-option>
            <a-select-option value="53">古巴</a-select-option>
            <a-select-option value="357">塞浦路斯</a-select-option>
            <a-select-option value="420">捷克</a-select-option>
            <a-select-option value="45">丹麦</a-select-option>
            <a-select-option value="253">吉布提</a-select-option>
            <a-select-option value="1890">多米尼加共和国</a-select-option>
            <a-select-option value="593">厄瓜多尔</a-select-option>
            <a-select-option value="20">埃及</a-select-option>
            <a-select-option value="503">萨尔瓦多</a-select-option>
            <a-select-option value="372">爱沙尼亚</a-select-option>
            <a-select-option value="251">埃塞俄比亚</a-select-option>
            <a-select-option value="679">斐济</a-select-option>
            <a-select-option value="358">芬兰</a-select-option>
            <a-select-option value="33">法国</a-select-option>
            <a-select-option value="594">法属圭亚那</a-select-option>
            <a-select-option value="241">加蓬</a-select-option>
            <a-select-option value="220">冈比亚</a-select-option>
            <a-select-option value="995">格鲁吉亚</a-select-option>
            <a-select-option value="49">德国</a-select-option>
            <a-select-option value="233">加纳</a-select-option>
            <a-select-option value="350">直布罗陀</a-select-option>
            <a-select-option value="30">希腊</a-select-option>
            <a-select-option value="1809">格林纳达</a-select-option>
            <a-select-option value="1671">关岛</a-select-option>
            <a-select-option value="502">危地马拉</a-select-option>
            <a-select-option value="224">几内亚</a-select-option>
            <a-select-option value="592">圭亚那</a-select-option>
            <a-select-option value="509">海地</a-select-option>
            <a-select-option value="504">洪都拉斯</a-select-option>
            <a-select-option value="852">香港</a-select-option>
            <a-select-option value="36">匈牙利</a-select-option>
            <a-select-option value="354">冰岛</a-select-option>
            <a-select-option value="91">印度</a-select-option>
            <a-select-option value="62">印度尼西亚</a-select-option>
            <a-select-option value="98">伊朗</a-select-option>
            <a-select-option value="964">伊拉克</a-select-option>
            <a-select-option value="353">爱尔兰</a-select-option>
            <a-select-option value="972">以色列</a-select-option>
            <a-select-option value="39">意大利</a-select-option>
            <a-select-option value="225">科特迪瓦</a-select-option>
            <a-select-option value="1876">牙买加</a-select-option>
            <a-select-option value="81">日本</a-select-option>
            <a-select-option value="962">约旦</a-select-option>
            <a-select-option value="855">柬埔寨</a-select-option>
            <a-select-option value="327">哈萨克斯坦</a-select-option>
            <a-select-option value="254">肯尼亚</a-select-option>
            <a-select-option value="82">韩国</a-select-option>
            <a-select-option value="965">科威特</a-select-option>
            <a-select-option value="331">吉尔吉斯坦</a-select-option>
            <a-select-option value="856">老挝</a-select-option>
            <a-select-option value="371">拉脱维亚</a-select-option>
            <a-select-option value="961">黎巴嫩</a-select-option>
            <a-select-option value="266">莱索托</a-select-option>
            <a-select-option value="231">利比里亚</a-select-option>
            <a-select-option value="218">利比亚</a-select-option>
            <a-select-option value="423">列支敦士登</a-select-option>
            <a-select-option value="370">立陶宛</a-select-option>
            <a-select-option value="352">卢森堡</a-select-option>
            <a-select-option value="853">澳门</a-select-option>
            <a-select-option value="261">马达加斯加</a-select-option>
            <a-select-option value="265">马拉维</a-select-option>
            <a-select-option value="60">马来西亚</a-select-option>
            <a-select-option value="960">马尔代夫</a-select-option>
            <a-select-option value="223">马里</a-select-option>
            <a-select-option value="356">马耳他</a-select-option>
            <a-select-option value="1670">马里亚那群岛</a-select-option>
            <a-select-option value="596">马提尼克</a-select-option>
            <a-select-option value="230">毛里求斯</a-select-option>
            <a-select-option value="52">墨西哥</a-select-option>
            <a-select-option value="373">摩尔多瓦</a-select-option>
            <a-select-option value="377">摩纳哥</a-select-option>
            <a-select-option value="976">蒙古</a-select-option>
            <a-select-option value="1664">蒙特塞拉特岛</a-select-option>
            <a-select-option value="212">摩洛哥</a-select-option>
            <a-select-option value="258">莫桑比克</a-select-option>
            <a-select-option value="264">纳米比亚</a-select-option>
            <a-select-option value="674">瑙鲁</a-select-option>
            <a-select-option value="977">尼泊尔</a-select-option>
            <a-select-option value="599">荷属安的列斯</a-select-option>
            <a-select-option value="31">荷兰</a-select-option>
            <a-select-option value="64">新西兰</a-select-option>
            <a-select-option value="505">尼加拉瓜</a-select-option>
            <a-select-option value="227">尼日尔</a-select-option>
            <a-select-option value="234">尼日利亚</a-select-option>
            <a-select-option value="850">朝鲜</a-select-option>
            <a-select-option value="47">挪威</a-select-option>
            <a-select-option value="968">阿曼</a-select-option>
            <a-select-option value="92">巴基斯坦</a-select-option>
            <a-select-option value="507">巴拿马</a-select-option>
            <a-select-option value="675">巴布亚新几内亚</a-select-option>
            <a-select-option value="595">巴拉圭</a-select-option>
            <a-select-option value="51">秘鲁</a-select-option>
            <a-select-option value="63">菲律宾</a-select-option>
            <a-select-option value="48">波兰</a-select-option>
            <a-select-option value="689">法属玻利尼西亚</a-select-option>
            <a-select-option value="351">葡萄牙</a-select-option>
            <a-select-option value="1787">波多黎各</a-select-option>
            <a-select-option value="974">卡塔尔</a-select-option>
            <a-select-option value="262">留尼旺</a-select-option>
            <a-select-option value="40">罗马尼亚</a-select-option>
            <a-select-option value="7">俄罗斯</a-select-option>
            <a-select-option value="1758">圣卢西亚</a-select-option>
            <a-select-option value="1784">圣文森特岛</a-select-option>
            <a-select-option value="684">东萨摩亚(美)</a-select-option>
            <a-select-option value="685">西萨摩亚</a-select-option>
            <a-select-option value="378">圣马力诺</a-select-option>
            <a-select-option value="239">圣多美和普林西比</a-select-option>
            <a-select-option value="966">沙特阿拉伯</a-select-option>
            <a-select-option value="221">塞内加尔</a-select-option>
            <a-select-option value="248">塞舌尔</a-select-option>
            <a-select-option value="232">塞拉利昂</a-select-option>
            <a-select-option value="65">新加坡</a-select-option>
            <a-select-option value="421">斯洛伐克</a-select-option>
            <a-select-option value="386">斯洛文尼亚</a-select-option>
            <a-select-option value="677">所罗门群岛</a-select-option>
            <a-select-option value="252">索马里</a-select-option>
            <a-select-option value="27">南非</a-select-option>
            <a-select-option value="34">西班牙</a-select-option>
            <a-select-option value="94">斯里兰卡</a-select-option>
            <a-select-option value="1758">圣卢西亚</a-select-option>
            <a-select-option value="1784">圣文森特</a-select-option>
            <a-select-option value="249">苏丹</a-select-option>
            <a-select-option value="597">苏里南</a-select-option>
            <a-select-option value="268">斯威士兰</a-select-option>
            <a-select-option value="46">瑞典</a-select-option>
            <a-select-option value="41">瑞士</a-select-option>
            <a-select-option value="963">叙利亚</a-select-option>
            <a-select-option value="886">台湾省</a-select-option>
            <a-select-option value="992">塔吉克斯坦</a-select-option>
            <a-select-option value="255">坦桑尼亚</a-select-option>
            <a-select-option value="66">泰国</a-select-option>
            <a-select-option value="228">多哥</a-select-option>
            <a-select-option value="676">汤加</a-select-option>
            <a-select-option value="1809">特立尼达和多巴哥</a-select-option>
            <a-select-option value="216">突尼斯</a-select-option>
            <a-select-option value="90">土耳其</a-select-option>
            <a-select-option value="993">土库曼斯坦</a-select-option>
            <a-select-option value="256">乌干达</a-select-option>
            <a-select-option value="380">乌克兰</a-select-option>
            <a-select-option value="971">阿拉伯联合酋长国</a-select-option>
            <a-select-option value="44">英国</a-select-option>
            <a-select-option value="1">美国</a-select-option>
            <a-select-option value="598">乌拉圭</a-select-option>
            <a-select-option value="233">乌兹别克斯坦</a-select-option>
            <a-select-option value="58">委内瑞拉</a-select-option>
            <a-select-option value="84">越南</a-select-option>
            <a-select-option value="967">也门</a-select-option>
            <a-select-option value="381">南斯拉夫</a-select-option>
            <a-select-option value="263">津巴布韦</a-select-option>
            <a-select-option value="243">扎伊尔</a-select-option>
            <a-select-option value="260">赞比亚</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="备注" prop="remark">
          <a-input allow-clear v-model="form.remark" type="textarea" placeholder="请输入备注" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!smsTemplateId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button v-if="!smsTemplateId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="smsTemplateId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
  </a-modal>
</template>

<script>
import { save, findById } from "@/services/system/smstemplate/edit";
import { newGuid } from "@/utils/util";
import { query } from "@/services/system/smsconfig/list";
export default {
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        smsTemplateId: newGuid(),
        smsConfigId: undefined,
        name: null,
        code: null,
        templateId: null,
        sign: null,
        template: null,
        type: 0,
        nationCode: '86',
        remark: null,
      },

      rules: {
        name: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],
        smsConfigId: [
          {
            required: true,
            message: "请选择短信服务商配置",
            trigger: "blur",
          },
        ],
        code: [
          {
            required: true,
            message: "请输入系统代码",
            trigger: "blur",
          },
        ],
        templateId: [
          {
            required: true,
            message: "请输入短信代码",
            trigger: "blur",
          },
        ],
      },

      loading: false,
      spinning: false,
      save: {
        continue: false,
        retain: false,
      },

      smsConfigIds: []
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    smsTemplateId: {
      type: String,
    },
    title: {
      type: String,
    },
    copy: {
      type: Boolean,
      default: false,
    },
  },
  mounted() {
    this.smsConfigInit();
    this.find();
  },
  methods: {
    /**
  * 
  */
    smsConfigInit() {
      let that = this;
      query({
        current: 1,
        size: 200
      }).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.smsConfigIds = result.data;
        }
      });
    },
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },
    /**
     * 保存
     */
    saveConfirm() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          that.form.smsTemplateId = that.copy ? newGuid() : that.form.smsTemplateId;
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.smsTemplateId = newGuid();
              } else {
                that.cancel();
              }
              that.$emit("ok", result);
            } else {
              that.$message.error(result.msg);
            }
            that.loading = false;
            that.$loading.hide();
          });
        } else {
          return false;
        }
      });
    },
    /**
     *
     */
    saveContinue() {
      this.save.continue = true;
      this.saveConfirm();
    },

    /**
     *
     */
    saveClose() {
      this.save.continue = false;
      this.saveConfirm();
    },
    /**
     * 根据Id查找
     */
    find() {
      let that = this;
      if (this.smsTemplateId) {
        that.spinning = true;
        findById(this.smsTemplateId).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            that.form = result.data;
          } else {
            that.$message.error(result.msg);
          }
          that.spinning = false;
        });
      }
    },
  },
};
</script>