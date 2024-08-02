<template>
  <div style="width: 100%">
    <div class="template-view beauty-scroll" :style="{ height: height, overflow: 'auto' }">
      <div class="template-wrap ">
        <a-layout v-if="data.length == 0">
          <a-layout-content :style="{
            margin: '0',
            background: '#fff',
            minHeight: '280px',
          }">
            <a-result title="数据为空" sub-title="">
              <template #icon>
                <img src="/images/empty.png" />
              </template>
              <template #extra>
              </template>
            </a-result>
          </a-layout-content></a-layout>
        <a-row :gutter="[10]">
          <a-col :span="6" v-for="(row, index) in data" :key="index">
            <div class="item">
              <div style="height: 250px;">
                <img @click="open(row.path)" :src="row.thumbnail">
              </div>
              <div class="footer">
                <div class="name">{{ row.name }}</div>
              </div>
            </div>
          </a-col>
        </a-row>
      </div>
    </div>
  </div>
</template>

<script>

export default {
  data() {
    return {
      height: this.eipHeaderHeight() - 26 + "px",
      data: [{
        name: '智慧城市',
        thumbnail: '/bigscreen/智慧城市/大数据可视化展板通用模板/大数据可视化展板通用模板.png',
        path: '/bigscreen/智慧城市/大数据可视化展板通用模板/index.html'
      }, {
        name: '智慧工地',
        thumbnail: '/bigscreen/智慧城市/智慧工地/智慧工地.png',
        path: '/bigscreen/智慧城市/智慧工地/index.html'
      }, {
        name: '电子商务公共服务平台大数据中心',
        thumbnail: '/bigscreen/智慧电商/电子商务公共服务平台大数据中心/电子商务公共服务平台大数据中心.png',
        path: '/bigscreen/智慧电商/电子商务公共服务平台大数据中心/index.html'
      }, {
        name: '销售大数据页面模板',
        thumbnail: '/bigscreen/智慧电商/销售大数据页面模板/生意参谋大数据可视化平台.png',
        path: '/bigscreen/智慧电商/销售大数据页面模板/index.html'
      }, {
        name: '运营大数据',
        thumbnail: '/bigscreen/智慧电商/运营大数据/运营大数据.png',
        path: '/bigscreen/智慧电商/运营大数据/index.html'
      }, {
        name: '大数据视频监控',
        thumbnail: '/bigscreen/智慧交通/大数据视频监控/大数据视频监控平台.png',
        path: '/bigscreen/智慧交通/大数据视频监控/index.html'
      }, {
        name: '高速综合管控大数据平台',
        thumbnail: '/bigscreen/智慧交通/高速交通大数据分析平台/高速综合管控大数据平台.png',
        path: '/bigscreen/智慧交通/高速交通大数据分析平台/index.html'
      }, {
        name: '智慧旅游综合服务平台',
        thumbnail: '/bigscreen/智慧交通/旅游大数据/智慧旅游综合服务平台.png',
        path: '/bigscreen/智慧交通/旅游大数据/index.html'
      }, {
        name: '银行企业信用风险实时监测',
        thumbnail: '/bigscreen/智慧金融/银行企业信用风险实时监测/银行企业信用风险实时监测平台.png',
        path: '/bigscreen/智慧金融/银行企业信用风险实时监测/index.html'
      }, {
        name: '气象预报大数据平台',
        thumbnail: '/bigscreen/智慧气象/气象预报大数据平台/气象预报大数据平台.png',
        path: '/bigscreen/智慧气象/气象预报大数据平台/index.html'
      }, {
        name: '物联网监控平台',
        thumbnail: '/bigscreen/智慧物联/物联网平台数据统计/物联网监控平台.png',
        path: '/bigscreen/智慧物联/物联网平台数据统计/index.html'
      }, {
        name: '车联网监控平台',
        thumbnail: '/bigscreen/智慧物联/新能源车联网综合大数据平台/车联网监控平台.png',
        path: '/bigscreen/智慧物联/新能源车联网综合大数据平台/index.html'
      }, {
        name: '物流大数据服务平台',
        thumbnail: '/bigscreen/智慧物流/物流大数据服务平台/物流大数据服务平台.png',
        path: '/bigscreen/智慧物流/物流大数据服务平台/index.html'
      }, {
        name: '智慧物流服务平台',
        thumbnail: '/bigscreen/智慧物流/智慧物流/智慧物流服务平台.png',
        path: '/bigscreen/智慧物流/智慧物流/index.html'
      }, {
        name: '大数据医疗服务平台',
        thumbnail: '/bigscreen/智慧医疗/大数据医疗/大数据医疗服务平台.png',
        path: '/bigscreen/智慧医疗/大数据医疗/index.html'
      }, {
        name: '大数据统计展示大屏',
        thumbnail: '/bigscreen/智慧运维/大数据统计展示大屏/大数据统计展示大屏.png',
        path: '/bigscreen/智慧运维/大数据统计展示大屏/index.html'
      }, {
        name: '大数据运维总览图',
        thumbnail: '/bigscreen/智慧运维/大数据运维总览图/大数据运维总览图.png',
        path: '/bigscreen/智慧运维/大数据运维总览图/index.html'
      }, {
        name: '可视化监控管理平台',
        thumbnail: '/bigscreen/智慧运维/可视化监控管理/可视化监控管理平台.png',
        path: '/bigscreen/智慧运维/可视化监控管理/index.html'
      }, {
        name: '设备环境监测平台',
        thumbnail: '/bigscreen/智慧运维/设备环境监测平台/设备环境监测平台.png',
        path: '/bigscreen/智慧运维/设备环境监测平台/index.html'
      }, {
        name: '厅店效能大屏监控看板',
        thumbnail: '/bigscreen/智慧运营/厅店效能大屏监控看板/厅店效能大屏监控看板.png',
        path: '/bigscreen/智慧运营/厅店效能大屏监控看板/index.html'
      }, {
        name: '工商办税大数据监控平台',
        thumbnail: '/bigscreen/智慧政务/工商办税大数据监控平台/工商办税大数据监控平台.png',
        path: '/bigscreen/智慧政务/工商办税大数据监控平台/index.html'
      }, {
        name: '土地交易大数据可视化平台',
        thumbnail: '/bigscreen/智慧政务/土地交易大数据可视化平台/土地交易大数据可视化平台.png',
        path: '/bigscreen/智慧政务/土地交易大数据可视化平台/index.html'
      }, {
        name: '舆情分析平台',
        thumbnail: '/bigscreen/智慧政务/舆情分析/舆情分析平台.png',
        path: '/bigscreen/智慧政务/舆情分析/index.html'
      }, {
        name: '智慧社区内网比对平台',
        thumbnail: '/bigscreen/智慧政务/智慧社区/智慧社区内网比对平台.png',
        path: '/bigscreen/智慧政务/智慧社区/index.html'
      }, {
        name: '综合分析平台',
        thumbnail: '/bigscreen/智慧政务/综合分析平台/综合分析平台.png',
        path: '/bigscreen/智慧政务/综合分析平台/index.html'
      }]
    };
  },
  created() {
  },
  mounted() {
  },
  methods: {
    open(url) {
      window.open(url)
    }
  },
};
</script>

<style lang="less" scoped>
.el-main {
  --el-main-padding: 20px;
  display: block;
  flex: 1;
  flex-basis: auto;
  overflow: auto;
  box-sizing: border-box;
}

.sa-padding-0,
.sa-p-0 {
  padding: 0 !important;
}

.template-view .template-wrap {
  padding: 10px 20px;
}

.template-view .template-wrap {
  padding: 10px 20px
}

.template-view .template-wrap .item {
  position: relative;
  height: 250px;
  border: 1px solid #f3f3f3;
  box-shadow: 0 0 4px #59595933;
  box-sizing: border-box;
  border-radius: 4px;
  margin-bottom: 20px;
  overflow: hidden;
  margin-right: 20px
}

.template-view .template-wrap .item img {
  width: 100%;
  height: 100%;
}

.template-view .template-wrap .item:hover {
  transform: scale(1.02);
  box-shadow: 0 4px 16px #5959593d
}

.template-view .template-wrap .item:hover .footer {
  opacity: 1
}

.template-view .template-wrap .item .image-slot {
  height: 200px
}

.template-view .template-wrap .item .footer {
  position: absolute;
  bottom: 0;
  width: 100%;
  height: fit-content;
  padding: 0 12px;
  background: #fdfdfd;
  transition: all .5s;
  opacity: 0
}

.template-view .template-wrap .item .footer .name {
  padding: 12px 0 4px;
  color: #262626;
  font-size: 16px
}

.template-view .template-wrap .item .footer .platform {
  height: 28px;
  font-size: 14px;
  color: #8c8c8c
}

.template-view .template-wrap .item .footer .memo,
.template-view .template-wrap .item .footer .update-time {
  height: 20px;
  font-size: 12px;
  color: #8c8c8c
}

.template-view .template-wrap .item .footer .label {
  flex-shrink: 0
}

.template-view .template-wrap .item .footer .oper {
  height: 36px
}

.template-view .template-wrap .item .footer .oper .el-button+.el-button {
  margin-left: 8px
}

.ant-btn-link {
  border-color: transparent;
  background: transparent;
  padding: 2px;
  height: auto;
}
</style>
