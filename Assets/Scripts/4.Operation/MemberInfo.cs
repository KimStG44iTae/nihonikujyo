using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberInfo : MonoBehaviour
{
    public MemberInfomation memberInfo;

    public void UpdateOn()
    {
        OperationSceneManager.instance.operationUIManager.UpdateOn();
        OperationSceneManager.instance.updateManager.SetMemberInformation(memberInfo);
    }
}
