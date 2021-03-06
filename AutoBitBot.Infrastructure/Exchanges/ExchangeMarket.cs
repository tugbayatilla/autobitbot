﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public class ExchangeMarket : ObservableObject
    {
        Decimal minTraceSize;
        String marketName, currency, baseCurrency;
        Boolean isActive;

        public String MarketName
        {
            get => marketName;
            set
            {
                marketName = value;
                RaisePropertyChanged();
            }
        }

        public String Currency
        {
            get => currency;
            set
            {
                currency = value;
                RaisePropertyChanged();
            }
        }

        public String BaseCurrency
        {
            get => baseCurrency;
            set
            {
                baseCurrency = value;
                RaisePropertyChanged();
            }
        }

        public Decimal MinTradeSize
        {
            get => minTraceSize;
            set
            {
                minTraceSize = value;
                RaisePropertyChanged();
            }
        }

        public Boolean IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                RaisePropertyChanged();
            }
        }

    }
}
