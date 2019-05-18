using Hyt.Model;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.CRM
{
    public class CrWeChatBindBo : BOBase<CrWeChatBindBo>
    {
        public int InnerMod(CrWeChatBind mod)
        {
            return Hyt.DataAccess.Order.ICrWeChatBindDao.Instance.InnerMod(mod);
        }
        public void UpdateMod(CrWeChatBind mod)
        {
            Hyt.DataAccess.Order.ICrWeChatBindDao.Instance.UpdateMod(mod);
        }
        public void DeleteMod(int SysNo)
        {
            Hyt.DataAccess.Order.ICrWeChatBindDao.Instance.DeleteMod(SysNo);
        }

        public BCCrWeChatBind GetWeChatBindData(int SysNo)
        {
            return Hyt.DataAccess.Order.ICrWeChatBindDao.Instance.GetWeChatBindData(SysNo);
        }

        public void GetChatBindDataPager(ref Pager<BCCrWeChatBind> pager)
        {
            Hyt.DataAccess.Order.ICrWeChatBindDao.Instance.GetChatBindDataPager(ref pager);
        }

        public List<BCCrWeChatBind> GetWeChatBindListByTrue()
        {
             return Hyt.DataAccess.Order.ICrWeChatBindDao.Instance.GetWeChatBindListByTrue();
        }
    }
}
