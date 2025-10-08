using UnityEngine;
using System.Threading.Tasks;
using PassthroughCameraSamples;
using SimpleWebRTC;

namespace StarWhisper
{
    /// <summary>
    /// Decart-XR移动端控制器
    /// 整合Decart-XR功能并优化移动端体验
    /// </summary>
    public class DecartXRMobileController : MonoBehaviour
    {
        [Header("Decart-XR核心组件")]
        [SerializeField] private WebCamTextureManager cameraManager;
        [SerializeField] private WebRTCConnection webRTCConnection;
        [SerializeField] private WebRTCController webRTCController;

        [Header("移动端优化")]
        [SerializeField] private MobileARConfig mobileConfig;
        [SerializeField] private bool enableBatteryOptimization = true;
        [SerializeField] private bool enablePerformanceMonitoring = true;

        [Header("教育AR设置")]
        [SerializeField] private EducationalARSettings arSettings;

        public bool IsInitialized { get; private set; }
        public bool IsARActive { get; private set; }
        public bool IsConnectedToDecartAI { get; private set; }

        private DeviceAdaptationManager deviceManager;
        private PerformanceMonitor performanceMonitor;

        private void Awake()
        {
            deviceManager = FindObjectOfType<DeviceAdaptationManager>();
            if (enablePerformanceMonitoring)
            {
                performanceMonitor = gameObject.AddComponent<PerformanceMonitor>();
            }
        }

        /// <summary>
        /// 初始化移动端AR系统
        /// </summary>
        public async Task InitializeMobileAR()
        {
            Debug.Log("🔧 初始化Decart-XR移动端系统...");

            try
            {
                // 1. 配置移动端优化设置
                await ConfigureMobileOptimizations();

                // 2. 初始化摄像头管理器
                await InitializeCameraManager();

                // 3. 配置WebRTC连接
                await ConfigureWebRTCConnection();

                // 4. 连接Decart AI服务
                await ConnectToDecartAI();

                // 5. 启动性能监控
                if (enablePerformanceMonitoring)
                {
                    performanceMonitor.StartMonitoring();
                }

                IsInitialized = true;
                Debug.Log("✅ Decart-XR移动端系统初始化完成!");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Decart-XR移动端初始化失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 配置移动端优化设置
        /// </summary>
        private async Task ConfigureMobileOptimizations()
        {
            if (deviceManager == null) return;

            var deviceSpecs = deviceManager.GetCurrentDeviceSpecs();

            // 根据设备性能配置Decart-XR参数
            if (mobileConfig == null)
            {
                mobileConfig = CreateMobileConfig(deviceSpecs);
            }

            // 配置分辨率
            var recommendedResolution = deviceManager.GetRecommendedARResolution();
            mobileConfig.targetResolution = recommendedResolution;

            // 配置帧率
            mobileConfig.targetFrameRate = deviceManager.IsHighPerformanceDevice() ? 60 : 30;

            // 电池优化
            if (enableBatteryOptimization)
            {
                ConfigureBatteryOptimization();
            }

            Debug.Log($"移动端优化配置: {recommendedResolution.x}x{recommendedResolution.y}@{mobileConfig.targetFrameRate}fps");

            await Task.Delay(100); // 等待配置应用
        }

        /// <summary>
        /// 创建移动端配置
        /// </summary>
        private MobileARConfig CreateMobileConfig(DeviceSpecs deviceSpecs)
        {
            return new MobileARConfig
            {
                deviceType = deviceSpecs.deviceType,
                targetResolution = new Vector2Int(1280, 720),
                targetFrameRate = 30,
                enableLowLatencyMode = true,
                enableBatteryOptimization = enableBatteryOptimization,
                maxConcurrentConnections = 1,
                compressionQuality = deviceSpecs.deviceType == DeviceType.Tablet ? 0.8f : 0.6f
            };
        }

        /// <summary>
        /// 配置电池优化
        /// </summary>
        private void ConfigureBatteryOptimization()
        {
            // 降低后台处理
            Application.runInBackground = false;

            // 配置屏幕超时
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

            // 优化物理更新频率
            Time.fixedDeltaTime = 1.0f / 30.0f; // 30fps physics

            Debug.Log("⚡ 电池优化配置已启用");
        }

        /// <summary>
        /// 初始化摄像头管理器
        /// </summary>
        private async Task InitializeCameraManager()
        {
            if (cameraManager == null)
            {
                // 查找或创建摄像头管理器
                cameraManager = FindObjectOfType<WebCamTextureManager>();
                if (cameraManager == null)
                {
                    var cameraObject = new GameObject("MobileCameraManager");
                    cameraManager = cameraObject.AddComponent<WebCamTextureManager>();
                }
            }

            // 配置摄像头参数
            if (mobileConfig != null)
            {
                cameraManager.RequestedResolution = mobileConfig.targetResolution;
            }

            // 等待摄像头初始化
            var timeout = Time.time + 10f; // 10秒超时
            while (cameraManager.WebCamTexture == null && Time.time < timeout)
            {
                await Task.Delay(100);
            }

            if (cameraManager.WebCamTexture != null && cameraManager.WebCamTexture.isPlaying)
            {
                Debug.Log($"📷 摄像头初始化成功: {cameraManager.WebCamTexture.width}x{cameraManager.WebCamTexture.height}");
                IsARActive = true;
            }
            else
            {
                throw new System.Exception("摄像头初始化失败");
            }
        }

        /// <summary>
        /// 配置WebRTC连接
        /// </summary>
        private async Task ConfigureWebRTCConnection()
        {
            if (webRTCConnection == null)
            {
                webRTCConnection = FindObjectOfType<WebRTCConnection>();
                if (webRTCConnection == null)
                {
                    throw new System.Exception("未找到WebRTCConnection组件");
                }
            }

            // 配置移动端WebRTC参数
            ConfigureMobileWebRTC();

            // 等待WebRTC准备就绪
            await Task.Delay(500);

            Debug.Log("🌐 WebRTC连接配置完成");
        }

        /// <summary>
        /// 配置移动端WebRTC参数
        /// </summary>
        private void ConfigureMobileWebRTC()
        {
            if (webRTCConnection == null) return;

            // 根据设备类型配置视频参数
            if (mobileConfig != null)
            {
                // 设置视频分辨率
                var videoResolutionField = webRTCConnection.GetType().GetField("VideoResolution");
                if (videoResolutionField != null)
                {
                    videoResolutionField.SetValue(webRTCConnection, mobileConfig.targetResolution);
                }

                // 启用移动端优化
                Debug.Log($"WebRTC移动端参数: {mobileConfig.targetResolution.x}x{mobileConfig.targetResolution.y}");
            }
        }

        /// <summary>
        /// 连接Decart AI服务
        /// </summary>
        private async Task ConnectToDecartAI()
        {
            if (webRTCConnection == null) return;

            Debug.Log("🤖 连接Decart AI服务...");

            try
            {
                // 启动WebRTC连接
                // 注意：实际的连接逻辑可能需要根据Decart-XR的具体实现进行调整
                var connectMethod = webRTCConnection.GetType().GetMethod("StartConnection");
                if (connectMethod != null)
                {
                    connectMethod.Invoke(webRTCConnection, null);
                }

                // 等待连接建立
                var timeout = Time.time + 30f; // 30秒超时
                while (!IsWebRTCConnected() && Time.time < timeout)
                {
                    await Task.Delay(1000);
                    Debug.Log("等待Decart AI连接...");
                }

                if (IsWebRTCConnected())
                {
                    IsConnectedToDecartAI = true;
                    Debug.Log("✅ 已连接到Decart AI服务");
                }
                else
                {
                    throw new System.Exception("连接Decart AI服务超时");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ 连接Decart AI失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 检查WebRTC连接状态
        /// </summary>
        private bool IsWebRTCConnected()
        {
            if (webRTCConnection == null) return false;

            // 检查连接状态
            var isConnectedProperty = webRTCConnection.GetType().GetProperty("IsWebSocketConnected");
            if (isConnectedProperty != null)
            {
                return (bool)isConnectedProperty.GetValue(webRTCConnection);
            }

            return false;
        }

        /// <summary>
        /// 创建教育AR场景
        /// </summary>
        public async Task<ARLearningExperience> CreateEducationalARScene(
            string educationalPrompt,
            DeviceSpecs deviceSpecs = null)
        {
            if (!IsInitialized || !IsConnectedToDecartAI)
            {
                Debug.LogError("Decart-XR系统未准备就绪");
                return null;
            }

            Debug.Log($"🎨 创建教育AR场景: {educationalPrompt.Substring(0, Mathf.Min(50, educationalPrompt.Length))}...");

            try
            {
                // 1. 发送教育Prompt到Decart AI
                var arScene = await SendEducationalPromptToDecartAI(educationalPrompt);

                // 2. 配置AR场景参数
                if (arScene != null)
                {
                    ConfigureARSceneForMobile(arScene, deviceSpecs);
                }

                // 3. 启动性能监控
                if (performanceMonitor != null)
                {
                    performanceMonitor.StartSceneMonitoring(arScene);
                }

                Debug.Log("✨ 教育AR场景创建成功!");
                return arScene;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ 创建教育AR场景失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 发送教育Prompt到Decart AI
        /// </summary>
        private async Task<ARLearningExperience> SendEducationalPromptToDecartAI(string prompt)
        {
            // 这里需要根据Decart-XR的实际API进行实现
            // 目前提供一个模拟实现

            // 发送Prompt
            await SendPromptToDecart(prompt);

            // 等待AI处理
            await Task.Delay(2000); // 模拟AI处理时间

            // 创建AR学习体验对象
            var experience = new ARLearningExperience
            {
                sceneId = System.Guid.NewGuid().ToString(),
                prompt = prompt,
                createdTime = System.DateTime.Now,
                isActive = true
            };

            return experience;
        }

        /// <summary>
        /// 发送Prompt到Decart
        /// </summary>
        private async Task SendPromptToDecart(string prompt)
        {
            if (webRTCController == null) return;

            // 使用反射调用Decart-XR的Prompt发送方法
            var sendPromptMethod = webRTCController.GetType().GetMethod("QueueCustomPrompt");
            if (sendPromptMethod != null)
            {
                sendPromptMethod.Invoke(webRTCController, new object[] { prompt });
                Debug.Log($"📤 已发送Prompt到Decart AI: {prompt.Length}字符");
            }

            await Task.Delay(100);
        }

        /// <summary>
        /// 为移动端配置AR场景
        /// </summary>
        private void ConfigureARSceneForMobile(ARLearningExperience scene, DeviceSpecs deviceSpecs)
        {
            if (scene == null) return;

            // 根据设备类型优化场景
            if (deviceSpecs != null)
            {
                switch (deviceSpecs.deviceType)
                {
                    case DeviceType.Phone:
                        scene.optimizationLevel = OptimizationLevel.Mobile;
                        scene.maxObjects = 5;
                        break;
                    case DeviceType.Tablet:
                        scene.optimizationLevel = OptimizationLevel.Tablet;
                        scene.maxObjects = 10;
                        break;
                }
            }

            Debug.Log($"AR场景已针对{deviceSpecs?.deviceType}优化");
        }

        /// <summary>
        /// 开始过程演示
        /// </summary>
        public async Task StartProcessDemonstration(string process)
        {
            Debug.Log($"🎭 开始演示过程: {process}");

            var demonstrationPrompt = $"演示{process}的完整过程，包含分步骤的可视化说明";
            await CreateEducationalARScene(demonstrationPrompt);
        }

        /// <summary>
        /// 获取当前性能状态
        /// </summary>
        public PerformanceStatus GetCurrentPerformance()
        {
            if (performanceMonitor != null)
            {
                return performanceMonitor.GetCurrentStatus();
            }

            return new PerformanceStatus
            {
                fps = Mathf.RoundToInt(1.0f / Time.deltaTime),
                memoryUsage = System.GC.GetTotalMemory(false) / 1024 / 1024, // MB
                isOptimal = true
            };
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Cleanup()
        {
            if (performanceMonitor != null)
            {
                performanceMonitor.StopMonitoring();
            }

            IsInitialized = false;
            IsARActive = false;
            IsConnectedToDecartAI = false;

            Debug.Log("🧹 Decart-XR移动端控制器已清理");
        }

        private void OnDestroy()
        {
            Cleanup();
        }
    }

    /// <summary>
    /// 移动端AR配置
    /// </summary>
    [System.Serializable]
    public class MobileARConfig
    {
        public DeviceType deviceType;
        public Vector2Int targetResolution;
        public int targetFrameRate;
        public bool enableLowLatencyMode;
        public bool enableBatteryOptimization;
        public int maxConcurrentConnections;
        public float compressionQuality;
    }

    /// <summary>
    /// 教育AR设置
    /// </summary>
    [System.Serializable]
    public class EducationalARSettings
    {
        public bool enableInteractiveObjects = true;
        public bool enableVoiceInteraction = true;
        public bool enableGestureRecognition = true;
        public int maxLearningObjects = 10;
        public float interactionRange = 2.0f;
    }

    /// <summary>
    /// AR学习体验
    /// </summary>
    public class ARLearningExperience
    {
        public string sceneId;
        public string prompt;
        public System.DateTime createdTime;
        public bool isActive;
        public OptimizationLevel optimizationLevel;
        public int maxObjects;
    }

    /// <summary>
    /// 优化等级
    /// </summary>
    public enum OptimizationLevel
    {
        Mobile,    // 手机优化
        Tablet,    // 平板优化
        Desktop    // 桌面优化
    }

    /// <summary>
    /// 性能状态
    /// </summary>
    public class PerformanceStatus
    {
        public int fps;
        public long memoryUsage;
        public bool isOptimal;
        public float temperature;
        public float batteryLevel;
    }
}