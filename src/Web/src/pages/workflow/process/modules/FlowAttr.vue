<template>
  <div>
    <activitybegin ref="activityBegin" v-if="activity.begin.visible" :visible.sync="activity.begin.visible"
      :formId="formId" :data="currentSelect" :title="activity.begin.title" @ok="activityOk">
    </activitybegin>

    <activitytask ref="activitytask" v-if="activity.task.visible" :formId="formId" :visible.sync="activity.task.visible"
      :data="currentSelect" :title="activity.task.title" @ok="activityOk">
    </activitytask>

    <activitylink ref="activityLink" v-if="activity.link.visible" :visible.sync="activity.link.visible"
      :remark="activity.link.title" title="连线说明" @ok="linkOk"></activitylink>

    <activityfork ref="activityfork" v-if="activity.fork.visible" :visible.sync="activity.fork.visible"
      :data="currentSelect" :title="activity.fork.title" @ok="activityOk"></activityfork>

    <activityforklink ref="activityforklink" v-if="activity.forklink.visible" :visible.sync="activity.forklink.visible"
      :data="currentSelect" :formId="formId" :title="activity.forklink.title" @ok="activityForkLinkOk"></activityforklink>

    <activityend ref="activityend" v-if="activity.end.visible" :visible.sync="activity.end.visible" :data="currentSelect"
      :title="activity.end.title" @ok="activityOk"></activityend>

    <activityjoin ref="activityjoin" v-if="activity.join.visible" :visible.sync="activity.join.visible"
      :data="currentSelect" :title="activity.join.title" @ok="activityOk"></activityjoin>

    <activitysubprocess ref="activitysubprocess" v-if="activity.subprocess.visible"
      :visible.sync="activity.subprocess.visible" :data="currentSelect" :title="activity.subprocess.title"
      :formId="formId" @ok="activityOk"></activitysubprocess>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import activitybegin from "../activity/begin";
import activitytask from "..//activity/task";
import activitylink from "..//activity/link";
import activityfork from "..//activity/fork";
import activityforklink from "..//activity/forklink";
import activityend from "..//activity/end";
import activityjoin from "..//activity/join";
import activitysubprocess from "..//activity/subprocess";
export default {
  computed: {
    ...mapGetters("account", ["systemAgile"]),
  },
  props: ['plumb', 'flowData', 'select', 'formId'],
  components: {
    activitybegin,
    activitylink,
    activitytask,
    activityfork,
    activityforklink,
    activityend,
    activityjoin,
    activitysubprocess,
  },
  data() {
    return {
      activity: {
        begin: {
          visible: false,
          title: "",
        },
        task: {
          visible: false,
          title: "",
        },
        link: {
          visible: false,
          title: "",
        },
        fork: {
          visible: false,
          title: "",
        },

        forklink: {
          visible: false,
          title: "",
        },

        end: {
          visible: false,
          title: "",
        },

        join: {
          visible: false,
          title: "",
        },

        subprocess: {
          visible: false,
          title: "",
        },

      },

      currentSelect: this.select,
      config: {
        labelCol: { span: 6 },
        wrapperCol: { span: 18 }
      },
      activeKey: 'flow-attr',

      forms: [], //节点配置表单

      visible: false,
    }
  },
  mounted() {

  },
  methods: {
    /**
     * 确定
     */
    activityOk(data) {

    },

    /**
     * 
     * @param {*} value 
     */
    linkOk(value) {
      let label = value
      this.currentSelect.label = label
      let conn = this.plumb.getConnections({
        source: this.currentSelect.sourceId,
        target: this.currentSelect.targetId
      })[0]
      let linkId = conn.canvas.id
      let labelHandle = e => {
        let event = window.event || e
        event.stopPropagation()
        this.currentSelect = this.flowData.linkList.find(l => l.id === linkId)
      }

      if (label !== '') {
        conn.setLabel({
          label: label,
          cssClass: `linkLabel ${linkId}`
        })
        // 添加label点击事件
        document.querySelector('.' + linkId).addEventListener('click', labelHandle)
      } else {
        // 移除label点击事件
        document.querySelector('.' + linkId).removeEventListener('click', labelHandle)

        let labelOverlay = conn.getLabelOverlay()
        if (labelOverlay) conn.removeOverlay(labelOverlay.id)
      }
    },

    /**
     * 
     */
    activityForkLinkOk(value) {
      this.linkOk(value.title)
    },

    show(type) {
      if (type == 'begin') {
        this.activity.begin.visible = true;
        this.activity.begin.title = this.currentSelect.text.text
      }
      if (type == 'task') {
        this.activity.task.visible = true;
        this.activity.task.title = this.currentSelect.text.text
      }
      if (type == 'end') {
        this.activity.end.visible = true;
        this.activity.end.title = this.currentSelect.text.text
      }
      if (type == 'link') {
        this.activity.link.visible = true;
        this.activity.link.title = this.currentSelect.label
      }
      if (type == 'forklink') {
        this.activity.forklink.visible = true;
        this.activity.forklink.title = this.currentSelect.label
      }

      if (type == 'subprocess') {
        this.activity.subprocess.visible = true;
        this.activity.subprocess.title = this.currentSelect.text.text
      }

      if (type == 'fork') {
        this.activity.fork.visible = true;
        this.activity.fork.title = this.currentSelect.text.text
      }

      if (type == 'join') {
        this.activity.join.visible = true;
        this.activity.join.title = this.currentSelect.text.text
      }
    }
  },
  watch: {
    select(val) {
      this.currentSelect = val
    },
    currentSelect: {
      handler(val) {
        this.$emit('update:select', val)
      },
      deep: true
    }
  }
}
</script>
<style scoped>
/deep/ .ant-tabs-bar {
  margin: 0 !important;

}

/deep/ .ant-tabs-tabpane {
  padding: 2px !important;
}

/deep/ .ant-collapse {
  border-radius: 0 !important;
}

/deep/ .ant-collapse>.ant-collapse-item>.ant-collapse-header {
  padding: 6px 16px;
  padding-left: 40px;
}
</style>
