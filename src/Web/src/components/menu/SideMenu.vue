<template>
  <a-layout-sider
    :theme="sideTheme"
    :class="[
      'side-menu eip-admin-collapse',
      'beauty-scroll',
      isMobile ? null : 'shadow',
    ]"
    collapsedWidth="48px"
    width="200px"
    :collapsible="collapsible"
    v-model="collapsed"
    :trigger="null"
  >
    <div :class="['logo', theme]">
      <router-link to="/dashboard/workplace">
        <img v-if="systemLogo" :src="systemLogo" />
        <h1 :style="{ width: (systemLogo ? 150 : 190) + 'px' }">
          {{ systemTitle }}
        </h1>
      </router-link>
    </div>
    <i-menu
      :theme="theme"
      :collapsed="collapsed"
      :options="menuData"
      @select="onSelect"
      class="menu eip-admin-sidebar"
    />
  </a-layout-sider>
</template>

<script>
import IMenu from "./menu";
import { mapState } from "vuex";
export default {
  name: "SideMenu",
  components: { IMenu },
  props: {
    collapsible: {
      type: Boolean,
      required: false,
      default: false,
    },
    collapsed: {
      type: Boolean,
      required: false,
      default: false,
    },
    menuData: {
      type: Array,
      required: true,
    },
    theme: {
      type: String,
      required: false,
      default: "dark",
    },
    systemTitle: {
      type: String,
      default: "",
    },
    systemShortName: {
      type: String,
      default: "",
    },
    systemLogo: {
      type: String,
      default: "",
    },
  },
  computed: {
    sideTheme() {
      return this.theme == "light" ? this.theme : "dark";
    },
    ...mapState("setting", ["isMobile", "systemName"]),
  },
  methods: {
    onSelect(obj) {
      this.$emit("menuSelect", obj);
    },
  },
};
</script>

<style lang="less" scoped>
@import "index";

.logo {
  text-align: center;
  padding: 0 4px;
}

.side-menu .logo h1 {
  margin-left: 4px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
