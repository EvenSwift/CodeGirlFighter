using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Model.Data;
using QFramework;

namespace Main.Scripts.Entity.Base
{
    /// <summary>
    /// 角色实体接口，实现此接口的角色可接入 TimeSystem 和 Command 架构
    /// </summary>
    public interface ICharacter : ITickGameTime, IController
    {
        CharacterData Data { get; set; }
    }
}