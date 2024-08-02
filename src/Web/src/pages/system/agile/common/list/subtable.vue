<template>
  <a-drawer width="800px" :destroyOnClose="true" :visible="visible" :zIndex="style.zIndex"
    :body-style="{ padding: '10px' }" title="子表配置" @close="cancel">
    <vxe-table row-key ref="styleTable" :data="subtables" :height="height">
      <template #loading>
        <a-spin></a-spin>
      </template>
      <template #empty>
        <eip-empty />
      </template>
      <vxe-column title="新增" width="54px" align="center">
        <template #header>
          <a-button @click="add" size="small" type="primary">
            <a-icon type="plus" />
          </a-button>
        </template>
        <template v-slot="{ row }">
          <a-popconfirm title="确定删除配置?" ok-text="确定" cancel-text="取消" @confirm="del(row)">
            <a-button @click.stop="" size="small" type="danger">
              <a-icon type="delete"></a-icon>
            </a-button>
          </a-popconfirm>
        </template>
      </vxe-column>

      <vxe-column title="子表" width="150px">
        <template v-slot="{ row }">
          <a-select show-search optionFilterProp="label" @change="selectChange(row, $event)" placeholder="请选择子表"
            v-model="row.id" style="width: calc(100%)">
            <a-select-option v-for="( item, i ) in data " :key="i" :value="item.configId"
              :label="(item.remark ? item.remark + '-' : '') + item.name"
              :title="(item.remark ? item.remark + '-' : '') + item.name">{{ (item.remark ? item.remark + '-' : '') }}
              {{
                item.name
              }}</a-select-option>
          </a-select>
        </template>
      </vxe-column>
      <vxe-column title="名称" width="170px">
        <template v-slot="{ row }">
          <a-input allow-clear v-model="row.name" placeholder="请输入名称" />
        </template>
      </vxe-column>
      <vxe-column title="条件" width="400px">
        <template v-slot="{ row }">
          <a-input v-model="row.condition" placeholder="字段使用{xxx}表示。如 RelationId='{RelationId}' and name='{Name}'">
            <a-tooltip slot="suffix" title="RelationId='{RelationId}' and name='{Name}' ">
              <a-icon type="info-circle" />
            </a-tooltip>
          </a-input>
        </template>
      </vxe-column>
    </vxe-table>

    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button key="back" @click="cancel"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="ok" type="primary"><a-icon type="save" />提交</a-button>
      </a-space>
    </div>
  </a-drawer>
</template>

<script>

export default {
  name: "list-style",
  data() {
    return {
      style: {
        zIndex: 1020,
      },
      height: window.innerHeight - 132,
      subtables: [],
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    config: {
      type: Array,
      default: []
    },
    data: {
      type: Array,
      default: []
    }
  },
  mounted() {
    this.init();
  },
  methods: {
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 保存
     */
    ok() {
      this.cancel();
      this.$emit("ok", this.subtables);
    },

    /**
     * 新增
     */
    add() {
      this.subtables.push({
        id: undefined,
        condition: null,
        name: null
      });
    },

    /**
     * 子表选择
     */
    selectChange(item, data) {
      var subtable = this.data.filter(f => f.configId == data);
      item.name = (subtable[0].remark ? subtable[0].remark + '-' : '') + subtable[0].name;
    },

    /**
     * 删除
     */
    del(row) {
      let spIndex = -1;
      this.subtables.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.subtables.splice(spIndex, 1);
      }
    },

    /**
     * 初始化
     */
    init() {
      if (this.config) {
        this.subtables = this.$utils.clone(this.config, true);
      } else {
        this.subtables = [];
      }
    },
  },
};
</script>

<style lang="less" scoped>
.ant-tag {
  border-radius: 40px !important;
}
</style>
