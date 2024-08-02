<template>
  <a-modal :bodyStyle="{ padding: '1px' }" title="快捷键大全" width="600px" :visible="modalVisible" :footer="null"
    @cancel="close">
    <vxe-table size="small" height="500px" :data="dataSource">
      <template #loading>
        <a-spin></a-spin>
      </template>
      <template #empty>
        <eip-empty />
      </template>
      <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
      <vxe-column field="shortcutName" title="功能" min-width="150" showOverflow="ellipsis"></vxe-column>
      <vxe-column field="codeName" title="快捷键" min-width="150" showOverflow="ellipsis"></vxe-column>
    </vxe-table>
  </a-modal>
</template>

<script>
import { shortcutKeys } from '../config/shortcutKeys.js'

export default {
  data() {
    return {
      modalVisible: false,
      columns: [
        {
          title: '功能',
          align: 'center',
          key: 'shortcutName',
          dataIndex: 'shortcutName',
          width: '50%'
        },
        {
          title: '快捷键',
          align: 'center',
          key: 'codeName',
          dataIndex: 'codeName',
          width: '50%'
        }
      ],
      dataSource: []
    }
  },
  methods: {
    open() {
      this.modalVisible = true
      this.dataSource = Object.values(shortcutKeys)
    },
    close() {
      this.dataSource = []
      this.modalVisible = false
    },
    saveSetting() {
      this.close()
    }
  }
}
</script>
