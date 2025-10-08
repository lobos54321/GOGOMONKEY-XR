# StarWhisper AR Education - GitHub推送和Zeabur部署指南

## 🚀 快速部署步骤

### 1. GitHub身份验证和推送

```bash
# 方法1: 使用GitHub CLI (推荐)
gh auth login
cd /Users/channyLiu/StarWhisper-Education
git add .
git commit -m "Add Zeabur deployment configuration"
git push -u origin main
```

```bash
# 方法2: 使用Personal Access Token
# 1. 访问 GitHub.com -> Settings -> Developer settings -> Personal access tokens
# 2. 创建新token，选择repo权限
# 3. 使用token作为密码：
git push https://YOUR_TOKEN@github.com/lobos54321/GOGOMONKEY-XR.git main
```

### 2. Zeabur部署

1. **登录Zeabur控制台**
   - 访问 [zeabur.com](https://zeabur.com)
   - 使用GitHub账号登录

2. **创建新项目**
   - 点击 "New Project"
   - 选择 "Import from GitHub"
   - 选择 `lobos54321/GOGOMONKEY-XR` 仓库

3. **配置部署设置**
   - Framework: `Static Site`
   - Build Command: `./deploy_zeabur.sh`
   - Output Directory: `public`
   - Node Version: `18`

4. **部署并访问**
   - 点击 "Deploy" 开始构建
   - 构建完成后获得访问链接
   - 在手机/平板浏览器中测试AR功能

## 🎯 Zeabur优化配置

已经为你准备了：
- ✅ **package.json** - Node.js项目配置
- ✅ **deploy_zeabur.sh** - 云端WebGL构建脚本
- ✅ **.zeabur/config.json** - Zeabur部署配置
- ✅ **静态文件优化** - CORS和缓存头配置

## 📱 测试访问

部署成功后，你将获得类似这样的链接：
- `https://your-project.zeabur.app`

在手机或平板浏览器中打开即可体验：
- 🎮 AR学习场景
- 📱 设备自适应界面
- 🎨 3D交互体验
- 🤖 AI教育内容

## 🔧 如果遇到问题

1. **GitHub推送失败**
   ```bash
   # 检查远程仓库配置
   git remote -v

   # 重新配置认证
   gh auth login --web
   ```

2. **Zeabur构建失败**
   - 检查构建日志
   - 确认Unity项目路径正确
   - 验证WebGL构建设置

3. **AR功能不工作**
   - 确保使用HTTPS访问
   - 检查摄像头权限
   - 尝试不同浏览器

## 📞 获取帮助

- 📋 GitHub Issues: 报告问题和建议
- 💬 Zeabur文档: [docs.zeabur.com](https://docs.zeabur.com)
- 🎯 Unity WebGL优化: [docs.unity3d.com](https://docs.unity3d.com)