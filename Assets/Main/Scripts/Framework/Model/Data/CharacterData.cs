using System;
using Main.Scripts.Framework.Model.Data.Interface;
using UnityEngine;

namespace Main.Scripts.Framework.Model.Data
{
    /// <summary>
    /// 角色数据类，存储角色的身份、属性和视图状态
    /// </summary>
    [Serializable]
    public class CharacterData : IData
    {
        /// <summary>
        /// 唯一标识符，全局不可重复
        /// </summary>
        public string uid;

        /// <summary>
        /// 配置表 ID，对应角色基础属性、立绘路径等
        /// </summary>
        public int configId;

        /// <summary>
        /// 当前生命值
        /// </summary>
        public int health;

        /// <summary>
        /// 最大生命值
        /// </summary>
        public int maxHealth;

        /// <summary>
        /// 当前耐力值，用于必杀技、防御等消耗
        /// </summary>
        public int stamina;

        /// <summary>
        /// 最大耐力值
        /// </summary>
        public int maxStamina;

        /// <summary>
        /// 世界坐标位置，View 层同步
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// 是否水平翻转，控制角色朝向（true = 向左）
        /// </summary>
        public bool isFlipped;

        /// <summary>
        /// 当前播放的动画状态名，View 层同步
        /// </summary>
        public string currentAnimation;

        /// <summary>
        /// 构造角色数据，uid 为空时自动生成
        /// </summary>
        public CharacterData(int configId, Vector3 position, string uid = null)
        {
            this.configId = configId;
            this.position = position;
            this.uid = string.IsNullOrEmpty(uid) ? Guid.NewGuid().ToString() : uid;
        }

        /// <summary>
        /// 无参构造，供序列化反序列化使用
        /// </summary>
        private CharacterData() { }
    }
}