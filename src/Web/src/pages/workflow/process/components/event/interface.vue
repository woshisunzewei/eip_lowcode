<template>
  <a-modal width="600px" v-drag centered :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ paddingTop: 0 }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item label="请求方式" prop="type">
              <a-select v-model="form.type">
                <a-select-option value="POST"> POST </a-select-option>
                <a-select-option value="GET"> GET </a-select-option>
              </a-select>
            </a-form-model-item>
            <a-form-model-item label="接口地址" prop="path">
              <a-input allow-clear v-model="form.path" type="textarea" placeholder="请输入接口地址" />
              <a-tag color="pink">
                默认会带上请求Token认证头及表单所有数据，<br />统一返回数据格式为{"Code":200,"Message":"操作成功"}<br />若返回代码为500则不会继续往下面执行
              </a-tag>
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
export default {
  name: "eip-workflow-event-interface",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        type: "POST",
        path: "",
      },
      rules: {
        type: [
          {
            required: true,
            message: "请输入Js代码",
            trigger: "blur",
          },
        ],

        path: [
          {
            required: true,
            message: "请输入地址",
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
    edit: Object,
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
     *
     */
    find() {
      if (this.edit.config) {
        this.form = this.edit.config;
      }
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.$emit("ok", that.form);
          that.cancel();
        } else {
          return false;
        }
      });
    },
  },
};
</script>