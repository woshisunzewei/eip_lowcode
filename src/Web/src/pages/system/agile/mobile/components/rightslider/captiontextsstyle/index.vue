<template>
  <section class="captiontextsstyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <!-- 表单 -->
    <a-form :label-col="{ span: 5 }" :wrapper-col="{ span: 19 }">
      <!-- 标题内容 -->
      <a-form-item label="标题内容">
        <a-input v-model="datas.name" placeholder="请输入标题" show-word-limit />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 描述内容 -->
      <a-form-item label="描述内容">
        <a-input type="textarea" v-model="datas.description" placeholder="请输入要说明的文字，最多 100 字" maxlength="100" />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 显示位置 -->
      <a-form-item label="显示位置">
        <div class="weiz">
          <i :class="datas.positions === 'left' ? 'active' : ''" class="iconfont icon-horizontal-left"
            @click="datas.positions = 'left'" />
          <i :class="datas.positions === 'center' ? 'active' : ''" class="iconfont icon-juzhong"
            @click="datas.positions = 'center'" />
          <!-- <i
          :class="datas.positions === 'right' ? 'active': ''"
          class="iconfont icon-juyou"
          @click="datas.positions = 'right'"
          /> -->
        </div>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 标题大小 -->
      <a-form-item label="标题大小" prop="wordSize" :hide-required-asterisk="true">
        <a-input type="number" v-model.number="datas.wordSize" placeholder="请输入标题文字大小" :maxlength="2" />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 描述大小 -->
      <a-form-item label="描述大小" prop="descriptionSize" :hide-required-asterisk="true">
        <a-input type="number" v-model.number="datas.descriptionSize" placeholder="请输入描述文字大小" />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 标题粗细 -->
      <a-form-item label="标题粗细" prop="wordWeight" :hide-required-asterisk="true">
        <a-input type="number" v-model.number="datas.wordWeight" placeholder="请输入标题粗细" />
      </a-form-item>

      <div style="height: 10px" />

      <!--描述粗细 -->
      <a-form-item label="描述粗细" prop="descriptionWeight" :hide-required-asterisk="true">
        <a-input type="number" v-model.number="datas.descriptionWeight" placeholder="请输入描述粗细" />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 框体高度 -->
      <a-form-item class="lef" label="框体高度">
        <a-slider v-model="datas.wordHeight" :max="100" :min="20"> </a-slider>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 标题颜色 -->
      <a-form-item label="标题颜色">
        <!-- 颜色选择器 -->
        <eip-color :value="datas.wordColor" @change="(value) => { datas.wordColor = value }"></eip-color>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 描述颜色 -->
      <a-form-item label="描述颜色">
        <!-- 颜色选择器 -->
        <eip-color :value="datas.descriptionColor" @change="(value) => { datas.descriptionColor = value }"></eip-color>
      </a-form-item>

      <div style="height: 10px" />

      <!-- 背景颜色 -->
      <a-form-item label="背景颜色">
        <!-- 背景颜色 -->
        <eip-color :value="datas.backColor" @change="(value) => { datas.backColor = value }"></eip-color>
      </a-form-item>

      <div style="height: 10px" />

      <!--查看更多 -->
      <a-form-item label="底部分割线">
        {{ datas.borderBott ? "显示" : "隐藏" }}
        <a-checkbox v-model="datas.borderBott" />
      </a-form-item>

      <div style="height: 10px" />

      <!--查看更多 -->
      <a-form-item label="查看更多">
        {{ datas.more.show ? "显示" : "隐藏" }}
        <a-checkbox v-model="datas.more.show" />
      </a-form-item>

      <div style="height: 10px" />

      <!-- 更多样式选择 -->
      <div v-show="datas.more.show ? true : false">
        <a-radio-group v-model="datas.more.type">
          <a-radio :value="0">样式一</a-radio>
          <a-radio :value="1">样式二</a-radio>
          <a-radio :value="2">样式三</a-radio>
        </a-radio-group>

        <div style="height: 10px" />

        <!-- 更多文字 -->
        <a-input v-show="datas.more.type === 2 ? false : true" type="text" style="width: 110px; margin: 15px"
          v-model="datas.more.text" size="small" />

        <div style="height: 10px" />

        <!-- 跳转链接 -->
        <a-form-item label="跳转链接">
          <a-radio-group v-model="datas.more.httpType" style="margin-left: 18px">
            <a-radio :value="10">内部链接</a-radio>
            <a-radio :value="11">外部链接</a-radio>
          </a-radio-group>

          <!-- 输入http -->
          <a-input v-model="datas.more.http" placeholder="请输入跳转链接" show-word-limit style="margin-top: 10px" />
        </a-form-item>
      </div>
    </a-form>
  </section>
</template>

<script>
export default {
  name: "captiontextsstyle",
  props: {
    datas: Object,
  },
  data() {
    let checkAge = (rule, value, callback) => {
      if (value.length === 0) callback(new Error("请输入有效数字"));
      if (value > 99) callback(new Error("数字最大为99"));
    };
    let kon = (rule, value, callback) => {
      if (value.length === 0) callback(new Error("请输入有效数字"));
    };
    return {
      options: [],
      rules: {
        wordSize: [{ required: true, validator: checkAge, trigger: "blur" }],
        descriptionSize: [
          { required: true, validator: checkAge, trigger: "blur" },
        ],
        wordWeight: [{ required: true, validator: kon, trigger: "blur" }],
        descriptionWeight: [
          { required: true, validator: kon, trigger: "blur" },
        ],
      },
      predefineColors: [
        // 颜色选择器预设
        "#ff4500",
        "#ff8c00",
        "#ffd700",
        "#90ee90",
        "#00ced1",
        "#1e90ff",
        "#c71585",
        "#409EFF",
        "#909399",
        "#C0C4CC",
        "rgba(255, 69, 0, 0.68)",
        "rgb(255, 120, 0)",
        "hsv(51, 100, 98)",
        "hsva(120, 40, 94, 0.5)",
        "hsl(181, 100%, 37%)",
        "hsla(209, 100%, 56%, 0.73)",
        "#c7158577",
      ],
    };
  },
  methods: {},
};
</script>

<style scoped lang="less">
.captiontextsstyle {
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

  /* 颜色选择器 */
  .picke {
    float: right;
  }

  /* 位置 */
  .weiz {
    text-align: right;

    i {
      padding: 5px 14px;
      margin-left: 10px;
      border-radius: 0;
      border: 1px solid #ebedf0;
      font-size: 20px;
      font-weight: 500;
      cursor: pointer;

      &:last-child {
        font-size: 22px;
      }

      &.active {
        color: @primary-color;
        border: 1px solid @primary-color;
        background: #e0edff;
      }
    }
  }

  /deep/.wid .ant-form-item-label {
    width: 94px !important;
  }

  /deep/.wid .a-form-item__content {
    float: right;
  }
}
</style>
