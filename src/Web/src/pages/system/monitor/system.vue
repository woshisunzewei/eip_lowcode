<template>
  <div>
    <a-row :gutter="8">
      <a-col :md="12" :sm="24">
        <a-card shadow="hover" size="small" title="系统信息">
          <a-spin :spinning="machineBaseInfo.spinning">
            <a-descriptions
              class="eip-descriptions"
              bordered
              size="small"
              :column="1"
            >
              <a-descriptions-item label="主机名称">{{
                machineBaseInfo.hostName
              }}</a-descriptions-item>
              <a-descriptions-item label="操作系统">{{
                machineBaseInfo.systemOs
              }}</a-descriptions-item>
              <a-descriptions-item label="系统架构">
                {{ machineBaseInfo.osArchitecture }}</a-descriptions-item
              >
              <a-descriptions-item label="CPU核数">{{
                machineBaseInfo.processorCount
              }}</a-descriptions-item>
              <a-descriptions-item label="运行时长">{{
                machineBaseInfo.sysRunTime
              }}</a-descriptions-item>
              <a-descriptions-item label="运行框架">{{
                machineBaseInfo.frameworkDescription
              }}</a-descriptions-item>
            </a-descriptions>
          </a-spin>
        </a-card>
      </a-col>
      <a-col :md="12" :sm="24">
        <a-card
          shadow="hover"
          title="使用信息"
          style="height: 292px"
          size="small"
        >
          <a-spin :spinning="machineUseInfo.spinning">
            <a-row>
              <a-col
                :xs="12"
                :sm="12"
                :md="12"
                :lg="12"
                :xl="12"
                style="text-align: center"
              >
                <a-progress
                  type="dashboard"
                  :percent="
                    parseInt(
                      machineUseInfo.ramRate == undefined
                        ? 0
                        : machineUseInfo.ramRate.substr(
                            0,
                            machineUseInfo.ramRate.length - 1
                          )
                    )
                  "
                >
                  <template #format>
                    <span>{{ machineUseInfo.ramRate }}<br /></span>
                    <span style="font-size: 10px">
                      已用:{{ machineUseInfo.usedRam }}<br />
                      剩余:{{ machineUseInfo.freeRam }}<br />
                      内存使用率
                    </span>
                  </template>
                </a-progress>
              </a-col>
              <a-col
                :xs="12"
                :sm="12"
                :md="12"
                :lg="12"
                :xl="12"
                style="text-align: center"
              >
                <a-progress
                  type="dashboard"
                  :percent="
                    parseInt(
                      machineUseInfo.cpuRate == undefined
                        ? 0
                        : machineUseInfo.cpuRate.substr(
                            0,
                            machineUseInfo.cpuRate.length - 1
                          )
                    )
                  "
                  :color="'var(--a-color-primary)'"
                >
                  <template #format>
                    <span>{{ machineUseInfo.cpuRate }}<br /></span>
                    <span style="font-size: 10px"> CPU使用率 </span>
                  </template>
                </a-progress>
              </a-col>
            </a-row>

            <a-row>
              <a-descriptions
                class="eip-descriptions"
                bordered
                size="small"
                :column="1"
              >
                <a-descriptions-item label="启动时间">{{
                  machineUseInfo.startTime
                }}</a-descriptions-item>
                <a-descriptions-item label="运行时长">{{
                  machineUseInfo.runTime
                }}</a-descriptions-item>
              </a-descriptions>
            </a-row>
          </a-spin>
        </a-card>
      </a-col>
    </a-row>

    <a-row :gutter="8">
      <a-col :md="24" :sm="24">
        <a-card
          shadow="hover"
          title="磁盘信息"
          style="margin-top: 8px"
          size="small"
        >
          <a-spin :spinning="machineDiskInfoSpinning">
            <a-row>
              <a-col
                :span="4"
                :xs="(24 / machineDiskInfo.length) * 2"
                :sm="24 / machineDiskInfo.length"
                :md="24 / machineDiskInfo.length"
                :lg="24 / machineDiskInfo.length"
                :xl="24 / machineDiskInfo.length"
                v-for="d in machineDiskInfo"
                :key="d.diskName"
                style="text-align: center"
              >
                <a-progress
                  type="circle"
                  :percent="d.availablePercent"
                  :status="d.availablePercent > 90 ? 'exception' : 'normal'"
                >
                  <template #format>
                    <span>{{ d.availablePercent }}%<br /></span>
                    <span style="font-size: 10px">
                      已用:{{ d.used }}GB<br />
                      剩余:{{ d.availableFreeSpace }}GB<br />
                      {{ d.diskName }}
                    </span>
                  </template>
                </a-progress>
              </a-col>
            </a-row>
          </a-spin>
        </a-card>
      </a-col>
    </a-row>
  </div>
</template>
  
<script>
import {
  serverBase,
  serverDisk,
  serverUsed,
} from "@/services/system/monitor/system";

export default {
  data() {
    return {
      machineBaseInfo: {
        hostName: "",
        systemOs: "",
        osArchitecture: "",
        processorCount: "",
        sysRunTime: "",
        frameworkDescription: "",
        spinning: true,
      },
      machineUseInfo: {
        ramRate: "",
        usedRam: "",
        freeRam: "",
        startTime: "",
        runTime: "",
        spinning: true,
      },
      machineDiskInfo: [],
      machineDiskInfoSpinning: true,

      setInterval: null, //定时器
    };
  },
  beforeDestroy() {
    if (this.setInterval) {
      clearInterval(this.setInterval);
    }
  },
  created() {
    this.init();
    this.setInterval = setInterval(() => {
      this.init();
    }, 5000);
  },

  methods: {
    init() {
      let that = this;
      if (!this.setInterval) {
        that.machineBaseInfo.spinning = true;
        that.machineDiskInfoSpinning = true;
        that.machineUseInfo.spinning = true;
      }

      serverBase().then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.machineBaseInfo = result.data;
        }
        that.machineBaseInfo.spinning = false;
      });

      serverDisk().then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.machineDiskInfo = result.data;
        }
        that.machineDiskInfoSpinning = false;
      });

      serverUsed().then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.machineUseInfo = result.data;
        }
        that.machineUseInfo.spinning = false;
      });
    },
  },
};
</script>
<style scoped>
.sysInfo_table {
  width: 100%;
  min-height: 40px;
  line-height: 40px;
  text-align: center;
}

.sysInfo_td {
  border-bottom: 1px solid #e8e8e8;
}
</style>