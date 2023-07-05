using System.Collections;
using UnityEngine;
using TMPro;
using System.Text;

public class Countable : MonoBehaviour
{
    [SerializeField] private float _textAnimtaionSpeed=2;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int minNumOfDigits = 1;

    protected string BaseText = "";

    int _savedTotalNumber = 0;
    int _viewTotalNumber = 0;
    string _zeroDigits = "";

    protected virtual void OnEnable()
    {
        SetStartingValue();
    }

    protected virtual void SetStartingValue(int value = 0)
    {
        _viewTotalNumber = value;
        _savedTotalNumber = value;
        SetText();
    }

    //public void AddNumber(int addedNumber)
    //{
    //    StartCoroutine(AddNumberCoroutine(addedNumber));
    //    _savedTotalNumber += addedNumber;
    //}

    public void SetNumber(int newNumber)
    {
        StartCoroutine(AddNumberCoroutine(newNumber - _savedTotalNumber));
        _savedTotalNumber = newNumber;
    }

    private void UpdateText(int addedNumber)
    {
        _viewTotalNumber += addedNumber;
        SetText();
    }

    private IEnumerator AddNumberCoroutine(int number)
    {
        int numberToAddThisFrame;
        int numberLeftToAdd = number;
        bool isPositive = number >= 0 ;
        while (numberLeftToAdd != 0)
        {
            numberToAddThisFrame = Mathf.CeilToInt(Time.deltaTime * number * _textAnimtaionSpeed);

            //if (isPositive)
            //{
                if ((isPositive && numberToAddThisFrame < numberLeftToAdd) || (!isPositive && numberToAddThisFrame > numberLeftToAdd))
                {
                    UpdateText(numberToAddThisFrame);
                    numberLeftToAdd -= numberToAddThisFrame;
                }
                else
                {
                    UpdateText(numberLeftToAdd);
                    numberLeftToAdd -= numberLeftToAdd;
                }
            //}
            //else
            //{
            //    if (numberToAddThisFrame > numberLeftToAdd)
            //    {
            //        UpdateText(numberToAddThisFrame);
            //        numberLeftToAdd -= numberToAddThisFrame;
            //    }
            //    else
            //    {
            //        UpdateText(numberLeftToAdd);
            //        numberLeftToAdd -= numberLeftToAdd;
            //    }
            //}

            yield return null;
        }
    }

    private void SetText()
    {
        string stringTotalNumber = _viewTotalNumber.ToString();
        int numOfZeros = minNumOfDigits - stringTotalNumber.Length;

        if(numOfZeros!=_zeroDigits.Length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numOfZeros; i++)
            {
                sb.Append('0');
            }
            _zeroDigits = sb.ToString();
        }
        
        _text.text = BaseText + _zeroDigits + stringTotalNumber;
    }
}
