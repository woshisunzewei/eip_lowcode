<template>
  <a-modal :zIndex="1001" width="700px" v-drag centered :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" title="流程处理" @cancel="cancel">
    <div style="padding: 10px">
      <a-spin :spinning="spinning">
        <a-card :bodyStyle="{ 'max-height': '300px', 'overflow-y': 'auto' }" :hoverable="true" size="small"
          v-for="(item, index) in person" :key="index">
          <div slot="title">
            <a-badge :count="form.person.length">
              {{ item.activity.title }}
            </a-badge>
          </div>
          <div slot="extra">
            <!-- <a-space>
              <a-input-search placeholder="名称模糊查询..." allowClear style="width: 200px" />
            </a-space> -->
          </div>
          <a-checkbox-group v-model="form.person" v-if="item.persons.length > 0 && item.persons[0].pattern == '1'">
            <a-row>
              <a-col :span="6" v-for="(pitem, pindex) in item.persons" :key="pindex">
                <div class="padding-bottom-sm">
                  <a-checkbox style="width:100%" :value="pitem.userId">
                    <img style="width: 32px; height: 32px; border-radius: 50%;" v-real-img="pitem.headImage" />
                    <span class="margin-left-sm">{{ pitem.name }}</span>
                    <!-- <span>【{{ pitem.organizationName }}】</span> -->
                  </a-checkbox>
                </div>
              </a-col>
            </a-row>
          </a-checkbox-group>

          <a-radio-group @change="radioGroupChange" v-if="item.persons.length > 0 && item.persons[0].pattern != '1'">
            <a-row>
              <a-col :span="6" v-for="(pitem, pindex) in item.persons" :key="pindex">
                <div class="padding-bottom-sm">
                  <a-radio :value="pitem.userId">
                    <img style="width: 32px; height: 32px; border-radius: 50%;" v-real-img="pitem.headImage" />
                    <span class="margin-left-sm">{{ pitem.name }}</span>
                    <!-- <span>【{{ pitem.organizationName }}】</span> -->
                  </a-radio>
                </div>
              </a-col>
            </a-row>
          </a-radio-group>
        </a-card>
        <a-card style="margin-top: 4px" size="small" title="通知类型" :hoverable="true" v-if="notice.length > 0">
          <a-checkbox-group v-model="form.notice">
            <a-row>
              <a-col :span="6" v-for="( item, index ) in notice " :key="index">
                <div class="padding-bottom-sm">
                  <a-checkbox :value="index">
                    {{ item.remark }}
                  </a-checkbox>
                </div>
              </a-col>
            </a-row>
          </a-checkbox-group>
        </a-card>
      </a-spin>
    </div>

    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
  </a-modal>
</template>

<script>
export default {
  name: "start",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        notice: [],
        person: [],
      },
      radioStyle: {
        display: "block",
        height: "30px",
        lineHeight: "30px",
      },
      loading: false,
      spinning: false,
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
    },

    notice: {
      type: Array,
    },
    person: {
      type: Array,
    },
  },
  mounted() {
    this.init();
  },
  methods: {
    /**
     * 单选确认
     */
    radioGroupChange(e) {
      this.form.person.push(e.target.value)
    },

    /**
     *初始化通知
     */
    init() {
      let that = this;
      for (let index = 0; index < that.notice.length; index++) {
        that.form.notice.push(index);
      }
    },

    /**
     * 关闭
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      this.$emit("confirm", that.form);
    },
  },
};
</script>

<style lang="less" scoped>
/deep/ .ant-checkbox-group {
  width: 100% !important;
}

/deep/ .ant-radio-group {
  width: 100% !important;
}
</style>