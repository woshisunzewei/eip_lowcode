<template>
  <a-form>
    <a-radio-group v-model="radioValue">
      <a-form-item>
        <a-radio :value="1">
          分钟，允许的通配符[, - * /]
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="2">
          周期从
          <a-input-number size="small" v-model="cycle01" :min="0" :max="58" /> -
          <a-input-number size="small" v-model="cycle02" :min="cycle01 ? cycle01 + 1 : 1" :max="59" />
          分钟
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="3">
          从
          <a-input-number size="small" v-model="average01" :min="0" :max="58" /> 分钟开始，每
          <a-input-number size="small" v-model="average02" :min="1" :max="59 - average01 || 0" />
          分钟执行一次
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="4">
          指定
          <a-select clearable size="small" v-model="checkboxList" placeholder="可多选" mode="multiple" style="width: 100%">
            <a-select-option v-for="item in 60" :key="item" :value="item - 1">{{
      item - 1
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
      cycle01: 1,
      cycle02: 2,
      average01: 0,
      average02: 1,
      checkboxList: [],
      checkNum: this.check
    };
  },
  name: "crontab-min",
  props: ["check", "cron"],
  methods: {
    // 单选按钮值变化时
    radioChange() {
      switch (this.radioValue) {
        case 1:
          this.$emit("update", "min", "*", "min");
          break;
        case 2:
          this.$emit("update", "min", this.cycleTotal, "min");
          break;
        case 3:
          this.$emit("update", "min", this.averageTotal, "min");
          break;
        case 4:
          this.$emit("update", "min", this.checkboxString, "min");
          break;
      }
    },
    // 周期两个值变化时
    cycleChange() {
      if (this.radioValue == "2") {
        this.$emit("update", "min", this.cycleTotal, "min");
      }
    },
    // 平均两个值变化时
    averageChange() {
      if (this.radioValue == "3") {
        this.$emit("update", "min", this.averageTotal, "min");
      }
    },
    // checkbox值变化时
    checkboxChange() {
      if (this.radioValue == "4") {
        this.$emit("update", "min", this.checkboxString, "min");
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
      const cycle01 = this.checkNum(this.cycle01, 0, 58);
      const cycle02 = this.checkNum(
        this.cycle02,
        cycle01 ? cycle01 + 1 : 1,
        59
      );
      return cycle01 + "-" + cycle02;
    },
    // 计算平均用到的值
    averageTotal: function () {
      const average01 = this.checkNum(this.average01, 0, 58);
      const average02 = this.checkNum(this.average02, 1, 59 - average01 || 0);
      return average01 + "/" + average02;
    },
    // 计算勾选的checkbox值合集
    checkboxString: function () {
      let str = this.checkboxList.join();
      return str == "" ? "*" : str;
    }
  }
};
</script>
