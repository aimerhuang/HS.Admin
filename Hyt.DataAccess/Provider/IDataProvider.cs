using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Hyt.DataAccess.Base;
using IsolationLevel = System.Data.IsolationLevel;

namespace Hyt.DataAccess.Provider
{
    public interface IDataProvider
    {
        T Create<T>() where T : DaoBase<T>;
    }
}
