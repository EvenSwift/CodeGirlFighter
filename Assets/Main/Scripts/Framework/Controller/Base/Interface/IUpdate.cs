namespace CodeFighter.Framework.Controller.Base.Interface
{
    /*
     * 使用该接口去管理游戏中所有需要 Update 的组件
     * 在ControllerSet中集合，在GameManager中调用Update
     */
    public interface IUpdate
    {
        void Update();
    }
}
