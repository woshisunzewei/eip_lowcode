<template>
  <div class="option-change-container" ref="container">
    <a-row :gutter="8" class="option-change-box-row">
      <div class="option-change-box" v-for="(val, index) in value" :key="index">
        <a-col :span="9">
          <a-select v-model="val.model">
            <a-select-option :key="lindex" :value="lval.model"
              v-for="(lval, lindex) in list.filter(f => ['number'].includes(f.type))">{{ lval.label
              }}</a-select-option>
          </a-select>
        </a-col>
        <a-col :span="9">
          <a-select v-model="val.type">
            <a-select-option :key="tindex" :value="tval.value" v-for="(tval, tindex) in types">{{ tval.label
            }}</a-select-option>
          </a-select>
        </a-col>
        <a-col :span="6">
          <a-space>
            <a-tooltip>
              <template slot="title">
                删除
              </template>
              <a-icon @click="handleDelete(index)" type="delete" style="color:red" />
            </a-tooltip>
          </a-space>
        </a-col>
      </div>
    </a-row>
    <a-col :span="24"><a @click="handleAdd">添加</a></a-col>
  </div>
</template>
<script>

export default {
  name: "KFooter",
  data() {
    return {
      config: {
        labelCol: { span: 6 },
        wrapperCol: { span: 16 },
      },
      types: [
        {
          value: 'sum',
          label: '求和'
        },
        {
          value: 'avg',
          label: '平均'
        }
      ],
      footer: {

      },
    }
  },
  props: {
    value: {
      type: Array,
      required: true,
    },
    list: {
      type: Array,
      required: true,
    }
  },
  created() {

  },
  mounted() {
  },
  methods: {


    /**
     * 新增按钮
     */
    handleAdd() {
      const addData = [
        ...this.value,
        {
          model: undefined,
          type: 'sum'
        },
      ];
      this.$emit("input", addData);
    },

    /**
     * 删除按钮
     * @param {*} deleteIndex 
     */
    handleDelete(deleteIndex) {
      // 删除
      this.$emit(
        "input",
        this.value.filter((val, index) => index !== deleteIndex)
      );
    },
  },
};
</script>
<style lang="less" scoped>
.option-change-container {
  width: calc(100% - 8px);
}

.option-change-box {
  height: 38px;
  padding-bottom: 6px;

  .option-delete-box {
    margin-top: 3px;
    background: #ffe9e9;
    color: #f22;
    width: 32px;
    height: 32px;
    line-height: 32px;
    text-align: center;
    border-radius: 50%;
    overflow: hidden;
    transition: all 0.3s;

    &:hover {
      background: #f22;
      color: #fff;
    }
  }

  .option-config-box {
    margin-top: 3px;
    background: @primary-color;
    color: #fff;
    width: 32px;
    height: 32px;
    line-height: 32px;
    text-align: center;
    border-radius: 50%;
    overflow: hidden;
    transition: all 0.3s;

    &:hover {
      background: @primary-color;
      color: #fff;
    }
  }
}
</style>
