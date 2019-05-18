
using Hyt.DataAccess.Base;

namespace Hyt.DataAccess
{
    public abstract class ITest:DaoBase<ITest>
    {
        public abstract string GetName(string s);
        public abstract int Count();
        }
}
