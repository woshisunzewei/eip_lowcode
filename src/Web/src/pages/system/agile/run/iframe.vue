<template>
  <div style="width: 100%">
    <a-spin :spinning="loading">
      <iframe ref="iframe" :height="height" frameborder="0" class="iframe" id="iframe" onload="onload"
        :src="src"></iframe>
    </a-spin>
  </div>
</template>

<script>
import { logout } from "@/services/account/login";
import { mapGetters } from "vuex";
export default {
  data() {
    return {
      loading: true,
      src: "",
      height: this.eipHeaderHeight() - 16 + "px",
    };
  },
  computed: {
    ...mapGetters("account", ["user"]),
  },
  props: {
    url: {
      type: String,
    },
  },

  mounted() {
    this.initIframe();
  },

  methods: {
    /**
     * 初始化
     */
    initIframe() {
      let that = this;
      that.$loading.show({ text: that.eipMsg.loading });
      if (this.url) {
        this.src = this.url.replace(/{userId}/g, this.user.userId);
        window.addEventListener("message", this.handleMessage);
        let iframe = document.getElementById("iframe");
        if (iframe.attachEvent) {
          // IE下
          iframe.attachEvent("onload", function () {
            //后续操作
            that.loading = false;
            that.$loading.hide();
          });
        } else {
          iframe.onload = function () {
            //后续操作
            that.loading = false;
            that.$loading.hide();
          };
        }
      } else {
        that.loading = false;
        that.$loading.hide();
      }
    },

    /**
     * 子页面传回postMessage
     *   window.parent.postMessage({
            type: "loginOut"
        }, '*');
     */
    handleMessage(event) {
      let that = this;
      const data = event.data;
      if (data.type === "loginOut") {
        this.$error({
          title: "登录提示",
          content: "登录状态已失效,请重新登录",
          okText: "确定",
          okType: "danger",
          
          onOk() {
            logout();
            that.$router.push("/login");
          },
        });
      }
    },
  },
};
</script>

<style lang="less" scoped>
.iframe {
  width: 100%;
}
</style>