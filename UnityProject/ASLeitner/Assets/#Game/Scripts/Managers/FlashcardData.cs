using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASLeitner.DataStructs
{
    public enum LearningStages { Ignorant, Superficial, Acquired }
    [Serializable]
    public struct FlashcardData
    {
        public string CardFront;
        public string CardBack;
        public LearningStages LearningStage;
    }
}
