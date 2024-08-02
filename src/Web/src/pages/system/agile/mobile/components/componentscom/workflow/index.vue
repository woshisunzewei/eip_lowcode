<template>
  <div class="workflow">
    <div padding v-for="(item, indexitem) in librarys" :key="indexitem">
      <div style="background-color: #fff;    padding: 12px 10px;"> {{ item.name }}</div>
      <van-grid :column-num="datas.columnnum" :border="datas.border">
        <van-grid-item v-for="(childrenitem, childrenindex) in item.children" :key="childrenindex">
          <div class="grid-item-box">
            <img style="height: 48px;width: 48px" :src="childrenitem.image" />
            <label class="text">{{ childrenitem.name }}</label>
          </div>
        </van-grid-item>
      </van-grid>
    </div>
    <!-- 删除组件 -->
    <slot name="deles" />
  </div>
</template>

<script>
import { library } from "@/services/workflow/send/library";
export default {
  name: 'workflow',
  data() {
    return {
      librarys: []
    }
  },
  props: {
    datas: Object,
  },
  created() {
    this.initLibrary();
  },
  methods: {
    /**
     * 初始化
     */
    initLibrary() {
      let that = this;
      library().then(result => {
        let data = result.data;
        for (let i = 0; i < data.length; i++) {
          var item = data[i];
          var library = {
            name: item.typeName,
            remark: item.remark
          };
          var libraryj = [];
          //判断是否还具有下级
          var lists = item.list;
          for (let j = 0; j < lists.length; j++) {
            var itemj = lists[j];
            var librarycj = {
              name: itemj.name,
              remark: itemj.remark,
              processId: itemj.processId,
              image: itemj.image,
              url: "?id=" + itemj.processId
            };
            libraryj.push(librarycj);
          }
          library.children = libraryj;
          that.librarys.push(library);
        }
        if (data.length == 0) {

        }
      }).then(result => { }).catch(err => {

      })
    }
  },
}
</script>

<style scoped lang="less">
.workflow {
  background-color: #fff;
  position: relative;

  .grid-item-box {
    flex: 1;
    // position: relative;
    /* #ifndef APP-NVUE */
    display: flex;
    /* #endif */
    flex-direction: column;
    align-items: center;
    justify-content: center;
    padding: 15px 0;
  }
}
</style>
