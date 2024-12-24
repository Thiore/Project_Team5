using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, ITouchable
{
    [SerializeField] private Transform pivot;
    private Transform playerCam;
    private MeshRenderer render;
    [SerializeField] private Item3D clueBook;
    [Range(10f, 50f)]
    [SerializeField] private float enabledSpeed;

    private bool isGet;
    private bool isOn;

    private float rotTime;
    private Coroutine rotBook_co = null;

    private BoxCollider col;
    private Outline outline;
    private void Awake()
    {
        TryGetComponent(out render);
        TryGetComponent(out col);
        TryGetComponent(out outline);
        outline.enabled = false;
    }

    private void Start()
    {
        this.playerCam = PlayerManager.Instance.playerCam;
        rotTime = 1f;
        isOn = true;

        if (DataSaveManager.Instance.GetItemState(clueBook.ID))
        {
            col.enabled = false;
            render.enabled = false;

            isGet = true;
        }
        else
        {
            isGet = false;
        }
        if (isGet)
        {
            transform.SetParent(pivot);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one * 2f;
            OnUseTrigger(clueBook.item);
            SetUseBook();
        }
    }

    private void OnDestroy()
    {
        if (isGet)
        {
            TriggerButton.OnUseTrigger -= OnUseTrigger;
        }

    }

    private IEnumerator GetBook_co()
    {
        transform.SetParent(pivot);
        float delayTime = 0;
        while (true)
        {
            delayTime += Time.fixedDeltaTime;
            transform.localPosition = Vector3.LerpUnclamped(transform.localPosition, Vector3.zero, delayTime);
            transform.localRotation = Quaternion.SlerpUnclamped(transform.localRotation, Quaternion.identity, delayTime);
            transform.localScale = Vector3.LerpUnclamped(transform.localScale, Vector3.one * 2f, delayTime);
            if (delayTime >= 1f)
            {
                SetUseBook();
                yield break;
            }
            yield return null;
        }

    }

    private IEnumerator RotateTablet_co(float dir)
    {

        while (true)
        {
            rotTime += dir * Time.fixedDeltaTime * 0.5f;
            if (rotTime <= 0f)
            {
                render.enabled = false;
                isOn = false;
                rotBook_co = null;
                yield break;
            }
            if (rotTime >= 1f)
            {

                isOn = true;
                rotBook_co = null;
                yield break;
            }
            transform.RotateAround(playerCam.transform.position, playerCam.transform.right, -dir * Time.fixedDeltaTime * enabledSpeed);


            yield return null;
        }
    }

    public void OnUseTrigger(Item item)
    {
        if (item.id.Equals(clueBook.ID))
        {
            if (isOn)
            {
                if (rotBook_co != null)
                    StopCoroutine(rotBook_co);

                rotBook_co = StartCoroutine(RotateTablet_co(-1f));
            }
            else
            {
                if (rotBook_co != null)
                    StopCoroutine(rotBook_co);
                render.enabled = true;
                StartCoroutine(RotateTablet_co(1f));
            }
        }
    }
    public void SetUseBook()
    {
        TriggerButton.OnUseTrigger += OnUseTrigger;
    }

    public void OnTouchStarted(Vector2 position)
    {
    }

    public void OnTouchHold(Vector2 position)
    {
    }

    public void OnTouchEnd(Vector2 position)
    {
        if (!isGet)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    clueBook.GetItem(false);
                    StartCoroutine(GetBook_co());
                    isGet = true;
                    col.enabled = false;
                    outline.enabled = false;
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = false;
        }
    }
}
