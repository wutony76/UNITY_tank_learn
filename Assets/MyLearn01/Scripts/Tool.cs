using System.Collections;
using System.Collections.Generic;
using UnityEngine;



static class Tool {

    public static Vector3 getRotationValue(Transform transform) {
        System.Type transformType = transform.GetType();
        System.Reflection.PropertyInfo m_propertyInfo_rotationOrder = transformType.GetProperty("rotationOrder", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(transform, null);
        System.Reflection.MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        object value = m_methodInfo_GetLocalEulerAngles.Invoke(transform, new object[] { m_OldRotationOrder });
        string temp = value.ToString();

        //将字符串第一个和最后一个去掉
        temp = temp.Remove(0, 1);
        temp = temp.Remove(temp.Length - 1, 1);
        //用‘，’号分割
        string[] tempVector3;
        tempVector3 = temp.Split(',');
        //将分割好的数据传给Vector3
        Vector3 vector3 = new Vector3(float.Parse(tempVector3[0]), float.Parse(tempVector3[1]), float.Parse(tempVector3[2]));
        //Debug.Log("vector3 =" + vector3);
        return vector3;
    }

}


