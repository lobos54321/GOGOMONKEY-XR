#!/bin/bash

# StarWhisper AR教育平台 - 自动部署脚本
# 支持Android APK构建和GitHub发布

echo "🚀 StarWhisper AR教育平台自动部署开始..."

# 配置变量
PROJECT_NAME="StarWhisper AR Education"
PACKAGE_NAME="com.starwhisper.education"
VERSION_CODE=1
VERSION_NAME="1.0.0"
BUILD_TARGET="Android"

# 检查Unity是否安装
check_unity() {
    echo "🔍 检查Unity安装..."
    if command -v /Applications/Unity/Hub/Editor/2023.3.34f1/Unity.app/Contents/MacOS/Unity &> /dev/null; then
        UNITY_PATH="/Applications/Unity/Hub/Editor/2023.3.34f1/Unity.app/Contents/MacOS/Unity"
        echo "✅ 找到Unity: $UNITY_PATH"
    elif command -v /Applications/Unity/Hub/Editor/6000.0.34f1/Unity.app/Contents/MacOS/Unity &> /dev/null; then
        UNITY_PATH="/Applications/Unity/Hub/Editor/6000.0.34f1/Unity.app/Contents/MacOS/Unity"
        echo "✅ 找到Unity 6: $UNITY_PATH"
    else
        echo "❌ 未找到Unity，请安装Unity 6000.0.34f1"
        echo "下载地址: https://unity.com/unity-hub"
        exit 1
    fi
}

# 检查Android SDK
check_android_sdk() {
    echo "📱 检查Android SDK..."
    if [ -z "$ANDROID_HOME" ]; then
        echo "⚠️ ANDROID_HOME未设置，尝试常见路径..."

        # 尝试常见Android SDK路径
        COMMON_PATHS=(
            "$HOME/Library/Android/sdk"
            "$HOME/Android/Sdk"
            "/usr/local/android-sdk"
        )

        for path in "${COMMON_PATHS[@]}"; do
            if [ -d "$path" ]; then
                export ANDROID_HOME="$path"
                echo "✅ 找到Android SDK: $ANDROID_HOME"
                break
            fi
        done

        if [ -z "$ANDROID_HOME" ]; then
            echo "❌ 未找到Android SDK，请安装Android Studio"
            echo "下载地址: https://developer.android.com/studio"
            exit 1
        fi
    else
        echo "✅ Android SDK: $ANDROID_HOME"
    fi
}

# 构建APK
build_apk() {
    echo "📦 开始构建Android APK..."

    local build_method="BuildScript.BuildAndroid"
    local log_file="build.log"
    local build_dir="Builds/Android"

    # 创建构建目录
    mkdir -p "$build_dir"

    # Unity构建命令
    "$UNITY_PATH" \
        -batchmode \
        -quit \
        -projectPath "$(pwd)/Mobile/StarWhisper-AR" \
        -buildTarget Android \
        -executeMethod "$build_method" \
        -logFile "$log_file" \
        -buildPath "../../$build_dir/StarWhisper-Education.apk"

    if [ $? -eq 0 ]; then
        echo "✅ APK构建成功: $build_dir/StarWhisper-Education.apk"
        return 0
    else
        echo "❌ APK构建失败，查看日志: $log_file"
        return 1
    fi
}

# 构建WebGL版本
build_webgl() {
    echo "🌐 开始构建WebGL版本..."

    local build_method="BuildScript.BuildWebGL"
    local log_file="webgl_build.log"
    local build_dir="Builds/WebGL"

    # 创建构建目录
    mkdir -p "$build_dir"

    # Unity构建命令
    "$UNITY_PATH" \
        -batchmode \
        -quit \
        -projectPath "$(pwd)/Mobile/StarWhisper-AR" \
        -buildTarget WebGL \
        -executeMethod "$build_method" \
        -logFile "$log_file" \
        -buildPath "../../$build_dir"

    if [ $? -eq 0 ]; then
        echo "✅ WebGL构建成功: $build_dir"

        # 创建简单的HTTP服务器启动脚本
        cat > "$build_dir/start_server.sh" << 'EOF'
#!/bin/bash
echo "🌐 启动StarWhisper WebGL演示服务器..."
echo "📱 在浏览器中打开: http://localhost:8000"
python3 -m http.server 8000
EOF
        chmod +x "$build_dir/start_server.sh"

        return 0
    else
        echo "❌ WebGL构建失败，查看日志: $log_file"
        return 1
    fi
}

# 创建GitHub Release
create_github_release() {
    echo "📤 准备GitHub发布..."

    # 检查是否有git和gh
    if ! command -v git &> /dev/null; then
        echo "❌ Git未安装"
        return 1
    fi

    if ! command -v gh &> /dev/null; then
        echo "⚠️ GitHub CLI未安装，跳过自动发布"
        echo "请手动上传构建文件到GitHub Releases"
        echo "APK文件位置: Builds/Android/StarWhisper-Education.apk"
        echo "WebGL文件位置: Builds/WebGL/"
        return 1
    fi

    # 创建Git标签
    local tag_name="v$VERSION_NAME"
    git tag "$tag_name"
    git push origin "$tag_name"

    # 创建GitHub Release
    gh release create "$tag_name" \
        --title "StarWhisper AR Education v$VERSION_NAME" \
        --notes "🚀 StarWhisper AR教育平台 v$VERSION_NAME

## 🎯 新功能
- 📱 智能设备适配（手机/平板双端支持）
- 🎨 响应式AR界面
- 🤖 Decart-XR AI场景变换
- 📝 《星语低语》教育内容引擎
- 🎮 交互式3D学习对象

## 📥 下载和安装

### Android APK
1. 下载 \`StarWhisper-Education.apk\`
2. 在设备上启用\"未知来源\"安装
3. 安装APK并授予摄像头权限
4. 享受AR学习体验！

### WebGL在线版
1. 下载WebGL构建文件
2. 解压并运行 \`start_server.sh\`
3. 浏览器打开 http://localhost:8000
4. 在桌面浏览器中体验演示

## 🔧 系统要求
- Android 7.0+ (API Level 24+)
- ARCore支持
- 摄像头权限
- 网络连接（连接Decart AI）

## 🌟 快速体验
启动应用 → 点击\"开始AR学习体验\" → 触摸蓝色立方体 → 体验《星语低语》学习世界！" \
        "Builds/Android/StarWhisper-Education.apk#StarWhisper-Education.apk"

    echo "✅ GitHub Release创建成功!"
}

# 主函数
main() {
    echo "🎯 StarWhisper AR教育平台自动构建脚本"
    echo "=================================="

    # 检查环境
    check_unity
    check_android_sdk

    # 构建选项
    echo ""
    echo "请选择构建类型:"
    echo "1) Android APK"
    echo "2) WebGL"
    echo "3) 两者都构建"
    echo "4) 仅设置GitHub仓库"
    echo ""
    read -p "选择 (1-4): " choice

    case $choice in
        1)
            echo "📱 开始Android APK构建..."
            build_apk
            ;;
        2)
            echo "🌐 开始WebGL构建..."
            build_webgl
            ;;
        3)
            echo "📦 开始完整构建..."
            build_apk && build_webgl
            ;;
        4)
            echo "⚙️ 仅设置GitHub仓库..."
            ;;
        *)
            echo "❌ 无效选择"
            exit 1
            ;;
    esac

    # GitHub发布（可选）
    echo ""
    read -p "是否创建GitHub Release? (y/n): " create_release
    if [[ $create_release == "y" || $create_release == "Y" ]]; then
        create_github_release
    fi

    echo ""
    echo "🎉 StarWhisper部署完成!"
    echo "=================================="

    if [ -f "Builds/Android/StarWhisper-Education.apk" ]; then
        echo "📱 Android APK: Builds/Android/StarWhisper-Education.apk"
        echo "   安装命令: adb install Builds/Android/StarWhisper-Education.apk"
    fi

    if [ -d "Builds/WebGL" ]; then
        echo "🌐 WebGL版本: Builds/WebGL/"
        echo "   启动命令: cd Builds/WebGL && ./start_server.sh"
    fi

    echo ""
    echo "📚 详细文档: README.md"
    echo "🐛 问题反馈: https://github.com/your-username/StarWhisper-Education/issues"
}

# 运行主函数
main "$@"