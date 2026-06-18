namespace Main.Scripts.Framework.Constant
{
    /// <summary>
    /// 项目全局常量，集中管理所有硬编码数值
    /// </summary>
    public static class GameConst
    {
        /// <summary>
        /// 场景参考宽度（像素）
        /// </summary>
        public const int SCENE_WIDTH = 854;

        /// <summary>
        /// 场景参考高度（像素）
        /// </summary>
        public const int SCENE_HEIGHT = 480;

        /// <summary>
        /// 场景世界半宽 = SCENE_WIDTH / 2 / PIXELS_PER_UNIT
        /// </summary>
        public const float PIXELS_PER_UNIT = 1f;

        /// <summary>
        /// 场景世界半高 = SCENE_HEIGHT / 2 / PIXELS_PER_UNIT
        /// </summary>
        public const float HALF_WORLD_HEIGHT = SCENE_HEIGHT * 0.5f / PIXELS_PER_UNIT;

        /// <summary>
        /// 场景世界半宽 = SCENE_WIDTH / 2 / PIXELS_PER_UNIT
        /// </summary>
        public const float HALF_WORLD_WIDTH = SCENE_WIDTH * 0.5f / PIXELS_PER_UNIT;
    }
}