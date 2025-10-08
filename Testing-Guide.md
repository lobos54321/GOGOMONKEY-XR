# 🚀 StarWhisper AR教育平台 - 测试指南

## 📱 如何测试项目

### 方法1：Unity编辑器测试（推荐新手）

#### 🔧 准备工作
```bash
# 1. 下载Unity Hub
https://unity.com/unity-hub

# 2. 安装Unity 6
版本：Unity 6000.0.34f1（LTS）
模块：Android Build Support, iOS Build Support

# 3. 打开项目
Unity Hub -> Add -> 选择文件夹：
/Users/channyLiu/StarWhisper-Education/Mobile/StarWhisper-AR/
```

#### 🎮 编辑器内测试
```bash
# 1. 创建测试场景
- 新建Scene：StarWhisper-Demo
- 添加Main Camera
- 创建Empty GameObject命名为"StarWhisperDemo"
- 将StarWhisperDemo.cs脚本拖拽到对象上

# 2. 运行测试
- 点击Play按钮
- 观察Console窗口的日志输出
- 查看Game窗口的UI界面
- 点击"开始AR学习体验"按钮

# 3. 测试功能
✅ 设备信息自动检测
✅ UI界面自适应
✅ 教育Prompt生成
✅ 3D对象交互
✅ 触摸反馈效果
```

### 方法2：真机测试（完整体验）

#### 📱 构建到手机/平板
```bash
# 1. 配置构建设置
File -> Build Settings
Platform: Android 或 iOS
Switch Platform

# 2. 项目设置
Edit -> Project Settings -> Player
Company Name: StarWhisper
Product Name: StarWhisper AR Education

# 3. Android设置
- Minimum API Level: 24
- Target API Level: 34
- Scripting Backend: IL2CPP
- Target Architectures: ARM64

# 4. iOS设置
- Target minimum iOS Version: 12.0
- Architecture: ARM64
- Camera Usage Description: "AR教育需要使用摄像头"

# 5. 构建并安装
Build and Run -> 选择输出目录
等待编译完成并自动安装到设备
```

### 方法3：快速Web演示（预览版）

#### 🌐 WebGL构建
```bash
# 1. 切换平台
File -> Build Settings -> WebGL -> Switch Platform

# 2. 构建设置
Player Settings -> Publishing Settings
- Compression Format: Gzip
- Memory Size: 512MB

# 3. 构建
Build -> 选择输出目录
等待构建完成

# 4. 本地服务器运行
cd build输出目录
python -m http.server 8000
浏览器打开: http://localhost:8000
```

## 🎯 当前可测试的功能

### ✅ 设备适配演示
```bash
功能：自动检测设备类型和规格
测试：观察设备信息显示是否正确
预期：显示屏幕尺寸、分辨率、设备类型等
```

### ✅ 响应式UI演示
```bash
功能：根据设备类型调整界面布局
测试：在不同尺寸设备上运行
预期：手机显示简化版，平板显示完整版
```

### ✅ 教育Prompt生成演示
```bash
功能：生成《星语低语》风格的学习内容
测试：点击"开始学习体验"按钮
预期：Console显示完整的教育Prompt内容
```

### ✅ 3D AR对象交互演示
```bash
功能：可触摸的3D学习对象
测试：点击蓝色旋转立方体
预期：对象变黄0.3秒，显示学习反馈
```

### ✅ 学习分析演示
```bash
功能：记录学习交互和时间
测试：多次点击3D对象
预期：Console显示交互记录和学习数据
```

## 📊 测试检查清单

### 🔍 基础功能测试
- [ ] 项目成功在Unity中打开
- [ ] 无编译错误和警告
- [ ] 场景正常加载和运行
- [ ] UI界面正确显示
- [ ] 设备信息检测准确

### 🎮 交互功能测试
- [ ] 按钮点击响应
- [ ] 3D对象触摸交互
- [ ] 视觉反馈效果
- [ ] 状态信息更新
- [ ] 学习序列流程

### 📱 设备兼容性测试
- [ ] 手机设备运行正常
- [ ] 平板设备运行正常
- [ ] UI适配不同屏幕尺寸
- [ ] 性能表现良好
- [ ] 内存使用合理

### 🧠 教育功能测试
- [ ] Prompt内容生成正确
- [ ] 《星语低语》风格体现
- [ ] 年龄适配内容合理
- [ ] 学习目标清晰
- [ ] 交互引导明确

## 🐛 常见问题解决

### ❌ Unity项目打不开
```bash
解决方案：
1. 确认Unity版本为6000.0.34f1
2. 检查项目路径是否正确
3. 删除Library文件夹重新导入
4. 重启Unity Hub和Unity编辑器
```

### ❌ 编译错误
```bash
解决方案：
1. 检查脚本语法错误
2. 确认所有必要的包已安装
3. 清理并重新构建项目
4. 检查API兼容性设置
```

### ❌ 真机运行崩溃
```bash
解决方案：
1. 检查设备最低系统要求
2. 确认相机权限已授予
3. 降低图形质量设置
4. 检查内存使用情况
```

### ❌ UI显示异常
```bash
解决方案：
1. 检查Canvas设置
2. 确认UI缩放配置
3. 验证设备分辨率支持
4. 调整SafeArea设置
```

## 📈 性能基准

### 🎯 目标性能指标
```bash
帧率：30-60 fps
内存：< 2GB
启动时间：< 5秒
响应延迟：< 100ms
电池消耗：< 30%/小时
```

### 📊 测试数据收集
```bash
# Unity Profiler监控
Window -> Analysis -> Profiler
- CPU使用率
- 内存分配
- 渲染性能
- 网络延迟

# 设备性能监控
- 帧率稳定性
- 发热情况
- 电池消耗
- 网络流量
```

## 🎉 测试成功标准

### ✅ 基础成功标准
1. 项目无错误编译运行
2. UI界面正确显示和交互
3. 设备信息准确检测
4. 3D对象正常显示和交互
5. 学习内容生成完整

### 🌟 高级成功标准
1. 多设备兼容性良好
2. 性能表现稳定
3. 用户体验流畅
4. 教育内容质量高
5. 《星语低语》风格突出

---

## 🚀 立即开始测试

**最简单的测试方法：**

1. 打开Unity Hub
2. 安装Unity 6
3. 添加项目：`/Users/channyLiu/StarWhisper-Education/Mobile/StarWhisper-AR/`
4. 创建新场景并添加StarWhisperDemo组件
5. 点击Play按钮开始测试！

**5分钟内就能看到StarWhisper AR教育平台的核心功能演示！** ✨