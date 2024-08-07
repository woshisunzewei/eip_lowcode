<template>
  <div>
    <a-form-model-item label="关联工作表">
      <a-select
        :filter-option="filterOption"
        show-search
        v-model="value.dynamicConfig.table"
        placeholder="请选择一个工作表"
        @change="tableChange"
      >
        <a-select-option
          v-for="(item, index) in tables"
          :key="index"
          :value="item.configId"
          >{{ item.name }}</a-select-option
        >
      </a-select>
    </a-form-model-item>

    <a-form-model-item label="显示方式">
      <a-select v-model="value.showType">
        <a-select-option value="table"> 列表 </a-select-option>
        <a-select-option value="select"> 下拉框 </a-select-option>
        <a-select-option value="dialog"> 弹框选择 </a-select-option>
      </a-select>
    </a-form-model-item>

    <a-form-item
      v-if="
        typeof value.dialog !== 'undefined' &&
        typeof value.dialog.width !== 'undefined' &&
        value.showType == 'dialog'
      "
      label="弹出类型"
    >
      <a-select v-model="value.dialog.type">
        <a-select-option value="modal"> 对话框 </a-select-option>
        <a-select-option value="drawer"> 抽屉 </a-select-option>
      </a-select>
    </a-form-item>

    <a-form-item
      v-if="
        typeof value.dialog !== 'undefined' &&
        typeof value.dialog.width !== 'undefined' &&
        value.showType == 'dialog'
      "
      label="弹出框宽度"
    >
      <a-input-group compact>
        <a-input-number
          style="width: 174px"
          v-model="value.dialog.width"
          placeholder="请输入弹出宽度"
        />
        <a-select v-model="value.dialog.widthUnit" style="width: 60px">
          <a-select-option value="px"> px </a-select-option>
          <a-select-option value="%"> % </a-select-option>
        </a-select>
      </a-input-group>
    </a-form-item>

    <a-form-item
      label="弹出选项"
      v-if="
        typeof value.dialog !== 'undefined' &&
        typeof value.dialog.centered !== 'undefined' &&
        value.showType == 'dialog'
      "
    >
      <a-checkbox
        v-if="value.dialog.type == 'modal'"
        v-model="value.dialog.centered"
        >弹出居中</a-checkbox
      >
      <a-checkbox v-model="value.dialog.maskClosable"
        >点击蒙层是否允许关闭</a-checkbox
      >
    </a-form-item>

    <a-form-item
      label="弹出层级数"
      v-if="
        typeof value.dialog !== 'undefined' &&
        typeof value.dialog.zIndex !== 'undefined' &&
        value.showType == 'dialog'
      "
    >
      <a-tooltip>
        <div slot="title">数值越大越在顶部</div>
        <a-input-number
          class="eip-width-full"
          :min="1000"
          v-model="value.dialog.zIndex"
        />
      </a-tooltip>
    </a-form-item>

    <a-form-model-item label="显示列">
      <a-badge
        :dot="value.dynamicConfig.table != undefined"
        style="width: 100%"
      >
        <a-button @click="columnsChosen" block
          ><a-icon type="unordered-list" /> 配置显示列
        </a-button>
      </a-badge>
    </a-form-model-item>

    <a-form-model-item label="数据映射">
      <a-badge :dot="value.map.length > 0" style="width: 100%">
        <a-button @click="mapChosen" block
          ><a-icon type="pull-request" /> 配置数据映射
        </a-button>
      </a-badge>
    </a-form-model-item>

    <a-form-model-item label="过滤范围">
      <a-badge
        :dot="value.filter.rules.length > 0 || value.filter.groups.length > 0"
        style="width: 100%"
      >
        <a-button @click="filterChosen" block
          ><a-icon type="pull-request" /> 配置过滤范围
        </a-button>
      </a-badge>
    </a-form-model-item>

    <a-form-model-item label="查询条件">
      <a-badge :dot="value.search.columns.length > 0" style="width: 100%">
        <a-button @click="searchChosen" block
          ><a-icon type="search" /> 配置查询条件
        </a-button></a-badge
      >
    </a-form-model-item>

    <a-form-model-item label="记录选项">
      <a-checkbox v-model="value.canAdd"> 允许新增记录 </a-checkbox>
      <a-checkbox v-model="value.canDetail"> 允许打开记录 </a-checkbox>
    </a-form-model-item>

    <a-modal
      centered
      :zIndex="99999"
      width="370px"
      v-drag
      :visible="condition.visible"
      :bodyStyle="{ padding: '1px' }"
      :destroyOnClose="true"
      :maskClosable="false"
      :title="condition.title"
      @cancel="conditionCancel"
      :footer="null"
    >
      <a-spin :spinning="condition.tree.spinning">
        <a-row>
          <a-col>
            <a-card class="eip-admin-card-small" size="small">
              <a-directory-tree
                :expandAction="false"
                default-expand-all
                :style="condition.tree.style"
                :tree-data="condition.tree.data"
                @select="onSelect"
                v-if="condition.tree.data.length > 0"
              ></a-directory-tree>
              <a-empty
                :style="condition.tree.style"
                v-if="condition.tree.data.length == 0"
                description="无相关信息"
              />
            </a-card>
          </a-col>
        </a-row>
      </a-spin>
    </a-modal>

    <a-modal
      width="1300px"
      v-drag
      :visible="columns.visible"
      :bodyStyle="{ padding: '1px' }"
      :destroyOnClose="true"
      :maskClosable="false"
      title="显示需要查看列"
      @cancel="columns.visible = false"
    >
      <template slot="footer">
        <a-button key="back" @click="columns.visible = false">关闭</a-button>
      </template>
      <a-row>
        <a-col>
          <a-card class="eip-admin-card-small" size="small">
            <vxe-table
              ref="tableColumns"
              size="small"
              :height="500"
              :data="value.columns"
              row-key
            >
              <template #loading>
                <a-spin></a-spin>
              </template>

              <template #empty>
                <eip-empty />
              </template>
              <vxe-column
                type="seq"
                title="序号"
                align="center"
                width="60"
              ></vxe-column>
              <vxe-column title="排序" width="48" align="center">
                <template>
                  <span class="drag-btn">
                    <a-icon type="drag" />
                  </span>
                </template>
              </vxe-column>
              <vxe-column field="name" title="字段" width="150"> </vxe-column>

              <vxe-column
                field="isSearch"
                title="查询"
                width="60"
                align="center"
              >
                <template v-slot="{ row }">
                  <a-switch
                    checked-children="是"
                    un-checked-children="否"
                    v-model="row.isSearch"
                  />
                </template>
              </vxe-column>
              <vxe-column field="isShow" title="显示" width="70" align="center">
                <template v-slot="{ row }">
                  <a-switch
                    checked-children="是"
                    un-checked-children="否"
                    v-model="row.isShow"
                  />
                </template>
              </vxe-column>
              <vxe-column title="显示名" align="center" width="200">
                <template v-slot="{ row }">
                  <a-space size="small">
                    <a-input
                      allow-clear
                      :placeholder="row.description"
                      v-model="row.title"
                    />
                  </a-space>
                </template>
              </vxe-column>
              <vxe-column
                field="format"
                title="格式化"
                align="center"
                width="120"
              >
                <template v-slot="{ row }">
                  <a-select
                    allow-clear
                    placeholder="格式化"
                    v-model="row.format"
                    style="width: 100px"
                  >
                    <a-select-option
                      v-for="item in eipFormat"
                      :value="item.value"
                      :key="item.value"
                      >{{ item.label }}</a-select-option
                    >
                  </a-select>
                </template>
              </vxe-column>
              <vxe-column title="掩码显示" align="center" width="160">
                <template v-slot="{ row }">
                  <a-input-group compact>
                    <select
                      class="ant-select-selection ant-select-selection--single"
                      v-model="row.mask"
                      placeholder="掩码显示"
                      style="width: 100%"
                    >
                      <option value=""></option>
                      <option
                        v-for="item in eipMask"
                        :value="item.value"
                        :title="item.label"
                        :key="item.value"
                      >
                        {{ item.label }}
                      </option>
                    </select>
                  </a-input-group>
                </template>
              </vxe-column>
              <vxe-column field="fixed" title="固定" align="center" width="120">
                <template v-slot="{ row }">
                  <a-select
                    allow-clear
                    placeholder="请选择固定"
                    v-model="row.fixed"
                    style="width: 100px"
                  >
                    <a-select-option value="left">左</a-select-option>
                    <a-select-option value="right">右</a-select-option>
                  </a-select>
                </template>
              </vxe-column>
              <vxe-column title="宽度(px)" align="center" width="110">
                <template v-slot="{ row }">
                  <a-space size="small">
                    <a-input-number
                      placeholder="请输入宽度"
                      v-model="row.width"
                    />
                  </a-space>
                </template>
              </vxe-column>
              <vxe-column field="sord" title="排序" width="60" align="center">
                <template v-slot="{ row }">
                  <a-switch
                    checked-children="是"
                    un-checked-children="否"
                    v-model="row.sord"
                  />
                </template>
              </vxe-column>
              <vxe-column
                field="sordType"
                title="排序类型"
                align="center"
                width="120"
              >
                <template v-slot="{ row }">
                  <a-select
                    allow-clear
                    v-model="row.sordType"
                    style="width: 100px"
                    placeholder="请选择排序类型"
                  >
                    <a-select-option value="desc">降序</a-select-option>
                    <a-select-option value="asc">升序</a-select-option>
                  </a-select>
                </template>
              </vxe-column>
            </vxe-table>
          </a-card>
        </a-col>
      </a-row>
    </a-modal>

    <a-modal
      width="800px"
      v-drag
      :visible="map.visible"
      :bodyStyle="{ padding: '1px' }"
      :destroyOnClose="true"
      :maskClosable="false"
      title="配置数据映射"
      @cancel="map.visible = false"
    >
      <template slot="footer">
        <a-button key="back" @click="map.visible = false">关闭</a-button>
      </template>
      <a-row>
        <a-col>
          <a-card class="eip-admin-card-small" size="small">
            <vxe-table
              ref="tableMap"
              id="tableMap"
              size="small"
              :height="500"
              :data="value.map"
            >
              <template #loading>
                <a-spin></a-spin>
              </template>

              <template #empty>
                <eip-empty />
              </template>
              <vxe-column
                type="seq"
                title="序号"
                align="center"
                width="60"
              ></vxe-column>

              <vxe-column field="name" title="字段" width="200"> </vxe-column>
              <vxe-column
                field="description"
                title="描述"
                width="130"
                showOverflow="ellipsis"
              >
              </vxe-column>
              <vxe-column
                field="to"
                title="至"
                width="40"
                align="center"
                showOverflow="ellipsis"
              >
              </vxe-column>
              <vxe-column title="映射字段" align="center" width="350">
                <template v-slot="{ row }">
                  <a-space size="small">
                    <eip-editor
                      style="width: 200px"
                      :ref="'editor_' + row.name"
                      v-model="row.map"
                      content_style="p{margin:5px} body{margin:4px} .mce-content-body:not([dir=rtl])[data-mce-placeholder]:not(.mce-visualblocks)::before{left:6px}"
                      :height="tinymce.height"
                      :toolbar="tinymce.toolbar"
                      :plugins="tinymce.plugins"
                      :menubar="tinymce.menubar"
                    ></eip-editor>

                    <a-button @click="conditionSetting('map', row)">
                      <a-icon type="setting" />
                    </a-button>
                  </a-space>
                </template>
              </vxe-column>
            </vxe-table>
          </a-card>
        </a-col>
      </a-row>
    </a-modal>

    <a-drawer
      v-if="search.visible"
      width="1000px"
      :bodyStyle="{ padding: '1px' }"
      title="查询条件"
      :visible="search.visible"
      @close="search.visible = false"
      :destroyOnClose="true"
    >
      <search-index
        ref="search"
        v-if="search.visible"
        :visible.sync="search.visible"
        :config="search.config"
        :columns="search.columns"
        @ok="searchOk"
      />
    </a-drawer>

    <a-modal
      :zIndex="1003"
      width="670px"
      v-drag
      :visible="filter.visible"
      :bodyStyle="{ padding: '1px' }"
      :destroyOnClose="true"
      :maskClosable="false"
      title="过滤选择范围"
      @cancel="filter.visible = false"
      @ok="filterOk"
    >
      <div style="height: 500px; overflow: auto">
        <eip-rule
          ref="rule"
          :filters="filter.config"
          :columns="filter.columns"
          :dataColumns="filter.dataColumns"
        ></eip-rule>
      </div>
    </a-modal>
  </div>
</template>

<script>
import { table } from "@/services/system/agile/form/designer";
import searchIndex from "@/pages/system/agile/common/search/index";
import Sortable from "sortablejs";
/*
 * author 孙泽伟
 * date 2024-03-28
 * description 关联记录
 */
export default {
  name: "KCorrelationRecord",
  components: {
    searchIndex,
  },
  data() {
    return {
      tinymce: {
        plugins: "",
        toolbar: "",
        height: 48,
        menubar: "",
      },
      bodyStyle: {
        padding: "4px",
        "background-color": "#f0f2f5",
      },
      labelCol: { span: 4 },
      wrapperCol: { span: 20 },
      tables: [], //所有工作表
      condition: {
        title: "选择字段",
        visible: false,

        tree: {
          row: null,
          style: {
            overflow: "auto",
            height: "400px",
          },
          data: [],
          spinning: false,
        },
      },

      loading: false,

      columns: {
        visible: false,
      },
      filter: {
        visible: false,
        columns: [],
        dataColumns: [],
        config: {
          groupOp: "AND",
          rules: [],
          groups: [],
        },
      },

      map: {
        visible: false,
      },

      search: {
        visible: false,
        columns: [],
        config: {
          labelCol: 8,
          wrapperCol: 16,
          num: 4, //显示个数
          columns: [], //查询条件
        },
      },

      sortable: null,
    };
  },
  props: {
    value: {
      type: Object,
      required: true,
    },
  },
  beforeDestroy() {
    if (this.sortable) {
      this.sortable.destroy();
    }
  },
  created() {
    this.initTable();
    this.conditionInit();
    this.rowDrop();
  },
  methods: {
    /**
     * 工作表切换
     */
    tableChange(e) {
      let that = this;
      let data = this.$utils.find(this.tables, (f) => f.configId == e);
      if (data && data.columnJson) {
        this.value.map = [];
        this.value.columns = [];
        this.value.search = {
          labelCol: 8,
          wrapperCol: 16,
          num: 6, //显示个数
          columns: [],
        }; //查询条件;

        let columns = JSON.parse(data.columnJson);

        columns.forEach((item) => {
          this.value.columns.push({
            name: item.model,
            description: item.label,
            format: that.getFormat(item),
            mask: "",
            isSearch: false,
            isShow: false,
            sord: false,
            sordType: undefined,
            title: item.label,
            fixed: undefined,
            width: 100,
            options: item.options,
            type: item.type,
          });
        });

        that.eipSystemColumns.forEach((systemColumn) => {
          this.value.columns.push({
            name: systemColumn.name,
            description: systemColumn.description,
            format: undefined,
            mask: "",
            isSearch: false,
            isShow: false,
            sord: false,
            sordType: undefined,
            title: systemColumn.description,
            fixed: undefined,
            width: 100,
          });
        });
        that.value.dynamicConfig.title = data.name;
      }
      this.rowDrop();
    },
    /**
     * 拖拽
     */
    rowDrop() {
      let that = this;
      this.$nextTick(() => {
        const xTable = this.$refs.tableColumns;
        if (xTable) {
          that.sortable = Sortable.create(
            xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
            {
              handle: ".drag-btn",
              onEnd: ({ newIndex, oldIndex }) => {
                const currRow = that.value.columns.splice(oldIndex, 1)[0];
                that.value.columns.splice(newIndex, 0, currRow);
              },
            }
          );
        }
      });
    },
    /**
     * 获取默认格式化
     * @param {*} type
     */
    getFormat(item) {
      switch (item.type) {
        case "uploadImg":
        case "uploadFile":
          return "File";
        case "batch":
          return "Batch";
        case "user":
          return "User";
        case "organization":
          return "Organization";
        case "map":
          return "Map";
        case "dataShow":
          switch (item.options.type) {
            case "qrCode":
              return "QrCode";
            case "barCode":
              return "BarCode";
            case "img":
              return "Image";
            case "label":
              return "Label";
          }
          return "";
        default:
          return undefined;
      }
    },
    /**
     *
     * @param {搜索} input
     * @param {*} option
     */
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text
          .toLowerCase()
          .indexOf(input.toLowerCase()) >= 0
      );
    },

    /**
     * 初始化所有的工作表
     */
    initTable() {
      let that = this;
      table().then(function (result) {
        that.tables = result.data;
      });
    },

    /**
     * 查询配置点击
     */
    searchChosen() {
      let that = this;
      this.search.columns = [];
      var columns = this.$utils
        .clone(this.value.columns, true)
        .filter((f) => f.isShow || f.isSearch);
      columns.forEach((item) => {
        that.search.columns.push({
          name: item.name,
          description: item.description,
          isShow: item.isShow,
        });
      });
      this.search.config = this.value.search;
      this.search.visible = true;
    },

    /**
     * 查询配置保存
     */
    searchOk(config) {
      this.value.search = config;
    },

    /**
     * 显示列源选择
     */
    mapChosen() {
      let that = this;
      this.tableChange(this.value.dynamicConfig.table);
      var columns = this.$utils.clone(this.value.columns, true);
      //移除已不在里面项
      that.value.map.forEach((f, index) => {
        var have = columns.filter((c) => c.name == f.name);
        if (have.length == 0) {
          that.value.map.splice(index, 1);
        }
      });
      columns.forEach((item, index) => {
        //判断字段是否存在
        var have = that.value.map.filter((f) => f.name == item.name);
        if (have.length == 0) {
          that.value.map.push({
            index: index,
            name: item.name,
            description: item.title,
            to: "->",
            map: "",
          });
        }
      });
      this.map.visible = true;
    },

    /**
     * 过滤选择
     */
    filterChosen() {
      var columns = this.$utils.clone(this.value.columns, true);
      this.filter.columns = [];
      columns.forEach((item) => {
        var type = item.type ? item.type : "input";
        this.filter.columns.push({
          title: item.description,
          field: item.name,
          type: type,
        });
      });
      var treeData = this.condition.tree.data;
      let treeList = this.$utils.toTreeArray(treeData);

      treeList.forEach((item) => {
        this.filter.dataColumns.push({
          title: item.title,
          field: item.key,
        });
      });

      this.filter.config = this.value.filter;
      this.filter.visible = true;
    },

    /**
     *
     */
    filterOk() {
      this.value.filter = this.filter.config;
      this.filter.visible = false;
    },

    /**
     * 初始化条件
     */
    conditionInit() {
      let that = this;

      that.condition.tree.spinning = true;
      let fieldSchema = this.eipAgileDesigner.kfd.getFieldSchema();
      fieldSchema
        .filter((f) => !["batch", "text", "divider"].includes(f.type))
        .forEach((item) => {
          that.condition.tree.data.push({
            key: item.key,
            isLeaf: true,
            title: item.label != "" ? item.label : item.model,
          });
        });

      //得到子表
      fieldSchema
        .filter((f) => f.type == "batch")
        .forEach((item) => {
          var childrens = [];
          if (item.list.length > 0) {
            var children = {
              title: item.label != "" ? item.label : item.model,
            };

            item.list.forEach((control) => {
              childrens.push({
                key: item.key + "." + control.key,
                isLeaf: true,
                title:
                  control.label != ""
                    ? "(" + control.label + ")"
                    : control.model,
              });
            });
            children.children = childrens;
            that.condition.tree.data.push(children);
          }
        });
      that.condition.tree.data.push(this.eipSystemFiled);
      that.condition.tree.spinning = false;
    },

    /**
     * 显示列源选择
     */
    columnsChosen() {
      this.columns.visible = true;
      this.rowDrop();
    },

    /**
     * 条件配置
     */
    conditionSetting(type, row) {
      //得到所有组件
      this.condition.tree.type = type;
      this.condition.tree.row = row;
      this.condition.visible = true;
    },

    /** */
    conditionCancel() {
      this.condition.visible = false;
    },

    /**
     * 选择
     */
    onSelect(keys, event) {
      var treeData = this.condition.tree.data;
      let treeList = this.$utils.toTreeArray(treeData);
      var chosenColumn = this.$utils.find(treeList, (f) => f.key == keys[0]);

      if (event.node.getNodeChildren().length == 0) {
        if (typeof this.condition.tree.row.condition != "undefined") {
          let html =
            "<span id='" +
            encodeURIComponent(
              JSON.stringify({
                type: "column",
                column: chosenColumn.key,
              })
            ) +
            "' class='non-editable'>" +
            chosenColumn.key +
            "</span>";
          this.$refs["editor_" + this.condition.tree.row.name].insertIndex(
            html,
            0
          );
        }
        if (typeof this.condition.tree.row.map != "undefined") {
          let html =
            "<span id='" +
            encodeURIComponent(
              JSON.stringify({
                type: "column",
                column: chosenColumn.key,
              })
            ) +
            "' class='non-editable'>" +
            chosenColumn.title +
            "</span>";
          this.$refs["editor_" + this.condition.tree.row.name].insertIndex(
            html,
            this.condition.tree.row.index
          );
        }
        this.conditionCancel();
      }
    },

    addItem() {
      this.edit.title = "新增数据源配置";
      this.edit.visible = true;
    },
  },
};
</script>

<style lang="less" scoped>
.ant-form-item {
  margin-bottom: 0;
}
</style>
