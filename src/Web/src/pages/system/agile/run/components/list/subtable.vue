<template>
  <div class="card-container">
    <a-tabs size="small" type="card" v-if="table.columnLoadComplate">
      <a-tab-pane :key="sindex" v-for="(subtable, sindex) in item.subtable"
        :tab="table.subtableNames[subtable.id] + '（' + data.subtableDatas[subtable.id].length + '）'">
        <div class="padding-xs">
          <vxe-table :data="data.subtableDatas[subtable.id]" :height="height ? height : 300">
            <template #loading>
              <a-spin></a-spin>
            </template>

            <template #empty>
              <eip-empty />
            </template>
            <vxe-column type="seq" title="序号" width="60"></vxe-column>
            <vxe-column v-for="(sitem, i) in data.subtableColumns[sindex]  " :key="i" :field="sitem.field"
              :title="sitem.title" :width="sitem.width" :align="sitem.align" :sortable="sitem.sort"
              showOverflow="ellipsis">

              <template v-slot="{ row }">
                <span
                  v-if="sitem.style && sitem.style.length > 0 && row[sitem.fieldReplaceTxt] && (typeof (row[sitem.fieldReplaceTxt]) != 'number')">
                  <template v-for="(dvalue) in row[sitem.fieldReplaceTxt].split(',')  ">
                    <template v-for="(items, index) in sitem.style  ">
                      <a-tag :key="index" :color="items.presets ? items.presets : items.custom"
                        v-if="items.value == dvalue">
                        {{ items.content }}
                      </a-tag>
                    </template>
                  </template>
                </span>

                <template
                  v-else-if="sitem.style && sitem.style.length > 0 && (typeof (row[sitem.fieldReplaceTxt]) == 'number')"
                  v-for="(items, index) in sitem.style  ">
                  <a-tag :key="index" :color="items.presets ? items.presets : items.custom"
                    v-if="items.value == row[sitem.fieldReplaceTxt]">
                    {{ items.content }}
                  </a-tag>
                </template>

                <template v-else-if="sitem.format == 'File' && row[sitem.field]">
                  <eip-file :files="JSON.parse(row[sitem.field])"></eip-file>
                </template>

                <template v-else-if="sitem.format == 'User' && row[item.field]">
                  <span class="run-list-user">
                    <a-tag :closable="false" v-for="(user, index) in row[item.field].split(',')" :key="index">
                      <img class="img"
                        v-real-img="row[item.field + 'Header'] ? row[item.field + 'Header'].split(',')[index] : ''" />
                      <label style="vertical-align: middle;">{{ user }}</label>
                    </a-tag>
                  </span>
                </template>

                <template v-else-if="sitem.format == 'Organization' && row[item.field]">
                  <span class="run-list-user">
                    <a-tag :closable="false" v-for="(org, index) in row[item.field].split(',')" :key="index">
                      <span class="controlTags">
                        <span class="departWrap">
                          <a-icon style="font-size: 12px;" type="apartment" />
                        </span>
                      </span>
                      <label style="vertical-align: middle;">{{ org }}</label>
                    </a-tag>
                  </span>
                </template>

                <template v-else-if="item.field == 'CreateUserName' && row[item.field]">
                  <span class="run-list-user">
                    <a-tag :closable="false">
                      <img class="img" v-real-img="row['CreateUserIdHeader']" />
                      <label style="vertical-align: middle;">{{ row[item.field] }}</label>
                    </a-tag>
                  </span>
                </template>

                <template v-else-if="item.field == 'UpdateUserName' && row[item.field]">
                  <span class="run-list-user">
                    <a-tag :closable="false">
                      <img class="img" v-real-img="row['UpdateUserIdHeader']" />
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

                <template v-else-if="sitem.format == 'Map' && row[item.field]">
                  {{ JSON.parse(row[item.field]).address }}
                </template>

                <template v-else-if="sitem.format == 'Switch'">
                  <a-tag :color="row[item.field] == 1 ? '#3c9cff' : '#f56c6c'">
                    {{ row[item.field] == 1 ? '是' : '否' }}
                  </a-tag>
                </template>

                <template v-else-if="sitem.format == 'WebSite'">
                  <a :src="row[item.field]" target="_blank" />
                </template>
                <template v-else-if="sitem.format == 'QrCode'">
                  QrCode
                </template>

                <template v-else-if="sitem.format == 'BarCode'">
                  BarCode
                </template>
                <span v-else>{{ row[sitem.field] }} {{ item.format }}</span>


              </template>


            </vxe-column>
          </vxe-table>
        </div>
      </a-tab-pane>
    </a-tabs>
  </div>
</template>

<script>
export default {
  name: "subtable",
  data() {
    return {};
  },

  props: {
    table: {
      type: Object,
    },
    data: {
      type: Object,
    },
    item: {
      type: Object,
    },
    height: {
      type: Number
    }
  },
  mounted() {

  },
  methods: {
  },
};
</script>

<style lang="less" scoped>
.ant-tag {
  border-radius: 40px !important;
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
  height: 22px;
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
  height: 22px;
  justify-content: center;
  width: 22px;
}

/deep/ .ant-tabs-tabpane {
  padding: 1px !important;
}
</style>
