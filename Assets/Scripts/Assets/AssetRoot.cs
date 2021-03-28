using System.Collections.Generic;
using UnityEngine;

namespace Assets {
    [CreateAssetMenu(menuName = "Assets/Asset root", fileName = "Asset Root")]
    public class AssetRoot : ScriptableObject {
        public List<LevelAsset> Levels;
        
    }
}