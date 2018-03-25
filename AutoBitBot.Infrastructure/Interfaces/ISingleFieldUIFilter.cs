using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Interfaces
{
    public interface ISingleFieldUIFilter<T>
    {
        T FilterField { get; }
    }

    public interface ISingleFieldUIFilter : ISingleFieldUIFilter<String>
    {
    }


}
