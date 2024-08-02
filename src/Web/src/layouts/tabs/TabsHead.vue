<template>
  <div :class="['tabs-head', layout, pageWidth]">
    <a-tabs
      type="editable-card"
      :class="[
        'eip-admin-tabs tabs-container',
        layout,
        pageWidth,
        {
          affixed: affixed,
          'fixed-header': fixedHeader,
          collapsed: adminLayout.collapsed,
        },
      ]"
      :active-key="active"
      :hide-add="true"
    >
      <a-tooltip placement="left" slot="tabBarExtraContent">
        <a-dropdown style="width: 30px">
          <div>
            <a-icon type="down" />
          </div>
          <a-menu slot="overlay">
            <a-menu-item @click="onMenuSelect('4')">
              <a-icon type="sync" />
              <span>{{ $t("refresh") }}</span>
            </a-menu-item>
            <a-menu-item @click="onMenuSelect('1')">
              <a-icon type="vertical-right" />
              <span> {{ $t("closeLeft") }}</span>
            </a-menu-item>
            <a-menu-item @click="onMenuSelect('2')">
              <a-icon type="vertical-left" />
              <span>{{ $t("closeRight") }}</span>
            </a-menu-item>
            <a-menu-item @click="onMenuSelect('3')">
              <a-icon type="close" />
              <span>{{ $t("closeOthers") }}</span>
            </a-menu-item>
          </a-menu>
        </a-dropdown>
      </a-tooltip>
      <!-- <a-tooltip placement="left" :title="lockTitle" slot="tabBarExtraContent">
        <a-icon
            theme="filled"
            @click="onLockClick"
            class="header-lock"
            :type="fixedTabs ? 'lock' : 'unlock'"
        />
      </a-tooltip> -->
      <a-tab-pane v-for="page in pageList" :key="page.fullPath">
        <div
          slot="tab"
          class="tab"
          @contextmenu="(e) => onContextmenu(page.fullPath, e)"
        >
          <a-icon :type="page.icon" v-if="page.fullPath !== active" />

          <a-icon
            @click="onRefresh(page)"
            :class="[
              'icon-sync',
              { hide: page.fullPath !== active && !page.loading },
            ]"
            :type="page.loading ? 'loading' : 'sync'"
          />

          <div class="title" @click="onTabClick(page.fullPath)">
            {{ pageName(page) }}
          </div>
          <a-icon
            v-if="!page.unclose"
            @click="onClose(page.fullPath)"
            class="icon-close"
            type="close"
          />
        </div>
      </a-tab-pane>
    </a-tabs>
    <div v-if="affixed" class="virtual-tabs"></div>
  </div>
</template>

<script>
import { mapState, mapMutations } from "vuex";
import { getI18nKey } from "@/utils/routerUtil";

export default {
  name: "TabsHead",
  i18n: require("./i18n"),
  props: {
    pageList: Array,
    active: String,
    fixed: Boolean,
  },
  data() {
    return {
      affixed: true,
    };
  },
  inject: ["adminLayout"],
  created() {
    this.affixed = this.fixedTabs;
  },
  computed: {
    ...mapState("setting", [
      "layout",
      "pageWidth",
      "fixedHeader",
      "fixedTabs",
      "customTitles",
    ]),
    lockTitle() {
      return this.$t(this.fixedTabs ? "unlock" : "lock");
    },
  },
  methods: {
    ...mapMutations("setting", ["setFixedTabs"]),
    onLockClick() {
      this.setFixedTabs(!this.fixedTabs);
      if (this.fixedTabs) {
        setTimeout(() => {
          this.affixed = true;
        }, 200);
      } else {
        this.affixed = false;
      }
    },
    onTabClick(key) {
      if (this.active !== key) {
        this.$emit("change", key);
      }
    },
    onClose(key) {
      this.$emit("close", key);
    },
    onRefresh(page) {
      this.$emit("refresh", page.fullPath, page);
    },
    onContextmenu(pageKey, e) {
      this.$emit("contextmenu", pageKey, e);
    },
    onMenuSelect(key) {
      this.$emit("menuselect", key, "", this.active);
    },
    pageName(page) {
      const pagePath = page.fullPath.split("?")[0];
      const custom = this.customTitles.find((item) => item.path === pagePath);
      return (
        (custom && custom.title) ||
        page.title ||
        this.$t(getI18nKey(page.keyPath))
      );
    },
  },
};
</script>

<style scoped lang="less">
.tab {
  margin: 0 -16px;
  padding: 0 16px;
  font-size: 14px;
  user-select: none;
  transition: all 0.2s;

  .title {
    display: inline-block;
    height: 100%;
  }

  .icon-close {
    font-size: 12px;
    margin-left: 6px;
    margin-right: -4px !important;
    color: @text-color-second;

    &:hover {
      color: @text-color;
    }
  }

  .icon-sync {
    margin-left: -4px;
    color: @primary-4;
    transition: all 0.3s ease-in-out;

    &:hover {
      color: @primary-color;
    }

    font-size: 14px;

    &.hide {
      font-size: 0;
    }
  }
}

.tabs-head {
  margin: 0 auto;

  &.head.fixed {
    width: 1400px;
  }
}

.tabs-container {
  margin: -16px auto 8px;
  transition: top, left 0.2s;

  .header-lock {
    font-size: 18px;
    cursor: pointer;
    color: @primary-3;

    &:hover {
      color: @primary-color;
    }
  }

  &.affixed {
    margin: 0 auto;
    top: 0px;
    position: fixed;
    height: 48px;
    z-index: 1;
    background-color: @layout-body-background;

    &.side,
    &.mix {
      right: 0;
      left: 200px;

      &.collapsed {
        left: 48px;
      }
    }

    &.head {
      width: inherit;
      padding: 8px 0 0;

      &.fluid {
        left: 0;
        right: 0;
        padding: 8px 24px 0;
      }
    }

    &.fixed-header {
      top: 48px;
    }
  }
}

.virtual-tabs {
  height: 28px;
}
</style>
