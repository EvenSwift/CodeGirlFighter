using Cysharp.Threading.Tasks;
using Main.Scripts.Entity;
using Main.Scripts.Framework.Controller.Base;
using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Model.Data;
using UnityEngine;

namespace Main.Scripts.Framework.Controller.Character
{
    /// <summary>
    /// 格斗者控制器，负责角色的创建、回收和生命周期管理
    /// </summary>
    public class FighterController : SyncPureController
    {
        private const string CHARACTER_PREFAB_PATH = "Assets/Main/Addressable/Prefabs/Character/CharacterPre.prefab";

        public CharacterEntityView Player { get; private set; }
        public CharacterEntityView Opponent { get; private set; }

        protected override async UniTask OnInitialize(IInitContext context)
        {
            await base.OnInitialize(context);
            await LoadCharacters();
        }

        private async UniTask LoadCharacters()
        {
            var playerPos = new Vector3(-2f, 0f, 0f);
            var opponentPos = new Vector3(2f, 0f, 0f);

            Player = await CreateCharacter(playerPos, false);
            Opponent = await CreateCharacter(opponentPos, true);
        }

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