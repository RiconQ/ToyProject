using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpWindow : MonoBehaviour
{
    /*Todo
        ��ư ������ HelpWindowȰ��ȭ, HelpButton��Ȱ��ȭ
        HelpWindow�ȿ��� ���� ����� ĳ���� ������ ����
        CloseButton ������ HelpWindow ��Ȱ��ȭ��
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
