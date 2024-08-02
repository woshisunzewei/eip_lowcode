<template>
  <a-modal width="1000px" v-drag centered :destroyOnClose="true" :maskClosable="false" :title="title" :visible="visible"
    :bodyStyle="{ padding: '1px' }" @cancel="cancel">
    <vxe-table ref="batchTable" size="small" :height="height" :data="field">
      <template #loading>
        <a-spin></a-spin>
      </template>

      <template #empty>
        <eip-empty />
      </template>
      <vxe-column type="seq" align="center" title="序号" width="60"></vxe-column>
      <vxe-column field="name" title="字段名" width="180" showOverflow="ellipsis"></vxe-column>
      <vxe-column field="description" title="说明" width="180" showOverflow="ellipsis"></vxe-column>
      <vxe-column field="isRead" title="读" align="center" width="80">

        <template v-slot="{ row }">
          <a-switch :checked="row.isRead" @change="row.isRead = !row.isRead" />
        </template>
      </vxe-column>
      <vxe-column field="isWrite" title="写" align="center" width="80">

        <template v-slot="{ row }">
          <a-switch :checked="row.isWrite" @change="row.isWrite = !row.isWrite" />
        </template>
      </vxe-column>
      <vxe-column field="isDelete" title="删" align="center" width="80">

        <template v-slot="{ row }">
          <a-switch :checked="row.isDelete" @change="row.isDelete = !row.isDelete" />
        </template>
      </vxe-column>
      <vxe-column field="isHide" title="隐藏" align="center" width="80">

        <template v-slot="{ row }">
          <a-switch :checked="row.isHide" @change="row.isHide = !row.isHide" />
        </template>
      </vxe-column>
      <vxe-column field="isDefault" title="默认值" align="center" width="80">

        <template v-slot="{ row }">
          <a-switch :checked="row.isDefault" @change="row.isDefault = !row.isDefault" />
        </template>
      </vxe-column>
      <vxe-column title="验证规则" width="100" align="center">

        <template v-slot="{ row }">
          <a-badge :dot="row.validator && row.validator.length > 0">
            <a-button type="primary" @click="setValidator(row)">验证</a-button>
          </a-badge>
        </template>
      </vxe-column>
    </vxe-table>

    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>

    <eip-workflow-validator ref="validator" v-if="validator.visible" :visible.sync="validator.visible"
      :validator="validator.data" :title="validator.title" @ok="validatorOk"></eip-workflow-validator>

  </a-modal>
</template>

<script>
export default {
  name: "eip-workflow-batch-field",
  data() {
    return {
      height: this.eipHeaderHeight() - 22 + "px",
      column: {
        row: {},
      },
      //验证
      validator: {
        visible: false,
        data: [],
        title: "",
      },
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    field: {
      type: Array,
    },
    title: {
      type: String,
    },
  },
  mounted() {
  },
  methods: {
    /**
     * 保存
     */
    save() {
      this.$emit("ok", this.field);
      this.cancel();
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 设置验证规则
     * @param {*} row
     */
    setValidator(row) {
      this.column.row = row;
      this.validator.data = row.validator;
      this.validator.title = row.description;
      this.validator.visible = true;
    },

    /**
     *验证填写完成
     */
    validatorOk(validator) {
      this.column.row.validator = validator;
    },
  },
};
</script>