using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSwiper : MonoBehaviour
{
    public RectTransform cardHolder; // Parent container for cards
    public float transitionSpeed = 10f;
    public float swipeThreshold = 50f;
    public float tapMovementThreshold = 10f; // Distance in pixels considered a "tap"
    public float closeBufferTime = 0.3f; // Buffer time before close is allowed

    private List<GameObject> cards = new List<GameObject>();
    private int currentIndex = 0;
    private float cardWidth;
    private Vector3 targetPosition;
    public RectTransform darkenPanel;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool isSwiping = false;

    private float activeTime = 0f; // Time since swiper became active

    public void ClearCards()
    {
        int ind = 0;
        foreach (Transform child in cardHolder)
        {
            ind++;
            Destroy(child.gameObject);
        }
        cards.Clear();
        //Debug.Log(cards.Count);
        //Debug.Log(ind);
    }

    public void SetCards(List<GameObject> infoPages, List<GameObject> Cards, int currentCardNum = 0)
    {
        cardHolder.gameObject.SetActive(true);
        ClearCards();

        for (int i = 0; i < infoPages.Count; i++)
        {
            GameObject InfoPage = Instantiate(infoPages[i], cardHolder);
            GameObject card = Cards[i];
            InfoPage.GetComponent<InfoPageScript>().writeinfo(card);
            RectTransform rt = InfoPage.GetComponent<RectTransform>();
            cardWidth = rt.rect.width * 1.25f;
            rt.anchoredPosition = new Vector2(i * cardWidth, 0);
            cards.Add(InfoPage);
        }

        currentIndex = Mathf.Clamp(currentCardNum, 0, cards.Count - 1); // <-- this line sets initial index safely
        UpdateTargetPosition();
        cardHolder.anchoredPosition = targetPosition;
        GameManager.Instance.DisplayingInfo = true;
        activeTime = 0f;
    }

    void Darkening()
    {
        if (GameManager.Instance.DisplayingInfo)
        {
            darkenPanel.gameObject.SetActive(true);
        }
        else
        {
            darkenPanel.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Darkening();
        if (!GameManager.Instance.DisplayingInfo) return;
        activeTime += Time.deltaTime;
        HandleSwipe();
        cardHolder.anchoredPosition = Vector3.Lerp(cardHolder.anchoredPosition, targetPosition, Time.deltaTime * transitionSpeed);

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            TryCloseSwiperOnTap(startTouchPos, endTouchPos);
        }
#else
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPos = touch.position;
                TryCloseSwiperOnTap(startTouchPos, endTouchPos);

                float deltaX = endTouchPos.x - startTouchPos.x;
                if (Mathf.Abs(deltaX) > swipeThreshold)
                {
                    if (deltaX < 0) NextCard();
                    else PrevCard();
                }

                isSwiping = false;
            }
        }
#endif
    }

    void HandleSwipe()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            endTouchPos = Input.mousePosition;
            float deltaX = endTouchPos.x - startTouchPos.x;

            if (Mathf.Abs(deltaX) > swipeThreshold)
            {
                if (deltaX < 0) NextCard();
                else PrevCard();
            }

            isSwiping = false;
        }
#endif
    }

    void TryCloseSwiperOnTap(Vector2 startPos, Vector2 endPos)
    {
        if (activeTime < closeBufferTime) return;

        float distance = Vector2.Distance(startPos, endPos);

        if (distance < tapMovementThreshold && !IsPointerOverCard())
        {
            cardHolder.gameObject.SetActive(false);
            GameManager.Instance.DisplayingInfo = false;
            //Debug.Log("Swiper closed from tap outside cards.");
        }
    }

    void NextCard()
    {
        if (currentIndex < cards.Count - 1)
        {
            currentIndex++;
            UpdateTargetPosition();
        }
    }

    void PrevCard()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateTargetPosition();
        }
    }

    void UpdateTargetPosition()
    {
        targetPosition = -Vector3.right * currentIndex * cardWidth * 1.442f;
    }

    bool IsPointerOverCard()
    {
        foreach (var card in cards)
        {
            RectTransform rect = card.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition, null))
            {
                return true;
            }
        }
        return false;
    }
}
