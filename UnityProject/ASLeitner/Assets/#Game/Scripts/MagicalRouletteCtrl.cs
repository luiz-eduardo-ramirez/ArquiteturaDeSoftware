using ASLeitner.DataStructs;
using ASLeitner.Managers;
using Base.Extensions.Utilites;
using Base.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASLeitner
{
    public class MagicalRouletteCtrl : MonoBehaviour
    {
        [SerializeField]
        private Flashcard m_flashcardPrefab;
        [SerializeField]
        [Header("O X representa velocidade linear")]
        [Header("O Y representa velocidade da interpolacao")]
        private Vector2 m_rotationAnimationSpeed;
        [SerializeField]
        private float m_distanceFromCam;
        [SerializeField]
        private float m_flashcardsSpacing;

        [Space]
        [Header("O X representa velocidade linear")]
        [Header("O Y representa velocidade da interpolacao")]
        [SerializeField]
        private Vector2 m_translationAnimationSpeed;
        [SerializeField]
        private float m_flashCardExitHeight;

        private List<Flashcard> m_flashcards;
        private bool m_isAnimating;
        private int m_highlightedFlashcardIndex;
        public Flashcard HighlitedFlashcard { get => m_flashcards[m_highlightedFlashcardIndex]; }
        public bool IsAnimating { get => m_isAnimating; }
        // Start is called before the first frame update
        void Awake()
        {
            m_isAnimating = false;
            m_highlightedFlashcardIndex = -1;
        }
        /// <summary>
        /// Incrementa o indice e retorna a distancia para o proximo flashcard
        /// </summary>
        /// <returns>retorna a distancia de indices para o proximo flashcard</returns>
        private int IncrementFlashcardIndex()
        {
            int i = 0;
            for (; i < m_flashcards.Count; i++)
            {
                m_highlightedFlashcardIndex = (m_highlightedFlashcardIndex + 1) % m_flashcards.Count;
                if (HighlitedFlashcard.isActiveAndEnabled) break;
            }

            return i + 1;
        }
        /// <summary>
        /// Decrementa o indice e retorna a distancia para o proximo flashcard
        /// </summary>
        /// <returns>retorna a distancia de indices para o proximo flashcard</returns>
        private int DecrementFlashcardIndex()
        {
            int i = 0;
            for (; i < m_flashcards.Count; i++)
            {
                m_highlightedFlashcardIndex--;
                if (m_highlightedFlashcardIndex < 0) m_highlightedFlashcardIndex = m_flashcards.Count - 1;
                if (HighlitedFlashcard.isActiveAndEnabled) break;
            }
            return i + 1;
        }
        private float GetAngleBetweenFlashcards() { return m_flashcards.Count > 1 ? 360f / m_flashcards.Count : 0; }
        private void AssingFlashcardsPositions()
        {
            float angleFlashcards = GetAngleBetweenFlashcards();
            float c = m_flashcardsSpacing;
            float m_rouletteRadius;

            if (angleFlashcards == 0) m_rouletteRadius = 0;
            else m_rouletteRadius = Mathf.Sqrt(Mathf.Pow(c, 2) / (2 - (2* Mathf.Cos(Mathf.Deg2Rad * angleFlashcards))));            

            transform.position = Vector3.forward * (m_rouletteRadius + m_distanceFromCam);

            Vector2 flashCardPos = new Vector2(0, -m_rouletteRadius);

            for (int i = 0; i < m_flashcards.Count; i++)
            {
                m_flashcards[i].transform.position = new Vector3(flashCardPos.x, 0, flashCardPos.y) + transform.position;
                flashCardPos = Vec2Uts.RotateVec2(flashCardPos, angleFlashcards);
            }
        }
        private IEnumerator AnimateRoulette(bool _rotatingRight)
        {
            float angleFlashcards = GetAngleBetweenFlashcards();
            Vector3 currentRotation = transform.rotation.eulerAngles;
            Vector3 desiredRotation = currentRotation;
            int indexDiff;

            if (_rotatingRight)
                indexDiff = IncrementFlashcardIndex();
            else
                indexDiff = DecrementFlashcardIndex();

            if (!_rotatingRight) angleFlashcards = -angleFlashcards;

            desiredRotation.y += angleFlashcards * indexDiff;

            while (currentRotation.y != desiredRotation.y)
            {
                currentRotation = Vec3Uts.LerpAndMoveTo(currentRotation, desiredRotation, m_rotationAnimationSpeed.x, m_rotationAnimationSpeed.y, Time.deltaTime);
                transform.rotation = Quaternion.Euler(currentRotation);
                yield return null;
            }


            m_isAnimating = false;
        }
        private IEnumerator AnimateFlashcardRotation()
        {
            HighlitedFlashcard.LookForward = false;
            Vector3 currentRotation = HighlitedFlashcard.transform.rotation.eulerAngles;
            Vector3 desiredRotation = currentRotation;

            desiredRotation.y += 180;

            while (currentRotation.y != desiredRotation.y)
            {
                currentRotation = Vec3Uts.LerpAndMoveTo(currentRotation, desiredRotation, m_rotationAnimationSpeed.x, m_rotationAnimationSpeed.y, Time.deltaTime);
                HighlitedFlashcard.transform.rotation = Quaternion.Euler(currentRotation);
                yield return null;
            }

            HighlitedFlashcard.ShowingTerm = !HighlitedFlashcard.ShowingTerm;
            HighlitedFlashcard.LookForward = true;

            m_isAnimating = false;
        }
        private IEnumerator AnimateFlashcardRemoved()
        {
            Vector3 currentPos = HighlitedFlashcard.transform.position;
            Vector3 desiredPos = HighlitedFlashcard.transform.position + new Vector3(0, m_flashCardExitHeight, 0);

            while (currentPos.y != desiredPos.y)
            {
                currentPos = Vec3Uts.LerpAndMoveTo(currentPos, desiredPos, m_translationAnimationSpeed.x, m_translationAnimationSpeed.y, Time.deltaTime);
                HighlitedFlashcard.transform.position = currentPos;
                yield return null;
            }

            HighlitedFlashcard.gameObject.SetActive(false);

            if (m_flashcards.Count > 0)
            {
                StartCoroutine(AnimateRoulette(true));
            }
            else
                m_isAnimating = false;
        }

        private IEnumerator AnimateFlashcardInserted()
        {
            Flashcard newFlashcard = Instantiate(m_flashcardPrefab, transform);
            newFlashcard.LookForward = true;
            newFlashcard.SetFlashCard(new FlashcardData(PlayerDataManager.Instance.GetAvailableDefaultKey(), "Definicao", LearningStages.Ignorant));
            m_flashcards.Insert(m_highlightedFlashcardIndex, newFlashcard);

            transform.rotation = Quaternion.Euler(Vector3.zero);

            AssingFlashcardsPositions();

            float angleFlashcards = GetAngleBetweenFlashcards();
            transform.rotation = Quaternion.Euler(0, angleFlashcards * m_highlightedFlashcardIndex, 0);
            newFlashcard.transform.position = newFlashcard.transform.position + (Vector3.up * m_flashCardExitHeight);

            Vector3 currentPos = newFlashcard.transform.position;
            Vector3 desiredPos = newFlashcard.transform.position;
            desiredPos.y = 0;

            while (currentPos.y != desiredPos.y)
            {
                currentPos = Vec3Uts.LerpAndMoveTo(currentPos, desiredPos, m_translationAnimationSpeed.x, m_translationAnimationSpeed.y, Time.deltaTime);
                HighlitedFlashcard.transform.position = currentPos;
                yield return null;
            }
            
            m_isAnimating = false;
        }
        public void InsertNewFlashcard()
        {
            if (!m_isAnimating)
            {
                m_isAnimating = true;
                StartCoroutine(AnimateFlashcardInserted());
            }
        }

        public void InstantiateFlashcards(FlashcardData[] _flashcards)
        {
            Flashcard newFlashcard;
            int childCount = _flashcards.Length;

            m_flashcards = new List<Flashcard>();
            m_highlightedFlashcardIndex = 0;
            for (int i = 0; i < childCount; i++)
            {
                newFlashcard = Instantiate(m_flashcardPrefab, transform);
                newFlashcard.LookForward = true;
                newFlashcard.SetFlashCard(_flashcards[i]);
                m_flashcards.Add(newFlashcard);
            }
            if (_flashcards.Length > 0) AssingFlashcardsPositions();
        }
        public void AnimateLeft()
        {
            if (!m_isAnimating && m_flashcards.Count > 0)
            {
                m_isAnimating = true;
                if (!HighlitedFlashcard.ShowingTerm)
                {
                    StartCoroutine(AnimateFlashcardRotation());
                    TimersManager.CallAfterConditionIsTrue(() => StartCoroutine(AnimateRoulette(false)), () => (HighlitedFlashcard.ShowingTerm && !m_isAnimating));
                }
                else
                    StartCoroutine(AnimateRoulette(false));
            }
        }
        public void AnimateRight()
        {
            if (!m_isAnimating && m_flashcards.Count > 0)
            {
                m_isAnimating = true;
                if (!HighlitedFlashcard.ShowingTerm)
                {
                    StartCoroutine(AnimateFlashcardRotation());
                    TimersManager.CallAfterConditionIsTrue(() => StartCoroutine(AnimateRoulette(true)), () => (HighlitedFlashcard.ShowingTerm && !m_isAnimating));
                }
                else
                    StartCoroutine(AnimateRoulette(true));
            }
        }
        public void RemoveHighlightedFlashcard()
        {
            if (!m_isAnimating && m_flashcards.Count > 0)
            {
                m_isAnimating = true;
                StartCoroutine(AnimateFlashcardRemoved());
            }
        }
        public void RotateCurrentFlashcard()
        {
            if (!m_isAnimating && m_flashcards.Count > 0)
            {
                m_isAnimating = true;
                StartCoroutine(AnimateFlashcardRotation());
            }
        }
    }

}