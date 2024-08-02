<template>
  <a-modal :zIndex="10001" width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" title="邀请阅示" @cancel="cancel">
    <div style="padding: 10px">
      <a-spin :spinning="spinning">
        <a-form-model ref="form" :model="form" layout="vertical" :rules="rules">
          <a-row>
            <a-col>
              <a-form-model-item label="">
                <a-alert message="邀请阅示" description="邀请他人对任务给出指导、发表意见。向邀请人员发送通知并产生一个阅示待办项，阅示人员批阅时需给出意见。"
                  type="warning" />
              </a-form-model-item>
              <a-form-model-item label="阅示人员" prop="userName">
                <a-input v-model="form.userName" placeholder="请选择阅示人员" disabled>
                  <a-icon @click="user.visible = true" style="cursor: pointer" slot="addonAfter" type="search" />
                </a-input>
              </a-form-model-item>
            </a-col>
          </a-row>
        </a-form-model>
      </a-spin>
    </div>

    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
    <eip-user v-if="user.visible" :visible.sync="user.visible" :topOrg="user.topOrg" title="选择人员"
      @ok="chosenUserOk"></eip-user>
  </a-modal>
</template>

<script>

export default {
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        userId: null,
        userName: "",
      },
      user: {
        visible: false,
        topOrg: "",
      },
      rules: {
        userName: [
          {
            required: true,
            message: "请选择阅示人员",
            trigger: ["blur", "change"],
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
    title: {
      type: String,
    },
  },
  mounted() { },
  methods: {
    chosenUserOk(data) {
      this.form.userId = data.map((m) => m.userId).join(",");
      this.form.userName = data.map((m) => m.name).join(",");
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
    save() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.$emit("confirm", {
            userId: that.form.userId,
            userName: that.form.userName,
          });
          that.cancel();
        } else {
          return false;
        }
      });
    },
  },
};
</script>