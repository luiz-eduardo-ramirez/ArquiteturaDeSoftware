using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASLeitner.DataStructs
{
    [Serializable]
    public class DeckData
    {
        public FlashcardData[] FlashCards;
        public int DeckSize { get => FlashCards.Length; }
    }
}
