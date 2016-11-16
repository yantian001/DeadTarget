using UnityEngine;
using System.Collections;

public class TPSCamera : MonoBehaviour
{

    public Vector3 offsetAnim = new Vector3(.58f, 1.56f, -2.21f);
    public Vector3 offsetNormal = new Vector3(0, 2.57f, -5.7f);
    public float offsetChangeSpeed = 5;
    [HideInInspector]
    public Vector3 targetOffset;
    [HideInInspector]
    public Vector3 currentOffset;

    public Vector3 cameraFocusOffset = Vector3.zero;

    // Default rotation to set on start and on camera reset to player's back
    public float defaultLeftRightRot = 0;
    public float defaultUpDownRot = 10;

    public Transform playerF;

    // Clamping mouse rotations
    public float rotVerticalUpLimit = 30;
    public float rotVerticalDownLimit = 55;
    public float rotVerticalUpLimitAiming = 30;
    public float rotVerticalDownLimitAiming = 55;
    public float rotVerticalUpLimitFpsLook = 30;
    public float rotVerticalDownLimitFpsLook = 55;
    public float rotHorizontalLimitFpsLook = 55;

    public float rotLeft = -55f;
    public float rotRight = 55f;

    // smoothing vars
    public float smoothPosition = 5f;
    public float smoothR = 5f;
    private Vector3 posSmoothVelocity;

    // Right Click Zoom Vars
    public float zoomFov = 24;
    public float zoomSpeedInverse = .15f;
    private float targetFov, defFov;
    private float zoomLevel = 1;    // used to change fov of camera
    private float zoomLevel_V;

    #region General vars
    float rotLeftRight, rotUpDown;
    bool isAim = false;
    private float currentLeftRightRotation;
    private float xSpeed = 25.0f;
    private float ySpeed = 12.0f;

    public Animator anim = null;
    public Quaternion defQ;

    public Quaternion cameraQ;
    private float fovOnSprint;
    #endregion

    public TPSInput tpsInput;
    public PlayerMovement playMove;

    // Use this for initialization
    void Start()
    {
        if(!playerF)
        {
            playerF = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (anim == null)
        {
            anim = playerF.GetComponent<Animator>();
        }
        defQ = playerF.rotation;
        cameraQ = defQ;
        //cameraQ = transform.rotation;
        targetFov = Camera.main.fieldOfView;
        defFov = targetFov;

        if (!tpsInput)
        {
            tpsInput = playerF.GetComponent<TPSInput>();
        }

        if (!playMove)
        {
            playMove = playerF.GetComponent<PlayerMovement>();
        }

    }

    public void LateUpdate()
    {
        isAim = tpsInput.IsAim && (!tpsInput.IsMoveing) && tpsInput.isActiveAndEnabled;

        //anim.SetBool("isAim", isAim);
        GetMouse();
        GetTargetOffsetAndClampedRotation();
        // Clamp UpDown Camera movement
        rotUpDown = !isAim ? ClampRotation(rotUpDown, -rotVerticalUpLimit, rotVerticalDownLimit) : ClampRotation(rotUpDown, -rotVerticalUpLimitAiming, rotVerticalDownLimitAiming);
        rotLeftRight = ClampRotation(rotLeftRight, rotLeft, rotRight);
        // lerp Offset to make sure no sudden changes or flickering
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * offsetChangeSpeed);
        // lerp Left Right Rotation to make sure no sudden changes or flickering
        currentLeftRightRotation = Mathf.Lerp(currentLeftRightRotation, rotLeftRight, Time.deltaTime * 20f);

        Quaternion rotation = Quaternion.Euler(rotUpDown + cameraQ.eulerAngles.x, currentLeftRightRotation + cameraQ.eulerAngles.y, cameraQ.eulerAngles.z);
        Vector3 position = rotation * currentOffset + playerF.position + cameraFocusOffset;


        transform.position = Vector3.SmoothDamp(transform.position, position, ref posSmoothVelocity, smoothPosition * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothR * Time.deltaTime);
        if (isAim)
        {
            playerF.rotation = transform.rotation;
        }
        else
        {
            if(GameValue.staus == GameStatu.InGame)
            {
                playerF.rotation = defQ;
            }
        }
        float zoomFovToSet = zoomFov;
        ChangeFov(ref zoomFovToSet);
    }

    void GetTargetOffsetAndClampedRotation()
    {
        if (isAim)
        {
            targetOffset = offsetAnim;
        }
        else
        {
            targetOffset =  offsetNormal;
        }
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * offsetChangeSpeed);

    }

    private void ChangeFov(ref float zoomFovToSet)
    {
        if (isAim)
        {
            zoomLevel = Mathf.SmoothDamp(zoomLevel, 0, ref zoomLevel_V, zoomSpeedInverse);
            zoomFovToSet = zoomFov;
        }
        else
        {
            zoomLevel = Mathf.SmoothDamp(zoomLevel, 1, ref zoomLevel_V, zoomSpeedInverse);
        }


        Camera.main.fieldOfView = Mathf.Lerp(zoomFovToSet, defFov - fovOnSprint, zoomLevel);

    }

    void GetMouse()
    {
        rotLeftRight += tpsInput.horizontal * 0.1f;
        rotUpDown -= tpsInput.vertical * 0.1f;
    }

    private float ClampRotation(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
