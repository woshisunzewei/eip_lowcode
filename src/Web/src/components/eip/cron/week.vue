<template>
  <a-form>
    <a-radio-group v-model="radioValue">
      <a-form-item>
        <a-radio :value="1">
          周，允许的通配符[, - * ? / L #]
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="2"> 不指定 </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="3">
          周期从星期
          <a-select style="width:100px" clearable size="small" v-model="cycle01">
            <a-select-option v-for="(item, index) of weekList" :key="index" :value="item.key"
              :disabled="item.key === 1">{{ item.value }}</a-select-option>
          </a-select>
          -
          <a-select style="width:100px" clearable size="small" v-model="cycle02">
            <a-select-option v-for="(item, index) of weekList" :key="index" :value="item.key"
              :disabled="item.key < cycle01 && item.key !== 1">{{ item.value }}</a-select-option>
          </a-select>
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="4">
          第
          <a-input-number size="small" v-model="average01" :min="1" :max="4" /> 周的星期
          <a-select clearable size="small" v-model="average02" style="width:100px">
            <a-select-option v-for="(item, index) of weekList" :key="index" :value="item.key">{{ item.value
              }}</a-select-option>
          </a-select>
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="5">
          本月最后一个星期
          <a-select style="width:100px" clearable size="small" v-model="weekday">
            <a-select-option v-for="(item, index) of weekList" :key="index" :value="item.key">{{ item.value
              }}</a-select-option>
          </a-select>
        </a-radio>
      </a-form-item>

      <a-form-item>
        <a-radio :value="6">
          指定
          <a-select clearable size="small" v-model="checkboxList" placeholder="可多选" mode="multiple" style="width: 100%">
            <a-select-option v-for="(item, index) of weekList" :key="index" :value="item.key">{{
      item.value }}</a-select-option>
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
      radioValue: 2,
      weekday: 2,
      cycle01: 2,
      cycle02: 3,
      average01: 1,
      average02: 2,
      checkboxList: [],
      weekList: [
        {
          key: 2,
          value: "星期一"
        },
        {
          key: 3,
          value: "星期二"
        },
        {
          key: 4,
          value: "星期三"
        },
        {
          key: 5,
          value: "星期四"
        },
        {
          key: 6,
          value: "星期五"
        },
        {
          key: 7,
          value: "星期六"
        },
        {
          key: 1,
          value: "星期日"
        }
      ],
      checkNum: this.check
    };
  },
  name: "crontab-week",
  props: ["check", "cron"],
  methods: {
    // 单选按钮值变化时
    radioChange() {
      if (this.radioValue !== 2 && this.cron.day !== "?") {
        this.$emit("update", "day", "?", "week");
      }
      switch (this.radioValue) {
        case 1:
          this.$emit("update", "week", "*");
          break;
        case 2:
          this.$emit("update", "week", "?");
          break;
        case 3:
          this.$emit("update", "week", this.cycleTotal);
          break;
        case 4:
          this.$emit("update", "week", this.averageTotal);
          break;
        case 5:
          this.$emit("update", "week", this.weekdayCheck + "L");
          break;
        case 6:
          this.$emit("update", "week", this.checkboxString);
          break;
      }
    },

    // 周期两个值变化时
    cycleChange() {
      if (this.radioValue == "3") {
        this.$emit("update", "week", this.cycleTotal);
      }
    },
    // 平均两个值变化时
    averageChange() {
      if (this.radioValue == "4") {
        this.$emit("update", "week", this.averageTotal);
      }
    },
    // 最近工作日值变化时
    weekdayChange() {
      if (this.radioValue == "5") {
        this.$emit("update", "week", this.weekday + "L");
      }
    },
    // checkbox值变化时
    checkboxChange() {
      if (this.radioValue == "6") {
        this.$emit("update", "week", this.checkboxString);
      }
    }
  },
  watch: {
    radioValue: "radioChange",
    cycleTotal: "cycleChange",
    averageTotal: "averageChange",
    weekdayCheck: "weekdayChange",
    checkboxString: "checkboxChange"
  },
  computed: {
    // 计算两个周期值
    cycleTotal: function () {
      this.cycle01 = this.checkNum(this.cycle01, 1, 7);
      this.cycle02 = this.checkNum(this.cycle02, 1, 7);
      return this.cycle01 + "-" + this.cycle02;
    },
    // 计算平均用到的值
    averageTotal: function () {
      this.average01 = this.checkNum(this.average01, 1, 4);
      this.average02 = this.checkNum(this.average02, 1, 7);
      return this.average02 + "#" + this.average01;
    },
    // 最近的工作日（格式）
    weekdayCheck: function () {
      this.weekday = this.checkNum(this.weekday, 1, 7);
      return this.weekday;
    },
    // 计算勾选的checkbox值合集
    checkboxString: function () {
      let str = this.checkboxList.join();
      return str == "" ? "*" : str;
    }
  }
};
</script>
