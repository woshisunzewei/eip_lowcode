<template>
  <div>
    <a-tooltip
      :title="org.name"
      v-for="(org, index) in record.options.chosen"
      :key="org.id"
    >
      <a-tag
        @close="organizationClose(index)"
        :closable="!record.options.disabled"
      >
        <div class="controlTags">
          <div class="departWrap">
            <a-icon style="font-size: 12px" type="apartment" />
          </div>
        </div>
        <label style="vertical-align: middle">{{ org.name }}</label>
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

    <eip-organization
      v-if="options.visible"
      :visible.sync="options.visible"
      :chosen="record.options.chosen"
      :multiple="record.options.multiple"
      :range="record.options.range"
      :title="record.options.placeholder"
      :checkStrictly="record.options.checkStrictly"
      @ok="chosenOrganizationOk"
    ></eip-organization>
  </div>
</template>
<script>
import { mapGetters } from "vuex";
export default {
  data() {
    return {
      //范围
      options: {
        visible: false,
      },
    };
  },
  computed: {
    ...mapGetters("account", ["user"]),
  },
  props: ["value", "record", "disabled"],
  methods: {
    /**
     * 组织选择
     */
    onChosen() {
      this.options.visible = true;
    },

    /**
     * 删除
     */
    organizationClose(spIndex) {
      this.record.options.chosen.splice(spIndex, 1);
      this.$emit("change", JSON.stringify(this.record.options.chosen));
    },
    /**
     * 选中组织
     */
    chosenOrganizationOk(data) {
      var titles = data.map((m) => m.title).join(",");
      var orgs = [];
      data.forEach((item) => {
        orgs.push({
          id: item.value,
          name: item.title,
        });
      });
      this.record.options.chosen = orgs;
      this.$emit("change", titles);
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

.controlTags {
  align-items: center;
  border-color: @primary-color;
  display: inline-flex;
  height: 23px;
  max-width: 100%;
  position: relative;
  vertical-align: top;
}

.controlTags .departWrap {
  align-items: center;
  border-radius: 13px;
  color: #fff;
  background-color: @primary-color;
  display: inline-flex;
  flex-shrink: 0;
  height: 23px;
  justify-content: center;
  width: 23px;
}
</style>
