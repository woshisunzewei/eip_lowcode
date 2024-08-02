<template>
  <div>

    <a-card class="eip-admin-card-small" size="small">
      <div slot="title">
        <eip-toolbar @map="districtMapShow" @onload="toolbarOnload"></eip-toolbar>
      </div>
      <div slot="extra">
        <vxe-toolbar style="height: 30px" ref="toolbar" custom print export
          :refresh="{ query: tableInit }"></vxe-toolbar>
      </div>

      <vxe-table :loading="table.loading" ref="table" id="systemdistrictlist" :radio-config="{
          trigger: 'row',
          highlight: true,
          strict: false,
        }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data"
        :tree-config="{ transform: true, rowField: 'districtId', parentField: 'parentId', lazy: true, hasChild: 'hasChild', loadMethod: loadChildrenMethod, trigger: 'row' }">
        <template #loading>
          <a-spin></a-spin>
        </template>
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="radio" width="60" align="center"></vxe-column>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column field="name" title="名称" tree-node min-width="120" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="districtId" title="代码" showOverflow="ellipsis" width="220"></vxe-column>
        <vxe-column field="shortName" title="简称" width="120" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="cityCode" title="城市代码" width="100" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="zipCode" title="邮编" width="100" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="lng" title="经度" width="100" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="lat" title="纬度" width="100" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="pinYin" title="拼音" width="100" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="isFreeze" title="禁用" align="center" width="80">
          <template v-slot="{ row }">
            <a-switch :checked="row.isFreeze" />
          </template>
        </vxe-column>
        <vxe-column field="orderNo" title="排序" align="center" width="80"></vxe-column>
        <vxe-column title="操作" align="center" fixed="right" width="100px">
          <template #default="{ row }">
            <a-tooltip @click="tableMapRow(row)" title="地图" v-if="visible.map">
              <label class="text-eip eip-cursor-pointer">地图</label>
            </a-tooltip>
          </template>
        </vxe-column>
      </vxe-table>
    </a-card>

    <districtmap ref="map" v-if="map.visible" :visible.sync="map.visible" :title="map.title" :lng="map.lng"
      :lat="map.lat">
    </districtmap>
  </div>
</template>

<script>
import { query } from "@/services/system/district/list";

import { selectTableRowRadio } from "@/utils/util";
import districtmap from "./map";
export default {
  components: { districtmap },
  data() {
    return {
      table: {
        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 72 + "px",
      },

      map: {
        visible: false,
        lng: "",
        lat: "",
        title: "",
      },

      visible: {
        map: false,
      },
    };
  },
  created() {
    this.tableInit(100000000000)
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    loadChildrenMethod({ row }) {
      let that = this;
      // 异步加载子节点
      return new Promise(resolve => {
        setTimeout(() => {
          query(row.districtId).then((result) => {
            if (result.code == that.eipResultCode.success) {
              result.data.forEach(item => {
                item.hasChild = item.levelType == 5 ? false : true;
              })
              resolve(result.data)
            }
          });
        }, 500)
      })
    },

    /**
     * 初始化列表数据
     */
    tableInit(id) {
      let that = this;
      that.table.loading = true;
      query(id).then((result) => {
        if (result.code == that.eipResultCode.success) {
          result.data.forEach(item => {
            item.hasChild = item.levelType == 5 ? false : true;
          })
          that.table.data = result.data;
        }
        that.table.loading = false;
      });
    },
    /**
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      this.visible.map = buttons.filter((f) => f.method == "map").length > 0;
    },

    /**
     * 区域地图
     */
    districtMapShow() {
      let that = this;
      selectTableRowRadio(
        that.$refs.table,
        function (row) {
          that.map.lng = row.lng;
          that.map.lat = row.lat;
          that.map.title = row.name;
          that.map.visible = true;
        },
        that
      );
    },

    /**
     * 地图
     */
    tableMapRow(row) {
      this.map.lng = row.lng;
      this.map.lat = row.lat;
      this.map.title = row.name;
      this.map.visible = true;
    },
  },
};
</script>