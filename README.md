# StarWhisper AR Education Platform

[![Unity Version](https://img.shields.io/badge/Unity-6000.0.34f1-blue.svg)](https://unity.com/)
[![Platform](https://img.shields.io/badge/Platform-iOS%20%7C%20Android-green.svg)]()
[![AR Foundation](https://img.shields.io/badge/AR%20Foundation-5.0-orange.svg)]()
[![License](https://img.shields.io/badge/License-MIT-red.svg)](LICENSE)

> 基于《星语低语》IP的沉浸式AR教育平台，支持手机和平板双端使用

![StarWhisper Banner](https://via.placeholder.com/800x200/4CAF50/FFFFFF?text=StarWhisper+AR+Education)

## 🌟 项目特色

- 🚀 **《星语低语》沉浸式学习** - 学生化身太空探索者，在外星基地学习知识
- 📱 **智能双端适配** - 手机全屏AR + 平板分屏模式，完美适配不同设备
- 🤖 **AI驱动个性化** - 基于Decart-XR + Graph RAG的智能教育内容生成
- 🎯 **多学科支持** - 数学、科学、历史、语文等核心学科AR场景
- 👶 **年龄自适应** - 3-12岁分层词汇和概念，个性化学习体验

## 🎮 立即体验

### 📱 方法1：APK直接安装（推荐）

```bash
# 下载预编译的APK文件
wget https://github.com/your-username/StarWhisper-Education/releases/latest/download/StarWhisper-Android.apk

# 安装到Android设备
adb install StarWhisper-Android.apk
```

### 🛠️ 方法2：Unity开发者模式

```bash
# 1. 克隆项目
git clone https://github.com/your-username/StarWhisper-Education.git
cd StarWhisper-Education

# 2. 安装Unity 6
# 下载Unity Hub: https://unity.com/unity-hub
# 安装Unity 6000.0.34f1 + Android/iOS Build Support

# 3. 打开项目
# Unity Hub -> Add -> 选择 Mobile/StarWhisper-AR 文件夹

# 4. 构建到设备
# File -> Build Settings -> Android/iOS -> Build and Run
```

### 🌐 方法3：Web演示版

```bash
# 访问在线演示
https://your-username.github.io/StarWhisper-Education/

# 或本地运行Web版本
cd WebGL-Build
python -m http.server 8000
# 浏览器打开: http://localhost:8000
```

## 🎯 快速测试功能

### ✅ 5分钟快速体验

1. **设备检测** - 自动识别手机/平板并优化界面
2. **AR学习场景** - 点击"开始学习体验"
3. **3D交互** - 触摸蓝色立方体学习几何知识
4. **《星语低语》风格** - 体验太空探索学习氛围
5. **实时分析** - 查看学习数据和设备性能

### 📊 演示内容

- 🔍 **智能设备适配演示**
- 🎨 **响应式UI界面演示**
- 📝 **教育Prompt生成演示**
- 🎯 **3D AR对象交互演示**
- 📱 **双端兼容性演示**

## 🏗️ 技术架构

### 核心技术栈
- **Unity 6** - 跨平台AR开发引擎
- **AR Foundation** - Unity官方AR框架
- **Decart-XR** - AI驱动的实时场景变换
- **C#** - 主要开发语言
- **Graph RAG** - 智能知识检索系统

### 项目结构
```
StarWhisper-Education/
├── 📱 Mobile/StarWhisper-AR/           # Unity主项目
│   ├── Assets/StarWhisper/Scripts/     # C#核心代码
│   │   ├── StarWhisperEducationPlatform.cs    # 主控制器
│   │   ├── DeviceAdaptationManager.cs         # 设备适配
│   │   ├── ResponsiveUIManager.cs              # 响应式UI
│   │   ├── DecartXRMobileController.cs         # AR控制
│   │   ├── EducationalPromptEngine.cs          # 教育AI
│   │   └── StarWhisperDemo.cs                 # 测试演示
│   └── ProjectSettings/                # Unity项目设置
├── 🌐 Backend/                        # 后端服务（开发中）
├── 💻 Frontend/                       # 教师Web端（计划中）
└── 📖 Docs/                          # 技术文档
```

### 核心组件

#### 📱 设备适配系统
```csharp
// 自动检测设备类型并优化配置
DeviceType detectedType = DetectDeviceType();
if (detectedType == DeviceType.Tablet) {
    EnableHighQualityRendering();
    EnableMultiTouchInteraction();
} else {
    OptimizeForBattery();
    EnableSingleTouchMode();
}
```

#### 🎨 响应式UI管理
```csharp
// 不同设备的UI布局自动适配
if (deviceType == DeviceType.Tablet) {
    // 平板：AR区域70% + 知识面板30%
    arViewport.anchorMax = new Vector2(0.7f, 1f);
    knowledgePanel.SetActive(true);
} else {
    // 手机：AR全屏显示
    arViewport.anchorMax = new Vector2(1f, 1f);
    knowledgePanel.SetActive(false);
}
```

#### 🧠 教育AI引擎
```csharp
// 个性化教育内容生成
var prompt = await promptEngine.GenerateEducationalPrompt(
    knowledge: conceptKnowledge,
    student: currentStudent,
    difficulty: adaptiveDifficulty,
    deviceType: detectedDeviceType
);
```

## 🚀 开发路线图

### ✅ Phase 1 - MVP基础 (已完成)
- [x] 项目基础架构
- [x] 双端设备适配系统
- [x] 响应式UI管理器
- [x] Decart-XR移动端集成
- [x] 教育Prompt引擎
- [x] 测试演示程序

### 🔄 Phase 2 - 核心功能 (开发中)
- [ ] Graph RAG知识图谱集成
- [ ] 真实AR学习场景
- [ ] 学习分析和数据追踪
- [ ] 多学科内容扩展
- [ ] 用户管理系统

### 📋 Phase 3 - 完整平台 (计划中)
- [ ] 后端API服务
- [ ] 教师Web管理平台
- [ ] 家长微信小程序
- [ ] 高级AI功能
- [ ] 多人协作学习

## 🔧 环境要求

### 开发环境
- **Unity 6** (6000.0.34f1 LTS)
- **Visual Studio 2022** 或 **JetBrains Rider**
- **Android Studio** (Android开发)
- **Xcode 15+** (iOS开发)

### 目标设备
- **Android 7.0+** (API Level 24+)
- **iOS 12.0+**
- **ARCore/ARKit支持**
- **最低2GB RAM**

### 网络要求
- **8+ Mbps** 网络连接
- **Decart AI API** 访问权限

## 📊 性能指标

- 🎯 **帧率**: 30-60 fps
- 🧠 **内存**: < 2GB
- ⚡ **延迟**: < 200ms
- 🔋 **电池**: < 30%/小时
- 📱 **兼容性**: 95%+ 现代设备

## 🤝 贡献指南

### 快速开始
1. Fork 这个项目
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 创建 Pull Request

### 开发规范
- 遵循 [Unity C# 编码规范](https://unity.com/how-to/naming-and-code-style-tips-c-scripting-unity)
- 使用语义化版本号
- 添加详细的代码注释
- 编写对应的单元测试

## 📄 许可证

本项目基于 [MIT License](LICENSE) 开源。

## 👨‍💻 开发团队

- **技术架构** - AI驱动的AR教育平台设计
- **教育专家** - 《星语低语》IP内容设计
- **AR开发** - Unity 6 + Decart-XR集成
- **AI工程** - Graph RAG + 个性化学习

## 📞 联系我们

- 📧 **Email**: starwhisper.edu@example.com
- 💬 **Discord**: [StarWhisper Community](https://discord.gg/starwhisper)
- 📱 **微信群**: 添加微信号 `starwhisper-ar`
- 🐛 **Issues**: [GitHub Issues](https://github.com/your-username/StarWhisper-Education/issues)

## 🌟 致谢

感谢以下开源项目的支持：
- [Decart-XR](https://github.com/DecartAI/Decart-XR) - AI驱动的实时AR渲染
- [Unity AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.0/) - AR开发框架
- [《星语低语》](https://starwhisper.ai) - 原创IP支持

---

**让每个孩子都能在《星语低语》的宇宙中快乐学习！** 🌟

[![GitHub stars](https://img.shields.io/github/stars/your-username/StarWhisper-Education.svg?style=social&label=Star)](https://github.com/your-username/StarWhisper-Education)
[![GitHub forks](https://img.shields.io/github/forks/your-username/StarWhisper-Education.svg?style=social&label=Fork)](https://github.com/your-username/StarWhisper-Education/fork)