using UnityEngine;

namespace StarWhisper
{
    /// <summary>
    /// StarWhisper测试演示控制器
    /// 用于快速验证核心功能
    /// </summary>
    public class StarWhisperDemo : MonoBehaviour
    {
        [Header("演示界面")]
        public GameObject welcomePanel;
        public UnityEngine.UI.Text statusText;
        public UnityEngine.UI.Text deviceInfoText;
        public UnityEngine.UI.Button startLearningButton;

        [Header("模拟组件")]
        public Camera arCamera;
        public GameObject demoARObject;

        private DeviceAdaptationManager deviceManager;
        private bool isDemoActive = false;

        private void Start()
        {
            InitializeDemo();
        }

        /// <summary>
        /// 初始化演示
        /// </summary>
        private void InitializeDemo()
        {
            Debug.Log("🚀 StarWhisper演示启动!");

            // 创建设备管理器
            var deviceManagerObject = new GameObject("DeviceManager");
            deviceManager = deviceManagerObject.AddComponent<DeviceAdaptationManager>();

            // 设置UI
            SetupDemoUI();

            // 显示设备信息
            ShowDeviceInfo();

            // 配置按钮
            if (startLearningButton != null)
            {
                startLearningButton.onClick.AddListener(StartLearningDemo);
            }

            UpdateStatus("StarWhisper演示已准备就绪!");
        }

        /// <summary>
        /// 设置演示UI
        /// </summary>
        private void SetupDemoUI()
        {
            // 创建基础UI
            if (welcomePanel == null)
            {
                CreateDemoUI();
            }

            // 显示欢迎界面
            if (welcomePanel != null)
            {
                welcomePanel.SetActive(true);
            }
        }

        /// <summary>
        /// 创建演示UI
        /// </summary>
        private void CreateDemoUI()
        {
            // 创建Canvas
            var canvasObject = new GameObject("DemoCanvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            // 创建状态文本
            var statusObject = new GameObject("StatusText");
            statusObject.transform.SetParent(canvasObject.transform);
            var statusRect = statusObject.AddComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0.1f, 0.8f);
            statusRect.anchorMax = new Vector2(0.9f, 0.9f);
            statusRect.offsetMin = Vector2.zero;
            statusRect.offsetMax = Vector2.zero;

            statusText = statusObject.AddComponent<UnityEngine.UI.Text>();
            statusText.text = "StarWhisper AR教育平台演示";
            statusText.fontSize = 24;
            statusText.alignment = TextAnchor.MiddleCenter;
            statusText.color = Color.white;

            // 设置字体（使用Unity默认字体）
            statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // 创建设备信息文本
            var deviceInfoObject = new GameObject("DeviceInfoText");
            deviceInfoObject.transform.SetParent(canvasObject.transform);
            var deviceInfoRect = deviceInfoObject.AddComponent<RectTransform>();
            deviceInfoRect.anchorMin = new Vector2(0.1f, 0.5f);
            deviceInfoRect.anchorMax = new Vector2(0.9f, 0.7f);
            deviceInfoRect.offsetMin = Vector2.zero;
            deviceInfoRect.offsetMax = Vector2.zero;

            deviceInfoText = deviceInfoObject.AddComponent<UnityEngine.UI.Text>();
            deviceInfoText.fontSize = 16;
            deviceInfoText.alignment = TextAnchor.UpperLeft;
            deviceInfoText.color = Color.cyan;
            deviceInfoText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // 创建开始按钮
            var buttonObject = new GameObject("StartButton");
            buttonObject.transform.SetParent(canvasObject.transform);
            var buttonRect = buttonObject.AddComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.3f, 0.2f);
            buttonRect.anchorMax = new Vector2(0.7f, 0.3f);
            buttonRect.offsetMin = Vector2.zero;
            buttonRect.offsetMax = Vector2.zero;

            var buttonImage = buttonObject.AddComponent<UnityEngine.UI.Image>();
            buttonImage.color = new Color(0.2f, 0.8f, 0.2f, 0.8f);

            startLearningButton = buttonObject.AddComponent<UnityEngine.UI.Button>();

            // 按钮文本
            var buttonTextObject = new GameObject("ButtonText");
            buttonTextObject.transform.SetParent(buttonObject.transform);
            var buttonTextRect = buttonTextObject.AddComponent<RectTransform>();
            buttonTextRect.anchorMin = Vector2.zero;
            buttonTextRect.anchorMax = Vector2.one;
            buttonTextRect.offsetMin = Vector2.zero;
            buttonTextRect.offsetMax = Vector2.zero;

            var buttonText = buttonTextObject.AddComponent<UnityEngine.UI.Text>();
            buttonText.text = "开始AR学习体验";
            buttonText.fontSize = 18;
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.color = Color.white;
            buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            welcomePanel = canvasObject;
        }

        /// <summary>
        /// 显示设备信息
        /// </summary>
        private void ShowDeviceInfo()
        {
            if (deviceInfoText == null) return;

            var deviceInfo = $@"
📱 设备信息检测：

屏幕分辨率：{Screen.width} x {Screen.height}
屏幕DPI：{Screen.dpi:F0}
屏幕尺寸：{GetScreenSizeInches():F1} 英寸

设备类型：{GetDeviceType()}
操作系统：{SystemInfo.operatingSystem}
处理器：{SystemInfo.processorType}
内存：{SystemInfo.systemMemorySize} MB
显卡：{SystemInfo.graphicsDeviceName}

AR支持：
- ARCore：{IsAndroid()}
- ARKit：{IsIOS()}
- 多点触控：{Input.multiTouchEnabled}

StarWhisper优化配置：
- 推荐分辨率：{GetRecommendedResolution()}
- 推荐帧率：{GetRecommendedFrameRate()} fps
- UI缩放：{GetRecommendedUIScale():F1}x
            ";

            deviceInfoText.text = deviceInfo;
        }

        /// <summary>
        /// 开始学习演示
        /// </summary>
        private void StartLearningDemo()
        {
            if (isDemoActive) return;

            isDemoActive = true;
            UpdateStatus("🎯 启动AR学习场景...");

            StartCoroutine(DemoLearningSequence());
        }

        /// <summary>
        /// 演示学习序列
        /// </summary>
        private System.Collections.IEnumerator DemoLearningSequence()
        {
            // 1. 检测设备
            UpdateStatus("📱 检测设备类型和性能...");
            yield return new WaitForSeconds(1f);

            // 2. 初始化AR
            UpdateStatus("📷 启动AR摄像头...");
            yield return new WaitForSeconds(1f);

            // 3. 连接AI服务
            UpdateStatus("🤖 连接Decart AI服务...");
            yield return new WaitForSeconds(2f);

            // 4. 生成教育Prompt
            UpdateStatus("📝 生成《星语低语》学习场景...");
            var prompt = GenerateTestPrompt();
            Debug.Log($"生成的教育Prompt:\n{prompt}");
            yield return new WaitForSeconds(1f);

            // 5. 创建AR场景
            UpdateStatus("✨ 创建AR几何学习场景...");
            CreateDemoARScene();
            yield return new WaitForSeconds(1f);

            // 6. 完成演示
            UpdateStatus("🎉 AR学习场景已准备就绪!\n触摸屏幕与3D对象互动");
        }

        /// <summary>
        /// 生成测试Prompt
        /// </summary>
        private string GenerateTestPrompt()
        {
            return $@"
创建一个基于《星语低语》世界观的数学学习场景：

## 教育背景
- 学习目标：理解3D几何图形的基本属性
- 核心概念：立方体、球体、圆柱体
- 学生年龄：8岁
- 设备类型：{GetDeviceType()}

## 《星语低语》叙事框架
学生扮演：年轻的太空探索者
AI伙伴：智慧的AI学习伙伴小星
学习环境：神秘的外星几何研究站
探索主题：解锁宇宙几何密码

## 互动要求
- 创建3个可触摸的3D几何体
- 触摸时显示形状属性
- 支持旋转和缩放操作
- 提供即时学习反馈
- 适配{GetDeviceType()}设备显示

让学习几何成为一次有趣的星际探索之旅！
            ";
        }

        /// <summary>
        /// 创建演示AR场景
        /// </summary>
        private void CreateDemoARScene()
        {
            // 创建一个简单的3D对象作为演示
            if (demoARObject == null)
            {
                // 创建立方体
                demoARObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                demoARObject.name = "StarWhisper学习立方体";

                // 位置设置
                demoARObject.transform.position = new Vector3(0, 0, 5);
                demoARObject.transform.localScale = Vector3.one * 2f;

                // 添加材质
                var renderer = demoARObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    var material = new Material(Shader.Find("Standard"));
                    material.color = new Color(0.3f, 0.8f, 1f, 0.8f); // 星语蓝色
                    material.SetFloat("_Mode", 3); // 透明模式
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    renderer.material = material;
                }

                // 添加旋转动画
                var rotator = demoARObject.AddComponent<DemoObjectRotator>();

                // 添加触摸交互
                var touchHandler = demoARObject.AddComponent<DemoTouchHandler>();
                touchHandler.onTouch = () => {
                    UpdateStatus("🎯 太好了！你发现了立方体的秘密！\n立方体有6个面、8个顶点、12条边");
                };
            }

            demoARObject.SetActive(true);
        }

        /// <summary>
        /// 更新状态文本
        /// </summary>
        private void UpdateStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log($"StarWhisper: {message}");
        }

        // 辅助方法
        private float GetScreenSizeInches()
        {
            float dpi = Screen.dpi > 0 ? Screen.dpi : 160f;
            float widthInches = Screen.width / dpi;
            float heightInches = Screen.height / dpi;
            return Mathf.Sqrt(widthInches * widthInches + heightInches * heightInches);
        }

        private string GetDeviceType()
        {
            return GetScreenSizeInches() >= 7.0f ? "平板" : "手机";
        }

        private string GetRecommendedResolution()
        {
            return GetDeviceType() == "平板" ? "1920x1440" : "1280x720";
        }

        private int GetRecommendedFrameRate()
        {
            return GetDeviceType() == "平板" ? 60 : 30;
        }

        private float GetRecommendedUIScale()
        {
            return GetDeviceType() == "平板" ? 1.2f : 1.0f;
        }

        private bool IsAndroid()
        {
            return Application.platform == RuntimePlatform.Android;
        }

        private bool IsIOS()
        {
            return Application.platform == RuntimePlatform.IPhonePlayer;
        }
    }

    /// <summary>
    /// 演示对象旋转器
    /// </summary>
    public class DemoObjectRotator : MonoBehaviour
    {
        public float rotationSpeed = 30f;

        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            transform.Rotate(Vector3.right, rotationSpeed * 0.5f * Time.deltaTime);
        }
    }

    /// <summary>
    /// 演示触摸处理器
    /// </summary>
    public class DemoTouchHandler : MonoBehaviour
    {
        public System.Action onTouch;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        OnTouched();
                    }
                }
            }
        }

        private void OnTouched()
        {
            // 视觉反馈
            StartCoroutine(TouchFeedback());

            // 触发回调
            onTouch?.Invoke();
        }

        private System.Collections.IEnumerator TouchFeedback()
        {
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                var originalColor = renderer.material.color;
                renderer.material.color = Color.yellow;
                yield return new WaitForSeconds(0.3f);
                renderer.material.color = originalColor;
            }
        }
    }
}