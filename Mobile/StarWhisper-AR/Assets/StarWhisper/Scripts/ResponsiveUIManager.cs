using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StarWhisper
{
    /// <summary>
    /// 响应式UI管理器
    /// 根据设备类型自动调整UI布局和尺寸
    /// </summary>
    public class ResponsiveUIManager : MonoBehaviour
    {
        [Header("UI配置")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private GraphicRaycaster graphicRaycaster;

        [Header("UI面板")]
        [SerializeField] private GameObject phoneUIPanel;
        [SerializeField] private GameObject tabletUIPanel;
        [SerializeField] private GameObject commonUIPanel;

        [Header("响应式组件")]
        [SerializeField] private RectTransform arViewport;
        [SerializeField] private RectTransform controlPanel;
        [SerializeField] private RectTransform knowledgePanel;
        [SerializeField] private RectTransform progressPanel;

        [Header("文本组件")]
        [SerializeField] private List<TextMeshProUGUI> responsiveTexts = new List<TextMeshProUGUI>();

        [Header("按钮组件")]
        [SerializeField] private List<Button> responsiveButtons = new List<Button>();

        [Header("对话框")]
        [SerializeField] private GameObject welcomeDialog;
        [SerializeField] private GameObject explanationDialog;
        [SerializeField] private GameObject errorDialog;
        [SerializeField] private GameObject quizDialog;

        private DeviceAdaptationManager deviceManager;
        private bool isUIConfigured = false;

        private void Awake()
        {
            deviceManager = FindObjectOfType<DeviceAdaptationManager>();
            if (deviceManager == null)
            {
                Debug.LogError("❌ 未找到DeviceAdaptationManager!");
            }
        }

        /// <summary>
        /// 为当前设备配置UI
        /// </summary>
        public async Task ConfigureForCurrentDevice()
        {
            if (deviceManager == null) return;

            Debug.Log("🎨 开始配置响应式UI...");

            // 1. 配置Canvas缩放器
            ConfigureCanvasScaler();

            // 2. 根据设备类型选择UI布局
            await ConfigureDeviceSpecificUI();

            // 3. 调整UI组件尺寸
            AdjustUIComponents();

            // 4. 配置交互设置
            ConfigureInteractionSettings();

            isUIConfigured = true;
            Debug.Log("✅ 响应式UI配置完成!");
        }

        /// <summary>
        /// 配置Canvas缩放器
        /// </summary>
        private void ConfigureCanvasScaler()
        {
            if (canvasScaler == null) return;

            var deviceSpecs = deviceManager.GetCurrentDeviceSpecs();

            // 设置缩放器模式
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            switch (deviceManager.CurrentDeviceType)
            {
                case DeviceType.Phone:
                    // 手机配置
                    canvasScaler.referenceResolution = new Vector2(1080, 1920);
                    canvasScaler.matchWidthOrHeight = 0.5f;
                    Debug.Log("📱 配置手机Canvas缩放器");
                    break;

                case DeviceType.Tablet:
                    // 平板配置
                    canvasScaler.referenceResolution = new Vector2(1536, 2048);
                    canvasScaler.matchWidthOrHeight = 0.5f;
                    Debug.Log("🖥️ 配置平板Canvas缩放器");
                    break;

                default:
                    // 默认配置
                    canvasScaler.referenceResolution = new Vector2(1080, 1920);
                    canvasScaler.matchWidthOrHeight = 0.5f;
                    break;
            }

            Debug.Log($"Canvas缩放器已配置: {canvasScaler.referenceResolution}");
        }

        /// <summary>
        /// 配置设备特定的UI
        /// </summary>
        private async Task ConfigureDeviceSpecificUI()
        {
            switch (deviceManager.CurrentDeviceType)
            {
                case DeviceType.Phone:
                    await ConfigurePhoneUI();
                    break;
                case DeviceType.Tablet:
                    await ConfigureTabletUI();
                    break;
                default:
                    await ConfigurePhoneUI(); // 默认使用手机UI
                    break;
            }
        }

        /// <summary>
        /// 配置手机UI布局
        /// </summary>
        private async Task ConfigurePhoneUI()
        {
            Debug.Log("📱 配置手机UI布局...");

            // 激活手机UI面板
            if (phoneUIPanel != null) phoneUIPanel.SetActive(true);
            if (tabletUIPanel != null) tabletUIPanel.SetActive(false);

            // 配置AR视窗 - 手机全屏AR
            if (arViewport != null)
            {
                arViewport.anchorMin = new Vector2(0f, 0.15f);
                arViewport.anchorMax = new Vector2(1f, 1f);
                arViewport.offsetMin = Vector2.zero;
                arViewport.offsetMax = Vector2.zero;
            }

            // 配置控制面板 - 底部占15%
            if (controlPanel != null)
            {
                controlPanel.anchorMin = new Vector2(0f, 0f);
                controlPanel.anchorMax = new Vector2(1f, 0.15f);
                controlPanel.offsetMin = Vector2.zero;
                controlPanel.offsetMax = Vector2.zero;
            }

            // 隐藏知识面板（手机屏幕太小）
            if (knowledgePanel != null)
            {
                knowledgePanel.gameObject.SetActive(false);
            }

            await Task.Delay(50); // 等待布局更新
        }

        /// <summary>
        /// 配置平板UI布局
        /// </summary>
        private async Task ConfigureTabletUI()
        {
            Debug.Log("🖥️ 配置平板UI布局...");

            // 激活平板UI面板
            if (tabletUIPanel != null) tabletUIPanel.SetActive(true);
            if (phoneUIPanel != null) phoneUIPanel.SetActive(false);

            // 配置AR视窗 - 平板左侧70%
            if (arViewport != null)
            {
                arViewport.anchorMin = new Vector2(0f, 0.15f);
                arViewport.anchorMax = new Vector2(0.7f, 1f);
                arViewport.offsetMin = Vector2.zero;
                arViewport.offsetMax = Vector2.zero;
            }

            // 配置知识面板 - 右侧30%
            if (knowledgePanel != null)
            {
                knowledgePanel.gameObject.SetActive(true);
                knowledgePanel.anchorMin = new Vector2(0.7f, 0.15f);
                knowledgePanel.anchorMax = new Vector2(1f, 1f);
                knowledgePanel.offsetMin = Vector2.zero;
                knowledgePanel.offsetMax = Vector2.zero;
            }

            // 配置控制面板 - 底部15%
            if (controlPanel != null)
            {
                controlPanel.anchorMin = new Vector2(0f, 0f);
                controlPanel.anchorMax = new Vector2(1f, 0.15f);
                controlPanel.offsetMin = Vector2.zero;
                controlPanel.offsetMax = Vector2.zero;
            }

            await Task.Delay(50); // 等待布局更新
        }

        /// <summary>
        /// 调整UI组件尺寸
        /// </summary>
        private void AdjustUIComponents()
        {
            float uiScale = deviceManager.GetRecommendedUIScale();

            // 调整文本大小
            AdjustTextComponents(uiScale);

            // 调整按钮大小
            AdjustButtonComponents(uiScale);

            // 调整进度面板
            AdjustProgressPanel();

            Debug.Log($"UI组件已调整: 缩放比例{uiScale:F1}x");
        }

        /// <summary>
        /// 调整文本组件
        /// </summary>
        private void AdjustTextComponents(float scale)
        {
            foreach (var text in responsiveTexts)
            {
                if (text != null)
                {
                    float baseFontSize = text.fontSize;
                    text.fontSize = baseFontSize * scale;

                    // 根据设备类型调整字体大小
                    if (deviceManager.CurrentDeviceType == DeviceType.Tablet)
                    {
                        text.fontSize *= 1.1f; // 平板稍微增大字体
                    }
                }
            }
        }

        /// <summary>
        /// 调整按钮组件
        /// </summary>
        private void AdjustButtonComponents(float scale)
        {
            foreach (var button in responsiveButtons)
            {
                if (button != null)
                {
                    var rectTransform = button.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        // 按钮尺寸根据设备类型调整
                        if (deviceManager.CurrentDeviceType == DeviceType.Tablet)
                        {
                            // 平板按钮更大，便于触控
                            rectTransform.sizeDelta *= 1.2f;
                        }
                    }

                    // 调整按钮文本
                    var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null)
                    {
                        buttonText.fontSize *= scale;
                    }
                }
            }
        }

        /// <summary>
        /// 调整进度面板
        /// </summary>
        private void AdjustProgressPanel()
        {
            if (progressPanel == null) return;

            switch (deviceManager.CurrentDeviceType)
            {
                case DeviceType.Phone:
                    // 手机进度面板放在顶部
                    progressPanel.anchorMin = new Vector2(0f, 0.9f);
                    progressPanel.anchorMax = new Vector2(1f, 1f);
                    break;

                case DeviceType.Tablet:
                    // 平板进度面板放在知识面板顶部
                    progressPanel.anchorMin = new Vector2(0.7f, 0.9f);
                    progressPanel.anchorMax = new Vector2(1f, 1f);
                    break;
            }
        }

        /// <summary>
        /// 配置交互设置
        /// </summary>
        private void ConfigureInteractionSettings()
        {
            if (graphicRaycaster == null) return;

            // 根据设备类型配置触控设置
            switch (deviceManager.CurrentDeviceType)
            {
                case DeviceType.Phone:
                    // 手机优化单点触控
                    Input.multiTouchEnabled = false;
                    break;

                case DeviceType.Tablet:
                    // 平板启用多点触控
                    Input.multiTouchEnabled = true;
                    break;
            }

            Debug.Log($"交互设置已配置: 多点触控={Input.multiTouchEnabled}");
        }

        /// <summary>
        /// 显示欢迎消息
        /// </summary>
        public void ShowWelcomeMessage(string message)
        {
            if (welcomeDialog != null)
            {
                welcomeDialog.SetActive(true);
                var messageText = welcomeDialog.GetComponentInChildren<TextMeshProUGUI>();
                if (messageText != null)
                {
                    messageText.text = message;
                }

                // 3秒后自动隐藏
                Invoke(nameof(HideWelcomeDialog), 3f);
            }
        }

        /// <summary>
        /// 显示概念解释
        /// </summary>
        public void ShowConceptExplanation(string concept)
        {
            if (explanationDialog != null)
            {
                explanationDialog.SetActive(true);
                var explanationText = explanationDialog.GetComponentInChildren<TextMeshProUGUI>();
                if (explanationText != null)
                {
                    explanationText.text = $"概念解释：{concept}";
                }
            }
        }

        /// <summary>
        /// 显示错误对话框
        /// </summary>
        public void ShowErrorDialog(string title, string message)
        {
            if (errorDialog != null)
            {
                errorDialog.SetActive(true);
                var texts = errorDialog.GetComponentsInChildren<TextMeshProUGUI>();
                if (texts.Length >= 2)
                {
                    texts[0].text = title;
                    texts[1].text = message;
                }
            }
        }

        /// <summary>
        /// 显示测验界面
        /// </summary>
        public void ShowQuizInterface(object quiz)
        {
            if (quizDialog != null)
            {
                quizDialog.SetActive(true);
                // TODO: 实现测验界面逻辑
            }
        }

        /// <summary>
        /// 显示解释对话框
        /// </summary>
        public void ShowExplanationDialog(string explanation)
        {
            ShowConceptExplanation(explanation);
        }

        /// <summary>
        /// 隐藏欢迎对话框
        /// </summary>
        private void HideWelcomeDialog()
        {
            if (welcomeDialog != null)
            {
                welcomeDialog.SetActive(false);
            }
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        public void CloseDialog(GameObject dialog)
        {
            if (dialog != null)
            {
                dialog.SetActive(false);
            }
        }

        /// <summary>
        /// 切换分屏模式（仅平板支持）
        /// </summary>
        public void ToggleSplitScreenMode()
        {
            if (deviceManager.CurrentDeviceType == DeviceType.Tablet)
            {
                // 切换分屏模式逻辑
                bool isSplitMode = knowledgePanel != null && knowledgePanel.gameObject.activeInHierarchy;
                if (knowledgePanel != null)
                {
                    knowledgePanel.gameObject.SetActive(!isSplitMode);
                }

                // 调整AR视窗大小
                if (arViewport != null)
                {
                    if (isSplitMode)
                    {
                        // 全屏模式
                        arViewport.anchorMax = new Vector2(1f, 1f);
                    }
                    else
                    {
                        // 分屏模式
                        arViewport.anchorMax = new Vector2(0.7f, 1f);
                    }
                }
            }
        }

        /// <summary>
        /// 检查UI是否已配置
        /// </summary>
        public bool IsUIConfigured()
        {
            return isUIConfigured;
        }

        /// <summary>
        /// 获取当前UI模式
        /// </summary>
        public string GetCurrentUIMode()
        {
            switch (deviceManager.CurrentDeviceType)
            {
                case DeviceType.Phone:
                    return "手机模式";
                case DeviceType.Tablet:
                    return "平板模式";
                default:
                    return "未知模式";
            }
        }
    }
}