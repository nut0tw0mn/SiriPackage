using System.Collections.Generic;
using UnityEngine;

namespace newUI.LearningPath
{
    [System.Serializable]
    public class LevelAudioData
    {
        public List<AudioClip> unitClip = new List<AudioClip>();
    }

    public class LearningPathAsset : ScriptableObject
    {
        //public List<LevelAudioData> levelAudio = new List<LevelAudioData>();
        public List<AudioClip> unitClip = new List<AudioClip>();
    }
}