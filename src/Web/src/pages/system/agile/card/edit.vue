<template>
  <a-modal v-drag width="600px" :visible="visible" :destroyOnClose="true" :title="title" @cancel="cancel">
    <template slot="footer">
      <a-button key="back" @click="cancel" :disabled="loading"> 取消 </a-button>
      <a-button key="submit" @click="save" type="primary" :loading="loading">
        确定
      </a-button>
    </template>
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item ref="configId" label="记录ID" prop="configId">
              <a-input v-model="form.configId" placeholder="记录ID自动生成" :disabled="true" />
            </a-form-model-item>
            <a-form-model-item ref="name" label="名称" prop="name">
              <a-input v-model="form.name" placeholder="请输入名称" />
            </a-form-model-item>
            <a-form-model-item ref="tableName" label="表名" prop="tableName">
              <a-input v-model="form.tableName" placeholder="请输入表名" />
            </a-form-model-item>
            <a-form-model-item ref="orderNo" label="排序号" prop="orderNo">
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
  </a-modal>
</template>

<script>
import {
  save,
  findById,
  tableExist,
  table,
} from "@/services/system/agile/designer/form/edit";
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
        configId: newGuid(),
        tableName: "",
        name: "",
        orderNo: 1,
        isFreeze: false,
        remark: "",
      },

      rules: {
        tableName: [
          {
            validator: (rule, value, callback) => {
              if (typeof value === "undefined" || value === "") {
                callback();
              } else {
                //表名是否具有
                let param = {
                  id: this.form.configId,
                  tableName: value,
                };
                tableExist(param).then((result) => {
                  if (result.data) {
                    callback();
                  } else {
                    callback(new Error("表名已存在"));
                  }
                });
              }
            },
            trigger: "blur",
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
    };
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
          table(that.form).then((result) => {
            if (result.code === that.eipResultCode.success) {
              save(that.form).then(function (result) {
                if (result.code === that.eipResultCode.success) {
                  that.$message.success(result.msg);
                  that.reset();
                  that.$emit("ok", result);
                } else {
                  that.$message.error(result.msg);
                }
                that.loading = false;
                that.spinning = false;
              });
            } else {
              that.$message.error(result.msg);
              that.loading = false;
              that.spinning = false;
            }
          });
        } else {
          return false;
        }
      });
    },

    /**
     * 根据Id查找
     */
    find(configId) {
      let that = this;
      that.spinning = true;
      findById(configId).then(function (result) {
        let form = result.data;
        that.form.configId = form.configId;
        that.form.tableName = form.tableName;
        that.form.name = form.name;
        that.form.orderNo = form.orderNo;
        that.form.isFreeze = form.isFreeze;
        that.form.remark = form.remark;
        that.spinning = false;
      });
    },
  },
};
</script>