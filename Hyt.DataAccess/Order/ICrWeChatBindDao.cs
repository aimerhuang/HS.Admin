using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Order
{
    public abstract class ICrWeChatBindDao : DaoBase<ICrWeChatBindDao>
    {
        public abstract int InnerMod(CrWeChatBind mod);
        public abstract void UpdateMod(CrWeChatBind mod);
        public abstract void DeleteMod(int SysNo);

        public abstract BCCrWeChatBind GetWeChatBindData(int SysNo);

        public abstract void GetChatBindDataPager(ref Pager<BCCrWeChatBind> pager);

        public abstract List<BCCrWeChatBind> GetWeChatBindListByTrue();
    }
}
