<template>
  <a-modal width="700px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">

        <a-form-model-item label="名称" prop="name">
          <a-input v-model="form.name" placeholder="请输入名称" allow-clear />
        </a-form-model-item>

        <a-form-model-item label="缺省表单" prop="sn">
          <a-select placeholder="请选择缺省表单" allow-clear v-model="form.formId" show-search :filter-option="filterOption">
            <a-select-option v-for="(item, i) in forms" :key="i" :value="item.configId">
              {{ item.name }}
            </a-select-option>
          </a-select>
        </a-form-model-item>

        <a-form-model-item label="图标">
          <a-row>
            <eip-icon :name="form.icon" :theme="form.theme" @click="iconClick" @clear="iconClear"></eip-icon>
          </a-row>
        </a-form-model-item>

        <a-form-model-item label="图片" prop="image">
          <a-input v-model="form.image" placeholder="请输入图片地址" allow-clear>
            <a-avatar slot="prefix" size="small" :src="form.image" v-if="form.image" />
            <a-icon @click.stop="material.visible = true" slot="addonAfter" type="setting" />
          </a-input>
        </a-form-model-item>

        <a-form-model-item label="排序号" prop="orderNo">
          <a-input-number id="orderNo" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo" :min="0"
            :max="999" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="remark">
          <a-input allow-clear v-model="form.remark" type="textarea" placeholder="请输入备注" />
        </a-form-model-item>

        <a-form-model-item label="简称" prop="shortName">
          <a-input v-model="form.shortName" placeholder="请输入简称" allow-clear />
        </a-form-model-item>

        <a-form-model-item label="图标颜色">
          <div class="flex align-center">
            <eip-color :value="form.iconColor" @change="(value) => { form.iconColor = value }"></eip-color>
            <a-icon v-if="form.icon" :style="{
              fontSize: '30px',
              'margin-left': '3px',
              color: form.iconColor,
            }" :type="form.icon"></a-icon>
          </div>
        </a-form-model-item>
        <a-form-model-item label="选项">
          <a-checkbox v-model="form.showLibrary"> 流程库显示 </a-checkbox>
          <a-checkbox v-model="form.isFreeze"> 冻结 </a-checkbox>
        </a-form-model-item>
        <a-form-model-item label="流水号格式" prop="sn">
          <span class="ant-input-group-wrapper"><span class="ant-input-wrapper ant-input-group">
              <div class="ant-input" v-html="form.sn"></div>
              <span class="ant-input-group-addon"> <a-icon @click="snClick" type="setting" /></span>
            </span>
          </span>
        </a-form-model-item>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!processId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button v-if="!processId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="processId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
    <sn ref="sn" v-if="sn.visible" :visible.sync="sn.visible" :sn="sn.value" title="标题规则" @ok="snOk"></sn>
    <eip-material v-if="material.visible" :visible.sync="material.visible" @ok="materialOk" />
  </a-modal>
</template>
<script>
import {
  findForm,
  save,
  findById,
} from "@/services/workflow/process/edit";
import { newGuid } from "@/utils/util";
import sn from "@/pages/workflow/process/sn";
export default {
  components: {
    sn,
    VNodes: {
      functional: true,
      render: (h, ctx) => ctx.props.vnodes,
    },
  },
  name: "edit",
  data() {
    return {
      material: {
        visible: false
      },
      type: [],

      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },

      form: {
        processId: newGuid(),
        designJson:
          "{states:{'" +
          newGuid() +
          "':{type:'begin',text:{text:'开始'}, attr:{ x:176, y:176,width:50, height:50}, props:{base:{'title':'开始','formName':'','isopinion':false,'isarchive':false,'titleRule':'您有一条待审批流程'},timeout:[],button:[],notice:[],event:[],field:[]}},'" +
          newGuid() +
          "':{type:'end',text:{text:'结束'}, attr:{ x:516, y:174,width:50, height:50}, props:{base:{'title':'结束','formName':''},notice:[],event:[]}}},paths:{}}",
        saveJson:
          "{states:{'" +
          newGuid() +
          "':{type:'begin',text:{text:'开始'}, attr:{ x:176, y:176,width:50, height:50}, props:{base:{'title':'开始','formName':'','isopinion':false,'isarchive':false,'titleRule':'您有一条待审批流程'},timeout:[],button:[],notice:[],event:[],field:[]}},'" +
          newGuid() +
          "':{type:'end',text:{text:'结束'}, attr:{ x:516, y:174,width:50, height:50}, props:{base:{'title':'结束','formName':''},notice:[],event:[]}}},paths:{}}",
        name: null,
        orderNo: 1,
        iconColor: "",
        icon: "",
        theme: "",
        image: "",
        sn: '<p><span id="%E5%B9%B4" class="non-editable" contenteditable="false">年</span><span id="%E6%9C%88" class="non-editable" contenteditable="false">月</span><span id="%E6%97%A5" class="non-editable" contenteditable="false">日</span><span id="0000" class="non-editable" contenteditable="false">0000</span></p>',
        showLibrary: true,
        formId: undefined,
        isFreeze: false,
        remark: null,
        typeId: undefined,
      },
      forms: [], //节点配置表单
      rules: {
        typeId: [
          {
            required: true,
            message: "请选择分类",
            trigger: ["blur", "change"],
          },
        ],
        name: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],
      },
      sn: {
        visible: false,
        value: "",
      },
      loading: false,
      spinning: false,
      save: {
        continue: false,
        retain: false,
      },
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },

    title: {
      type: String,
    },
    copy: {
      type: Boolean,
      default: false,
    },
    processId: {
      type: String,
    },
  },

  mounted() {
    this.find();
    this.formBaseInit();
  },

  methods: {
    /**
    * 确认素材
    */
    materialOk(path) {
      this.form.image = path;
    },

    /**
     *设置规则
     */
    snClick() {
      this.sn.visible = true;
      this.sn.value = this.form.sn;
    },
    /**
     *规则设置完成
     */
    snOk(value) {
      this.form.sn = value.sn;
    },
    /**
     * 初始化表单
     */
    formBaseInit() {
      let that = this;
      findForm({
        configType: 2,
      }).then(function (result) {
        if (result.code === that.eipResultCode.success) {
          that.forms = result.data;
        }
      });
    },
    /**
     *
     * @param {搜索} input
     * @param {*} option
     */
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text
          .toLowerCase()
          .indexOf(input.toLowerCase()) >= 0
      );
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     *图标点击
     */
    iconClick(icon) {
      this.form.icon = icon.name;
      this.form.theme = icon.theme;
    },

    /**
     * 清空图标
     */
    iconClear() {
      this.form.icon = null;
      this.form.theme = null;
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
          that.form.processId = that.copy ? newGuid() : that.form.processId;
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              that.$emit("ok", result);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.processId = newGuid();
              } else {
                that.cancel();
              }
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
      if (this.processId) {
        that.spinning = true;
        findById(this.processId).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            if (that.copy) {
              result.data.orderNo += 1;
            }
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

<style lang="less" scoped>
/deep/.m-colorPicker .box {
  position: fixed !important;
}
</style>
