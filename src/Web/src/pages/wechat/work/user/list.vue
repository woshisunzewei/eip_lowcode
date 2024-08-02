<template>
  <div>
    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar
        ref="toolbar"
        custom
        print
        export
        :refresh="{ query: tableInit }"
      >
        <template v-slot:buttons>
          <eip-toolbar
            @asyncUser="asyncUser"
            @asyncUserToSystem="asyncUserToSystem"
          ></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table
        :loading="table.loading"
        ref="table"
        id="workflowbuttonlist"
        size="small"
        :height="table.height"
        :export-config="{}"
        :print-config="{}"
        :data="table.data"
      >
        <template #loading>
          <a-spin></a-spin>
        </template>
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column
          field="platformUserId"
          title="企业微信用户Id"
          showOverflow="ellipsis"
          width="140"
        ></vxe-column>
        <vxe-column field="name" title="名称" width="150"> </vxe-column>
        <vxe-column
          field="systemUserId"
          title="绑定用户"
          align="center"
          width="140"
        >
          <template v-slot="{ row }">
            <a-tag
              v-if="!row.systemUserId"
              @click="userBind(row)"
              :color="row.systemUserId ? '#2db7f5' : '#f50'"
            >
              {{ row.systemUserId ? "解绑" : "去绑定" }}
            </a-tag>
            <span class="run-list-user" v-if="row.systemUserId">
              <a-tag @click="userDetail(row)" :closable="false">
                <img class="img" v-real-img-user="row.bindHeadImage" />
                <label style="vertical-align: middle">{{
                  row.bindUserName
                }}</label>
              </a-tag>
            </span>
            <a-tooltip v-if="row.systemUserId" title="解绑">
              <a-button
                type="danger"
                @click="userBind(row)"
                shape="circle"
                icon="delete"
                size="small"
              />
            </a-tooltip>
          </template>
        </vxe-column>
        <vxe-column
          field="organizationNames"
          title="所属部门"
          showOverflow="ellipsis"
          width="180"
        ></vxe-column>
        <vxe-column
          field="createUserName"
          title="创建人"
          width="100"
        ></vxe-column>
        <vxe-column
          field="createTime"
          title="创建时间"
          width="160"
        ></vxe-column>
        <vxe-column
          field="updateUserName"
          title="修改人"
          width="100"
        ></vxe-column>
        <vxe-column
          field="updateTime"
          title="修改时间"
          width="160"
        ></vxe-column>
      </vxe-table>
    </a-card>

    <detail
      ref="detail"
      v-if="detail.visible"
      :visible.sync="detail.visible"
      :userId="detail.userId"
      :title="detail.title"
      :privilegeMaster="privilege.master"
      :privilegeMasterValue="privilege.masterValue"
      @save="operateStatus"
    ></detail>
    <eip-user
      v-if="user.visible"
      :multiple="false"
      :visible.sync="user.visible"
      :chosen="user.chosen"
      title="选择人员"
      @ok="chosenUserOk"
    ></eip-user>
  </div>
</template>

<script>
import {
  query,
  async,
  asyncToSystem,
  bindUserId,
} from "@/services/wechat/work/user/list";
import detail from "@/pages/system/user/detail";

import { operationConfirm } from "@/utils/util";
export default {
  components: { detail },
  data() {
    return {
      user: {
        visible: false,
        chosen: [],
        threeUserId: null,
      },
      detail: {
        visible: false,
        userId: "",
        title: "",
      },
      privilege: {
        master: this.eipPrivilegeMaster.user,
        masterValue: null,
      },
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "",
            filters: "",
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 73 + "px",
        search: {
          option: {
            num: 6,
            config: [
              {
                field: "Name",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "Remark",
                op: "cn",
                placeholder: "请输入备注",
                title: "备注",
                value: "",
                type: "text",
              },
            ],
          },
        },
      },
    };
  },
  created() {
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
     * 用户详情
     */
    userBind(row) {
      let that = this;
      this.user.threeUserId = row.threeUserId;
      if (row.systemUserId) {
        operationConfirm(
          "确定取消绑定",
          function () {
            that.$loading.show({ text: "解绑中..." });

            bindUserId({
              systemUserId: null,
              threeUserId: row.threeUserId,
            }).then((result) => {
              that.$loading.hide();
              that.$message.destroy();
              if (result.code === that.eipResultCode.success) {
                that.$message.success(result.msg);
                that.tableInit();
              } else {
                that.$message.error(result.msg);
              }
            });
          },
          that
        );
      } else {
        this.user.visible = true;
      }
    },
    /**
     * 用户详情
     */
    userDetail(row) {
      this.detail.userId = row.systemUserId;
      this.detail.title = "用户详情-" + row.name;
      this.detail.visible = true;
    },
    /**
     * 选择
     */
    chosenUserOk(data) {
      let that = this;
      //弹出绑定用户
      that.$loading.show({ text: "绑定中..." });
      bindUserId({
        systemUserId: data[0].userId,
        threeUserId: this.user.threeUserId,
      }).then((result) => {
        that.$loading.hide();
        that.$message.destroy();
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.tableInit();
        } else {
          that.$message.error(result.msg);
        }
      });
    },
    /**
     * 同步
     */
    asyncUser() {
      let that = this;
      that.$loading.show({ text: "获取中" });
      async()
        .then((result) => {
          that.$loading.hide();
          that.$message.destroy();
          if (result.code === that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.tableInit();
          } else {
            that.$message.error(result.msg);
          }
        })
        .catch((err) => {
          that.$loading.hide();
          that.$message.destroy();
        });
    },

    /**
     * 同步
     */
    asyncUserToSystem() {
      let that = this;
      operationConfirm(
        "确定同步到系统用户,同步后人员关系发生变化",
        function () {
          that.$loading.show({ text: "同步中" });
          asyncToSystem()
            .then((result) => {
              that.$loading.hide();
              that.$message.destroy();
              if (result.code === that.eipResultCode.success) {
                that.$message.success(result.msg);
                that.tableInit();
              } else {
                that.$message.error(result.msg);
              }
            })
            .catch((err) => {
              that.$loading.hide();
              that.$message.destroy();
            });
        },
        that
      );
    },

    /**
     * 初始化流程按钮数据
     */
    tableInit() {
      let that = this;
      that.table.loading = true;
      query(that.table.page.param).then((result) => {
        if (result.code == that.eipResultCode.success) {
          that.table.data = result.data;
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

    //提示状态信息
    operateStatus(result) {
      if (result.code === this.eipResultCode.success) {
        this.reload();
      }
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
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;
    },
  },
};
</script>

<style lang="less" scoped>
.ant-tag {
  border-radius: 40px !important;
}

.img {
  width: 23px;
  height: 23px;
  border-radius: 50%;
}

/deep/ .run-list-user .ant-tag {
  cursor: pointer;
  padding: 0 7px 0 0 !important;
  border-radius: 50px !important;
}

/deep/ .run-list-user .ant-tag label {
  font-size: 12px;
  margin-left: 4px;
}
</style>