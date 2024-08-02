<template>
  <a-drawer
    width="1500"
    placement="right"
    :visible="visible"
    :closable="false"
    :bodyStyle="{ padding: '0' }"
    :destroyOnClose="true"
    @close="cancel"
  >
    <div class="eip-form-designer-container">
      <div class="operating-area">
        <div class="left-btn-box">
          <a-tooltip title="若数据源属于敏捷开放设计,则会自动过滤Id字段">
            <a @click="tableReset">
              <a-icon type="retweet" />
              <span>重置</span>
            </a>
          </a-tooltip>
          <a-divider type="vertical" />
          <a-tooltip
            title="根据选择的数据源进行重置,如根据表则查询所有的表结构"
          >
            <a @click="tableResetByDataFromName">
              <a-icon type="retweet" />
              <span>数据源重置</span>
            </a>
          </a-tooltip>
          <a-divider type="vertical" />
          <a-tooltip title="保存">
            <a @click="tableSave">
              <a-icon type="save" />
              <span>保存</span>
            </a>
          </a-tooltip>
          <a-divider type="vertical" />
          <a-tooltip title="发布">
            <a @click="tablePublic">
              <a-icon type="build" />
              <span>发布</span>
            </a>
          </a-tooltip>
        </div>
        <div class="right-btn-box">
          <a-tooltip :title="title"
            ><span class="text-bold">{{ title }}</span></a-tooltip
          >
          <a-divider type="vertical" />
          <a-tooltip title="关闭">
            <a @click="cancel" style="color: #ff4d4f">
              <a-icon type="close" style="font-size: 14px" />
              <span>关闭</span>
            </a>
          </a-tooltip>
        </div>
      </div>
      <!-- 操作区域 end -->
      <div class="content toolbars-top">
        <a-spin :spinning="loading">
          <a-tabs
            :default-active-key="tabkey"
            v-model="tabkey"
            tab-position="left"
          >
            <a-tab-pane key="1">
              <span slot="tab"><a-icon type="form" />配置 </span>

              <div class="tab-margin">
                <column-index
                  ref="column"
                  :configId="configId"
                  :columns="list.columns"
                  :subtable="list.subtable"
                  :controls="list.controls"
                />
              </div>
            </a-tab-pane>
            <a-tab-pane key="2">
              <span slot="tab"
                ><a-badge :count="list.filter.length">
                  <a-icon type="filter" />过滤</a-badge
                >
              </span>
              <div class="tab-margin">
                <filter-index
                  ref="filter"
                  :columns="list.columns"
                  :filter="list.filter"
                />
              </div>
            </a-tab-pane>
            <a-tab-pane key="3">
              <span slot="tab"
                ><a-badge :count="list.search.columns.length">
                  <a-icon type="search" />查询</a-badge
                >
              </span>
              <div class="tab-margin">
                <search-index
                  ref="search"
                  :columns="list.columns"
                  :config="list.search"
                />
              </div>
            </a-tab-pane>
            <a-tab-pane key="4">
              <span slot="tab"> <a-icon type="highlight" />样式 </span>
              <div class="tab-margin">
                <style-index
                  ref="style"
                  :columns="list.columns"
                  :config="list.style"
                />
              </div>
            </a-tab-pane>
            <a-tab-pane key="6">
              <span slot="tab"> <a-icon type="mobile" />手机 </span>
              <phone-index ref="phone" :columns="list.columns" />
            </a-tab-pane>
            <a-tab-pane key="7">
              <span slot="tab"> <a-icon type="edit" />操作 </span>
              <action-index ref="edit" :config="list.action" />
            </a-tab-pane>
            <a-tab-pane key="8">
              <span slot="tab"> <a-icon type="audio" />读取 </span>
              <read-index ref="edit" :config="list.read" />
            </a-tab-pane>
          </a-tabs>
        </a-spin>
      </div>
    </div>
  </a-drawer>
</template>

<script>
import {
  findById,
  column,
  query,
  findDataFromName,
  findWorkflowById,
} from "@/services/system/agile/list/designer";
import "@/pages/system/agile/form/components/styles/form-design.less";
import columnIndex from "@/pages/system/agile/common/column/index";
import searchIndex from "@/pages/system/agile/common/search/index";
import filterIndex from "@/pages/system/agile/common/filter/index";
import styleIndex from "@/pages/system/agile/common/style/index";
import phoneIndex from "@/pages/system/agile/common/phone/index";
import actionIndex from "@/pages/system/agile/common/action/index";
import readIndex from "@/pages/system/agile/common/read/index";
import Sortable from "sortablejs";
import { mapMutations, mapGetters } from "vuex";
import { listSave, listPublic } from "@/services/system/agile/list/designer";
export default {
  name: "businessdatadesigner",
  components: {
    actionIndex,
    columnIndex,
    searchIndex,
    filterIndex,
    styleIndex,
    phoneIndex,
    readIndex,
  },
  computed: {
    ...mapGetters("account", ["systemAgile"]),
  },
  data() {
    return {
      loading: false,
      tabkey: "1",
      bodyStyle: {
        padding: "0",
      },
      form: {
        configId: "", //配置Id
        menuId: "", //菜单Id
      },

      list: this.$utils.clone(this.eipTableConfig, true),

      editRow: {}, //当前编辑行
      systemColumns: this.eipSystemColumns,
    };
  },
  created() {
    this.find();
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    configId: String,
    title: {
      type: String,
    },
  },

  methods: {
    /**
     * 根据Id查找
     */
    async find() {
      let that = this;
      that.loading = true;
      let param = {
        limit: 9999,
        sidx: "OrderNo",
        configType: 1,
      };

      //查询所有子表
      var result = await query(param);
      if (result.code == that.eipResultCode.success) {
        result.data.forEach((item) => {
          item.condition = null;
        });
        that.list.subtable = result.data;
      }

      result = await findById(that.configId);

      let form = result.data;
      let controls = [];

      //判断表是否为敏捷开发来
      var systemAgileData = that.systemAgile.filter(
        (f) => f.configId == result.data.editConfigId
      );
      var findResult = null;
      if (systemAgileData.length > 0) {
        findResult = {
          code: 200,
          data: {
            columnJson: systemAgileData[0].columnJson,
          },
          msg: "存在配置表",
        };
      } else {
        findResult = await findDataFromName({
          dataFromName: form.dataFromName,
        });
      }
      var workflow = await findWorkflowById(form.menuId);
      if (workflow.data != null && workflow.data.publicJson) {
        that.systemColumns.push({
          name: "WorkflowStatus",
          description: "状态",
          isShow: false,
          isSearch: false,
        });
        that.systemColumns.push({
          name: "WorkflowSn",
          description: "流水号",
          isShow: false,
          isSearch: false,
        });
        that.systemColumns.push({
          name: "WorkflowTitle",
          description: "标题",
          isShow: false,
          isSearch: false,
        });
      }

      if (findResult.code === that.eipResultCode.success) {
        let columnJson = JSON.parse(findResult.data.columnJson).filter(
          (f) => !["text", "divider"].includes(f.type)
        );
        //所有字段
        columnJson.forEach((item) => {
          //判断是否需要添加后缀
          switch (item.type) {
            case "checkbox":
            case "radio":
            case "select":
            case "user":
            case "post":
            case "dictionary":
            case "organization":
            case "cascader":
            case "treeSelect":
            case "correlationRecord":
            case "district":
              item.model += "_Txt";
              break;
          }
          controls.push({
            name: item.model,
            description: item.label,
            type: item.type,
            options: item.options,
          });
        });
        that.systemColumns.forEach((systemColumn) => {
          controls.push({
            name: systemColumn.name,
            description: systemColumn.description,
            type: null,
            options: null,
          });
        });
      } else {
        result = await column({
          dataBaseId: form.dataBaseId,
          name: form.dataFromName,
          dataType: this.getDataType(form.dataFrom),
        });
        result.data.forEach((item) => {
          controls.push({
            name: item.name,
            description: item.description,
            type: null,
            options: null,
          });
        });
      }
      that.list.controls = controls;

      let addColumns = [];
      //如果已配置列表选项
      if (form.saveJson) {
        let configColumns = JSON.parse(form.saveJson);
        configColumns.columns.forEach((item) => {
          //判断是否已经删除字段
          let haveColumns = controls.filter((f) => f.name == item.name);
          if (haveColumns.length > 0) {
            that.list.columns.push(item);
          }
        });

        controls.forEach((item) => {
          //是否已配置
          let haveColumns = configColumns.columns.filter(
            (f) => f.name == item.name
          );
          if (haveColumns.length == 0) {
            addColumns.unshift(item);
          }
        });

        addColumns.forEach((item) => {
          that.list.columns.unshift(that.getColumn(item));
        });
      } else {
        let columns = [];
        controls.forEach((item) => {
          columns.push(that.getColumn(item));
        });
        that.list.columns = columns;
      }

      if (form.saveJson) {
        var saveJson = JSON.parse(form.saveJson);
        that.list.style = saveJson.style;
        that.list.filter = saveJson.filter;
        that.list.search = saveJson.search;
        that.list.phone = saveJson.phone;
        that.list.action =
          saveJson.action == undefined
            ? { cellDbClick: false }
            : saveJson.action;
        that.list.read =
          saveJson.read == undefined ? { cache: true } : saveJson.read;
      }

      that.loading = false;
    },

    /**
     * 获取数据类型
     */
    getDataType(dataFrom) {
      let dataType = "";
      switch (dataFrom) {
        case 0:
          dataType = "table";
          break;
        case 1:
          dataType = "view";
          break;
        case 2:
          dataType = "proc";
          break;
        case 3:
          dataType = "api";
          break;
      }
      return dataType;
    },

    /**
     * 获取列
     */
    getColumn(item) {
      let column = {};
      //是否系统字段
      var systemColumnFilter = this.systemColumns.filter(
        (f) => f.name == item.name
      );
      column.isShow =
        systemColumnFilter.length > 0
          ? systemColumnFilter.filter((f) => f.isShow).length > 0
          : true;
      column.isSearch =
        systemColumnFilter.length > 0
          ? systemColumnFilter.filter((f) => f.isSearch).length > 0
          : true;
      column.isSort = true;
      column.isSortDefalut = false;
      column.isSortAsc = false;
      column.isTotal = false;
      column.name = item.name;
      column.description = item.description;
      column.remark = item.remark;
      column.width = 150;
      column.align = "left";
      column.fixed = undefined;
      column.type = item.type;
      column.options = item.options;
      column.format = this.getFormat(item);
      column.mask = undefined;
      column.subtable = [];
      column.dialog = {
        id: undefined,
        condition: null,
        height: 200,
        heightUnit: ["px"],
        width: 300,
        widthUnit: ["px"],
      };
      column.header = {
        title: null,
        field: [],
      };
      column.style = [];
      column.script = item.script;
      return column;
    },

    /**
     * 获取默认格式化
     * @param {*} type
     */
    getFormat(item) {
      switch (item.type) {
        case "sign":
          return "Image";
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
        case "correlationRecord":
          return "CorrelationRecord";
        case "treeSelect":
          return "TreeSelect";
        case "dictionary":
          return "Dictionary";
        case "district":
          return "District";
        case "rate":
          return "Rate";
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
        case "switch":
          return "Switch";
        default:
          return undefined;
      }
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     *
     */
    async tableReset() {
      let that = this;
      that.loading = true;
      let param = {
        limit: 9999,
        sidx: "OrderNo",
        configType: 1,
      };
      //查询所有子表
      query(param).then(function (result) {
        result.data.forEach((item) => {
          item.condition = null;
        });
        that.list.subtable = result.data;
      });

      await findById(that.configId).then(async (result) => {
        let form = result.data;
        let controls = [];

        var findResult = await findDataFromName({
          dataFromName: form.dataFromName,
        });
        if (findResult.code === that.eipResultCode.success) {
          var columnJson = JSON.parse(findResult.data.columnJson).filter(
            (f) => !["text"].includes(f.type)
          );
          //所有字段
          columnJson.forEach((item) => {
            //判断是否需要添加后缀
            switch (item.type) {
              case "checkbox":
              case "radio":
              case "select":
              case "user":
              case "post":
              case "dictionary":
              case "organization":
              case "cascader":
              case "treeSelect":
              case "correlationRecord":
              case "district":
                item.model += "_Txt";
                break;
            }
            controls.push({
              name: item.model,
              description: item.label,
              type: item.type,
              options: item.options,
            });
          });
          that.systemColumns.forEach((systemColumn) => {
            controls.push({
              name: systemColumn.name,
              description: systemColumn.description,
              type: null,
              options: null,
            });
          });
        } else {
          var resultColumns = await column({
            dataBaseId: form.dataBaseId,
            name: form.dataFromName,
          });
          resultColumns.data.forEach((item) => {
            controls.push({
              name: item.name,
              description: item.description,
              type: null,
              options: null,
            });
          });
        }

        that.form.configId = that.configId;
        let columns = [];
        controls.forEach((item) => {
          columns.push(that.getColumn(item));
        });
        that.list.columns = columns;
        that.loading = false;
        that.rowDrop();
      });
    },

    /**
     *
     */
    tableResetByDataFromName() {
      let that = this;

      that.loading = true;
      let param = {
        limit: 9999,
        sidx: "OrderNo",
        configType: 1,
      };
      //查询所有子表
      query(param).then(function (result) {
        result.data.forEach((item) => {
          item.condition = null;
        });
        that.list.subtable = result.data;
      });

      findById(that.configId).then(function (result) {
        let form = result.data;
        that.form.configId = that.configId;
        //所有列
        column({
          name: form.dataFromName,
        }).then((result) => {
          let columns = [];
          result.data.forEach((item) => {
            if (item.name.indexOf("_RelationId") < 0) {
              let column = {};
              column.isShow = true;
              column.isSearch = true;
              column.isSort = true;
              column.isSortDefalut = false;
              column.isSortAsc = false;
              column.isTotal = false;
              column.name = item.name;
              column.description = item.description;
              column.remark = item.remark;
              column.width = 150;
              column.align = "left";
              column.fixed = null;
              column.format = that.getFormat(item);
              column.mask = undefined;
              column.subtable = [];
              column.script = item.script;
              column.dialog = {
                id: undefined,
                condition: null,
                height: 200,
                heightUnit: ["px"],
                width: 300,
                widthUnit: ["px"],
              };
              column.header = {
                title: null,
                field: [],
              };
              column.style = [];
              columns.push(column);
            }
          });
          that.list.columns = columns;
          that.loading = false;
        });

        that.rowDrop();
      });
    },

    /**
     * 拖拽
     */
    rowDrop() {
      let that = this;
      this.$nextTick(() => {
        const xTable = this.$refs.column;
        if (xTable) {
          this.sortable1 = Sortable.create(
            xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
            {
              handle: ".drag-btn",
              onEnd: ({ newIndex, oldIndex }) => {
                const currRow = that.list.columns.splice(oldIndex, 1)[0];
                that.list.columns.splice(newIndex, 0, currRow);
              },
            }
          );
        }
      });
    },

    ...mapMutations("account", ["setSystemAgile"]),

    /**
     * 表格配置保存
     */
    tableSave() {
      let that = this;
      let form = {
        configId: this.configId,
        saveJson: JSON.stringify(this.list),
      };
      that.$loading.show({ text: "保存中,请稍等..." });
      listSave(form).then(function (result) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 表格配置发布
     */
    tablePublic() {
      let that = this;
      const modal = this.$confirm({
        okText: "确定",
        okType: "danger",
        cancelText: "取消",
        title: "表单发布提示",
        content: "若已发布会覆盖原来结构，且不可恢复？",
        onOk() {
          modal.destroy();
          let form = {
            configId: that.configId,
            saveJson: JSON.stringify(that.list),
            publicJson: JSON.stringify(that.list),
          };
          that.$loading.show({ text: "发布中,请稍等..." });
          listPublic(form).then(function (result) {
            that.$loading.hide();
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              var systemAgileData = that.systemAgile.filter(
                (f) => f.configId == form.configId
              );
              if (systemAgileData && systemAgileData.length > 0) {
                systemAgileData[0].publicJson = form.publicJson;
                that.setSystemAgile(that.systemAgile);
              }
              that.$emit("public", form);
            } else {
              that.$message.error(result.msg);
            }
          });
        },
        onCancel() {},
      });
    },

    /**
     * 表格预览
     */
    tablePreview() {},
  },
};
</script>

<style lang="less" scoped>
.close-box {
  bottom: 100px !important;
}

.eip-form-designer-container .content section {
  -webkit-box-flex: 1;
  -ms-flex: 1;
  flex: 1;
  max-width: calc(100% - 270px);
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
  margin: 0 8px 0;
  -webkit-box-shadow: 0px 0px 1px 1px #ccc;
  box-shadow: 0px 0px 1px 1px #ccc;
}

.eip-form-designer-container .content aside.right .ant-form-item {
  margin-bottom: 0;
}

.tab-margin {
  margin: 5px;
}

/deep/ .ant-tabs-bar {
  margin: 0 0 4px 0 !important;
}

/deep/ .ant-tabs {
  height: calc(100vh - 57px) !important;
  box-shadow: 0px 0px 1px 1px #ccc;
}

/deep/ .ant-tabs .ant-tabs-left-content {
  border-left: 0 !important;
  padding-left: 0 !important;
}

.ant-tabs-left-bar {
  border-right: 1px solid #ccc;
}

/deep/ .ant-tabs-tab {
  padding: 8px 14px !important;
}
</style>
