<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">

    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item label="归属组织" prop="organizationId">
              <a-tree-select placeholder="请选择归属组织" allow-clear v-model="form.organizationId" show-search
                style="width: 100%" treeNodeFilterProp="title" :tree-data="data"
                :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }" @select="treeSelect">
              </a-tree-select>
            </a-form-model-item>
            <a-form-model-item label="名称" prop="name">
              <a-input v-model="form.name" placeholder="请输入名称" />
            </a-form-model-item>
            <a-form-model-item label="排序号" prop="orderNo">
              <a-input-number id="inputNumber" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo" :min="0"
                :max="999" />
            </a-form-model-item>
            <a-form-model-item label="禁用" prop="isFreeze">
              <a-switch v-model="form.isFreeze" />
            </a-form-model-item>
            <a-form-model-item label="备注" prop="remark">
              <a-input v-model="form.remark" type="textarea" placeholder="请输入备注" />
            </a-form-model-item>
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!roleId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>
        <a-space>
          <a-button v-if="!roleId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="roleId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
  </a-modal>
</template>
<script>
import { save, findById } from "@/services/system/role/edit";
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
        roleId: newGuid(),
        organizationId: undefined,
        organizationName: null,
        name: "",
        orderNo: 1,
        isFreeze: false,
        remark: "",
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
            message: "请输入名称",
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

  mounted() {
    this.find();
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    data: {
      type: Array,
    },
    title: {
      type: String,
    },
    roleId: String,
  },

  methods: {
    /**
     * 树选择
     */
    treeSelect(electedKeys, e) {
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
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.roleId = newGuid();
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
      if (this.roleId) {
        that.spinning = true;
        findById(that.roleId).then(function (result) {
          that.form = result.data;
          that.spinning = false;
        });
      }
    },
  },
};
</script>