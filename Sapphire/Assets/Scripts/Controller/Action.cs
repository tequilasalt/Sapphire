using System;
using SimpleJSON;
using UnityEngine;

public class Action {

    public const string FROM_MODBUS = "ModbusValue";
    public const string FROM_REGISTER = "fromRegister";
    
    private int _index;
    private int _lastValue = int.MaxValue;

    private bool _coil;

    private string _format;
    private string _where;

    private string _selfInvokeParam;

    private string[] _splittedFormat;

    public Action(JSONNode node) {

        _coil = node["modbusType"].Value == "coil";
        _index = node["modbusIndex"].AsInt;

        _where = node["where"].Value;
        _format = node["format"].Value;

        _selfInvokeParam = node["selfInvokeParam"].Value;

        _format = _format.Replace("<where>", _where);
        
        _splittedFormat = _format.Split(new []{"<modbus>"}, StringSplitOptions.None);
    }

    public bool Coil => _coil;

    public int Index => _index;

    public string Where => _where;

    public void UpdateDevice(string value) {

        //Debug.Log(value);

        value = value.Replace(_splittedFormat[0], "");
        value = value.Replace(_splittedFormat[1], "");

        //Debug.Log(_splittedFormat[0]+":"+ _splittedFormat[1]);

        if (value.IndexOf("*") > 0 || value.IndexOf("#") > 0) {
            //Debug.Log(value + "-----");
            return;
        }
        
        int val = int.Parse(value);

        if (_lastValue != int.MaxValue && _lastValue == val) {
            return;
        }

        _lastValue = val;

        AppController.Instance.SendBraodcastModbus(_index, _coil, int.Parse(value));
    }

    public void UpdateModbus(int value) {

        if (_lastValue != int.MaxValue && _lastValue == value) {
            return;
        }

        _lastValue = value;

        AppController.Instance.SendCommand(_format.Replace("<modbus>", value.ToString()));
    }

    public void SelfInvoke() {

        if (string.IsNullOrEmpty(_selfInvokeParam)) {
            return;
        }

        UpdateModbus(int.Parse(_selfInvokeParam));
    }
}
