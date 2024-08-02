<template>
  <a-drawer width="100%" :zIndex="300" :destroyOnClose="true" placement="right" :visible="visible" :closable="false"
    :bodyStyle="{ padding: '0px' }" @close="cancel">
    <div style="height: 100%;">
      <a-layout class="container">
        <a-layout-sider width="64" theme="light" class="select-area" v-if="!processInstanceId">
          <a-row>
            <div class="tab">控件</div>
            <node-list :nodeList="field.commonNodes" type="commonNodes" @setDragInfo="setDragInfo" />
          </a-row>
        </a-layout-sider>
        <a-layout>
          <a-layout-header class="header-option">
            <div class="header-option__tools">
              <a-tooltip title="生成图片" placement="bottom">
                <a @click="exportFlowPicture(false)">
                  <a-icon type="picture" />
                  <span>生成图片</span>
                </a>
              </a-tooltip>
              <a-divider type="vertical" />
              <a-popconfirm v-if="!processInstanceId" title="确认要重新绘制吗？" placement="bottom" okText="确认" cancelText="取消"
                @confirm="clear">
                <a>
                  <a-icon type="delete" />
                  <span>重绘</span>
                </a>
              </a-popconfirm>
              <a-divider type="vertical" v-if="!processInstanceId" />
              <a-tooltip :title="flowData.config.showGridText" placement="bottom">
                <a @click="toggleShowGrid">
                  <a-icon :type="flowData.config.showGridIcon" />
                  <span>{{ flowData.config.showGridText }}</span>
                </a>
              </a-tooltip>
              <a-divider type="vertical" v-if="!processInstanceId" />
              <a-tooltip v-if="!processInstanceId" title="设置" placement="bottom">
                <a @click="setting">
                  <a-icon type="setting" />
                  <span>设置</span>
                </a>
              </a-tooltip>
              <a-divider type="vertical" v-if="!processInstanceId" />
              <a-tooltip v-if="!processInstanceId" title="测试" placement="bottom">
                <a @click="openTest">
                  <a-icon type="tool" />
                  <span>测试</span>
                </a>
              </a-tooltip>
              <a-divider v-if="!processInstanceId" type="vertical" />
              <a-tooltip v-if="!processInstanceId" title="快捷键" placement="bottom">
                <a @click="shortcutHelper">
                  <a-icon type="book" />
                  <span>快捷键</span>
                </a>
              </a-tooltip>
              <a-divider v-if="!processInstanceId" type="vertical" />
              <a-tooltip v-if="!processInstanceId" title="保存" placement="bottom">
                <a @click="saveFlow">
                  <a-icon type="save" />
                  <span>保存</span>
                </a>
              </a-tooltip>
              <a-divider v-if="!processInstanceId" type="vertical" />
              <a-tooltip v-if="!processInstanceId" title="发布" placement="bottom">
                <a @click="publicFlow">
                  <a-icon type="build" />
                  <span>发布</span>
                </a>
              </a-tooltip>
            </div>
            <div class="header-option__buttons">
              <a-tooltip :title="title"><span class="text-bold">{{ title }}</span></a-tooltip>
              <a-divider type="vertical" />
              <a-tooltip title="关闭">
                <a @click="cancel" style="color: #ff4d4f">
                  <a-icon type="close" style="font-size: 14px;" />
                  <span>关闭</span>
                </a>
              </a-tooltip>
            </div>
          </a-layout-header>

          <a-layout-content class="content">
            <div style="position: absolute;left: 170px;z-index: 9;top: 44px;">
              <a-space>
                <span v-for="tool in field.tools" :key="tool.type">
                  <a-tooltip :title="tool.name" placement="bottom" v-if="!processInstanceId">
                    <a-button :icon="tool.icon" size="small"
                      :type="currentTool.type === tool.type ? 'primary' : 'default'" @click="selectTool(tool.type)">
                    </a-button>
                  </a-tooltip>
                </span>
                <a-switch v-if="!processInstanceId" checked-children="开启" un-checked-children="关闭" default-checked />
              </a-space>
            </div>
            <flow-area ref="flowArea" :dragInfo="dragInfo" :browserType="browserType" :flowData="flowData"
              :flowConfig="flowConfig" :select.sync="currentSelect" :selectGroup.sync="currentSelectGroup"
              :plumb="plumb" :processInstanceId="processInstanceId" :currentTool="currentTool"
              @findNodeConfig="findNodeConfig" @selectTool="selectTool" @getShortcut="getShortcut" @saveFlow="saveFlow"
              @settingNode="settingNode">
            </flow-area>
            <vue-context-menu :contextMenuData="linkContextMenuData" @deleteLink="deleteLink"
              @settingLink="settingLink">
            </vue-context-menu>
          </a-layout-content>
        </a-layout>
      </a-layout>
      <!-- 生成流程图片 -->
      <a-modal :title="'流程设计图_' + flowData.attr.id + '.png'" centered width="90%" :closable="flowPicture.closable"
        :maskClosable="flowPicture.maskClosable" :visible="flowPicture.modalVisible" okText="下载到本地" cancelText="取消"
        @ok="downLoadFlowPicture" @cancel="cancelDownLoadFlowPicture">
        <img :src="flowPicture.url" style="width: 100%" />
      </a-modal>
      <!-- 设置 -->
      <setting-modal :flowConfig="flowConfig" ref="settingModal"></setting-modal>

      <!-- 快捷键大全 -->
      <shortcut-modal ref="shortcutModal"></shortcut-modal>

      <!-- 测试 -->
      <test-modal :flowConfig="flowConfig" ref="testModal" @loadFlow="loadFlow"></test-modal>

      <!-- 流程属性 -->
      <flow-attr ref="flowAttr" :plumb="plumb" :flowData="flowData" :select.sync="currentSelect"
        :formId="activity.formId"></flow-attr>

      <a-modal :zIndex="1001" :title="public.title" :visible="public.visible" @cancel="public.visible = false"
        :destroyOnClose="true" v-drag>
        <template slot="footer">
          <a-button @click="saveDataOk(false)" type="primary">
            替换当前版本
          </a-button>
          <a-button @click="saveDataOk(true)" type="danger">
            发布新版本
          </a-button>
        </template>
        <div style="font-size: 20px;">{{ public.content }}</div>
      </a-modal>
    </div>
  </a-drawer>
</template>

<script>
import './components/assets/style/index.less'
import {
  save,
  findById,
  flowchart,
  thumbnail
} from "@/services/workflow/process/designer";
import { deleteConfirm } from "@/utils/util";
import { jsPlumb } from 'jsplumb'
import cloneDeep from 'lodash/cloneDeep'
import { tools } from './config/tools.js'
import { commonNodes, highNodes, laneNodes } from './config/nodes.js'
import {
  flowConfig as defaultFlowConfig,
  settingConfig,
  flowStatus
} from './config/flow.js'
import { shortcutKeys } from './config/shortcutKeys.js'
import { linkMenu } from './config/contextMenu.js'
import html2canvas from 'html2canvas'
import canvg from 'canvg'
import { utils, setFlowConfig, getBrowserType } from './utils/common.js'
import FlowArea from './modules/FlowArea'
import FlowAttr from './modules/FlowAttr'
import SettingModal from './modules/SettingModal'
import ShortcutModal from './modules/ShortcutModal'
import TestModal from './modules/TestModal'
import NodeList from './modules/NodeList'

export default {
  name: 'vfdp',
  components: {
    html2canvas,
    canvg,
    FlowArea,
    FlowAttr,
    SettingModal,
    ShortcutModal,
    TestModal,
    NodeList
  },
  data() {
    return {
      flowConfig: cloneDeep(defaultFlowConfig),
      browserType: 3,
      plumb: {},
      field: {
        tools: tools,
        commonNodes: commonNodes,
        highNodes: highNodes,
        laneNodes: laneNodes
      },
      flowData: {
        nodeList: [],
        linkList: [],
        attr: {
          id: ''
        },
        config: {
          showGrid: true,
          showGridText: '隐藏网格',
          showGridIcon: 'eye'
        },
        status: flowStatus.CREATE,
        remarks: []
      },
      currentTool: {
        type: 'drag',
        icon: 'drag',
        name: '拖拽'
      },
      currentSelect: {},
      currentSelectGroup: [],
      activeShortcut: true, // 画布聚焦开启快捷键
      linkContextMenuData: linkMenu,
      flowPicture: {
        url: '',
        modalVisible: false,
        closable: false,
        maskClosable: false
      },
      dragInfo: {
        type: '',
        belongTo: ''
      },

      saveData: {
        visible: false,
        dialog: true,
        isPublic: false,
        value: {
          activities: [],
          links: [],
          designJson: "",
          processId: this.processId,
          isNewVersion: null,
        },
      },

      activity: {
        formId: null, //默认表单Id
      },
      public: {
        title: '',
        content: '',
        visible: false,
      },

      process: {
        version: null,
        name: null,
      },
    }
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
    },

    processId: {
      type: String,
    },

    agile: {
      type: Object,
    }, //敏捷开发实例

    //流程实例Id
    processInstanceId: {
      type: String,
    },
  },
  mounted() {
    // 浏览器兼容性
    this.dealCompatibility()
    // 实例化JsPlumb
    this.initJsPlumb()
    // 初始化快捷键
    this.listenShortcut()
    // 初始画布设置
    this.initSettingConfig()
    // 初始化流程图
    this.initFlow()
    this.initData();
  },
  methods: {
    /**
     * 初始化数据
     */
    initData() {
      var that = this;
      //流程配置
      if (this.processId) {
        that.$message.loading("加载中...", 0);
        findById(this.processId).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            var designJson = JSON.stringify({
              "nodeList"
                : [
                  {
                    "type": "begin", "text": { "text": "开始" }, "icon": "play-circle", "props": { "base": { "title": "开始", "formName": "", "isOpinion": false, "isArchive": false, "titleRule": "您有一条待审批流程" }, "timeout": [], "button": [], "notice": [], "event": [], "column": [] }, "id": "begin-" +
                      utils.getId() +
                      "", "height": 50, "x": 235, "width": 120, "y": 145
                  },
                  {
                    "type": "end", "text": { "text": "结束" }, "icon": "stop", "props": { "base": { "title": "结束", "formName": "" }, "notice": [], "event": [] }, "id": "end-" +
                      utils.getId() +
                      "", "height": 50, "x": 880, "width": 50, "y": 145
                  }],
              "linkList": [],
              "attr": { "id": "flow-17d3ce4e-2073-420b-82cb-6ecd91e7e819" },
              "config": { "showGrid": true, "showGridText": "隐藏网格", "showGridIcon": "eye" },
              "status": "1", "remarks": []
            })
            if (result.data) {
              that.process.version = result.data.version;
              that.process.name = result.data.name;
              designJson = result.data.saveJson;
            } else {
              that.process.version = "1.0";
              that.process.name = that.agile.name;

              that.saveData.value.name = that.agile.name;
              that.saveData.value.orderNo = 1;
              that.saveData.value.version = "1.0";

              that.saveData.value.sn = '<p><span id="%E5%B9%B4" class="non-editable" contenteditable="false">年</span><span id="%E6%9C%88" class="non-editable" contenteditable="false">月</span><span id="%E6%97%A5" class="non-editable" contenteditable="false">日</span><span id="0000" class="non-editable" contenteditable="false">0000</span></p>';
              that.saveData.value.snTxt = '年月日0000';
              that.saveData.value.showLibrary = true;
              that.saveData.value.formId = that.agile.configId;
              that.saveData.value.typeId = that.agile.typeId;
              that.saveData.value.isFreeze = false;
              that.saveData.value.icon = that.agile.icon;
              that.saveData.value.theme = that.agile.theme;
              that.saveData.value.iconColor = "#000";
            }
            if (that.agile) {
              that.activity.formId = that.agile.configId;
            } else {
              that.activity.formId = result.data.formId;
            }

            that.$message.destroy();
          } else {
            that.$message.error(result.msg);
          }

          that.loadFlow(designJson)
          that.spinning = false;
        });
      }

      //实例查看
      if (this.processInstanceId) {
        that.$message.loading("加载中...", 0);
        flowchart({ id: this.processInstanceId }).then(function (result) {
          that.flowChartData = result.data.instanceProcess;
          that.loadFlow(result.data.designJson)
          that.$message.destroy();
        });
      }
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
      this.saveData.isPublic = false;
    },

    /**
     * 初始画布设置
     */
    initSettingConfig() {
      if (!this.$ls.get('settingConfig')) {
        this.$ls.set('settingConfig', settingConfig)
      } else {
        this.flowConfig = setFlowConfig(
          this.flowConfig,
          this.$ls.get('settingConfig')
        )
      }
    },

    /**
     * 设置dragInfo
     * @param {*} info 
     */
    setDragInfo(info) {
      this.dragInfo = info
    },


    /**
     * 浏览器兼容性
     */
    dealCompatibility() {
      this.browserType = getBrowserType()
      if (this.browserType === 2) {
        shortcutKeys.scaleContainer = {
          code: 16,
          codeName: 'SHIFT(chrome下为ALT)',
          shortcutName: '画布缩放'
        }
      }
    },

    /**
     * 实例化JsPlumb
     */
    initJsPlumb() {
      this.plumb = jsPlumb.getInstance(this.flowConfig.jsPlumbInsConfig)

      this.plumb.bind('beforeDrop', info => {
        let sourceId = info.sourceId
        let targetId = info.targetId

        if (sourceId === targetId) return false
        // let hasMultipleLine = this.flowData.linkList.find(
        //   link => link.sourceId === sourceId && link.targetId === targetId
        // )
        // if (hasMultipleLine) {
        //   this.$message.error('同方向的两节点连线只能有一条！')
        //   return false
        // }
        return true
      })

      this.plumb.bind('connection', conn => {
        let connObj = conn.connection.canvas
        let o = {}
        let id
        let label
        let sourceFork = false;
        let sourceNode = this.flowData.nodeList.filter(f => f.id == conn.sourceId);
        let targetNode = this.flowData.nodeList.filter(f => f.id == conn.targetId);
        if (sourceNode[0].type == 'fork') {
          sourceFork = true;
        }
        if (
          this.flowData.status === flowStatus.CREATE ||
          this.flowData.status === flowStatus.MODIFY
        ) {
          id = 'link-' + utils.getId()
          label = 'TO ' + targetNode[0].text.text;
        } else if (this.flowData.status === flowStatus.LOADING) {
          let l = this.flowData.linkList[this.flowData.linkList.length - 1]
          id = l.id
          label = l.label
        }
        connObj.id = id
        o.type = 'link'
        o.id = id
        o.sourceId = conn.sourceId
        o.targetId = conn.targetId
        o.label = label
        o.props = {
          base: {
            title: label,
            type: sourceFork ? 1 : 0,
            formId: "",
            judge: "",
          }
        }
        o.cls = {
          linkType: this.flowConfig.jsPlumbInsConfig.Connector[0],
          linkColor: this.flowConfig.jsPlumbInsConfig.PaintStyle.stroke,
          linkThickness: this.flowConfig.jsPlumbInsConfig.PaintStyle.strokeWidth
        }
        document.querySelector('#' + id).addEventListener('contextmenu', e => {
          this.showLinkContextMenu(e)
          this.currentSelect = this.flowData.linkList.find(l => l.id === id)
        })

        document.querySelector('#' + id).addEventListener('click', e => {
          let event = window.event || e
          event.stopPropagation()
          this.currentSelect = this.flowData.linkList.find(l => l.id === id)
        })

        if (this.flowData.status !== flowStatus.LOADING) {
          this.flowData.linkList.push(o)
          let connGet = this.plumb.getConnections({
            source: conn.sourceId,
            target: conn.targetId
          })[0]
          connGet.setLabel({
            label: label,
            cssClass: `linkLabel ${id}`
          })
        }
      })

      this.plumb.importDefaults({
        ConnectionsDetachable: this.flowConfig.jsPlumbConfig.conn.isDetachable
      })
    },

    /**
     * 初始化快捷键
     */
    listenShortcut() {
      document.onkeydown = e => {
        let event = window.event || e

        // 画布聚焦开启快捷键
        if (!this.activeShortcut) return
        let key = event.keyCode

        switch (key) {
          case shortcutKeys.multiple.code:
            this.$refs.flowArea.rectangleMultiple.flag = true
            break
          case shortcutKeys.dragContainer.code:
            this.$refs.flowArea.container.dragFlag = true
            break
          case shortcutKeys.dragTool.code:
            this.selectTool('drag')
            break
          case shortcutKeys.connTool.code:
            this.selectTool('connection')
            break
          case shortcutKeys.leftMove.code:
            this.moveNode('left')
            break
          case shortcutKeys.upMove.code:
            this.moveNode('up')
            break
          case shortcutKeys.rightMove.code:
            this.moveNode('right')
            break
          case shortcutKeys.downMove.code:
            this.moveNode('down')
            break
        }

        if (event.ctrlKey) {
          switch (key) {
            case shortcutKeys.settingModal.code:
              this.saveFlow()
              break
            case shortcutKeys.testModal.code:
              this.openTest()
              break
          }
        }
      }
      // 拖拽、缩放、多选快捷键复位
      document.onkeyup = e => {
        let event = window.event || e

        let key = event.keyCode
        if (key === shortcutKeys.dragContainer.code) {
          this.$refs.flowArea.container.dragFlag = false
        } else if (key === shortcutKeys.multiple.code) {
          this.$refs.flowArea.rectangleMultiple.flag = false
        }
      }
    },

    /**
     * 初始化流程图
     */
    initFlow() {
      if (this.flowData.status === flowStatus.CREATE) {
        this.flowData.attr.id = 'flow-' + utils.getId()
      } else {
        this.loadFlow()
      }
    },

    /**
     * 渲染流程
     * @param {*} json 
     */
    loadFlow(json) {
      this.clear()
      this.$nextTick(() => {
        let loadData = JSON.parse(json)
        this.flowData.attr = loadData.attr
        this.flowData.config = loadData.config
        this.flowData.status = flowStatus.LOADING
        this.plumb.batch(() => {
          let nodeList = loadData.nodeList
          nodeList.forEach(node => {
            this.flowData.nodeList.push(node)
          })
          let linkList = loadData.linkList
          this.$nextTick(() => {
            linkList.forEach(link => {
              this.flowData.linkList.push(link)
              let conn = this.plumb.connect({
                source: link.sourceId,
                target: link.targetId,
                anchor: this.flowConfig.jsPlumbConfig.anchor.default,
                connector: [
                  link.cls.linkType,
                  {
                    gap: 5,
                    cornerRadius: 8,
                    alwaysRespectStubs: true
                  }
                ],
                paintStyle: {
                  stroke: link.cls.linkColor,
                  strokeWidth: link.cls.linkThickness
                }
              })
              let linkId = conn.canvas.id
              let labelHandle = e => {
                let event = window.event || e
                event.stopPropagation()
                this.currentSelect = this.flowData.linkList.find(
                  l => l.id === linkId
                )
              }

              if (link.label !== '') {
                conn.setLabel({
                  label: link.label,
                  cssClass: `linkLabel ${linkId}`
                })

                // 添加label点击事件
                document
                  .querySelector('.' + linkId)
                  .addEventListener('click', labelHandle)
              } else {
                if (document.querySelector('.' + linkId)) {
                  // 移除label点击事件
                  document
                    .querySelector('.' + linkId)
                    .removeEventListener('click', labelHandle)
                }
              }
            })
            this.currentSelect = {}
            this.currentSelectGroup = []
            this.flowData.status = flowStatus.MODIFY
          })
        }, true)
        this.$refs.flowArea.container.pos = {
          top: 0,
          left: 0
        }
      })
    },

    /**
     * 查找相关节点
     * @param {*} belongTo 
     * @param {*} type 
     * @param {*} callback 
     */
    findNodeConfig(belongTo, type, callback) {
      let node = null
      switch (belongTo) {
        case 'commonNodes':
          node = commonNodes.find(n => n.type === type)
          break
        case 'highNodes':
          node = highNodes.find(n => n.type === type)
          break
        case 'laneNodes':
          node = laneNodes.find(n => n.type === type)
          break
      }
      callback(node)
    },

    /**
     * 设置工具
     * @param {*} type 
     */
    selectTool(type) {
      let tool = tools.find(t => t.type === type)
      if (tool) this.currentTool = tool

      switch (type) {
        case 'drag':
          this.changeToDrag()
          break
        case 'connection':
          this.changeToConnection()
          break
      }
    },

    /**
     * 切换为拖拽
     */
    changeToDrag() {
      this.flowData.nodeList.forEach(node => {
        let f = this.plumb.toggleDraggable(node.id)
        if (!f) {
          this.plumb.toggleDraggable(node.id)
        }
        if (node.type !== 'x-lane' && node.type !== 'y-lane') {
          this.plumb.unmakeSource(node.id)
          this.plumb.unmakeTarget(node.id)
        }
      })
    },

    /**
     * 切换为连线
     */
    changeToConnection() {
      this.flowData.nodeList.forEach(node => {
        let f = this.plumb.toggleDraggable(node.id)
        if (f) {
          this.plumb.toggleDraggable(node.id)
        }
        if (node.type !== 'x-lane' && node.type !== 'y-lane') {
          this.plumb.makeSource(
            node.id,
            this.flowConfig.jsPlumbConfig.makeSourceConfig
          )
          this.plumb.makeTarget(
            node.id,
            this.flowConfig.jsPlumbConfig.makeTargetConfig
          )
        }
      })

      this.currentSelect = {}
      this.currentSelectGroup = []
    },

    /**
     * 切换为放大工具
     */
    changeToZoomIn() {
      console.log('切换到放大工具')
    },
    /**
     * 切换为缩小工具
     */
    changeToZoomOut() {
      console.log('切换到缩小工具')
    },

    /**
     * 检测流程数据有效性
     */
    checkFlow() {
      let nodeList = this.flowData.nodeList

      if (nodeList.length <= 0) {
        this.$message.error('流程图中无任何节点！')
        return false
      }
      return true
    },

    /**
     * 检测流程数据有效性
     */
    publicFlow() {
      this.saveData.isPublic = true;
      this.submitFlow();
    },

    /**
     * 检测流程数据有效性
     */
    saveFlow() {
      this.saveData.isPublic = false;
      this.submitFlow();
    },

    /**
     * 保存流程
     */
    submitFlow() {
      let process = Object.assign({}, this.flowData)

      if (!this.checkFlow()) return

      let that = this;
      //转化为Json字符串
      var activity = [],
        link = [],
        processId = that.processId;
      //状态
      process.nodeList.forEach((item, i) => {
        var type = item.type;
        if (['x-lane', 'y-lane'].indexOf(item.type) == -1) {
          activity.push({
            activityId: item.id.replace(type + '-', ''),
            type: type,
            title: item.text.text,
            base: { formId: ['fork', 'join'].indexOf(item.type) > -1 ? null : item.props.base.formId },
            json: JSON.stringify(item),
          });
        }
      });
      //连线
      process.linkList.forEach((item, i) => {
        let type = item.type;
        let begin = item.sourceId.split('-').length == 6 ? item.sourceId.replace(item.sourceId.split('-')[0] + '-', '') : item.sourceId;
        let end = item.targetId.split('-').length == 6 ? item.targetId.replace(item.targetId.split('-')[0] + '-', '') : item.targetId;
        link.push({
          linkId: item.id.replace(type + '-', ''),
          title: item.props.base.title,
          begin: begin,
          end: end,
          type: item.props.base.type,
          judge: item.props.base.judge,
          json: JSON.stringify(item),
        });
      });
      that.saveData.value.activities = activity;
      that.saveData.value.links = link;
      that.saveData.value.designJson = JSON.stringify(process);
      that.saveData.value.processId = processId;
      that.saveData.value.isNewVersion = null;
      that.saveData.value.isPublic = that.saveData.isPublic;
      if (that.saveData.isPublic) {
        that.public.title = "发布流程:" + that.process.name;
        that.public.content = "当前版本:" + that.process.version;
        that.public.visible = true;
      } else {
        that.saveDataOk(false)
      }
    },

    /**
         * 保存数据
         */
    saveDataOk(isNewVersion = false) {
      let that = this;
      that.$message.loading({
        content: this.saveData.isPublic ? "发布中..." : "保存中...",
      });
      this.public.visible = false;
      this.saveData.value.isNewVersion = isNewVersion;
      this.loading = true;
      save(this.saveData.value).then(function (result) {
        that.loading = false;
        that.$message.destroy();
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.$emit('save')
          // that.exportFlowPicture(true);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 生成流程图片
     * @param {*} isSave 
     */
    exportFlowPicture(isSave = false) {
      if (!this.checkFlow()) return
      if (!isSave) {
        this.$message.loading('生成中,请稍等...', 0)
      }


      let that = this;
      let $Container = this.$refs.flowArea.$el.children[0]
      let svgElems = $Container.querySelectorAll('svg[id^="link-"]')
      let removeArr = []

      svgElems.forEach(svgElem => {
        let linkCanvas = document.createElement('canvas')
        let canvasId = 'linkCanvas-' + utils.getId()
        linkCanvas.id = canvasId
        removeArr.push(canvasId)

        let svgContent = svgElem.outerHTML.trim()
        canvg(linkCanvas, svgContent)
        if (svgElem.style.position) {
          linkCanvas.style.position += svgElem.style.position
          linkCanvas.style.left += svgElem.style.left
          linkCanvas.style.top += svgElem.style.top
        }
        $Container.appendChild(linkCanvas)
      })

      let canvasSize = this.computeCanvasSize()

      let pbd = this.flowConfig.defaultStyle.photoBlankDistance
      let offsetPbd = utils.div(pbd, 2)
      html2canvas($Container, {
        useCORS: true,
        width: canvasSize.maxX + offsetPbd,
        height: canvasSize.maxY + offsetPbd,
        scrollX: 0,
        scrollY: 0,
        logging: false,
        onclone: () => {
          removeArr.forEach(id => {
            let currentNode = document.querySelector('#' + id)
            currentNode.parentNode.removeChild(currentNode)
          })
        }
      }).then(canvas => {
        let dataURL = canvas.toDataURL('image/png')
        that.flowPicture.url = dataURL
        if (isSave) {
          thumbnail({
            processId: that.processId,
            thumbnail: dataURL
          }).then(function (result) {
          });
        } else {
          that.flowPicture.modalVisible = true
        }
        that.$message.destroy();
      })
    },

    /**
     * 下载图片
     */
    downLoadFlowPicture() {
      let alink = document.createElement('a')
      let alinkId = 'alink-' + utils.getId()
      alink.id = alinkId
      alink.href = this.flowPicture.url
      alink.download = '流程设计图_' + this.flowData.attr.id + '.png'
      alink.click()
    },

    /**
     * 取消下载
     */
    cancelDownLoadFlowPicture() {
      this.flowPicture.url = ''
      this.flowPicture.modalVisible = false
    },

    /**
     * 计算流程图宽高
     */
    computeCanvasSize() {
      let nodeList = Object.assign([], this.flowData.nodeList)
      let minX = nodeList[0].x
      let minY = nodeList[0].y
      let maxX = nodeList[0].x + nodeList[0].width
      let maxY = nodeList[0].y + nodeList[0].height

      nodeList.forEach(node => {
        minX = Math.min(minX, node.x)
        minY = Math.min(minY, node.y)
        maxX = Math.max(maxX, node.x + node.width)
        maxY = Math.max(maxY, node.y + node.height)
      })
      let canvasWidth = maxX - minX
      let canvasHeight = maxY - minY
      return {
        width: canvasWidth,
        height: canvasHeight,
        minX: minX,
        minY: minY,
        maxX: maxX,
        maxY: maxY
      }
    },
    /**
     * 清除画布
     */
    clear() {
      this.flowData.nodeList.forEach(node => {
        this.plumb.remove(node.id)
      })
      this.currentSelect = {}
      this.currentSelectGroup = []
      this.flowData.nodeList = []
      this.flowData.linkList = []
      this.flowData.remarks = []
    },
    /**
     * 显示隐藏网格
     */
    toggleShowGrid() {
      let flag = this.flowData.config.showGrid
      if (flag) {
        this.flowData.config.showGrid = false
        this.flowData.config.showGridText = '显示网格'
        this.flowData.config.showGridIcon = 'eye-invisible'
      } else {
        this.flowData.config.showGrid = true
        this.flowData.config.showGridText = '隐藏网格'
        this.flowData.config.showGridIcon = 'eye'
      }
    },
    /**
     * 设置
     */
    setting() {
      this.$refs.settingModal.open()
    },
    /**
     * 快捷键大全
     */
    shortcutHelper() {
      this.$refs.shortcutModal.open()
    },

    /**
     * 连接线右键
     * @param {*} e 
     */
    showLinkContextMenu(e) {
      let event = window.event || e

      event.preventDefault()
      event.stopPropagation()
      document.querySelector('.vue-contextmenuName-flow-menu').style.display =
        'none'
      document.querySelector('.vue-contextmenuName-node-menu').style.display =
        'none'
      let x = event.clientX
      let y = event.clientY
      this.linkContextMenuData.axis = { x, y }
    },
    /**
     * 删除线
     */
    deleteLink() {
      let that = this;
      deleteConfirm(
        "确定连线" + (that.currentSelect.label ? '【' + that.currentSelect.label + '】' : ""),
        function () {
          let sourceId = that.currentSelect.sourceId
          let targetId = that.currentSelect.targetId
          that.plumb.deleteConnection(
            that.plumb.getConnections({
              source: sourceId,
              target: targetId
            })[0]
          )
          let linkList = that.flowData.linkList
          linkList.splice(
            linkList.findIndex(
              link => link.sourceId === sourceId && link.targetId === targetId
            ),
            1
          )
          that.currentSelect = {}
        },
        that
      );

    },
    /**
     * 设置快捷键失效
     */
    loseShortcut() {
      this.activeShortcut = false
    },
    /**
     * 设置快捷键启用
     */
    getShortcut() {
      this.activeShortcut = true
    },
    /**
     * 测试
     */
    openTest() {
      let flowObj = Object.assign({}, this.flowData)
      this.$refs.testModal.flowData = flowObj
      this.$refs.testModal.testVisible = true
    },
    /**
     * 键盘移动节点
     * @param {*} type 
     */
    moveNode(type) {
      let m = this.flowConfig.defaultStyle.movePx
      let isX = true

      switch (type) {
        case 'left':
          m = -m
          break
        case 'up':
          m = -m
          isX = false
          break
        case 'right':
          break
        case 'down':
          isX = false
      }

      if (this.currentSelectGroup.length > 0) {
        this.currentSelectGroup.forEach(node => {
          if (isX) {
            node.x += m
          } else {
            node.y += m
          }
        })
        this.plumb.repaintEverything()
      } else if (this.currentSelect.id) {
        if (isX) {
          this.currentSelect.x += m
        } else {
          this.currentSelect.y += m
        }
        this.plumb.repaintEverything()
      }
    },

    settingNode(data) {
      this.$refs.flowAttr.show(data.type)
    },

    settingLink() {
      this.$refs.flowAttr.show(this.currentSelect.props.base.type == 1 ? 'forklink' : 'link')
    }
  }
}
</script>
<style scoped>
/deep/ .ant-drawer-wrapper-body {
  overflow: hidden !important;
}

/deep/ .ant-layout-sider-light {
  background: #002752 !important;
}

/deep/ .ant-layout-header {
  height: 34px !important;
  padding: 0 10px !important;
  line-height: 34px !important;
  background: #fff !important;
}
</style>
