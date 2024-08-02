<template>
  <a-modal width="600px" v-drag centered :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" @cancel="cancel" :title="'规则设置:' + title">
    <vxe-table ref="table" id="workflowValidator" size="small" :height="height" :data="data" row-key>
      <template #loading>
        <a-spin></a-spin>
      </template>
      <template #empty>
        <eip-empty />
      </template>
      <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
      <vxe-column title="排序" width="50" align="center">
        <template #default>
          <span class="drag-btn" style="cursor: move">
            <a-icon type="drag" />
          </span>
        </template>
      </vxe-column>
      <vxe-column width="80" align="center">
        <template #header>
          <a-button size="small" type="primary" @click="add">
            <a-icon type="plus" />
          </a-button>
        </template>
        <template #default="{ row }">
          <a-popconfirm title="确定删除规则?" ok-text="确定" cancel-text="取消" @confirm="del(row)">
            <a-button @click.stop="" size="small" type="danger">
              <a-icon type="delete" />
            </a-button>
          </a-popconfirm>
        </template>
      </vxe-column>
      <vxe-column title="类型" align="center">
        <template v-slot="{ row }">
          <a-space size="small">
            <a-select style="width: 120px" v-model="row.validators">
              <a-select-option value="required">不允许为空</a-select-option>
              <a-select-option value="regexp">正则表达式</a-select-option>
            </a-select>
            <a-button @click="settingSet(row)">
              <a-icon type="setting" />
            </a-button>
          </a-space>
        </template>
      </vxe-column>
      <vxe-column field="remark" title="错误显示值">
        <template v-slot="{ row }">
          <a-input placeholder="请输入错误显示值" v-model="row.message" />
        </template>
      </vxe-column>
    </vxe-table>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
    <regexp ref="regexp" v-if="regexp.visible" :visible.sync="regexp.visible" :value="regexp.value" @ok="regexpOk">
    </regexp>
  </a-modal>
</template>

<script>
import Sortable from "sortablejs";
import regexp from "./components/regexp";
export default {
  name: "eip-workflow-validator",
  data() {
    return {
      loading: false,
      data: [],
      height: "300px",
      regexp: {
        title: "",
        visible: false,
        value: "",
      },
      setting: {},
    };
  },
  components: { regexp },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    validator: {
      type: Array,
    },
    title: {
      type: String,
    },
  },

  mounted() {
    this.init();
  },

  methods: {
    /**
     *初始化值
     */
    init() {
      let that = this;
      if (this.validator.length > 0) {
        this.data = this.validator;
        setTimeout(function () {
          that.drop();
        }, 500);
      }
    },

    /**
     * 拖拽
     */
    drop() {
      let that = this;
      const xTable = this.$refs.table;
      Sortable.create(
        xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
        {
          handle: ".drag-btn",
          onEnd: ({ newIndex, oldIndex }) => {
            const currRow = that.data.splice(oldIndex, 1)[0];
            that.data.splice(newIndex, 0, currRow);
          },
        }
      );
    },

    /**
     *
     */
    add() {
      this.data.push({
        validators: "required",
        verification: "",
        message: this.title + "不能为空",
      });
      this.drop();
    },

    /**
     * 删除
     */
    del(row) {
      var delIndex = -1;
      this.data.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          delIndex = index;
        }
      });
      if (delIndex != -1) {
        this.data.splice(delIndex, 1);
      }
      this.drop();
    },
    /**
     *
     */
    settingSet(row) {
      this.setting = row;
      switch (row.validators) {
        case "regexp":
          this.regexp.value = row.verification;
          this.regexp.visible = true;
          break;
        default:
          this.$message.error("改类型无需另外设置规则");
          break;
      }
    },

    /**
     * 正则表达式设置成功
     */
    regexpOk(value) {
      this.setting.verification = value;
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
      this.$emit("ok", this.data);
      this.cancel();
    },
  },
};
</script>