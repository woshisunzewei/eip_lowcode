<template>
  <a-modal width="700px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">

        <a-form-model-item label="系统名称" prop="name">
          <a-input v-model="form.name" placeholder="请输入系统名称" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="数据库类型" prop="connectionType">
          <a-select v-model="form.connectionType" style="width: 100%" placeholder="请选择数据库">
            <a-select-option value="mysql"> mysql </a-select-option>
            <a-select-option value="mssql"> mssql </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="数据库连接名" prop="connectionString">
          <a-input v-model="form.connectionString" placeholder="数据库连接名,需要在配置文件中配置数据库连接名称 如 EIP:ConnectionString"
            allow-clear />
        </a-form-model-item>
        <a-form-model-item label="排序号" prop="orderNo">
          <a-input-number id="orderNo" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo" :min="0"
            :max="999" />
        </a-form-model-item>
        <a-form-model-item label="简介" prop="remark">
          <a-input v-model="form.remark" type="textarea" placeholder="请输入简介" allow-clear />
        </a-form-model-item>

      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!dataBaseId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button v-if="!dataBaseId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="dataBaseId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
  </a-modal>
</template>

<script>
import { save, findById } from "@/services/system/database/edit";
import { newGuid } from "@/utils/util";
import moment from "moment";
export default {
  name: "edit",
  data() {
    return {
      tinymce: {
        plugins: "preview  fullscreen code",
        toolbar: "undo redo |fullscreen|code",
        height: 200,
        show: false,
        menubar: "",
      },
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },

      form: {
        dataBaseId: newGuid(),
        connectionType: 'mysql',
        connectionString: '',
        name: null,
        orderNo: 1,
        remark: null
      },

      rules: {
        name: [
          {
            required: true,
            message: "请输入标题",
            trigger: "blur",
          },
        ],
        connectionType: [
          {
            required: true,
            message: "请选择类型",
            trigger: "blur",
          },
        ],
        connectionString: [
          {
            required: true,
            message: "请输入配置名称",
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
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    dataBaseId: {
      type: String,
    },
    title: {
      type: String,
    },
  },

  mounted() {
    setTimeout(() => {
      this.tinymce.show = true;
    }, 10);
    this.find();
  },
  methods: {
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
          that.spinning = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.dataBaseId = newGuid();
              } else {
                that.cancel();
              }
              that.$emit("ok", result);
            } else {
              that.$message.error(result.msg);
            }
            that.loading = false;
            that.spinning = false;
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
      if (this.dataBaseId) {
        that.spinning = true;
        findById(this.dataBaseId).then(function (result) {
          if (result.code == that.eipResultCode.success) {
            var data = result.data;
            if (data.publicTime) {
              data.publicTime = moment(data.publicTime);
            }
            if (data.beginTime) {
              data.beginTime = moment(data.beginTime);
            }
            if (data.endTime) {
              data.endTime = moment(data.endTime);
            }
            that.form = data;
          }
          that.spinning = false;
        });
      }
    },
  },
};
</script>

<style lang="less">
.edui-default {
  line-height: 20px !important;
}
</style>
