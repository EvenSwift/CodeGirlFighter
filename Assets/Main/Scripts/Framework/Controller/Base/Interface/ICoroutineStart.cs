using System.Collections;
using CodeFighter.Framework.Core;
using UnityEngine;

namespace CodeFighter.Framework.Controller.Base.Interface
{
    public interface ICoroutineStarter
    {
        GameManager GetGameManager();
    }

    public static class ICoroutineStarterExtension
    {
        public static Coroutine StartCoroutine(this ICoroutineStarter self, IEnumerator enumerator)
        {
            var gameManager = self.GetGameManager();
            return gameManager && gameManager.isActiveAndEnabled ? gameManager.StartCoroutine(enumerator) : null;
        }

        public static void StopCoroutine(this ICoroutineStarter self, Coroutine coroutine)
        {
            var gameManager = self.GetGameManager();
            if (!gameManager || !gameManager.isActiveAndEnabled)
            {
                return;
            }

            gameManager.StopCoroutine(coroutine);
        }
    }
}
