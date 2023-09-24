using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecodInfo : MonoBehaviour
{
    public int id;
    public string date;
    public Record record;

    public void SelectRecord()
    {
        CallLogData callLogData = new CallLogData();
        callLogData.userID = id;
        callLogData.date = date;
        callLogData.entry_time = record.entry_time;
        callLogData.exit_time = record.exit_time;

        Monitoring_LogManager.instance.receiveLogData.StartReceiveLogData(callLogData);
    }
}
