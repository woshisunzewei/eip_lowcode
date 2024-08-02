<template>
  <a-modal width="800px" v-drag :visible="visible" :destroyOnClose="true" :maskClosable="false" title="组织架构维护"
    on-ok="handleOk">
    <template slot="footer">
      <a-button key="back"> 取消 </a-button>
      <a-button key="submit" @click="onSubmit" type="primary"> 确定 </a-button>
    </template>
    <a-form-model ref="ruleForm" :model="form" :rules="rules" :label-col="labelCol" :wrapper-col="wrapperCol">
      <a-row>
        <a-col :span="12">
          <a-form-model-item ref="ParentName" label="上级单位" prop="ParentName">
            <a-tree-select :tree-data="treeData" v-model="value" show-search style="width: 100%"
              :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }" placeholder="Please select" allow-clear>
            </a-tree-select>
          </a-form-model-item>
          <a-form-model-item ref="Name" label="全称" prop="Name">
            <a-input v-model="form.Name" />
          </a-form-model-item>
          <a-form-model-item ref="ShortName" label="简称" prop="ShortName">
            <a-input v-model="form.ShortName" />
          </a-form-model-item>
          <a-form-model-item ref="MainSupervisor" label="负责人" prop="MainSupervisor">
            <a-input v-model="form.MainSupervisor" />
          </a-form-model-item>
          <a-form-model-item label="备注" prop="Remark">
            <a-input v-model="form.Remark" type="textarea" />
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item ref="MainSupervisorContact" label="联系方式" prop="MainSupervisorContact">
            <a-input v-model="form.MainSupervisorContact" />
          </a-form-model-item>
          <a-form-model-item label="类型" prop="Nature">
            <a-select v-model="form.Nature" placeholder="请选择类型">
              <a-select-option value="shanghai"> Zone one </a-select-option>
              <a-select-option value="beijing"> Zone two </a-select-option>
            </a-select>
          </a-form-model-item>

          <a-form-model-item label="禁用" prop="isFreeze">
            <a-switch v-model="form.IsFreeze" />
          </a-form-model-item>
          <a-form-model-item ref="OrderNo" label="排序号" prop="OrderNo">
            <a-input-number id="inputNumber" style="width: 100%" v-model="form.OrderNo" :min="1" :max="10" />
          </a-form-model-item>
        </a-col>
      </a-row>
    </a-form-model>
  </a-modal>
</template>

<script>
const treeData = [

];
export default {
  data() {
    return {
      value: "",
      labelCol: { span: 6 },
      wrapperCol: { span: 20 },
      other: "",
      form: {
        ParentName: "",
        Name: "",
        ShortName: "",
        MainSupervisor: "",
        MainSupervisorContact: "",
        Nature: "",
        IsFreeze: "",
        OrderNo: "",
        Remark: "",
      },
      rules: {
        Name: [
          {
            required: true,
            message: "Please input Activity name",
            trigger: "blur",
          },
        ],
        region: [
          {
            required: true,
            message: "Please select Activity zone",
            trigger: "change",
          },
        ],
        date1: [
          { required: true, message: "Please pick a date", trigger: "change" },
        ],
        type: [
          {
            type: "array",
            required: true,
            message: "Please select at least one activity type",
            trigger: "change",
          },
        ],
        resource: [
          {
            required: true,
            message: "Please select activity resource",
            trigger: "change",
          },
        ],
        desc: [
          {
            required: true,
            message: "Please input activity form",
            trigger: "blur",
          },
        ],
      },
      visible: false,
    };
  },
  created() { },
  methods: {
    onSubmit() {
      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          alert("submit!");
        } else {
          return false;
        }
      });
    },
    resetForm() {
      this.$refs.ruleForm.resetFields();
    },

    onSelect(selectedKeys, info) {
    },
    onCheck(checkedKeys, info) {
    },
  },
};
</script>

<style lang="less" scoped></style>
