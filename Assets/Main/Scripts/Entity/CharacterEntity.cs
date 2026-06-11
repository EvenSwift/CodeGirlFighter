using Main.Scripts.Entity.Base;
using Main.Scripts.Framework.Core;
using Main.Scripts.Framework.Model.Data;
using QFramework;

namespace Main.Scripts.Entity
{
    /// <summary>
    /// 角色实体，管理角色的纯数据逻辑
    /// View 相关表现由 CharacterViewBase 负责
    /// </summary>
    public class CharacterEntity : ICharacter
    {
        public CharacterData Data { get; set; }

        public CharacterEntity(CharacterData data)
        {
            Data = data;
        }

        /// <summary>
        /// TimeSystem 每帧回调，处理倒计时、状态推进等逻辑
        /// </summary>
        public void TickGameTime(double gameDeltaTime)
        {
            // 角色被动状态恢复等逻辑在此处理
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}