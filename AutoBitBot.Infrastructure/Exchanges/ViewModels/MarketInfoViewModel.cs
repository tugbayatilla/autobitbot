﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges.ViewModels
{
    public class MarketInfoViewModel : ObservableObject
    {
        Decimal fee, minTraceSize;
        String marketName, currency, baseCurrency;
        Boolean isActive;

        public String MarketName
        {
            get => marketName;
            set
            {
                marketName = value;
                OnPropertyChanged();
            }
        }

        public String Currency
        {
            get => currency;
            set
            {
                currency = value;
                OnPropertyChanged();
            }
        }

        public String BaseCurrency
        {
            get => baseCurrency;
            set
            {
                baseCurrency = value;
                OnPropertyChanged();
            }
        }

        public Decimal Fee
        {
            get => fee;
            set
            {
                fee = value;
                OnPropertyChanged();
            }
        }

        public Decimal MinTradeSize
        {
            get => minTraceSize;
            set
            {
                minTraceSize = value;
                OnPropertyChanged();
            }
        }

        public Boolean IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                OnPropertyChanged();
            }
        }



    }
}