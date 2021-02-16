using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxManager : MonoBehaviour
{
    [SerializeField] RectTransform conversationBox;
    [SerializeField] RectTransform startPos;
    [SerializeField] RectTransform destinationPos;

    [SerializeField] float slideDuration = 0.5f;

    public RectTransform ConversationBox { get => conversationBox; }

    private void Awake()
    {
        conversationBox.localPosition = startPos.localPosition;
        conversationBox.gameObject.SetActive(false);
    }

    public bool slidingChatbox;
    public IEnumerator SlideChatBox(bool slideIn)
    {
        slidingChatbox = true;
        conversationBox.gameObject.SetActive(true);

        float timer = 0f;
        Vector3 start;
        Vector3 destination;

        if(slideIn)
        {
            start = startPos.localPosition;
            destination = destinationPos.localPosition;
        }
        else
        {
            start = destinationPos.localPosition;
            destination = startPos.localPosition;
        }

        while (conversationBox.localPosition != destination)
        {
            conversationBox.localPosition = Vector3.Lerp(start, destination, (timer / slideDuration));
            timer += Time.deltaTime;

            yield return null;
        }
        if(!slideIn) 
            conversationBox.gameObject.SetActive(false);

        slidingChatbox = false;
    }
}
