using UnityEngine;
using System.Threading.Tasks;

namespace StarWhisper
{
    /// <summary>
    /// StarWhisper AR教育平台主控制器
    /// 支持手机和平板双端适配
    /// </summary>
    public class StarWhisperEducationPlatform : MonoBehaviour
    {
        [Header("核心技术栈")]
        [SerializeField] private DecartXRMobileController decartController;
        [SerializeField] private GraphRAGAgent knowledgeAgent;

        [Header("教育专用模块")]
        [SerializeField] private EducationalPromptEngine promptEngine;
        [SerializeField] private LearningAnalyticsEngine analyticsEngine;
        [SerializeField] private StarWhisperContentManager contentManager;

        [Header("设备适配")]
        [SerializeField] private DeviceAdaptationManager deviceManager;
        [SerializeField] private ResponsiveUIManager uiManager;

        [Header("用户管理")]
        [SerializeField] private StudentProfileManager studentManager;

        private Student currentStudent;
        private LearningSession currentSession;
        private bool isPlatformInitialized = false;

        private async void Start()
        {
            Debug.Log("🚀 StarWhisper AR教育平台启动...");
            await InitializePlatform();
        }

        /// <summary>
        /// 平台初始化流程
        /// </summary>
        private async Task InitializePlatform()
        {
            try
            {
                // 1. 检测设备类型并适配
                Debug.Log("📱 检测设备类型...");
                await deviceManager.DetectAndConfigureDevice();

                // 2. 配置响应式UI
                Debug.Log("🎨 配置响应式UI...");
                await uiManager.ConfigureForCurrentDevice();

                // 3. 初始化Decart-XR移动端
                Debug.Log("🔧 初始化Decart-XR...");
                await decartController.InitializeMobileAR();

                // 4. 连接教育知识图谱
                Debug.Log("🧠 连接Graph RAG知识库...");
                await knowledgeAgent.ConnectToKnowledgeBase();

                // 5. 加载学生档案
                Debug.Log("👨‍🎓 加载学生档案...");
                currentStudent = await studentManager.LoadOrCreateStudent();

                // 6. 启动学习分析引擎
                Debug.Log("📊 启动学习分析...");
                await analyticsEngine.Initialize(currentStudent);

                isPlatformInitialized = true;
                Debug.Log("✅ StarWhisper教育平台初始化完成！");

                // 7. 创建欢迎场景
                await CreateWelcomeScene();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ 平台初始化失败: {ex.Message}");
                ShowErrorDialog("初始化失败", "请检查网络连接后重试");
            }
        }

        /// <summary>
        /// 创建学习体验
        /// </summary>
        public async Task<LearningExperience> CreateLearningExperience(
            string subject,
            string concept,
            DifficultyLevel difficulty = DifficultyLevel.Medium)
        {
            if (!isPlatformInitialized)
            {
                Debug.LogWarning("平台未初始化完成");
                return null;
            }

            Debug.Log($"🎯 创建{subject}-{concept}学习体验...");

            try
            {
                // 1. Graph RAG检索相关知识
                var knowledgeContext = await knowledgeAgent.RetrieveRelevantKnowledge(
                    studentProfile: currentStudent,
                    subject: subject,
                    concept: concept
                );

                // 2. 生成教育专用Prompt
                var educationalPrompt = await promptEngine.GenerateEducationalPrompt(
                    knowledge: knowledgeContext,
                    student: currentStudent,
                    difficulty: difficulty,
                    deviceType: deviceManager.CurrentDeviceType
                );

                // 3. Decart-XR创建AR学习场景
                var arExperience = await decartController.CreateEducationalARScene(
                    prompt: educationalPrompt,
                    deviceSpecs: deviceManager.GetCurrentDeviceSpecs()
                );

                // 4. 启动学习会话跟踪
                currentSession = await analyticsEngine.StartLearningSession(
                    student: currentStudent,
                    subject: subject,
                    concept: concept,
                    arExperience: arExperience
                );

                // 5. 返回完整学习体验
                var experience = new LearningExperience
                {
                    knowledgeFoundation = knowledgeContext,
                    arVisualization = arExperience,
                    learningSession = currentSession,
                    deviceOptimized = true
                };

                Debug.Log("✨ 学习体验创建成功！");
                return experience;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ 创建学习体验失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 创建欢迎场景
        /// </summary>
        private async Task CreateWelcomeScene()
        {
            var welcomeExperience = await CreateLearningExperience(
                subject: "数学",
                concept: "几何入门",
                difficulty: DifficultyLevel.Easy
            );

            if (welcomeExperience != null)
            {
                Debug.Log("🎉 欢迎场景已准备就绪！");
                uiManager.ShowWelcomeMessage("欢迎来到《星语低语》学习世界！");
            }
        }

        /// <summary>
        /// 处理学生交互
        /// </summary>
        public async void OnStudentInteraction(InteractionType type, Vector3 position, string input = null)
        {
            if (currentSession == null) return;

            // 记录交互数据
            await analyticsEngine.RecordInteraction(
                sessionId: currentSession.sessionId,
                interactionType: type,
                position: position,
                additionalData: input
            );

            // 处理具体交互
            switch (type)
            {
                case InteractionType.Touch:
                    await HandleTouchInteraction(position);
                    break;
                case InteractionType.Voice:
                    await HandleVoiceInteraction(input);
                    break;
                case InteractionType.Gesture:
                    await HandleGestureInteraction(position);
                    break;
            }

            // 实时学习分析
            await analyticsEngine.AnalyzeLearningProgress(currentSession);
        }

        private async Task HandleTouchInteraction(Vector3 position)
        {
            var touchedObject = GetARObjectAtPosition(position);
            if (touchedObject != null)
            {
                await touchedObject.OnTouch();
                uiManager.ShowConceptExplanation(touchedObject.concept);
            }
        }

        private async Task HandleVoiceInteraction(string voiceInput)
        {
            var intent = await ParseEducationalIntent(voiceInput);
            await ExecuteEducationalAction(intent);
        }

        private async Task HandleGestureInteraction(Vector3 position)
        {
            // 根据设备类型处理不同的手势
            if (deviceManager.CurrentDeviceType == DeviceType.Tablet)
            {
                await HandleTabletGesture(position);
            }
            else
            {
                await HandlePhoneGesture(position);
            }
        }

        private ARLearningObject GetARObjectAtPosition(Vector3 position)
        {
            // 射线检测AR对象
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.collider.GetComponent<ARLearningObject>();
            }
            return null;
        }

        private async Task<EducationalIntent> ParseEducationalIntent(string voiceInput)
        {
            // 使用NLP解析教育意图
            return await knowledgeAgent.ParseEducationalIntent(voiceInput);
        }

        private async Task ExecuteEducationalAction(EducationalIntent intent)
        {
            // 执行教育相关操作
            switch (intent.ActionType)
            {
                case EducationalActionType.Explain:
                    await ShowConceptExplanation(intent.Target);
                    break;
                case EducationalActionType.Demonstrate:
                    await DemonstrateProcess(intent.Target);
                    break;
                case EducationalActionType.Quiz:
                    await StartQuiz(intent.Target);
                    break;
            }
        }

        private async Task ShowConceptExplanation(string concept)
        {
            var explanation = await knowledgeAgent.GetConceptExplanation(concept, currentStudent.age);
            uiManager.ShowExplanationDialog(explanation);
        }

        private async Task DemonstrateProcess(string process)
        {
            await decartController.StartProcessDemonstration(process);
        }

        private async Task StartQuiz(string topic)
        {
            var quiz = await knowledgeAgent.GenerateQuiz(topic, currentStudent.currentLevel);
            uiManager.ShowQuizInterface(quiz);
        }

        private void ShowErrorDialog(string title, string message)
        {
            uiManager.ShowErrorDialog(title, message);
        }

        private async Task HandleTabletGesture(Vector3 position)
        {
            // 平板特有的手势处理
            Debug.Log("处理平板手势...");
        }

        private async Task HandlePhoneGesture(Vector3 position)
        {
            // 手机特有的手势处理
            Debug.Log("处理手机手势...");
        }
    }
}