<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :title="title" :visible="visible"
    :bodyStyle="modal.bodyStyle" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="joinForm" :model="base" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item label="名称" prop="title">
              <a-input v-model="base.title" placeholder="请输入地址" allow-clear />
            </a-form-model-item>
            <a-form-model-item label="类型" prop="path">
              <a-radio-group v-model="base.type">
                <a-radio :value="0">分支</a-radio>
                <a-radio :value="1">合并</a-radio>
              </a-radio-group>
            </a-form-model-item>

            <a-form-model-item label="选项" prop="pass" v-if="base.type == 1">
              <a-radio-group v-model="base.pass">
                <a-radio :value="0">所有都要通过</a-radio>
                <a-radio :value="1">一个同意就通过</a-radio>
                <a-radio :value="2">按百分比通过</a-radio>
              </a-radio-group>
            </a-form-model-item>

            <a-form-model-item v-if="base.pass == 2" label="比例" prop="passPercent">
              <a-input-number v-model="base.passPercent" :min="0" :max="100" :formatter="(value) => `${value}%`"
                :parser="(value) => value.replace('%', '')" />
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
  name: "join",
  data() {
    return {
      modal: {
        bodyStyle: {
          padding: "1px",
        },
      },
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      base: {
        title: "合并",
        type: 0, //默认为条件
        pass: 0, //默认需要所有人通过才可以下一步
        passPercent: 0, //百分百
      },
      rules: {
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
    data: {
      type: Object,
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
      this.$refs.joinForm.validate((valid) => {
        if (valid) {
          that.data.props.base = that.base;
          this.data.text.text = this.base.title;
          that.$emit("ok", that.data);
          that.cancel();
        } else {
          return false;
        }
      });
    },

    /**
     *
     */
    find() {
      this.base = this.$utils.clone(this.data.props.base, true);
    },
  },
};
</script>