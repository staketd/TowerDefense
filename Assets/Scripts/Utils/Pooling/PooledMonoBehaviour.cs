using UnityEngine;

namespace Utils.Pooling {
    public class PooledMonoBehaviour : MonoBehaviour {
        private int m_PrefabId;

        public int PrefabId => m_PrefabId;

        public virtual void AwakePooled() {
            
        }

        public void SetPrefabId(int id) {
            m_PrefabId = id;
        }
    }
}