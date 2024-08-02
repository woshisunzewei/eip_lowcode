<template>
  <div>
    <a-tooltip
      :title="user.name"
      v-for="(user, index) in record.options.chosen"
      :key="user.id"
    >
      <a-tag @close="userClose(index)" :closable="!record.options.disabled">
        <img
          style="width: 23px; height: 23px; border-radius: 50%"
          v-real-img="user.headImage"
        />
        <label style="vertical-align: middle">{{ user.name }}</label>
      </a-tag>
    </a-tooltip>

    <a-tooltip
      v-if="!record.options.disabled"
      :title="record.options.placeholder"
    >
      <div @click="onChosen" class="addBtn">
        <a-icon type="plus" @click="onChosen" />
      </div>
    </a-tooltip>

    <eip-user
      v-if="options.visible"
      :visible.sync="options.visible"
      :topOrg="options.topOrg"
      :specificOrg="record.options.specificOrg"
      :chosen="record.options.chosen"
      :multiple="record.options.multiple"
      :disabled="record.options.disabled"
      :title="record.options.placeholder"
      @ok="chosenUserOk"
    ></eip-user>
  </div>
</template>
<script>
import { mapGetters } from "vuex";
export default {
  data() {
    return {
      //范围选择人员
      options: {
        visible: false,
        topOrg: "",
      },
    };
  },
  computed: {
    ...mapGetters("account", ["user"]),
  },
  props: ["value", "record", "disabled"],
  created() {
    if (this.record.options.chosen.length == 0) {
      switch (this.record.options.defaultValueType) {
        case "username":
          var users = [];
          users.push({
            id: this.user.userId,
            name: this.user.name,
            headImage: this.user.headImage,
          });
          this.record.options.chosen = users;
          break;
      }
    }
  },
  methods: {
    /**
     * 人员选择
     */
    onChosen() {
      //判断选择类型是否选择所有
      this.options.topOrg = this.record.options.topOrg
        ? this.user.organizationId
        : "";
      this.options.visible = true;
    },

    /**
     * 删除
     */
    userClose(spIndex) {
      this.record.options.chosen.splice(spIndex, 1);
      this.$emit("change", JSON.stringify(this.record.options.chosen));
    },

    /**
     * 选中人员
     */
    chosenUserOk(datas) {
      var users = [];
      datas.forEach((item) => {
        users.push({
          id: item.userId,
          name: item.name,
          headImage: item.headImage,
        });
      });
      this.record.options.chosen = users;
      this.$emit("change", JSON.stringify(this.record.options.chosen));
    },
  },
};
</script>
<style lang="less" scoped>
/deep/ .ant-tag {
  cursor: pointer;
  padding: 0 7px 0 0 !important;
  border-radius: 50px !important;
}

/deep/ .ant-tag label {
  font-size: 12px;
  margin-left: 4px;
}

/deep/ .ant-tag .anticon-close {
  vertical-align: middle;
}

.addBtn {
  align-items: center;
  border: 1px solid #ddd;
  border-radius: 50%;
  display: inline-flex;
  height: 26px;
  justify-content: center;
  line-height: 26px;
  margin: 8px 0 0 0;
  vertical-align: top;
  text-align: center !important;
  width: 26px;
  cursor: pointer;
}

.addBtn:hover {
  border-color: @primary-color;
  color: @primary-color;
}
</style>
