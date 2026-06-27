using UnityEngine;
using System.Collections;
 
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class RoadSign30 : MonoBehaviour
{
    public string strMessage1;
    private static int localPort;
   
    // prefs
    private string IP;  // define in init
    public int port;  // define in init
    
    

   
    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;
   

     public float SpeedLimit = 30;



    // checking for nearby cars
    public float Nearby_Cars = 0;
    public bool Nearby_Cars_bool_30 = false;
    public string[] TagList = {"Cars"};
    public List<GameObject> ObjectsInRange = new List<GameObject>();



        public void OnTriggerEnter(Collider col){
            foreach (string TagToAdd in TagList){
                if (col.gameObject.tag == TagToAdd){
                    ObjectsInRange.Add(col.gameObject);
                    Nearby_Cars = Nearby_Cars + 1;
                    Nearby_Cars_bool_30 = true;
                }
            }
        }
            
        public void OnTriggerExit(Collider col){
            foreach (string TagToAdd in TagList){
                if (col.gameObject.tag == TagToAdd){
                    ObjectsInRange.Remove(col.gameObject);
                    Nearby_Cars = Nearby_Cars - 1;

                }
            }
        }


 
 







// sending data procedure
 void Update()
    {
    strMessage1 = SpeedLimit.ToString();   
}

    
       
    // call it from shell (as program)
    private static void Main()
    {
        UDPSend sendObj=new UDPSend();
        sendObj.init();
        
       
        // testing via console
        // sendObj.inputFromConsole();
       
        // as server sending endless
        //sendObj.sendEndless(" endless infos \n");
       
    }
    // start from unity3d
    public void Start()
    {
        init();
    }






    // OnGUI
    void OnGUI()
    {
        Rect rectObj=new Rect(40,380,200,400);
            GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj,"# UDPSend-Data\n127.0.0.1 "+port+" #\n"
                    + "shell> nc -lu 127.0.0.1  "+port+" \n"
                ,style);
       
        // ------------------------
        // send it
        // ------------------------
        //strMessage=GUI.TextField(new Rect(40,420,140,20),strMessage());


        if (Nearby_Cars > 0)
        {
            //send if a car is nearby
            sendString(strMessage1+"\n"); // Send Speed Limit
            
            
        }      

    }
   
    // init
    public void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");
       
        // define
        IP="127.0.0.1";
        port=8051;
       
        // ----------------------------
        // Senden
        // ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();
       
        // status
        print("Sending to "+IP+" : "+port);
        print("Testing: nc -lu "+IP+" : "+port);
   
    }
 
    // inputFromConsole
    private void inputFromConsole()
    {
        try
        {
            String text;
            do
            {
                text = Console.ReadLine();
 
                // Den Text zum Remote-Client senden.
                if (text != "")
                {
 
                    // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
                    byte[] data = Encoding.UTF8.GetBytes(text);
 
                    // Den Text zum Remote-Client senden.
                    client.Send(data, data.Length, remoteEndPoint);
                }
            } while (text != "");
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
 
    }
 
    // sendData
    private void sendString(string message)
    {
        try
        {
                //if (message != "")
                //{
 
                    // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
                    byte[] data = Encoding.UTF8.GetBytes(message);
 
                    // Den message zum Remote-Client senden.
                    client.Send(data, data.Length, remoteEndPoint);
                //}
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }
   
   
    // endless test
    private void sendEndless(string testStr)
    {
        do
        {
            sendString(testStr);
           
           
        }
        while(true);
       
    }

    
}
