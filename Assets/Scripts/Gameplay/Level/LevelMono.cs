using UnityEngine;
namespace Gameplay.Level
{
    public class LevelMono : MonoBehaviour
    {
        [SerializeField]
        private EdgeCollider2D spawnEdges;
        [SerializeField]
        private BoxCollider2D gameBorders;
        
        public EdgeCollider2D SpawnEdges => spawnEdges;
        public BoxCollider2D GameBorders => gameBorders;
    }
}
