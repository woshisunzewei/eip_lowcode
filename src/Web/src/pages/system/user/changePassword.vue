<template>
  <a-modal width="500px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :closable="!user.changePassword" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-form-model-item has-feedback label="旧密码" prop="oldPassword">
          <a-input-password v-model="form.oldPassword" placeholder="旧密码" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item has-feedback label="新密码" prop="newPassword">
          <a-input-password v-model="form.newPassword" placeholder="新密码" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item has-feedback label="确认新密码" prop="confirmNewPassword">
          <div slot="extra">
            <div v-for="(item, index) in passwordRule" :key="index">
              {{ index + 1 }}, {{ item }}
            </div>
          </div>
          <a-input-password v-model="form.confirmNewPassword" placeholder="确认新密码" autocomplete="off" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>

    <template slot="footer">
      <a-space>
        <a-button v-if="!user.changePassword" key="back" @click="cancel" :disabled="loading"><a-icon
            type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
  </a-modal>
</template>

<script>
import { mapGetters } from "vuex";
import { changePassword, checkPasswordRule } from "@/services/system/user/changePassword";
export default {
  name: "changePassword",
  data() {
    return {
      config: {
        labelCol: { span: 5 },
        wrapperCol: { span: 19 },
      },
      form: {
        oldPassword: null,
        newPassword: null,
        confirmNewPassword: null,
        id: "",
      },
      rules: {
        oldPassword: [
          {
            required: true,
            message: "请输入旧密码",
            trigger: "change",
          },
        ],
        newPassword: [
          {
            validator: (rule, value, callback) => {
              if (typeof value === "undefined" || value === "") {
                callback();
              } else {
                let param = {
                  password: value,
                };
                checkPasswordRule(param).then((result) => {
                  if (result.code == 200) {
                    callback();
                  } else {
                    callback(new Error(result.msg));
                  }
                });
              }
            },
            trigger: "change",
          },
        ],

        confirmNewPassword: [
          {
            required: true,
            message: "请输入确认新密码",
            trigger: "blur",
          }
        ],
      },

      loading: false,
      spinning: false,
    };
  },

  computed: {
    ...mapGetters("account", ["user"]),
  },

  created() {
    this.form.id = this.user.userId;
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
    },
    passwordRule: {
      type: Array,
    },

  },

  methods: {
    /**
     * 重置密码保存
     */
    save() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          if (that.form.newPassword != that.form.confirmNewPassword) {
            that.$message.error("新密码与确认新密码不一致");
            return;
          }
          that.loading = true;
          that.spinning = true;
          that.$message.loading("正在修改密码", 0);
          changePassword(that.form).then((result) => {
            that.$message.destroy();
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              that.$emit("ok");
              that.cancel();
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
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },
  },
};
</script>

<style lang="less"></style>
