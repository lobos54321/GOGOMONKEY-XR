using UnityEngine;
using System.Threading.Tasks;

namespace StarWhisper
{
    /// <summary>
    /// 设备类型枚举
    /// </summary>
    public enum DeviceType
    {
        Phone,      // 手机 (屏幕 < 7寸)
        Tablet,     // 平板 (屏幕 >= 7寸)
        Unknown     // 未知设备
    }

    /// <summary>
    /// 设备规格信息
    /// </summary>
    [System.Serializable]
    public class DeviceSpecs
    {
        public DeviceType deviceType;
        public Vector2Int screenResolution;
        public float screenDPI;
        public float screenSizeInches;
        public int memoryMB;
        public bool supportMultiTouch;
        public int maxTouchPoints;
        public string gpuName;
        public bool supportsARCore;
        public bool supportsARKit;
    }

    /// <summary>
    /// 设备适配管理器
    /// 自动检测设备类型并优化配置
    /// </summary>
    public class DeviceAdaptationManager : MonoBehaviour
    {
        [Header("设备配置")]
        [SerializeField] private DeviceSpecs currentDeviceSpecs;

        [Header("性能配置")]
        [SerializeField] private PerformanceProfile phoneProfile;
        [SerializeField] private PerformanceProfile tabletProfile;

        public DeviceType CurrentDeviceType { get; private set; }
        public DeviceSpecs CurrentSpecs => currentDeviceSpecs;

        private void Awake()
        {
            // 初始化设备检测
            DetectDeviceType();
        }

        /// <summary>
        /// 检测并配置设备
        /// </summary>
        public async Task DetectAndConfigureDevice()
        {
            Debug.Log("🔍 开始设备检测...");

            // 1. 检测设备类型
            DetectDeviceType();

            // 2. 获取设备规格
            await GetDeviceSpecs();

            // 3. 配置性能参数
            ConfigurePerformanceProfile();

            // 4. 优化Unity设置
            OptimizeUnitySettings();

            Debug.Log($"📱 设备检测完成: {CurrentDeviceType} ({currentDeviceSpecs.screenSizeInches:F1}寸)");
        }

        /// <summary>
        /// 检测设备类型
        /// </summary>
        private void DetectDeviceType()
        {
            // 获取屏幕尺寸（英寸）
            float screenInches = GetScreenSizeInches();

            // 根据屏幕尺寸判断设备类型
            if (screenInches >= 7.0f)
            {
                CurrentDeviceType = DeviceType.Tablet;
                Debug.Log($"📱 检测到平板设备: {screenInches:F1}寸");
            }
            else if (screenInches >= 4.0f && screenInches < 7.0f)
            {
                CurrentDeviceType = DeviceType.Phone;
                Debug.Log($"📱 检测到手机设备: {screenInches:F1}寸");
            }
            else
            {
                CurrentDeviceType = DeviceType.Unknown;
                Debug.LogWarning($"⚠️ 未知设备类型: {screenInches:F1}寸");
            }
        }

        /// <summary>
        /// 计算屏幕尺寸（英寸）
        /// </summary>
        private float GetScreenSizeInches()
        {
            float dpi = Screen.dpi;
            if (dpi <= 0) dpi = 160f; // 默认DPI

            float widthInches = Screen.width / dpi;
            float heightInches = Screen.height / dpi;

            // 计算对角线长度
            return Mathf.Sqrt(widthInches * widthInches + heightInches * heightInches);
        }

        /// <summary>
        /// 获取详细设备规格
        /// </summary>
        private async Task GetDeviceSpecs()
        {
            currentDeviceSpecs = new DeviceSpecs
            {
                deviceType = CurrentDeviceType,
                screenResolution = new Vector2Int(Screen.width, Screen.height),
                screenDPI = Screen.dpi > 0 ? Screen.dpi : 160f,
                screenSizeInches = GetScreenSizeInches(),
                memoryMB = SystemInfo.systemMemorySize,
                supportMultiTouch = Input.multiTouchEnabled,
                maxTouchPoints = Input.touchSupported ? 10 : 0,
                gpuName = SystemInfo.graphicsDeviceName,
                supportsARCore = IsARCoreSupported(),
                supportsARKit = IsARKitSupported()
            };

            // 异步获取更多设备信息
            await Task.Delay(100); // 模拟异步检测

            Debug.Log($"📊 设备规格:\n" +
                     $"   分辨率: {currentDeviceSpecs.screenResolution.x}x{currentDeviceSpecs.screenResolution.y}\n" +
                     $"   DPI: {currentDeviceSpecs.screenDPI:F0}\n" +
                     $"   内存: {currentDeviceSpecs.memoryMB}MB\n" +
                     $"   GPU: {currentDeviceSpecs.gpuName}\n" +
                     $"   AR支持: ARCore={currentDeviceSpecs.supportsARCore}, ARKit={currentDeviceSpecs.supportsARKit}");
        }

        /// <summary>
        /// 配置性能参数
        /// </summary>
        private void ConfigurePerformanceProfile()
        {
            switch (CurrentDeviceType)
            {
                case DeviceType.Phone:
                    ApplyPerformanceProfile(phoneProfile);
                    Debug.Log("📱 应用手机性能配置");
                    break;

                case DeviceType.Tablet:
                    ApplyPerformanceProfile(tabletProfile);
                    Debug.Log("🖥️ 应用平板性能配置");
                    break;

                default:
                    ApplyPerformanceProfile(phoneProfile); // 默认使用手机配置
                    Debug.Log("❓ 应用默认性能配置");
                    break;
            }
        }

        /// <summary>
        /// 应用性能配置
        /// </summary>
        private void ApplyPerformanceProfile(PerformanceProfile profile)
        {
            if (profile == null) return;

            // 设置目标帧率
            Application.targetFrameRate = profile.targetFrameRate;

            // 设置渲染质量
            QualitySettings.SetQualityLevel(profile.qualityLevel);

            // 配置阴影设置
            QualitySettings.shadows = profile.shadowQuality;
            QualitySettings.shadowDistance = profile.shadowDistance;

            // 配置纹理质量
            QualitySettings.masterTextureLimit = profile.textureQuality;

            // 配置抗锯齿
            QualitySettings.antiAliasing = profile.antiAliasing;

            Debug.Log($"⚙️ 性能配置已应用: 目标{profile.targetFrameRate}fps, 质量等级{profile.qualityLevel}");
        }

        /// <summary>
        /// 优化Unity设置
        /// </summary>
        private void OptimizeUnitySettings()
        {
            // 根据设备类型优化设置
            switch (CurrentDeviceType)
            {
                case DeviceType.Phone:
                    OptimizeForPhone();
                    break;
                case DeviceType.Tablet:
                    OptimizeForTablet();
                    break;
            }
        }

        /// <summary>
        /// 手机优化设置
        /// </summary>
        private void OptimizeForPhone()
        {
            // 降低分辨率以节省性能
            if (currentDeviceSpecs.screenResolution.x > 1920)
            {
                Screen.SetResolution(1920, 1080, true);
                Debug.Log("📱 手机分辨率已优化到1920x1080");
            }

            // 启用电池优化
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

            // 优化物理设置
            Physics.bounceThreshold = 4f;
            Physics.sleepThreshold = 0.01f;
        }

        /// <summary>
        /// 平板优化设置
        /// </summary>
        private void OptimizeForTablet()
        {
            // 保持高分辨率以充分利用大屏
            Debug.Log("🖥️ 平板保持原生高分辨率");

            // 启用更高质量设置
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;

            // 优化触控设置
            Input.multiTouchEnabled = true;
        }

        /// <summary>
        /// 检测ARCore支持
        /// </summary>
        private bool IsARCoreSupported()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return true; // 简化检测，实际应用中需要更详细的检测
#else
            return false;
#endif
        }

        /// <summary>
        /// 检测ARKit支持
        /// </summary>
        private bool IsARKitSupported()
        {
#if UNITY_IOS && !UNITY_EDITOR
            return true; // 简化检测，实际应用中需要更详细的检测
#else
            return false;
#endif
        }

        /// <summary>
        /// 获取当前设备规格
        /// </summary>
        public DeviceSpecs GetCurrentDeviceSpecs()
        {
            return currentDeviceSpecs;
        }

        /// <summary>
        /// 检查是否为高性能设备
        /// </summary>
        public bool IsHighPerformanceDevice()
        {
            return CurrentDeviceType == DeviceType.Tablet ||
                   currentDeviceSpecs.memoryMB >= 6000;
        }

        /// <summary>
        /// 获取推荐的AR分辨率
        /// </summary>
        public Vector2Int GetRecommendedARResolution()
        {
            switch (CurrentDeviceType)
            {
                case DeviceType.Phone:
                    return new Vector2Int(1280, 720);   // 720p for phones
                case DeviceType.Tablet:
                    return new Vector2Int(1920, 1440);  // Higher resolution for tablets
                default:
                    return new Vector2Int(1280, 720);
            }
        }

        /// <summary>
        /// 获取推荐的UI缩放比例
        /// </summary>
        public float GetRecommendedUIScale()
        {
            switch (CurrentDeviceType)
            {
                case DeviceType.Phone:
                    return 1.0f;    // 标准缩放
                case DeviceType.Tablet:
                    return 1.2f;    // 平板稍微放大UI
                default:
                    return 1.0f;
            }
        }
    }

    /// <summary>
    /// 性能配置文件
    /// </summary>
    [System.Serializable]
    public class PerformanceProfile
    {
        [Header("基础设置")]
        public int targetFrameRate = 30;
        public int qualityLevel = 2;

        [Header("渲染设置")]
        public ShadowQuality shadowQuality = ShadowQuality.HardOnly;
        public float shadowDistance = 50f;
        public int textureQuality = 0;
        public int antiAliasing = 0;

        [Header("AR设置")]
        public bool enableOcclusion = true;
        public bool enableLighting = true;
        public int maxTrackingObjects = 20;
    }
}