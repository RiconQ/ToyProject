using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpWindow : MonoBehaviour
{
    /*Todo
        버튼 누를시 HelpWindow활성화, HelpButton비활성화
        HelpWindow안에는 게임 방법과 캐릭터 설명이 있음
        CloseButton 누를시 HelpWindow 비활성화ㅏ
    */

    [SerializeField] private Button helpButton;
    [SerializeField] private GameObject helpWindowPanel;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        helpWindowPanel.SetActive(false);
    }

    public void ToggleHelpWindow()
    {
        helpWindowPanel.SetActive(!helpWindowPanel.activeSelf);
        helpButton.gameObject.SetActive(!helpWindowPanel.activeSelf);
    }
}
