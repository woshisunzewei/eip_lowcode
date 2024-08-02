<template>
  <a-drawer width="100%" placement="right" :visible="visible" :closable="false" :bodyStyle="drawer.bodyStyle"
    @close="cancel" :destroyOnClose="true">
    <a-spin :spinning="spinning">
      <div class="home">
        <div class="operating-area">
          <div class="left-btn-box">
            <a-tooltip title="预览">
              <a @click="preview">
                <a-icon type="chrome" />
                <span>预览</span>
              </a>
            </a-tooltip>
            <a-divider type="vertical" />
            <a-tooltip title="查看">
              <a @click="catJson">
                <a-icon type="eye" />
                <span>查看</span>
              </a>
            </a-tooltip>

            <a-divider type="vertical" />
            <a-tooltip title="导出">
              <a @click="handleExportJson">
                <a-icon type="arrow-down" />
                <span>导出</span>
              </a>
            </a-tooltip>

            <a-divider type="vertical" />
            <a-tooltip title="导入">
              <a @click="$refs.file.click()">
                <a-icon type="file-text" />
                <span>导入</span>
              </a>
            </a-tooltip>
            <a-divider type="vertical" />
            <a-tooltip title="保存">
              <a @click="mobileSave">
                <a-icon type="save" />
                <span>保存</span>
              </a>
            </a-tooltip>
            <a-divider type="vertical" />
            <a-tooltip title="发布">
              <a @click="mobilePublc">
                <a-icon type="build" />
                <span>发布</span>
              </a>
            </a-tooltip>
            <input type="file" ref="file" id="file" accept=".json" @change="importJSON" style="display: none" />
          </div>
          <div class="right-btn-box">
            <div class="margin-right-xl">
              <a-tooltip :title="form.name"><span class="text-bold">{{ form.name }}</span></a-tooltip>
              <a-divider type="vertical" />
              <a-tooltip title="关闭">
                <a @click="cancel" style="color: #ff4d4f">
                  <a-icon type="close" />
                  <span>关闭</span>
                </a>
              </a-tooltip>
            </div>
          </div>
        </div>
        <!-- 装修操作 -->
        <section class="operation">
          <div>
            <!-- 组件 -->
            <sliderassembly :pointer="pointer" />
          </div>

          <!-- 手机 -->
          <div class="phone">
            <section class="phoneAll" ref="imageTofile" id="imageTofile">
              <img src="./assets/images/phoneTop.png" alt="" class="statusBar" />

              <!-- 头部导航 -->
              <headerTop :pageSetup="pageSetup" @click.native="headTop" />

              <!-- 主体内容 -->
              <section class="phone-container" :style="{
                'background-color': pageSetup.bgColor,
                backgroundImage: 'url(' + pageSetup.bgImg + ')',
              }" @drop="drop($event)" @dragover="allowDrop($event)" @dragleave="dragleaves($event)">
                <div :class="pointer.show ? 'pointer-events' : ''">
                  <!-- 动态组件 -->
                  <component :is="item.component" v-for="(item, index) in pageComponents" :key="index"
                    :datas="item.setStyle" :style="{
                      border:
                        item.active && deleShow ? '1px solid #d0d0d0' : '',
                    }" @click.native="activeComponent(item, index)" class="componentsClass" :data-type="item.type">
                    <div v-show="deleShow" class="deles" slot="deles">
                      <a-popconfirm title="确定删除配置?" ok-text="确定" cancel-text="取消" @confirm="deleteObj(index)">
                        <!-- 删除组件 -->
                        <div @click.stop="">
                          <span class="iconfont icon-sanjiaoxingzuo"></span>
                          {{ item.text }}
                          <a-icon type="delete"></a-icon>
                        </div>
                      </a-popconfirm>
                    </div>
                  </component>
                </div>
              </section>
            </section>
          </div>

          <!-- 页面设置tab -->
          <div class="decorateTab">
            <span :class="rightcom === 'decorate' ? 'active' : ''" @click="rightcom = 'decorate'">
              <i class="iconfont icon-wangye" />
              页面设置
            </span>
            <span :class="rightcom === 'componenmanagement' ? 'active' : ''" @click="rightcom = 'componenmanagement'">
              <i class="iconfont icon-zujian" />
              组件管理
            </span>
            <span class="active" v-show="rightcom != 'componenmanagement' && rightcom != 'decorate'
              ">
              <i class="iconfont icon-zujian" />
              组件设置
            </span>
          </div>

          <!-- 右侧工具栏 -->
          <div class="decorateAll">
            <!-- 页面设置 -->
            <transition name="decorateAnima">
              <!-- 动态组件 -->
              <component :is="rightcom" :datas="currentproperties" @componenmanagement="componenmanagement" />
            </transition>
          </div>
        </section>

        <realTimeView v-if="realTimeView.show" :datas="realTimeView" :val="{
          id,
          name: pageSetup.name,
          pageSetup: pageSetup,
          pageComponents: pageComponents,
        }" />
      </div>
    </a-spin>

    <a-modal title="导出" v-drag :zIndex="1001" :visible="jsonVisible" @cancel="jsonVisible = false"
      :destroyOnClose="true" style="top: 20px; " width="850px">
      <eip-code :value.sync="editorJson"></eip-code>
      <template slot="footer">
        <a-button @click="handleCopyJson" type="primary" class="copy-btn" data-clipboard-action="copy"
          :data-clipboard-text="editorJson">
          复制数据
        </a-button>
        <a-button @click="handleExportJson" type="primary">
          导出代码
        </a-button>
      </template>
    </a-modal>
  </a-drawer>
</template>

<script>
import utils from "@/pages/system/agile/mobile/utils/index"; // 方法类
import componentProperties from "@/pages/system/agile/mobile/utils/componentProperties"; // 组件数据
import { findById, editSave, editPublic } from "@/services/system/agile/form/designer";
import { thumbnail } from "@/services/system/agile/mobile/designer";
import Clipboard from "clipboard";
import html2canvas from 'html2canvas' // 生成图片
import realTimeView from "@/pages/system/agile/mobile/components/realTimeView";
export default {
  name: "home",
  components: {
    realTimeView
  },
  data() {
    return {
      cmOptions: {
        tabSize: 4, // tab的空格个数
        theme: 'blackboard', //主题样式
        lineNumbers: true, //是否显示行数
        lineWrapping: true, //是否自动换行
        styleActiveLine: true, //line选择是是否加亮
        matchBrackets: true, //括号匹配
        mode: 'text/x-sparksql', //实现javascript代码高亮
        readOnly: false, //只读
        keyMap: 'default',
        extraKeys: { tab: 'autocomplete' },
        foldGutter: true,
        gutters: ['CodeMirror-linenumbers', 'CodeMirror-foldgutter'],
        hintOptions: {
          completeSingle: false,
          tables: {},
        },
      },
      form: {
        configId: "",
        name: "",
      },
      drawer: {
        bodyStyle: {
          padding: "0",
        },
      },

      loading: false,
      spinning: false,

      realTimeView: {
        show: false, // 是否显示预览
      },
      id: null, //当前页面
      deleShow: true, //删除标签显示
      index: "", //当前选中的index
      rightcom: "decorate", //右侧组件切换
      currentproperties: {}, //当前属性
      pageSetup: {
        // 页面设置属性
        name: "页面标题", //页面名称
        details: "", //页面描述
        isPerson: false, // 是否显示个人中心
        isBack: true, // 是否返回按钮
        titleHeight: 35, // 高度
        bgColor: "#F9F9F9", //背景颜色
        bgImg: "", // 背景图片
      },
      pageComponents: [], //页面组件
      offsetY: 0, //记录上一次距离父元素高度
      pointer: { show: false }, //穿透
      onlyOne: ["1-5", "1-16"], // 只能存在一个的组件(组件的type)

      jsonVisible: false,
      editorJson: ""
    };
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
    this.pageSetup.name = "页面标题";
    this.currentproperties = this.pageSetup;
    this.find();
  },

  methods: {
    /**
     * 预览
     */
    preview() {
      let that = this;
      let form = {
        configId: this.configId,
        saveJson: JSON.stringify({
          id: this.configId,
          name: this.pageSetup.name,
          setup: this.pageSetup,
          components: this.pageComponents,
        }),
      };
      that.realTimeView.show = true
      // editSave(form).then(function (result) {
      //   that.$loading.hide();
      //   if (result.code === that.eipResultCode.success) {
      //     that.$nextTick(() => {
      //       that.toImage()
      //     })
      //   } else {
      //     that.$message.error(result.msg);
      //   }
      // });
    },

    /**
     * 配置保存
     */
    mobileSave() {
      let that = this;
      let form = {
        configId: this.configId,
        saveJson: JSON.stringify({
          id: this.configId,
          name: this.pageSetup.name,
          setup: this.pageSetup,
          components: this.pageComponents,
        }),
      };
      that.$loading.show({ text: "保存中,请稍等..." });
      editSave(form).then(function (result) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.$nextTick(() => {
            that.toImage()
          })
        } else {
          that.$message.error(result.msg);
        }
      });
    },
    /**
        * 发布表单设计
        */
    mobilePublc() {
      let that = this;
      var json = JSON.stringify({
        id: this.configId,
        name: this.pageSetup.name,
        setup: this.pageSetup,
        components: this.pageComponents,
      });
      const modal = this.$confirm({
        okText: "确定",
        okType: "danger",
        cancelText: "取消",
        title: "移动端发布提示",
        content: "若已发布会覆盖原来移动端，且不可恢复？",
        onOk() {
          modal.destroy();
          let param = {
            configId: that.configId,
            saveJson: json,
            publicJson: json,
          };

          that.$loading.show({ text: "发布中,请稍等..." });
          editPublic(param).then(function (result) {
            that.$loading.hide();
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              that.$nextTick(() => {
                that.toImage()
              })
            } else {
              that.$message.error(result.msg);
            }
          });
        },
        onCancel() { },
      });
    },

    /**
     * 页面截图
     */
    toImage() {
      // let that = this;
      // const imageTofiles = document.querySelector('#imageTofile')
      // /* 截图 */
      // html2canvas(this.$refs.imageTofile, {
      //   backgroundColor: null,
      //   height: imageTofiles.scrollHeight,
      //   width: imageTofiles.scrollWidth - 8,
      //   useCORS: true,
      // }).then((canvas) => {
      //   /* 显示border和删除图标 */
      //   this.deleShow = true
      //   let url = canvas.toDataURL('image/png')
      //   thumbnail({
      //     configId: that.configId,
      //     thumbnail: url
      //   }).then(function (result) {
      //     that.$emit("reload")
      //   });
      // })
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
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
        if (result.data.saveJson) {
          var importJSON = JSON.parse(result.data.saveJson)
          that.id = importJSON.id;
          that.pageSetup = importJSON.setup;
          that.pageComponents = importJSON.components;
        }
        that.spinning = false;
      });
    },
    /**
     * 查看JSON
     */
    catJson() {
      this.editorJson = JSON.stringify({
        id: this.configId,
        name: this.pageSetup.name,
        setup: this.pageSetup,
        components: this.pageComponents,
      }, null, "\t");
      this.jsonVisible = true;
    },

    /**
     * 
     */
    handleExportJson() {
      // 将json转换成字符串
      const data = JSON.stringify({
        id: this.id,
        name: this.pageSetup.name,
        setup: this.pageSetup,
        components: this.pageComponents,
      })
      const blob = new Blob([data], { type: '' })
      this.$XSaveFile({
        filename: this.pageSetup.name, type: 'json', content: blob
      })
    },

    /**
     * 
     */
    handleCopyJson() {
      // 复制数据
      const clipboard = new Clipboard(".copy-btn");
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
     * 当将元素或文本选择拖动到有效放置目标（每几百毫秒）上时，会触发此事件
     *
     * @param {Object} event event对象
     */
    allowDrop(event) {
      //阻止浏览器的默认事件
      event.preventDefault();

      /* 获取鼠标高度 */
      let eventoffset = event.offsetY;

      /* 如果没有移动不触发事件减少损耗 */
      if (this.offsetY === eventoffset) return;
      else this.offsetY = eventoffset;

      /* 获取组件 */
      const childrenObject = event.target.children[0];

      // 一个以上的组件计算
      if (this.pageComponents.length) {
        /* 如果只有一个组件并且第一个是提示组件直接返回 */
        if (
          this.pageComponents.length === 1 &&
          this.pageComponents[0].type === 0
        )
          return;

        /* 如果鼠标的高度小于第一个的一半直接放到第一个 */
        if (eventoffset < childrenObject.children[0].clientHeight / 2) {
          /* 如果第一个是提示组件直接返回 */
          if (this.pageComponents[0].type === 0) return;

          /* 删除提示组件 */
          this.pageComponents = this.pageComponents.filter(
            (res) => res.component !== "placementarea"
          );

          /* 最后面添加提示组件 */
          this.pageComponents.unshift({
            component: "placementarea",
            type: 0,
          });

          return;
        }

        /* 记录距离父元素高度 */
        const childOff = childrenObject.offsetTop;

        /* 鼠标在所有组件下面 */
        if (
          eventoffset > childrenObject.clientHeight ||
          childrenObject.lastChild.offsetTop -
          childOff +
          childrenObject.lastChild.clientHeight / 2 <
          eventoffset
        ) {
          /* 最后一个组件是提示组件返回 */
          if (this.pageComponents[this.pageComponents.length - 1].type === 0)
            return;

          /* 清除提示组件 */
          this.pageComponents = this.pageComponents.filter(
            (res) => res.component !== "placementarea"
          );

          /* 最后一个不是提示组件添加 */
          this.pageComponents.push({
            component: "placementarea",
            type: 0,
          });

          return;
        }

        const childrens = childrenObject.children;

        /* 在两个组件中间，插入 */
        for (let i = 0, l = childrens.length; i < l; i++) {
          const childoffset = childrens[i].offsetTop - childOff;

          if (childoffset + childrens[i].clientHeight / 2 > event.offsetY) {
            /* 如果是提示组件直接返回 */
            if (this.pageComponents[i].type === 0) break;

            if (this.pageComponents[i - 1].type === 0) break;

            /* 清除提示组件 */
            this.pageComponents = this.pageComponents.filter(
              (res) => res.component !== "placementarea"
            );

            this.pageComponents.splice(i, 0, {
              component: "placementarea",
              type: 0,
            });
            break;
          } else if (childoffset + childrens[i].clientHeight > event.offsetY) {
            if (this.pageComponents[i].type === 0) break;

            if (
              !this.pageComponents[i + 1] ||
              this.pageComponents[i + 1].type === 0
            )
              break;

            this.pageComponents = this.pageComponents.filter(
              (res) => res.component !== "placementarea"
            );

            this.pageComponents.splice(i, 0, {
              component: "placementarea",
              type: 0,
            });

            break;
          }
        }
      } else {
        /* 一个组件都没有直接push */
        this.pageComponents.push({
          component: "placementarea",
          type: 0,
        });
      }
    },

    /**
     * 当在有效放置目标上放置元素或选择文本时触发此事件
     *
     * @param {Object} event event对象
     */
    drop(event) {
      /* 获取数据 */
      let data = utils.deepClone(
        componentProperties.get(event.dataTransfer.getData("componentName"))
      );

      /* 查询是否只能存在一个的组件且在第一个 */
      let someOne = this.pageComponents.some((item, index) => {
        return (
          item.component === "placementarea" &&
          index === 0 &&
          this.onlyOne.includes(data.type)
        );
      });
      if (someOne) {
        this.$message.info("固定位置的组件(如: 底部导航、悬浮)不能放在第一个!");
        /* 删除提示组件 */
        this.dragleaves();
        return;
      }

      /* 查询是否只能存在一个的组件 */
      let someResult = this.pageComponents.some((item) => {
        return (
          this.onlyOne.includes(item.type) &&
          item.component === event.dataTransfer.getData("componentName")
        );
      });
      if (someResult) {
        this.$message.info("当前组件只能添加一个!");
        /* 删除提示组件 */
        this.dragleaves();
        return;
      }

      /* 替换 */
      utils.forEach(this.pageComponents, (res, index) => {
        /* 修改选中 */
        if (res.active === true) res.active = false;
        /* 替换提示 */
        this.index = index;
        if (res.component === "placementarea")
          this.$set(this.pageComponents, index, data);
      });

      /* 切换组件 */
      this.rightcom = data.style;
      /* 丢样式 */
      this.currentproperties = data.setStyle;

    },

    /**
     * 当拖动的元素或文本选择离开有效的放置目标时，会触发此事件
     *
     * @param {Object} event event对象
     */
    dragleaves() {
      /* 删除提示组件 */
      this.pageComponents = this.pageComponents.filter(
        (res) => res.component !== "placementarea"
      );
    },

    /**
     * 切换组件位置
     *
     * @param {Object} res 组件切换后返回的位置
     */
    componenmanagement(res) {
      this.pageComponents = res;
    },

    /**
     * 选择组件
     *
     * @param {Object} res 当前组件对象
     */
    activeComponent(res, index) {
      this.index = index;
      /* 切换组件 */
      this.rightcom = res.style;
      /* 丢样式 */
      this.currentproperties = res.setStyle;

      /* 替换 */
      utils.forEach(this.pageComponents, (res) => {
        /* 修改选中 */
        if (res.active === true) res.active = false;
      });

      /* 选中样式 */
      res.active = true;
    },

    /**
     * 标题切换
     *
     * @param {Object} res 当前组件对象
     */
    headTop() {
      this.rightcom = "decorate";

      /* 替换 */
      utils.forEach(this.pageComponents, (res) => {
        /* 修改选中 */
        if (res.active === true) res.active = false;
      });
    },

    /* 删除组件 */
    deleteObj(index) {
      this.pageComponents.splice(index, 1);
      if (this.index === index) this.rightcom = "decorate";
      if (index < this.index) this.index = this.index - 1;
    },

    // 返回上一步
    Previous() {
      this.$confirm("返回列表您添加或者修改的数据将会失效, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          this.$router.go(-1);
        })
        .catch(() => { });
    },

    /**
     * 导入json
     */
    importJSON() {
      const file = document.getElementById("file").files[0];
      const reader = new FileReader();
      reader.readAsText(file);
      let that = this;
      reader.onload = function () {
        // this.result为读取到的json字符串，需转成json对象
        let importJSON = JSON.parse(this.result);
        // 检测是否导入成功
        that.id = importJSON.id;
        that.pageSetup = importJSON.setup;
        that.pageComponents = importJSON.components;
      };
    },
  },

  watch: {
    /* 监听右侧属性设置切换 */
    rightcom(newval) {
      if (newval === "decorate") {
        utils.forEach(this.pageComponents, (res) => {
          /* 修改选中 */
          if (res.active === true) res.active = false;
        });
        this.currentproperties = this.pageSetup;
        return;
      }
      if (newval === "componenmanagement") {
        /* 替换 */
        utils.forEach(this.pageComponents, (res) => {
          /* 修改选中 */
          if (res.active === true) res.active = false;
        });
        this.currentproperties = this.pageComponents;
      }
    },
  },
};
</script>

<style lang="less" scoped>
.pointer-events {
  pointer-events: none;
}

.home {
  width: 100%;
  height: 100vh;
  background: #fff;
  overflow: hidden;

  /* 删除组件 */
  .deles {
    position: absolute;
    min-width: 80px;
    text-align: center;
    line-height: 25px;
    background: #fff;
    height: 25px;
    font-size: 12px;
    left: 103%;
    top: 50%;
    transform: translateY(-50%);

    .icon-sanjiaoxingzuo {
      position: absolute;
      left: -11px;
      color: #fff;
      font-size: 12px;
      top: 50%;
      transform: translateY(-50%);
    }

    &:hover {
      i {
        display: block;
        position: absolute;
        left: 0;
        font-size: 16px;
        top: 0;
        text-align: center;
        line-height: 25px;
        width: 100%;
        color: #fff;
        height: 100%;
        z-index: 10;
        background: rgba(0, 0, 0, 0.5);
      }

      .icon-sanjiaoxingzuo {
        color: rgba(0, 0, 0, 0.5);
      }
    }

    i {
      display: none;
    }
  }

  /* 操作主体 */
  .operation {
    margin-top: 6px;
    width: 100%;
    height: calc(100% - 51px - 6px);
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    background: #f7f8fa;
  }

  /* 手机 */
  .phone {
    width: 100%;
    height: 100%;
    padding-top: 10px;
    background: rgb(247, 248, 250);
    box-shadow: rgb(204, 204, 204) 0px 0px 1px 1px;
    overflow-y: auto;
    position: relative;
    z-index: 0;
    justify-content: center;
    display: flex;
    flex-direction: row;
    align-items: center;

    &::-webkit-scrollbar {
      width: 1px;
    }

    // &::-webkit-scrollbar-thumb {
    //   background-color: @primary-color;
    // }

    /* 手机样式 */
    .phoneAll {
      position: absolute;
      top: 18px;
      width: 375px;
      min-height: calc(100% - 26px);
      background: #fdfdfd;
      display: flex;
      flex-direction: column;

      /* 手机高度 */
      .phoneSize {
        position: absolute;
        left: -137px;
        top: 640px;
        font-size: 12px;
        color: #a2a2a2;
        border-bottom: 1px solid #dedede;
        width: 130px;
        height: 21px;
        line-height: 21px;
      }

      /* 状态栏 */
      .statusBar {
        width: 100%;
        display: block;
      }

      /* 主体内容 */
      .phone-container {
        height: 603px;
        box-sizing: border-box;
        cursor: pointer;
        width: 100%;
        position: relative;
        flex: 1;
        background-repeat: no-repeat;
        background-size: contain;

        .componentsClass {
          border: 1px solid #fff;

          &:hover {
            border: 1px dashed @primary-color;
          }
        }
      }
    }

    .phoneAll:before {
      content: "";
      border-radius: 16px;
      position: absolute;
      top: -8px;
      left: 50%;
      right: 0;
      bottom: -8px;
      z-index: 1;
      margin-left: -195.5px;
      width: 390px;
      height: inherit;
      border: 8px solid #262626;
      pointer-events: none;
      cursor: move;
      overflow: hidden;
    }
  }

  /* 右侧工具栏 */
  .decorateAll {
    width: 576px;
    overflow-y: scroll;
    overflow-x: hidden;
    position: relative;
    padding: 0 12px;
    background: #fff;
    box-shadow: 0px 0px 1px 1px #ccc;

    &::-webkit-scrollbar {
      width: 1px;
    }

    &::-webkit-scrollbar-thumb {
      background-color: @primary-color;
    }
  }

  /* 页面设置tab */
  .decorateTab {
    position: fixed;
    display: flex;
    right: 392px;
    top: 58px;
    flex-direction: column;

    span {
      background-color: #fff;
      box-shadow: 0 2px 8px 0 rgba(0, 0, 0, 0.1);
      border-radius: 2px;
      width: 94px;
      height: 32px;
      display: inline-block;
      text-align: center;
      line-height: 32px;
      margin-bottom: 12px;
      transition: all 0.8s;
      cursor: pointer;

      &.active {
        background-color: @primary-color;
        color: #fff;
      }

      /* 图标 */
      i {
        font-size: 12px;
        margin-right: 5px;
      }
    }
  }
}

/* 动画 */
.decorateAnima-enter-active {
  transition: all 1.5s ease;
}

.decorateAnima-leave-active {
  transition: all 1.5s ease;
}

.decorateAnima-enter {
  transform: translate(8px, 8px);
  opacity: 0;
}

.decorateAnima-leave-to {
  transform: translate(8px, 8px);
  opacity: 0;
}

.operating-area {
  border-bottom: 1px solid #e8e8e8;
  font-size: 16px;
  text-align: left;
  padding: 0px 12px;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-pack: justify;
  -ms-flex-pack: justify;
  justify-content: space-between;
  -ms-flex-line-pack: center;
  align-content: center;
}

.operating-area a {
  color: #666;
  margin: 0 5px;
}

.operating-area a>span {
  font-size: 14px;
  padding-left: 2px;
}
</style>
