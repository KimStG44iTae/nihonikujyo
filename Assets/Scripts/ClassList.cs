using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Networking;

//인증키 생성 클래스
public class CertPublicKey : CertificateHandler
{
    public string PUB_KEY;

    // Encoded RSAPublicKey
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();

        if (pk.ToLower().Equals(PUB_KEY.ToLower()))
            return true;
        else
            return false;
    }
}

//userInfo
[Serializable]
public class UserZoneData
{
    public int userID;
    public List<string> zone_data;
    public string date;
}


//ReceiveUWBData
[Serializable]
public class UWBData
{
    public string _id;
    public string tagId;
    public double[] position;
    public string dateTime;
}

[Serializable]
public class UWBDataList
{
    public UWBData[] uwbInfoList;
}


//ReceiveWatchData
[Serializable]
public class WatchData
{
    public string _id;
    public string date;
    public double heart_Rate;
    public int watchID;
}

[Serializable]
public class WatchDataList
{
    public WatchData[] watchInfoList;
}


//ReceiveUserData
[Serializable]
public class UserData
{
    public int userID;
    public string covid;
}

[Serializable]
public class UserDataList
{
    public UserData[] userList;
}



//ReceiveMultiData
[Serializable]
public class MultiData
{
    public int userID;
    public double position_X;
    public double position_Y;
    public double position_Z;
    public string motion;
    public double heart_rate;
    public bool fall;
    public List<string> zone;
    public string date;
    public string time;
    public string contactor;
}

[Serializable]
public class MultiDataList
{
    public MultiData[] multiData;
}
