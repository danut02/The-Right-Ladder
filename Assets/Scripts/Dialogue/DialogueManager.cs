using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private Message[] currentMessages;
    private Actor[] currentActors;
    private int activeMessage = 0;
    public static bool isActive = false;
    private bool isCoroutineRunning = false;
    private int selectedMessageIndex;
    public TMP_Text[] options;
    public GameObject messagePanel;
    private int index=0;
    private Color defaultTextColor = Color.red;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        isCoroutineRunning = false;
        Debug.Log("Started Conversation! Loaded Messages: "+ messages.Length);
    }

    public IEnumerator NextMessage()
    {
        isCoroutineRunning = true;
        while (isActive)
        {
            if (currentMessages[activeMessage].message.Count == 1)
            {
                Monologue.instance.DisplayText(
                    currentActors[currentMessages[activeMessage].actorId].name + ": " +
                    currentMessages[activeMessage].message[0],
                    4
                );
            }
            else if (currentMessages[activeMessage].message.Count > 1)
            {
                messagePanel.gameObject.SetActive(true);
                DisplayMessageList(currentMessages[activeMessage].message);
                Debug.Log(currentMessages[activeMessage].message.Count);
                yield return StartCoroutine(WaitForMessageSelection());
                string selectedMessage = GetSelectedMessage();
                Monologue.instance.DisplayText(
                    currentActors[currentMessages[activeMessage].actorId].name + ": " + selectedMessage,
                    4
                );
                messagePanel.gameObject.SetActive(false);
            }
            activeMessage++;
            if (activeMessage >= currentMessages.Length)
            {
                isActive = false;
                activeMessage = 0;
                Debug.Log("Conversation ended!");
                break;
            }
            yield return new WaitForSeconds(4);
        }
        isCoroutineRunning = false;
    }
    IEnumerator WaitForMessageSelection()
    {
        int selectedIndex = 0;
        bool messageSelected = false;
        UpdateMessageSelectionUI(selectedIndex,Color.red);
        while (!messageSelected)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UpdateMessageSelectionUI(selectedIndex,Color.white);
                selectedIndex = Mathf.Max(selectedIndex - 1, 0);
                UpdateMessageSelectionUI(selectedIndex,Color.red);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                UpdateMessageSelectionUI(selectedIndex,Color.white);
                selectedIndex = Mathf.Min(selectedIndex + 1, currentMessages[activeMessage].message.Count - 1);
                UpdateMessageSelectionUI(selectedIndex,Color.red);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                UpdateMessageSelectionUI(selectedIndex,Color.white);
                messageSelected = true;
            }
            yield return null;
        }

        selectedMessageIndex = selectedIndex;
    }
    
    string GetSelectedMessage()
    {
        return currentMessages[activeMessage].message[selectedMessageIndex];
    }
    
    void DisplayMessageList(List<string> activeMessages)
    {
        index = 0;
        for (; index < activeMessages.Count; index++)
        {
            options[index].enabled = true;
            options[index].text = activeMessages[index];
        }
        for (; index < options.Length; index++)
        {
            options[index].enabled = false;
        }
    }
    void UpdateMessageSelectionUI(int selectedIndex,Color color)
    {
        options[selectedIndex].color = color;
    }
    private void Update()
    {
        if (isActive && !isCoroutineRunning) 
        {
            StartCoroutine(NextMessage());
        }
    }

    private void Start()
    {
        messagePanel.gameObject.SetActive(false);
        if (options.Length > 0)
        {
            options[0].color = defaultTextColor;
        }
    }
}
