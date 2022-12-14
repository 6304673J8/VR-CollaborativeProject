using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
public class HapticUI : MonoBehaviour
{
    private ActionBasedController controller1;
    private ActionBasedController controller2;
    private XRRayInteractor interactor1;
    private XRRayInteractor interactor2;
    private bool interactor1Active = false;
    private bool interactor2Active = false;

    private void Awake()
    {
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Button>().Select(x => x.gameObject));
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Slider>().Select(x => x.gameObject));
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Dropdown>().Select(x => x.gameObject));
        foreach (var item in gameObjects)
        {
            var trigger = item.AddComponent<EventTrigger>();
            var e = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            e.callback.AddListener(Hover);
            trigger.triggers.Add(e);

            var e2 = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            e2.callback.AddListener(Exit);
            trigger.triggers.Add(e2);
        }
    }

    private void GetControllers()
    {
        if (controller1 == null || controller2 == null)
        {
            var controllers = FindObjectsOfType<ActionBasedController>();
            if (controllers.Length > 0)
            {
                controller1 = controllers[0];
                interactor1 = controller1.gameObject.GetComponent<XRRayInteractor>();
            }
            if (controllers.Length > 1)
            {
                controller2 = controllers[1];
                interactor2 = controller2.gameObject.GetComponent<XRRayInteractor>();
            }
        }
    }
    private void Hover(BaseEventData arg0)
    {
        StartCoroutine(OnHoverDelayed());
    }

    private IEnumerator OnHoverDelayed()
    {
        yield return new WaitForSeconds(0.02f);

        GetControllers();
        if (interactor1.TryGetHitInfo(out Vector3 v1, out Vector3 v2, out int i1, out bool isValid) && isValid && !interactor1Active)
        {
            interactor1Active = true;
            controller1.SendHapticImpulse(interactor1.hapticHoverEnterIntensity, interactor1.hapticHoverEnterDuration);
            controller1.SendHapticImpulse(interactor1.hapticHoverEnterIntensity, interactor1.hapticHoverEnterDuration);
            interactor1.playAudioClipOnHoverEntered = true;
        }
        if (interactor2.TryGetHitInfo(out Vector3 v3, out Vector3 v4, out int i2, out bool isValid2) && isValid2 && !interactor2Active)
        {
            interactor2Active = true;
            controller2.SendHapticImpulse(interactor2.hapticHoverEnterIntensity, interactor2.hapticHoverEnterDuration);
            interactor2.playAudioClipOnHoverEntered = true;
        }
    }

    private void Exit(BaseEventData arg0)
    {
        StartCoroutine(OnExitDelayed());
    }

    private IEnumerator OnExitDelayed()
    {
        yield return new WaitForSeconds(0.02f);

        GetControllers();
        if (!interactor1.TryGetHitInfo(out Vector3 v1, out Vector3 v2, out int i1, out bool isValid) || !isValid)
        {
            interactor1Active = false;
        }
        if (!interactor2.TryGetHitInfo(out Vector3 v3, out Vector3 v4, out int i2, out bool isValid2) || !isValid2)
        {
            interactor2Active = false;
        }
    }
}