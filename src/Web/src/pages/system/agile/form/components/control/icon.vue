<template>
  <div>
    <a-dropdown :trigger="trigger" v-model="visible">
      <div class="eip-icon-picker ant-dropdown-trigger">
        <span class="ant-input-affix-wrapper">
          <span class="ant-input-prefix" v-if="icon.name">
            <a-icon slot="prefix" :type="icon.name" :theme="icon.theme" /></span>
          <input placeholder="请选择菜单图标" readonly="" v-model="icon.name" type="text" :size="record.options.size"
            style="cursor: pointer" class="ant-input" />
          <span class="ant-input-suffix">
            <div class="eip-text-placeholder eip-icon-picker-arrow">
              <a-icon type="close-circle" theme="filled" @click="iconClear" />
              <a-icon type="down" />
            </div>
          </span></span>
      </div>
      <div slot="overlay" class="ant-dropdown-menu eip-icon-picker-popper">
        <div class="ant-tabs-line ant-tabs ant-tabs-top">
          <a-tabs default-active-key="outline" @change="tabChange">
            <a-tab-pane key="all" tab="所有"> </a-tab-pane>
            <a-tab-pane key="outline" tab="线框风格"> </a-tab-pane>
            <a-tab-pane key="fill" tab="实底风格"> </a-tab-pane>
            <a-tab-pane key="twotone" tab="双色风格"> </a-tab-pane>
            <div class="eip-icon-picker-search" slot="tabBarExtraContent">
              <a-input-search v-model="key" size="small" placeholder="关键字搜索图标" style="width: 150px" @change="keySearch"
                @search="keySearch" />
            </div>
          </a-tabs>
        </div>
        <div class="eip-icon-picker-pane">
          <div class="eip-icon-picker-list">
            <div class="eip-icon-picker-item" v-for="(item, index) in iconList" :key="index" @click="iconClick(item)">
              <a-tooltip>
                <template slot="title">
                  {{ item.name }}
                </template>
                <a-card :hoverable="true">
                  <div class="eip-icon-picker-item-icon">
                    <a-icon :type="item.name" :theme="getTheme(item)"></a-icon>
                  </div>
                </a-card></a-tooltip>
            </div>
          </div>
          <div>
            <a-pagination style="margin-right: 9px" v-model="page.param.current" class="padding-top-sm float-right"
              size="small" show-quick-jumper :page-size-options="page.sizeOptions"
              :show-total="(total) => `共 ${total} 条`" :page-size="page.param.size" :total="page.total"
              @change="pageChange" />
          </div>
        </div>
      </div>
    </a-dropdown>
  </div>
</template>
<script>
import VueIcon from "@ant-design/icons-vue";
export default {
  name: "icons",
  props: ["value", "record", "disabled"],
  data() {
    return {
      trigger: ["click"],
      visible: false,
      icons: VueIcon.definitions.collection,
      iconList: [],
      tabKey: "all",
      key: "",
      page: {
        param: {
          current: 1,
          size: 28,
          sord: "asc",
          sidx: "",
          id: "",
          filters: "",
        },
        total: 0,
        sizeOptions: this.eipPage.sizeOptions,
      },
      icon: {
        name: this.name,
        theme: this.theme,
      },
    };
  },
  watch: {
    value(value) {
      if (value) {
        this.icon = JSON.parse(value);
      }
    }
  },
  // props: {
  //   name: {
  //     type: String,
  //   },
  //   theme: {
  //     type: String,
  //   },
  // },
  created() {
    this.getIcons();
  },
  methods: {
    /**
     * 清空图标
     */
    iconClear() {
      this.icon.name = null;
      this.icon.theme = null;
      this.$emit("clear");
    },
    /**
     *图标点击
     */
    iconClick(item) {
      this.icon.name = item.name;
      this.icon.theme = this.getTheme(item);
      this.visible = false;
      this.$emit("change", JSON.stringify(this.icon));
    },

    /**
     * 分页改变
     */
    pageChange(page, pageSize) {
      this.page.param.current = page;
      this.page.param.size = pageSize;
      this.getIcons();
    },

    /**
     * tab切换
     */
    tabChange(activeKey) {
      this.page.param.current = 1;
      this.tabKey = activeKey;
      this.getIcons();
    },

    /**
     * 关键字搜索
     */
    keySearch() {
      this.page.param.current = 1;
      this.getIcons();
    },

    /**
     * 获取所有图标
     */
    getIcons() {
      let searchIcons = [];
      if (this.tabKey == "all") {
        if (this.key) {
          searchIcons = Object.values(this.icons).filter(
            (t) => t.name.indexOf(this.key.trim()) > -1
          );
        } else {
          searchIcons = Object.values(this.icons).filter((t) => t);
        }
      } else {
        if (this.key) {
          searchIcons = Object.values(this.icons).filter(
            (t) =>
              t.theme == this.tabKey && t.name.indexOf(this.key.trim()) > -1
          );
        } else {
          searchIcons = Object.values(this.icons).filter(
            (t) => t.theme == this.tabKey
          );
        }
      }
      const m = (this.page.param.current - 1) * this.page.param.size;
      const n = this.page.param.current * this.page.param.size;
      this.iconList = searchIcons.slice(m, n);
      this.page.total = searchIcons.length;
    },

    /**
     * theme切换
     */
    getTheme(item) {
      if (item.theme == "outline") return "outlined";
      if (item.theme == "fill") return "filled";
      if (item.theme == "twotone") return "twoTone";
    },
  },
};
</script>

<style lang="less" scoped>
.eip-icon-picker-popper .eip-icon-picker-pane>.ant-menu .ant-menu-item {
  padding: 0 12px !important;
  margin: 12px 0 0 0 !important;
}

.eip-icon-picker {
  cursor: pointer;
}

.eip-icon-picker .eip-icon-picker-arrow {
  font-size: 12px;
}

.eip-text-placeholder {
  color: #bfbfbf;
}

.eip-icon-picker:hover .eip-icon-picker-arrow .anticon-close-circle+.anticon-down,
.eip-icon-picker:not(:hover) .eip-icon-picker-arrow .anticon-close-circle {
  display: none;
}

.eip-icon-picker-popper .eip-icon-picker-item {
  width: 14.1%;
  display: inline-block;
  padding: 6px 2px 6px 10px;
}

.eip-icon-picker-popper {
  width: 520px;
  max-width: 90vw;
  padding: 0;
}

.eip-icon-picker-popper /deep/ .ant-tabs-tab {
  padding: 10px 0 !important;
  margin: 0 12px !important;
}

.eip-icon-picker-popper .eip-icon-picker-pane {
  height: 280px;
}

.eip-icon-picker-popper .eip-icon-picker-pane>.ant-menu {
  width: 120px;
  height: 100%;
  flex-shrink: 0;
  overflow-x: hidden;
}

.eip-icon-picker-popper .eip-icon-picker-list {
  height: 230px;
}

.eip-icon-picker-popper /deep/ .ant-card-body {
  padding: 0;
}

.eip-icon-picker-popper .eip-icon-picker-item-icon {
  padding: 8px 0;
  font-size: 18px;
  text-align: center;
  transition: transform 0.1s;
}

.eip-icon-picker-popper .eip-icon-picker-item-icon:hover {
  transform: scale(1.6);
}

.eip-icon-picker-popper .eip-icon-picker-search {
  width: 150px;
  margin-right: 9px;
}

.eip-icon-picker-popper .ant-tabs-bar {
  margin: 0;
}

.ant-card-hoverable {
  cursor: pointer;
}

.anticon-close-circle:hover {
  color: rgba(0, 0, 0, 0.45);
}
</style>
