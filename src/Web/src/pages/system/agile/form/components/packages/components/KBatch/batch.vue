<!--
 * @Description: 动态表格 用于批量填入数据
 * @Author: kcz
 * @Date: 2020-03-27 18:36:56
 * @LastEditors: kcz
 * @LastEditTime: 2021-05-14 14:04:14
 -->
<template>
  <a-form-model
    ref="dynamicValidateForm"
    layout="inline"
    :model="dynamicValidateForm"
  >
    <vxe-toolbar ref="batchToolbar" custom print export>
      <template v-slot:buttons>
        <a-space>
          <template
            v-for="(item, index) in record.options.buttons.filter(
              (f) => f.isShow
            )"
          >
            <a-button
              :key="index"
              v-if="item.trigger.type != 'import'"
              @click="buttonEvent(item)"
              :type="item.type"
            >
              <a-icon v-if="item.icon" :type="item.icon" />{{ item.label }}
            </a-button>
            <a-upload
              :key="index"
              v-else
              accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
              :showUploadList="false"
              :customRequest="importData"
            >
              <a-button type="primary">
                <a-icon v-if="item.icon" :type="item.icon" />{{ item.label }}
              </a-button></a-upload
            ></template
          >
        </a-space>
      </template>
    </vxe-toolbar>
    <vxe-table
      :height="record.options.scrollY"
      :edit-rules="validRules"
      border
      keep-source
      :ref="record.model"
      :loading="chosen.table.loading"
      class="my-xtable-antd"
      :data="dynamicValidateForm.domains"
      :edit-config="{ trigger: 'click', mode: 'row', showStatus: true }"
      :footer-method="returnDataFooter"
      :show-footer="record.options.showFooter"
      :tooltip-config="tooltipConfig"
      row-key
    >
      <template #loading>
        <a-spin></a-spin>
      </template>
      <template #empty>
        <eip-empty />
      </template>
      <vxe-column width="40" align="center" v-if="record.options.canDrag">
        <template #default>
          <a-tooltip placement="right" title=" 拖动排序">
            <span class="drag-btn">
              <a-icon type="drag" />
            </span>
          </a-tooltip>
        </template>
      </vxe-column>
      <vxe-column
        v-if="record.options.multiple"
        type="checkbox"
        width="60"
        align="center"
        fixed="left"
      >
        <template #header="{ checked, indeterminate }">
          <span @click.stop="$refs[record.model].toggleAllCheckboxRow()">
            <a-checkbox :indeterminate="indeterminate" :checked="checked">
            </a-checkbox>
          </span>
        </template>
        <template #checkbox="{ row, checked, indeterminate }">
          <span @click.stop="$refs[record.model].toggleCheckboxRow(row)">
            <a-checkbox :indeterminate="indeterminate" :checked="checked">
            </a-checkbox>
          </span>
        </template>
      </vxe-column>
      <vxe-column
        v-if="!record.options.hideSequence"
        type="seq"
        width="60"
        fixed="left"
      >
        <template #header>
          <span>序号</span>
        </template>
      </vxe-column>
      <vxe-column
        v-for="(item, index) in record.list.filter(
          (item) => !item.options.hidden
        )"
        :key="index"
        :width="
          item.options.width
            ? item.options.width.includes('%')
              ? item.options.width
              : parseFloat(item.options.width) + 20
            : item.label.length * 25 + ' px'
        "
        :field="item.model"
        :title="item.label"
        :edit-render="{ placeholder: item.options.placeholder }"
      >
        <template #edit="scope">
          <!-- 数据选择 -->
          <a-input
            v-if="item.type === 'dataList'"
            :disabled="true"
            :placeholder="item.options.placeholder"
            :allowClear="item.options.clearable"
            v-model="scope.row[item.model]"
          >
            <a-icon
              slot="addonAfter"
              type="search"
              @click="chosenDatalistClick(item, scope)"
            />
          </a-input>

          <!-- 选择框 -->
          <a-switch
            v-if="item.type === 'switch'"
            :disabled="item.options.disabled || parentDisabled"
            v-model="scope.row[item.model]"
            @change="updateStatus(scope)"
            :checked-children="item.options.txt.yes"
            :un-checked-children="item.options.txt.no"
          />

          <!-- 文本框 -->
          <a-input
            v-if="item.type === 'input'"
            :disabled="item.options.disabled || parentDisabled"
            :placeholder="item.options.placeholder"
            :allowClear="item.options.clearable"
            :maxLength="item.options.maxLength"
            v-model="scope.row[item.model]"
            @change="updateStatus(scope)"
          />

          <!-- 数字输入框 -->
          <a-input-number
            v-if="item.type === 'number'"
            :disabled="item.options.disabled || parentDisabled"
            :min="
              item.options.min || item.options.min === 0
                ? item.options.min
                : -Infinity
            "
            :max="
              item.options.max || item.options.max === 0
                ? item.options.max
                : Infinity
            "
            :step="item.options.step"
            :placeholder="item.options.placeholder"
            v-model="scope.row[item.model]"
            @change="updateStatus(scope)"
            style="width: 100%"
          />

          <!-- 文本域 -->
          <a-textarea
            v-if="item.type === 'textarea'"
            :autoSize="{
              minRows: item.options.minRows,
              maxRows: item.options.maxRows,
            }"
            :disabled="item.options.disabled || parentDisabled"
            :placeholder="item.options.placeholder"
            :allowClear="item.options.clearable"
            :maxLength="item.options.maxLength"
            :rows="4"
            v-model="scope.row[item.model]"
            @change="updateStatus(scope)"
          />

          <!-- 单选框 -->
          <a-radio-group
            v-else-if="item.type === 'radio'"
            :options="
              !item.options.dynamic
                ? item.options.options
                : dynamicData[record.model + '.' + item.key]
                ? dynamicData[record.model + '.' + item.key]
                : []
            "
            :disabled="item.options.disabled || parentDisabled"
            :placeholder="item.options.placeholder"
            v-model="scope.row[item.model]"
            :checked="value"
            @change="updateStatus(scope)"
          />

          <!-- 多选框 -->
          <a-checkbox-group
            v-else-if="item.type === 'checkbox'"
            :options="
              !item.options.dynamic
                ? item.options.options
                : dynamicData[record.model + '.' + item.key]
                ? dynamicData[record.model + '.' + item.key]
                : []
            "
            :disabled="item.options.disabled || parentDisabled"
            :placeholder="item.options.placeholder"
            v-model="scope.row[item.model]"
            @change="updateStatus(scope)"
          />

          <!-- 下拉框 -->
          <a-select
            v-else-if="item.type === 'select'"
            :mode="item.options.multiple ? 'multiple' : ''"
            :placeholder="item.options.placeholder"
            allow-clear
            v-model="scope.row[item.model]"
            :disabled="item.options.disabled || parentDisabled"
            @change="updateStatus(scope)"
          >
            {{ dynamicData[record.model + "." + item.model] }}
            <a-select-option
              v-for="(sitem, sindex) in !item.options.dynamic
                ? item.options.options
                : dynamicData[record.model + '.' + item.model]
                ? dynamicData[record.model + '.' + item.model]
                : []"
              :key="sindex"
              :value="sitem.value"
            >
              {{ sitem.label }}
            </a-select-option>
          </a-select>

          <!-- 时间 -->
          <a-date-picker
            :show-time="{
              disabled: item.options.format.includes('HH'),
              format: item.options.format.replace('YYYY-MM-DD '),
            }"
            :placeholder="item.placeholder"
            :format="item.options.format"
            allow-clear
            v-model="scope.row[item.model]"
            v-else-if="item.type == 'date'"
            @change="updateStatus(scope)"
          />

          <a-range-picker
            :mode="[item.options.mode, item.options.mode]"
            :show-time="{
              disabled: item.options.format.includes('HH'),
              format: item.options.format.replace('YYYY-MM-DD '),
            }"
            :placeholder="[item.placeholder, item.placeholder]"
            :format="item.options.format"
            allow-clear
            v-model="scope.row[item.model]"
            v-else-if="item.type == 'date' && item.options.range"
            @change="
              (e) => {
                item.value = e;
                item.open = false;
              }
            "
            @panelChange="
              (e) => {
                item.value = e;
                item.open = false;
              }
            "
            @openChange="
              (e) => {
                item.open = e;
              }
            "
          />

          <!-- 滑块 -->
          <div
            v-else-if="item.type === 'slider'"
            :style="`width:${record.options.width}`"
            class="slider-box"
          >
            <div class="slider">
              <a-slider
                :disabled="item.options.disabled || parentDisabled"
                :min="item.options.min"
                :marks="{
                  0: '开始',
                  50: '测试',
                  70: '试运行',
                  100: '结束',
                }"
                :vertical="item.options.vertical"
                :max="item.options.max"
                :step="item.options.step"
                v-model="scope.row[item.model]"
                @change="updateStatus(scope)"
              />
            </div>
            <div class="number" v-if="item.options.showInput">
              <a-input-number
                style="width: 100%"
                :disabled="item.options.disabled || parentDisabled"
                :min="item.options.min"
                :max="item.options.max"
                :step="item.options.step"
                v-model="scope.row[item.model]"
                @change="updateStatus(scope)"
              />
            </div>
          </div>

          <!-- 文本 -->
          <div v-else-if="item.type === 'text'">
            <div :style="{ textAlign: item.options.textAlign }">
              <label
                :class="{
                  'ant-form-item-required': item.options.showRequiredMark,
                }"
                :style="{
                  fontFamily: item.options.fontFamily,
                  fontSize: item.options.fontSize,
                  color: item.options.color,
                }"
                v-text="item.label"
              ></label>
            </div>
          </div>

          <!-- html -->
          <div
            v-else-if="item.type === 'html'"
            v-html="item.options.defaultValue"
          ></div>

          <!-- dataShow -->
          <div v-else-if="item.type === 'dataShow'">
            <label
              :style="{
                'font-size': item.options.label.fontSize,
                color: item.options.label.color,
                'font-family': item.options.label.fontFamily,
              }"
              v-if="
                item.options.type == undefined || item.options.type == 'label'
              "
              >{{ scope.row[item.model] }}</label
            >

            <template
              v-if="isJSON(scope.row[item.model]) && item.options.type == 'img'"
            >
              <eip-file :files="JSON.parse(row[item.model])"></eip-file>
            </template>

            <vue-barcode
              :value="scope.row[item.model]"
              v-if="item.options.type == 'barcode'"
            />
            <vue-qr
              ref="qrCode"
              :text="scope.row[item.model]"
              width="100"
              height="100"
              v-if="item.options.type == 'qrCode'"
            />
          </div>

          <!-- 人员选择 -->
          <template v-else-if="item.type === 'user'">
            <a-tooltip
              :title="user.name"
              v-for="(user, index) in scope.row[item.model]"
              :key="index"
            >
              <a-tag
                @close="chosenUserDelete(scope, index, item)"
                :closable="true"
              >
                <img
                  style="width: 23px; height: 23px; border-radius: 50%"
                  v-real-img="user.headImage"
                />
                <label style="vertical-align: middle">{{ user.name }}</label>
              </a-tag>
            </a-tooltip>
            <a-tooltip :title="item.options.placeholder">
              <div @click="chosenUser(scope, item)" class="addBtn">
                <a-icon type="plus" @click="chosenUser(scope, item)" />
              </div>
            </a-tooltip>
          </template>

          <!-- 组织选择 -->
          <template v-else-if="item.type === 'organization'">
            <a-tooltip
              :title="organization.name"
              v-for="(organization, index) in scope.row[item.model]"
              :key="index"
            >
              <a-tag
                @close="chosenUserDelete(scope, index, item)"
                :closable="true"
              >
                <div class="controlTags">
                  <div class="departWrap">
                    <a-icon style="font-size: 12px" type="apartment" />
                  </div>
                </div>
                <label style="vertical-align: middle">{{
                  organization.name
                }}</label>
              </a-tag>
            </a-tooltip>
            <a-tooltip :title="item.options.placeholder">
              <div @click="chosenOrganization(scope, item)" class="addBtn">
                <a-icon type="plus" @click="chosenOrganization(scope, item)" />
              </div>
            </a-tooltip>
          </template>
        </template>

        <template #default="{ row }">
          <!-- switch -->
          <a-tag
            color="#2db7f5"
            v-if="item.type === 'switch' && row[item.model] == true"
          >
            {{ item.options.txt.yes }}</a-tag
          >
          <a-tag
            color="#f50"
            v-else-if="item.type === 'switch' && row[item.model] == false"
          >
            {{ item.options.txt.no }}</a-tag
          >

          <!-- 单选框 -->
          <span v-else-if="item.type === 'radio' && row[item.model]">{{
            item.options.options.filter((f) => f.value == row[item.model])
              .length > 0
              ? item.options.options.filter(
                  (f) => f.value == row[item.model]
                )[0].label
              : ""
          }}</span>

          <!-- 复选框 -->
          <span v-else-if="item.type === 'checkbox' && row[item.model]">{{
            item.options.options.filter((f) =>
              row[item.model].includes(f.value)
            ).length > 0
              ? item.options.options
                  .filter((f) => row[item.model].includes(f.value))
                  .map((m) => m.label)
                  .join(",")
              : ""
          }}</span>

          <!-- 下拉框 -->
          <span v-else-if="item.type === 'select' && row[item.model]">
            {{
              !item.options.dynamic
                ? item.options.options
                : dynamicData[record.model + "." + item.model]
                ? dynamicData[record.model + "." + item.model]
                : [].filter((f) => row[item.model].includes(f.value)).length > 0
                ? item.options.options
                    .filter((f) => row[item.model].includes(f.value))
                    .map((m) => m.label)
                    .join(",")
                : ""
            }}</span
          >

          <span v-else-if="item.type === 'dataShow'">
            <label
              :style="{
                'font-size': item.options.label.fontSize,
                color: item.options.label.color,
                'font-family': item.options.label.fontFamily,
              }"
              v-if="
                item.options.type == undefined || item.options.type == 'label'
              "
              >{{ row[item.model] }}</label
            >

            <template
              v-if="isJSON(row[item.model]) && item.options.type == 'img'"
            >
              <eip-file :files="JSON.parse(row[item.model])"></eip-file>
            </template>

            <!-- 条形码 -->
            <vue-barcode
              :value="row[item.model]"
              v-if="item.options.type == 'barcode'"
            />

            <!-- 二维码 -->
            <vue-qr
              ref="qrCode"
              :text="row[item.model]"
              width="100"
              height="100"
              v-if="item.options.type == 'qrCode'"
            />
          </span>

          <!-- 时间 -->
          <span v-else-if="item.type === 'date' && row[item.model]">
            {{ formatDate(row[item.model], item.options.format) }}
          </span>

          <!-- 用户 -->
          <template v-else-if="item.type === 'user' && row[item.model]">
            <a-tooltip
              :title="user.name"
              v-for="(user, index) in row[item.model]"
              :key="index"
            >
              <a-tag>
                <img
                  style="width: 23px; height: 23px; border-radius: 50%"
                  v-real-img="user.headImage"
                />
                <label style="vertical-align: middle">{{ user.name }}</label>
              </a-tag>
            </a-tooltip>
          </template>

          <!-- 组织架构 -->
          <template v-else-if="item.type === 'organization' && row[item.model]">
            <a-tooltip
              :title="organization.name"
              v-for="(organization, index) in row[item.model]"
              :key="index"
            >
              <a-tag>
                <div class="controlTags">
                  <div class="departWrap">
                    <a-icon style="font-size: 12px" type="apartment" />
                  </div>
                </div>
                <label style="vertical-align: middle">{{
                  organization.name
                }}</label>
              </a-tag>
            </a-tooltip>
          </template>

          <!-- 其他 -->
          <span v-else>{{ row[item.model] }}</span>
        </template>
      </vxe-column>

      <vxe-column
        v-if="(copy || del) && !disabled"
        title="操作"
        align="center"
        fixed="right"
        width="100px"
      >
        <template #default="{ row }">
          <a-tooltip title="复制添加" v-if="copy">
            <a-icon
              v-if="!disabled"
              type="copy-o"
              class="dynamic-opr-button"
              @click="copyDomain(row)"
            />
          </a-tooltip>
          <a-divider type="vertical" v-if="copy && del" />
          <a-popconfirm
            v-if="!disabled && del"
            title="确定删除数据?"
            ok-text="确定"
            cancel-text="取消"
            @confirm="removeDomain(row)"
          >
            <a-icon
              @click.stop=""
              class="dynamic-opr-button"
              type="minus-circle-o"
            />
          </a-popconfirm>
        </template>
      </vxe-column>
    </vxe-table>

    <a-modal
      v-if="chosen.modal.visible"
      v-drag
      :destroyOnClose="true"
      :maskClosable="false"
      :centered="chosen.modal.centered"
      :bodyStyle="{ padding: '1px', 'background-color': '#f0f2f5' }"
      :width="chosen.modal.width"
      :visible="chosen.modal.visible"
      :title="chosen.modal.title"
      @cancel="chosen.modal.visible = false"
    >
      <a-card class="margin-bottom-xs eip-admin-card-small">
        <eip-search
          :option="chosen.table.search.option"
          @search="chosenSearch"
        ></eip-search>
        <a-spin :spinning="chosen.table.loading">
          <vxe-table
            ref="batchTable"
            v-if="chosen.table.loaded"
            :column-config="{ isCurrent: true, isHover: true }"
            :row-config="{ isCurrent: true, isHover: true }"
            :seq-config="{
              startIndex:
                (chosen.table.page.param.current - 1) *
                chosen.table.page.param.size,
            }"
            :height="chosen.table.height"
            :data="chosen.table.data"
            @cell-dblclick="chosenCellDbClick"
            row-key
          >
            <template #loading>
              <a-spin></a-spin>
            </template>
            <template #empty>
              <eip-empty />
            </template>
            <vxe-column width="60">
              <template #default>
                <span class="drag-btn">
                  <a-icon type="drag" />
                </span>
              </template>
            </vxe-column>
            <vxe-column
              :type="chosen.modal.multiple ? 'checkbox' : 'radio'"
              width="60"
              align="center"
              fixed="left"
            >
              <template #header="{ checked, indeterminate }">
                <span @click.stop="$refs.batchTable.toggleAllCheckboxRow()">
                  <a-checkbox :indeterminate="indeterminate" :checked="checked">
                  </a-checkbox>
                </span>
              </template>
              <template #checkbox="{ row, checked, indeterminate }">
                <span @click.stop="$refs.batchTable.toggleCheckboxRow(row)">
                  <a-checkbox :indeterminate="indeterminate" :checked="checked">
                  </a-checkbox>
                </span>
              </template>
              <template #radio="{ row, checked }">
                <span @click.stop="$refs.batchTable.toggleCheckboxRow(row)">
                  <a-radio :checked="checked"> </a-radio>
                </span>
              </template>
            </vxe-column>
            <vxe-column type="seq" title="序号" width="60"></vxe-column>
            <vxe-column
              v-for="(item, i) in chosen.table.columns"
              :key="i"
              :field="item.field"
              :title="item.title"
              :width="item.width"
              :align="item.align"
              :sortable="item.sort"
              min-width="150"
              showOverflow="ellipsis"
            >
              <template v-slot="{ row }">
                <span v-if="item.style && item.style.length > 0">
                  <template v-for="(items, index) in item.style">
                    <a-tag
                      :key="index"
                      :color="items.custom ? items.custom : items.presets"
                      v-if="items.value == row[item.field]"
                    >
                      {{ items.content }}
                    </a-tag>
                  </template>
                </span>
                <template v-if="isJSON(row[item.field])">
                  <eip-file :files="JSON.parse(row[item.field])"></eip-file>
                </template>
                <span v-else>{{ row[item.field] }}</span>
              </template>
            </vxe-column>
          </vxe-table>

          <a-pagination
            v-if="chosen.table.page.param.isPaging"
            class="padding-top-sm float-right"
            v-model="chosen.table.page.param.current"
            size="small"
            show-size-changer
            show-quick-jumper
            :page-size-options="chosen.table.page.sizeOptions"
            :show-total="(total) => `共 ${total} 条`"
            :page-size="chosen.table.page.param.size"
            :total="chosen.table.page.total"
            @change="initData"
            @showSizeChange="chosenSizeChange"
          ></a-pagination>
        </a-spin>
      </a-card>

      <template slot="footer">
        <a-space>
          <a-button key="back" @click="chosen.modal.visible = false">
            取消
          </a-button>
          <a-button
            :loading="chosen.loading"
            key="submit"
            @click="chosenConfirm"
            type="primary"
          >
            确定
          </a-button></a-space
        >
      </template>
    </a-modal>

    <eip-user
      v-if="user.visible"
      :visible.sync="user.visible"
      :topOrg="user.topOrg"
      :specificOrg="user.specificOrg"
      :chosen="user.chosen"
      :multiple="user.multiple"
      :title="user.placeholder"
      @ok="chosenUserConfirm"
    ></eip-user>

    <eip-organization
      v-if="organization.visible"
      :visible.sync="organization.visible"
      :chosen="organization.chosen"
      :multiple="organization.multiple"
      :range="organization.range"
      :title="organization.placeholder"
      @ok="chosenOrganizationConfirm"
    ></eip-organization>
  </a-form-model>
</template>

<script>
import { query } from "@/services/system/agile/components/control/datalist";
import { importDataGet } from "@/services/system/agile/run/edit";
import { selectTableRow, deleteConfirm } from "@/utils/util";
import Sortable from "sortablejs";
// 条形码
import VueBarcode from "vue-barcode";
//二维码
import VueQr from "vue-qr";
export default {
  name: "KBatch",
  props: [
    "record",
    "value",
    "dynamicData",
    "config",
    "parentDisabled",
    "formValue",
  ],
  components: {
    VueQr,
    VueBarcode,
  },
  watch: {
    value: {
      // value 需要深度监听及默认先执行handler函数
      handler(val) {
        this.dynamicValidateForm.domains = val || [];
      },
      immediate: true,
      deep: true,
    },
  },
  data() {
    return {
      tooltipConfig: {
        showAll: true,
        enterable: true,
        contentMethod: ({ type, column, row, items, _columnIndex }) => {
          if (type == "header") {
            return "";
          }
          var fieldFilter = this.$utils.find(
            this.record.list,
            (f) => f.model == column.field
          );
          if (
            fieldFilter &&
            (fieldFilter.type == "user" || fieldFilter.type == "organization")
          ) {
            return "";
          }
          const { field } = column;
          return null;
        },
      },
      dynamicValidateForm: {
        domains: [],
      },
      copy: this.record.options.copy,
      del: this.record.options.delete,
      button: {
        trigger: {
          option: {
            columns: [],
            search: {},
            source: {},
            map: [],
          },
        },
      }, //当前选中按钮
      loading: false,
      buttonChosenData: false, //是否为按钮触发选择数据
      buttonCloseScope: null,
      chosen: {
        modal: {
          bodyStyle: {
            padding: "2px",
          },
          visible: false,
          title: "",
          width: 800,
          top: "20px",
          centered: false,
          multiple: true,
        },

        //表格配置信息
        table: {
          page: {
            param: {
              current: 1,
              size: this.eipPage.size,
              sord: "desc",
              sidx: "",
              filters: "",
              isPaging: false, //是否分页
              fields: "",
              dataSourceId: undefined,
              inParams: [],
              formValue: "",
            },
            total: 0,
            sizeOptions: this.eipPage.sizeOptions,
          },
          sidx: [],
          height: 200,
          loading: false,
          loaded: false, //配置是否加载完成
          data: [],
          original: [],
          search: {
            option: {
              num: 6,
              config: [],
            },
          },
          columns: [],
        },

        loading: false,
      },

      validRules: {},
      footerData: [[]],

      user: {
        visible: false,
        topOrg: "",
        placeholder: "",
        specificOrg: "",
        chosen: [],
        multiple: false,
        item: null,
        scope: null,
      },

      organization: {
        visible: false,
        placeholder: "",
        chosen: [],
        range: null,
        multiple: false,
        item: null,
        scope: null,
      },
    };
  },
  mounted() {
    this.$refs[this.record.model].connect(this.$refs.batchToolbar);
  },
  computed: {
    disabled() {
      return this.record.options.disabled || this.parentDisabled;
    },
  },
  created() {
    this.ruleInit();
    this.footerInit();
  },
  methods: {
    /**
     * 用户选择
     */
    chosenUser(scope, item) {
      this.user.visible = true;
      this.user.placeholder = item.options.placeholder;
      this.user.multiple = item.options.multiple;
      this.user.chosen = scope.row[item.model];
      this.user.item = item;
      this.user.scope = scope;
    },

    /**
     * 删除用户
     */
    chosenUserDelete(scope, spIndex, item) {
      this.updateStatus(scope.row[item.model].splice(spIndex, 1));
    },

    /**
     * 选择用户确定
     */
    chosenUserConfirm(data) {
      var users = [];
      data.forEach((item) => {
        users.push({
          id: item.userId,
          name: item.name,
          headImage: item.headImage,
        });
      });
      this.user.scope.row[this.user.item.model] = users;
      this.updateStatus(this.user.scope);
    },

    /**
     * 组织选择
     */
    chosenOrganization(scope, item) {
      this.organization.visible = true;
      this.organization.placeholder = item.options.placeholder;
      this.organization.multiple = item.options.multiple;
      this.organization.range = item.options.range;
      this.organization.chosen = scope.row[item.model];
      this.organization.item = item;
      this.organization.scope = scope;
    },

    /**
     * 删除组织
     */
    chosenOrganizationDelete(scope, spIndex, item) {
      this.updateStatus(scope.row[item.model].splice(spIndex, 1));
    },

    /**
     * 选择组织确定
     */
    chosenOrganizationConfirm(data) {
      var organizations = [];
      data.forEach((item) => {
        organizations.push({
          id: item.value,
          name: item.title,
        });
      });
      this.organization.scope.row[this.organization.item.model] = organizations;
      this.updateStatus(this.organization.scope);
    },

    /**
     * 时间格式化
     * @param {*} value
     * @param {*} format
     */
    formatDate(value, format) {
      if (typeof value == "string") {
        return value;
      }
      return value ? value.format(format) : null;
    },

    /**
     *返回底部数据
     */
    returnDataFooter() {
      return this.footerData;
    },

    /**
     * 初始化底部
     */
    footerInit() {
      var number = 0;
      if (this.record.options.canDrag) {
        number += 1;
      }
      if (this.record.options.multiple) {
        number += 1;
      }
      if (!this.record.options.hideSequence) {
        number += 1;
      }
      var footer = [];
      for (let index = 0; index < number - 1; index++) {
        footer.push("");
      }
      let that = this;
      if (typeof that.record.options.footerOptions != "undefined") {
        if (number != 0) {
          footer.push(this.record.options.footerName);
        }
        this.record.list.forEach((item, index) => {
          var isShow = that.record.options.footerOptions.filter(
            (f) => f.model == item.model
          );
          if (isShow.length > 0) {
            var option = isShow[0];
            if (option.type == "sum") {
              var sum = that.$utils.sum(
                that.dynamicValidateForm.domains,
                option.model
              );
              footer.push(sum);
            } else if (option.type == "avg") {
              var avg = that.$utils.mean(
                that.dynamicValidateForm.domains,
                option.model
              );
              footer.push(avg);
            }
          } else {
            footer.push("");
          }
        });
      }

      this.footerData = [footer];
    },

    /**
     *
     */
    rowDrop() {
      let that = this;
      this.$nextTick(() => {
        const xTable = that.$refs[that.record.model];
        if (xTable) {
          Sortable.create(
            xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
            {
              handle: ".drag-btn",
              onEnd: ({ newIndex, oldIndex }) => {
                const currRow = that.dynamicValidateForm.domains.splice(
                  oldIndex,
                  1
                )[0];
                that.dynamicValidateForm.domains.splice(newIndex, 0, currRow);
              },
            }
          );
        }
      });
    },
    /**
     * 下载文件
     * @param {*} file
     */
    downloadFile(file) {
      let that = this;
      that.$message.loading("下载中,请稍等...", 0);
      fetch(file.url)
        .then((response) => {
          response.blob().then((blob) => {
            that.$XSaveFile({
              filename: file.name.split(".")[0],
              type: file.name.substring(file.name.lastIndexOf(".") + 1),
              content: blob,
            });
            that.$message.destroy();
          });
        })
        .catch((err) => {
          that.$message.destroy();
          that.$message.error(err);
        });
    },

    /**
     * 是否为Json
     * @param {*} str
     */
    isJSON(str) {
      if (typeof str == "string") {
        try {
          var obj = JSON.parse(str);
          if (typeof obj == "object" && obj) {
            return true;
          } else {
            return false;
          }
        } catch (e) {
          return false;
        }
      }
    },
    /**
     * 规则
     */
    ruleInit() {
      let that = this;
      this.record.list
        .filter((item) => !item.options.hidden)
        .forEach((item) => {
          if (item.rules) {
            that.validRules[item.model] = item.rules;
          }
          that.rowDrop();
        });
    },
    /**
     * 按钮事件
     */
    buttonEvent(item) {
      this.button = item;

      switch (item.trigger.type) {
        case "add":
          this.addDomain();
          break;
        case "reset":
          this.removeAllDomain();
          break;
        case "delete":
          this.deleteDomain();
          break;
        case "chosen": //弹出选择数据
          this.chosenClick();
          break;
        case "export":
          this.exportData();
          break;
        case "customer":
          break;
      }
    },

    /**
     * 导出数据
     */
    exportData() {
      var columns = this.record.list
        .filter((item) => !item.options.hidden)
        .map((m) => m.model);
      this.$refs[this.record.model].exportData({
        filename: this.record.label,
        type: "csv",
        columnFilterMethod({ column }) {
          return columns.includes(column.property);
        },
      });
    },
    /**
     * 解析excel
     * @param {*} file
     */
    importDataAnalysis(file) {
      let that = this;
      const formData = new FormData();
      that.$message.loading({
        content: "解析中,请稍等...",
        duration: 0,
      });

      formData.append("Files", file.file);
      importDataGet(formData)
        .then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            var domainsData = [];
            var onlys = [];
            result.data.forEach((item) => {
              var data = {};
              that.record.list.forEach((litem) => {
                var defaultValue = litem.options.defaultValue;
                data[litem.model] = item[litem.label]
                  ? item[litem.label]
                  : defaultValue
                  ? defaultValue
                  : null;
                //判断是否具有唯一判断
                if (litem.options.only) {
                  var check = that.onlyCheck(litem.model, data[litem.model]);
                  if (check) {
                    onlys.push(
                      litem.label + ":" + data[litem.model] + "已存在"
                    );
                  }
                }
              });
              domainsData.push(data);
            });
            if (onlys.length == 0) {
              that.dynamicValidateForm.domains =
                that.dynamicValidateForm.domains.concat(domainsData);
              that.handleInput();
            } else {
              this.$notification.error({
                message: this.record.label,
                description: (h) => {
                  return onlys.map((m) => h("div", m));
                },
              });
            }
            that.loading = false;
            that.$message.destroy();
          } else {
            that.$message.destroy();
            that.$message.error("解析有误");
          }
        })
        .catch(() => {
          that.$message.destroy();
          that.$message.error("上传出错");
        });
    },

    /**
     * 导入文件
     * @param {*} file
     */
    importData(file) {
      const types = file.file.name.split(".")[1]; //获取文件后缀
      const fileType = ["xlsx", "xls"].some((item) => item === types);
      if (!fileType) {
        this.$message.error("格式错误！请重新选择");
        return;
      }
      this.loading = true;
      this.importDataAnalysis(file);
    },

    /**
     * 唯一值判断
     */
    onlyCheck(key, value) {
      return (
        this.dynamicValidateForm.domains.filter((f) => f[key] == value).length >
        0
      );
    },

    /**
     * 清空
     */
    removeAllDomain() {
      let that = this;
      deleteConfirm(
        "清空数据",
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          that.dynamicValidateForm.domains = [];
          that.$loading.hide();
        },
        that
      );
    },

    /**
     * 删除选中数据
     */
    deleteDomain() {
      let that = this;
      selectTableRow(
        that.$refs[this.record.model],
        function (rows) {
          //提示是否删除
          deleteConfirm(
            "确定删除选中数据",
            function () {
              that.$loading.show({ text: that.eipMsg.delloading });
              rows.forEach((item) => {
                const index = that.dynamicValidateForm.domains.indexOf(item);
                if (index !== -1) {
                  that.dynamicValidateForm.domains.splice(index, 1);
                }
              });
              that.$loading.hide();
            },
            that
          );
        },
        that,
        false
      );
    },

    /**
     *
     */
    chosenClick() {
      this.buttonChosenData = true;
      this.chosen.modal.visible = false;
      this.chosen.modal.title = this.record.label;
      this.chosen.modal.width = this.button.trigger.option.width + "px";
      this.chosen.modal.centered = this.button.trigger.option.centered;
      this.chosen.modal.multiple = this.button.trigger.option.multiple;
      this.chosen.table.height = this.button.trigger.option.height + "px";
      this.chosen.table.page.param.isPaging =
        this.button.trigger.option.isPaging;

      this.chosen.table.page.param.dataSourceId =
        this.button.trigger.option.dynamicConfig.dataSourceId;
      this.chosen.table.page.param.inParams = JSON.stringify(
        this.button.trigger.option.dynamicConfig.inParams
      );
      this.chosen.table.page.param.formValue = JSON.stringify(this.formValue);

      this.chosenConfigInit();
    },

    /**
     * 数据选择
     */
    chosenDatalistClick(item, scope) {
      this.buttonChosenData = false;
      this.buttonCloseScope = scope;
      this.chosen.modal.visible = false;
      this.chosen.modal.title = item.label;
      this.chosen.modal.width = item.options.dialog.width + "px";
      this.chosen.modal.centered = item.options.dialog.centered;
      this.chosen.modal.multiple = false;
      this.chosen.table.height = item.options.dialog.height + "px";
      this.chosen.table.page.param.isPaging = item.options.isPaging;
      this.chosen.table.page.param.table = item.options.source.name;

      this.button.trigger.option.search = item.options.search;
      this.button.trigger.option.columns = item.options.columns;
      this.button.trigger.option.source = item.options.source;
      this.button.trigger.option.map = item.options.map;

      this.chosenConfigInit();
    },

    /**
     * 选择数据源初始化
     */
    chosenConfigInit() {
      let that = this;
      that.chosen.table.loading = true;
      that.initSearch();
      that.initColumns();
      that.initData();
      that.chosen.table.loaded = true;
      that.chosen.modal.visible = true;
    },

    /**
     * 列表初始化
     */
    chosenSearch(filters) {
      let that = this;
      this.chosen.table.page.param.current = 1;
      this.chosen.table.page.param.filters = filters;
      //判断是否不分页
      if (this.chosen.table.page.param.isPaging) {
        this.initData();
      } else {
        //本地搜索
        var filtersJson = JSON.parse(filters);
        if (filtersJson.rules.length > 0) {
          const filterName = that.$utils
            .toValueString(filtersJson.rules[0].data)
            .trim()
            .toLowerCase();
          if (filterName) {
            const filterRE = new RegExp(filterName, "gi");
            const searchProps = filtersJson.rules.map((m) => m.field);
            const rest = that.chosen.table.original.filter((item) =>
              searchProps.some(
                (key) =>
                  that.$utils
                    .toValueString(item[key])
                    .toLowerCase()
                    .indexOf(filterName) > -1
              )
            );
            that.chosen.table.data = rest.map((row) => {
              const item = Object.assign({}, row);
              searchProps.forEach((key) => {
                item[key] = that.$utils
                  .toValueString(item[key])
                  .replace(filterRE, (match) => `${match}`);
              });
              return item;
            });
          } else {
            that.chosen.table.data = that.chosen.table.original;
          }
        } else {
          that.chosen.table.data = that.chosen.table.original;
        }
      }
    },

    /**
     * 确认
     */
    chosenConfirm() {
      if (this.buttonChosenData) {
        this.chosenConfirmButton();
      } else {
        this.chosenConfirmDataList();
      }
    },
    /**
     * 列表选择数据确定
     */
    chosenConfirmDataList() {
      var map = this.button.trigger.option.map.filter((f) => f.mapcolumn);
      let that = this;
      if (map.length > 0) {
        this.chosen.loading = true;
        var row = [this.$refs.batchTable.getRadioRecord()];
        row.forEach((item, index) => {
          that.record.list.forEach((mitem) => {
            var getMap = map.filter((f) => f.mapcolumn == mitem.model);
            var defaultValue = mitem.options.defaultValue;
            that.buttonCloseScope.row[mitem.model] =
              getMap.length > 0
                ? item[getMap[0].name]
                : defaultValue
                ? defaultValue
                : null;
          });
          that.dynamicValidateForm.domains.forEach((ditem) => {
            if (ditem.key == that.buttonCloseScope.row.key) {
              ditem = that.buttonCloseScope.row;
            }
          });
          that.updateStatus(that.buttonCloseScope);
        });
        this.handleInput();
        this.chosen.loading = false;
        this.chosen.modal.visible = false;
      } else {
        this.$message.error("请确认当前字段已映射");
      }
    },

    /**
     * 按钮选择数据确定
     */
    chosenConfirmButton() {
      var map = this.button.trigger.option.map.filter((f) => f.mapcolumn);
      let that = this;
      if (map.length > 0) {
        this.chosen.loading = true;
        if (this.record.options.multiple) {
          this.record.options.selectRows =
            this.$refs.batchTable.getCheckboxRecords();
        } else {
          var row = this.$refs.batchTable.getRadioRecord();
          this.record.options.selectRows = row ? [row] : [];
        }
        var domainsData = [];
        var onlys = [];
        this.record.options.selectRows.forEach((item, index) => {
          var obj = {};
          that.record.list.forEach((mitem) => {
            var mitemModel = that.record.model + "." + mitem.model;
            var getMap = map.filter((f) => f.mapcolumn == mitemModel);
            var defaultValue = mitem.options.defaultValue;
            obj[mitem.model] =
              getMap.length > 0
                ? item[getMap[0].name]
                : defaultValue
                ? defaultValue
                : null;

            //判断是否具有唯一判断
            if (mitem.options.only) {
              var check = that.onlyCheck(mitem.model, obj[mitem.model]);
              if (check) {
                onlys.push(mitem.label + ":" + obj[mitem.model] + "已存在");
              }
            }
          });
          domainsData.push({
            ...obj,
            key: Date.now() + "" + index,
          });
        });
        if (onlys.length == 0) {
          that.dynamicValidateForm.domains =
            that.dynamicValidateForm.domains.concat(domainsData);
          this.handleInput();
        } else {
          this.$notification.error({
            message: this.record.label,
            description: (h) => {
              return onlys.map((m) => h("div", m));
            },
          });
        }
        this.chosen.loading = false;
        this.chosen.modal.visible = false;
      } else {
        this.$message.error("请确认当前字段已映射");
      }
    },

    /**
     *数量改变
     */
    chosenSizeChange(current, pageSize) {
      this.chosen.table.page.param.size = pageSize;
    },

    /**
     * 双击事件
     */
    chosenCellDbClick({
      row,
      rowIndex,
      $rowIndex,
      column,
      columnIndex,
      $columnIndex,
      $event,
    }) {
      if (this.record.options.rowDoubleClickConfirm) {
        this.record.options.selectRows = [row];
        var map = this.record.options.map.filter(
          (f) => f.mapcolumn == this.record.model
        );
        if (map.length > 0) {
          let value = row[map[0].name];
          this.$emit("change", value);
          this.modal.visible = false;
        }
      }
    },

    /**
     * 渲染列
     */
    initColumns() {
      let that = this;
      that.chosen.table.sidx = [];
      that.chosen.table.columns = [];
      var fields = [];
      that.button.trigger.option.columns.forEach((item) => {
        if (item.isShow) {
          var column = {
            field: item.name,
            title: item.title,
            align: item.align,
            sort: item.isSort,
            width: item.width,
            style: item.style,
          };
          that.chosen.table.columns.push(column);
          fields.push(item.name);
        }
        if (item.isSearch) {
          var have = fields.filter((f) => f == item.name).length;
          if (have == 0) {
            fields.push(item.name);
          }
        }
        if (item.sord) {
          that.chosen.table.sidx.push(item.name + " " + item.sordType);
        }
      });
      that.chosen.table.page.param.fields = fields.join(",");
    },

    /**
     * 初始化查询
     */
    initSearch() {
      let that = this;
      if (this.button.trigger.option.search.num) {
        that.chosen.table.search.option.num =
          this.button.trigger.option.search.num;
      }
      let config = [];
      this.button.trigger.option.search.columns.forEach((item) => {
        switch (item.type) {
          case "input":
            config.push({
              field: item.field,
              op: item.op,
              placeholder: item.placeholder,
              title: item.title,
              value: "",
              type: "text",
            });
            break;
        }
      });

      that.chosen.table.search.option.config = config;
    },

    /**
     * 初始化列表数据
     */
    async initData() {
      let that = this;
      that.chosen.table.loading = true;
      that.chosen.table.page.param.sidx = that.chosen.table.sidx.join(",");
      var result = await query(that.chosen.table.page.param);
      if (result.code == that.eipResultCode.success) {
        let queryData = result.data;
        that.chosen.table.data = queryData.data;
        if (that.chosen.table.page.param.isPaging) {
          that.chosen.table.page.total = queryData.count;
        }
        that.chosen.table.original = queryData.data;
      } else {
        that.$message.error(result.msg);
      }
      that.chosen.table.loading = false;
    },

    /**
     *数量改变
     */
    sizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
    },

    /**
     * 验证
     */
    async validationSubform() {
      var validate = await this.$refs[this.record.model].fullValidate(true);
      if (validate) {
        const msgList = [];
        Object.values(validate).forEach((errList) => {
          errList.forEach((params) => {
            const { rowIndex, column, rules } = params;
            rules.forEach((rule) => {
              msgList.push(
                `第 ${rowIndex + 1} 行 ${column.title} 校验错误：${
                  rule.message
                }\n`
              );
            });
          });
        });
        this.$notification.error({
          message: this.record.label,
          description: (h) => {
            return msgList.map((m) => h("div", m));
          },
        });
      }
      return typeof validate == "undefined";
    },

    /**
     * 重置表单
     */
    resetForm() {
      this.$refs.dynamicValidateForm.resetFields();
    },

    /**
     * 移除数据
     * @param {*} item
     */
    removeDomain(item) {
      const index = this.dynamicValidateForm.domains.indexOf(item);
      if (index !== -1) {
        this.dynamicValidateForm.domains.splice(index, 1);
      }
    },

    /**
     * 复制数据
     * @param {*} record
     */
    copyDomain(record) {
      const data = {};
      this.record.list.forEach((item) => {
        data[item.model] = record[item.model];
      });
      this.dynamicValidateForm.domains.push({
        ...data,
        key: Date.now(),
      });
      this.handleInput();
    },

    /**
     * 新增数据
     */
    addDomain() {
      const data = {};
      this.record.list.forEach((item) => {
        //判断类型
        data[item.model] = item.options.defaultValue;
      });

      this.dynamicValidateForm.domains.push({
        ...data,
        key: Date.now(),
      });
      this.handleInput();
    },

    /**
     * 更改值状态
     */
    updateStatus(scope) {
      this.$refs[this.record.model].updateStatus(scope);
      this.handleInput();
    },

    /**
     * 改变事件
     */
    handleInput() {
      this.footerInit();
      this.$emit("change", this.dynamicValidateForm.domains);
    },
  },
};
</script>
<style scoped lang="less">
/deep/ .ant-table-title {
  padding: 6px !important;
}

.dynamic-opr-button:last {
  margin-left: 0px;
}

.dynamic-opr-button {
  cursor: pointer;
  position: relative;
  top: 4px;
  font-size: 16px;
  color: #999;
  transition: all 0.3s;
  margin-left: 6px;
}

.dynamic-opr-button:hover {
  color: #e89;
}

.dynamic-opr-button[disabled] {
  cursor: not-allowed;
  opacity: 0.5;
}

.slider-box {
  display: flex;

  > .slider {
    flex: 1;
    margin-right: 16px;
  }

  > .number {
    width: 70px;
  }
}

.eip-table-file {
  width: 40px;
  height: 40px;
  padding: 2px;
  cursor: pointer;
}

.ant-tag {
  border-radius: 40px !important;
}

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
  margin: 0;
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
