<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-tabs default-active-key="1" size="small">
        <a-tab-pane key="1" tab="基础信息">
          <a-descriptions bordered class="eip-descriptions" size="small"
            :column="{ xxl: 1, xl: 3, lg: 3, md: 3, sm: 2, xs: 1 }">
            <a-descriptions-item label="登录名">{{
    user.code
  }}</a-descriptions-item>
            <a-descriptions-item label="姓名">{{
      user.name
    }}</a-descriptions-item>
            <a-descriptions-item label="组织架构">{{
      user.organizationName
    }}</a-descriptions-item>
            <a-descriptions-item label="手机号码">{{
      user.mobile
    }}</a-descriptions-item>
            <a-descriptions-item label="邮箱">{{
      user.email
    }}</a-descriptions-item>
            <a-descriptions-item label="其他联系信息">{{
      user.otherContactInformation
    }}</a-descriptions-item>
            <a-descriptions-item label="禁用">{{
      user.isFreezeFormatter
    }}</a-descriptions-item>
            <a-descriptions-item label="类型">{{
      user.statusName
    }}</a-descriptions-item>
            <a-descriptions-item label="备注">{{
              user.remark
              }}</a-descriptions-item>

          </a-descriptions>
        </a-tab-pane>
        <a-tab-pane key="2" tab="角色">
          <vxe-table show-header-overflow size="small" :height="height" :data="role">
            <template #loading>
              <a-spin></a-spin>
            </template>
            <template #empty>
              <eip-empty />
            </template>
            <vxe-column type="seq" title="序号" width="60"></vxe-column>
            <vxe-column field="name" title="名称"></vxe-column>
            <vxe-column field="organization" title="组织架构"></vxe-column>
          </vxe-table>
        </a-tab-pane>

        <a-tab-pane key="3" tab="岗位">
          <vxe-table show-header-overflow size="small" :height="height" :data="post">
            <template #loading>
              <a-spin></a-spin>
            </template>
            <template #empty>
              <eip-empty />
            </template>
            <vxe-column type="seq" title="序号" width="60"></vxe-column>
            <vxe-column field="name" title="名称"></vxe-column>
            <vxe-column field="organization" title="组织架构"></vxe-column>
          </vxe-table>
        </a-tab-pane>
        <a-tab-pane key="4" tab="组">
          <vxe-table show-header-overflow size="small" :height="height" :data="group">
            <template #loading>
              <a-spin></a-spin>
            </template>
            <template #empty>
              <eip-empty />
            </template>
            <vxe-column type="seq" title="序号" width="60"></vxe-column>
            <vxe-column field="name" title="名称"></vxe-column>
            <vxe-column field="organization" title="组织架构"></vxe-column>
          </vxe-table>
        </a-tab-pane>
      </a-tabs>
    </a-spin>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel"><a-icon type="close" />关闭</a-button>
      </a-space>
    </template>
  </a-modal>
</template>

<script>
import { detail } from "@/services/system/user/detail";
export default {
  name: "detail-user",
  data() {
    return {
      height: "343px",
      user: {},
      role: [],
      group: [],
      post: [],
      spinning: false,
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    userId: String,
    title: {
      type: String,
    },
  },
  created() {
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
    find(id) {
      let that = this;
      this.spinning = true;
      detail(this.userId).then(function (result) {
        that.spinning = false;
        let form = result.data;
        that.role = form.role;
        that.group = form.group;
        that.post = form.post;
        that.user = form;
      });
    },
  },
};
</script>

<style lang="less" scoped>
/deep/ .ant-tabs-bar {
  margin: 0 0 4px 0 !important;
}

.eip-descriptions /deep/ .ant-descriptions-item-colon {
  width: 140px !important;
}
</style>
