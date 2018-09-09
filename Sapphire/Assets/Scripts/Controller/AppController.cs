using System;
using System.Collections;
using System.Collections.Generic;
using EasyModbus;
using OpenWebNet;
using SimpleJSON;
using UnityEditor;
using UnityEngine;

public class AppController : MonoBehaviour {

    private static AppController _instance;

    private bool _allConnected;
    private JSONNode _node;
    private Header _header;
    private RectTransform _rectTransform;
    private Dictionary<string, List<Action>> _actions;

    private EthGateway _monitor;
    private EthGateway _command;

    private ModbusServer _server;
    
    void Awake() {

        Screen.SetResolution(480, 140, false);

        _rectTransform = gameObject.GetComponent<RectTransform>();
        _instance = this;
    }

    void Start() {

        
#if UNITY_EDITOR
        LoadRequest r = QueueLoader.Instance.CreateRequest(Application.dataPath + "/Setup/Setup.json", FileType.TEXT, false, OnLoad);
#else
        LoadRequest r = QueueLoader.Instance.CreateRequest(Application.dataPath + "../../Setup.json", FileType.TEXT, false, OnLoad);
#endif
        
        _header = new Header(_rectTransform);
    }

    void Update() {

        /*if (!_allConnected && _node != null) {

            if (_command.IsConnected && _monitor.IsConnected) {
                StartListen();
            } else {
                _header.SetConnected(false);
            }

        }*/

        if (_node != null) {
            if (_monitor.IsConnected) {

                if (!_allConnected) {
                    _allConnected = true;
                    StartListen();
                }

                _header.SetConnected();

            } else {
                _header.SetConnected(false);
                _allConnected = false;
            }
        }

    }

    private void StartListen() {
        _allConnected = true;
        
        foreach (KeyValuePair<string, List<Action>> actionPair in _actions) {

            foreach (var action in actionPair.Value) {

                action.SelfInvoke();

            }

        }
    }

    private void OnLoad(LoadRequest r, string error) {

        var node = JSON.Parse(r.Text);
       
        _monitor = new EthGateway(node["GatewayIp"].Value, node["GatewayPort"].AsInt, OpenSocketType.Monitor);

        _server = new ModbusServer();

        _actions = new Dictionary<string, List<Action>>();

        int len = node["Devices"].Count;

        for (int i = 0; i < len; i++) {
            
            Action action = new Action(node["Devices"][i]);

            string where = node["Devices"][i]["where"].Value;

            if (!_actions.ContainsKey(where)) {
                _actions.Add(where, new List<Action>());
            }

            _actions[where].Add(action);
            
        }
        
        //_command.Connect();
        _monitor.Connect();

        _monitor.MessageReceived += (sender, e) => {
            
            Message message = MessageAnalyzer.GetMessage(e.Data);
            Debug.Log("OnMessage: " + e.Data + ":" + message);

            if (message != null) {

                if (_actions.ContainsKey(message.Where)) {

                    foreach (var action in _actions[message.Where]) {
                        action.UpdateDevice(e.Data);

                    }

                }
            }

        };

        _monitor.ConnectionError += (sender, args) => {
            Debug.Log(args.Exception);
        };

        _server.Listen();

        _server.CoilsChanged += new ModbusServer.CoilsChangedHandler(CoilsChanged);
        _server.HoldingRegistersChanged += new ModbusServer.HoldingRegistersChangedHandler(HoldingRegistersChanged);

        _node = node;
    }

    private void CoilsChanged(int coil, int numberOfCoil){

        foreach (KeyValuePair<string, List<Action>> actionPair in _actions) {

            foreach (var action in actionPair.Value) {

                if (action.Coil && action.Index == coil) {

                    int val = _server.coils[coil] ? 1:0;

                    action.UpdateModbus(val);
                }

            }

        }

    }

    private void HoldingRegistersChanged(int register, int numberOfRegisters){

        foreach (KeyValuePair<string, List<Action>> actionPair in _actions) {

            foreach (var action in actionPair.Value) {

                if (!action.Coil && action.Index == register) {

                    int val = _server.holdingRegisters[register];

                    action.UpdateModbus(val);
                }

            }

        }
    }

    public void SendCommand(string command) {
        //_command.SendData(command);


        if (_command == null) {
            _command = new EthGateway(_node["GatewayIp"].Value, _node["GatewayPort"].AsInt, OpenSocketType.Command);
        }

        if (!_command.IsConnectedToGateway) {

            _command.Connected += delegate(object sender, EventArgs args) {

                Debug.Log("Send Command:" + command);

                _command.SendData(command);

            };

            _command.Disconnected += delegate(object sender, EventArgs args) {

                Debug.Log("onDisconnect");

                _command = null;
            };

            _command.Connect();

        } else {
            _command.SendData(command);
        }

    }

    public void SendBraodcastModbus(int index, bool coil, int data) {
        
        Debug.Log("Broadcast: "+index+"-"+coil+"-"+data);

        if (coil) {
            _server.coils[index] = data == 1;
            
        } else {
            _server.holdingRegisters[index] = (Int16)data;
            
        }

    }

    public static AppController Instance => _instance;

}
