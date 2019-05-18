using Hyt.DataAccess.Pos;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    /// <summary>
    /// 钱箱金额设定和修改
    /// </summary>
    public class DsPosMoneyBoxBo : BOBase<DsPosMoneyBoxBo>
    {
        /// <summary>
        /// 添加钱箱金额
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public  int InsertMod(Model.Pos.DsPosMoneyBox box)
        {
            return IDsPosMoneyBoxDao.Instance.InsertMod(box);
        }
        /// <summary>
        /// 修改钱箱金额
        /// </summary>
        /// <param name="box"></param>
        public  void UpdateMod(Model.Pos.DsPosMoneyBox box)
        {
            IDsPosMoneyBoxDao.Instance.UpdateMod(box);
        }
        /// <summary>
        /// 删除钱箱金额
        /// </summary>
        /// <param name="SysNo"></param>
        public  void DeleteMod(int SysNo)
        {
            IDsPosMoneyBoxDao.Instance.DeleteMod(SysNo);
        }
        /// <summary>
        /// 获取钱箱金额实体
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public  Model.Pos.CBDsPosMoneyBox GetEntity(int SysNo)
        {
            return IDsPosMoneyBoxDao.Instance.GetEntity(SysNo);
        }

        /// <summary>
        /// 获取销售单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="dsSysNo"></param>
        /// <returns></returns>
        public PagedList<CBDsPosMoneyBox> GetDsPosMoneyBoxListPagerByDsSysNo(int? pageIndex, int? dsSysNo)
        {
            var returnValue = new PagedList<CBDsPosMoneyBox>();

            var pager = new Pager<CBDsPosMoneyBox>
            {

                PageFilter = new CBDsPosMoneyBox
                {
                    DsSysNo =dsSysNo==null?0 : dsSysNo.Value
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            IDsPosMoneyBoxDao.Instance.GetDsPosMoneyBoxListPagerByDsSysNo(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }
    }
}
