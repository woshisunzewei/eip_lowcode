<template>
  <div>
    <vxe-table ref="table" row-key :height="height" :data="columns" :tooltip-config="tooltipConfig">
      <template #loading>
        <a-spin></a-spin>
      </template>
      <template #empty>
        <eip-empty />
      </template>
      <vxe-column type="seq" title="序号" align="center" width="40"></vxe-column>
      <vxe-column title="排序" width="48" align="center">
        <template>
          <span class="drag-btn">
            <a-icon type="drag" />
          </span>
        </template>
      </vxe-column>

      <vxe-column title="显示" align="center" width="51">
        <template v-slot="{ row }">
          <a-checkbox v-model="row.isShow" />
        </template>
      </vxe-column>
      <vxe-column field="isSearch" title="查询" align="center" width="51">
        <template v-slot="{ row }">
          <a-checkbox v-model="row.isSearch" />
        </template>
      </vxe-column>

      <vxe-column field="description" title="标题" align="center" min-width="180">
        <template v-slot="{ row }">
          <a-input v-model="row.description" :placeholder="row.name">
            <a-tooltip slot="suffix" :title="row.name">
              <a-icon type="info-circle" />
            </a-tooltip>
          </a-input>
        </template>
      </vxe-column>
      <vxe-column title="对齐" align="center" width="51">
        <template v-slot="{ row }">
          <select class="ant-select-selection ant-select-selection--single" v-model="row.align" placeholder="请选择对齐方式">
            <option value="left">左</option>
            <option value="center">中</option>
            <option value="right">右</option>
          </select>
        </template>
      </vxe-column>
      <vxe-column field="width" title="宽度(px)" align="center" width="100">
        <template v-slot="{ row }">
          <a-input-number style="width: 80px" placeholder="请输入宽度(px)" v-model="row.width" />
        </template>
      </vxe-column>
      <vxe-column field="format" title="格式化" align="center" width="120">
        <template v-slot="{ row }">
          <select class="ant-select-selection ant-select-selection--single" placeholder="格式化" v-model="row.format"
            style="width: 100px">
            <option value=""></option>
            <option v-for="item in eipFormat" :value="item.value" :title="item.label" :key="item.value">{{ item.label }}
            </option>
          </select>
        </template>
      </vxe-column>
      <vxe-column field="fixed" title="固定" align="center" width="51">

        <template v-slot="{ row }">
          <select class="ant-select-selection ant-select-selection--single" placeholder="固定" v-model="row.fixed">
            <option value=""></option>
            <option value="left">左</option>
            <option value="right">右</option>
          </select>
        </template>
      </vxe-column>
      <vxe-column field="isTotal" title="合计" align="center" width="51">

        <template v-slot="{ row }">
          <a-checkbox v-model="row.isTotal" />
        </template>
      </vxe-column>
      <vxe-column field="isSort" title="排序" align="center" width="51">

        <template v-slot="{ row }">
          <a-checkbox v-model="row.isSort" />
        </template>
      </vxe-column>
      <vxe-column field="isSort" title="升序" align="center" width="51">

        <template v-slot="{ row }">
          <a-checkbox v-model="row.isSortAsc" />
        </template>
      </vxe-column>
      <vxe-column field="isSort" title="默认" align="center" width="51">

        <template v-slot="{ row }">
          <a-checkbox v-model="row.isSortDefalut" />
        </template>
      </vxe-column>
      <vxe-column field="mask" title="掩码显示" align="center" width="160">

        <template v-slot="{ row }">
          <a-input-group compact>
            <select class="ant-select-selection ant-select-selection--single" v-model="row.mask" placeholder="掩码显示"
              style="width: 100%">
              <option value=""></option>
              <option v-for="item in eipMask" :value="item.value" :title="item.label" :key="item.value">{{
                item.label }}
              </option>

            </select>
          </a-input-group>
        </template>
      </vxe-column>
      <vxe-column field="remark" title="备注" align="center" width="100">

        <template v-slot="{ row }">
          <a-input v-model="row.remark" :placeholder="row.remark">
          </a-input>
        </template>
      </vxe-column>
      <vxe-column title="样式" align="center" width="48">

        <template v-slot="{ row }">
          <a-badge :count="row.style.length" v-if="row.style !== undefined &&
            row.style !== null &&
            row.style.length > 0
          ">
            <a-button size="small" @click="styleClick(row)" type="primary">
              <a-icon type="bg-colors" />
            </a-button>
          </a-badge>

          <a-button size="small" v-else @click="styleClick(row)" type="primary">
            <a-icon type="bg-colors" />
          </a-button>
        </template>
      </vxe-column>

      <vxe-column title="子表" align="center" width="48px">

        <template v-slot="{ row }">
          <a-badge :count="row.subtable.length" v-if="row.subtable !== undefined &&
            row.subtable !== null &&
            row.subtable.length > 0
          ">
            <a-button size="small" @click="subtableClick(row)" type="primary">
              <a-icon type="table" />
            </a-button>
          </a-badge>

          <a-button size="small" v-else @click="subtableClick(row)" type="primary">
            <a-icon type="table" />
          </a-button>
        </template>
      </vxe-column>

      <vxe-column title="弹出" align="center" width="48px">
        <template v-slot="{ row }">
          <a-badge :dot="row.dialog.id != null && row.dialog.id != ''">
            <a-button size="small" @click="dialogClick(row)" type="primary">
              <a-icon type="select" />
            </a-button>
          </a-badge>
        </template>
      </vxe-column>

      <vxe-column title="脚本" align="center" width="48px">
        <template v-slot="{ row }">
          <a-badge :dot="row.script != null && row.script != ''">
            <a-button size="small" @click="scriptClick(row)" type="primary">
              <a-icon type="code" />
            </a-button>
          </a-badge>
        </template>
      </vxe-column>
      <vxe-column title="多表头" align="center" width="58px">
        <template v-slot="{ row }">
          <a-badge :count="row.header.field.length" v-if="row.header.field.length > 0">
            <a-button size="small" @click="headerClick(row)" type="primary">
              <a-icon type="bars" />
            </a-button>
          </a-badge>
          <a-button size="small" v-else @click="headerClick(row)" type="primary">
            <a-icon type="bars" />
          </a-button>
        </template>
      </vxe-column>
    </vxe-table>

    <list-subtable v-if="drawer.subtable.visible" :visible.sync="drawer.subtable.visible"
      :config="drawer.subtable.config" :data="subtable" ref="listSubtable" @ok="subtableOk" />

    <list-dialog v-if="drawer.dialog.visible" :visible.sync="drawer.dialog.visible" :config="drawer.dialog.config"
      ref="listDialog" @ok="dialogOk" />

    <list-style v-if="drawer.style.visible" :visible.sync="drawer.style.visible" :config="drawer.style.config"
      ref="listStyle" @ok="styleOk" />

    <list-header v-if="drawer.header.visible" :visible.sync="drawer.header.visible" :config="drawer.header.config"
      :data="controls" ref="listHeader" @ok="headerOk" />

    <list-script v-if="drawer.script.visible" :visible.sync="drawer.script.visible" :config="drawer.script.config"
      :data="controls" ref="listScript" @ok="scriptOk" />
  </div>
</template>

<script>
import listSubtable from "@/pages/system/agile/common/list/subtable";
import listDialog from "@/pages/system/agile/common/list/dialog";
import listStyle from "@/pages/system/agile/common/list/style";
import listHeader from "@/pages/system/agile/common/list/header";
import listScript from "@/pages/system/agile/common/list/script";
import Sortable from "sortablejs";
export default {
  name: "filter-index",
  components: { listSubtable, listDialog, listStyle, listHeader, listScript },
  data() {
    return {
      tooltipConfig: {
        showAll: true,
        enterable: true,
        contentMethod: ({ type, column, row, items, _columnIndex }) => {
          const { field } = column
          if (field === 'format' || field === 'mask') {
            return ''
          }
          return null
        }
      },
      height: window.innerHeight - 66,
      drawer: {
        visible: this.visible,
        subtable: {
          visible: false,
          zIndex: 9999,
          config: []
        },

        dialog: {
          visible: false,
          zIndex: 1040,
          config: {},
        },
        script: {
          visible: false,
          zIndex: 1040,
          config: {},
        },
        style: {
          visible: false,
          config: {},
        },

        header: {
          visible: false,
          zIndex: 1040,
          config: {},
        },

      },

    };
  },

  props: {
    configId: {
      type: String,
    },
    columns: {
      type: Array,
    },
    subtable: {
      type: Array,
    },
    controls: {
      type: Array,
    }
  },

  mounted() {
    this.rowDrop();
  },

  methods: {
    /**
     * 弹出框点击
     */
    scriptClick(item) {
      this.drawer.script.visible = true;
      this.drawer.script.config = item.script;
      this.editRow = item;
    },

    /**
     * 弹出框规则保存
     */
    scriptOk(config) {
      this.editRow.script = config.script;
    },

    /**
     * 子表规则点击
     */
    subtableClick(item) {
      this.drawer.subtable.visible = true;
      this.drawer.subtable.config = item.subtable;
      this.editRow = item;
    },

    /**
     * 子表规则保存
     */
    subtableOk(config) {
      this.editRow.subtable = config;
    },

    /**
       * 多表头点击
       */
    headerClick(item) {
      this.drawer.header.visible = true;
      this.drawer.header.config = item;
      this.editRow = item;
    },

    /**
     * 多表头保存
     */
    headerOk(config) {
      this.editRow.header = config;
    },

    /**
     * 弹出框点击
     */
    dialogClick(item) {
      this.drawer.dialog.visible = true;
      this.drawer.dialog.config = item;
      this.editRow = item;
    },

    /**
     * 弹出框规则保存
     */
    dialogOk(config) {
      this.editRow.dialog = config;
    },

    /**
     * 弹出框点击
     */
    styleClick(item) {
      this.drawer.style.visible = true;
      this.drawer.style.config = item;
      this.editRow = item;
    },

    /**
     * 弹出框规则保存
     */
    styleOk(config) {
      this.editRow.style = config;
    },

    /**
     * 拖拽
     */
    rowDrop() {
      let that = this;
      this.$nextTick(() => {
        const xTable = this.$refs.table;
        if (xTable) {
          Sortable.create(
            xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
            {
              handle: ".drag-btn",
              onEnd: ({ newIndex, oldIndex }) => {
                const currRow = that.columns.splice(oldIndex, 1)[0];
                that.columns.splice(newIndex, 0, currRow);
              },
            }
          );
        }
      })
    },
    /**
     * 删除
     */
    del(row) {
      let spIndex = -1;
      this.columns.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.columns.splice(spIndex, 1);
      }
    },

    /**
     *显示改变
     */
    isShowChange(item, e) {
      item.isShow = e.target.checked;
    },

    /**
     *查询改变
     */
    isSearchChange(item, e) {
      item.isSearch = e.target.checked;
    },

    /**
     *排序改变
     */
    isSortChange(item, e) {
      item.isSort = e.target.checked;
    },

    /**
     *默认改变
     */
    isSortDefalutChange(item, e) {
      item.isSortDefalut = e.target.checked;
    },

    /**
     *Asc改变
     */
    isSortAscChange(item, e) {
      item.isSortAsc = e.target.checked;
    },

    /**
     *合计改变
     */
    isTotalChange(item, e) {
      item.isTotal = e.target.checked;
    },
  },
};
</script>

<style lang="less" scoped>
/deep/ .vxe-table--render-default .vxe-cell {
  padding-left: 6px !important;
  padding-right: 6px !important;
}
</style>