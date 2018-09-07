using System.Collections;
using UnityEngine;

public class Json{

    public static string GetJson(ArrayList data) {

        string json = "[" + ConvertArrayList(data) + "]";

//        Debug.Log(json);

        return json;
    }

    public static string GetJson(Hashtable data){
        string json = "{" +ConvertHashtable(data)+ "}";
//        Debug.Log(json);

        return json;
    }

   

    private static string ConvertArrayList(ArrayList array){

        bool first = true;
        string data = "";

        foreach (var variable in array){
            
            if(first){
                first = false;
            }else{
                data += ",";
            }

            if(variable is Hashtable){

                data += "{"+ConvertHashtable((Hashtable)variable)+"}";
                continue;
            }

            if(variable is ArrayList){
                data += "[" + ConvertArrayList((ArrayList) variable) + "]";
                continue;
            }

            data += "\"" + variable + "\"";

        }

        return data;
    }

    private static string ConvertHashtable(Hashtable hashtable){
        bool first = true;
        string data = "";

        foreach (DictionaryEntry argument in hashtable) {
            if (first) {
                first = false;
            } else {
                data += ",";
            }

            data += "\"" + argument.Key + "\":";

            if(argument.Value is Hashtable){
                data += "{" + ConvertHashtable((Hashtable)argument.Value) + "}";
                continue;
            }

            if(argument.Value is ArrayList){
                data += "[" + ConvertArrayList((ArrayList)argument.Value) + "]";
                continue;
            }

            data += "\"" + argument.Value.ToString() + "\"";

        }

        return data;
    }
}

