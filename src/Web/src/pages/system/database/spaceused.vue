<template>
  <div>
    <a-card class="eip-admin-card-small" size="small">
      <div slot="title">表空间使用情况</div>
      <vxe-table :loading="table.loading" ref="table" :column-config="{ isCurrent: true, isHover: true }"
        :row-config="{ isCurrent: true, isHover: true }" :height="table.height" :export-config="{}" :print-config="{}"
        :data="table.data">
        <template #loading>
          <a-spin></a-spin>
        </template>
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column field="name" title="表名"></vxe-column>
        <vxe-column field="description" title="描述"></vxe-column>
        <vxe-column field="rows" title="记录数"></vxe-column>

        <vxe-column field="reserved" title="保留空间" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="data" title="使用空间" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="indexSize" title="索引使用空间" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="unused" title="未用空间" showOverflow="ellipsis"></vxe-column>
      </vxe-table>
    </a-card>
  </div>
</template>

<script>
import { spaceused } from "@/services/system/database/spaceused";

export default {
  components: {},
  data() {
    return {
      table: {
        param: {
          table: null,
        },
        loading: true,
        data: [],
        height: window.innerHeight - 182,
      },
    };
  },
  created() {
    this.tableInit();
  },

  methods: {
    /**
     * 列表数据
     */
    tableInit() {
      let that = this;
      that.table.loading = true;
      spaceused().then((result) => {
        if (result.code == that.eipResultCode.success) {
          that.table.data = result.data;
        }
        that.table.loading = false;
      });
    },
  },
};
</script>