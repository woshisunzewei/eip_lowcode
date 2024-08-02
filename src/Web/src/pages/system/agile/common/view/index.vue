<template>
  <div>
    <vxe-table row-key ref="searchTable" :data="views" :height="height">
      <template #loading>
        <a-spin></a-spin>
      </template>
      <template #empty>
        <eip-empty />
      </template>
      <vxe-column type="seq" title="序号" width="50" align="center"></vxe-column>
      <vxe-column title="排序" width="53px" align="center">
        <template>
          <span class="drag-btn">
            <a-icon type="drag" />
          </span>
        </template>
      </vxe-column>
      <vxe-column title="新增" width="54px" align="center">
        <template #header>
          <a-button @click="add" size="small" type="primary">
            <a-icon type="plus" />
          </a-button>
        </template>
        <template v-slot="{ row }">
          <a-popconfirm title="确定删除查询配置?" ok-text="确定" cancel-text="取消" @confirm="del(row)">
            <a-button @click.stop="" size="small" type="danger">
              <a-icon type="delete"></a-icon>
            </a-button>
          </a-popconfirm>
        </template>
      </vxe-column>

      <vxe-column title="标题" width="130px">
        <template v-slot="{ row }">
          <a-input placeholder="标题" v-model="row.title" />
        </template>
      </vxe-column>
      <vxe-column title="备注" width="180px">
        <template v-slot="{ row }">
          <a-input placeholder="提示说明" v-model="row.备注" />
        </template>
      </vxe-column>

      <vxe-column title="类型" width="170px">
        <template v-slot="{ row }">
          <a-input-group compact>
            <a-select show-search style="width: 100px" v-model="row.type" placeholder="请选择类型"
              @change="typeChange(row, $event)">
              <a-select-option value="table">列表</a-select-option>
              <a-select-option value="sxTable">上下列表</a-select-option>
              <a-select-option value="zyTable">左右列表</a-select-option>
              <a-select-option value="zTableYDetail">左列表右详情</a-select-option>
              <a-select-option value="sTableXDetail">上列表下详情</a-select-option>
              <a-select-option value="bulletinBoard">看板</a-select-option>
              <a-select-option value="calendar">日历</a-select-option>
              <a-select-option value="gallery">画廊</a-select-option>
              <a-select-option value="level">层级</a-select-option>
              <a-select-option value="map">地图</a-select-option>
              <a-select-option value="gantt">甘特图</a-select-option>
            </a-select>
            <a-button @click="typeClick(row)">
              <a-icon type="setting"></a-icon>
            </a-button>
          </a-input-group>
        </template>
      </vxe-column>
      <vxe-column title="冻结" width="180px">
        <template v-slot="{ row }">
          <a-switch default-checked v-model="row.isFreeze" />
        </template>
      </vxe-column>

    </vxe-table>
    <view-filter ref="viewFilter" :visible.sync="view.filter.visible" @cancel="optionsClose"
      @ok="optionsOk"></view-filter>

  </div>
</template>

<script>
import { query } from "@/services/system/datasource/list";
import Sortable from "sortablejs";
import viewFilter from "./filter";
export default {
  name: "view-index",
  components: {
    viewFilter
  },
  data() {
    return {
      height: window.innerHeight - 110,
      views: [],
      view: {
        filter: {
          visible: false,
        }
      },
    };
  },

  props: {
    config: {
      type: Object,
      default: function () {
        return {

        }
      },
    },
  },
  methods: {
    /**
     * 拖拽
     */
    rowDrop() {
      let that = this;
      this.$nextTick(() => {
        const xTable = this.$refs.searchTable;
        if (xTable) {
          this.sortable1 = Sortable.create(
            xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
            {
              handle: ".drag-btn",
              onEnd: ({ newIndex, oldIndex }) => {
                const currRow = that.views.splice(oldIndex, 1)[0];
                that.views.splice(newIndex, 0, currRow);
              },
            }
          );
        }
      })
    },

    /**
     * 新增
     */
    add() {
      this.views.push({
        title: "全部",
        remark: "",
        type: "table",
        config: null,
        isFreeze: false
      });
      this.rowDrop();
    },

    /**
     * 删除
     */
    del(row) {
      let spIndex = -1;
      this.views.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.views.splice(spIndex, 1);
      }
    },

    /**
     * 类型点击
     */
    typeClick(row) {
      this.typeItem = row;
      switch (row.type) {
        case "table":
          this.view.filter.visible = true;
          this.$refs.searchSelect.init(row.options);
          break;

        default:
          this.$message.error("当前类型无配置");
          break;
      }
    },

    /**
     * 类型关闭
     */
    optionsClose() {
      this.drawer.select.visible = false;
      this.drawer.correlationRecord.visible = false;
      this.drawer.datetime.visible = false;
      this.drawer.bool.visible = false;
      this.drawer.user.visible = false;
      this.drawer.organization.visible = false;
      this.drawer.dictionary.visible = false;
    },

    /**
     * 类型下拉改变,改变比较符
     */
    typeChange(row, value) {
      this.typeItem = row;
    },

    /**
     * select下拉配置
     */
    optionsOk(options) {
      this.typeItem.options = options;
      this.optionsClose();
    },
  },
};
</script>

<style lang="less" scoped></style>
