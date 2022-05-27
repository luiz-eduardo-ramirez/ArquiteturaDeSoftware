using ASLeitner.DataStructs;
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
        private Vector2 m_deletetionAnimationSpeed;
        [SerializeField]
        private float m_flashCardExitHeight;

        private List<Flashcard> m_flashcards;
        private bool m_isAnimating;
        private int m_highlightedFlashcardIndex;
        public Flashcard HighlitedFlashcard { get => m_flashcards[m_highlightedFlashcardIndex]; }
        // Start is called before the first frame update
        void Awake()
        {
            m_isAnimating = false;
            m_highlightedFlashcardIndex = -1;
        }
        private void IncrementFlashcardIndex()
        {
            m_highlightedFlashcardIndex = (m_highlightedFlashcardIndex + 1) % m_flashcards.Count;
        }
        private void DecrementFlashcardIndex()
        {
            m_highlightedFlashcardIndex--;
            if (m_highlightedFlashcardIndex < 0) m_highlightedFlashcardIndex = m_flashcards.Count - 1;
        }
        private void AssingFlashcardsPositions()
        {
            float angleFlashcards = 360 / m_flashcards.Count;
            float c = m_flashcardsSpacing;
            float m_rouletteRadius = Mathf.Sqrt(Mathf.Pow(c, 2) / (2 - (2* Mathf.Cos(Mathf.Deg2Rad * angleFlashcards))));
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
            float angleFlashcards = 360 / m_flashcards.Count;
            m_isAnimating = true;
            Vector3 currentRotation = transform.rotation.eulerAngles;
            Vector3 desiredRotation = currentRotation;

            if (_rotatingRight) angleFlashcards = -angleFlashcards;

            desiredRotation.y += angleFlashcards;

            while (currentRotation.y != desiredRotation.y)
            {
                currentRotation = Vec3Uts.LerpAndMoveTo(currentRotation, desiredRotation, m_rotationAnimationSpeed.x, m_rotationAnimationSpeed.y, Time.deltaTime);
                transform.rotation = Quaternion.Euler(currentRotation);
                yield return null;
            }

            if (_rotatingRight)
                DecrementFlashcardIndex();
            else
                IncrementFlashcardIndex();


            m_isAnimating = false;
        }
        private IEnumerator AnimateFlashcardRotation()
        {
            m_isAnimating = true;
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
            m_isAnimating = true;
            Vector3 currentPos = HighlitedFlashcard.transform.position;
            Vector3 desiredPos = HighlitedFlashcard.transform.position + new Vector3(0, m_flashCardExitHeight, 0);

            while (currentPos.y != desiredPos.y)
            {
                currentPos = Vec3Uts.LerpAndMoveTo(currentPos, desiredPos, m_deletetionAnimationSpeed.x, m_deletetionAnimationSpeed.y, Time.deltaTime);
                HighlitedFlashcard.transform.position = currentPos;
                yield return null;
            }

            HighlitedFlashcard.gameObject.SetActive(false);
            m_flashcards.Remove(HighlitedFlashcard);

            if (m_flashcards.Count > 0)
            {                
                IncrementFlashcardIndex();
            }

            m_isAnimating = false;

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
            AssingFlashcardsPositions();
        }
        public void AnimateLeft()
        {
            if (!m_isAnimating)
            {
                if (!HighlitedFlashcard.ShowingTerm)
                {
                    StartCoroutine(AnimateFlashcardRotation());
                    TimersManager.CallAfterConditionIsTrue(() => StartCoroutine(AnimateRoulette(true)), () => (HighlitedFlashcard.ShowingTerm && !m_isAnimating));
                }
                else
                    StartCoroutine(AnimateRoulette(true));
            }
        }
        public void AnimateRight()
        {
            if (!m_isAnimating)
            {
                if (!HighlitedFlashcard.ShowingTerm)
                {
                    StartCoroutine(AnimateFlashcardRotation());
                    TimersManager.CallAfterConditionIsTrue(() => StartCoroutine(AnimateRoulette(false)), () => (HighlitedFlashcard.ShowingTerm && !m_isAnimating));
                }
                else
                    StartCoroutine(AnimateRoulette(false));
            }
        }
        public void RemoveHighlightedFlashcard()
        {
            if (!m_isAnimating)
            {
                StartCoroutine(AnimateFlashcardRemoved());
            }
        }
        public void RotateCurrentFlashcard()
        {
            if (!m_isAnimating)
            {
                StartCoroutine(AnimateFlashcardRotation());
            }
        }
    }

}