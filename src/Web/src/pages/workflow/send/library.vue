<template>
  <a-card :hoverable="true">
    <a-spin :spinning="spinning">
      <div class="form_container" :style="{ 'min-height': height + 'px' }">
        <a-result status="500" title="请配置流程权限" sub-title="" v-if="!spinning && data.length == 0">
        </a-result>
        <ul class="set_ul">
          <li class="set_li" v-for="(item, index) in data" :key="index">
            <div class="set_title">
              {{ item.typeName }}
              <a-badge class="margin-left-xs" :count="item.list.length" />
            </div>

            <div class="set_cont">
              <div class="set_list" @click="workflowCreate(process)" v-for="(process, indexp) in item.list" :key="indexp">
                <div class="set_left">
                  <h1 class="set_list_title">
                    <a-tooltip>
                      <template slot="title"> 点击收藏 </template>
                      <a-icon type="star"></a-icon>
                    </a-tooltip>

                    <a-tooltip>
                      <template slot="title">
                        {{ process.name }}
                      </template>
                      {{ process.name }}
                    </a-tooltip>
                  </h1>
                  <div class="set_list_cont">
                    <a-tooltip>
                      <template slot="title">
                        {{ process.remark ? process.remark : "暂无说明。" }}
                      </template>
                      {{ process.remark ? process.remark : "暂无说明。" }}
                    </a-tooltip>
                  </div>
                </div>
                <div class="set_right">
                  <img v-if="process.image" :src="process.image" alt="" />
                  <a-icon :style="{ fontSize: '50px', color: process.iconColor }" v-if="process.icon && !process.image"
                    :type="process.icon"></a-icon>

                  <a-icon v-if="!process.icon && !process.image" :style="{ fontSize: '50px', color: process.iconColor }"
                    type="audit"></a-icon>
                </div>
              </div>
            </div>
          </li>
        </ul>
        <a-back-top :target="targetFn" />
      </div>
    </a-spin>

    <workflow-view ref="edit" v-if="workflowDo.visible" :visible.sync="workflowDo.visible" :title="workflowDo.title"
      :isWorkflow="true" :workflowData="workflowDo.data" @ok="initWorkflow"></workflow-view>
  </a-card>
</template>

<script>
import { library } from "@/services/workflow/send/library";
import workflowView from "@/pages/system/agile/run/edit";
export default {
  components: { workflowView },
  data() {
    return {
      height: window.innerHeight - 160,
      data: [],
      spinning: false,
      //编辑组件
      workflowDo: {
        visible: false,
        title: null,
        data: {
          processInstanceId: null,
          activityId: null,
          taskId: null,
        },
      },
    };
  },
  created() {
    this.init();
  },

  methods: {
    /**
     * 发起流程
     */
    workflowCreate(process) {
      this.workflowDo.data = {
        processId: process.processId,
        processInstanceId: null,
        activityId: null,
        taskId: null,
        type: this.eipWorkflowDoType["审核"],
      };
      this.workflowDo.title = process.name;
      this.workflowDo.visible = true;
    },
    initWorkflow() { },
    /**
     *
     */
    targetFn() {
      return document.getElementById("popContainer");
    },
    /**
     * 初始化流程意见数据
     */
    init() {
      let that = this;
      that.spinning = true;
      library().then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.data = result.data;
        }
        that.spinning = false;
      });
    },
  },
};
</script>

<style lang="less" scoped>
.set_ul .set_li .set_title {
  color: rgba(0, 0, 0, 0.6);
  font-size: 14px;
  margin-bottom: 8px;
  font-weight: bold;
}

.set_ul .set_li .set_cont {
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -ms-flex-wrap: wrap;
  flex-wrap: wrap;
}

.set_ul .set_li .set_cont .set_list {
  cursor: pointer;
  width: 240px;
  height: 86px;
  border: 1px solid rgba(0, 0, 0, 0.10196078);
  -webkit-box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
  border-radius: 2px;
  padding: 10px;
  margin-right: 24px;
  margin-bottom: 24px;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
}

.set_ul .set_li .set_cont .set_list:hover {
  border: 1px solid @primary-color;
  -webkit-box-shadow: 0 2px 4px rgba(208, 78, 55, 0.2);
  box-shadow: 0 2px 4px rgba(208, 78, 55, 0.2);
}

.set_ul .set_li .set_cont .set_list .set_left {
  margin-right: 10px;
  width: 180px;
}

.set_ul .set_li .set_cont .set_list .set_left .set_list_title {
  color: rgba(0, 0, 0, 0.8);
  font-weight: 700;
  margin-bottom: 8px;
  font-size: 15px;

  word-break: break-all;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 1;
  overflow: hidden;
}

.set_ul .set_li .set_cont .set_list .set_left .set_list_cont {
  font-size: 14px;
  color: rgba(0, 0, 0, 0.4);
  word-break: break-all;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 2;
  overflow: hidden;
}

.set_ul .set_li .set_cont .set_list .set_right {
  line-height: 96px;
}

.set_ul .set_li .set_cont .set_list .set_right img {
  margin-top: 0px;
  width: 48px;
}

/deep/ img {
  vertical-align: initial;
}

.set_ul .set_li {
  list-style-type: none;
}

.form_container {
  /*! max-width:792px; */
  margin: 10px auto 0;
}
</style>
