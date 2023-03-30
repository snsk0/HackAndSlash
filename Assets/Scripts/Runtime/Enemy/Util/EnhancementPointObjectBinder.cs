using UnityEngine;

using UniRx;

using Runtime.Enemy.Component;

using InGame.DropItems;

namespace Runtime.Enemy.Util
{
    public class EnhancementPointObjectBinder : MonoBehaviour
    {
        //�e��}�l�[�W���[
        [SerializeField] private EnemyManager enemyManager;
        private EnhancementPointObjectManager epoManager;



        private void Start()
        {
            enemyManager.onGenerateEventHandler.Subscribe(enemy =>
            {
                EnemyItemDropper dropper = enemy.GetComponent<EnemyItemDropper>();
                dropper.initialize(epoManager.GenerateEnhancementPointObject);
            });
        }

    }
}
