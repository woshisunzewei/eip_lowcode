<template>
  <div class="investigatestyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <!-- 内容 -->
    <div>
      <p class="info" style="color: #ff0000">
        下拉框，单选，多选等文本用，符号隔开#如：(男#女)
      </p>
    </div>
    <!-- <a-from ref="form" :model="datas" label-width="80px">
      
    </a-from>-->
    <a-form ref="form" :model="datas" label-width="80px">
      <a-form-item label="名称" label-width="40px">
        <a-input v-model="datas.title" style="width: 87%"></a-input>
      </a-form-item>
      <a-form-item v-for="(item, index) in datas.jsonData" :key="index" class="lef">
        <span class="delete" @click="deletetext(index)">x</span>
        <a-input v-model="item.name" class="title" placeholder="表单模块名称"></a-input>
        <a-select v-model="item.type" placeholder="请选择显示格式" @change="conChange(index)">
          <a-select-option :value="index" v-for="(item, index) in selecttext" :key="index">{{ item
            }}</a-select-option>
        </a-select>
        <a-input type="textarea" v-model="item.value" placeholder="提示语句如:(请输入姓名)" v-if="item.type == 0"></a-input>
        <a-input type="textarea" v-model="item.value" @input="item.value1 = item.value.split('#')"
          placeholder="多项之间用‘#’逗号隔开" v-else></a-input>
      </a-form-item>
      <a-form-item>
        <a-button @click="addText" class="uploadImg" type="primary" plain>
          <a-icon type="plus" />点击添加内容
        </a-button>
      </a-form-item>
    </a-form>
  </div>
</template>

<script>
export default {
  name: "investigatestyle",
  props: {
    datas: Object,
  },
  data() {
    return {
      selecttext: ["文本", "下拉框", "单选", "多选"],
      index1: 0,
    };
  },
  mounted() { },
  methods: {
    //添加文本
    addText() {
      var text = {
        name: "",
        type: "",
        value: "",
        value1: [],
        value2: "",
        showPicker: false,
      };
      this.datas.jsonData.push(text);
    },
    //删除文本
    deletetext(index) {
      this.datas.jsonData.splice(index, 1);
    },
    //下拉内容改变发生发生事件
    conChange(index) {
      this.datas.jsonData[index].value = "";
      this.datas.jsonData[index].value1 = [];
    },
  },
};
</script>

<style scoped lang="less">
.investigatestyle {
  width: 100%;
  position: absolute;
  left: 0;
  top: 0;
  padding: 0 10px 20px;
  box-sizing: border-box;

  /* 标题 */
  h2 {
    padding: 24px 16px 24px 0;
    margin-bottom: 15px;
    border-bottom: 1px solid #f2f4f6;
    font-size: 18px;
    font-weight: 600;
    color: #323233;
  }

  .lef {
    position: relative;

    /deep/.ant-form-item-label {
      text-align: left;
    }

    .delete {
      background: #ff0000;
      color: #ffffff;
      top: -6px;
      right: -6px;
      border-radius: 50%;
      display: block;
      width: 16px;
      height: 16px;
      line-height: 16px;
      text-align: center;
      position: absolute;
      z-index: 10;
      cursor: pointer;
    }
  }

  /deep/.a-form-item__content {
    margin-left: 0 !important;

    // display: flex;
    div {
      &:nth-child(2) {
        // flex: 1;
        width: 90%;
        margin-right: 2%;
        margin-bottom: 10px;
      }

      &:nth-child(3) {
        width: 90%;
        // flex: 1;
      }

      &:nth-child(4) {
        width: 100%;
        margin-top: 5px;
        // flex: 3;
      }
    }
  }

  /* 上传图片按钮 */
  .uploadImg {
    width: 100%;
    height: 40px;
    margin-top: 20px;
  }
}

// /deep/.a-input__inner{
//   padding: 0 5px;
// }</style>
