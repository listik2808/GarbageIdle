using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Data))]
public class AddRewardGarbage : MonoBehaviour
{
    [SerializeField] private ValueHandler _valueHandler;
    [SerializeField] private TMP_Text _textMoney;
    [SerializeField] private TMP_Text _textLevel;
    [SerializeField] private MoneyTransfer _moneyTransfer;

    private Data _data;

    private IntegrationMetric _integrationMetric = new IntegrationMetric();

    private List<Garbage> _garbage = new List<Garbage>();

    private const string SaveNameMoney = "MoneyRewardGarbage";
    private const string SaveNameLevel = "RewardLevelWorker";
    private const string SaveNameAmount = "AmountMoney";
    private const string SaveNameCount = "NumberUsesRevardGarbage";

    private string _type = "improvement";
    private string _name = "AddRewardGarbage";
    private int _amount = 0;
    private int _count = 0;
    private float _money = 600;
    private int _level = 0;

    private void Start()
    {
        _data = GetComponent<Data>();

        _amount = _data.GetSave(SaveNameAmount, _amount);
        _money = _data.GetSaveFloat(SaveNameMoney, _money);
        _level = _data.GetSave(SaveNameLevel, _level);
        _count = _data.GetSave(SaveNameCount, _count);

        _textMoney = _moneyTransfer.CurrencyConversion(_money, _textMoney);
        _textLevel.text = "lvl." + _level.ToString();
    }

    public void AddListGarbage(Garbage garbage)
    {
        _garbage.Add(garbage);
    }

    public void AssignValue()
    {
        if (_valueHandler.Money >= _money)
        {
            _amount += (int)_money;
            _count++;
            _integrationMetric.OnSoftCurrencySpend(_type,_name,_amount,_count,(int)_money);
            PlayerPrefs.SetInt(SaveNameAmount, _amount);
            PlayerPrefs.SetInt(SaveNameCount, _count);
            _valueHandler.PayPurchase(_money);
            _level++;
            PlayerPrefs.SetInt(SaveNameLevel, _level);

            foreach (var item in _garbage)
            {
                if (item.Removed == false)
                {
                    item.RisePrice();
                }
            }
            SetPrice();
        }
        else
            return;
    }

    private void SetPrice()
    {
        _money += 400;
        PlayerPrefs.SetFloat(SaveNameMoney, _money);
        _textMoney = _moneyTransfer.CurrencyConversion(_money, _textMoney);
        _textLevel.text = "lvl." + _level.ToString();
    }  
}