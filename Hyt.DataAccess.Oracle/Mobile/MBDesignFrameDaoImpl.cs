using Hyt.DataAccess.Mobile;
using Hyt.Model.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Mobile
{
    public class MBDesignFrameDaoImpl : IMBDesignFrameDao
    {
        public override int InsreMod(Model.Mobile.MBDesignFrame mod)
        {
            return Context.Insert("MBDesignFrame", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Mobile.MBDesignFrame mod)
        {
            Context.Update("MBDesignFrame", mod).AutoMap(p => p.SysNo).Execute();
        }

        public override void DeleteMod(int SysNo)
        {
            string sql = "delete from MBDesignFrame where SysNo = " + SysNo;
            Context.Sql(sql).Execute();
        }

        public override List<Model.Mobile.MBDesignFrame> GetPageDataList(int customSysNo, string pageText, string tipCode)
        {
            string sql = "select * from MBDesignFrame where CustomerSysNo = " + customSysNo + " and PageText  = '" + pageText + "' and  TipCode = '" + tipCode + "' order by SortBy ASC ";
            return Context.Sql(sql).QueryMany<MBDesignFrame>();
        }

        public override List<Model.Mobile.MBDesignFrame> GetPageDataListByPSysNo(int pSysNo, string pageText, string tipCode)
        {
            string sql = "select * from MBDesignFrame where PSysNo = " + pSysNo + " and PageText  = '" + pageText + "' and  TipCode = '" + tipCode + "'  order by SortBy ASC ";
            return Context.Sql(sql).QueryMany<MBDesignFrame>();
        }

        public override List<MBDesignFrame> GetPageDataAllList()
        {
            string sql = " select * from MBDesignFrame ";
            return Context.Sql(sql).QueryMany<MBDesignFrame>();
        }
    }
}
