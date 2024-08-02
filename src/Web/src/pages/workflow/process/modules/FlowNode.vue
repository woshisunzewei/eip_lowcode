<template>
  <div v-if="node.type === 'begin' || node.type === 'end'" :id="node.id" class="common-circle-node" :class="{
    active: isActive(),
    isStart: node.type === 'begin',
    isEnd: node.type === 'end'
  }" :style="{
  top: node.y + 'px',
  left: node.x + 'px',
  cursor: setCursor(currentTool.type)
}" @click.stop="selectNode" @dblclick.stop="dblclickNode" @contextmenu.stop="showNodeContextMenu">
    {{ node.text.text }}
    <a-tooltip :title="node.remark" v-if="node.remark">
      <div style="color: #555;" v-html="node.remark.split('|')[0]"></div>
    </a-tooltip>
  </div>

  <div v-else-if="node.type === 'task' || node.type === 'subprocess'" :id="node.id" class="common-rectangle-node"
    :class="{ active: isActive() }" :style="{
      top: node.y + 'px',
      left: node.x + 'px',
      cursor: setCursor(currentTool.type)
    }" @click.stop="selectNode" @dblclick.stop="dblclickNode" @contextmenu.stop="showNodeContextMenu">
    <a-icon :type="setIcon(node.type)" class="node-icon" />
    {{ node.text.text }}
    <a-tooltip :title="node.remark" v-if="node.remark">
      <div v-html="node.remark.split('|')[0]"></div>
    </a-tooltip>
  </div>

  <div v-else-if="node.type === 'fork'" :id="node.id" class="common-fork-node" :class="{ active: isActive() }" :style="{
    top: node.y + 'px',
    left: node.x + 'px',
    cursor: setCursor(currentTool.type)
  }" @click.stop="selectNode" @dblclick.stop="dblclickNode" @contextmenu.stop="showNodeContextMenu">
    <label>{{ node.text.text }}</label>
  </div>
  <div v-else-if="node.type === 'join'" :id="node.id" class="common-join-node" :class="{ active: isActive() }" :style="{
    top: node.y + 'px',
    left: node.x + 'px',
    cursor: setCursor(currentTool.type)
  }" @click.stop="selectNode" @dblclick.stop="dblclickNode" @contextmenu.stop="showNodeContextMenu">
    <label>{{ node.text.text }}</label>
  </div>
  <div v-else-if="node.type === 'x-lane'" :id="node.id" class="common-x-lane-node" :class="{ active: isActive() }" :style="{
    top: node.y + 'px',
    left: node.x + 'px',
    height: node.height + 'px',
    width: node.width + 'px',
  }">
    <div class="lane-text-div" :style="{ cursor: setCursor(currentTool.type) }" @click.stop="selectNode"
      @dblclick.stop="dblclickNode" @contextmenu.stop="showNodeContextMenu">
      <span class="lane-text">{{ node.text.text }}</span>
    </div>
  </div>

  <div v-else-if="node.type === 'y-lane'" :id="node.id" class="common-y-lane-node" :class="{ active: isActive() }" :style="{
    top: node.y + 'px',
    left: node.x + 'px',
    height: node.height + 'px',
    width: node.width + 'px',
  }">
    <div class="lane-text-div" :style="{ cursor: setCursor(currentTool.type) }" @click.stop="selectNode"
      @dblclick.stop="dblclickNode" @contextmenu.stop="showNodeContextMenu">
      <span class="lane-text">{{ node.text.text }}</span>
    </div>
  </div>

  <div v-else></div>
</template>

<script>
import { Resizable } from 'resizable-dom'

export default {
  props: ['select', 'selectGroup', 'node', 'plumb', 'currentTool', 'flowConfig', 'processInstanceId'],
  mounted() {
    if (!this.processInstanceId) {
      this.registerNode()
    }

  },
  data() {
    return {
      currentSelect: this.select,
      currentSelectGroup: this.selectGroup
    }
  },
  methods: {
    /**
     * 设置ICON
     * @param {*} type 
     */
    setIcon(type) {
      switch (type) {
        case 'task':
          return 'user'
        case 'freedom':
          return 'sync'
        case 'subprocess':
          return 'apartment'
        default:
          return 'tool'
      }
    },
    /**
     * 设置鼠标样式
     * @param {*} type 
     */
    setCursor(type) {
      switch (type) {
        case 'drag':
          return 'move'
        case 'connection':
          return 'crosshair'
        default:
          return 'default'
      }
    },
    /**
     * 初始节点拖拽
     */
    registerNode() {

      this.plumb.draggable(this.node.id, {
        containment: 'parent',
        handle: (e, el) => {
          // 判断节点类型
          let possibles = el.parentNode.querySelectorAll('.common-circle-node,.common-rectangle-node,.common-join-node,.common-fork-node,.lane-text-div')
          for (let i = 0; i < possibles.length; i++) {
            if (possibles[i] === el || e.target.className === 'lane-text') return true
          }
          return false
        },
        grid: this.flowConfig.defaultStyle.alignGridPX,
        drag: e => {
          if (this.flowConfig.defaultStyle.isOpenAuxiliaryLine) {
            this.$emit('alignForLine', e)
          }
        },
        stop: e => {
          this.node.x = e.pos[0]
          this.node.y = e.pos[1]
          // 是否为组
          if (this.currentSelectGroup.length > 1) {
            // 更新组节点信息
            this.$emit('updateNodePos')
          }
          // 隐藏辅助线
          this.$emit('hideAlignLine')
        }
      })

      if (this.node.type === 'x-lane' || this.node.type === 'y-lane') {
        let node = document.querySelector('#' + this.node.id)
        new Resizable(node, {
          handles: ['e', 'w', 'n', 's', 'nw', 'ne', 'sw', 'se'],
          initSize: {
            maxWidth: 2000,
            maxHeight: 2000,
            minWidth: 200,
            minHeight: 200
          }
        }, () => {
          this.node.height = Math.ceil(parseInt(node.style.height))
          this.node.width = Math.ceil(parseInt(node.style.width))
        })
      }
      this.currentSelect = this.node
      this.currentSelectGroup = []

    },
    /**
     * 点击节点
     */
    selectNode() {
      this.currentSelect = this.node
      this.$emit('isMultiple', flag => {
        if (!flag) {
          this.currentSelectGroup = []
        } else {
          let f = this.currentSelectGroup.find(s => s.id === this.node.id)
          if (!f) {
            this.plumb.addToDragSelection(this.node.id)
            this.currentSelectGroup.push(this.node)
          }
        }
      })
    },
    /**
     * 双击节点
     * @param {*} e 
     */
    dblclickNode(e) {
      this.currentSelect = this.node
      this.$emit('dblclickNode')
    },
    /**
     * 节点右键
     * @param {*} e 
     */
    showNodeContextMenu(e) {
      if (!this.processInstanceId) {
        this.$emit('showNodeContextMenu', e)
        this.selectNode()
      }

    },
    /**
     * 节点是否激活
     */
    isActive() {
      if (this.currentSelect.id === this.node.id) return true
      return !!this.currentSelectGroup.find(n => n.id === this.node.id)
    }
  },
  watch: {
    select(val) {
      this.currentSelect = val
    },
    currentSelect: {
      handler(val) {
        this.$emit('update:select', val)
      },
      deep: true
    },
    selectGroup(val) {
      this.currentSelectGroup = val
    },
    currentSelectGroup: {
      handler(val) {
        this.$emit('update:selectGroup', val)
      },
      deep: true
    }
  }
}
</script>
