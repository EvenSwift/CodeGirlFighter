namespace CodeFighter.Framework.Controller.Base.Interface
{
    // 所有需要追踪受 TimeSystem 影响的游戏时间的类都应该实现此接口
    public interface ITickGameTime
    {
        /// <summary>
        /// 当游戏时间流逝时被调用。
        /// </summary>
        /// <param name="gameDeltaTime">在上一帧中流逝的游戏时间（秒），受 TimeSpeedFactor 影响。</param>
        void TickGameTime(double gameDeltaTime);
    }
}
