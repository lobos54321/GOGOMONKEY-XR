using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace StarWhisper
{
    /// <summary>
    /// 学习难度等级
    /// </summary>
    public enum DifficultyLevel
    {
        Easy = 1,      // 简单
        Medium = 2,    // 中等
        Hard = 3,      // 困难
        Expert = 4     // 专家
    }

    /// <summary>
    /// 学习风格
    /// </summary>
    public enum LearningStyle
    {
        Visual,        // 视觉型
        Kinesthetic,   // 动觉型
        Auditory,      // 听觉型
        ReadWrite      // 读写型
    }

    /// <summary>
    /// 教育Prompt引擎
    /// 专门为教育场景生成优化的AI Prompt
    /// </summary>
    public class EducationalPromptEngine : MonoBehaviour
    {
        [Header("学科配置")]
        [SerializeField] private SubjectStyleDatabase subjectStyles;
        [SerializeField] private StarWhisperStyleLibrary starWhisperStyles;

        [Header("年龄适配")]
        [SerializeField] private AgeGroupSettings[] ageGroups;

        [Header("设备优化")]
        [SerializeField] private DevicePromptSettings deviceSettings;

        private Dictionary<string, SubjectPromptTemplate> subjectTemplates;
        private Dictionary<int, AgeAppropriateVocabulary> ageVocabularies;

        private void Awake()
        {
            InitializePromptEngine();
        }

        /// <summary>
        /// 初始化Prompt引擎
        /// </summary>
        private void InitializePromptEngine()
        {
            // 初始化学科模板
            InitializeSubjectTemplates();

            // 初始化年龄词汇库
            InitializeAgeVocabularies();

            Debug.Log("📝 教育Prompt引擎初始化完成");
        }

        /// <summary>
        /// 生成教育专用Prompt
        /// </summary>
        public async Task<string> GenerateEducationalPrompt(
            KnowledgeContext knowledge,
            Student student,
            DifficultyLevel difficulty,
            DeviceType deviceType = DeviceType.Phone)
        {
            Debug.Log($"🎯 生成教育Prompt: {knowledge.subject} - {knowledge.targetConcept}");

            try
            {
                // 1. 获取学科风格
                var visualStyle = GetSubjectVisualStyle(knowledge.subject);

                // 2. 获取《星语低语》元素
                var starWhisperElements = GetStarWhisperElements(student.age);

                // 3. 适配年龄和能力
                var adaptedContent = await AdaptContentForStudent(knowledge, student, difficulty);

                // 4. 设备优化
                var deviceOptimizations = GetDeviceOptimizations(deviceType);

                // 5. 构建完整Prompt
                var prompt = await BuildComprehensivePrompt(
                    knowledge,
                    student,
                    adaptedContent,
                    visualStyle,
                    starWhisperElements,
                    deviceOptimizations,
                    difficulty
                );

                Debug.Log($"✅ Prompt生成完成: {prompt.Length}字符");
                return prompt;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Prompt生成失败: {ex.Message}");
                return GenerateFallbackPrompt(knowledge.subject, knowledge.targetConcept);
            }
        }

        /// <summary>
        /// 获取学科视觉风格
        /// </summary>
        private SubjectVisualStyle GetSubjectVisualStyle(string subject)
        {
            var styleMap = new Dictionary<string, SubjectVisualStyle>
            {
                ["数学"] = new SubjectVisualStyle
                {
                    primaryColors = new[] { "#4CAF50", "#2196F3", "#FF9800" },
                    visualMetaphors = new[] { "几何星座", "数字宇宙", "计算星云" },
                    environmentStyle = "简洁现代的数学实验室",
                    interactionStyle = "精准的几何操作"
                },
                ["科学"] = new SubjectVisualStyle
                {
                    primaryColors = new[] { "#9C27B0", "#3F51B5", "#00BCD4" },
                    visualMetaphors = new[] { "科学实验站", "分子世界", "物理定律可视化" },
                    environmentStyle = "高科技实验室环境",
                    interactionStyle = "实验式探索互动"
                },
                ["历史"] = new SubjectVisualStyle
                {
                    primaryColors = new[] { "#795548", "#FF5722", "#FFC107" },
                    visualMetaphors = new[] { "时空隧道", "历史场景重现", "文明遗迹" },
                    environmentStyle = "历史时期真实场景",
                    interactionStyle = "沉浸式历史体验"
                },
                ["语文"] = new SubjectVisualStyle
                {
                    primaryColors = new[] { "#E91E63", "#9C27B0", "#673AB7" },
                    visualMetaphors = new[] { "文字花园", "诗词星河", "故事世界" },
                    environmentStyle = "诗意的文学空间",
                    interactionStyle = "创作型文字互动"
                }
            };

            return styleMap.ContainsKey(subject) ? styleMap[subject] : styleMap["数学"];
        }

        /// <summary>
        /// 获取《星语低语》风格元素
        /// </summary>
        private StarWhisperElements GetStarWhisperElements(int age)
        {
            return new StarWhisperElements
            {
                narrativeRole = "年轻的太空探索者",
                companionCharacter = "智慧的AI学习伙伴小星",
                environmentTheme = "神秘的外星学习基地",
                discoveryMotif = "解锁宇宙知识密码",
                achievementSystem = "星际学者徽章系统",
                ageAppropriateElements = GetAgeAppropriateElements(age)
            };
        }

        /// <summary>
        /// 获取年龄适配元素
        /// </summary>
        private string[] GetAgeAppropriateElements(int age)
        {
            if (age <= 6)
            {
                return new[] { "可爱的外星小动物", "彩色的能量水晶", "简单的星际工具" };
            }
            else if (age <= 9)
            {
                return new[] { "神奇的科技装置", "发光的知识符文", "智能机器人助手" };
            }
            else if (age <= 12)
            {
                return new[] { "复杂的星际设备", "古老的文明遗迹", "高级AI分析系统" };
            }
            else
            {
                return new[] { "前沿科技实验室", "多维度知识网络", "自主研究系统" };
            }
        }

        /// <summary>
        /// 为学生适配内容
        /// </summary>
        private async Task<AdaptedEducationalContent> AdaptContentForStudent(
            KnowledgeContext knowledge,
            Student student,
            DifficultyLevel difficulty)
        {
            // 获取年龄适配词汇
            var vocabulary = GetAgeAppropriateVocabulary(student.age);

            // 适配概念解释
            var adaptedConcepts = await AdaptConceptsForAge(knowledge.keyConcepts, student.age, vocabulary);

            // 生成学习活动
            var learningActivities = GenerateLearningActivities(
                knowledge.targetConcept,
                student.learningStyle,
                difficulty
            );

            // 创建评估检查点
            var assessmentPoints = CreateAssessmentPoints(knowledge.targetConcept, student.currentLevel);

            return new AdaptedEducationalContent
            {
                adaptedConcepts = adaptedConcepts,
                learningActivities = learningActivities,
                assessmentPoints = assessmentPoints,
                vocabularyLevel = vocabulary.level,
                estimatedDuration = CalculateEstimatedDuration(student.attentionSpan, difficulty)
            };
        }

        /// <summary>
        /// 获取年龄适配词汇
        /// </summary>
        private AgeAppropriateVocabulary GetAgeAppropriateVocabulary(int age)
        {
            if (ageVocabularies.ContainsKey(age))
            {
                return ageVocabularies[age];
            }

            // 根据年龄范围返回适当的词汇等级
            if (age <= 6)
            {
                return new AgeAppropriateVocabulary { level = "简单", complexity = 1 };
            }
            else if (age <= 9)
            {
                return new AgeAppropriateVocabulary { level = "基础", complexity = 2 };
            }
            else if (age <= 12)
            {
                return new AgeAppropriateVocabulary { level = "中级", complexity = 3 };
            }
            else
            {
                return new AgeAppropriateVocabulary { level = "高级", complexity = 4 };
            }
        }

        /// <summary>
        /// 适配概念为年龄合适的表达
        /// </summary>
        private async Task<List<string>> AdaptConceptsForAge(
            List<string> concepts,
            int age,
            AgeAppropriateVocabulary vocabulary)
        {
            var adaptedConcepts = new List<string>();

            foreach (var concept in concepts)
            {
                var adaptedConcept = await AdaptSingleConcept(concept, age, vocabulary);
                adaptedConcepts.Add(adaptedConcept);
            }

            return adaptedConcepts;
        }

        /// <summary>
        /// 适配单个概念
        /// </summary>
        private async Task<string> AdaptSingleConcept(string concept, int age, AgeAppropriateVocabulary vocabulary)
        {
            // 简化实现，实际应用中可以使用更复杂的NLP技术
            var adaptationRules = GetConceptAdaptationRules(age);

            foreach (var rule in adaptationRules)
            {
                if (concept.Contains(rule.Key))
                {
                    concept = concept.Replace(rule.Key, rule.Value);
                }
            }

            await Task.Delay(10); // 模拟异步处理
            return concept;
        }

        /// <summary>
        /// 获取概念适配规则
        /// </summary>
        private Dictionary<string, string> GetConceptAdaptationRules(int age)
        {
            if (age <= 6)
            {
                return new Dictionary<string, string>
                {
                    ["几何体"] = "形状",
                    ["坐标"] = "位置",
                    ["函数"] = "规律",
                    ["变量"] = "会变的数"
                };
            }
            else if (age <= 9)
            {
                return new Dictionary<string, string>
                {
                    ["算法"] = "解题方法",
                    ["数据结构"] = "信息整理方式",
                    ["概率"] = "可能性"
                };
            }
            else
            {
                return new Dictionary<string, string>(); // 高年级使用原始术语
            }
        }

        /// <summary>
        /// 生成学习活动
        /// </summary>
        private List<LearningActivity> GenerateLearningActivities(
            string concept,
            LearningStyle style,
            DifficultyLevel difficulty)
        {
            var activities = new List<LearningActivity>();

            switch (style)
            {
                case LearningStyle.Visual:
                    activities.Add(new LearningActivity
                    {
                        type = "可视化探索",
                        description = $"通过3D模型和图表理解{concept}",
                        duration = 5
                    });
                    break;

                case LearningStyle.Kinesthetic:
                    activities.Add(new LearningActivity
                    {
                        type = "动手操作",
                        description = $"通过触摸和移动AR对象学习{concept}",
                        duration = 7
                    });
                    break;

                case LearningStyle.Auditory:
                    activities.Add(new LearningActivity
                    {
                        type = "语音交互",
                        description = $"通过对话和声音效果理解{concept}",
                        duration = 6
                    });
                    break;
            }

            return activities;
        }

        /// <summary>
        /// 创建评估检查点
        /// </summary>
        private List<string> CreateAssessmentPoints(string concept, int currentLevel)
        {
            return new List<string>
            {
                $"能否识别{concept}的基本特征",
                $"能否解释{concept}的工作原理",
                $"能否将{concept}应用到新情境",
                $"能否与已学知识建立联系"
            };
        }

        /// <summary>
        /// 计算预估时长
        /// </summary>
        private int CalculateEstimatedDuration(int attentionSpan, DifficultyLevel difficulty)
        {
            var baseDuration = attentionSpan * 0.8f; // 使用80%的注意力时长
            var difficultyMultiplier = 1.0f + ((int)difficulty - 1) * 0.3f;
            return Mathf.RoundToInt(baseDuration * difficultyMultiplier);
        }

        /// <summary>
        /// 获取设备优化配置
        /// </summary>
        private DeviceOptimizations GetDeviceOptimizations(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.Phone:
                    return new DeviceOptimizations
                    {
                        maxObjects = 5,
                        interactionComplexity = "简单",
                        visualComplexity = "中等",
                        recommendedViewDistance = "30-50cm"
                    };

                case DeviceType.Tablet:
                    return new DeviceOptimizations
                    {
                        maxObjects = 10,
                        interactionComplexity = "复杂",
                        visualComplexity = "高",
                        recommendedViewDistance = "40-70cm"
                    };

                default:
                    return new DeviceOptimizations
                    {
                        maxObjects = 5,
                        interactionComplexity = "简单",
                        visualComplexity = "中等",
                        recommendedViewDistance = "30-50cm"
                    };
            }
        }

        /// <summary>
        /// 构建完整的教育Prompt
        /// </summary>
        private async Task<string> BuildComprehensivePrompt(
            KnowledgeContext knowledge,
            Student student,
            AdaptedEducationalContent content,
            SubjectVisualStyle visualStyle,
            StarWhisperElements starWhisperElements,
            DeviceOptimizations deviceOpt,
            DifficultyLevel difficulty)
        {
            var promptBuilder = new StringBuilder();

            // 1. 场景设定
            promptBuilder.AppendLine($"创建一个基于《星语低语》世界观的{knowledge.subject}学习场景：");
            promptBuilder.AppendLine();

            // 2. 教育背景
            promptBuilder.AppendLine("## 教育背景");
            promptBuilder.AppendLine($"- 学习目标：{knowledge.learningObjective}");
            promptBuilder.AppendLine($"- 核心概念：{string.Join("、", content.adaptedConcepts)}");
            promptBuilder.AppendLine($"- 学生年龄：{student.age}岁");
            promptBuilder.AppendLine($"- 难度等级：{difficulty}");
            promptBuilder.AppendLine($"- 学习风格：{student.learningStyle}");
            promptBuilder.AppendLine($"- 预计时长：{content.estimatedDuration}分钟");
            promptBuilder.AppendLine();

            // 3. 《星语低语》叙事框架
            promptBuilder.AppendLine("## 《星语低语》叙事框架");
            promptBuilder.AppendLine($"学生扮演：{starWhisperElements.narrativeRole}");
            promptBuilder.AppendLine($"AI伙伴：{starWhisperElements.companionCharacter}");
            promptBuilder.AppendLine($"学习环境：{starWhisperElements.environmentTheme}");
            promptBuilder.AppendLine($"探索主题：{starWhisperElements.discoveryMotif}");
            promptBuilder.AppendLine($"成就系统：{starWhisperElements.achievementSystem}");
            promptBuilder.AppendLine();

            // 4. 视觉风格要求
            promptBuilder.AppendLine("## 视觉风格要求");
            promptBuilder.AppendLine($"- 主色调：{string.Join("、", visualStyle.primaryColors)}");
            promptBuilder.AppendLine($"- 环境风格：{visualStyle.environmentStyle}");
            promptBuilder.AppendLine($"- 交互方式：{visualStyle.interactionStyle}");
            promptBuilder.AppendLine($"- 视觉隐喻：{string.Join("、", visualStyle.visualMetaphors)}");
            promptBuilder.AppendLine($"- 年龄元素：{string.Join("、", starWhisperElements.ageAppropriateElements)}");
            promptBuilder.AppendLine();

            // 5. 互动学习要求
            promptBuilder.AppendLine("## 互动学习要求");
            promptBuilder.AppendLine("创建AR元素，要求：");
            promptBuilder.AppendLine("- 让抽象概念变得具体可操作");
            promptBuilder.AppendLine("- 触摸时提供即时视觉反馈");
            promptBuilder.AppendLine("- 鼓励主动探索和发现");
            promptBuilder.AppendLine("- 连接到真实世界应用");
            promptBuilder.AppendLine("- 支持多种学习方式");
            promptBuilder.AppendLine();

            // 6. 学习活动设计
            promptBuilder.AppendLine("## 学习活动设计");
            foreach (var activity in content.learningActivities)
            {
                promptBuilder.AppendLine($"- {activity.type}：{activity.description}（{activity.duration}分钟）");
            }
            promptBuilder.AppendLine();

            // 7. 设备优化
            promptBuilder.AppendLine("## 设备优化要求");
            promptBuilder.AppendLine($"- 最大对象数：{deviceOpt.maxObjects}个");
            promptBuilder.AppendLine($"- 交互复杂度：{deviceOpt.interactionComplexity}");
            promptBuilder.AppendLine($"- 视觉复杂度：{deviceOpt.visualComplexity}");
            promptBuilder.AppendLine($"- 推荐观看距离：{deviceOpt.recommendedViewDistance}");
            promptBuilder.AppendLine();

            // 8. 评估检查点
            promptBuilder.AppendLine("## 学习评估检查点");
            foreach (var checkpoint in content.assessmentPoints)
            {
                promptBuilder.AppendLine($"- {checkpoint}");
            }
            promptBuilder.AppendLine();

            // 9. 技术规格
            promptBuilder.AppendLine("## 技术规格要求");
            promptBuilder.AppendLine("- 优化移动端AR性能（30-60fps）");
            promptBuilder.AppendLine("- 支持触摸和语音交互");
            promptBuilder.AppendLine("- 最小化动晕风险");
            promptBuilder.AppendLine("- 节能高效渲染");
            promptBuilder.AppendLine();

            // 10. 最终要求
            promptBuilder.AppendLine("请创建一个让学生感觉像在《星语低语》宇宙中");
            promptBuilder.AppendLine($"探索{knowledge.subject}奥秘的沉浸式AR学习体验！");

            await Task.Delay(50); // 模拟构建时间
            return promptBuilder.ToString();
        }

        /// <summary>
        /// 生成备用Prompt
        /// </summary>
        private string GenerateFallbackPrompt(string subject, string concept)
        {
            return $@"
创建一个{subject}学习场景，重点学习{concept}概念：

在《星语低语》的神秘外星基地中，学生作为年轻的宇宙探索者，
需要通过互动AR对象来理解{concept}的原理。

要求：
- 创建3-5个可互动的AR学习对象
- 使用科幻但温暖的视觉风格
- 支持触摸交互和语音提示
- 适合移动设备显示
- 提供即时学习反馈

让学习{concept}成为一次有趣的星际探索之旅！
            ";
        }

        /// <summary>
        /// 初始化学科模板
        /// </summary>
        private void InitializeSubjectTemplates()
        {
            subjectTemplates = new Dictionary<string, SubjectPromptTemplate>();
            // 这里可以加载预定义的学科模板
        }

        /// <summary>
        /// 初始化年龄词汇库
        /// </summary>
        private void InitializeAgeVocabularies()
        {
            ageVocabularies = new Dictionary<int, AgeAppropriateVocabulary>();
            // 这里可以加载年龄适配的词汇库
        }
    }

    // 支持类定义
    [System.Serializable]
    public class SubjectVisualStyle
    {
        public string[] primaryColors;
        public string[] visualMetaphors;
        public string environmentStyle;
        public string interactionStyle;
    }

    [System.Serializable]
    public class StarWhisperElements
    {
        public string narrativeRole;
        public string companionCharacter;
        public string environmentTheme;
        public string discoveryMotif;
        public string achievementSystem;
        public string[] ageAppropriateElements;
    }

    [System.Serializable]
    public class AdaptedEducationalContent
    {
        public List<string> adaptedConcepts;
        public List<LearningActivity> learningActivities;
        public List<string> assessmentPoints;
        public string vocabularyLevel;
        public int estimatedDuration;
    }

    [System.Serializable]
    public class LearningActivity
    {
        public string type;
        public string description;
        public int duration;
    }

    [System.Serializable]
    public class DeviceOptimizations
    {
        public int maxObjects;
        public string interactionComplexity;
        public string visualComplexity;
        public string recommendedViewDistance;
    }

    [System.Serializable]
    public class AgeAppropriateVocabulary
    {
        public string level;
        public int complexity;
    }

    [System.Serializable]
    public class SubjectPromptTemplate
    {
        public string subject;
        public string baseTemplate;
        public Dictionary<string, string> conceptMappings;
    }
}