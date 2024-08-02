<template>
  <div class="getSingleData">
    <a-form-model :layout="form.layout">
      <a-form-model-item label="通知内容">
        <div class="flex justify-start">
          <eip-editor
            class="flex-sub"
            ref="noticeSite"
            v-if="tinymce.show"
            v-model="drawerData.content"
            :height="tinymce.height"
            :toolbar="tinymce.toolbar"
            :plugins="tinymce.plugins"
            :statusbar="tinymce.statusbar"
            content_style="p{margin:5px} body{margin:4px 0 0 4px}"
            :menubar="tinymce.menubar"
          ></eip-editor>
          <a-popover placement="bottomLeft">
            <div slot="content" style="width: 600px">
              <a-input-search
                placeholder="搜索流程节点对象下的字段"
                style="width: 100%; padding-bottom: 4px"
              />
              <a-collapse
                expand-icon-position="right"
                style="max-height: 400px; overflow: auto"
              >
                <a-collapse-panel
                  v-for="(item, index) in configList"
                  :key="index"
                  :header="item.title"
                >
                  <ul class="conditionFieldBox show">
                    <li
                      @click="columnClick(item, itemc)"
                      v-for="itemc in item.data.columns"
                      :key="itemc.model"
                      class="flexRow ThemeHoverBGColor3"
                    >
                      <div class="ellipsis">
                        <span class="field">[{{ itemc.type }}]</span
                        ><span title="自动编号">{{ itemc.label }}</span>
                      </div>
                    </li>
                  </ul>
                  <span slot="extra" class="Gray_9e">
                    {{
                      item.type == "0"
                        ? "工作表“" + item.data.tableName + "”"
                        : ""
                    }}
                  </span>
                </a-collapse-panel>
              </a-collapse>
            </div>
            <a-button
              type="primary"
              icon="diff"
              class="margin-left-xs"
            ></a-button>
          </a-popover>
        </div>
      </a-form-model-item>

      <a-form-model-item label="通知人类型" prop="name">
        <a-select
          :default-value="0"
          v-model="drawerData.userType"
          @change="typeChange"
        >
          <a-select-option
            v-for="(item, index) in type"
            :key="index"
            :value="item.key"
            >{{ item.value }}</a-select-option
          >
        </a-select>
      </a-form-model-item>
      <a-form-model-item
        label="类型值选择"
        v-if="
          drawerData.userType != undefined &&
          drawerData.userType != 0 &&
          drawerData.userType != 12 &&
          drawerData.userType != 22 &&
          drawerData.userType != 24 &&
          drawerData.userType != 26 &&
          drawerData.userType != 100
        "
      >
        <a-input
          v-model="drawerData.rangeTxt"
          placeholder="请选择类型值"
          disabled
        >
          <a-icon
            @click="rangeClick"
            style="cursor: pointer"
            slot="addonAfter"
            type="search"
          />
        </a-input>
      </a-form-model-item>
    </a-form-model>

    <eip-organization
      v-if="range.organization.visible"
      :visible.sync="range.organization.visible"
      :chosen="form.range"
      :range="0"
      title="选择组织架构"
      @ok="chosenOrganizationOk"
    ></eip-organization>

    <eip-user
      v-if="range.user.visible"
      :visible.sync="range.user.visible"
      :topOrg="range.user.topOrg"
      :chosen="form.range"
      title="选择人员"
      @ok="chosenUserOk"
    ></eip-user>

    <eip-role
      v-if="range.role.visible"
      :visible.sync="range.role.visible"
      :topOrg="range.role.topOrg"
      :chosen="form.range"
      title="选择角色"
      @ok="chosenRoleOk"
    ></eip-role>

    <eip-post
      v-if="range.post.visible"
      :visible.sync="range.post.visible"
      :topOrg="range.post.topOrg"
      :chosen="form.range"
      title="选择岗位"
      @ok="chosenPostOk"
    ></eip-post>

    <eip-group
      v-if="range.group.visible"
      :visible.sync="range.group.visible"
      :topOrg="range.group.topOrg"
      :chosen="form.range"
      title="选择组"
      @ok="chosenGroupOk"
    ></eip-group>

    <eip-column
      v-if="range.column.visible"
      :visible.sync="range.column.visible"
      :data="column"
      :chosen="form.range"
      title="选择字段"
      @ok="chosenColumnOk"
    ></eip-column>

    <eip-sql
      v-if="range.sql.visible"
      :visible.sync="range.sql.visible"
      :column="column"
      :chosen="form.range"
      title="自定义Sql"
      @ok="chosenSqlOk"
    ></eip-sql>
  </div>
</template>

<script>
export default {
  name: "noticeSite",
  components: {},
  props: {
    tables: {
      required: true,
      type: Array,
      default: () => {
        return [];
      },
    },
    configList: {
      required: true,
      type: Array,
      default: () => {
        return [];
      },
    },
    drawerData: {
      required: true,
      type: Object,
    },
  },

  data() {
    return {
      form: {
        layout: "horizontal",
      },
      range: {
        //范围选择组织架构
        organization: {
          visible: false,
        },
        //范围选择人员
        user: {
          visible: false,
          topOrg: "",
        },
        //范围选择角色
        role: {
          visible: false,
          topOrg: "",
        },
        //范围选择岗位
        post: {
          visible: false,
          topOrg: "",
        },
        //范围选择组
        group: {
          visible: false,
          topOrg: "",
        },
        column: {
          visible: false,
        },
        //自定义Sql
        sql: {
          visible: false,
        },
      },
      tinymce: {
        plugins: "preview fullscreen code",
        toolbar: "",
        height: 150,
        statusbar: false,
        show: false,
        menubar: "",
      },
      type: [
        {
          key: 0,
          value: "所有成员",
        },
        {
          key: 2,
          value: "组织",
        },
        {
          key: 4,
          value: "人员",
        },
        {
          key: 6,
          value: "角色",
        },
        {
          key: 8,
          value: "岗位",
        },
        {
          key: 10,
          value: "组",
        },
        {
          key: 22,
          value: "发起者",
        },
        {
          key: 24,
          value: "发起者直线领导",
        },
        {
          key: 28,
          value: "表单人员",
        },
        {
          key: 90,
          value: "自定义",
        },
        {
          key: 100,
          value: "程序指定",
        },
      ],
    };
  },

  mounted() {
    this.tinymce.show = true;
    this.init();
  },
  methods: {
    init() {
      let that = this;
      this.configList.forEach((f) => {
        if (f.type == 0) {
          var table = that.$utils.find(
            that.tables,
            (item) => item.configId == f.data.table
          );
          f.data.tableName = table.name;
          f.data.columns = JSON.parse(table.columnJson);
        }
      });
    },

    /**
     * 选择
     * @param {*} item
     * @param {*} itemc
     */
    columnClick(item, itemc) {
      let html =
        "<span id='" +
        encodeURIComponent(
          JSON.stringify({
            id: item.id,
            model: itemc.model,
          })
        ) +
        "' class='non-editable'>" +
        itemc.label +
        "</span>";
      this.$refs.noticeSite.insert(html);
      this.visible = false;
    },

    /**
     * 处理人类型改变
     */
    typeChange() {
      this.drawerData.rangeTxt = "";
      this.drawerData.name = "";
      switch (this.drawerData.userType) {
        case 0:
        case 22:
        case 24:
        case 26:
        case 90:
        case 100:
          this.drawerData.name = this.type.filter(
            (f) => f.key == this.drawerData.userType
          )[0].value;
          break;
      }
    },

    /**
     *范围点击
     */
    rangeClick() {
      switch (this.drawerData.userType) {
        case 2: //组织
          this.range.organization.visible = true;
          break;
        case 4: //人员
          this.range.user.visible = true;
          break;
        case 6: //角色
          this.range.role.visible = true;
          break;
        case 8: //岗位
          this.range.post.visible = true;
          break;
        case 10: //组
          this.range.group.visible = true;
          break;
        case 28: //表单人员
          this.range.column.visible = true;
          break;
        case 90: //自定义Sql
          this.range.sql.visible = true;
          break;
        default:
      }
    },

    /**
     * 选择组织架构成功
     */
    chosenOrganizationOk(organizations) {
      var value = this.$utils
        .map(organizations, (item) => item.title)
        .join(",");
      let that = this;
      that.drawerData.range = [];
      that.drawerData.rangeTxt = value;
      that.drawerData.name = value;
      this.$utils.each(organizations, (item, key) => {
        that.drawerData.range.push({
          id: item.key,
          name: item.title,
        });
      });
    },

    /**
     * 选择人员成功
     */
    chosenUserOk(users) {
      var value = this.$utils.map(users, (item) => item.name).join(",");
      let that = this;
      that.drawerData.range = [];
      that.drawerData.rangeTxt = value;
      that.drawerData.name = value;
      this.$utils.each(users, (item, key) => {
        that.drawerData.range.push({
          id: item.userId,
          name: item.name,
        });
      });
    },

    /**
     * 选择角色成功
     */
    chosenRoleOk(roles) {
      var value = this.$utils.map(roles, (item) => item.name).join(",");
      let that = this;
      that.drawerData.range = [];
      that.drawerData.rangeTxt = value;
      that.drawerData.name = value;
      this.$utils.each(roles, (item, key) => {
        that.drawerData.range.push({
          id: item.roleId,
          name: item.name,
        });
      });
    },

    /**
     *
     */
    chosenGroupOk(groups) {
      var value = this.$utils.map(groups, (item) => item.name).join(",");
      let that = this;
      that.drawerData.range = [];
      that.drawerData.rangeTxt = value;
      that.drawerData.name = value;
      this.$utils.each(groups, (item, key) => {
        that.drawerData.range.push({
          id: item.groupId,
          name: item.name,
        });
      });
    },

    /**
     *
     */
    chosenPostOk(posts) {
      var value = this.$utils.map(posts, (item) => item.name).join(",");
      let that = this;
      that.drawerData.range = [];
      that.drawerData.rangeTxt = value;
      that.drawerData.name = value;
      this.$utils.each(posts, (item, key) => {
        that.drawerData.range.push({
          id: item.postId,
          name: item.name,
        });
      });
    },

    /**
     *
     */
    chosenColumnOk(columns) {
      var value = this.$utils.map(columns, (item) => item.title).join(",");
      let that = this;
      that.drawerData.range = [];
      that.drawerData.rangeTxt = value;
      that.drawerData.name = value;
      this.$utils.each(columns, (item, key) => {
        that.drawerData.range.push({
          id: item.key,
          name: item.title,
        });
      });
    },

    /**
     *
     */
    chosenSqlOk(sql) {
      let that = this;
      that.drawerData.range = [];
      that.drawerData.rangeTxt = sql.contentTxt;
      that.drawerData.name = "自定义";
      that.drawerData.range.push({
        id: sql.contentTxt,
        name: "自定义",
      });
    },
  },
};
</script>

<style scoped>
.conditionFieldBox {
  background-color: #f5f5f5;
  display: none;
  height: 0;
  overflow: hidden;
  transition: all 20s ease-in-out;
}

.conditionFieldBox.show {
  display: block;
  height: 100%;
}

.conditionFieldBox li {
  align-items: center;
  cursor: pointer;
  height: 36px;
  padding: 0 16px 0 39px;
}

.conditionFieldBox li:not(:hover) {
  background-color: transparent !important;
}

.conditionFieldBox li .field {
  color: #8f8f8f;
  margin-right: 5px;
}

.conditionFieldBox li:not(.conditionFieldNull):hover span {
  color: #fff;
}

.ThemeHoverBGColor3:hover {
  background-color: #1e88e5 !important;
}

.ellipsis {
  overflow: hidden;
  text-overflow: ellipsis;
  vertical-align: top;
  white-space: nowrap;
}

.Gray_9e {
  color: #9e9e9e !important;
}

.flexRow {
  display: flex;
  min-width: 0;
}

/deep/ .ant-collapse-content > .ant-collapse-content-box {
  padding: 0 !important;
}

/deep/
  .ant-collapse-icon-position-right
  > .ant-collapse-item
  > .ant-collapse-header {
  padding: 6px 36px 6px 14px !important;
}

/deep/.ant-form-item-label > label {
  font-weight: bold;
}
</style>