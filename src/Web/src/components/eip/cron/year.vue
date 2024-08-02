<template>
  <a-form>
    <a-radio-group v-model="radioValue">
      <a-form-item>
        <a-radio :value="1">
          不填，允许的通配符[, - * /]
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="2"> 每年 </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="3">
          周期从
          <a-input-number size="small" v-model="cycle01" :min="fullYear" :max="2098" /> -
          <a-input-number size="small" v-model="cycle02" :min="cycle01 ? cycle01 + 1 : fullYear + 1" :max="2099" />
        </a-radio>
      </a-form-item>
    </a-radio-group>
  </a-form>
</template>

<script>
export default {
  data() {
    return {
      fullYear: 0,
      radioValue: 1,
      cycle01: 0,
      cycle02: 0,
      average01: 0,
      average02: 1,
      checkboxList: [],
      checkNum: this.check
    };
  },
  name: "crontab-year",
  props: ["check", "month", "cron"],
  methods: {
    // 单选按钮值变化时
    radioChange() {
      switch (this.radioValue) {
        case 1:
          this.$emit("update", "year", "");
          break;
        case 2:
          this.$emit("update", "year", "*");
          break;
        case 3:
          this.$emit("update", "year", this.cycleTotal);
          break;
        case 4:
          this.$emit("update", "year", this.averageTotal);
          break;
        case 5:
          this.$emit("update", "year", this.checkboxString);
          break;
      }
    },
    // 周期两个值变化时
    cycleChange() {
      if (this.radioValue == "3") {
        this.$emit("update", "year", this.cycleTotal);
      }
    },
    // 平均两个值变化时
    averageChange() {
      if (this.radioValue == "4") {
        this.$emit("update", "year", this.averageTotal);
      }
    },
    // checkbox值变化时
    checkboxChange() {
      if (this.radioValue == "5") {
        this.$emit("update", "year", this.checkboxString);
      }
    }
  },
  watch: {
    radioValue: "radioChange",
    cycleTotal: "cycleChange",
    averageTotal: "averageChange",
    checkboxString: "checkboxChange"
  },
  computed: {
    // 计算两个周期值
    cycleTotal: function () {
      const cycle01 = this.checkNum(this.cycle01, this.fullYear, 2098);
      const cycle02 = this.checkNum(
        this.cycle02,
        cycle01 ? cycle01 + 1 : this.fullYear + 1,
        2099
      );
      return cycle01 + "-" + cycle02;
    },
    // 计算平均用到的值
    averageTotal: function () {
      const average01 = this.checkNum(this.average01, this.fullYear, 2098);
      const average02 = this.checkNum(
        this.average02,
        1,
        2099 - average01 || this.fullYear
      );
      return average01 + "/" + average02;
    },
    // 计算勾选的checkbox值合集
    checkboxString: function () {
      let str = this.checkboxList.join();
      return str;
    }
  },
  mounted: function () {
    // 仅获取当前年份
    this.fullYear = Number(new Date().getFullYear());
    this.cycle01 = this.fullYear;
    this.average01 = this.fullYear;
  }
};
</script>
