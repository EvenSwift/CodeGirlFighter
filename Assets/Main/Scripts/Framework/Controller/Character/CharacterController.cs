using Cysharp.Threading.Tasks;
using Main.Scripts.Entity;
using Main.Scripts.Framework.Controller.Base;
using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Model.Data;
using UnityEngine;

namespace Main.Scripts.Framework.Controller.Character
{
    /// <summary>
    /// 角色控制器，负责角色的创建、回收和生命周期管理
    /// </summary>
    public class CharacterController : SyncPureController
    {
        /// <summary>
        /// 角色预制体 Addressable 路径
        /// </summary>
        private const string CHARACTER_PREFAB_PATH = "Assets/Main/Addressable/Prefabs/Character/CharacterPre.prefab";

        /// <summary>
        /// 玩家角色
        /// </summary>
        public CharacterEntityView Player { get; private set; }

        /// <summary>
        /// 对手角色
        /// </summary>
        public CharacterEntityView Opponent { get; private set; }

        protected override async UniTask OnInitialize(IInitContext context)
        {
            await base.OnInitialize(context);
            await LoadCharacters();
        }

        /// <summary>
        /// 加载双方角色（玩家在左侧，对手在右侧）
        /// </summary>
        private async UniTask LoadCharacters()
        {
            var playerPos = new Vector3(-2f, 0f, 0f);
            var opponentPos = new Vector3(2f, 0f, 0f);

            Player = await CreateCharacter(playerPos, false);
            Opponent = await CreateCharacter(opponentPos, true);
        }

        /// <summary>
        /// 在指定位置创建一个角色
        /// </summary>
        private async UniTask<CharacterEntityView> CreateCharacter(Vector3 position, bool flipped)
        {
            var data = new CharacterData(configId: 1, position, isFlipped: flipped);
            var context = new CharacterEntityViewInitContext(this, data);

            var view = await this.InstantiateAsync<CharacterEntityView>(
                CHARACTER_PREFAB_PATH,
                context: context
            );

            return view;
        }

        protected override void OnRelease()
        {
            if (Player != null)
            {
                Object.Destroy(Player.gameObject);
                Player = null;
            }

            if (Opponent != null)
            {
                Object.Destroy(Opponent.gameObject);
                Opponent = null;
            }
        }
    }
}