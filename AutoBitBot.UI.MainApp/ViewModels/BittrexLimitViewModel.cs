using AutoBitBot.Infrastructure;
using AutoBitBot.ServerEngine;
using AutoBitBot.UI.MainApp.Commands.Bittrex;
using AutoBitBot.UI.MainApp.Notifiers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class BittrexLimitViewModel : ObservableObject
    {
        public BittrexLimitViewModel()
        {
            Server.Instance.Wallet.PropertyChanged += Wallet_PropertyChanged;
            this.OutputData = new ObservableCollection<OutputData>();
        }

        private void Wallet_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            FireOnPropertyChangedForAllProperties();
        }

        String market, buttonText;
        Decimal rate, quantity;

        public String Market
        {
            get => market;
            set
            {
                market = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Currency));
            }
        }

        public String Currency
        {
            get => Constants.GetCurrenyFromMarketName(this.Market, LimitType);
        }

        public LimitTypes LimitType { get; set; }

        public Decimal Rate
        {
            get => rate;
            set
            {
                rate = value;
                OnPropertyChanged();
            }
        }

        public Decimal Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged();
            }
        }

        public Decimal Available
        {
            get => Server.Instance.Wallet.Get(Constants.BITTREX, this.Currency).Amount;
        }

        public ObservableCollection<OutputData> OutputData { get; set; }

        public String ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                OnPropertyChanged();
            }
        }

        public ICommand BittrexSellLimitCommand => new BittrexSellLimitCommand();
        public ICommand BittrexBuyLimitCommand => new BittrexBuyLimitCommand();

    };




}