<template>
  <div class="beauty-scroll">
    <template v-if="layout.length">
      <grid-layout
        :style="{ height: height }"
        style="overflow: auto"
        :layout.sync="layout"
        :row-height="40"
        :is-draggable="false"
        :is-resizable="false"
      >
        <grid-item
          v-for="item in layout"
          :x="item.x"
          :y="item.y"
          :w="item.w"
          :h="item.h"
          :i="item.i"
          :key="item.i"
          static
        >
          <notice v-if="item.name === '公告栏'" :panelSetIcon="true"></notice>
          <document
            v-if="item.name === '待办公文'"
            :panelSetIcon="true"
          ></document>
          <quick-operation
            v-if="item.name === '快捷操作'"
            :panelSetIcon="true"
          ></quick-operation>
          <often-app
            v-if="item.name === '常用应用'"
            :panelSetIcon="true"
          ></often-app>
          <often-apply
            v-if="item.name === '常用流程'"
            :panelSetIcon="true"
          ></often-apply>
          <person v-if="item.name === '个人信息'" :panelSetIcon="true"></person>
          <wait-matter
            v-if="item.name === '待办事项'"
            :panelSetIcon="true"
          ></wait-matter>
          <remind v-if="item.name === '督办提醒'" :panelSetIcon="true"></remind>
          <meeting
            v-if="item.name === '我的会议'"
            :panelSetIcon="true"
          ></meeting>
          <my-task
            v-if="item.name === '我的任务'"
            :panelSetIcon="true"
          ></my-task>
          <process
            v-if="item.name === '待办流程'"
            :panelSetIcon="true"
          ></process>
          <my-document
            v-if="item.name === '督办公文'"
            :panelSetIcon="true"
          ></my-document>
          <zj v-if="item.name === '人事'" :panelSetIcon="true"></zj>
          <zjSearch v-if="item.name === '搜索'" :panelSetIcon="true"></zjSearch>
          <unit
            :name="item.name"
            v-if="item.type === 'unit'"
            :id="item.i"
            :panelSetIcon="true"
            :item="item"
          >
          </unit>
        </grid-item>
      </grid-layout>
    </template>
    <div class="eip-layout-nodata" v-else>
      <img src="@/assets/report-nodata.png" alt="" class="eip-nodata-img" />
      <p class="eip-nodata-txt">暂无数据</p>
    </div>
  </div>
</template>

<script>
import { findById } from "@/services/system/agile/form/designer";
import VueGridLayout from "vue-grid-layout";
import notice from "../portal/components/Notice";
import document from "../portal/components/Document";
import quickOperation from "../portal/components/quickOperation";
import oftenApp from "../portal/components/oftenApp";
import oftenApply from "../portal/components/oftenApply";
import Person from "../portal/components/Person";
import waitMatter from "../portal/components/WaitMatter";
import Remind from "../portal/components/Remind";
import Meeting from "../portal/components/Meeting";
import MyTask from "../portal/components/MyTask";
import Process from "../portal/components/Process";
import MyDocument from "../portal/components/MyDocument";
export default {
  data() {
    return {
      height: this.eipHeaderHeight() - 30 + "px",
      layout: [],
    };
  },
  components: {
    GridLayout: VueGridLayout.GridLayout,
    GridItem: VueGridLayout.GridItem,
    notice,
    document,
    quickOperation,
    oftenApp,
    oftenApply,
    Person,
    waitMatter,
    Remind,
    Meeting,
    MyTask,
    Process,
    MyDocument,
  },
  props: {
    configId: {
      type: String,
    },
    haveCard: {
      type: Boolean,
      default: false,
    }, //是否为设计时界面
  },

  mounted() {
    this.initConfig();
  },

  methods: {
    /**
     * 初始化配置信息
     */
    initConfig() {
      let that = this;
      that.$message.loading("加载中,请稍等...", 0);
      findById(that.configId).then((result) => {
        if (result.code == that.eipResultCode.success) {
          if (result.data.saveJson) {
            that.layout = JSON.parse(result.data.saveJson);
          }
        }
        that.$message.destroy();
      });
    },
  },
};
</script>

<style lang="less" scoped>
.eip-layout-nodata {
  text-align: center;

  left: calc(50% - 200px);

  .eip-nodata-img {
    width: 400px;
    height: 400px;
  }

  .eip-nodata-txt {
    margin-top: -60px;
    font-size: 20px;
    color: #909399;
    line-height: 30px;
  }
}
</style>