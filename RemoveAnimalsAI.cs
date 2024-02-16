using Rust.Ai;

namespace Oxide.Plugins
{
    [Info("Remove Animals AI", "Orange", "1.0.3")]
    [Description("Removing AI only for animals (not for bots)")]
    public class RemoveAnimalsAI : RustPlugin
    {
        #region Oxide Hooks

        private void Init()
        {
            Unsubscribe(nameof(OnEntitySpawned));
        }
        
        private void OnServerInitialized()
        {
            Subscribe(nameof(OnEntitySpawned));
            CheckAnimals();
        }

        private void OnEntitySpawned(BaseNpc entity)
        {
            timer.Once(1f, () => { RemoveMovement(entity);});
        }

        #endregion

        #region Core

        private void CheckAnimals()
        {
            foreach (var entity in UnityEngine.Object.FindObjectsOfType<BaseNpc>())
            {
                RemoveMovement(entity);
            }
        }

        private void RemoveMovement(BaseNpc npc)
        {
            if (npc == null)
            {
                return;
            }
            
            npc.CancelInvoke(npc.TickAi);
            var script1 = npc.GetComponent<AiManagedAgent>();
            UnityEngine.Object.Destroy(script1);
            var script2 = npc.GetComponent<AnimalBrain>();
            UnityEngine.Object.Destroy(script2);
            var script3 = npc.GetComponent<NPCNavigator>();
            UnityEngine.Object.Destroy(script3);

            var obj = npc as BaseAnimalNPC;
            if (obj != null)
            {
                AIThinkManager.RemoveAnimal(obj);
            }
            
            var obj2 = npc as BaseFishNPC;
            if (obj2 != null)
            {
                AIThinkManager.RemoveAnimal(obj2);
            }
        }

        #endregion
    }
}