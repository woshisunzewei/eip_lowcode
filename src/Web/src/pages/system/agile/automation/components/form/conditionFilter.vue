<template>
  <div class="groupOp-group-container tree box">
    <a-card
      class="box-card"
      style="margin-top: 4px"
      :bodyStyle="{ padding: '0 2px 0 10px' }"
    >
      <div style="padding: 4px">
        <a-row>
          <a-row class="child">
            <a-radio-group button-style="solid" v-model="filters.groupOp">
              <a-radio-button value="AND">且</a-radio-button>
              <a-radio-button value="OR">或</a-radio-button>
            </a-radio-group>
            <a-button
              style="float: right; margin-left: 10px"
              type="primary"
              icon="gateway"
              @click="
                filters.groups.push({
                  groupOp: 'AND',
                  rules: [
                    {
                      fieldOption: { type: null },
                      field: undefined,
                      op: undefined,
                      data: undefined,
                      fieldVisible: false,
                      dataVisible: false,
                    },
                  ],
                  groups: [],
                })
              "
              >分组</a-button
            >
            <a-button
              style="float: right; margin-left: 10px"
              type="primary"
              icon="plus"
              @click="
                filters.rules.push({
                  fieldOption: { type: null },
                  field: undefined,
                  op: undefined,
                  data: undefined,
                  fieldVisible: false,
                  dataVisible: false,
                })
              "
              >条件</a-button
            >
          </a-row>

          <template v-for="(item, index) in filters.rules">
            <div :key="'condition' + index" class="child">
              <div class="flex justify-start">
                <eip-editor
                  class="flex-sub"
                  :ref="'conditionFilterField_' + index"
                  :height="tinymce.height"
                  :toolbar="tinymce.toolbar"
                  :plugins="tinymce.plugins"
                  :statusbar="tinymce.statusbar"
                  v-model="item.field"
                  placeholder="请选择查询条件"
                  content_style="p{margin:5px} body{margin:4px 0 0 4px}"
                  :menubar="tinymce.menubar"
                ></eip-editor>
                <a-popover
                  placement="bottomLeft"
                  v-model="item.fieldVisible"
                  trigger="click"
                >
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
                        v-for="(iteml, indexc) in configList"
                        :key="indexc"
                        :header="iteml.title"
                      >
                        <ul class="conditionFieldBox show">
                          <li
                            @click="
                              conditionFilterFieldClick(
                                item,
                                iteml,
                                itemc,
                                index
                              )
                            "
                            v-for="itemc in iteml.data.columns"
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
                            iteml.type == "0"
                              ? "工作表“" + iteml.data.tableName + "”"
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
              <div class="flex justify-start align-center margin-top-xs">
                <a-space>
                  <a-select
                    style="width: 100px"
                    v-model="item.op"
                    showSearch
                    allowClear
                    placeholder="请选择"
                  >
                    <template v-if="item.fieldOption.type == 'datetime'">
                      <a-select-option
                        v-for="itemOp in opControls.datetime"
                        :key="itemOp.value"
                        :value="itemOp.value"
                        >{{ itemOp.title }}</a-select-option
                      >
                    </template>

                    <template
                      v-else-if="
                        ['select', 'radio', 'checkbox'].includes(
                          item.fieldOption.type
                        )
                      "
                    >
                      <a-select-option
                        v-for="itemOp in opControls.select"
                        :key="itemOp.value"
                        :value="itemOp.value"
                        >{{ itemOp.title }}</a-select-option
                      >
                    </template>

                    <template v-else-if="item.fieldOption.type == 'user'">
                      <a-select-option
                        v-for="itemOp in opControls.user"
                        :key="itemOp.value"
                        :value="itemOp.value"
                        >{{ itemOp.title }}</a-select-option
                      >
                    </template>

                    <template
                      v-else-if="item.fieldOption.type == 'organization'"
                    >
                      <a-select-option
                        v-for="itemOp in opControls.organization"
                        :key="itemOp.value"
                        :value="itemOp.value"
                        >{{ itemOp.title }}</a-select-option
                      >
                    </template>

                    <template v-else-if="item.fieldOption.type == 'bool'">
                      <a-select-option
                        v-for="itemOp in opControls.bool"
                        :key="itemOp.value"
                        :value="itemOp.value"
                        >{{ itemOp.title }}</a-select-option
                      >
                    </template>

                    <template v-else-if="item.fieldOption.type == 'datetime'">
                      <a-select-option
                        v-for="itemOp in opControls.datetime"
                        :key="itemOp.value"
                        :value="itemOp.value"
                        >{{ itemOp.title }}</a-select-option
                      >
                    </template>

                    <template v-else-if="item.fieldOption.type == 'number'">
                      <a-select-option
                        v-for="itemOp in opControls.number"
                        :key="itemOp.value"
                        :value="itemOp.value"
                        >{{ itemOp.title }}</a-select-option
                      >
                    </template>

                    <template v-else>
                      <a-select-option
                        v-for="itemOp in opControls.input"
                        :key="itemOp.value"
                        :value="itemOp.value"
                        >{{ itemOp.title }}</a-select-option
                      >
                    </template>
                  </a-select>
                  <div
                    class="flex justify-start"
                    style="width: 330px"
                    v-if="!['nu', 'nn'].includes(item.op)"
                  >
                    <eip-editor
                      class="flex-sub"
                      :ref="'conditionFilterData_' + index"
                      :height="tinymce.height"
                      :toolbar="tinymce.toolbar"
                      :plugins="tinymce.plugins"
                      :statusbar="tinymce.statusbar"
                      placeholder="请选择查询值"
                      v-model="item.data"
                      content_style="p{margin:5px} body{margin:4px 0 0 4px}"
                      :menubar="tinymce.menubar"
                    ></eip-editor>
                    <a-popover
                      placement="bottomLeft"
                      v-model="item.dataVisible"
                      trigger="click"
                    >
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
                            v-for="(iteml, indexc) in configList"
                            :key="indexc"
                            :header="iteml.title"
                          >
                            <ul class="conditionFieldBox show">
                              <li
                                @click="
                                  conditionFilterDataClick(
                                    item,
                                    iteml,
                                    itemc,
                                    index
                                  )
                                "
                                v-for="itemc in iteml.data.columns"
                                :key="itemc.model"
                                class="flexRow ThemeHoverBGColor3"
                              >
                                <div class="ellipsis">
                                  <span class="field">[{{ itemc.type }}]</span
                                  ><span title="自动编号">{{
                                    itemc.label
                                  }}</span>
                                </div>
                              </li>
                            </ul>
                            <span slot="extra" class="Gray_9e">
                              {{
                                iteml.type == "0"
                                  ? "工作表“" + iteml.data.tableName + "”"
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
                  <!-- <a-select
                    v-if="
                      ['select', 'radio', 'checkbox'].includes(
                        item.fieldOption.type
                      ) && !item.fieldOption.dynamic
                    "
                    style="width: 100px"
                    v-model="item.data"
                    showSearch
                    allowClear
                    placeholder="请选择"
                  >
                    <a-select-option
                      v-for="itemOp in item.fieldOption.options"
                      :key="itemOp.value"
                      :value="itemOp.value"
                      >{{ itemOp.label }}</a-select-option
                    >
                  </a-select>

                  <a-input
                    v-else
                    placeholder="请输入内容"
                    v-model="item.data"
                    style="width: 100px"
                  ></a-input> -->

                  <a-popconfirm
                    ok-text="确认"
                    cancel-text="取消"
                    @confirm="filters.rules.splice(index, 1)"
                  >
                    <template slot="title"> 删除规则 </template>
                    <a-button icon="delete" type="danger"></a-button>
                  </a-popconfirm>
                </a-space>
              </div>
            </div>
          </template>

          <template v-for="(item, index) in filters.groups">
            <a-row
              :key="'group' + index"
              class="child"
              v-if="item.rules.length > 0"
            >
              <conditionFilter
                :configList="configList"
                :tables="tables"
                :filters.sync="item"
              />
            </a-row>
          </template>
        </a-row>
      </div>
    </a-card>
  </div>
</template>

<script>
export default {
  name: "conditionFilter",
  props: {
    filters: {
      type: Object,
      default: function () {
        return {
          groupOp: "AND",
          rules: [],
          groups: [],
        };
      },
    },
    columns: {
      type: Array,
    },
    tables: {
      type: Array,
    },
    configList: {
      type: Array,
    },
  },
  data() {
    return {
      tinymce: {
        plugins: "preview fullscreen code",
        toolbar: "",
        height: 32,
        statusbar: false,
        show: false,
        menubar: "",
      },
    };
  },
  methods: {
    /**
     * 选择
     * @param {*} item
     * @param {*} itemc
     */
    conditionFilterFieldClick(item, iteml, itemc, index) {
      let html =
        "<span id='" +
        encodeURIComponent(
          JSON.stringify({
            id: iteml.id,
            model: itemc.model,
          })
        ) +
        "' class='non-editable'>" +
        itemc.label +
        "</span>";
      this.$refs["conditionFilterField_" + index][0].insertIndex(
        html,
        index * 2
      );
      item.fieldVisible = false;

      this.optionsChange(item, iteml, itemc);
    },
    /**
     * 选择
     * @param {*} item
     * @param {*} itemc
     */
    conditionFilterDataClick(item, iteml, itemc, index) {
      let html =
        "<span id='" +
        encodeURIComponent(
          JSON.stringify({
            id: iteml.id,
            model: itemc.model,
          })
        ) +
        "' class='non-editable'>" +
        itemc.label +
        "</span>";
      this.$refs["conditionFilterData_" + index][0].insertIndex(
        html,
        index * 2 + 1
      );
      item.dataVisible = false;

      this.optionsChange(item, iteml, itemc);
    },
    /**
     * 搜索
     * @param {*} input
     * @param {*} option
     */
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text
          .toUpperCase()
          .indexOf(input.toUpperCase()) >= 0
      );
    },
    /**
     * 选项改变
     * @param {*} e
     */
    optionsChange(item, iteml, itemc) {
      var data = this.$utils.find(
        iteml.data.columns,
        (f) => f.model == itemc.model
      );
      item.fieldOption.type = data.type;
      switch (data.type) {
        case "select":
        case "radio":
        case "checkbox":
          item.fieldOption.dynamic = data.options.dynamic;
          item.fieldOption.dynamicConfig = data.options.dynamicConfig;
          item.fieldOption.options = data.options.options;
          item.op = this.opControls.select[0].value;
          break;
        case "user":
          item.op = this.opControls.user[0].value;
          break;
        case "organization":
          item.op = this.opControls.organization[0].value;
          break;
        case "bool":
          item.op = this.opControls.bool[0].value;
          break;
        case "datetime":
          item.op = this.opControls.datetime[0].value;
          break;
        case "number":
          item.op = this.opControls.number[0].value;
          break;
        default:
          item.op = this.opControls.input[0].value;
          break;
      }
    },
  },
};
</script>

<style scoped>
.box {
  width: 100%;
}

/* 只需要左边边框线 */
.child {
  width: 100%;
  position: relative;
  border: 1px solid #d9d9d9;
  border-style: none none none solid;
  padding: 2px 0;
  padding-left: 12px;
}

/* 设置一个伪元素宽2px 高50% 用于遮挡多余的左边框线 */
.child::before {
  display: block;
  content: "";
  position: absolute;
  background-color: white;
  width: 1px;
  height: 50%;
}

/* 设置第一个子元素的伪类定位 */
.box .child:first-child::before {
  left: -1px;
  top: 0;
}

/* 设置第二个子元素的伪类定位 */
.box .child:last-child::before {
  left: -1px;
  bottom: 0;
}

/* 设置子元素的横线，定位在高度的50% */
.box .child::after {
  top: 50%;
  left: 0;
  position: absolute;
  content: "";
  display: block;
  width: 10px;
  height: 1px;
  border: 1px solid #d9d9d9;
  border-style: solid none none none;
}

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
