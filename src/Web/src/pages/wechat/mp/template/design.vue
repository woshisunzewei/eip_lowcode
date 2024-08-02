<template>
  <a-drawer width="100%" placement="right" :destroyOnClose="true" :visible="visible" :bodyStyle="drawer.bodyStyle"
    @close="cancel">
    <a-spin :spinning="spinning">
      <k-form-design ref="kfd" toolbarsTop :showToolbarsText="true" :showHead="false" :toolbars="design.toolbars"
        :style="{ height: height.edit + 'px' }" @save="saveDesign">
        <!-- 左侧操作区域插槽 start -->
        <template slot="left-action">
          <a-tooltip :title="title">
            {{ title }}
          </a-tooltip>
        </template>
        <!-- 左侧操作区域插槽 end -->
        <!-- 右侧操作区域插槽 start -->
        <template slot="right-action"> </template>
        <!-- 右侧操作区域插槽 end --></k-form-design>
    </a-spin>
  </a-drawer>
</template>

<script>
import Vue from "vue";
import KFormDesign from "@/pages/system/agile/form/components/packages";
import "@/pages/system/agile/form/components/styles/form-design.less";

Vue.use(KFormDesign);
import { save, findById } from "@/services/wechat/mp/template/edit";
export default {
  name: "wechatmptemplatedesigner",
  data() {
    return {
      design: {
        toolbars: [
          "save",
          "preview",
          "importJson",
          "exportJson",
          "exportCode",
          "reset",
        ],
      },

      height: {
        edit: window.innerHeight - 26,
      },

      form: {
        templateId: null,
        code: null,
        title: null,
        design: "",
        industryOne: "",
        industryTwo: "",
        remark: null,
      },

      drawer: {
        bodyStyle: {
          padding: "10px",
          "padding-bottom": "4px",
        },
      },

      loading: false,
      spinning: false,

      controls: [],
      edit: {
        controls: [], //所有控件信息
      },
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    templateId: {
      type: String,
    },
    title: {
      type: String,
    },
  },
  mounted() {
    this.find();
  },
  methods: {
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 保存表单设计
     */
    saveDesign(values) {
      let that = this;
      this.form.design = values;
      save(this.form).then(function (result) {
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 根据Id查找
     */
    find() {
      let that = this;
      that.spinning = true;
      if (this.templateId) {
        findById(this.templateId).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            that.form = result.data;
            if (that.form.design) {
              that.$refs.kfd.handleSetData(JSON.parse(that.form.design));
            }
          } else {
            that.$message.error(result.msg);
          }
          that.spinning = false;
        });
      }
    },
  },
};
</script>