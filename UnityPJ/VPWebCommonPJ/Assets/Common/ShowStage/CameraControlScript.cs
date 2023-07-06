using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class CameraControlScript : MonoBehaviour
{
    /** 主要的摄像机 */
    public Camera mainCamera;

    /** 摄像机直接观察的对象 */
    public GameObject lookTarget;

    /** 控制摄像机水平旋转(绕Y轴旋转) */
    public GameObject cameraRotationH;

    /** 控制摄像机垂直方向旋转(绕Z轴旋转)(范围是0~60度) */
    public GameObject cameraRotationV;

    private Boolean isDownMouse = false;

    private Vector3 downMousePos = Vector3.zero;

    /** 按下时的水平旋转角度 */
    private Quaternion downHRotation = Quaternion.identity;

    /** 按下时的垂直旋转角度 */
    private Quaternion downVRotation = Quaternion.identity;
    
    /** 最后一帧鼠标的位置,用于当前帧的鼠标位置进行相减得出惯性方向和惯性大小 */
    private Vector3 lastFramePosition = Vector3.zero;

    /** 上一次记录position时的时间 */
    private float lastFramePositionTime = 0.0f;

    /** 
    * 阻尼，越大停得越快
    * 0~1之间
    */
    private float damping = 0.1f;

    /** 惯性的方向（规格化） */
    private Vector3 velocityDirection = Vector3.zero;

    /** 当前的速度 */
    private float velocity = 0.0f;

    void Update()
    {
        if(cameraRotationH == null){return;}
        if(cameraRotationV == null){return;}
        if(mainCamera == null){return;}
        if(lookTarget == null){return;}

        var isUpMouse = false;
        if(Input.GetMouseButtonDown(0))// 鼠标按下
        {
            var isOnUI = EventSystem.current.IsPointerOverGameObject();
            if(isOnUI) return;
            if(!isDownMouse){
                isDownMouse = true;
                downMousePos = Input.mousePosition;
                downHRotation = cameraRotationH.transform.localRotation;
                downVRotation = cameraRotationV.transform.localRotation;
            }
            
        }else if(Input.GetMouseButtonUp(0))// 鼠标松开
        {
            if(isDownMouse){
                isDownMouse = false;
                isUpMouse = true;
            }
        }

        if(isDownMouse)
        {
            OnMouseMove();
        }else{
            OnVelocityMove();
        }

        // 让摄像机始终指向观察的目标(摄像机的Z轴要指向lookTarget)
        Vector3 pos =  lookTarget.transform.position;
        mainCamera.transform.LookAt(pos);

        if(isUpMouse)
        {
            OnMouseUp();
        }

        var curTime = Time.time;
        if(curTime >= (lastFramePositionTime + 0.05f))
        {
            // 保存上一次的帧位置
            lastFramePosition = Input.mousePosition;
            lastFramePositionTime = Time.time;
        }
    }

    /** 根据惯性进行移动摄像机 */
    void OnVelocityMove()
    {
        // 进行减速
        if(velocity > 0){
            float deltaX = velocityDirection.x * velocity;
            Quaternion addAngle = Quaternion.AngleAxis(deltaX * 0.5f,Vector3.up);
            Quaternion newQuaternion = addAngle * cameraRotationH.transform.localRotation;
            
            // 纵向像素差
            float deltaY = velocityDirection.y * velocity;
            Quaternion addAngle2 = Quaternion.AngleAxis(deltaY * -0.2f,Vector3.forward);
            Quaternion newQuaternion2 = addAngle2 * cameraRotationV.transform.localRotation;

            cameraRotationV.transform.localRotation = LimitVRotation(newQuaternion2);
            cameraRotationH.transform.localRotation = newQuaternion;

            velocity = velocity - velocity * damping;
            if(velocity < 0)velocity = 0;
        }
    }

    /** 限制垂直的旋转角度，不能大于60度，不能小于10度 */
    Quaternion LimitVRotation(Quaternion q)
    {
        Quaternion ret = q;
        float angle = q.eulerAngles[2];// Z轴的旋转角度
        if(angle > 180)angle = angle - 360;// 为处理大于180度其实是负数角度的问题
        float max = 60;
        float min = -10;
        if(angle < min){
            ret = Quaternion.AngleAxis(min,Vector3.forward);
        }else if(angle > max){
            ret = Quaternion.AngleAxis(max,Vector3.forward);
        }
        return ret;
    }

    /** 鼠标松开时，进行创建惯性速度 */
    void OnMouseUp()
    {
        var curPos = Input.mousePosition;
        // 上一帧鼠标的位置指向当前帧鼠标的位置
        var vec = curPos - lastFramePosition;
        velocity = vec.magnitude;
        velocityDirection = vec.normalized;
    }

    /** 在屏幕上进行拖动 */
    void OnMouseMove()
    {
        var curPos = Input.mousePosition;
        // 横向像素差
        float deltaX = curPos.x - downMousePos.x;
        Quaternion addAngle = Quaternion.AngleAxis(deltaX * 0.5f,Vector3.up);
        Quaternion newQuaternion = addAngle * downHRotation;

        // 纵向像素差
        float deltaY = curPos.y - downMousePos.y;
        Quaternion addAngle2 = Quaternion.AngleAxis(deltaY * -0.2f,Vector3.forward);
        Quaternion newQuaternion2 = addAngle2 * downVRotation;

        cameraRotationV.transform.localRotation = LimitVRotation(newQuaternion2);
        cameraRotationH.transform.localRotation = newQuaternion;
    }
}
