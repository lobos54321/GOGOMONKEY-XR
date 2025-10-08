using UnityEngine;
using System.Collections.Generic;

namespace StarWhisper
{
    /// <summary>
    /// 学生档案
    /// </summary>
    [System.Serializable]
    public class Student
    {
        [Header("基本信息")]
        public string id;
        public string name;
        public int age;
        public int gradeLevel;

        [Header("学习特征")]
        public LearningStyle learningStyle;
        public int currentLevel;
        public int attentionSpan; // 注意力持续时间（分钟）

        [Header("能力和偏好")]
        public List<string> masteredConcepts = new List<string>();
        public List<string> preferences = new List<string>();
        public StudentCapabilities capabilities;

        [Header("学习记录")]
        public float totalLearningTime;
        public int sessionsCompleted;
        public float averageScore;
        public System.DateTime lastLearningDate;

        /// <summary>
        /// 年龄组
        /// </summary>
        public AgeGroup ageGroup
        {
            get
            {
                if (age <= 6) return AgeGroup.EarlyChildhood;
                if (age <= 9) return AgeGroup.ElementaryEarly;
                if (age <= 12) return AgeGroup.ElementaryLate;
                return AgeGroup.MiddleSchool;
            }
        }
    }

    /// <summary>
    /// 年龄组枚举
    /// </summary>
    public enum AgeGroup
    {
        EarlyChildhood,  // 3-6岁：幼儿期
        ElementaryEarly, // 7-9岁：小学低年级
        ElementaryLate,  // 10-12岁：小学高年级
        MiddleSchool     // 13+岁：中学
    }

    /// <summary>
    /// 学生能力评估
    /// </summary>
    [System.Serializable]
    public class StudentCapabilities
    {
        public float cognitiveCapacity;      // 认知能力
        public float processingSpeed;        // 处理速度
        public float memoryRetention;        // 记忆保持
        public float spatialAbility;         // 空间能力
        public float logicalReasoning;       // 逻辑推理
        public int availableTime;            // 可用学习时间（分钟）
        public string[] preferredLearningModes; // 偏好的学习模式
    }

    /// <summary>
    /// 知识上下文
    /// </summary>
    [System.Serializable]
    public class KnowledgeContext
    {
        [Header("核心信息")]
        public string subject;           // 学科
        public string targetConcept;     // 目标概念
        public string learningObjective; // 学习目标

        [Header("知识结构")]
        public List<string> keyConcepts = new List<string>();
        public List<string> prerequisites = new List<string>();
        public List<string> realWorldApplications = new List<string>();

        [Header("个性化内容")]
        public LearningPath personalizedPath;
        public List<string> potentialDifficulties = new List<string>();
        public List<string> assessmentPoints = new List<string>();
        public List<string> connectionsToPriorKnowledge = new List<string>();
        public List<string> suggestedActivities = new List<string>();
    }

    /// <summary>
    /// 学习路径
    /// </summary>
    [System.Serializable]
    public class LearningPath
    {
        public string pathId;
        public List<LearningStep> steps = new List<LearningStep>();
        public int estimatedDuration; // 预计时长（分钟）
        public DifficultyLevel difficulty;
        public string personalizedFor; // 为哪个学生定制
    }

    /// <summary>
    /// 学习步骤
    /// </summary>
    [System.Serializable]
    public class LearningStep
    {
        public string stepId;
        public string title;
        public string description;
        public string concept;
        public int duration; // 步骤时长（分钟）
        public bool isCompleted;
        public float completionScore;
    }

    /// <summary>
    /// 学习体验
    /// </summary>
    [System.Serializable]
    public class LearningExperience
    {
        [Header("体验基础")]
        public string experienceId;
        public KnowledgeContext knowledgeFoundation;
        public ARLearningExperience arVisualization;
        public LearningSession learningSession;

        [Header("体验特征")]
        public bool deviceOptimized;
        public InteractionMode interactionMode;
        public System.DateTime createdTime;
        public bool isActive;

        [Header("学习成果")]
        public float engagementScore;
        public float learningEffectiveness;
        public List<string> achievedObjectives = new List<string>();
    }

    /// <summary>
    /// 交互模式枚举
    /// </summary>
    public enum InteractionMode
    {
        TouchOnly,        // 仅触摸
        VoiceOnly,        // 仅语音
        TouchAndVoice,    // 触摸+语音
        Gesture,          // 手势
        Multimodal        // 多模态
    }

    /// <summary>
    /// 学习会话
    /// </summary>
    [System.Serializable]
    public class LearningSession
    {
        [Header("会话信息")]
        public string sessionId;
        public string studentId;
        public string subject;
        public string concept;

        [Header("时间记录")]
        public System.DateTime startTime;
        public System.DateTime? endTime;
        public float duration; // 会话时长（分钟）

        [Header("互动记录")]
        public List<InteractionRecord> interactions = new List<InteractionRecord>();
        public int totalInteractions;

        [Header("学习分析")]
        public float engagementLevel;
        public float conceptMastery;
        public List<string> struggledConcepts = new List<string>();
        public List<string> masteredConcepts = new List<string>();

        [Header("反馈")]
        public string studentFeedback;
        public float satisfactionScore;
        public List<string> suggestions = new List<string>();
    }

    /// <summary>
    /// 交互记录
    /// </summary>
    [System.Serializable]
    public class InteractionRecord
    {
        public string interactionId;
        public InteractionType type;
        public System.DateTime timestamp;
        public Vector3 position;
        public string additionalData;
        public float duration;
        public bool wasSuccessful;
    }

    /// <summary>
    /// 交互类型枚举
    /// </summary>
    public enum InteractionType
    {
        Touch,           // 触摸
        Voice,           // 语音
        Gesture,         // 手势
        Gaze,            // 凝视
        Drag,            // 拖拽
        Pinch,           // 捏合
        Rotate,          // 旋转
        Scale            // 缩放
    }

    /// <summary>
    /// 教育意图
    /// </summary>
    [System.Serializable]
    public class EducationalIntent
    {
        public EducationalActionType ActionType;
        public string Target;           // 目标概念或对象
        public string Context;          // 上下文信息
        public float Confidence;        // 置信度
        public Dictionary<string, object> Parameters; // 参数
    }

    /// <summary>
    /// 教育动作类型
    /// </summary>
    public enum EducationalActionType
    {
        Explain,         // 解释
        Demonstrate,     // 演示
        Quiz,            // 测验
        Practice,        // 练习
        Review,          // 复习
        Explore,         // 探索
        Compare,         // 比较
        Apply            // 应用
    }

    /// <summary>
    /// AR学习对象
    /// </summary>
    public class ARLearningObject : MonoBehaviour
    {
        [Header("学习对象信息")]
        public string objectId;
        public string concept;          // 关联的概念
        public string description;      // 描述
        public DifficultyLevel difficulty;

        [Header("交互设置")]
        public bool isTouchable = true;
        public bool isRotatable = true;
        public bool isScalable = true;
        public bool hasAudioFeedback = true;

        [Header("学习数据")]
        public int interactionCount;
        public float totalInteractionTime;
        public float lastInteractionTime;

        /// <summary>
        /// 触摸响应
        /// </summary>
        public virtual async System.Threading.Tasks.Task OnTouch()
        {
            interactionCount++;
            lastInteractionTime = Time.time;

            // 播放触摸反馈
            await PlayTouchFeedback();

            // 触发学习事件
            OnLearningInteraction?.Invoke(this);
        }

        /// <summary>
        /// 播放触摸反馈
        /// </summary>
        protected virtual async System.Threading.Tasks.Task PlayTouchFeedback()
        {
            // 视觉反馈
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                var originalColor = renderer.material.color;
                renderer.material.color = Color.yellow;
                await System.Threading.Tasks.Task.Delay(200);
                renderer.material.color = originalColor;
            }

            // 音频反馈
            if (hasAudioFeedback)
            {
                var audioSource = GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Play();
                }
            }
        }

        /// <summary>
        /// 学习交互事件
        /// </summary>
        public System.Action<ARLearningObject> OnLearningInteraction;

        /// <summary>
        /// 序列化对象状态
        /// </summary>
        public virtual ARObjectState SerializeState()
        {
            return new ARObjectState
            {
                objectId = this.objectId,
                position = transform.position,
                rotation = transform.rotation,
                scale = transform.localScale,
                isActive = gameObject.activeInHierarchy,
                concept = this.concept,
                interactionCount = this.interactionCount
            };
        }
    }

    /// <summary>
    /// AR对象状态
    /// </summary>
    [System.Serializable]
    public class ARObjectState
    {
        public string objectId;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public bool isActive;
        public string concept;
        public int interactionCount;
    }

    /// <summary>
    /// 环境上下文
    /// </summary>
    [System.Serializable]
    public class EnvironmentContext
    {
        public string type;              // 环境类型（如：classroom, home, outdoor）
        public string lighting;          // 光照条件
        public Vector2 spaceSize;        // 可用空间大小
        public float noiseLevel;         // 噪音水平
        public List<string> availableObjects = new List<string>(); // 可用物体
    }

    /// <summary>
    /// 性能监控器
    /// </summary>
    public class PerformanceMonitor : MonoBehaviour
    {
        [Header("监控设置")]
        public bool enableMonitoring = true;
        public float updateInterval = 1.0f;

        private float lastUpdateTime;
        private PerformanceStatus currentStatus;

        private void Update()
        {
            if (!enableMonitoring) return;

            if (Time.time - lastUpdateTime >= updateInterval)
            {
                UpdatePerformanceStatus();
                lastUpdateTime = Time.time;
            }
        }

        /// <summary>
        /// 更新性能状态
        /// </summary>
        private void UpdatePerformanceStatus()
        {
            currentStatus = new PerformanceStatus
            {
                fps = Mathf.RoundToInt(1.0f / Time.deltaTime),
                memoryUsage = System.GC.GetTotalMemory(false) / 1024 / 1024, // MB
                temperature = SystemInfo.batteryTemperature,
                batteryLevel = SystemInfo.batteryLevel,
                isOptimal = CheckIfOptimal()
            };
        }

        /// <summary>
        /// 检查性能是否最优
        /// </summary>
        private bool CheckIfOptimal()
        {
            return currentStatus.fps >= 25 &&
                   currentStatus.memoryUsage < 2000 &&
                   currentStatus.temperature < 40f;
        }

        /// <summary>
        /// 开始监控
        /// </summary>
        public void StartMonitoring()
        {
            enableMonitoring = true;
            Debug.Log("🔍 性能监控已启动");
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void StopMonitoring()
        {
            enableMonitoring = false;
            Debug.Log("⏹️ 性能监控已停止");
        }

        /// <summary>
        /// 开始场景监控
        /// </summary>
        public void StartSceneMonitoring(ARLearningExperience scene)
        {
            Debug.Log($"🎬 开始监控场景: {scene?.sceneId}");
        }

        /// <summary>
        /// 获取当前状态
        /// </summary>
        public PerformanceStatus GetCurrentStatus()
        {
            return currentStatus ?? new PerformanceStatus();
        }
    }
}