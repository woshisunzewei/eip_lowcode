<template>
  <splitpanes class="default-theme" style="height: auto">
    <pane :min-size="split.minWidth" :size="split.minWidth">
      <a-card @contextDictionary.prevent size="small" class="eip-admin-card-small">
        <div slot="title">
          <a-space>
            <a-input-search v-model="organization.key" :allowClear="true" placeholder="名称/拼音模糊搜索"
              @change="organizationSearch" />
            <a-tooltip title="刷新">
              <a-button type="primary" @click="organizationInit()">
                <a-icon type="redo" />
              </a-button>
            </a-tooltip>
          </a-space>
        </div>
        <a-spin :spinning="organization.spinning" :delay="0" :style="organization.style">
          <div>
            <a-directory-tree v-if="!organization.spinning" :expandAction="false"
              :defaultExpandedKeys="[organization.data.length > 0 ? organization.data[0].key : '']"
              :tree-data="organization.data" :icon="organizationIcon" @select="organizationSelect"></a-directory-tree>
          </div>
        </a-spin>
      </a-card>
    </pane>
    <pane min-size="50">
      <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
        <eip-search :option="table.search.option" @search="tableSearch" @advanced="tableAdvanced"></eip-search>
      </a-card>

      <a-card class="eip-admin-card-small" :bordered="false">
        <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: userInit }">
          <template v-slot:buttons>
            <eip-toolbar @del="del" @update="update" @add="add" @menu="menu" @menuButton="menuButton" @data="data"
              @detail="userDetail" @menu-button-data="menuMenuButtonData" @resetpassword="resetPassword"
              @downTemplate="downTemplate" @dataImport="dataImport" @mobileImport="mobileImport" @onload="toolbarOnload"
              @leader="userLeader" @userExport="userExport"></eip-toolbar>
          </template>
        </vxe-toolbar>
        <vxe-table :loading="table.loading" ref="table" id="systemuserlist" size="small" :seq-config="{
          startIndex:
            (table.page.param.current - 1) * table.page.param.size,
        }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data" :sort-config="{
          trigger: 'cell',
          defaultSort: { field: 'createTime', order: 'asc' },
          orders: ['desc', 'asc'],
        }" @sort-change="tableSort">
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
          <vxe-column field="code" title="登录名" width="120" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="headImage" title="姓名" width="150">
            <template v-slot="{ row }">
              <div class="flex justify-start align-center" v-viewer>
                <img style="width: 32px; height: 32px; border-radius: 50%" v-real-img="row.headImage" />
                <label class="margin-left-xs"> {{ row.name }}</label>
              </div>
            </template>
          </vxe-column>
          <vxe-column field="role" title="角色" width="200" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="parentIdsName" title="归属组织" showOverflow="ellipsis" min-width="200"></vxe-column>
          <vxe-column field="userOrganizationNames" title="关联组织" showOverflow="ellipsis" min-width="200"></vxe-column>
          <vxe-column field="mobile" title="手机号码" width="120"></vxe-column>
          <!-- <vxe-column field="email" title="邮箱" width="160">
            </vxe-column>
            <vxe-column
              field="otherContactInformation"
              title="其他联系信息"
              width="100"
            >
            </vxe-column>-->
          <vxe-column field="lastVisitTime" title="最后登录时间" width="180"></vxe-column>
          <vxe-column field="isFreeze" title="禁用" align="center" width="80" sortable>
            <template v-slot="{ row }">
              <a-switch :checked="row.isFreeze" @change="isFreezeChange(row)" />
            </template>
          </vxe-column>
          <vxe-column field="orderNo" title="排序" align="center" width="80"></vxe-column>
          <vxe-column field="remark" title="备注" min-width="100" showOverflow="ellipsis"></vxe-column>
          <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
          <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
          <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
          <vxe-column field="updateTime" title="修改时间" width="160"></vxe-column>
          <vxe-column title="操作" align="center" fixed="right" width="100px">
            <template #default="{ row }">
              <a-tooltip @click="tableUpdateRow(row)" title="编辑" v-if="visible.update">
                <label class="text-eip eip-cursor-pointer">编辑</label>
              </a-tooltip>
              <a-divider type="vertical" v-if="visible.update" />
              <a-tooltip title="删除" v-if="visible.del" @click="tableDelRow(row)">
                <label class="text-red eip-cursor-pointer">删除</label>
              </a-tooltip>
            </template>
          </vxe-column>
        </vxe-table>

        <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
          show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
          :page-size="table.page.param.size" :total="table.page.total" @change="userInit"
          @showSizeChange="tableSizeChange"></a-pagination>
      </a-card>
    </pane>

    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :data="organization.data" :title="edit.title"
      :userId="edit.userId" @save="operateStatus"></edit>

    <detail ref="detail" v-if="detail.visible" :visible.sync="detail.visible" :userId="detail.userId"
      :title="detail.title" :privilegeMaster="privilege.master" :privilegeMasterValue="privilege.masterValue"
      @save="operateStatus"></detail>

    <eip-permission-menu ref="permissionMenu" v-if="permission.menu.visible" :visible.sync="permission.menu.visible"
      :title="permission.menu.title" :privilegeMaster="privilege.master"
      :privilegeMasterValue="privilege.masterValue"></eip-permission-menu>

    <eip-permission-menu-button ref="permissionMenuButton" v-if="permission.menuButton.visible"
      :visible.sync="permission.menuButton.visible" :title="permission.menuButton.title"
      :privilegeMaster="privilege.master" :privilegeMasterValue="privilege.masterValue"></eip-permission-menu-button>

    <eip-permission-data ref="permissionData" v-if="permission.data.visible" :visible.sync="permission.data.visible"
      :title="permission.data.title" :privilegeMaster="privilege.master"
      :privilegeMasterValue="privilege.masterValue"></eip-permission-data>

    <eip-permission-menu-menuButton-data ref="permissionMenuMenuButtonData" v-if="permission.menuMenuButtonData.visible"
      :visible.sync="permission.menuMenuButtonData.visible" :title="permission.menuMenuButtonData.title"
      :privilegeMaster="privilege.master"
      :privilegeMasterValue="privilege.masterValue"></eip-permission-menu-menuButton-data>

    <a-modal v-drag :destroyOnClose="true" :maskClosable="false" v-if="toolbar.resetPassword.visible"
      :visible="toolbar.resetPassword.visible" :title="toolbar.resetPassword.title"
      @cancel="toolbar.resetPassword.visible = false" @ok="resetPasswordSave">

      <a-input v-model="toolbar.resetPassword.value" placeholder="请输入重置密码值"></a-input>

      <a-alert type="warning" style="margin-top:18px">
        <template slot="description">
          密码规则:<br>
          <div v-for="(item, index) in toolbar.resetPassword.rule" :key="index">
            {{ index + 1 }}, {{ item }}
          </div>
        </template>
      </a-alert>
      <template slot="footer">
        <a-button @click="toolbar.resetPassword.visible = false"><a-icon type="close" />取消</a-button>
        <a-button key="submit" type="primary" :loading="toolbar.resetPassword.loading"
          @click="resetPasswordSave"><a-icon type="save" />提交</a-button>
      </template>
    </a-modal>
    <a-modal @cancel="upload.visible = false" title="导入错误" :visible="upload.visible" :footer="null" v-drag>
      <div v-html="upload.msg"></div>
    </a-modal>

    <leader ref="leader" v-if="leader.visible" :visible.sync="leader.visible" :data="organization.data"
      :title="leader.title" :userId="leader.userId" @save="perateStatusLeader"></leader>

  </splitpanes>
</template>

<script>
import Vue from 'vue'
import Viewer from 'v-viewer'
import 'viewerjs/dist/viewer.css'
Vue.use(Viewer)
Viewer.setDefaults({
  Options: {
    'inline': true, // 启用 inline 模式
    'button': true, // 显示右上角关闭按钮
    'navbar': true, // 显示缩略图导航
    'title': true, // 显示当前图片的标题
    'toolbar': true, // 显示工具栏
    'tooltip': true, // 显示缩放百分比
    'movable': true, // 图片是否可移动
    'zoomable': true, // 图片是否可缩放
    //'rotatable': true, // 图片是否可旋转
    //'scalable': true, // 图片是否可翻转
    'transition': true, // 使用 CSS3 过度
    //'fullscreen': true, // 播放时是否全屏
    'keyboard': true, // 是否支持键盘
    'url': 'data-source' // 设置大图片的 url
  }
})
import {
  organizationQuery,
  query,
  del,
  resetPassword,
  isFreeze,
  downImportTemplate,
  systemUserImport,
  systemUserExport,
  systemUserImportMobile
} from "@/services/system/user/list";
import edit from "./edit";
import leader from "./leader";
import detail from "./detail";

import { selectTableRow, deleteConfirm } from "@/utils/util";

import eipPermissionMenu from "@/pages/system/permission/menu";
import eipPermissionMenuButton from "@/pages/system/permission/menubutton";
import eipPermissionData from "@/pages/system/permission/data";
import eipPermissionMenuMenuButtonData from "@/pages/system/permission/menu-menubutton-data";

import { header } from "@/services/system/config/index";
import { Splitpanes, Pane } from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'
export default {
  components: {
    edit,
    detail,
    leader,
    eipPermissionMenu,
    eipPermissionMenuButton,
    eipPermissionData,
    eipPermissionMenuMenuButtonData,
    Splitpanes,
    Pane
  },
  data() {
    return {
      split: {
        minWidth: (250 / window.innerWidth) * 100,
      },
      organization: {
        style: {
          overflow: "auto",
          height: this.eipHeaderHeight() - 72 + "px",
        },
        key: null,
        original: [],
        data: [],
        spinning: true,
        right: {
          item: null,
          style: "",
        },
      },
      upload: {
        file: null,
        visible: false,
        msg: "",
      },
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "desc",
            sidx: "userInfo.CreateTime",
            filters: "",
            privilegeMasterValue: null,
            privilegeMaster: this.eipPrivilegeMaster.organization,
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: this.eipHeaderHeight(),

        search: {
          option: {
            num: 3,
            config: [
              {
                field: "userInfo.Code",
                op: "cn",
                placeholder: "请输入登录名",
                title: "登录名",
                value: "",
                type: "text",
              },
              {
                field: "userInfo.Name",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "userInfo.Mobile",
                op: "cn",
                placeholder: "请输入手机号",
                title: "手机号",
                value: "",
                type: "text",
              },
              {
                field: "userInfo.Email",
                op: "cn",
                placeholder: "请输入邮箱",
                title: "邮箱",
                value: "",
                type: "text",
              },
              {
                field: "userInfo.IsFreeze",
                op: "eq",
                placeholder: "请选择禁用状态",
                title: "禁用",
                type: "bool",
                options: {
                  yes: '是',
                  no: '否'
                }
              },
            ],
          },
        },
      },

      edit: {
        visible: false,
        userId: "",
        title: "",
        data: [],
      },
      leader: {
        visible: false,
        userId: "",
        title: "",
        data: [],
      },
      detail: {
        visible: false,
        userId: "",
        title: "",
      },
      visible: {
        update: false,
        del: false,
      },

      permission: {
        menu: {
          visible: false,
          title: "",
        },

        menuButton: {
          visible: false,
          title: "",
        },

        data: {
          visible: false,
          title: "",
        },

        menuMenuButtonData: {
          visible: false,
          title: "",
        },
      },
      privilege: {
        master: this.eipPrivilegeMaster.user,
        masterValue: null,
      },
      toolbar: {
        resetPassword: {
          loading: false,
          visible: false,
          title: null,
          value: "",
          id: "",
          rule: []
        },
      },
    };
  },
  created() {
    this.organizationInit();
    this.userInit();
    this.initSystemConfig()
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    resize(e) { },
    /**
     * 
     * @param {*} e 
     */
    organizationSearch(e) {
      var that = this;
      this.organization.data = that.$utils.clone(this.organization.original, true);
      this.organization.data = that.$utils.searchTree(
        this.organization.data,
        (item) => {
          if (that.organization.key) {
            var titlePinyin = pinyin.convert(item.title).toLowerCase();
            if (
              item.title
                .toLowerCase()
                .includes(that.organization.key.toLowerCase())
            ) {
              return true;
            } else if (
              titlePinyin.includes(that.organization.key.toLowerCase())
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
     *冻结
     */
    isFreezeChange(row) {
      if (this.visible.update) {
        let that = this;
        isFreeze({ id: row.userId }).then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.reload(false);
            that.$message.success(result.msg);
          } else {
            that.$message.error(result.msg);
          }
        });
      }
    },
    /**
     * 菜单树
     */
    organizationInit() {
      let that = this;
      that.organization.spinning = true;
      organizationQuery().then((result) => {
        that.organization.data = result.data;
        that.organization.original = result.data;
        that.organization.spinning = false;
        that.organizationSearch();
      });
    },

    /**
     * 树图标
     */
    organizationIcon(props) {
      const { expanded } = props;
      if (props.children.length == 0) {
        return <a-icon type="file" />;
      }
      return <a-icon type={expanded ? "folder-open" : "folder"} />;
    },

    /**
     * 树选中
     */
    organizationSelect(electedKeys, e) {
      this.table.page.param.privilegeMasterValue = electedKeys[0];
      this.table.page.param.current = 1;
      this.userInit();
    },

    /**
     * 列表数据
     */
    userInit() {
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
     * 列表排序改变
     */
    tableSort({ sortList }) {
      this.table.page.param.current = 1;
      this.table.page.param.sidx = sortList[0].property;
      this.table.page.param.sord = sortList[0].order;
      this.userInit();
    },

    /**
     *数量改变
     */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.userInit();
    },
    /**
     * 列表查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.userInit();
    },
    /**
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      this.visible.add = buttons.filter((f) => f.method == "add").length > 0;
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;
    },

    /**
     * 树更新
     */
    tableUpdateRow(row) {
      this.edit.userId = row.userId;
      this.edit.title = "用户编辑-" + row.name;
      this.edit.visible = true;
    },

    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "用户【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.userId }).then((result) => {
            that.$loading.hide();
            if (result.code == that.eipResultCode.success) {
              that.reload(false);
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
     * 新增
     */
    add() {
      this.edit.title = "新增用户";
      this.edit.userId = null;
      this.edit.visible = true;
    },

    /**
     * 修改
     */
    update() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.userId = row.userId;
          that.edit.title = "编辑用户-" + row.name;
          that.edit.visible = true;
        },
        that
      );
    },

    /**
     * 设置领导
     */
    userLeader() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.leader.userId = row.userId;
          that.leader.title = "领导设置-" + row.name;
          that.leader.visible = true;
        },
        that
      );
    },
    /**
     * 保存领导
     */
    perateStatusLeader() { },

    /**
     * 用户详情
     */
    userDetail() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.detail.userId = row.userId;
          that.detail.title = "用户详情-" + row.name;
          that.detail.visible = true;
        },
        that
      );
    },

    /**
     * 删除
     */
    del() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (rows) {
          //提示是否删除
          deleteConfirm(
            that.eipMsg.delete,
            function () {
              //加载提示
              that.$loading.show({ text: that.eipMsg.delloading });
              let ids = that.$utils.map(rows, (item) => item.userId);
              del({ id: ids.join(",") }).then((result) => {
                that.$loading.hide();
                if (result.code == that.eipResultCode.success) {
                  that.reload(false);
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
     * 提示状态信息
     * @param {*} result 
     */
    operateStatus(result) {
      if (result.code === this.eipResultCode.success) {
        this.reload(false);
      }
    },

    /**
     * 下载导入模版
     */
    downTemplate() {
      downImportTemplate();
    },

    /**
     *导入
     */
    dataImport(file) {
      let that = this;
      const formData = new FormData();
      formData.append("Files", file.file);
      that.$message.loading({
        content: "导入中...",
        duration: 0,
      });
      systemUserImport(formData)
        .then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.reload();
          } else {
            var msg = [];
            try {
              var datas = JSON.parse(result.msg);
              datas.forEach((item) => {
                var m = "";
                m += "第" + item.RowIndex + "行，";
                for (let key in item.FieldErrors) {
                  m += item.FieldErrors[key] + ",";
                }
                msg.push(m);
              });
              that.upload.msg = msg.join("<br/>");
              that.upload.visible = true;
            } catch (error) {
              result.data.forEach((item) => {
                msg.push(item);
              });
              that.upload.msg = msg.join("<br/>");
              that.upload.visible = true;
            }
          }
        })
        .catch(() => {
          that.$message.destroy();
          that.$message.error("上传出错");
        });
    },

    /**
     * 用户导出
     */
    userExport() {
      let that = this;
      systemUserExport(that.table.page.param).then((result) => {
        that.$message.success("导出成功");
      });
    },

    /**
     *导入
     */
    mobileImport(file) {
      let that = this;
      const formData = new FormData();
      formData.append("Files", file.file);
      that.$message.loading({
        content: "导入中...",
        duration: 0,
      });
      systemUserImportMobile(formData)
        .then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.reload();
          } else {
            var msg = [];
            try {
              var datas = JSON.parse(result.msg);
              datas.forEach((item) => {
                var m = "";
                m += "第" + item.RowIndex + "行，";
                for (let key in item.FieldErrors) {
                  m += item.FieldErrors[key] + ",";
                }
                msg.push(m);
              });
              that.upload.msg = msg.join("<br/>");
              that.upload.visible = true;
            } catch (error) {
              result.data.forEach((item) => {
                msg.push(item);
              });
              that.upload.msg = msg.join("<br/>");
              that.upload.visible = true;
            }
          }
        })
        .catch(() => {
          that.$message.error("上传出错");
        });
    },

    /**
     * 重新加载
     */
    reload(reset) {
      if (reset) this.table.page.param.id = this.eipEmptyGuid;
      this.table.page.param.current = 1;
      this.userInit();
    },

    /**
     * 调整,收起展开高度
     */
    tableAdvanced(advanced) {
      this.table.height = this.eipHeaderHeight() - (advanced ? 203 : 163);
    },

    /**
     * 菜单权限
     */
    menu() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.userId;
          that.permission.menu.visible = true;
          that.permission.menu.title = "人员模块权限-" + row.name;
        },
        that
      );
    },

    /**
     * 人员按钮
     */
    menuButton() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.userId;
          that.permission.menuButton.visible = true;
          that.permission.menuButton.title = "人员按钮权限-" + row.name;
        },
        that
      );
    },

    menuButtonData() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.userId;
          that.permission.menuButton.visible = true;
          that.permission.menuButton.title = "人员按钮权限-" + row.name;
        },
        that
      );
    },

    /**
     * 人员菜单,按钮,数据权限
     */
    menuMenuButtonData() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.userId;
          that.permission.menuMenuButtonData.visible = true;
          that.permission.menuMenuButtonData.title = "人员权限-" + row.name;
        },
        that
      );
    },

    /**
     * 数据权限
     */
    data() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.privilege.masterValue = row.userId;
          that.permission.data.visible = true;
          that.permission.data.title = "人员数据权限-" + row.name;
        },
        that
      );
    },

    /**
     * 重置密码
     */
    resetPassword() {
      let that = this;
      that.toolbar.resetPassword.value = "";
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.toolbar.resetPassword.visible = true;
          that.toolbar.resetPassword.title = "重置密码-" + row.name;
          that.toolbar.resetPassword.id = row.userId;
        },
        that
      );
    },
    /**
       * 获取系统配置
       */
    initSystemConfig() {
      let that = this;
      header().then(result => {
        if (result.code === this.eipResultCode.success) {
          let data = result.data;
          //是否有时间
          let rule = [];
          if (data.systemPasswordLength) {
            rule.push("密码长度需大于等于" + data.systemPasswordLength + "个字符串")
          }

          if (data.systemPasswordHaveNumber && data.systemPasswordHaveNumber == 'true') {
            rule.push("必须包含至少" + data.systemPasswordHaveNumberLength + "个数字")
          }

          if (data.systemPasswordHaveLetters && data.systemPasswordHaveLetters == 'true') {
            rule.push("必须包含至少" + data.systemPasswordHaveLettersLength + "个字母")
          }

          if (data.systemPasswordHaveSpecialCharacters && data.systemPasswordHaveSpecialCharacters == 'true') {
            rule.push("必须包含至少" + data.systemPasswordHaveSpecialCharactersLength + "个特殊字符")
          }
          that.toolbar.resetPassword.rule = rule
        }
      });
    },
    /**
     * 重置密码保存
     */
    resetPasswordSave() {
      let that = this;
      if (that.toolbar.resetPassword.value == "") {
        that.$message.error("请输入密码值");
        return false;
      }

      that.$message.loading("正在重置密码", 0);
      let param = {
        id: that.toolbar.resetPassword.id,
        encryptPassword: that.toolbar.resetPassword.value,
      };

      resetPassword(param).then((result) => {
        that.$message.destroy();
        if (result.code == that.eipResultCode.success) {
          that.toolbar.resetPassword.visible = false;
          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
      });
    },
  },
};
</script>

<style lang="less" scoped>
.eip-admin-card-small .ant-form-item {
  margin-bottom: 0;
}

.ant-card-body {
  padding: 10px !important;
}
</style>
