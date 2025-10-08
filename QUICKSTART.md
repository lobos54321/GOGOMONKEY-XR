# StarWhisper AR Education - 快速开始指南

## 🚀 立即测试 (3种方法)

### 方法1: GitHub上传 + Unity开发测试 (推荐)

```bash
# 1. 初始化Git并上传到GitHub
./setup_github.sh

# 2. 打开Unity项目
# Unity Hub -> Add -> 选择 Mobile/StarWhisper-AR

# 3. 连接手机/平板测试
# File -> Build Settings -> Android -> Build and Run
```

### 方法2: 直接构建APK

```bash
# 运行自动部署脚本
./deploy.sh

# 选择选项1: Android APK
# 构建完成后安装: adb install Builds/Android/StarWhisper-Education.apk
```

### 方法3: WebGL浏览器演示

```bash
./deploy.sh
# 选择选项2: WebGL
# 运行: cd Builds/WebGL && ./start_server.sh
# 浏览器打开: http://localhost:8000
```

## 📱 设备要求

- **Android 7.0+** (API Level 24+)
- **摄像头权限** (AR功能)
- **网络连接** (Decart AI)
- **最低2GB RAM**

## ⚡ 快速验证功能

1. **设备检测** - 启动自动识别手机/平板
2. **AR界面** - 点击"开始AR学习体验"
3. **3D交互** - 触摸蓝色立方体
4. **设备信息** - 查看右上角设备适配状态

## 🔧 如果遇到问题

1. **Unity未找到**: 安装Unity 6000.0.34f1
2. **Android SDK错误**: 安装Android Studio
3. **构建失败**: 检查build.log日志
4. **设备不识别**: 启用USB调试

## 📞 获取帮助

- 📋 查看详细文档: README.md
- 🐛 报告问题: GitHub Issues
- 💬 技术支持: 留言或创建Issue