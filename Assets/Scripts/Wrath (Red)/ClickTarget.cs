using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClickTarget : MonoBehaviour, IPointerDownHandler
{

    WrathGameManager gameManager;
    RectTransform rt;
    Image image;
    
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void Init(WrathGameManager manager, Vector2 anchoredPos)
    {
        gameManager = manager;
        rt.anchoredPosition = anchoredPos;
        
        // Debug.Log("Target spawned at: " + anchoredPos);
    }
      

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("Target clicked!");
        gameManager.RegisterClick();

        // visual feedback
        StopAllCoroutines();
        StartCoroutine(PunchScale());
    }

    System.Collections.IEnumerator PunchScale()
    {
        rt.localScale = Vector3.one * 0.9f;
        yield return new WaitForSeconds(0.05f);
        rt.localScale = Vector3.one;
    }
}