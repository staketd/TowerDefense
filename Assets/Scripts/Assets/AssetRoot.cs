using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Resources {
    [CreateAssetMenu(menuName = "Assets/Asset root", fileName = "Asset Root")]
    public class AssetRoot : ScriptableObject {
        public List<LevelAsset> Levels;
        
    }
}