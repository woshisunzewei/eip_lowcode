<template>
  <a-drawer width="100%" placement="right" :destroyOnClose="true" :visible="visible" :closable="false"
    :bodyStyle="{ padding: '0' }" @close="cancel">
    <div class="eip-form-designer-container">
      <div class="operating-area">
        <div class="left-btn-box">
          <a-space>
            <!-- 纸张设置 -->
            <a-select v-model="curPaperType" style="width:60px" @change="setPaper">
              <a-select-option :value="type" v-for="(value, type) in paperTypes" :key="type">
                {{ type }}
              </a-select-option>
            </a-select>

            <a-popover v-model="paperPopVisible" title="设置纸张宽高(mm)" trigger="click">
              <div slot="content">
                <a-input-group compact style="margin: 10px 10px">
                  <a-input type="number" v-model="paperWidth" style=" width: 100px; text-align: center"
                    placeholder="宽(mm)" />
                  <a-input style=" width: 30px; border-left: 0; pointer-events: none; backgroundColor: #fff"
                    placeholder="~" disabled />
                  <a-input type="number" v-model="paperHeight" style="width: 100px; text-align: center; border-left: 0"
                    placeholder="高(mm)" />
                </a-input-group>
                <a-button type="primary" style="width: 100%" @click="otherPaper">确定</a-button>
              </div>
              <a-button :type="'other' == curPaperType ? 'primary' : ''">自定义</a-button>


            </a-popover>

            <a-dropdown>
              <a-menu slot="overlay" @click="setElsAlign">
                <a-menu-item v-for="value in elsAlign" :key="value.type"> <a-icon :type="value.icon" />{{
                  value.title
                }}
                </a-menu-item>
              </a-menu>
              <a-button style="margin-left: 8px"> 快捷设置 <a-icon type="down" /> </a-button>
            </a-dropdown>
            <a-tooltip title="缩小">
              <a-button type="text" icon="zoom-out" @click="changeScale(false)"></a-button>
            </a-tooltip>
            <a-input-number :value="scaleValue" :min="scaleMin" :max="scaleMax" :step="0.1" disabled
              style="width: 70px;" :formatter="value => `${(value * 100).toFixed(0)}%`"
              :parser="value => value.replace('%', '')" />
            <a-tooltip title="放大">
              <a-button type="text" icon="zoom-in" @click="changeScale(true)"></a-button>
            </a-tooltip>
            <a-button-group>
              <a-tooltip title="预览">
                <a-button type="primary" @click="preView">
                  <a-icon type="eye" />预览
                </a-button>
              </a-tooltip>
              <!-- <a-tooltip title="直接打印">
                <a-button type="primary" @click="print">
                  <a-icon type="printer" />打印
                </a-button>
              </a-tooltip> -->
              <a-tooltip title="全选元素">
                <a-button type="primary" @click="selectAll">
                  <a-icon type="block" />全选</a-button>
              </a-tooltip>
            </a-button-group>
            <exportJson :template="template" />
            <importJson :template="template" />

            <a-button-group>
              <a-tooltip title="保存">
                <a-button type="primary" @click="printSaveClick">
                  <a-icon type="save" />
                  保存</a-button>
              </a-tooltip>
              <a-tooltip title="发布">
                <a-button type="primary" @click="printPublicClick">
                  <a-icon type="build" />发布</a-button>
              </a-tooltip>

              <a-popconfirm title="是否确认清空?" okType="danger" okText="确定清空" @confirm="clearPaper">
                <a-button type="danger">
                  <a-icon type="close" />
                  清空
                </a-button>
              </a-popconfirm>
            </a-button-group>

          </a-space>
        </div>
        <div class="right-btn-box">
          <a-tooltip :title="title"><span class="text-bold">{{ title }}</span></a-tooltip>
          <a-divider type="vertical" />
          <a-tooltip title="关闭">
            <a @click="cancel" style="color: #ff4d4f">
              <a-icon type="close" style="font-size: 14px;" />
              <span>关闭</span>
            </a>
          </a-tooltip>
        </div>
      </div>
      <a-spin :spinning="loading">
        <!-- 操作区域 end -->
        <div class="content toolbars-top">
          <aside class="left">
            <a-card :bordered="false" :bodyStyle="{ padding: '6px' }"
              style="height:calc(100vh - 50px);overflow-y: auto">
              <div class="rect-printElement-types hiprintEpContainer" />
            </a-card>
          </aside>
          <section>
            <!-- 多面板的容器 -->

            <div class="hiprint-printPagination padding-lr-sm padding-top-sm"></div>
            <a-card :bordered="false" class="card-design" style="height:calc(100vh - 100px);overflow: auto">
              <div id="hiprint-printTemplate" class="hiprint-printTemplate"></div>
            </a-card>
          </section>
          <aside class="right">
            <a-card :bordered="false" :bodyStyle="{ padding: '4px' }"
              style="height:calc(100vh - 50px);overflow-y: auto">
              <a-row class="hinnn-layout-sider">
                <div id="PrintElementOptionSetting"></div>
              </a-row>
            </a-card>
          </aside>
          <!-- <a-row :gutter="[8, 0]" type="flex">
            <a-col flex="270px">
             
            </a-col>
            <a-col flex="auto">
              
            </a-col>
            <a-col flex="270px" class="params_setting_container">
              
            </a-col>
          </a-row> -->
          <!-- 预览 -->
          <print-preview ref="preView" />
        </div>
      </a-spin>
    </div>
  </a-drawer>
</template>

<script>
import {
  findById,
  column,
  printSave,
  printPublic,
  findWorkflowById,
  findDataFromName
} from "@/services/system/agile/print/designer";
import scale from './components/scale.js'
import config from './components/config.js'
import printPreview from './components/preview'
import exportJson from './components/exportJsonModal.vue'
import importJson from './components/importJsonModal.vue'
import { hiprint } from './components/print'
import { mapMutations, mapGetters } from "vuex";
let hiprintTemplate;
export default {
  name: "printCustom",
  components: { printPreview, exportJson, importJson },
  data() {
    return {
      template: null,
      // 模板选择
      mode: 0,
      // 当前纸张
      curPaper: {
        type: 'other',
        width: 220,
        height: 80
      },
      // 纸张类型
      paperTypes: {
        'A3': {
          width: 420,
          height: 296.6
        },
        'A4': {
          width: 210,
          height: 296.6
        },
        'A5': {
          width: 210,
          height: 147.6
        },
        'B3': {
          width: 500,
          height: 352.6
        },
        'B4': {
          width: 250,
          height: 352.6
        },
        'B5': {
          width: 250,
          height: 175.6
        }
      },
      elsAlign: [{
        title: '左对齐',
        type: 'left',
        icon: 'align-left'
      }, {
        title: '居中',
        type: 'vertical',
        icon: 'align-center'
      }, {
        title: '右对齐',
        type: 'right',
        icon: 'align-right'
      }, {
        title: '顶部对齐',
        type: 'top',
        icon: 'border-top'
      }, {
        title: '垂直居中',
        type: 'horizontal',
        icon: 'pic-center'
      }, {
        title: '底部对齐',
        type: 'bottom',
        icon: 'border-bottom'
      }, {
        title: '横向分散',
        type: 'distributeHor',
        icon: 'column-width'
      }, {
        title: '纵向分散',
        type: 'distributeVer',
        icon: 'column-height'
      }],
      scaleValue: 1,
      scaleMax: 5,
      scaleMin: 0.5,
      // 自定义纸张
      paperPopVisible: false,
      paperWidth: '220',
      paperHeight: '80',
      lastjson: '',

      printData: {},//打印数据
      loading: true//加载状态
    }
  },
  computed: {
    ...mapGetters("account", ["user"]),
    curPaperType() {
      let type = 'A4'
      let types = this.paperTypes
      for (const key in types) {
        let item = types[key]
        let { width, height } = this.curPaper
        if (item.width === width && item.height === height) {
          type = key
        }
      }
      return type
    }
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
  mounted() {
    let that = this;
    setTimeout(function () {
      that.init()
    }, 500)
  },
  methods: {
    /**
  * 取消
  */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 获取配置字段
     * @param {*} result 
     */
    async initPrintElementType(result) {
      let that = this;
      //获取配置字段
      let printElementType = [];
      let form = result.data;
      var findResult = await findDataFromName({ dataFromName: form.dataFromName })
      if (findResult.code === that.eipResultCode.success) {
        let columnJson = JSON.parse(findResult.data.columnJson).filter(f => !["text", "divider"].includes(f.type));
        //所有字段
        columnJson.forEach((item) => {
          var testData = "???";
          //判断是否需要添加后缀
          switch (item.type) {
            case "checkbox":
            case "radio":
            case "select":
            case "post":
            case "dictionary":
            case "cascader":
            case "correlationRecord":
              item.model += "_Txt";
              break;
            case "organization":
              item.model += "_Txt";
              testData = that.user.organization;
              break;
            case "user":
              testData = that.user.name;
              item.model += "_Txt";
              break;
            case "number":
              testData = 999;
              break;
            case "date":
              testData = that.$utils.toDateString(new Date())
              break;
          }
          if (item.type == "batch") {
            var columns = [];
            var testDataRow = {};
            item.list.forEach(list => {
              var field = ["user", "organization"].includes(list.type) ? list.model + "_Txt" : list.model
              columns.push({
                title: list.label, align: 'center', field: field, width: list.options.width
              })
              testDataRow[field] = "???";
            })
            printElementType.push({
              tid: 'aProviderModule.' + item.model,
              title: item.label,
              data: item.label,
              type: 'table',
              options: {
                field: item.model,
                tableHeaderRepeat: 'first',
                tableFooterRepeat: 'last',
                scale: 1,
                config: JSON.stringify(item)
              },
              editable: true,
              columnDisplayEditable: true,//列显示是否能编辑
              columnDisplayIndexEditable: true,//列顺序显示是否能编辑
              columnTitleEditable: true,//列标题是否能编辑
              columnResizable: true, //列宽是否能调整
              columnAlignEditable: true,//列对齐是否调整
              isEnableEditField: true, //编辑字段
              isEnableContextMenu: true, //开启右键菜单 默认true
              isEnableInsertRow: true, //插入行
              isEnableDeleteRow: true, //删除行
              isEnableInsertColumn: true, //插入列
              isEnableDeleteColumn: true, //删除列
              isEnableMergeCell: true, //合并单元格
              columns: [
                columns
              ],
              rowsColumnsMerge: function (data, col, index) { },
              footerFormatter: function (options, rows, data, currentPageGridRowsData) { },

            });
            that.printData[item.model] = [testDataRow, testDataRow, testDataRow];
          }
          else if (item.type == "dataShow" && item.options.type == "qrCode") {
            printElementType.push({
              tid: 'aProviderModule.qrcode',
              title: item.label,
              data: item.label,
              type: 'qrcode',
              options: {
                field: item.model,
              }
            });

            that.printData[item.model] = testData;
          }
          else if (item.type == "dataShow" && item.options.type == "barCode") {
            printElementType.push({
              tid: 'aProviderModule.barcode',
              title: item.label,
              data: item.label,
              type: 'barcode',
              options: {
                field: item.model,
              }
            });

            that.printData[item.model] = testData;
          }
          else {
            printElementType.push({
              tid: 'aProviderModule.' + item.model,
              title: item.label,
              data: item.label,
              type: 'text',
              options: {
                testData: testData,
                height: 17,
                fontSize: 12,
                field: item.model,
                fontWeight: "700",
                textAlign: "left",
                textContentVerticalAlign: "middle",
                hideTitle: false,
                config: JSON.stringify(item),
                scale: 1
              }
            });

            that.printData[item.model] = testData;
          }

        });
        that.eipSystemColumns.forEach(systemColumn => {
          let testData = "";
          switch (systemColumn.name) {
            case "RelationId":
            case "CreateUserId":
            case "UpdateUserId":
              testData = that.user.userId;
              break;
            case "CreateOrganizationId":
            case "UpdateOrganizationId":
              testData = that.user.organizationId;
              break;
            case "CreateTime":
            case "UpdateTime":
              testData = that.$utils.toDateString(new Date())
              break;
            case "CreateUserName":
            case "UpdateUserName":
              testData = that.user.name;
              break;
            case "CreateOrganizationName":
            case "UpdateOrganizationName":
              testData = that.user.organizationName;
              break;

          }
          printElementType.push({
            tid: 'aProviderModule.' + systemColumn.name,
            title: systemColumn.description,
            data: systemColumn.description,
            type: 'text',
            options: {
              testData: testData,
              height: 17,
              fontSize: 12,
              field: systemColumn.name,
              fontWeight: "700",
              textAlign: "left",
              textContentVerticalAlign: "middle",
              hideTitle: false,
              scale: 1
            }
          });

          that.printData[systemColumn.name] = testData;
        })

        var workflow = await findWorkflowById(findResult.data.menuId)
        if (workflow.data != null && workflow.data.publicJson) {
          printElementType.push({
            tid: 'aProviderModule.WorkflowStatus',
            title: "流程状态",
            data: "流程状态",
            type: 'text',
            options: {
              testData: '???',
              height: 17,
              fontSize: 12,
              field: "WorkflowStatus",
              fontWeight: "700",
              textAlign: "left",
              textContentVerticalAlign: "middle",
              hideTitle: false,
              scale: 1
            }
          })
          printElementType.push({
            tid: 'aProviderModule.WorkflowSn',
            title: "流水号",
            data: "流水号",
            type: 'text',
            options: {
              testData: '???',
              height: 17,
              fontSize: 12,
              field: "WorkflowSn",
              fontWeight: "700",
              textAlign: "left",
              textContentVerticalAlign: "middle",
              hideTitle: false,
              scale: 1
            }
          })
          printElementType.push({
            tid: 'aProviderModule.WorkflowTitle',
            title: "标题",
            data: "标题",
            type: 'text',
            options: {
              testData: '???',
              height: 17,
              fontSize: 12,
              field: "WorkflowTitle",
              fontWeight: "700",
              textAlign: "left",
              textContentVerticalAlign: "middle",
              hideTitle: false,
              scale: 1
            }
          })
        }

      } else {
        result = await column({
          dataBaseId: form.dataBaseId,
          name: form.dataFromName,
          dataType: this.getDataType(form.dataFrom)
        })
        result.data.forEach((item) => {
          printElementType.push({
            tid: 'aProviderModule.' + item.name,
            title: item.description,
            data: item.description,
            type: 'text',
            options: {
              testData: '???',
              height: 17,
              fontSize: 12,
              field: item.name,
              fontWeight: "700",
              textAlign: "left",
              textContentVerticalAlign: "middle",
              hideTitle: false
            },
            config: null
          });
          that.printData[item.name] = "???";
        });
      }
      return printElementType;
    },

    /**
     * 重新加载配置
     */
    initConfig() {
      // 替换配置
      hiprint.setConfig({
        optionItems: [
          scale,
          config
        ],
        movingDistance: 2.5,
        text: {
          tabs: [
            {
              options: []
            },
            {
              name: '样式', options: [
                {
                  name: 'scale',
                  after: 'transform', // 自定义参数，插入在 transform 之后
                  hidden: false
                },
                {
                  name: 'config',
                  after: 'zIndex',
                  hidden: true
                },
              ]
            }
          ],
          supportOptions: [
            {
              name: 'scale', // 自定义参数，supportOptions 必须得添加
              after: 'transform', // 自定义参数，插入在 transform 之后
              hidden: false
            },
            {
              name: 'config',
              after: 'zIndex',
              hidden: true
            }
          ]
        },
      })
    },

    /**
     * 初始化
     */
    async init() {
      let that = this;
      that.loading = true;
      var result = await findById(that.configId);
      //获取配置字段
      let printElementType = await this.initPrintElementType(result);

      var aProviderModules = [];
      aProviderModules.push(new hiprint.PrintElementTypeGroup("字段", printElementType));

      aProviderModules.push(new hiprint.PrintElementTypeGroup("辅助", [{
        tid: 'aProviderModule.text',
        title: '文本',
        type: 'text'
      }, {
        tid: 'aProviderModule.longText',
        title: '长文本',
        type: 'longText'
      }, {
        tid: 'aProviderModule.hline',
        title: '横线',
        type: 'hline'
      }, {
        tid: 'aProviderModule.vline',
        title: '竖线',
        type: 'vline'
      },
      {
        tid: 'aProviderModule.rect',
        title: '矩形',
        type: 'rect'
      },
      {
        tid: 'aProviderModule.oval',
        title: '椭圆',
        type: 'oval'
      },
      {
        tid: 'aProviderModule.barcode',
        title: '条形码',
        type: 'barcode',
      },
      {
        tid: 'aProviderModule.qrcode',
        title: '二维码',
        type: 'qrcode',
      },
      { tid: 'aProviderModule.logo', title: '图片', type: 'image' },
      { tid: 'aProviderModule.html', title: 'Html', type: 'html' },
      ]))
      // 自定义设计元素
      const aProvider = function (ops) {
        var addElementTypes = function (context) {
          context.removePrintElementTypes("aProviderModule");
          context.addPrintElementTypes(
            "aProviderModule", aProviderModules
          );
        };
        return {
          addElementTypes: addElementTypes
        };
      };
      let provider = {
        name: 'EIP',
        value: 'aProviderModule',
        type: 1,
        f: aProvider()
      }
      hiprint.init({
        providers: [provider.f]
      });

      this.initConfig();

      // 先清空, 避免重复构建
      $('.hiprintEpContainer').empty()
      hiprint.PrintElementTypeManager.build('.hiprintEpContainer', provider.value);
      $('#hiprint-printTemplate').empty()
      let templates = this.$ls.get('KEY_TEMPLATES', {})
      let template = templates[provider.value] ? templates[provider.value] : {}
      this.template = hiprintTemplate = new hiprint.PrintTemplate({
        template: template,
        dataMode: 1, // 1:getJson 其他：getJsonTid 默认1
        history: true, // 是否需要 撤销重做功能
        onDataChanged: (type, json) => {
          console.log(type); // 新增、移动、删除、修改(参数调整)、大小、旋转
          console.log(json); // 返回 template
        },
        settingContainer: '#PrintElementOptionSetting',
        paginationContainer: '.hiprint-printPagination',
        defaultPanelName: '默认面板名称',
        onPanelAddClick: (panel, createPanel) => {
          panel.name = '新面板' + (panel.index + 1);
          createPanel(panel);
        },
        // 图片选择功能
        onImageChooseClick: (target) => {
          // 创建input，模拟点击，然后 target.refresh 更新
          let input = document.createElement("input");
          input.setAttribute("type", "file");
          input.click();
          input.onchange = function () {
            var file = this.files[0];
            if (file) {
              var reader = new FileReader();
              //通过文件流行文件转换成Base64字行串 
              reader.readAsDataURL(file);
              //转换成功后
              reader.onloadend = function () {
                // 通过 target.refresh 更新图片元素
                target.refresh(reader.result);
              }
            }
          }
          input.remove();
        },
      });
      hiprintTemplate.design('#hiprint-printTemplate');
      // 获取当前放大比例, 当zoom时传true 才会有
      this.scaleValue = hiprintTemplate.editingPanel.scale || 1;
      if (result.data.saveJson) {
        hiprintTemplate.update(JSON.parse(result.data.saveJson))
      }
      this.loading = false;

      $(".hiprint-printPanel").trigger("click")
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
      return dataType
    },
    selectAll() {
      this.template.selectAllElements()
    },

    /**
     * 设置纸张大小
     * @param type [A3, A4, A5, B3, B4, B5, other]
     * @param value {width,height} mm
     */
    setPaper(type) {
      let value = this.paperTypes[type]
      try {
        if (Object.keys(this.paperTypes).includes(type)) {
          this.curPaper = { type: type, width: value.width, height: value.height }
          hiprintTemplate.setPaper(value.width, value.height)
        } else {
          this.curPaper = { type: 'other', width: value.width, height: value.height }
          hiprintTemplate.setPaper(value.width, value.height)
        }
      } catch (error) {
        this.$message.error(`操作失败: ${error}`)
      }
    },
    changeScale(big) {
      let scaleValue = this.scaleValue;
      if (big) {
        scaleValue += 0.1;
        if (scaleValue > this.scaleMax) scaleValue = 5;
      } else {
        scaleValue -= 0.1;
        if (scaleValue < this.scaleMin) scaleValue = 0.5;
      }
      if (hiprintTemplate) {
        // scaleValue: 放大缩小值, false: 不保存(不传也一样), 如果传 true, 打印时也会放大
        hiprintTemplate.zoom(scaleValue, true);
        this.scaleValue = scaleValue;
      }
    },

    otherPaper() {
      let value = {}
      value.width = this.paperWidth
      value.height = this.paperHeight
      this.curPaper = { type: 'other', width: value.width, height: value.height }
      hiprintTemplate.setPaper(value.width, value.height)
      this.paperPopVisible = false
    },
    preView() {
      let { width } = this.curPaper
      this.$refs.preView.show(hiprintTemplate, this.printData, width)
    },
    print() {
      if (window.hiwebSocket.opened) {
        const printerList = hiprintTemplate.getPrinterList();
        console.log(printerList)
        hiprintTemplate.print2(this.printData, { printer: '', title: 'hiprint测试打印' });
        return
      }
      this.$error({
        title: "客户端未连接",
        content: (h) => (
          <div>
            连接【{hiwebSocket.host}】失败！
            <br />
            请确保目标服务器已
            <a
              href="https://gitee.com/CcSimple/electron-hiprint/releases"
              target="_blank"
            >
              下载
            </a>
            并
            <a href="hiprint://" target="_blank">
              运行
            </a>
            打印服务！
          </div>
        ),
      });
    },

    /**
     * 打印保存
     */
    printSaveClick() {
      let that = this;
      let param = {
        configId: this.configId,
        saveJson: JSON.stringify(hiprintTemplate.getJson()),
      };
      that.$loading.show({ text: "保存中,请稍等..." });
      printSave(param).then(function (result) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 打印发布
     */
    async printPublicClick() {
      let that = this;

      let param = {
        configId: that.configId,
        saveJson: JSON.stringify(hiprintTemplate.getJson()),
        publicJson: JSON.stringify(hiprintTemplate.getJson()),
      };
      that.$loading.show({ text: "发布中,请稍等..." });

      printPublic(param).then(function (result) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    setElsAlign(e) {
      var value = this.$utils.find(this.elsAlign, f => f.type == e.key);
      hiprintTemplate.setElsAlign(value.type)
    },

    clearPaper() {
      try {
        hiprintTemplate.clear();
      } catch (error) {
        this.$message.error(`操作失败: ${error}`);
      }
    }
  }
}
</script>

<style lang="less" scoped>
// build 拖拽
/deep/ .eip-form-designer-container .content aside.left ul li {
  text-overflow: ellipsis;
  text-align: left;
  overflow: hidden;
  white-space: nowrap;
}

// 默认图片
/deep/ .hiprint-printElement-image-content {
  img {
    content: url("~@/assets/logo.png");
  }
}

/deep/.eip-form-designer-container .title {
  margin-left: 10px;
  font-size: 16px;
  font-weight: bold;
}

// 设计容器
.card-design {
  overflow: hidden;
  overflow-x: auto;
  overflow-y: auto;
}


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

/deep/ .auto-submit {
  box-sizing: border-box !important;
  margin: 0 !important;
  padding: 0 !important;
  font-variant: tabular-nums !important;
  list-style: none !important;
  -webkit-font-feature-settings: 'tnum' !important;
  font-feature-settings: 'tnum' !important;
  position: relative !important;
  display: inline-block !important;
  width: 100%;
  padding: 4px 11px !important;
  color: rgba(0, 0, 0, 0.65) !important;
  font-size: 14px !important;
  line-height: 1.5 !important;
  background-color: #fff !important;
  background-image: none !important;
  border: 1px solid #d9d9d9 !important;
  border-radius: 2px !important;
  -webkit-transition: all 0.3s !important;
  transition: all 0.3s !important;
}

/deep/ .hiprint-option-items .hiprint-option-item-field input,
/deep/ .hiprint-option-items .hiprint-option-item-field select {
  height: 32px !important;
}

/deep/ .ep-draggable-item {
  display: block;
  text-overflow: ellipsis;
  text-align: left;
  overflow: hidden;
  white-space: nowrap;
}
</style>
