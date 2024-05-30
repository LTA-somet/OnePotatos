using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public Transform target;
    public Vector3 offset =new Vector3(0,0,-10);
    void LateUpdate()
    {
        if (target != null)
        {
            // Cập nhật vị trí của camera dựa trên vị trí của target và offset
            Vector3 newPosition = target.position + offset;
            // Đảm bảo camera chỉ di chuyển theo trục X và Y trong game 2D
            newPosition.z = -10;  // Giữ nguyên giá trị Z để camera không di chuyển trên trục Z
            transform.position = newPosition;
        }
    }
}
