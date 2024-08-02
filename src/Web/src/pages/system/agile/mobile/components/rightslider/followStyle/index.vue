<template>
  <div id="followStyle">
    <!-- 标题 -->
    <h2>{{ datas.text }}</h2>

    <a-form :label-col="{ span: 7 }" :wrapper-col="{ span: 17 }">
      <a-form-item class="lef" label="公众号图片">
        <div class="follow-pic">
          <img :src="datas.heade" alt="" />
          <a-button @click="upLoadimg.visible = true" class="uploadImg" type="primary" plain>
            <a-icon type="plus" />上传公众号图片
          </a-button>
        </div>
      </a-form-item>

      <a-form-item class="lef" label="公众号名称">
        <a-input type="text" v-model="datas.followName" placeholder="请输入公众号名称" />
      </a-form-item>

      <a-form-item class="lef" label="公众号Id">
        <a-input type="text" v-model="datas.followAppId" placeholder="请输入公众号Id" />
      </a-form-item>
    </a-form>

    <!-- 上传图片 -->
    <eip-material v-if="upLoadimg.visible" :visible.sync="upLoadimg.visible" @ok="materialOk" />
  </div>
</template>

<script>
export default {
  name: "followStyle",
  props: {
    datas: Object,
  },
  data() {
    return {
      upLoadimg: {
        visible: false
      },
    };
  },

  created() {
  },
  methods: {
    // 提交
    materialOk(res) {
      this.datas.heade = res;
    },
  },

};
</script>

<style lang="less" scoped>
#followStyle {
  width: 100%;
  position: absolute;
  left: 0;
  top: 0;
  padding: 0 10px 20px;
  box-sizing: border-box;
}

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
  /deep/.ant-form-item-label {
    text-align: left;
  }
}

.follow-pic {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;

  img {
    display: block;
    width: 50px;
    height: 50px;
    border-radius: 50%;
  }

  .uploadImg {
    width: 80%;
    margin-top: 20px;
  }
}
</style>
