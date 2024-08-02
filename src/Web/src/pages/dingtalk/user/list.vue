<template>
  <div style="width: 100%">
    <div style="width: 250px; float: left">
      <a-card
        @contextDictionary.prevent
        size="small"
        class="eip-admin-card-small"
      >
        <div slot="title">
          <a-space>
            <a-input-search
              v-model="department.key"
              :allowClear="true"
              placeholder="名称/拼音模糊搜索"
              @change="departmentSearch"
            />
            <a-tooltip title="刷新">
              <a-button type="primary" @click="init">
                <a-icon type="redo" />
              </a-button>
            </a-tooltip>
          </a-space>
        </div>
        <a-spin
          :spinning="department.spinning"
          :delay="0"
          :style="department.style"
        >
          <div>
            <a-directory-tree
              v-if="!department.spinning"
              :expandAction="false"
              :defaultExpandedKeys="[
                department.data.length > 0 ? department.data[0].key : '',
              ]"
              :tree-data="department.data"
              :icon="icon"
              @select="select"
            ></a-directory-tree>
          </div>
        </a-spin>
      </a-card>
    </div>
    <div class="eip-admin-card-tree-left">
      <a-card
        class="eip-admin-card-small eip-admin-card-small-bottom-border"
        :bordered="false"
      >
        <eip-search
          :option="table.search.option"
          @search="tableSearch"
        ></eip-search>
      </a-card>
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
              @bind="bindUser"
              @chosenOrganizationTransfer="chosenOrganizationTransfer"
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
          v-viewer
        >
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
          <vxe-column
            field="platformUserId"
            title="钉钉用户Id"
            showOverflow="ellipsis"
            width="100"
          ></vxe-column>
          <vxe-column field="name" title="名称" width="150"> </vxe-column>
          <vxe-column
            field="unionId"
            title="UnionId"
            width="100"
            showOverflow="ellipsis"
          ></vxe-column>
          <vxe-column field="avatar" title="头像" align="center" width="80">
            <template v-slot="{ row }">
              <img
                v-if="row.avatar"
                style="width: 32px; height: 32px; border-radius: 50%"
                :src="row.avatar"
              />
            </template>
          </vxe-column>
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
            field="stateCode"
            title="国际电话区号"
            showOverflow="ellipsis"
            width="120"
          ></vxe-column>
          <vxe-column field="mobile" title="电话号码" width="120"></vxe-column>
          <vxe-column
            field="jobNumber"
            title="员工工号"
            width="100"
          ></vxe-column>
          <vxe-column field="title" title="职位" width="100"></vxe-column>
          <vxe-column field="remark" title="邮箱" width="100"></vxe-column>
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
        <a-pagination
          class="padding-top-sm float-right"
          v-model="table.page.param.current"
          show-size-changer
          show-quick-jumper
          :page-size-options="table.page.sizeOptions"
          :show-total="(total) => `共 ${total} 条`"
          :page-size="table.page.param.size"
          :total="table.page.total"
          @change="tableInit"
          @showSizeChange="tableSizeChange"
        ></a-pagination>
      </a-card>
    </div>
    <eip-organization
      v-if="options.visible"
      :visible.sync="options.visible"
      :multiple="false"
      :range="0"
      title="选择组织架构"
      @ok="chosenOrganizationOk"
    ></eip-organization>
    <detail
      ref="detail"
      v-if="detail.visible"
      :visible.sync="detail.visible"
      :userId="detail.userId"
      :title="detail.title"
      :privilegeMaster="privilege.master"
      :privilegeMasterValue="privilege.masterValue"
      @save="operateStatus"
    >
    </detail>

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
import Viewer from "v-viewer";
import Vue from "vue";
import "viewerjs/dist/viewer.css";

Vue.use(Viewer);
Viewer.setDefaults({
  Options: {
    inline: true, // 启用 inline 模式
    button: true, // 显示右上角关闭按钮
    navbar: true, // 显示缩略图导航
    title: true, // 显示当前图片的标题
    toolbar: true, // 显示工具栏
    tooltip: true, // 显示缩放百分比
    movable: true, // 图片是否可移动
    zoomable: true, // 图片是否可缩放
    //'rotatable': true, // 图片是否可旋转
    //'scalable': true, // 图片是否可翻转
    transition: true, // 使用 CSS3 过度
    //'fullscreen': true, // 播放时是否全屏
    keyboard: true, // 是否支持键盘
    url: "data-source", // 设置大图片的 url
  },
});
import {
  query,
  async,
  tree,
  bind,
  bindUserId,
  asyncToSystem,
  transfer,
} from "@/services/dingtalk/user/list";

import detail from "@/pages/system/user/detail";

import { selectTableRow, operationConfirm } from "@/utils/util";
export default {
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
      options: {
        visible: false,
        userIds: [],
      },
      department: {
        key: "",
        style: {
          overflow: "auto",
          height: this.eipHeaderHeight() - 72 + "px",
        },
        data: [],
        original: [],
        spinning: true,
      },
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "userinfo_three.Id",
            filters: "",
            id: "",
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 162 + "px",
        search: {
          option: {
            num: 6,
            config: [
              {
                field: "userinfo_three.Name",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "userinfo_three.Mobile",
                op: "cn",
                placeholder: "请输入电话",
                title: "备注",
                value: "",
                type: "text",
              },

              {
                field: "userinfo_three.DeptNameList",
                op: "cn",
                placeholder: "请输入部门名称",
                title: "部门",
                value: "",
                type: "text",
              },
            ],
          },
        },
      },
    };
  },
  components: { detail },
  created() {
    this.init();
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
     * 初始化数据
     */
    init() {
      this.table.page.param.id = "";
      this.table.page.param.current = 1;
      this.tableInit();
      this.initTree();
    },
    /**
     * 树选中
     */
    select(electedKeys, e) {
      this.table.page.param.id = electedKeys[0];
      this.table.page.param.current = 1;
      this.tableInit();
    },
    /**
     * 树图标
     */
    icon(props) {
      const { expanded } = props;
      if (props.children.length == 0) {
        return <a-icon type="file" />;
      }
      return <a-icon type={expanded ? "folder-open" : "folder"} />;
    },

    /**
     * 查询
     * @param {*} e
     */
    departmentSearch(e) {
      var that = this;
      this.department.data = this.$utils.clone(this.department.original, true);
      this.department.data = this.$utils.searchTree(
        this.department.data,
        (item) => {
          if (that.department.key) {
            var titlePinyin = pinyin.convert(item.title).toLowerCase();
            if (
              item.title
                .toLowerCase()
                .indexOf(that.department.key.toLowerCase()) > -1
            ) {
              return true;
            } else if (
              titlePinyin.indexOf(that.department.key.toLowerCase()) > -1
            ) {
              return true;
            } else {
              return false;
            }
          } else {
            return true;
          }
        },
        { children: "children" }
      );
    },
    /**
     *
     */
    initTree() {
      let that = this;
      that.department.spinning = true;
      tree().then((result) => {
        that.department.original = result.data;
        that.department.data = result.data;
        that.department.spinning = false;
        that.departmentSearch();
      });
    },
    /**
     * 选择
     */
    chosenOrganizationTransfer() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (rows) {
          that.options.visible = true;
          that.options.userIds = rows.map((m) => m.userId).join(",");
        },
        that,
        false
      );
    },

    /**
     * 选择
     */
    chosenOrganizationOk(data) {
      let that = this;
      if (data.length != 0) {
        that.$loading.show({ text: "转移中" });
        transfer({
          userIds: this.options.userIds,
          organizationId: data.map((m) => m.key).join(","),
        })
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
      }
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
          asyncToSystem().then((result) => {
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
    },

    /**
     * 已导入用户数据,根据名称绑定钉钉用户
     */
    bindUser() {
      let that = this;
      operationConfirm(
        "确定根据名称绑定钉钉用户,请确保用户已在【系统用户】中进行导入",
        function () {
          that.$message.destroy();
          that.$loading.show({ text: "绑定中..." });
          bind().then((result) => {
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

    /**
     * 列表查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
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