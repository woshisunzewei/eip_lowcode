<template>
  <div class="getSingleData">
    <a-form-model layout="horizontal" :model="drawerData">
      <a-form-model-item label="选择更新对象">
        <a-select
          v-model="drawerData.updateObj"
          :filter-option="filterOption"
          show-search
          allow-clear
          placeholder="当前流程中的节点对象"
          style="width: 100%"
          @change="updateObjChange"
        >
          <a-select-option
            v-for="(item, index) in configList"
            :key="index"
            :value="item.id"
            >{{ item.title }}</a-select-option
          >
        </a-select>
      </a-form-model-item>
      <a-form-model-item label="更新字段" v-if="drawerData.updateObj">
        <div v-for="(item, index) in drawerData.updateData" :key="index">
          <span>{{ item.label }}</span>
          <div class="flex justify-start">
            <eip-editor
              :ref="item.model"
              class="flex-sub"
              v-model="item.value"
              :height="tinymce.height"
              :toolbar="tinymce.toolbar"
              :plugins="tinymce.plugins"
              :statusbar="tinymce.statusbar"
              content_style="p{margin:5px} body{margin:4px 0 0 4px} .mce-content-body:not([dir=rtl])[data-mce-placeholder]:not(.mce-visualblocks)::before{left:6px}"
              :menubar="tinymce.menubar"
              :placeholder="item.placeholder"
            ></eip-editor>
            <a-popover placement="bottomLeft" v-model="item.show">
              <div slot="content" style="width: 600px">
                <a-input-search
                  placeholder="搜索流程节点对象下的字段"
                  style="width: 100%; padding-bottom: 4px"
                />
                <a-collapse
                  :expand-icon-position="expandIconPosition"
                  style="max-height: 400px; overflow: auto"
                >
                  <a-collapse-panel
                    v-for="(citem, cindex) in configList"
                    :key="cindex"
                    :header="citem.title"
                  >
                    <!-- 表单触发 -->
                    <ul v-if="citem.type == 0" class="conditionFieldBox show">
                      <li
                        @click="columnClick(item, index, citem, itemc)"
                        v-for="itemc in citem.data.columns"
                        :key="itemc.model"
                        class="flexRow ThemeHoverBGColor3"
                      >
                        <div class="ellipsis">
                          <span class="field">[{{ itemc.type }}]</span
                          ><span title="自动编号">{{ itemc.label }}</span>
                        </div>
                      </li>
                    </ul>

                    <!-- 获取多条数据 -->
                    <ul v-if="citem.type == 4" class="conditionFieldBox show">
                      <li
                        @click="columnClick(item, index, citem, itemc)"
                        v-for="itemc in citem.data.columns"
                        :key="itemc.model"
                        class="flexRow ThemeHoverBGColor3"
                      >
                        <div class="ellipsis">
                          <span class="field">[{{ itemc.type }}]</span
                          ><span title="自动编号">{{ itemc.label }}</span>
                        </div>
                      </li>
                    </ul>

                    <!-- 子流程 -->
                    <ul v-if="citem.type == 301" class="conditionFieldBox show">
                      <li
                        @click="columnClick(item, index, citem, itemc)"
                        v-for="itemc in citem.data.columns"
                        :key="itemc.model"
                        class="flexRow ThemeHoverBGColor3"
                      >
                        <div class="ellipsis">
                          <span class="field" v-if="itemc.type"
                            >[{{ itemc.type }}]</span
                          ><span title="自动编号">{{ itemc.label }}</span>
                        </div>
                      </li>
                    </ul>

                    <!-- 子流程 -->
                    <ul v-if="citem.type == 21" class="conditionFieldBox show">
                      <li
                        @click="columnClick(item, index, citem, itemc)"
                        v-for="itemc in citem.data.columns"
                        :key="itemc.model"
                        class="flexRow ThemeHoverBGColor3"
                      >
                        <div class="ellipsis">
                          <span class="field" v-if="itemc.type"
                            >[{{ itemc.type }}]</span
                          ><span title="自动编号">{{ itemc.label }}</span>
                        </div>
                      </li>
                    </ul>

                    <span slot="extra" class="Gray_9e">
                      {{
                        citem.type == 0
                          ? "工作表“" + citem.data.tableName + "”"
                          : ""
                      }}
                    </span>
                  </a-collapse-panel>
                </a-collapse>
              </div>
              <a-button type="primary" icon="diff"></a-button>
            </a-popover>
          </div>
        </div>
      </a-form-model-item>
    </a-form-model>
  </div>
</template>

<script>
export default {
  name: "updateData",
  components: {},
  props: {
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
      visible: false,
      expandIconPosition: "right",
      tinymce: {
        plugins: "preview fullscreen code",
        toolbar: "",
        height: 32,
        statusbar: false,
        show: true,
        menubar: "",
      },
      columns: [],
    };
  },

  mounted() {
    this.init();
  },
  methods: {
    /**
     * 选择
     * @param {*} item
     * @param {*} itemc
     */
    columnClick(item, index, citem, itemc) {
      let html =
        "<span id='" +
        encodeURIComponent(
          JSON.stringify({
            id: citem.id,
            model: itemc.model,
          })
        ) +
        "' class='non-editable'>" +
        itemc.label +
        "</span>";
      this.$refs[item.model][0].insertIndex(html, index);
      item.show = false;
    },
    /**
     *
     * @param {搜索} input
     * @param {*} option
     */
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text
          .toLowerCase()
          .indexOf(input.toLowerCase()) >= 0
      );
    },
    updateObjChange(e) {
      var config = this.$utils.find(this.configList, (item) => item.id == e);
      if (config) {
        if (config.type == 0) {
          this.drawerData.updateData = config.data.columns;
        } else if (config.type == 21) {
          //得到属性
          config.data["columns"] = [];
          for (let item in config.data.test.data) {
            config.data.columns.push({
              model: item,
              label: item,
            });
          }
        }
      } else {
        this.drawerData.updateData = [];
      }
    },

    /**
     * 初始化
     */
    init() {},
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

.flexRow {
  display: flex;
  min-width: 0;
}

/deep/ .info {
  padding: 16px 0;
  font-size: 12px;
  width: 100% !important;
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