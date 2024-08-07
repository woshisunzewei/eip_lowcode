<template>

  <div class="fx-field " v-if="record.options.style == 1">
    <div class="field-label" :style="{ color: record.options.titleColor }">
      <div class="field-name">{{ record.label }}</div>
    </div>
    <div class="field-component">
      <div class="eip-form-separator">
        <div class="sep-line none" :style="{ 'border-color': record.options.backgroundColor }"></div>
        <div class="fx-sub-html" v-html="record.options.remark"></div>
      </div>
    </div>
  </div>

  <div class="fx-field " v-else-if="record.options.style == 2">
    <div class="field-label" :style="{ color: record.options.titleColor }">
      <div class="field-name">{{ record.label }}</div>
    </div>
    <div class="field-component">
      <div class="eip-form-separator">
        <div class="sep-line dashed" :style="{ 'border-color': record.options.backgroundColor }"></div>
        <div class="fx-sub-html" v-html="record.options.remark"></div>
      </div>
    </div>
  </div>

  <div class="fx-field " v-else-if="record.options.style == 3">
    <div class="field-label" :style="{ color: record.options.titleColor }">
      <div class="field-name">{{ record.label }}</div>
    </div>
    <div class="field-component">
      <div class="eip-form-separator">
        <div class="sep-line thin" :style="{ 'border-color': record.options.backgroundColor }"></div>
        <div class="fx-sub-html" v-html="record.options.remark"></div>
      </div>
    </div>
  </div>

  <div class="fx-field " v-else-if="record.options.style == 4">
    <div class="field-label" :style="{ color: record.options.titleColor }">
      <div class="field-name">{{ record.label }}</div>
    </div>
    <div class="field-component">
      <div class="eip-form-separator">
        <div class="sep-line thick" :style="{ 'border-color': record.options.backgroundColor }"></div>
        <div class="fx-sub-html" v-html="record.options.remark"></div>
      </div>
    </div>
  </div>

  <div class="fx-field " v-else-if="record.options.style == 5">
    <div class="field-component">
      <div class="eip-form-separator">
        <div class="sep-line style1">
          <div class="sep-label"
            :style="{ color: record.options.titleColor, 'background-color': record.options.backgroundColor }"><span>{{
              record.label }}</span>
          </div>
          <div class="sep-right">
            <div class="right-triangle" :style="{ 'background-color': record.options.backgroundColor }"></div>
            <div class="right-line" :style="{ 'border-color': record.options.backgroundColor }"></div>
            <div class="right-parallelogram1" :style="{ 'background-color': record.options.backgroundColor }"></div>
            <div class="right-parallelogram2" :style="{ 'background-color': record.options.backgroundColor }"></div>
            <div class="right-parallelogram3" :style="{ 'background-color': record.options.backgroundColor }"></div>
          </div>
        </div>
        <div class="fx-sub-html" v-html="record.options.remark"></div>
      </div>
    </div>
  </div>

  <div class="fx-field " v-else-if="record.options.style == 6">
    <div class="field-component">
      <div class="eip-form-separator">
        <div class="sep-line style3">
          <div class="sep-bg-wrapper">
            <div class="sep-bg" :style="{ 'background-color': record.options.backgroundColor }"></div>
          </div>
          <div class="sep-label"
            :style="{ color: record.options.titleColor, 'background-color': record.options.backgroundColor }">
            <span>{{
              record.label }}</span>
          </div>
        </div>
        <div class="fx-sub-html" v-html="record.options.remark"></div>
      </div>
    </div>
  </div>

  <div class="fx-field " v-else-if="record.options.style == 7">
    <div class="field-component">
      <div class="eip-form-separator">
        <div class="sep-line style4">
          <div class="sep-bg" :style="{ 'background-color': record.options.backgroundColor }"></div>
          <div class="sep-center">
            <div class="sep-label"
              :style="{ color: record.options.titleColor, 'background-color': record.options.backgroundColor }">
              <span>{{
                record.label }}</span>
            </div>
            <div class="left-triangle" :style="{ 'background-color': record.options.backgroundColor }"></div>
            <div class="left-border" :style="{ 'background-color': record.options.backgroundColor }"></div>
            <div class="right-triangle" :style="{ 'background-color': record.options.backgroundColor }"></div>
            <div class="right-border" :style="{ 'background-color': record.options.backgroundColor }"></div>
          </div>
        </div>
        <div class="fx-sub-html" v-html="record.options.remark"></div>
      </div>
    </div>
  </div>

  <div class="fx-field " v-else-if="record.options.style == 8">
    <div class="field-component">
      <div class="eip-form-separator">
        <div class="sep-line style5">
          <div class="left-wrapper">
            <div class="left-block" :style="{ 'background-color': record.options.backgroundColor }"></div>
            <div class="left-triangle" :style="{ 'background-color': record.options.backgroundColor }"></div>
            <div class="left-square"></div>
            <div class="left-line1" :style="{ 'border-color': record.options.backgroundColor }"></div>
            <div class="left-line2" :style="{ 'border-color': record.options.backgroundColor }"></div>
          </div>
          <div class="sep-label"
            :style="{ color: record.options.titleColor, 'background-color': record.options.backgroundColor }">
            <span>{{
              record.label }}</span>
          </div>
          <div class="right-wrapper">
            <div class="right-block" :style="{ 'background-color': record.options.backgroundColor }"></div>
            <div class="right-triangle" :style="{ 'background-color': record.options.backgroundColor }"></div>
            <div class="right-square"></div>
            <div class="right-line1" :style="{ 'border-color': record.options.backgroundColor }"></div>
            <div class="right-line2" :style="{ 'border-color': record.options.backgroundColor }"></div>
          </div>
        </div>
        <div class="fx-sub-html" v-html="record.options.remark"></div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "KDivider",
  props: ["record"],
};
</script>
<style>
.fx-field {
  line-height: 20px;
  position: relative
}

.eip-form-separator .fx-sub-html {
  color: #141e31;
  font-size: 14px;
  line-height: 1.4;
  margin-top: 2px;
}

.fx-sub-html {
  word-wrap: break-word;
  color: #141e31;
  font-size: 12px;
  line-height: 1.4;
  word-break: break-word;
}

.fx-field .field-label.single-line {
  padding-right: 19%
}

.fx-data-share .divider {
  background: #ebecee;
  height: 1px;
  margin: 20px 0
}

.fx-data-share .single-link-switch {
  margin-bottom: 10px
}

.eip-form-separator .sep-line {
  height: 0
}

.eip-form-separator .sep-line.none {
  border: none
}

.eip-form-separator .sep-line.dashed {
  border-top: 1px dashed
}

.eip-form-separator .sep-line.thin {
  border-top: 1px solid
}

.eip-form-separator .sep-line.thick {
  border-top: 2px solid
}

.eip-form-separator .sep-line.style1 {
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  height: 28px;
  position: relative
}

.eip-form-separator .sep-line.style1 .sep-label {
  -webkit-box-flex: 0;
  -webkit-box-sizing: content-box;
  box-sizing: content-box;
  -ms-flex: none;
  flex: none;
  height: 100%;
  line-height: 28px;
  max-width: 80%;
  min-width: 7%;
  overflow: hidden;
  padding: 0 10px;
  text-overflow: ellipsis;
  white-space: nowrap
}

.eip-form-separator .sep-line.style1 .sep-right {
  -webkit-box-flex: 1;
  -ms-flex: auto;
  flex: auto;
  position: relative
}

.eip-form-separator .sep-line.style1 .sep-right .right-line {
  border-bottom: 3px solid;
  bottom: 0;
  height: 0;
  left: 0;
  position: absolute;
  right: 0
}

.eip-form-separator .sep-line.style1 .sep-right .right-parallelogram1 {
  height: 22px;
  left: 15px;
  opacity: .9;
  position: absolute;
  top: 0;
  -webkit-transform: skew(26deg) translateX(-50%);
  transform: skew(26deg) translateX(-50%);
  width: 8px
}

.eip-form-separator .sep-line.style1 .sep-right .right-parallelogram2 {
  height: 22px;
  left: 27px;
  opacity: .6;
  position: absolute;
  top: 0;
  -webkit-transform: skew(26deg) translateX(-50%);
  transform: skew(26deg) translateX(-50%);
  width: 8px
}

.eip-form-separator .sep-line.style1 .sep-right .right-parallelogram3 {
  height: 22px;
  left: 39px;
  opacity: .3;
  position: absolute;
  top: 0;
  -webkit-transform: skew(26deg) translateX(-50%);
  transform: skew(26deg) translateX(-50%);
  width: 8px
}

.eip-form-separator .sep-line.style1 .sep-right .right-triangle {
  height: 100%;
  -webkit-transform: skew(26deg) translateX(-50%);
  transform: skew(26deg) translateX(-50%);
  width: 15px
}

.eip-form-separator .sep-line.style2 {
  height: 8px;
  position: relative
}

.eip-form-separator .sep-line.style2 .sep-label {
  border-radius: 0 8px;
  bottom: 0;
  left: 0;
  max-width: 81%;
  min-width: 8%;
  overflow: hidden;
  position: absolute;
  text-overflow: ellipsis;
  top: 0;
  white-space: nowrap
}

.eip-form-separator .sep-line.style2 .sep-label>span {
  visibility: hidden
}

.eip-form-separator .sep-line.style2 .sep-bg-wrapper {
  bottom: 0;
  left: 0;
  padding-left: 8px;
  position: absolute;
  right: 0
}

.eip-form-separator .sep-line.style2 .sep-bg-wrapper .sep-bg {
  border-bottom: 4px solid;
  height: 0;
  opacity: .2;
  width: 100%
}

.eip-form-separator .sep-line.style3 {
  height: 28px;
  position: relative
}

.eip-form-separator .sep-line.style3 .sep-label {
  border-radius: 14px 0;
  bottom: 0;
  -webkit-box-sizing: content-box;
  box-sizing: content-box;
  font-weight: 600;
  left: 0;
  line-height: 28px;
  max-width: 76%;
  min-width: 7%;
  overflow: hidden;
  padding: 0 20px;
  position: absolute;
  text-overflow: ellipsis;
  top: 0;
  white-space: nowrap
}

.eip-form-separator .sep-line.style3 .sep-bg-wrapper {
  bottom: 0;
  left: 0;
  padding-left: 20px;
  position: absolute;
  right: 0;
  top: 0
}

.eip-form-separator .sep-line.style3 .sep-bg-wrapper .sep-bg {
  height: 100%;
  opacity: .2;
  width: 100%
}

.eip-form-separator .sep-line.style4 {
  -webkit-box-pack: center;
  -ms-flex-pack: center;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  height: 38px;
  justify-content: center;
  position: relative
}

.eip-form-separator .sep-line.style4 .sep-center {
  -webkit-box-sizing: content-box;
  box-sizing: content-box;
  max-width: 50%;
  min-width: 7%;
  overflow: hidden;
  padding: 0 38px;
  position: relative
}

.eip-form-separator .sep-line.style4 .sep-center .left-border {
  border-radius: 0 0 0 16px;
  bottom: 0;
  left: 18px;
  position: absolute;
  top: 0;
  width: 21px
}

.eip-form-separator .sep-line.style4 .sep-center .right-border {
  border-radius: 0 0 16px;
  bottom: 0;
  position: absolute;
  right: 18px;
  top: 0;
  width: 21px
}

.eip-form-separator .sep-line.style4 .sep-center .left-triangle {
  height: 10px;
  left: 0;
  position: absolute;
  top: 0;
  -webkit-transform: skew(-60deg) translateX(50%);
  transform: skew(-60deg) translateX(50%);
  width: 18px
}

.eip-form-separator .sep-line.style4 .sep-center .right-triangle {
  height: 10px;
  position: absolute;
  right: 0;
  top: 0;
  -webkit-transform: skew(60deg) translateX(-50%);
  transform: skew(60deg) translateX(-50%);
  width: 18px
}

.eip-form-separator .sep-line.style4 .sep-center .sep-label {
  font-weight: 600;
  height: 100%;
  line-height: 38px;
  overflow: hidden;
  padding: 0 1px;
  text-align: center;
  text-overflow: ellipsis;
  white-space: nowrap
}

.eip-form-separator .sep-line.style4 .sep-bg {
  border-radius: 16px 16px 0 0;
  bottom: 0;
  height: 28px;
  left: 0;
  opacity: .15;
  position: absolute;
  right: 0
}

.eip-form-separator .sep-line.style5 {
  -webkit-box-pack: center;
  -ms-flex-pack: center;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  height: 28px;
  justify-content: center;
  overflow: hidden;
  position: relative
}

.eip-form-separator .sep-line.style5 .left-wrapper {
  -webkit-box-flex: 0;
  -ms-flex: none;
  flex: none;
  position: relative;
  width: 151px
}

.eip-form-separator .sep-line.style5 .left-wrapper .left-block {
  bottom: 0;
  position: absolute;
  right: -1px;
  top: 0;
  width: 53px
}

.eip-form-separator .sep-line.style5 .left-wrapper .left-triangle {
  height: 20px;
  position: absolute;
  right: 52px;
  top: 0;
  -webkit-transform: rotate(-45deg);
  transform: rotate(-45deg);
  -webkit-transform-origin: 100% 0;
  transform-origin: 100% 0;
  width: 20px
}

.eip-form-separator .sep-line.style5 .left-wrapper .left-square {
  background: #fff;
  height: 10px;
  position: absolute;
  right: 45px;
  top: 9px;
  -webkit-transform: rotate(45deg);
  transform: rotate(45deg);
  width: 10px
}

.eip-form-separator .sep-line.style5 .left-wrapper .left-line1 {
  border-top: 1px solid;
  height: 0;
  left: 27px;
  position: absolute;
  top: 9px;
  width: 48px
}

.eip-form-separator .sep-line.style5 .left-wrapper .left-line2 {
  border-bottom: 1px solid;
  bottom: 8px;
  height: 0;
  left: 0;
  position: absolute;
  width: 75px
}

.eip-form-separator .sep-line.style5 .sep-label {
  font-weight: 600;
  height: 100%;
  line-height: 28px;
  max-width: 54%;
  min-width: 10%;
  overflow: hidden;
  padding: 0 1px;
  text-align: center;
  text-overflow: ellipsis;
  white-space: nowrap
}

.eip-form-separator .sep-line.style5 .right-wrapper {
  -webkit-box-flex: 0;
  -ms-flex: none;
  flex: none;
  position: relative;
  width: 151px
}

.eip-form-separator .sep-line.style5 .right-wrapper .right-block {
  bottom: 0;
  left: -1px;
  position: absolute;
  top: 0;
  width: 53px
}

.eip-form-separator .sep-line.style5 .right-wrapper .right-triangle {
  height: 20px;
  left: 52px;
  position: absolute;
  top: 0;
  -webkit-transform: rotate(45deg);
  transform: rotate(45deg);
  -webkit-transform-origin: 0 0;
  transform-origin: 0 0;
  width: 20px
}

.eip-form-separator .sep-line.style5 .right-wrapper .right-square {
  background: #fff;
  height: 10px;
  left: 45px;
  position: absolute;
  top: 9px;
  -webkit-transform: rotate(45deg);
  transform: rotate(45deg);
  width: 10px
}

.eip-form-separator .sep-line.style5 .right-wrapper .right-line1 {
  border-top: 1px solid;
  height: 0;
  position: absolute;
  right: 27px;
  top: 9px;
  width: 48px
}

.eip-form-separator .sep-line.style5 .right-wrapper .right-line2 {
  border-bottom: 1px solid;
  bottom: 8px;
  height: 0;
  position: absolute;
  right: 0;
  width: 75px
}

.fx-field .field-label.single-line .field-name {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap
}

.fx-field .field-label {
  word-wrap: break-word;
  color: #141e31;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  font-weight: 600;
  line-height: 20px;
  padding: 5px 0;
  position: relative;
  word-break: break-word
}
</style>