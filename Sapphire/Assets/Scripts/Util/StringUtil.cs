using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringUtil{
    
    private static string defaultAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string RandomString(int newLength = 1, string userAlphabet = ""){
        
        char[] alphabet = GetAlphabet(userAlphabet);

		int alphabetLength = alphabet.Length;
		string randomLetters = "";
			
		for (int i = 0; i < newLength; i++){

            int rand = UnityEngine.Random.Range(0, alphabetLength - 1);//FlashProxy.getRandomRange(0, alphabetLength - 1); /*UnityEngine.Random.Range(0, alphabetLength - 1)*/;
            randomLetters += alphabet[rand].ToString();
		}
			
		return randomLetters;
    }

    public static string FormatDigit(int number, int digit) {

        string result = "";
        string numberString = number.ToString();
        int digitCount = numberString.Length;

        if (digitCount < digit) {

            int len = digit - digitCount;

            for (int i = 0; i < len; i++) {

                result += "0";
            }

        }
        result += number;
        
        return result;

    }

    public static string FormatNumber(int value, int cutStep = 0) {

        string append = "";

        double val = 0;

        if (cutStep > 0 && value > Mathf.Pow(1000, cutStep)) {

            val = value / Mathf.Pow(1000, cutStep);

            value = Mathf.FloorToInt(value/Mathf.Pow(1000, cutStep));

            val = (val*10 - value*10);

            val = Math.Floor(val);

            switch (cutStep) {
                case 1:
                    append = "K";
                    break;
                case 2:
                    append = "M";
                    break;

            }
        }

        char[] temp = value.ToString().ToCharArray();

        ArrayList tempArray = new ArrayList();
        foreach (char s in temp) {
            tempArray.Add(s.ToString());
        }
        tempArray.Reverse();

        ArrayList resultArray = new ArrayList();

        for (int i = 0; i < tempArray.Count; i++) {

            if (i > 0 && i % 3 == 0) {
                resultArray.Add(".");
            }
            resultArray.Add(tempArray[i]);
        }

        resultArray.Reverse();

        string resultString = "";

        foreach (string s in resultArray) {
            resultString += s;
        }

        string dec = "";

        if (val > 0) {
            dec = "," + val;
        }

        return resultString+dec+append;

    }

    public static List<string> SecondsToTimeArray(int time) {

        List<string> formattedTime = new List<string>();

        int daysNumber = Mathf.FloorToInt((float)time / 86400);
        string daysString = "";

        if (daysNumber < 10) {
            daysString = "";
        }
        daysString += daysNumber.ToString();
        time = time % 86400;

        int hoursNumber = Mathf.FloorToInt(time / 3600f);
        string hoursString = "";
        if (hoursNumber < 10) {
            hoursString = "";
        }
        hoursString += hoursNumber.ToString();
        time = time % 3600;

        int minsNumber = Mathf.FloorToInt((float)time / 60);
        string minsString = "";
        if (minsNumber < 10) {
            minsString = "0";
        }
        minsString += minsNumber.ToString();
        time = time % 60;

        string secsString = "";
        if (time < 10) {
            secsString = "0";
        }
        secsString += time.ToString();

        formattedTime.Add(daysString);
        formattedTime.Add(hoursString);
        formattedTime.Add(minsString);
        formattedTime.Add(secsString);

        return formattedTime;

    }

    public static string SecondsToTimeString(int time) {
        List<string> formattedTime = SecondsToTimeArray(time);

        string[] steps = new[] {"", "", "", ""};

        int index = 0;

        string final = "";

        foreach (var formatted in formattedTime) {

            if (formatted != "0") {

                if (final != "") {
                    final += " : ";
                }

                final += formatted+steps[index];
            }

            index++;
        }

        if (formattedTime.Count < 2) {
            //final = "00:" + final;
        }

        return final;
    }
    
    private static char[] GetAlphabet(string userAlphabet = ""){

        char[] alphabet;

        if(userAlphabet == ""){
            alphabet = defaultAlphabet.ToCharArray();
        }else{
            alphabet = userAlphabet.ToCharArray();
        }

        return alphabet;
    }
}

