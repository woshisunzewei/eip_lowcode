<template>
  <a-drawer width="100%" :destroyOnClose="true" placement="right" :closable="false" :visible="visible"
    :bodyStyle="{ padding: '0px' }" @cancel="cancel">
    <a-spin :spinning="spinning">
      <k-form-design ref="kfd" toolbarsTop :destroyOnClose="true" :showToolbarsText="true" :showHead="false"
        :toolbars="design.toolbars" @save="editFormSave">
        <!-- 左侧操作区域插槽 start -->
        <template slot="left-action">
          <a-divider type="vertical" />
          <a-tooltip title="直接发布" @click="editFormPublic(false)">
            <a>
              <a-icon type="build" />
              <span>直接发布</span>
            </a>
          </a-tooltip>
          <a-divider type="vertical" />
          <a-tooltip title="修改发布" @click="editFormPublic(true)">
            <a>
              <a-icon type="edit" />
              <span>修改发布</span>
            </a>
          </a-tooltip>
        </template>
        <!-- 左侧操作区域插槽 end -->
        <!-- 右侧操作区域插槽 start -->
        <template slot="right-action">
          <div v-if="!this.fromApp">
            <a-tooltip :title="form.name"><span class="text-bold">{{ form.name }}</span></a-tooltip>
            <a-divider type="vertical" />
            <a-tooltip title="关闭">
              <a @click="cancel" style="color: #ff4d4f">
                <a-icon type="close" style="font-size: 14px;" />
                <span>关闭</span>
              </a>
            </a-tooltip>
          </div>
        </template>
        <!-- 右侧操作区域插槽 end -->
      </k-form-design>
    </a-spin>

    <a-modal :zIndex="1005" width="1100px" v-drag :visible="publicForm.visible" :bodyStyle="{ padding: '1px' }"
      :destroyOnClose="true" :maskClosable="false" title="发布确认信息" @cancel="publicForm.visible = false">
      <a-tag color="pink">
        系统默认字段: Id,RelationId,Status,CreateUserId,CreateUserName,CreateTime,UpdateUserId,UpdateUserName,UpdateTime
      </a-tag>
      <vxe-toolbar ref="toolbar" custom print export>
        <template v-slot:buttons>
          <a-space class="margin-left-sm">
            <a-button icon="fullscreen" type="primary" @click="$refs.table.setAllTreeExpand(true)">展开所有</a-button>
            <a-button icon="fullscreen-exit" type="primary" @click="$refs.table.clearTreeExpand()">关闭所有</a-button>
            <a-button icon="edit" type="danger" @click="createModelName">生成所有字段拼音</a-button>
          </a-space>
        </template>
      </vxe-toolbar>
      <vxe-table ref="table"
        :tree-config="{ expandAll: true, transform: true, rowField: 'key', parentField: 'pid', trigger: 'row' }"
        height="600px" :data="publicForm.columns">
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column field="description" tree-node title="描述" min-width="190" showOverflow="ellipsis">
          <template #default="{ row }">
            <a-input :placeholder="row.description" :allowClear="true" v-model="row.description">
              <a-tooltip slot="suffix" title="点击去翻译">
                <a :href="'https://fanyi.baidu.com/translate?query=' + row.description + '&lang=auto2zh#zh/en/' + row.description"
                  target="_blank">翻译</a>
              </a-tooltip></a-input>
          </template>
        </vxe-column>
        <vxe-column field="id" title="字段名称" width="200" showOverflow="ellipsis">
          <template #default="{ row }">
            <a-input :placeholder="row.name" :allowClear="true" v-model="row.name" />
          </template>
        </vxe-column>
        <vxe-column field="null" title="为空" width="80" align="center">
          <template #default="{ row }">
            <a-switch v-model="row.null" />
          </template>
        </vxe-column>
        <vxe-column field="id" title="字段类型" width="200" showOverflow="ellipsis">
          <template #default="{ row }">
            <a-input v-if="row.controlType != 'batch'" :placeholder="row.name" :allowClear="true"
              v-model="row.dataType" />
            <a-tag color="#f50" v-else>子表</a-tag>
          </template>
        </vxe-column>
        <vxe-column field="id" title="控件类型" width="150" showOverflow="ellipsis">
          <template #default="{ row }">
            {{ row.controlType }}
          </template>
        </vxe-column>

        <vxe-column field="id" title="模版字段" align="center" width="100" showOverflow="ellipsis">
          <template #default="{ row }">
            <a-tooltip :title="row.copy">
              <a-icon class="configIdCopy" :data-clipboard-text="row.copy" @click="configIdCopy()"
                type="copy"></a-icon></a-tooltip>

          </template>
        </vxe-column>
      </vxe-table>
      <template slot="footer">
        <a-space>
          <a-button @click="publicForm.visible = false"><a-icon type="close" />取消发布</a-button>
          <a-button key="submit" @click="editFormPublicSure" type="primary"><a-icon type="save" />立即发布</a-button>
        </a-space>
      </template>
    </a-modal>
  </a-drawer>
</template>

<script>
import Clipboard from "clipboard";
import { mapMutations, mapGetters } from "vuex";
import {
  findById,
  editSave,
  editPublic,
  tableField,
} from "@/services/system/agile/form/designer";
import jsonFormat from "@/pages/system/agile/form/components/packages/components/KFormDesign/config/jsonFormat";
export default {
  name: "businessdatadesigner",
  data() {
    return {
      design: {
        toolbars: [
          "save",
          "preview",
          "importJson",
          "exportJson",
          "exportCode",
          "undo",
          "redo",
          "reset",
        ],
      },

      form: {
        configId: "",
        name: "",
      },

      loading: false,
      spinning: false,

      controls: [],
      edit: {
        controls: [], //所有控件信息
      },

      publicForm: {
        visible: false,//发布页面是否显示
        columns: []
      }
    };
  },
  computed: {
    ...mapGetters("account", ["systemAgile"]),
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    fromApp: {
      type: Boolean,
      default: false,
    }, //是否从App处来
    configId: String,
    title: {
      type: String,
    },

    publicField: {
      type: Boolean,
      default: true,//是否发布字段(有些情况是不需要发布具体的字段)
    },
  },
  mounted() {
    this.$nextTick(function () {
      this.find();
      if (this.$refs.table) {
        this.$refs.table.connect(this.$refs.toolbar);
      }
    })
  },
  methods: {
    /**
       * 
       */
    configIdCopy() {
      // 复制数据
      let clipboard = new Clipboard('.configIdCopy');
      clipboard.on("success", () => {
        this.$message.success("复制成功");
      });
      clipboard.on("error", () => {
        this.$message.error("复制失败");
      });
      setTimeout(() => {
        // 销毁实例
        clipboard.destroy();
      }, 122);
    },
    /**
     * 取消
     */
    cancel() {
      this.$emit("close");
    },

    /**
     * 保存表单设计
     */
    editFormSave(values) {
      let that = this;
      this.controls = JSON.parse(values);
      let param = {
        configId: this.form.configId,
        saveJson: values,
      };
      that.$loading.show({ text: "保存中,请稍等..." });
      editSave(param).then(function (result) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 获取控件字段类型
     */
    editFormDataType(control) {
      let columns = this.publicForm.columns.filter(f => f.key == control.key);
      if (columns.length > 0) {
        return columns[0].dataType;
      }
      if (control.dataType) {
        return control.dataType;
      }
      switch (control.type) {
        case "input": //输入框
        case "autoComplete": //输入框
        case "map": //输入框
        case "serialNo": //编码
        case "textarea": //文本框
        case "user":
        case "organization":
        case "uploadFile":
        case "uploadImg":
        case "dataShow":
        case "treeSelect":
          if (control.options.maxLength) {
            return `nvarchar(${control.options.maxLength})`;
          } else {
            return `text`;
          }
        case "number": //数字输入框
          //判断是否具有小数点
          if (control.options.precision) {
            return `decimal(18, ${control.options.precision})`;
          } else {
            return `int`;
          }
        case "radio":
          return control.options.dynamic ? `nvarchar(36)` : `nvarchar(2)`;
        case "select":
        case "dictionary":
        case "correlationRecord":
          return control.options.multiple ? `nvarchar(${control.options.maxLength})` : `nvarchar(36)`;
        case "checkbox":
          return `nvarchar(256)`;
        case "dataList":
        case "editor":
        case "sign":
        case "selectInputList":
          return `text`;
        case "district":
        case "cascader":
          return `nvarchar(256)`;
        case "date":
          return `datetime`;
        case "time":
          return `nvarchar(8)`;
        case "icon":
          return `nvarchar(64)`;
        case "rate":
          return `numeric(18, 1)`;
        case "slider":
          return `numeric(18, 2)`;
        case "switch":
          return `bit`;
      }
    },

    /**
     * 发布
     */
    editFormPublic(edit) {
      var columns = this.getColumns();
      var publicFormColumns = [];
      var types = ['select', 'correlationRecord', 'treeSelect', 'cascader', 'checkbox', 'radio'];
      columns.forEach(f => {
        if (f.controlType == "batch") {
          var value = f.value;
          var valueJson = JSON.parse(value);
          publicFormColumns.push({
            key: f.key,
            name: f.name,
            pid: null,
            description: f.description,
            null: f.null,
            dataType: f.dataType,
            controlType: f.controlType,
            maxLength: f.maxLength,
            children: valueJson,
            copy: "{{" + f.key + (types.includes(f.controlType) ? "_Txt" : "") + "}}"
          })
          valueJson.forEach(v => {
            publicFormColumns.push({
              key: v.key,
              name: v.name,
              pid: f.key,
              description: v.description,
              null: v.null,
              dataType: v.dataType,
              controlType: v.controlType,
              maxLength: v.maxLength,
              copy: "{{" + f.key + "." + v.key + (types.includes(f.controlType) ? "_Txt" : "") + "}}"
            })
          })
        } else {
          publicFormColumns.push({
            key: f.key,
            name: f.name,
            pid: null,
            description: f.description,
            null: f.null,
            dataType: f.dataType,
            controlType: f.controlType,
            maxLength: f.maxLength,
            copy: "{{" + f.key + (types.includes(f.controlType) ? "_Txt" : "") + "}}"
          })
        }
      })
      this.publicForm.columns = publicFormColumns;
      this.publicForm.visible = edit;
      if (!edit) {
        this.editFormPublicSure();
      }
    },

    /**
     * 所有组件
     */
    getFieldSchema() {
      let that = this;
      var types = ['select', 'correlationRecord', 'treeSelect', 'cascader', 'checkbox', 'radio'];
      const traverse = (array) => {
        array.forEach(element => {
          if (["grid", "tabs", "selectInputList", "collapse"].includes(element.type)) {
            element.columns.forEach(item => {
              traverse(item.list);
            });
          } else if (element.type === "card") {
            // 卡片布局
            traverse(element.list);
          } else if (element.type === "batch") {
            //处理父级为当前元素信息
            var batchList = that.publicForm.columns.filter(f => f.pid == element.key);
            element.list.forEach(l => {
              var batchItem = batchList.filter(f => f.key == l.key);
              if (batchItem.length > 0) {
                l.model = batchItem[0].name;
                l.label = batchItem[0].description;
                l.null = batchItem[0].null;
                l.dataType = batchItem[0].dataType;
                if (types.includes(l.type)) {
                  l.options.dynamicKey = element.model + "." + l.model
                }
              }
            })

            var columnBatchFilter = that.publicForm.columns.filter(f => f.key == element.key);
            if (columnBatchFilter.length > 0) {
              element.model = columnBatchFilter[0].name;
              element.label = columnBatchFilter[0].description;
              element.null = columnBatchFilter[0].null;
              element.dataType = columnBatchFilter[0].dataType;

              if (types.includes(element.type)) {
                element.options.dynamicKey = element.model
              }
            }
          } else if (element.type === "table") {
            // 表格布局
            element.trs.forEach(item => {
              item.tds.forEach(val => {
                traverse(val.list);
              });
            });
          } else {
            var columnFilter = that.publicForm.columns.filter(f => f.key == element.key);
            if (columnFilter.length > 0) {
              element.model = columnFilter[0].name;
              element.label = columnFilter[0].description;
              element.null = columnFilter[0].null;
              element.dataType = columnFilter[0].dataType;
              if (types.includes(element.type)) {
                element.options.dynamicKey = element.model
              }
            }
          }
        });
      };
      var list = this.$refs.kfd.getValue().list;
      traverse(list);
      return list;
    },

    /**
     * 一级拼音生成
     */
    createModelName() {
      this.publicForm.columns.forEach(f => {
        if (f.key == f.name) {
          var namePinyin = pinyin.convert(f.description);
          f.name = namePinyin;
        }
      })
    },

    /**
     * 发布表单设计
     */
    async editFormPublicSure() {
      let that = this;
      var list = this.getFieldSchema();
      this.publicForm.visible = false;
      //重新赋值
      var json = that.$refs.kfd.getValue();
      json.list = list;
      that.$refs.kfd.handleSetData(json);
      let fieldSchema = that.$refs.kfd.getFieldSchema();
      fieldSchema.filter((f) => f.type == "batch").forEach((item) => {
        item.options.selectRows = [];
      })
      let param = {
        configId: that.form.configId,
        saveJson: JSON.stringify(json),
        publicJson: JSON.stringify(json),
        columnJson: JSON.stringify(fieldSchema),
      };
      let publicParam = {
        configId: that.form.configId,
        columns: JSON.stringify(that.publicForm.columns.filter(f => f.pid == null)),
      };
      that.$loading.show({ text: "发布中,请稍等..." });

      let result = {
        code: 200
      }
      if (this.publicField) {
        result = await tableField(publicParam);
      }

      if (result.code === that.eipResultCode.success) {
        editPublic(param).then(function (result) {
          that.$loading.hide();
          if (result.code === that.eipResultCode.success) {
            that.$message.success(result.msg);
            //发布表单成功后触发事件
            that.formPublicOk(param);
            that.$emit('editFormPublic', param)
          } else {
            that.$message.error(result.msg);
          }
        });
      } else {
        that.$loading.hide();
        that.$message.error(result.msg);
      }
    },

    /**
     * 获取所有列
     */
    getColumns() {
      let that = this;
      let fieldSchema = that.$refs.kfd.getFieldSchema();
      fieldSchema.filter((f) => f.type == "batch").forEach((item) => {
        item.options.selectRows = [];
      })
      //创建修改字段
      let columns = [];
      //解析字段
      fieldSchema
        .filter((f) => !["batch", "text", "divider"].includes(f.type))
        .forEach((control) => {
          let column = {
            key: control.key,
            isSingle: true,
            name: control.model, //字段名
            dataType: that.editFormDataType(control), //字段类型
            help: control.help ? control.help : '', //帮助信息
            description: control.label, //描述
            null: control.null, //是否可为空
            controlType: control.type, //组件类型
            maxLength: control.options.maxLength
          };
          switch (column.controlType) {
            case "district":
            case "checkbox": //复选框
              column.isSingle = false; //是否为单选,若为多选则新建关系表
              break;
            case "select": //选择框
            case "organization": //组织架构
            case "user": //用户
              column.isSingle = !control.options.multiple;
              break;
            default:
              break;
          }
          columns.push(column);
        });

      //得到子表控件
      fieldSchema.filter((f) => f.type == "batch").forEach((item) => {
        let layoutColumns = [];
        item.list.forEach((control) => {
          let column = {
            key: control.key,
            isSingle: true,
            name: control.model, //字段名
            dataType: that.editFormDataType(control), //字段类型
            description: control.label, //描述
            help: control.help ? control.help : '', //帮助信息
            null: control.null, //是否可为空
            controlType: control.type, //组件类型
            maxLength: control.options.maxLength
          };
          switch (column.controlType) {
            case "checkbox": //复选框
              column.isSingle = false; //是否为单选,若为多选则新建关系表
              break;
            case "select": //选择框
              column.isSingle = !control.options.multiple;
              break;
            case "organization": //组织架构
            case "dictionary": //字典
            case "user": //用户
              break;
            default:
              break;
          }
          layoutColumns.push(column);
        });

        //按钮
        let batchColumn = {
          name: item.model, //字段名
          key: item.key,
          dataType: "", //字段类型
          description: item.label, //描述
          controlType: item.type, //组件类型
          button: item.options.button,
          value: JSON.stringify(layoutColumns),
          null: item.null
        };

        columns.push(batchColumn);
      });

      if (columns.length == 0) {
        that.$message.error("请添加控件");
        return false;
      }

      return columns;
    },

    ...mapMutations("account", ["setSystemAgile"]),

    /**
     * 重置表单
     * @param {} form 
     */
    formPublicOk(form) {
      var systemAgileData = this.systemAgile.filter(f => f.configId == form.configId);
      if (systemAgileData && systemAgileData.length > 0) {
        systemAgileData[0].columnJson = form.columnJson;
        systemAgileData[0].publicJson = form.publicJson;
        this.setSystemAgile(this.systemAgile);
      } else {
        //this.$message.error('未找到配置信息')
      }
      // else {
      //   menuAgile().then(function (result) {
      //     if (result.code === that.eipResultCode.success) {
      //       that.setSystemAgile(result.data);
      //     }
      //   });
      // }
    },

    /**
     * 根据Id查找
     */
    find() {
      let that = this;
      that.spinning = true;
      findById(this.configId).then(function (result) {
        let form = result.data;
        that.form.name = form.name;
        that.form.configId = that.configId;
        if (form.saveJson) {
          that.$refs.kfd.handleSetData(JSON.parse(form.saveJson));
        } else {
          let def = JSON.parse(jsonFormat);
          that.$refs.kfd.handleSetData(def);
        }
        that.spinning = false;
        that.eipAgileDesigner.kfd = that.$refs.kfd;
      });
    },

    /**
     * 触发保存
     */
    save() {
      this.$refs.kfd.handleSave();
    },

    /**
     * 
     */
    translate(name) {
      window.location.href = 'https://fanyi.baidu.com/translate?query=' + name + '&lang=auto2zh#zh/en/' + name
    }
  },
};
</script>