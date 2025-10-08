#!/bin/bash

# StarWhisper AR教育平台 - GitHub仓库初始化脚本
# 自动配置Git仓库并推送到GitHub

echo "🚀 StarWhisper GitHub仓库初始化开始..."

# 检查是否已经是Git仓库
if [ ! -d ".git" ]; then
    echo "📁 初始化Git仓库..."
    git init
    echo "✅ Git仓库初始化完成"
else
    echo "✅ 已存在Git仓库"
fi

# 添加所有文件到Git
echo "📦 添加项目文件到Git..."
git add .

# 创建初始提交
echo "💾 创建初始提交..."
git commit -m "🎯 Initial commit: StarWhisper AR Education Platform

🌟 Features:
- 📱 双端支持 (手机/平板自适应)
- 🎮 Decart-XR AR集成
- 📚 教育Prompt引擎
- 🎨 响应式UI系统
- 🔧 设备性能优化

🏗️ Tech Stack:
- Unity 6 (6000.0.34f1)
- AR Foundation
- Decart-XR移动端集成
- C# 教育系统架构

🚀 Ready for testing on mobile and tablet devices!"

# 检查是否配置了远程仓库
echo ""
echo "🌐 检查GitHub远程仓库配置..."
if git remote get-url origin &> /dev/null; then
    echo "✅ 远程仓库已配置: $(git remote get-url origin)"

    # 推送到GitHub
    echo "⬆️ 推送到GitHub..."
    git push -u origin main

    if [ $? -eq 0 ]; then
        echo "✅ 成功推送到GitHub!"
        echo ""
        echo "🎉 StarWhisper项目已上传到GitHub!"
        echo "📱 现在可以通过以下方式测试:"
        echo "   1. Unity开发模式: 打开Mobile/StarWhisper-AR项目"
        echo "   2. 自动构建: 运行 ./deploy.sh"
        echo "   3. 在线查看: 访问你的GitHub仓库"
    else
        echo "❌ 推送失败，请检查GitHub权限"
    fi
else
    echo "⚠️ 未配置GitHub远程仓库"
    echo ""
    echo "请先配置GitHub仓库:"
    echo "1. 在GitHub上创建新仓库 'StarWhisper-Education'"
    echo "2. 运行以下命令添加远程仓库:"
    echo ""
    echo "   git remote add origin https://github.com/YOUR_USERNAME/StarWhisper-Education.git"
    echo "   git branch -M main"
    echo "   git push -u origin main"
    echo ""
    echo "或者使用GitHub CLI:"
    echo "   gh repo create StarWhisper-Education --public"
    echo "   git remote add origin https://github.com/YOUR_USERNAME/StarWhisper-Education.git"
    echo "   git push -u origin main"
fi

echo ""
echo "📖 完整文档请查看: README.md"
echo "🔧 部署脚本请运行: ./deploy.sh"
echo "🐛 问题反馈: GitHub Issues"