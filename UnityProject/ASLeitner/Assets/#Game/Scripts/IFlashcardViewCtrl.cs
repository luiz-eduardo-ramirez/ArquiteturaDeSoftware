using ASLeitner.DataStructs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASLeitner
{
    public interface IFlashcardViewCtrl
    {
        List<FlashcardData> GetViewFlashCards();
        void SetHighlightedFlashcard(Flashcard _flashcard);

    }
}
