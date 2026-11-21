using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MascotController : MonoBehaviour
{
    [SerializeField] private RectTransform BodyGeneral;
    [SerializeField] private RectTransform pivotHead;
    [SerializeField] private RectTransform pivotLeftArm;
    [SerializeField] private RectTransform pivotRightArm;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float bodyMovement = 2f;
    [SerializeField] private float headTilt = 3f;
    [SerializeField] private float armMovement = 2f;

    private Vector2 BodyStartPos;

    private Quaternion StarterHeadRot;
    private Quaternion StarterLeftArmRot;
    private Quaternion StarterRightArmRot;

    private void Start()
    {
        BodyStartPos = BodyGeneral.anchoredPosition;

        StarterHeadRot = pivotHead.localRotation;
        StarterLeftArmRot = pivotLeftArm.localRotation;
        StarterRightArmRot = pivotRightArm.localRotation;
    }

    private void Update()
    {
        float timer = Time.time * speed;

        float moveY = Mathf.Sin(timer) * bodyMovement;

        BodyGeneral.anchoredPosition =
            BodyStartPos + Vector2.up * moveY;

        float headRot =
            Mathf.Sin(timer * 0.7f) * headTilt;

        pivotHead.localRotation =
            StarterHeadRot *
            Quaternion.Euler(0f, 0f, headRot);

        float armsRot =
            Mathf.Sin(timer) * armMovement;

        pivotLeftArm.localRotation =
            StarterLeftArmRot *
            Quaternion.Euler(0f, 0f, -armsRot);

        pivotRightArm.localRotation =
            StarterRightArmRot *
            Quaternion.Euler(0f, 0f, armsRot);
    }
}