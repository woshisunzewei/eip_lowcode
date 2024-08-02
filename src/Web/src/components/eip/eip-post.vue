<template>
  <a-modal width="1170px" v-drag centered :destroyOnClose="true" :maskClosable="false" :visible="visible" :zIndex="1001"
    :bodyStyle="{ padding: '1px', 'background-color': '#f0f2f5' }" :title="title" @cancel="cancel">
    <a-row :gutter="[1, 0]">
      <a-col :span="5">
        <a-card class="eip-admin-card-small" size="small">
          <div slot="title" style="height: 32px; line-height: 32px">
            <a-input-search v-model="organization.key" :allowClear="true" placeholder="名称/拼音模糊搜索"
              @change="organizationSearch" />
          </div>
          <a-spin :spinning="organization.spinning">
            <div :style="{ height: '514px', overflow: 'auto' }">
              <a-directory-tree :expandAction="false" v-if="!organization.spinning"
                :defaultExpandedKeys="[organization.data.length > 0 ? organization.data[0].key : '']"
                :tree-data="organization.data" @select="treeSelect"></a-directory-tree>
            </div>
          </a-spin>
        </a-card>
      </a-col>

      <a-col :span="19">
        <a-spin :spinning="post.spinning">
          <a-row>
            <a-col :span="12">
              <a-card size="small" :hoverable="true">
                <div slot="title">
                  <a-badge :offset="[8, 0]" :count="posts.filter((f) => !f.exist).length">
                    {{ post.org }} 待选岗位
                  </a-badge>
                </div>
                <div slot="extra">
                  <a-space>
                    <a-input-search v-model="post.key" placeholder="名称模糊查询..." allowClear style="width: 200px" />
                  </a-space>
                </div>
                <vxe-table row-id="postId" border id="chosenPost" ref="chosenPost" :data="posts.filter((f) => !f.exist)"
                  :height="height" :tooltip-config="tooltipConfig">
                  <template #loading>
                    <a-spin></a-spin>
                  </template>
                  <template #empty>
                    <eip-empty />
                  </template>
                  <vxe-column type="seq" title="序号" align="center" width="50" showOverflow="ellipsis"></vxe-column>
                  <vxe-column align="center" width="60" showOverflow="ellipsis">
                    <template #header>
                      <a-button @click="chosenPostCheckAddAll()" size="small" type="dashed">
                        <a-icon type="plus" />
                      </a-button>
                    </template>
                    <template v-slot="{ row }">
                      <a-button @click="chosenPostCheckAdd(row)" size="small" type="dashed">
                        <a-icon type="plus" />
                      </a-button>
                    </template>
                  </vxe-column>

                  <vxe-column field="name" title="名称" width="150" showOverflow="ellipsis"></vxe-column>
                  <vxe-column field="organizationName" title="组织" width="200" showOverflow="ellipsis"></vxe-column>
                </vxe-table>
              </a-card>
            </a-col>
            <a-col :span="12"><a-card size="small" :hoverable="true">
                <div slot="title">
                  <a-badge :offset="[8, 0]" :count="post.data.filter((f) => f.exist).length">
                    已选岗位
                  </a-badge>
                </div>
                <div slot="extra">
                  <a-space>
                    <a-input-search v-model="post.checkkey" placeholder="名称模糊查询..." allowClear style="width: 200px" />
                  </a-space>
                </div>
                <vxe-table row-id="postId" border id="chosenPostCheck" ref="chosenPostCheck" :data="postsExist"
                  :height="height" :tooltip-config="tooltipConfig">
                  <template #loading>
                    <a-spin></a-spin>
                  </template>
                  <template #empty>
                    <eip-empty />
                  </template>
                  <vxe-column type="seq" title="序号" align="center" width="50" showOverflow="ellipsis"></vxe-column>
                  <vxe-column align="center" width="60" showOverflow="ellipsis">
                    <template #header>
                      <a-button @click="chosenPostCheckDelAll()" size="small" type="danger">
                        <a-icon type="delete" />
                      </a-button>
                    </template>
                    <template v-slot="{ row }">
                      <a-button @click="chosenPostCheckDel(row)" size="small" type="danger">
                        <a-icon type="delete" />
                      </a-button>
                    </template>
                  </vxe-column>
                  <vxe-column field="name" title="名称" width="150" showOverflow="ellipsis"></vxe-column>
                  <vxe-column field="organizationName" title="组织" width="200" showOverflow="ellipsis"></vxe-column>
                </vxe-table>
              </a-card>
            </a-col>
          </a-row>
        </a-spin>
      </a-col>
    </a-row>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
  </a-modal>
</template>

<script>
import { organization, post } from "@/services/common/usercontrol/chosenpost";

export default {
  name: "eip-post",
  data() {
    return {
      tooltipConfig: {
        showAll: true,
        enterable: true,
        contentMethod: ({ type, column, row, items, _columnIndex }) => {
          const { property } = column;
          if (row && property === "organizationName") {
            return row['parentIdsName'];
          }
          return null;
        }
      },
      height: "500px",
      organization: {
        data: [],
        key: null,
        original: [],
        spinning: true,
      },
      selectRows: [],
      post: {
        org: "",
        data: [],
        key: "",
        checkkey: '',
        orgId: [],
        spinning: false,
      },
      loading: false,
    };
  },
  computed: {
    posts() {
      var arr = [];
      this.post.data.forEach((item) => arr.push(item));
      if (this.post.key) {
        arr = this.post.data.filter(
          (item) =>
            item.name.includes(this.post.key)
        );
      }
      if (this.post.orgId.length > 0) {
        var datas = []
        this.post.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            itemFilter.organizationId == item
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      if (this.post.key && this.post.orgId.length > 0) {
        var datas = []
        this.post.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            itemFilter.name.includes(this.post.key) &&
            itemFilter.organizationId == item
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      return arr;
    },

    /**
    * 已选中岗位
    */
    postsExist() {
      var arr = [];
      this.post.data.filter(f => f.exist).forEach((item) => arr.push(item));
      if (this.post.checkkey) {
        arr = arr.filter(
          (item) =>
            item.name.includes(this.post.checkkey)
        );
      }
      if (this.post.orgId.length > 0) {
        var datas = []
        this.post.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            itemFilter.organizationId.includes(item)
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      if (this.post.checkkey && this.post.orgId.length > 0) {
        var datas = []
        this.post.orgId.forEach(item => {
          var cdata = arr.filter((itemFilter) =>
            (itemFilter.name.includes(this.post.checkkey)) &&
            itemFilter.organizationId.includes(item)
          );
          datas = datas.concat(cdata);
        })
        arr = datas;
      }
      return arr;
    },
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    topOrg: {
      type: String,
    },
    title: {
      type: String,
    },
    chosen: {
      type: Array,
    }, //已选中对象,编辑传入
  },
  mounted() {
    this.organizationInit();
    this.postInit();
  },
  methods: {
    /**
     *
     * @param {*} e
     */
    organizationSearch(e) {
      var that = this;
      this.organization.data = this.$utils.clone(this.organization.original, true);
      this.organization.data = this.$utils.searchTree(
        this.organization.data,
        (item) => {
          if (that.organization.key) {
            var titlePinyin = pinyin.convert(item.title).toLowerCase();
            if (
              item.title
                .toLowerCase()
                .indexOf(that.organization.key.toLowerCase()) > -1
            ) {
              return true;
            } else if (
              titlePinyin.indexOf(that.organization.key.toLowerCase()) > -1
            ) {
              return true;
            } else {
              return false;
            }
          } else {
            return true;
          }

        },
        { children: "children" }
      );
    },

    /**
     * 树选中
     */
    treeSelect(electedKeys, e) {
      var orgId = electedKeys[0];
      var orgIds = [orgId]
      // 递归遍历控件树
      var orgFilters = [];
      const traverseC = (array) => {
        array.forEach((element) => {
          if (element.value != orgId) {
            traverseC(element.children);
          } else {
            orgFilters = element.children;
          }
        });
      };
      traverseC(this.organization.data);
      const traverse = (array) => {
        array.forEach((element) => {
          orgIds.push(element.value)
          if (element.children.length > 0) {
            traverse(element.children);
          }
        });
      };
      traverse(orgFilters);
      this.post.orgId = orgIds;
      this.post.org = e.selectedNodes[0].title;

    },
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 组织架构树
     */
    organizationInit() {
      let that = this;
      that.organization.spinning = true;
      organization({ topOrg: this.topOrg }).then((result) => {
        that.organization.data = result.data;
        that.organization.original = result.data;
        that.organization.spinning = false;
      });
    },

    /**
     * 初始化岗位
     */
    postInit() {
      let that = this;
      that.post.spinning = true;
      post({ topOrg: this.topOrg }).then((result) => {
        that.selectRows = [];
        that.post.spinning = false;
        result.data.forEach((item) => {
          if (that.chosen) {
            item.exist =
              that.chosen.filter((f) => f.id == item.roleId).length > 0;
          } else {
            item.exist = false;
          }
        });
        that.post.data = result.data;
      });
    },

    /**
     * 保存
     */
    save() {
      this.selectRows = this.post.data.filter((f) => f.exist);
      this.$emit("ok", this.selectRows);
      this.cancel();
    },

    /**
     * 添加对应人
     * @param {} row 
     */
    chosenPostCheckAdd(row) {
      row.exist = true;
    },

    /**
     * 添加所有
     */
    chosenPostCheckAddAll() {
      var getTableData = this.$refs.chosenPost.getTableData();
      getTableData.fullData.filter(f => !f.exist).forEach(item => {
        item.exist = true
      })
    },

    /**
     * 已选清除
     */
    chosenPostCheckDel(row) {
      row.exist = false;
    },

    /**
     * 清空所有
     */
    chosenPostCheckDelAll() {
      var getTableData = this.$refs.chosenPostCheck.getTableData();
      getTableData.fullData.forEach(item => {
        item.exist = false
      })
    }
  },
};
</script>