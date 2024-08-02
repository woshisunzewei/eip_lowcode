<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item label="代码" prop="code">
              <a-input v-model="form.code" placeholder="请输入代码" allow-clear />
            </a-form-model-item>
            <a-form-model-item label="名称" prop="title">
              <a-input allow-clear v-model="form.title" placeholder="请输入名称" />
            </a-form-model-item>
            <a-form-model-item label="一级行业" prop="industryOne">
              <a-input v-model="form.industryOne" placeholder="请输入代码" allow-clear />
            </a-form-model-item>
            <a-form-model-item label="二级行业" prop="industryTwo">
              <a-input v-model="form.industryTwo" placeholder="请输入代码" allow-clear />
            </a-form-model-item>
            <a-form-model-item label="备注" prop="remark">
              <a-input allow-clear v-model="form.remark" type="textarea" placeholder="请输入备注" />
            </a-form-model-item>
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>

    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
  </a-modal>
</template>


<script>
import { save, findById } from "@/services/wechat/mp/template/edit";
import { newGuid } from "@/utils/util";
export default {
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        templateId: newGuid(),
        code: null,
        title: null,
        design: "",
        industryOne: "",
        industryTwo: "",
        remark: null,
      },

      rules: {
        code: [
          {
            required: true,
            message: "请输入代码",
            trigger: "blur",
          },
        ],
        title: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],
      },

      loading: false,
      spinning: false,
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    templateId: {
      type: String,
    },
    title: {
      type: String,
    },
  },
  mounted() {
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
    save() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          that.spinning = true;
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              that.cancel();
              that.$emit("ok", result);
            } else {
              that.$message.error(result.msg);
            }
            that.loading = false;
            that.spinning = false;
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
      if (this.templateId) {
        that.spinning = true;
        findById(this.templateId).then(function (result) {
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