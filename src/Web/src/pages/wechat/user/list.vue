<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @unbind="unbinduser" @bind="binduserpage" @onload="toolbarOnload"></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="wechatuserlist" size="small" :seq-config="{
      startIndex: (table.page.param.current - 1) * table.page.param.size,
    }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data">
        <template #loading>
          <a-spin></a-spin>
        </template>
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="checkbox" width="40" align="center" fixed="left">
          <template #header="{ checked, indeterminate }">
            <span @click.stop="$refs.table.toggleAllCheckboxRow()">
              <a-checkbox :indeterminate="indeterminate" :checked="checked">
              </a-checkbox>
            </span>
          </template>
          <template #checkbox="{ row, checked, indeterminate }">
            <span @click.stop="$refs.table.toggleCheckboxRow(row)">
              <a-checkbox :indeterminate="indeterminate" :checked="checked">
              </a-checkbox>
            </span>
          </template>
        </vxe-column>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column field="nickName" title="昵称" width="250"></vxe-column>
        <vxe-column title="头像" align="center" width="50">
          <template v-slot="{ row }">
            <a-popover v-if="row.headImgurl" :title="row.nickName">
              <template slot="content">
                <img :src="row.headImgurl" style="width: 132px; height: 132px" />
              </template>
              <a-avatar v-if="row.headImgurl" :src="row.headImgurl" />
            </a-popover>
          </template>
        </vxe-column>
        <vxe-column title="类型" align="center" width="100">
          <template v-slot="{ row }">
            <a-tag color="#f50" v-if="row.type == 1">公众号</a-tag>
            <a-tag color="#2db7f5" v-if="row.type == 2">小程序</a-tag>
            <a-tag color="#87d068" v-if="row.type == 3">企业微信</a-tag>
          </template>
        </vxe-column>
        <vxe-column field="createTime" title="授权时间" width="160"></vxe-column>
        <vxe-column field="lastTime" title="最后授权时间" width="160"></vxe-column>
        <vxe-column field="userName" title="关联用户" width="150"></vxe-column>

        <vxe-column title="操作" align="center" fixed="right" width="160px">
          <template #default="{ row }">
            <a-tooltip @click="tableBindRow(row)" title="绑定" v-if="visible.bind">
              <label class="text-eip eip-cursor-pointer">绑定</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.bind" />
            <a-tooltip title="解绑" v-if="visible.unbind" @click="tableUnBindRow(row)">
              <label class="text-red eip-cursor-pointer">解绑</label>
            </a-tooltip>
          </template>
        </vxe-column>
      </vxe-table>

      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
        :page-size="table.page.param.size" :total="table.page.total" @change="tableInit"
        @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>

    <binduser ref="binduser" v-if="binduser.visible" :visible.sync="binduser.visible" :title="binduser.title"
      :weChatUserId="binduser.weChatUserId" :userId="binduser.userId" @save="reload"></binduser>
  </div>
</template>

<script>
import { query, unbind } from "@/services/wechat/user/list";
import binduser from "./binduser";



import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  data() {
    return {
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "desc",
            sidx: "Id",
            filters: "",
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: window.innerHeight - 254,
        search: {
          option: {
            num: 4,
            config: [
              {
                field: "u.nickName",
                op: "cn",
                placeholder: "请输入微信昵称",
                title: "微信昵称",
                value: "",
                type: "text",
              },
              {
                field: "userinfo.Name",
                op: "cn",
                placeholder: "请输入关联用户姓名",
                title: "关联用户",
                value: "",
                type: "text",
              },
              {
                field: "Type",
                op: "eq",
                title: "类型",
                placeholder: "请选择类型",
                type: "select",
                options: {
                  multiple: true, //多选
                  source: {
                    type: "custom",
                    format: [
                      { key: "1", value: "公众号" },
                      { key: "2", value: "小程序" },
                      { key: "3", value: "企业微信" },
                    ],
                  },
                }
              },
            ],
          },
        },
      },
      visible: {
        bind: true,
        unbind: true,
      },
      binduser: {
        visible: false,
        weChatUserId: "",
        userId: "",
        title: "",
      },
    };
  },
  components: { binduser },
  created() {
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
     * 微信授权用户查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    /**
     * 初始化微信授权用户数据
     */
    tableInit() {
      let that = this;
      that.table.loading = true;
      query(that.table.page.param).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.table.data = result.data;
          that.table.page.total = result.total;
        }
        that.table.loading = false;
      });
    },

    /**
     *数量改变
     */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.tableInit();
    },

    /**
     * 绑定
     */
    tableBindRow(row) {
      this.binduser.userId = row.userId;
      this.binduser.weChatUserId = row.weChatUserId;
      this.binduser.title = "绑定微信授权用户-" + row.nickName;
      this.binduser.visible = true;
    },

    /**
     * 解绑
     */
    tableUnBindRow(row) {
      let that = this;
      deleteConfirm(
        "解除微信授权用户【" + row.nickName + "】绑定",
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          unbind({ id: row.weChatUserId }).then((result) => {
            that.$loading.hide();
            if (result.code == that.eipResultCode.success) {
              that.reload();
              that.$message.success(result.msg);
            } else {
              that.$message.error(result.msg);
            }
          });
        },
        that
      );
    },

    /**
     * 绑定微信授权用户
     */
    binduserpage() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.binduser.userId = row.userId;
          that.binduser.weChatUserId = row.weChatUserId;
          that.binduser.title = "微信授权用户-" + row.nickName;
          that.binduser.visible = true;
        },
        that
      );
    },

    /**
     * 解绑
     */
    unbinduser() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (rows) {
          //提示是否删除
          deleteConfirm(
            "是否解除选中授权关联用户",
            function () {
              //加载提示
              that.$loading.show({ text: that.eipMsg.delloading });
              let ids = that.$utils.map(rows, (item) => item.weChatUserId);
              unbind({ id: ids.join(",") }).then((result) => {
                that.$loading.hide();
                if (result.code == that.eipResultCode.success) {
                  that.reload();
                  that.$message.success(result.msg);
                } else {
                  that.$message.error(result.msg);
                }
              });
            },
            that
          );
        },
        that,
        false
      );
    },

    /**
     * 重新加载
     */
    reload() {
      this.table.page.param.current = 1;
      this.tableInit();
    },

    /**
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      this.visible.bind = buttons.filter((f) => f.method == "bind").length > 0;
      this.visible.unbind =
        buttons.filter((f) => f.method == "unbind").length > 0;
    },
  },
};
</script>