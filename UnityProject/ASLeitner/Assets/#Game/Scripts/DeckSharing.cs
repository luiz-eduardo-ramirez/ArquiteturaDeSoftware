using ASLeitner.Managers;
using Base.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ASLeitner
{
    public class DeckSharing : MonoBehaviour
    {
        [SerializeField] private GameObject m_sharingMenu;
        [SerializeField] private GameObject m_mainMenu;
        [SerializeField] private GameObject m_onDeckSharingFail;
        [SerializeField] private GameObject m_onDeckSharingSuccess;
        [SerializeField] private GameObject m_inputAbsorver;
        [SerializeField] private TMP_InputField m_usrIdInputfield;

        private void Start()
        {
            m_onDeckSharingFail.SetActive(false);
            m_onDeckSharingSuccess.SetActive(false);
            m_inputAbsorver.SetActive(false);
        }
        private void DeckMergeResultCB(bool _successful)
        {
            if (_successful)
            {
                m_onDeckSharingFail.SetActive(false);
                m_onDeckSharingSuccess.SetActive(true);
                TimersManager.CallAfterTime(DeactivateSharingMenu, 3);
            }
            else
            {
                m_onDeckSharingFail.SetActive(true);
                m_inputAbsorver.SetActive(false);
            }
        }
        public void OnUsridInputed()
        {
            string usrID = m_usrIdInputfield.text;
            usrID = usrID.Replace("\r\n", string.Empty);

            m_inputAbsorver.SetActive(true);
            PlayerDataManager.Instance.MergeDecks(usrID, DeckMergeResultCB);
        }
        public void ActivateSharingMenu()
        {
            m_sharingMenu.SetActive(true);
            m_mainMenu.SetActive(false);
            m_onDeckSharingFail.SetActive(false);
            m_onDeckSharingSuccess.SetActive(false);
            m_inputAbsorver.SetActive(false);
        }
        public void DeactivateSharingMenu()
        {
            m_sharingMenu.SetActive(false);
            m_onDeckSharingFail.SetActive(false);
            m_onDeckSharingSuccess.SetActive(false);
            m_mainMenu.SetActive(true);
            m_inputAbsorver.SetActive(false);

        }
    }
}
