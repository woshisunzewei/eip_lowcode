<template>
  <a-modal
    width="600px"
    v-drag
    :destroyOnClose="true"
    :maskClosable="false"
    :visible="visible"
    :bodyStyle="{ padding: '1px' }"
    :zIndex="1005"
    :title="title"
    @cancel="cancel"
  >
    <a-spin :spinning="spinning">
      <a-form-model
        ref="form"
        :model="form"
        :rules="rules"
        :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol"
      >
        <a-row>
          <a-col>
            <a-form-model-item label="名称" prop="name">
              <a-input
                v-model="form.name"
                placeholder="请输入名称"
                allow-clear
              />
            </a-form-model-item>
            <a-form-model-item label="触发类型" prop="configType">
              <a-select
                placeholder="请选择触发类型"
                allow-clear
                v-model="form.configType"
              >
                <a-select-opt-group>
                  <span slot="label"
                    ><a-icon type="table" class="margin-right-sm" />工作表</span
                  >
                  <a-select-option :value="1">工作表新增或变更</a-select-option>
                </a-select-opt-group>
                <a-select-opt-group>
                  <span slot="label"
                    ><a-icon
                      type="calendar"
                      class="margin-right-sm"
                    />时间</span
                  >
                  <a-select-option :value="2">时间周期循环</a-select-option>
                  <a-select-option :value="3">指定日期字段</a-select-option>
                </a-select-opt-group>

                <a-select-opt-group>
                  <span slot="label"
                    ><a-icon
                      type="calendar"
                      class="margin-right-sm"
                    />业务封装</span
                  >
                  <a-select-option :value="4">业务封装</a-select-option>
                </a-select-opt-group>

                <a-select-opt-group>
                  <span slot="label"
                    ><a-icon
                      type="calendar"
                      class="margin-right-sm"
                    />其他</span
                  >
                  <a-select-option :value="100">其他</a-select-option>
                </a-select-opt-group>
              </a-select>
            </a-form-model-item>
            <a-form-model-item label="排序号" prop="orderNo">
              <a-input-number
                id="inputNumber"
                style="width: 100%"
                placeholder="请输入排序号"
                v-model="form.orderNo"
                :min="0"
                :max="999"
              />
            </a-form-model-item>
            <a-form-model-item label="禁用" prop="isFreeze">
              <a-switch v-model="form.isFreeze" />
            </a-form-model-item>
            <a-form-model-item label="备注" prop="remark">
              <a-input
                allow-clear
                v-model="form.remark"
                type="textarea"
                placeholder="请输入备注"
              />
            </a-form-model-item>
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!configId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button
            v-if="!configId"
            key="submit"
            @click="saveContinue"
            :loading="loading"
            ><a-icon type="save" />提交并继续创建</a-button
          >
          <a-button v-if="configId" @click="cancel"
            ><a-icon type="close" />关闭</a-button
          >
          <a-button
            key="submit"
            @click="saveClose"
            type="primary"
            :loading="loading"
            ><a-icon type="save" />提交</a-button
          >
        </a-space>
      </div>
    </template>
  </a-modal>
</template>

<script>
import { save, findById } from "@/services/system/agile/automation/edit";
import { newGuid } from "@/utils/util";
export default {
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 5 },
        wrapperCol: { span: 17 },
      },
      form: {
        configId: newGuid(),
        dataFromName: null,
        name: null,
        orderNo: 1,
        isFreeze: false,
        tableId: this.tableId,
        remark: null,
        configType: undefined,
      },

      rules: {
        configType: [
          {
            required: true,
            message: "请选择表单类型",
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

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    tableId: String,
    configId: String,
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
    saveConfirm() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          if (that.form.configType == 1) {
            save(that.form).then(function (result) {
              if (result.code === that.eipResultCode.success) {
                that.$message.success(result.msg);
                if (that.save.continue) {
                  //不保留上次内容
                  if (!that.save.retain) {
                    that.$refs.form.resetFields();
                  }
                  that.form.configId = newGuid();
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
            save(that.form).then(function (result) {
              if (result.code === that.eipResultCode.success) {
                that.$message.success(result.msg);
                if (that.save.continue) {
                  //不保留上次内容
                  if (!that.save.retain) {
                    that.$refs.form.resetFields();
                  }
                  that.form.configId = newGuid();
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
          }
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
    async find() {
      let that = this;
      that.spinning = true;
      if (this.configId) {
        findById(this.configId).then(function (result) {
          that.form = result.data;
        });
      }
      that.spinning = false;
    },
  },
};
</script>