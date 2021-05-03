using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets {
    [CreateAssetMenu(menuName = "Assets/Asset root", fileName = "Asset Root")]
    public class AssetRoot : ScriptableObject {
        public List<LevelAsset> Levels;
        public SceneAsset UIScene;

    }
}