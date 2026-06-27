/*
 
    -----------------------
    UDP-Send
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
   
    // > gesendetes unter
    // 127.0.0.1 : 8050 empfangen
   
    // nc -lu 127.0.0.1 8050
 
        // todo: shutdown thread at the end
*/


///////// we're sending vicinity to server
///////// we're sending Break
///////// we're getting children
///////// server should categorize that what cars are near each other 
///////// server should send command to the cars that are involved

//////////////// Server is not complete





using UnityEngine;
using System.Collections;
 
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
 
public class UDPSend : MonoBehaviour
{
    private static int localPort;
   
    // prefs
    private string IP;  // define in init
    public int port;  // define in init


    public string strMessage1;
    public string strMessage2;
    public string strMessage3;
    public float Vicinity;
    public float Speed1;
    public float Speed2;
    
    

   
    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;
   







    

 void Update()
    {
        // sending vicinity
    Vicinity_Check VicinityCheck = GameObject.Find("Car").GetComponent<Vicinity_Check> ();
    Vicinity = VicinityCheck.Car_InVicinity;
    strMessage1 = Vicinity.ToString();

        //sending speed
    GameObject Cars = GameObject.Find("Car");
    CarEngine Car_Engine = Cars.GetComponent<CarEngine>();
    Speed1 = Car_Engine.currentSpeed;
    strMessage2 = Speed1.ToString();


    GameObject Cars_2 = GameObject.Find("Car (1)");
    CarEngine Car_Engine2 = Cars_2.GetComponent<CarEngine>();
    Speed2 = Car_Engine2.currentSpeed;
    strMessage3 = Speed2.ToString();

        //checking speed
        if ((Vicinity >= 1) && (Speed1 > Speed2)){
                Car_Engine2.breaking = true;
                Car_Engine.currentSpeed = Car_Engine.currentSpeed / 2;
                Car_Engine.TargetSteerAngle = Car_Engine.TargetSteerAngle /2;
            } else {
                Car_Engine2.breaking = false;
            }
        if ((Vicinity >= 1) && (Speed2 > Speed1)){
                Car_Engine.breaking = true;
                Car_Engine2.currentSpeed = Car_Engine2.currentSpeed / 2;
                Car_Engine2.TargetSteerAngle = Car_Engine2.TargetSteerAngle /2;
            } else {
                Car_Engine.breaking = false;
            }

            

    }

    
       
    // call it from shell (as program)
    private static void Main()
    {
        UDPSend sendObj=new UDPSend();
        sendObj.init();
        
       
        // testing via console
        // sendObj.inputFromConsole();
       
        // as server sending endless
        sendObj.sendEndless(" endless infos \n");
       
    }
    // start from unity3d
    public void Start()
    {
        init();
    }






    // OnGUI
    void OnGUI()
    {
       
        // ------------------------
        // send it
        // ------------------------
        //strMessage=GUI.TextField(new Rect(40,420,140,20),strMessage());
        if (Vicinity != 0)
        {
            //send if near vicinity
            sendString(strMessage1+"\n"); //vicinity
            sendString(strMessage2+"\n"); //speed 1
            sendString(strMessage3+"\n"); //speed 2
            
        }      
    }
   
    // init
    public void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
       
        // define
        IP="127.0.0.1";
        port=8051;
       
        // ----------------------------
        // Senden
        // ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();
       
   
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