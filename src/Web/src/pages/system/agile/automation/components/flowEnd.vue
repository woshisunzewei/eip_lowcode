<template>
  <div class="end">流程结束</div>
</template>

<style scoped>
.end {
  position: relative;
  padding: 32px 0;
  font-size: 14px;
  font-weight: bold;
  color: #999;
  text-align: center;
}

.end::before {
  content: "";
  position: absolute;
  top: 10px;
  left: 50%;
  transform: translate(-50%, -50%);
  background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAHAAAABACAQAAABokzttAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAAmJLR0QA/4ePzL8AAAAHdElNRQfiCAkRLQN+59ZwAAAAfUlEQVRo3u3ZwQ2AIBAAQbFn69ce7qEumWng2BDC49Z1H1s7vz6AQIEC4c/WPfwH13Dg9Nudztv+DQqsE1gnsE5gncA6gXUC6wTWCawTWCewTmCdwLo1XRa8vWOYztv+BgXWCawTWCewTmCdwDqBdQLrBNYJrBNYJ7BOYN0Dws4H6yyzXK8AAAAldEVYdGRhdGU6Y3JlYXRlADIwMTgtMDgtMDlUMTc6NDU6MDMrMDg6MDBVMAcOAAAAJXRFWHRkYXRlOm1vZGlmeQAyMDE4LTA4LTA5VDE3OjQ1OjAzKzA4OjAwJG2/sgAAAABJRU5ErkJggg==) no-repeat;
  background-size: 56px 32px;
  height: 32px;
  margin: 0 auto;
  width: 56px;
}
</style>
