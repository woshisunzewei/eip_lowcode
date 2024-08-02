<template>
  <span>
    <span
      v-if="item.style && item.style.length > 0 && row[item.fieldReplaceTxt] && (typeof (row[item.fieldReplaceTxt]) != 'number')">
      <template v-for="(dvalue, dindex) in row[item.fieldReplaceTxt].split(',')">
        <template v-for="(items, index) in item.style">
          <a-tag :key="index" :color="items.presets ? items.presets : items.custom" v-if="items.value == dvalue">
            {{ items.content }}
          </a-tag>
        </template>
      </template>
    </span>

    <template v-else-if="item.style && item.style.length > 0 && (typeof (row[item.fieldReplaceTxt]) == 'number')"
      v-for="(items, index) in item.style">
      <a-tag :key="index" :color="items.presets ? items.presets : items.custom"
        v-if="items.value == row[item.fieldReplaceTxt]">
        {{ items.content }}
      </a-tag>
    </template>

    <!-- 值为空 -->
    <template v-else-if="item.style && item.style.length > 0 && row[item.fieldReplaceTxt] == null"
      v-for="(items, index) in item.style.filter(f => f.value == null)">
      <a-tag :key="index" :color="items.presets ? items.presets : items.custom"
        v-if="items.value == row[item.fieldReplaceTxt]">
        {{ items.content }}
      </a-tag>
    </template>

    <template v-else-if="item.format == 'File' && row[item.field]">
      <eip-file :files="JSON.parse(row[item.field])"></eip-file>
    </template>

    <template v-else-if="item.format == 'User' && row[item.field]">
      <span class="run-list-user">
        <a-tag :closable="false" v-for="(user, index) in row[item.field].split(',')" :key="index">
          <img class="img"
            v-real-img-user="row[item.field + 'Header'] ? row[item.field + 'Header'].split(',')[index] : ''" />
          <label style="vertical-align: middle;">{{ user }}</label>
        </a-tag>
      </span>
    </template>

    <template v-else-if="item.format == 'Organization' && row[item.field]">
      <span class="run-list-user">
        <a-tag @click="scriptClick(item, row)" :closable="false" v-for="(org, index) in row[item.field].split(',')"
          :key="index">
          <span class="controlTags">
            <span class="departWrap">
              <a-icon style="font-size: 12px;" type="apartment" />
            </span>
          </span>
          <label :style="{ 'cursor': 'pointer', 'text-decoration': item.script ? 'underline' : '' }"
            style="vertical-align: middle;">{{ org
            }}</label>
        </a-tag>
      </span>
    </template>

    <template v-else-if="item.format == 'Batch'">
      <a-tag @click="batchDetail()" color="#f2f2f2">
        <a-space style="color:#333;cursor: pointer;">
          <a-icon type="table" /><span>{{ row[item.field] }}</span>
        </a-space>
      </a-tag>
    </template>

    <template v-else-if="item.field == 'CreateUserName' && row[item.field]">
      <span class="run-list-user">
        <a-tag :closable="false">
          <img class="img" v-real-img-user="row['CreateUserIdHeader']" />
          <label style="vertical-align: middle;">{{ row[item.field] }}</label>
        </a-tag>
      </span>
    </template>

    <template v-else-if="item.field == 'UpdateUserName' && row[item.field]">
      <span class="run-list-user">
        <a-tag :closable="false">
          <img class="img" v-real-img-user="row['UpdateUserIdHeader']" />
          <label style="vertical-align: middle;">{{ row[item.field] }}</label>
        </a-tag>
      </span>
    </template>

    <template v-else-if="item.field == 'CreateOrganizationName' && row[item.field]">
      <span class="run-list-user">
        <a-tag :closable="false">
          <span class="controlTags">
            <span class="departWrap">
              <a-icon style="font-size: 12px;" type="apartment" />
            </span>
          </span>
          <label style="vertical-align: middle;">{{ row[item.field] }}</label>
        </a-tag>
      </span>
    </template>

    <template v-else-if="item.field == 'UpdateOrganizationName' && row[item.field]">
      <span class="run-list-user">
        <a-tag :closable="false" v-for="(org, index) in row[item.field].split(',')" :key="index">
          <span class="controlTags">
            <span class="departWrap">
              <a-icon style="font-size: 12px;" type="apartment" />
            </span>
          </span>
          <label style="vertical-align: middle;">{{ row[item.field] }}</label>
        </a-tag>
      </span>
    </template>

    <template v-else-if="item.format == 'Map' && row[item.field]">
      {{ JSON.parse(row[item.field]).address }}
    </template>

    <template v-else-if="item.format == 'Rate' && row[item.field]">
      <a-rate v-model="row[item.field]" disabled />
    </template>

    <template v-else-if="item.format == 'Switch'">
      <a-tag :color="row[item.field] == 1 ? '#3c9cff' : '#f56c6c'">
        {{ row[item.field] == 1 ? '是' : '否' }}
      </a-tag>
    </template>

    <template v-else-if="item.format == 'Image'">
      <eip-file v-if="row[item.field]" :files="[{ name: '图片.png', type: 'img', url: row[item.field] }]"></eip-file>
    </template>

    <template v-else-if="item.format == 'WebSite'">
      <a v-if="row[item.field]" :src="row[item.field]" target="_blank" />
    </template>

    <template v-else-if="item.format == 'QrCode'">
      <vue-qr v-if="row[item.field]" :text="row[item.field]" :correctLevel="item.options.qrCode.correctLevel"
        :colorLight="item.options.qrCode.colorLight" :colorDark="item.options.qrCode.colorDark"
        :backgroundColor="item.options.qrCode.backgroundColor" :size="item.options.qrCode.size"
        :margin="item.options.qrCode.margin" />
    </template>

    <template v-else-if="item.format == 'BarCode'">
      <vue-barcode v-if="row[item.field]" :value="row[item.field]" :fontOptions="item.options.barCode.fontOptions"
        :font="item.options.barCode.font" :textPosition="item.options.barCode.textPosition"
        :format="item.options.barCode.format" :displayValue="item.options.barCode.displayValue"
        :height="item.options.barCode.height" :textAlign="item.options.barCode.textAlign"
        :width="item.options.barCode.width" :textMargin="item.options.barCode.textMargin"
        :fontSize="item.options.barCode.fontSize" :background="item.options.barCode.background"
        :lineColor="item.options.barCode.lineColor" :margin="item.options.barCode.margin" />
    </template>
    <template v-else-if="item.format == 'Label'">
      <label :style="{
        'font-size': item.options.label.fontSize,
        'color': item.options.label.color,
        'font-family': item.options.label.fontFamily
      }" v-html="row[item.field]"></label>
    </template>

    <template
      v-else-if="['YYYY', 'YYYYMM', 'YYYYMMDD', 'YYYYMMDDHH', 'YYYYMMDDHHMM', 'YYYYMMDDHHMMSS', 'HHMMSS'].includes(item.format)">
      {{ formatTime(row[item.field], item.format) }}
    </template>

    <template v-else-if="item.format == 'District' && row[item.field]">
      <a-tag>{{ row[item.field] }}</a-tag>
    </template>

    <template v-else-if="item.format == 'CorrelationRecord'">
      <a-space v-if="row[item.field]">
        <a-button type="primary" size="small" v-for="(data, index) in row[item.field].split(',')" :key="index">
          {{ data }}
        </a-button>
      </a-space>
    </template>

    <span @click="scriptClick(item, row)"
      :style="{ 'cursor': 'pointer', 'text-decoration': item.script ? 'underline' : '' }" v-else
      v-html="row[item.field]">
    </span>

  </span>
</template>

<script>
// 条形码
import VueBarcode from 'vue-barcode';
//二维码
import VueQr from 'vue-qr'
export default {
  components: {
    VueQr,
    VueBarcode
  },
  name: "list-content",
  data() {
    return {
    };
  },

  props: {
    row: {
      type: Object,
    },
    item: {
      type: Object,
    }
  },
  mounted() {

  },
  methods: {
    /**
     * 时间格式化
     */
    formatTime(time, format) {
      switch (format) {
        case "YYYY":
          return this.$utils.toDateString(time, "yyyy");
        case "YYYYMM":
          return this.$utils.toDateString(time, "yyyy-MM");
        case "YYYYMMDD":
          return this.$utils.toDateString(time, "yyyy-MM-dd");
        case "YYYYMMDDHH":
          return this.$utils.toDateString(time, "yyyy-MM-dd HH");
        case "YYYYMMDDHHMM":
          return this.$utils.toDateString(time, "yyyy-MM-dd HH:mm");
        case "YYYYMMDDHHMMSS":
          return this.$utils.toDateString(time, "yyyy-MM-dd HH:mm:ss");
        case "HHMMSS":
          return this.$utils.toDateString(time, "HH:mm:ss");
        default:
          return time;
      }
    },
    /**
     * 组织架构点击
     * @param {*} item 
     */
    scriptClick(item, row) {
      let that = this;
      if (item.script) {
        eval(item.script);
      }
    },
    /**
     * 子表详情
     */
    batchDetail() {
      var data = {
        row: this.row,
        item: this.item
      }
      this.$emit('batchDetail', data)
    },

  },
};
</script>

<style lang="less" scoped>
.ant-tag {
  border-radius: 40px !important;
}

.img {
  width: 23px;
  height: 23px;
  border-radius: 50%
}

.card-container {
  background: #f5f5f5;
  overflow: hidden;
}

.card-container>.ant-tabs-card>.ant-tabs-content {
  height: 120px;
  margin-top: -16px;
}

.card-container>.ant-tabs-card>.ant-tabs-content>.ant-tabs-tabpane {
  background: #fff;
  padding: 16px;
}

.card-container>.ant-tabs-card>.ant-tabs-bar {
  border-color: #fff;
}

.card-container>.ant-tabs-card>.ant-tabs-bar .ant-tabs-tab {
  border-color: transparent;
  background: transparent;
  border: none !important;
}

/deep/ .ant-tabs.ant-tabs-card .ant-tabs-card-bar .ant-tabs-tab {
  border: none !important;
}

.card-container>.ant-tabs-card>.ant-tabs-bar .ant-tabs-tab-active {
  border-color: #fff;
  background: #fff;
}

.eip-table-file {
  width: 40px;
  height: 40px;
  padding: 2px;
  cursor: pointer
}

/deep/ .run-list-user .ant-tag {
  cursor: pointer;
  padding: 0 7px 0 0 !important;
  border-radius: 50px !important;
}

/deep/ .run-list-user .ant-tag label {
  font-size: 12px;
  margin-left: 4px;
}

.controlTags {
  align-items: center;
  border-color: @primary-color;
  display: inline-flex;
  height: 23px;
  max-width: 100%;
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

.hrefColor {
  color: @primary-color;
}
</style>
