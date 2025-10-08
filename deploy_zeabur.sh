#!/bin/bash

# StarWhisper AR Education - Zeabur云部署脚本
# 专为Zeabur平台优化的WebGL构建和部署

echo "🌐 StarWhisper Zeabur云部署开始..."

# 检查Unity路径
UNITY_PATHS=(
    "/Applications/Unity/Hub/Editor/6000.0.34f1/Unity.app/Contents/MacOS/Unity"
    "/Applications/Unity/Hub/Editor/2023.3.34f1/Unity.app/Contents/MacOS/Unity"
)

UNITY_PATH=""
for path in "${UNITY_PATHS[@]}"; do
    if [ -f "$path" ]; then
        UNITY_PATH="$path"
        echo "✅ 找到Unity: $UNITY_PATH"
        break
    fi
done

if [ -z "$UNITY_PATH" ]; then
    echo "❌ 未找到Unity，使用默认路径"
    UNITY_PATH="/Applications/Unity/Hub/Editor/6000.0.34f1/Unity.app/Contents/MacOS/Unity"
fi

# 创建优化的WebGL构建用于Zeabur部署
echo "🎮 开始构建Zeabur优化的WebGL版本..."

BUILD_DIR="public"
LOG_FILE="zeabur_build.log"

# 清理并创建public目录（Zeabur默认静态文件目录）
rm -rf "$BUILD_DIR"
mkdir -p "$BUILD_DIR"

# Unity WebGL构建命令
echo "⚡ 执行Unity WebGL构建..."
"$UNITY_PATH" \
    -batchmode \
    -quit \
    -projectPath "$(pwd)/Mobile/StarWhisper-AR" \
    -buildTarget WebGL \
    -executeMethod "BuildScript.BuildWebGL" \
    -logFile "$LOG_FILE" \
    -buildPath "../../$BUILD_DIR"

if [ $? -eq 0 ]; then
    echo "✅ WebGL构建成功!"

    # 创建Zeabur部署配置
    echo "⚙️ 创建Zeabur部署配置..."

    # 创建index.html（如果Unity没有生成）
    if [ ! -f "$BUILD_DIR/index.html" ]; then
        cat > "$BUILD_DIR/index.html" << 'EOF'
<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>StarWhisper AR Education Platform</title>
    <link rel="shortcut icon" href="TemplateData/favicon.ico">
    <link rel="stylesheet" href="TemplateData/style.css">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>
<body>
    <div id="unity-container" class="unity-desktop">
        <canvas id="unity-canvas" width=960 height=600></canvas>
        <div id="unity-loading-bar">
            <div id="unity-logo"></div>
            <div id="unity-progress-bar-empty">
                <div id="unity-progress-bar-full"></div>
            </div>
        </div>
        <div id="unity-mobile-warning">
            WebGL builds are not supported on mobile devices.
        </div>
        <div id="unity-footer">
            <div id="unity-webgl-logo"></div>
            <div id="unity-fullscreen-button"></div>
            <div id="unity-build-title">StarWhisper AR Education</div>
        </div>
    </div>
    <script>
        var buildUrl = "Build";
        var loaderUrl = buildUrl + "/StarWhisper-Education.loader.js";
        var config = {
            dataUrl: buildUrl + "/StarWhisper-Education.data.gz",
            frameworkUrl: buildUrl + "/StarWhisper-Education.framework.js.gz",
            codeUrl: buildUrl + "/StarWhisper-Education.wasm.gz",
            streamingAssetsUrl: "StreamingAssets",
            companyName: "StarWhisper",
            productName: "StarWhisper AR Education",
            productVersion: "1.0.0",
        };

        var container = document.querySelector("#unity-container");
        var canvas = document.querySelector("#unity-canvas");
        var loadingBar = document.querySelector("#unity-loading-bar");
        var progressBarFull = document.querySelector("#unity-progress-bar-full");
        var fullscreenButton = document.querySelector("#unity-fullscreen-button");
        var mobileWarning = document.querySelector("#unity-mobile-warning");

        if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
            container.className = "unity-mobile";
            config.devicePixelRatio = 1;
            mobileWarning.style.display = "block";
            setTimeout(() => {
                mobileWarning.style.display = "none";
            }, 5000);
        } else {
            canvas.style.width = "960px";
            canvas.style.height = "600px";
        }
        loadingBar.style.display = "block";

        var script = document.createElement("script");
        script.src = loaderUrl;
        script.onload = () => {
            createUnityInstance(canvas, config, (progress) => {
                progressBarFull.style.width = 100 * progress + "%";
            }).then((unityInstance) => {
                loadingBar.style.display = "none";
                fullscreenButton.onclick = () => {
                    unityInstance.SetFullscreen(1);
                };
            }).catch((message) => {
                alert(message);
            });
        };
        document.body.appendChild(script);
    </script>
</body>
</html>
EOF
    fi

    # 创建.zeabur配置
    cat > ".zeabur/config.json" << 'EOF'
{
  "build": {
    "commands": ["chmod +x deploy_zeabur.sh", "./deploy_zeabur.sh"]
  },
  "deploy": {
    "static": true,
    "outputDir": "public"
  },
  "env": {
    "NODE_VERSION": "18"
  }
}
EOF

    # 创建静态部署配置
    cat > "$BUILD_DIR/_headers" << 'EOF'
/*
  Cross-Origin-Embedder-Policy: require-corp
  Cross-Origin-Opener-Policy: same-origin
  Cache-Control: public, max-age=31536000
EOF

    echo "🎉 Zeabur部署包准备完成!"
    echo "📁 静态文件位置: $BUILD_DIR/"
    echo "🌐 准备推送到GitHub并在Zeabur部署"

else
    echo "❌ WebGL构建失败，查看日志: $LOG_FILE"
    if [ -f "$LOG_FILE" ]; then
        echo "📋 构建日志内容:"
        tail -20 "$LOG_FILE"
    fi
    exit 1
fi