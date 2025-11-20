<template>
  <el-container class="app-container" direction="vertical">
    <el-header class="app-header" height="auto">
      <div class="header-content">
        <h1>Система управления заявками на займы</h1>
        <el-menu
          mode="horizontal"
          :default-active="$route.path"
          router
          class="header-menu"
        >
          <el-menu-item index="/">
            <el-icon><List /></el-icon>
            <span>Список заявок</span>
          </el-menu-item>
          <el-menu-item index="/create">
            <el-icon><Plus /></el-icon>
            <span>Создать заявку</span>
          </el-menu-item>
        </el-menu>
      </div>
    </el-header>
    <el-main class="app-main">
      <router-view />
    </el-main>
  </el-container>
</template>

<script setup>
import { List, Plus } from '@element-plus/icons-vue'
</script>

<style scoped>
.app-container {
  min-height: 100vh;
  background: #f5f7fa;
  display: flex;
  flex-direction: column;
}

.app-header {
  background: #fff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 0;
  height: auto !important;
  z-index: 100;
}

.header-content {
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 20px;
  width: 100%;
}

.header-content h1 {
  margin: 20px 0 10px;
  font-size: 24px;
  color: #303133;
}

.header-menu {
  border-bottom: none;
}

.app-main {
  max-width: 1400px;
  margin: 0 auto;
  padding: 30px 20px;
  width: 100%;
  flex: 1;
  overflow: auto;
}
</style>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
}
</style>

