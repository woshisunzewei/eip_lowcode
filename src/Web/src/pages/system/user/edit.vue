<template>
  <a-modal width="650px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-form-model-item label="归属组织" prop="organizationId">
          <a-tree-select placeholder="请选择归属组织" allow-clear v-model="form.organizationId" show-search style="width: 100%"
            treeNodeFilterProp="title" :tree-data="data" :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
            @select="organizationSelect">
          </a-tree-select>
        </a-form-model-item>
        <a-form-model-item label="关联组织" prop="userOrganizationIds">
          <a-tree-select placeholder="请选择关联组织" allow-clear v-model="userOrganizationIds" tree-checkable
            style="width: 100%" treeNodeFilterProp="title" :maxTagCount="4" :tree-data="data"
            :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }">
          </a-tree-select>
        </a-form-model-item>
        <a-form-model-item label="姓名" prop="name">
          <a-input v-model="form.name" placeholder="请输入姓名" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="登录名" prop="code">
          <a-input v-model="form.code" placeholder="请输入登录名" allow-clear />
        </a-form-model-item>

        <a-form-model-item label="角色" prop="roleId">
          <a-select :default-value="roleIdDefault" :allowClear="true" show-search :filter-option="filterOption"
            option-filter-prop="children" :autoClearSearchValue="true" mode="multiple" style="width: 100%"
            placeholder="请选择角色" @change="roleChange">
            <a-select-option :value="role.roleId" v-for="role in roles" :key="role.name">
              {{ role.name }}
            </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="手机号" prop="mobile">
          <a-input allow-clear v-model="form.mobile" placeholder="请输入手机号" type="number" />
        </a-form-model-item>
        <a-form-model-item label="邮箱" prop="email">
          <a-input v-model="form.email" placeholder="请输入邮箱" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="其他" prop="otherContactInformation">
          <a-input allow-clear v-model="form.otherContactInformation" placeholder="请输入其他联系方式" />
        </a-form-model-item>


        <a-form-model-item label="性别" prop="sex">
          <a-radio-group name="radioGroup" v-model="form.sex">
            <a-radio :value="0"> 男 </a-radio>
            <a-radio :value="1"> 女 </a-radio>
          </a-radio-group>
        </a-form-model-item>

        <a-form-model-item label="禁用" prop="isFreeze">
          <a-switch v-model="form.isFreeze" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="remark">
          <a-input allow-clear v-model="form.remark" type="textarea" placeholder="请输入备注" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!userId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button v-if="!userId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="userId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
  </a-modal>
</template>

<script>
import {
  save,
  findById,
  roleSelect,
  detail,
  checkCode,
} from "@/services/system/user/edit";
import { newGuid } from "@/utils/util";
export default {
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      userOrganizationIds: [],
      form: {
        userId: newGuid(),
        organizationId: null,
        organizationName: null,
        name: null,
        code: null,
        sex: 0,
        mobile: null,
        roleId: null,
        email: null,
        otherContactInformation: null,
        isFreeze: false,
        remark: null,
        userOrganizationIds: null,
      },

      rules: {
        organizationId: [
          {
            required: true,
            message: "请选择归属组织",
            trigger: "change",
          },
        ],
        name: [
          {
            required: true,
            message: "请输入姓名",
            trigger: "blur",
          },
        ],

        mobile: [
          {
            validator: (rule, value, callback) => {
              if (
                typeof value === "undefined" ||
                value === "" ||
                value === null
              ) {
                callback();
              } else {
                const regex = /^1[3456789]\d{9}$/;
                if (!regex.test(value)) {
                  callback(new Error("请输入正确手机格式"));
                } else {
                  callback();
                }
              }
            },
            trigger: "blur",
          },
        ],
        email: [
          {
            validator: (rule, value, callback) => {
              if (
                typeof value === "undefined" ||
                value === "" ||
                value === null
              ) {
                callback();
              } else {
                const regex =
                  /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
                if (!regex.test(value)) {
                  callback(new Error("请输入正确邮箱格式"));
                } else {
                  callback();
                }
              }
            },
            trigger: "blur",
          },
        ],
        code: [
          {
            required: true,
            message: "请输入登录账号",
            trigger: "blur",
          },
          {
            validator: (rule, value, callback) => {
              let that = this;
              if (typeof value === "undefined" || value === "") {
                callback();
              } else {
                checkCode({
                  id: this.form.userId,
                  code: value,
                }).then((result) => {
                  if (result.code !== that.eipResultCode.success) {
                    callback(result.msg);
                  } else {
                    callback();
                  }
                });
              }
            },
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
      roleIdDefault: [],
      roles: [],
    };
  },
  created() {
    this.rolesInit();
    this.find();
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    userId: String,
    data: {
      type: Array,
    },
    title: {
      type: String,
    },
  },

  methods: {
    filterOption(input, option) {
      return option.componentOptions.children[0].text.toLowerCase().indexOf(input.toLowerCase()) >= 0
    },
    /**
     * 角色下拉
     */
    rolesInit() {
      roleSelect().then((result) => {
        this.roles = result.data;
      });
    },

    /**
     * 角色选择
     */
    roleChange(value) {
      this.form.roleId = value.join(",");
    },

    /**
     * 树选择
     */
    organizationSelect(electedKeys, e) {
      this.form.organizationName = e.title;
      this.form.organizationId = e.eventKey;
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
          that.form.userOrganizationIds = that.userOrganizationIds.join(",");
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.userId = newGuid();
              } else {
                that.cancel();
              }
              that.$emit("save", result);
            } else {
              that.$message.error(result.msg);
            }
            that.$loading.hide();
            that.loading = false;
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
      if (this.userId) {
        this.spinning = true;
        findById(this.userId).then((result) => {
          that.form = result.data;
          if (that.form.userOrganizationIds) {
            that.userOrganizationIds = that.form.userOrganizationIds.split(",");
          }
          detail(that.form.userId).then((result) => {
            result.data.role.forEach((element) => {
              that.roleIdDefault.push(element.privilegeMasterValue);
            });
            that.form.roleId = that.roleIdDefault.join(",");
          });
          that.spinning = false;
        });
      }
    },
  },
};
</script>