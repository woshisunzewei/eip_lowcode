<template>
  <a-form>
    <a-radio-group v-model="radioValue">
      <a-form-item>
        <a-radio :value="1">
          日，允许的通配符[, - * ? / L W]
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="2"> 不指定 </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="3">
          周期从
          <a-input-number size="small" v-model="cycle01" :min="1" :max="30" /> -
          <a-input-number size="small" v-model="cycle02" :min="cycle01 ? cycle01 + 1 : 2" :max="31" />
          日
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="4">
          从
          <a-input-number size="small" v-model="average01" :min="1" :max="30" /> 号开始，每
          <a-input-number size="small" v-model="average02" :min="1" :max="31 - average01 || 1" />
          日执行一次
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="5">
          每月
          <a-input-number size="small" v-model="workday" :min="1" :max="31" />
          号最近的那个工作日
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="6"> 本月最后一天 </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="7">
          指定
          <a-select clearable size="small" v-model="checkboxList" placeholder="可多选" mode="multiple" style="width: 100%">
            <a-select-option v-for="item in 31" :key="item" :value="item">{{
      item
              }}</a-select-option>
          </a-select>
        </a-radio>
      </a-form-item>
    </a-radio-group>
  </a-form>
</template>

<script>
export default {
  data() {
    return {
      radioValue: 1,
      workday: 1,
      cycle01: 1,
      cycle02: 2,
      average01: 1,
      average02: 1,
      checkboxList: [],
      checkNum: this.check
    };
  },
  name: "crontab-day",
  props: ["check", "cron"],
  methods: {
    // 单选按钮值变化时
    radioChange() {
      ("day rachange");
      if (this.radioValue !== 2 && this.cron.week !== "?") {
        this.$emit("update", "week", "?", "day");
      }

      switch (this.radioValue) {
        case 1:
          this.$emit("update", "day", "*");
          break;
        case 2:
          this.$emit("update", "day", "?");
          break;
        case 3:
          this.$emit("update", "day", this.cycleTotal);
          break;
        case 4:
          this.$emit("update", "day", this.averageTotal);
          break;
        case 5:
          this.$emit("update", "day", this.workday + "W");
          break;
        case 6:
          this.$emit("update", "day", "L");
          break;
        case 7:
          this.$emit("update", "day", this.checkboxString);
          break;
      }
      ("day rachange end");
    },
    // 周期两个值变化时
    cycleChange() {
      if (this.radioValue == "3") {
        this.$emit("update", "day", this.cycleTotal);
      }
    },
    // 平均两个值变化时
    averageChange() {
      if (this.radioValue == "4") {
        this.$emit("update", "day", this.averageTotal);
      }
    },
    // 最近工作日值变化时
    workdayChange() {
      if (this.radioValue == "5") {
        this.$emit("update", "day", this.workdayCheck + "W");
      }
    },
    // checkbox值变化时
    checkboxChange() {
      if (this.radioValue == "7") {
        this.$emit("update", "day", this.checkboxString);
      }
    }
  },
  watch: {
    radioValue: "radioChange",
    cycleTotal: "cycleChange",
    averageTotal: "averageChange",
    workdayCheck: "workdayChange",
    checkboxString: "checkboxChange"
  },
  computed: {
    // 计算两个周期值
    cycleTotal: function () {
      const cycle01 = this.checkNum(this.cycle01, 1, 30);
      const cycle02 = this.checkNum(
        this.cycle02,
        cycle01 ? cycle01 + 1 : 2,
        31,
        31
      );
      return cycle01 + "-" + cycle02;
    },
    // 计算平均用到的值
    averageTotal: function () {
      const average01 = this.checkNum(this.average01, 1, 30);
      const average02 = this.checkNum(this.average02, 1, 31 - average01 || 0);
      return average01 + "/" + average02;
    },
    // 计算工作日格式
    workdayCheck: function () {
      const workday = this.checkNum(this.workday, 1, 31);
      return workday;
    },
    // 计算勾选的checkbox值合集
    checkboxString: function () {
      let str = this.checkboxList.join();
      return str == "" ? "*" : str;
    }
  }
};
</script>
