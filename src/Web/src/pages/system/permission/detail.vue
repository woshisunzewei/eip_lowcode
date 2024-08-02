<template>

  <a-modal width="800px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">

    <a-tabs default-active-key="1" size="small">
      <a-tab-pane key="1" tab="角色">
        <vxe-table show-header-overflow :height="height" :data="role">
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="name" title="名称" width="100"></vxe-column>
          <vxe-column field="organization" title="组织架构"></vxe-column>
        </vxe-table>
      </a-tab-pane>
      <a-tab-pane key="2" tab="人员">http://localhost:8888/pages/system/menubutton/list
        <vxe-table show-header-overflow :height="height" :data="user">
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="name" title="名称" width="100"></vxe-column>
          <vxe-column field="organization" title="组织架构"></vxe-column>
        </vxe-table>
      </a-tab-pane>
      <a-tab-pane key="3" tab="归属机构">
        <vxe-table show-header-overflow :height="height" :data="organization">
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="name" title="名称" width="100"></vxe-column>
          <vxe-column field="organization" title="组织架构"></vxe-column>
        </vxe-table>
      </a-tab-pane>
      <a-tab-pane key="4" tab="岗位">
        <vxe-table show-header-overflow :height="height" :data="post">
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="name" title="名称" width="100"></vxe-column>
          <vxe-column field="organization" title="组织架构"></vxe-column>
        </vxe-table>
      </a-tab-pane>
      <a-tab-pane key="5" tab="组">
        <vxe-table show-header-overflow :height="height" :data="group">
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="name" title="名称" width="100"></vxe-column>
          <vxe-column field="organization" title="组织架构"></vxe-column>
        </vxe-table>
      </a-tab-pane>
    </a-tabs>

    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel">关闭</a-button>
      </a-space>
    </template>
  </a-modal>
</template>
<script>
import { detail } from "@/services/system/permission/detail";
export default {
  name: "detail-permission",
  data() {
    return {
      height: "300px",
      role: [],
      group: [],
      post: [],
      user: [],
      organization: [],
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
    },
    id: {
      type: String,
    },
    access: {
      type: Number,
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
     * 查找权限信息
     */
    find() {
      let that = this;
      detail(this.id, this.access).then(function (result) {
        let form = result.data;
        that.role = form.role;
        that.group = form.group;
        that.post = form.post;
        that.user = form.user;
        that.organization = form.organization;
      });
    },
  },
};
</script>

<style lang="less" scoped>
/deep/ .ant-tabs-bar {
  margin: 0 0 4px 0 !important;
}
</style>
