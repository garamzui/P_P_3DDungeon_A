using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance ;
  
    public LayerMask layerMask;

    public GameObject curInteractableGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    private UIQuickBoard UIQ;
    
    private void Awake()
    {
        UIQ = FindAnyObjectByType<UIQuickBoard>();
    }

    void Start()
    {
        camera = Camera.main;
    }


    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)

        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractableGameObject)
                {
                    curInteractableGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    //프롬포트에 출력해줘라/
                    SetPromptText();
                }
            }
            else
            {
                curInteractableGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }

    }
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        ItemObject Obj;
        bool dataFound;
        if (curInteractableGameObject != null)
        {
            Obj = curInteractableGameObject.GetComponent<ItemObject>();
            dataFound = true;
        }
        else
        {
            return;
        }
       if(dataFound) 
       { if (context.phase == InputActionPhase.Started && curInteractable != null&&Obj.data.type == ItemType.Consumable)
        {
            if (UIQ.IsSlotFull())
            {
                StartCoroutine(SlotFullWarning());
                
                return;
            }
            curInteractable.OnInteract();
            curInteractableGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
        
       }
     
    }
    
    IEnumerator SlotFullWarning()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = "가방이 가득 찼습니다.";
        yield return new WaitForSeconds(1f);
        promptText.gameObject.SetActive(false);
        promptText.text = string.Empty;
    }

    public void CheckDistanceChange(bool view)
    {
        if (view)
            maxCheckDistance *=2;
        else
            maxCheckDistance /= 2;

    }
    
    
}