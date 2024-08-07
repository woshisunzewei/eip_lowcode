<template>
  <!-- 标签页布局 -->
  <a-form-model-item
    v-if="['tabs', 'grid', 'collapse', 'card', 'table'].includes(record.type)"
  >
    <a-tabs
      v-if="record.type === 'tabs'"
      class="grid-row ant-col"
      :class="'ant-col-' + record.options.span"
      :default-active-key="0"
      :tabBarGutter="record.options.tabBarGutter"
      :type="record.options.type"
      :size="record.options.size"
      :tabPosition="record.options.tabPosition"
      :animated="record.options.animated"
      v-model="activeKey"
    >
      <a-tab-pane
        v-for="(tabItem, index) in record.columns"
        :key="index"
        :tab="tabItem.label"
        :forceRender="true"
      >
        <buildBlocks
          ref="nestedComponents"
          @handleReset="$emit('handleReset')"
          @change="handleChange"
          v-for="item in tabItem.list"
          :disabled="disabled"
          :formValue="formValue"
          :dynamicData="dynamicData"
          :key="item.key"
          :record="item"
          :formConfig="formConfig"
          :config="config"
        />
      </a-tab-pane>
    </a-tabs>
    <!-- 栅格布局 -->
    <a-row
      v-else-if="record.type === 'grid'"
      class="grid-row ant-col"
      :class="'ant-col-' + record.options.span"
      :gutter="record.options.gutter"
    >
      <a-col
        class="grid-col"
        v-for="(colItem, idnex) in record.columns"
        :key="idnex"
        :span="colItem.span || 0"
      >
        <buildBlocks
          ref="nestedComponents"
          @handleReset="$emit('handleReset')"
          @change="handleChange"
          v-for="item in colItem.list"
          :disabled="disabled"
          :formValue="formValue"
          :dynamicData="dynamicData"
          :key="item.key"
          :record="item"
          :formConfig="formConfig"
          :config="config"
        />
      </a-col>
    </a-row>

    <!-- 卡片布局 -->
    <a-card
      v-else-if="!record.options.hidden && record.type === 'card'"
      :style="{
        'margin-top': record.options.marginTop
          ? record.options.marginTop + 'px'
          : '0px',
      }"
      :bodyStyle="{ padding: cardIsShow ? '10px' : '0px' }"
      class="grid-row ant-col"
      :class="'ant-col-' + record.options.span"
      :size="record.options.size"
      :hoverable="record.options.hoverable"
      :bordered="record.options.bordered"
    >
      <div v-show="cardIsShow">
        <buildBlocks
          ref="nestedComponents"
          @handleReset="$emit('handleReset')"
          @change="handleChange"
          v-for="item in record.list"
          :disabled="disabled"
          :formValue="formValue"
          :dynamicData="dynamicData"
          :key="item.key"
          :record="item"
          :formConfig="formConfig"
          :config="config"
        />
      </div>
      <div
        slot="title"
        @click="cardIsShow = !cardIsShow"
        style="cursor: pointer"
      >
        <a-space
          ><a-icon
            v-if="record.options.titleIcon"
            :type="record.options.titleIcon"
          ></a-icon>
          <label>{{ record.label }}</label></a-space
        >
      </div>
      <div slot="extra">
        <a @click="cardIsShow = !cardIsShow" href="javascript:void()">
          <a-tooltip>
            <template slot="title">
              {{ cardIsShow ? "折叠" : "展开" }}
            </template>
            <a-icon :type="cardIsShow ? 'up' : 'down'"></a-icon>
          </a-tooltip>
        </a>
      </div>
    </a-card>

    <!-- 表格布局 -->
    <table
      v-else-if="record.type === 'table'"
      class="kk-table-9136076486841527 ant-col"
      :class="{
        bright: record.options.bright,
        small: record.options.small,
        bordered: record.options.bordered,
        span: 'ant-col-' + record.options.span,
      }"
      :style="
        'width:' + record.options.width + ';' + record.options.customStyle
      "
    >
      <tr v-for="(trItem, trIndex) in record.trs" :key="trIndex">
        <td
          class="table-td"
          v-for="(tdItem, tdIndex) in trItem.tds.filter(
            (item) => item.colspan && item.rowspan
          )"
          :key="tdIndex"
          :style="tdItem.style"
          :colspan="tdItem.colspan"
          :rowspan="tdItem.rowspan"
        >
          <buildBlocks
            ref="nestedComponents"
            @handleReset="$emit('handleReset')"
            @change="handleChange"
            v-for="item in tdItem.list"
            :disabled="disabled"
            :formValue="formValue"
            :dynamicData="dynamicData"
            :key="item.key"
            :record="item"
            :formConfig="formConfig"
            :config="config"
          />
        </td>
      </tr>
    </table>

    <!-- 折叠面板 -->
    <a-collapse
      v-if="record.type === 'collapse'"
      class="ant-collapse-borderless grid-row ant-col"
      :class="'ant-col-' + record.options.span"
      :expand-icon-position="record.options.expandIconPosition"
      :bordered="record.options.bordered"
      :accordion="record.options.accordion"
      v-model="collapseActiveKey"
    >
      <a-collapse-panel
        v-for="(tabItem, index) in record.columns"
        :key="index.toString()"
        :header="tabItem.label"
        :showArrow="record.options.showArrow"
      >
        <buildBlocks
          ref="nestedComponents"
          @handleReset="$emit('handleReset')"
          @change="handleChange"
          v-for="item in tabItem.list"
          :disabled="disabled"
          :formValue="formValue"
          :dynamicData="dynamicData"
          :key="item.key"
          :record="item"
          :formConfig="formConfig"
          :config="config"
        />
      </a-collapse-panel>
    </a-collapse>
  </a-form-model-item>

  <KFormItem
    v-else
    ref="nestedComponents"
    @handleReset="$emit('handleReset')"
    @change="handleChange"
    :disabled="disabled"
    :formValue="formValue"
    :dynamicData="dynamicData"
    :key="record.key"
    :record="record"
    :formConfig="formConfig"
    :config="config"
  />
</template>
<script>
/*
 * author kcz
 * date 2019-11-20
 */
import KFormItem from "../KFormItem/index";
export default {
  name: "buildBlocks",
  props: {
    record: {
      type: Object,
      required: true,
    },
    formConfig: {
      type: Object,
      required: true,
    },
    config: {
      type: Object,
      default: () => ({}),
    },
    dynamicData: {
      type: Object,
      required: true,
    },

    formValue: {
      type: Object,
      required: true,
    },

    disabled: {
      type: Boolean,
      default: false,
    },
    validatorError: {
      type: [Object, null],
      default: () => ({}),
    },
  },
  components: {
    KFormItem,
  },
  data() {
    return {
      cardIsShow: true,
      activeKey: 0,
      collapseActiveKey: [],
    };
  },
  created() {
    if (this.record.type === "collapse") {
      //判断显示类型
      switch (this.record.options.defaultType) {
        case "0":
          for (let index = 0; index < this.record.columns.length; index++) {
            this.collapseActiveKey.push(index.toString());
          }
          break;
        case "1":
          this.collapseActiveKey = ["0"];
          break;
      }
    }
  },
  methods: {
    validationSubform() {
      // 验证动态表格
      const nestedComponents = this.$refs.nestedComponents;
      if (
        typeof nestedComponents === "object" &&
        nestedComponents instanceof Array
      ) {
        for (let i = 0; nestedComponents.length > i; i++) {
          if (!nestedComponents[i].validationSubform()) {
            return false;
          }
        }
        return true;
      } else if (typeof nestedComponents !== "undefined") {
        return nestedComponents.validationSubform();
      } else {
        return true;
      }
    },
    handleChange(value, key) {
      this.$emit("change", value, key);
    },
  },
  watch: {
    /**
     * @author: lizhichao<meteoroc@outlook.com>
     * @description: 监视validatorError对象，当检测到Tabs中有表单校验无法通过时，切换到最近校验失败的tab页。
     */
    validatorError: {
      deep: true,
      handler: function (n) {
        const errorItems = Object.keys(n);
        if (errorItems.length) {
          if (!this.record.columns) return false;
          for (let i = 0; i < this.record.columns.length; i++) {
            const err = this.record.columns[i].list.filter((item) =>
              errorItems.includes(item.model)
            );
            if (err.length) {
              this.activeKey = i;
              break;
            }
          }
        }
      },
    },
  },
};
</script>
<style lang="less" scoped>
.ant-collapse {
  border-radius: 0 !important;
  background-color: #fff !important;
}

/deep/ .ant-collapse-content-box {
  padding: 6px 0 0 0 !important;
}

/deep/ .ant-card-bordered {
  border: 1px solid #d9d9d9 !important;
}

/deep/ .ant-card-head {
  border-bottom: 1px solid #d9d9d9 !important;
}

/deep/ .ant-collapse-header {
  padding: 8px 12px !important;
  border-bottom: 1px solid #d9d9d9 !important;
  padding-left: 36px !important;
  font-weight: 700 !important;
  color: #303133 !important;
  cursor: pointer;
}

.ant-collapse-borderless {
  background-color: #fff;
}

.ant-collapse-borderless .ant-collapse-item {
  background-color: #fff;
  margin-top: 10px;
}

.ant-collapse-borderless .ant-collapse-item .ant-collapse-content-box {
  padding: 16px;
}

.ant-collapse-borderless > .ant-collapse-item {
  border: 1px solid #d9d9d9;
}
</style>