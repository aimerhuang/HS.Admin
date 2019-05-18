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
    /// 会员卡数据
    /// </summary>
    public class DsMemberBo : BOBase<DsMemberBo>
    {
        /// <summary>
        ///  添加会员卡
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public int InsertMembershipCard(DsMembershipCard card)
        {
            return IDsMembershipCardDao.Instance.Insert(card);
        }
        public bool CheckMembershipCard(string cardNumber)
        {
            return IDsMembershipCardDao.Instance.CheckMembershipCard(cardNumber);
        }
        /// <summary>
        /// 添加会员等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int InsertDsMembershioLevel(DsMembershioLevel level)
        {
            return IDsMembershioLevelDao.Instance.Insert(level);
        }
        /// <summary>
        /// 添加付款配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public int InsertDsPaymentToPointConfig(DsPaymentToPointConfig config)
        {
            return IDsPaymentToPointConfigDao.Instance.Insert(config);
        }

        ///------------------------------------------------------------------
        /// <summary>
        ///  修改会员卡
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public void UpdateMembershipCard(DsMembershipCard card)
        {
            IDsMembershipCardDao.Instance.Update(card);
        }
        /// <summary>
        /// 修改会员等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public void UpdateDsMembershioLevel(DsMembershioLevel level)
        {
            IDsMembershioLevelDao.Instance.Update(level);
        }
        /// <summary>
        /// 修改付款配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public void UpdateDsPaymentToPointConfig(DsPaymentToPointConfig config)
        {
            IDsPaymentToPointConfigDao.Instance.Update(config);
        }
        ///------------------------------------------------------------------
       
        /// <summary>
        /// 获取会员等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public DsMembershioLevel GetDsMembershioLevel(int SysNo)
        {
            return IDsMembershioLevelDao.Instance.GetDsMembershioLevelBySysNo(SysNo);
        }
        /// <summary>
        /// 获取付款配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public DsPaymentToPointConfig GetDsPaymentToPointConfig(int dsSysNo)
        {
            return IDsPaymentToPointConfigDao.Instance.GetDsPaymentToPointConfigBySysNo(dsSysNo);
        }

        /// <summary>
        /// 获取会员信息的等级列表
        /// </summary>
        /// <returns></returns>
        public List<DsMembershioLevel> GetDsMembershioLevelList(int dsSysNo)
        {
            return IDsMembershioLevelDao.Instance.GetDsMembershipLevelList(dsSysNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DsMembershipCard> GetMembershipCardList(int dsSysNo)
        {
            return IDsMembershipCardDao.Instance.GetMembershipCardList(dsSysNo);
        }
        ///----------------------会员积分控制------------------------------
        public int InsertMemberPoint(MemberPointHistory memberMod)
        {
            return IMemberPointHistoryDao.Instance.Insert(memberMod);
        }

        public void UpdateMemberPoint(MemberPointHistory memberMod)
        {
            IMemberPointHistoryDao.Instance.Update(memberMod);
        }
        public List<MemberPointHistory> GetMemberPointHistoryList(string cardNumber)
        {
            return IMemberPointHistoryDao.Instance.GetMemberPointHistory(cardNumber);
        }



        public object GetMembershipCardListByPager(int? pageIndex, int dsSysNo)
        {
            var returnValue = new PagedList<CBDsMembershipCard>();

            var pager = new Pager<CBDsMembershipCard>
            {
                PageFilter = new CBDsMembershipCard
                {
                    DsSysNo = dsSysNo
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            IDsMembershipCardDao.Instance.GetMembershipCardListByPager(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }

        public CBDsMembershipCard GetMembershipCardBySysNo(int SysNo)
        {
            return IDsMembershipCardDao.Instance.GetMembershipCardBySysNo(SysNo);
        }

        public CBDsMembershipCard GetMembershipCardBySysNo(string Number)
        {
            return IDsMembershipCardDao.Instance.GetMembershipCardBySysNo(Number);
        }
    }
}
