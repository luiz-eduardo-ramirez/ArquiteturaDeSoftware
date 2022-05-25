using Base.Extensions.Utilites;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASLeitner
{
    public class MagicalRouletteCtrl : MonoBehaviour
    {
        [SerializeField]
        [Header("O X representa velocidade linear")]
        [Header("O Y representa velocidade da interpolacao")]
        private Vector2 m_animationSpeed;
        [SerializeField]
        private float m_rouletteRadius;
        private Flashcard[] m_flashcards;
        private bool m_isRotating;
        // Start is called before the first frame update
        void Start()
        {
            m_isRotating = false;
            GetChildFlashcards();
            AssingFlashcardsPositions();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
                AnimateRight();
        }

        private void AssingFlashcardsPositions()
        {
            float angleFlashcards = 360 / m_flashcards.Length;

            Vector2 flashCardPos = new Vector2(0, -m_rouletteRadius);

            for (int i = 0; i < m_flashcards.Length; i++)
            {
                m_flashcards[i].transform.position = new Vector3(flashCardPos.x, 0, flashCardPos.y) + transform.position;
                flashCardPos = Vec2Uts.RotateVec2(flashCardPos, angleFlashcards);
            }
        }

        private void GetChildFlashcards()
        {
            int childCount = transform.childCount;
            m_flashcards = new Flashcard[childCount];

            for (int i = 0; i < childCount; i++)
            {
                m_flashcards[i] = transform.GetChild(i).GetComponent<Flashcard>();
            }
        }
        private IEnumerator Animate(float _rotationAmount)
        {
            m_isRotating = true;
            Vector3 currentRotation = transform.rotation.eulerAngles;
            Vector3 desiredRotation = currentRotation; 

            desiredRotation.y += _rotationAmount;

            while (currentRotation.y != desiredRotation.y)
            {
                currentRotation = Vec3Uts.LerpAndMoveTo(currentRotation, desiredRotation, m_animationSpeed.x, m_animationSpeed.y);
                transform.rotation = Quaternion.Euler(currentRotation);
                yield return null;
            }

            m_isRotating = false;
        }
        private void AnimateLeft()
        {
            float angleFlashcards = 360 / m_flashcards.Length;
            if (!m_isRotating)
            {
                StartCoroutine(Animate(angleFlashcards));
            }
        }
        private void AnimateRight()
        {
            float angleFlashcards = 360 / m_flashcards.Length;
            if (!m_isRotating)
            {
                StartCoroutine(Animate(-angleFlashcards));
            }
        }
    }

}