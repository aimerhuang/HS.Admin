using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.SellBusiness;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 用户相关接口
    /// </summary>
    /// <remarks>2014-1-13 何方 创建</remarks>
    public class CrCustomerDaoImpl : ICrCustomerDao
    {
        /// <summary>
        /// 更新客户系统上下级关系
        /// </summary>
        /// <param name="sysNo">客户系统编号</param>
        /// <param name="customerSysNos">上下级关系码</param>
        /// <returns></returns>
        /// <remarks>2016-5-25 杨浩 创建</remarks>
        public override int UpdateCustomerSysNos(int sysNo, string customerSysNos)
        {
            return Context.Sql("UPDATE CrCustomer SET CustomerSysNos=@customerSysNos where SysNo=@sysNo")
                .Parameter("customerSysNos", customerSysNos)
                .Parameter("sysNo", sysNo)
                .Execute();
        }
        /// <summary>
        /// 更新客户可提返点
        /// </summary>
        /// <param name="brokerage">可提返点,可为负数</param>
        /// <param name="sysNo">客户系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public override int UpdateCustomerBrokerage(decimal brokerage, int sysNo)
        {
            string setStr = "";
            if (brokerage > 0)
            {
                setStr = " Brokerage=Brokerage+" + brokerage + ",BrokerageFreeze=BrokerageFreeze-" + brokerage;
            }
            else
            {
                setStr = " BrokerageFreeze=BrokerageFreeze-" + Math.Abs(brokerage) + ",BrokerageTotal=BrokerageTotal-" + Math.Abs(brokerage);
            }

            int rowsAffected = Context.Sql("Update CrCustomer set  " + setStr + "  where sysno = @sysno")
                                      .Parameter("sysno", sysNo)
                                      .Execute();
            return rowsAffected;
        }
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        /// <remarks>2013-07-11 黄波 修改</remarks>
        public override CrCustomer GetCrCustomerItem(int sysNo)
        {
            return Context.Sql(@"select * from CrCustomer where SysNO = @0", sysNo)
                          .QuerySingle<CrCustomer>();
        }

        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public override CBCrCustomer GetModel(int sysNo)
        {
            return Context.Sql(@"select cc.*,ccl.LevelName,
                                   pc.Account PAccount,pc.Name PName,pc.NickName PNickName,pc.HeadImage PHeadImage
                                   ,SellBusinessGradeName = case when sg.Name is null or cc.SellBusinessGradeId=0 then '普通会员' else sg.Name end
                                 from CrCustomer cc 
                                 left join CrCustomerLevel ccl on cc.LevelSysNo=ccl.sysNo 
                                 left join CrCustomer pc on cc.PSysNo=pc.SysNo
                                 left join CrSellBusinessGrade sg on cc.SellBusinessGradeId=sg.SysNo
                                 where cc.sysno=@0", sysNo)
                          .QuerySingle<CBCrCustomer>();
        }

        /// <summary>
        /// 根据会员等级ID获取等级信息
        /// </summary>
        /// <param name="sysNo">会员等级ID</param>
        /// <returns>等级信息</returns>
        /// <remarks>2013－07-01 黄志勇 创建</remarks>
        public override CrCustomerLevel GetCustomerLevel(int sysNo)
        {
            return Context.Sql(@"select * from CrCustomerLevel where SysNO = @0", sysNo)
                          .QuerySingle<CrCustomerLevel>();
        }

        /// <summary>
        /// 根据会员姓名搜索会员列表(模糊查询)
        /// </summary>
        /// <param name="name">会员姓名</param>
        /// <param name="rownum">最大条数</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－07-02 黄志勇 创建</remarks>
        public override IList<CrCustomer> SearchCustomerByName(string name, int rownum)
        {
            return Context.Sql(@"select * from CrCustomer where charindex(Name,@name)>0  and Status = 1")
                          .Parameter("name", name)
                //.Parameter("rownum", rownum)
                          .QueryMany<CrCustomer>();
        }

        /// <summary>
        /// 根据条件获取会员列表
        /// </summary>
        /// <param name="pager">会员查询条件</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－07-15 苟治国 创建</remarks>
        public override Pager<CBCrCustomer> Seach(Pager<CBCrCustomer> pager)
        {
            #region sql条件
            string sqlWhere = @"(@Status=-1 or cc.Status =@Status)
                             and (@LevelSysNo=-1 or cc.LevelSysNo =@LevelSysNo)
                             and (@EmailStatus=-1 or cc.EmailStatus =@EmailStatus)
                             and (@MobilePhoneStatus=-1 or cc.MobilePhoneStatus =@MobilePhoneStatus)

                             and (@IsReceiveEmail=-1 or IsReceiveEmail =@IsReceiveEmail)
                             and (@IsReceiveShortMessage=-1 or IsReceiveShortMessage =@IsReceiveShortMessage)
                             and (@IsPublicAccount=-1 or IsPublicAccount =@IsPublicAccount)
                             and (@IsLevelFixed=-1 or IsLevelFixed =@IsLevelFixed)
                             and (@IsExperiencePointFixed=-1 or IsExperiencePointFixed =@IsExperiencePointFixed)
                             and (@IsExperienceCoinFixed=-1 or IsExperienceCoinFixed =@IsExperienceCoinFixed)

                             and ((@Account is null or cc.Account like @Account1) or (@Name is null or cc.Name like @Name1))

                             and (@SellBusinessGradeId=-1 or cc.SellBusinessGradeId =@SellBusinessGradeId)";
            //判断是否绑定所有分销商
            if (!pager.PageFilter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                //if (pager.PageFilter.IsBindDealer)
                //{
                    sqlWhere += " and d.SysNo = " + pager.PageFilter.DealerSysNo;
                //}
                //else
                //{
                //    sqlWhere += " and d.CreatedBy = " + pager.PageFilter.DealerCreatedBy;
                //}
            }
            if (pager.PageFilter.SelectedAgentSysNo != -1)
            {
                if (pager.PageFilter.SelectedDealerSysNo != -1)
                {
                    sqlWhere += " and d.SysNo = " + pager.PageFilter.SelectedDealerSysNo;
                }
                else
                {
                    sqlWhere += " and d.CreatedBy = " + pager.PageFilter.SelectedAgentSysNo;
                }
            }
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBCrCustomer>("cc.*,ccl.LevelName,d.DealerName,u.UserName as AgentName,SellBusinessGradeName=case when cc.SellBusinessGradeId=0 then '普通会员' else sb.Name end")
                                    .From(@"CrCustomer cc left join CrCustomerLevel ccl on cc.LevelSysNo=ccl.sysNo
                                    left join DsDealer d on cc.DealerSysNo = d.SysNo 
                                    left join SyUser u on d.CreatedBy = u.SysNo
                                    left join CrSellBusinessGrade sb on cc.SellBusinessGradeId = sb.sysno ")
                                    .Where(sqlWhere)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Parameter("LevelSysNo", pager.PageFilter.LevelSysNo)
                                    .Parameter("EmailStatus", pager.PageFilter.EmailStatus)
                                    .Parameter("MobilePhoneStatus", pager.PageFilter.MobilePhoneStatus)

                                    .Parameter("IsReceiveEmail", pager.PageFilter.IsReceiveEmail)
                                    .Parameter("IsReceiveShortMessage", pager.PageFilter.IsReceiveShortMessage)
                                    .Parameter("IsPublicAccount", pager.PageFilter.IsPublicAccount)
                                    .Parameter("IsLevelFixed", pager.PageFilter.IsLevelFixed)
                                    .Parameter("IsExperiencePointFixed", pager.PageFilter.IsExperiencePointFixed)
                                    .Parameter("IsExperienceCoinFixed", pager.PageFilter.IsExperienceCoinFixed)

                                    .Parameter("Account", pager.PageFilter.Account)
                                    .Parameter("Account1", "%" + pager.PageFilter.Account + "%")
                                    .Parameter("Name", pager.PageFilter.Account)
                                    .Parameter("Name1", "%" + pager.PageFilter.Account + "%")
                                    .Parameter("SellBusinessGradeId", pager.PageFilter.SellBusinessGradeId)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("cc.sysNO desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                    .From(@"CrCustomer cc left join CrCustomerLevel ccl on cc.LevelSysNo=ccl.sysNo
                                    left join DsDealer d on cc.DealerSysNo = d.SysNo 
                                    left join SyUser u on d.CreatedBy = u.SysNo
                                    left join CrSellBusinessGrade sb on cc.SellBusinessGradeId = sb.sysno ")
                                    .Where(sqlWhere)
                                    .Parameter("Status", pager.PageFilter.Status)
                                    .Parameter("LevelSysNo", pager.PageFilter.LevelSysNo)
                                    .Parameter("EmailStatus", pager.PageFilter.EmailStatus)
                                    .Parameter("MobilePhoneStatus", pager.PageFilter.MobilePhoneStatus)

                                    .Parameter("IsReceiveEmail", pager.PageFilter.IsReceiveEmail)
                                    .Parameter("IsReceiveShortMessage", pager.PageFilter.IsReceiveShortMessage)
                                    .Parameter("IsPublicAccount", pager.PageFilter.IsPublicAccount)
                                    .Parameter("IsLevelFixed", pager.PageFilter.IsLevelFixed)
                                    .Parameter("IsExperiencePointFixed", pager.PageFilter.IsExperiencePointFixed)
                                    .Parameter("IsExperienceCoinFixed", pager.PageFilter.IsExperienceCoinFixed)

                                    .Parameter("Account", pager.PageFilter.Account)
                                    .Parameter("Account1", "%" + pager.PageFilter.Account + "%")
                                    .Parameter("Name", pager.PageFilter.Account)
                                    .Parameter("Name1", "%" + pager.PageFilter.Account + "%")
                                    .Parameter("SellBusinessGradeId", pager.PageFilter.SellBusinessGradeId)
                                    .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 更新会员
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-15 苟治国 创建</remarks>
        public override int Update(Model.CrCustomer model)
        {
            int rowsAffected = Context.Update<Model.CrCustomer>("CrCustomer", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 更新会员头像
        /// </summary>
        /// <param name="sysNo">会员系统编号</param>
        /// <param name="imagePath">头像图片地址</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－08-28 周瑜 创建</remarks>
        [Obsolete("作废")]
        public override int UpdateHeadImage(int sysNo, string imagePath)
        {
            int rowsAffected = Context.Sql("Update CrCustomer set HeadImage = @HeadImage where sysno = @sysno")
                                      .Parameter("HeadImage", imagePath)
                                      .Parameter("sysno", sysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 获取会员头像
        /// </summary>
        /// <param name="sysNo">会员系统编号</param>
        /// <returns>头像的地址</returns>
        /// <remarks>2013－08-29 周瑜 创建</remarks>
        public override string GetHeadImage(int sysNo)
        {
            return Context.Sql("select HeadImage from CrCustomer where sysno = @sysno")
                                      .Parameter("sysno", sysNo)
                                      .QuerySingle<string>();
        }

        /// <summary>
        /// 根据手机搜索会员列表
        /// </summary>
        /// <param name="mobile">手机</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        public override IList<CrCustomer> SearchCustomerByMobile(string mobile)
        {
            return Context.Sql(@"select * from CrCustomer where MOBILEPHONENUMBER = @0", mobile)
                          .QueryMany<CrCustomer>();
        }

        /// <summary>
        /// 根据帐号搜索会员列表(模糊查询)
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="rowNum">返回条数</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－7-02 黄志勇 创建</remarks>
        /// <remarks>2013－7-25 黄志勇 修改</remarks>
        public override IList<CrCustomer> SearchCustomerByAccount(string account, int rowNum)
        {
            return
                Context.Sql(
                    @"select TOP(@rownum) * from CrCustomer where (charindex(Account,@account)>0 or charindex(MobilePhoneNumber,@mobilePhoneNumber)>0)  and Status = 1")
                       .Parameter("account", account)
                       .Parameter("mobilePhoneNumber", account)
                       .Parameter("rownum", rowNum)
                       .QueryMany<CrCustomer>();
        }

        /// <summary>
        /// 获取中级会员、高级会员
        /// </summary>
        /// <returns>会员列表</returns>
        /// <remarks>2013－11-8 苟治国 创建</remarks>
        public override IList<CrCustomer> SearchCustomerList()
        {
            //select * from CrCustomer where LevelSysNo not in (select sysNo from CrCustomerLevel where rownum=1) and rownum=1
            //select * from CrCustomer where LevelSysNo not in (select sysNo from CrCustomerLevel where rownum=1) and rownum=1 and sysno=23449
            //select * from CrCustomer where LevelSysNo not in (select sysNo as LevelSysNo from (select sysNo from CrCustomerLevel order by lowerlimit asc)  where rownum=1)
            return Context.Sql(@"select * from CrCustomer where LevelSysNo not in (select sysNo from CrCustomerLevel where rownum=1)")// and rownum=1 and sysno=23449
                          .QueryMany<CrCustomer>();
        }

        /// <summary>
        /// 根据经销商筛选会员
        /// </summary>
        /// <param name="word">关键字</param>
        /// <param name="dealer">经销商id</param>
        /// <returns></returns>
        public override IList<CrCustomer> SearchCustomer(string word, int dealer)
        {
            return Context.Sql("Select * From CrCustomer Where (@0 is null or charindex(@0,Account)>0 or charindex(@0,Name)>0 or charindex(@0,NickName)>0 or charindex(@0,MobilePhoneNumber)>0) and (@1=-1 or DealerSysNo=@1)", word, dealer)
                          .QueryMany<CrCustomer>();
        }

        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="customer">会员信息</param>
        /// <returns>会员id</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        public override int CreateCustomer(CrCustomer customer)
        {
            if (customer.Birthday == DateTime.MinValue)
            {
                customer.Birthday = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (customer.LastLoginDate == DateTime.MinValue)
            {
                customer.LastLoginDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            customer.Subscribe = "0";
            var result = Context.Insert<CrCustomer>("CrCustomer", customer)
                                    .AutoMap(x => x.SysNo)
                                    .ExecuteReturnLastId<int>("SysNo");
            customer.SysNo = result;
            return result;
        }

        /// <summary>
        /// 创建收货地址
        /// </summary>
        /// <param name="address">收货地址</param>
        /// <returns>收货地址</returns>
        /// <remarks>2013－07-11 黄志勇 创建</remarks>
        public override CrReceiveAddress CreateCustomerReceiveAddress(CrReceiveAddress address)
        {
            address.SysNo = Context.Insert<CrReceiveAddress>("CrReceiveAddress", address)
                                   .AutoMap(x => x.SysNo)
                                   .ExecuteReturnLastId<int>("SysNo");
            return address;

        }

        /// <summary>
        /// 查询用户的收货地址
        /// </summary>
        /// <param name="customerSysNo">会员ID</param>
        /// <returns>收货地址列表</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        public override IList<CrReceiveAddress> LoadCustomerAddress(int customerSysNo)
        {
            return Context.Sql(@"select * from CrReceiveAddress where CustomerSysNo = @0", customerSysNo)
                          .QueryMany<CrReceiveAddress>();
        }

        /// <summary>
        /// 查询用户的默认收货地址
        /// </summary>
        /// <param name="customerSysNo">会员ID</param>
        /// <returns>收货地址列表</returns>
        /// <remarks>2014-03-18 周唐炬 创建</remarks>
        public override CrReceiveAddress LoadCustomerDefaultAddress(int customerSysNo)
        {
            return
                Context.Sql(@"select * from CrReceiveAddress where IsDefault=1 AND CustomerSysNo = @0",
                            customerSysNo).QuerySingle<CrReceiveAddress>();
        }

        /// <summary>
        /// 根据收货地址ID获取收货地址
        /// </summary>
        /// <param name="sysNo">地址</param>
        /// <returns>收货地址</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        public override CrReceiveAddress GetCustomerAddressBySysNo(int sysNo)
        {
            return Context.Sql(@"select * from CrReceiveAddress where SysNo = @0", sysNo)
                          .QuerySingle<CrReceiveAddress>();
        }

        /// <summary>
        /// 根据会员id获取默认收货地址
        /// </summary>
        /// <param name="customerSysNo">会员id</param>
        /// <returns>默认收货地址</returns>
        /// <remarks>2013－07-01 黄志勇 创建</remarks>
        public override CrReceiveAddress SearchReceiveAddressByCustomerSysNo(int customerSysNo)
        {
            return
                Context.Sql(@"select * from CrReceiveAddress where CustomerSysNo = @0 order by IsDefault desc",
                            customerSysNo)
                       .QuerySingle<CrReceiveAddress>();
        }

        /// <summary>
        /// 根据用户账号获取前台用户信息
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <returns>返回前台用户</returns>
        /// <remarks>2013-07-09 杨浩 创建</remarks>
        public override CBCrCustomer GetCrCustomer(string account)
        {
            return Context.Sql(@"select cc.*,ccl.LevelName,ccl.sysno as LevelSysNo from CrCustomer cc left join CrCustomerLevel ccl on cc.LevelSysNo=ccl.sysNo where cc.Account=@0", account)
                          .QuerySingle<CBCrCustomer>();
        }
        /// <summary>
        /// 查询所有的用户
        /// </summary>     
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2015-09-19 王耀发 创建
        /// 2016-04-11 刘伟豪 修改排序
        /// </remarks> 
        public override IList<CrCustomer> GetCrCustomerList()
        {
            return Context.Sql(@"select * from CrCustomer where Status=@Status Order By IsSellBusiness Desc,InviteTotal Desc,IndirectTotal Desc,RegisterDate Desc")
                          .Parameter("Status", (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效)
                          .QueryMany<CrCustomer>();
        }

        /// <summary>
        /// 执行分销商返利
        /// </summary>
        /// <param name="recommendSysNo">直接推荐人系统编号</param>
        /// <param name="complySysNo">消费客户系统编号</param>
        /// <param name="indirect2Id">间2推荐人系统编号</param>
        /// <param name="indirectId">间接推荐人系统编号</param>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <param name="action">0:注册 1:关注 2:购物</param>
        /// <param name="goodsAmount">商品总金额</param>
        /// <returns></returns>
        /// <remarks>2015－09-11 杨浩 创建 2015-10-28 王耀发引用</remarks>
        public override CrSellBusinessRebatesResult ExecuteSellBusinessRebates(int recommendSysNo, int complySysNo, int indirect2Id, int indirectId, int orderSysNo, string action, decimal goodsAmount)
        {
            var result = Context.StoredProcedure("pro_SellBusinessRebates")
               .Parameter("RecommendSysNo", recommendSysNo)
               .Parameter("ComplySysNo", complySysNo)
               .Parameter("Indirect2Id", indirect2Id)
               .Parameter("IndirectId", indirectId)
               .Parameter("OrderSysNo", orderSysNo)
               .Parameter("Action", action)
               .Parameter("GoodsAmount", goodsAmount)
               .QuerySingle<CrSellBusinessRebatesResult>();

            return result;
        }
        /// <summary>
        /// 更新会员对应 Brokerage，可提佣金 BrokerageFreeze，冻结佣金
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-8 王耀发引用</remarks>
        public override void UpdateCustomerValue(int SysNo, decimal Value)
        {
            Context.Sql("Update CrCustomer set Brokerage = Brokerage + @Value,BrokerageFreeze = BrokerageFreeze - @Value where SysNo=@SysNo")
                   .Parameter("Value", Value)
                   .Parameter("SysNo", SysNo).Execute();
        }
        /// <summary>
        /// 更新会员对应 BrokerageFreeze，冻结佣金
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-8 王耀发引用</remarks>
        public override void UpdateCustomerValueConfirm(int SysNo, decimal Value)
        {
            Context.Sql("Update CrCustomer set BrokerageFreeze = BrokerageFreeze - @Value where SysNo=@SysNo")
                   .Parameter("Value", Value)
                   .Parameter("SysNo", SysNo).Execute();
        }

        /// <summary>
        /// 获取分销商对应的会员
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        /// <remarks>2016-1-20 王耀发 创建</remarks>
        /// <remarks>2016-4-29 刘伟豪 添加关键字搜索</remarks>
        public override IList<CrCustomer> GetCrCustomerListByDealerSyNo(int DealerSysNo, string keyword = "")
        {
            return Context.Sql(@"Select * From CrCustomer 
                                 Where Status=@Status And DealerSysNo = @DealerSysNo 
                                       And (@keyword Is Null Or @keyword='' Or CHARINDEX(@keyword,Name) > 0 Or CHARINDEX(@keyword,NickName) > 0 Or CHARINDEX(@keyword,MobilePhoneNumber) > 0)
                                 Order By Subscribe Desc,Name Desc,RegisterDate Desc")
              .Parameter("Status", (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效)
              .Parameter("DealerSysNo", DealerSysNo)
              .Parameter("Keyword", keyword)
              .QueryMany<CrCustomer>();
        }

        public override List<CrCustomer> GetCrCustomerListByDealerSyNoQuery(int DealerSysNo)
        {
            return Context.Sql(@"select * from CrCustomer where Status=@Status and DealerSysNo = @DealerSysNo order by Subscribe desc,Name desc,RegisterDate Desc")
              .Parameter("Status", (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效)
              .Parameter("DealerSysNo", DealerSysNo)
              .QueryMany<CrCustomer>();
        }
        /// <summary>
        /// 判断分销商是否包含该会员
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-1-20 王耀发 创建</remarks>
        public override CrCustomer GetCustomerBySysNoDearler(int SysNo, int DealerSysNo)
        {
            return Context.Sql(@"select * from CrCustomer where SysNo=@0 and DealerSysNo=@1", SysNo, DealerSysNo)
                          .QuerySingle<CrCustomer>();
        }
        public override List<CBCrCustomer> SeachCanBeParentList(int id, string keyword = "")
        {
            var dataList = new List<CBCrCustomer>();

            var customer = GetModel(id);
            if (customer != null)
            {
                string sql = @"(Select 
                                 cc.*, ccl.LevelName,pc.Account PAccount,pc.Name PName,pc.NickName PNickName,pc.HeadImage PHeadImage
                            FROM 
                                 CrCustomer cc
                                 Left Join CrCustomerLevel ccl On cc.LevelSysNo=ccl.SysNo
                                 Left Join CrCustomer pc On cc.PSysNo=pc.SysNo
                            WHERE 
                                 (cc.SysNo <> @0)
                                 And (CHARINDEX(@1,cc.CustomerSysNos) = 0)
                                 And (cc.SysNo <> @2)
                                 And (cc.DealerSysNo=@3)
                                 And (cc.Status=@4)
                                 And (@4 Is Null Or @5='' Or CHARINDEX(@5,cc.Name) > 0 Or CHARINDEX(@5,cc.NickName) > 0 Or CHARINDEX(@5,cc.MobilePhoneNumber) > 0)
                                 And (cc.IsSellBusiness=1)
                                 ) tb";

                var paras = new object[]
                {
                    customer.SysNo,
                    ","+customer.SysNo+",",
                    customer.PSysNo,
                    customer.DealerSysNo,
                    (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                    keyword,
                };
                dataList = Context.Select<CBCrCustomer>("tb.*").From(sql).Parameters(paras).OrderBy(@"Subscribe Desc,Name Desc,RegisterDate Desc").QueryMany();
            }

            return dataList;
        }

        public override bool CustomerToParent(int cSysNo, int pSysNo)
        {
            var customer = GetModel(cSysNo);
            var parent = GetModel(pSysNo);

            if (customer == null || parent == null)
                return false;

            using (var tran = new System.Transactions.TransactionScope())
            {
                if (customer.PSysNo > 0)
                {
                    Context.Sql(@"Update CrCustomer Set PSysNo=@0 Where SysNo=@1
                                  Update CrCustomer Set CustomerSysNos=REPLACE(CustomerSysNos,SUBSTRING(CustomerSysNos,1,CHARINDEX(@2,CustomerSysNos)),@3) Where CHARINDEX(@4,CustomerSysNos) > 0
                                  ", pSysNo
                                   , cSysNo
                                   , "," + cSysNo + ","
                                   , parent.CustomerSysNos
                                   , "," + cSysNo + ",")
                           .Execute();
                }
                else
                {
                    Context.Sql(@"Update CrCustomer Set PSysNo=@0 Where SysNo=@1
                                  Update CrCustomer Set CustomerSysNos = @2 + SUBSTRING( CustomerSysNos,2 ,len( CustomerSysNos)-1 ) Where CHARINDEX(@3,CustomerSysNos) > 0
                                  ", pSysNo
                                   , cSysNo
                                   , parent.CustomerSysNos
                                   , "," + cSysNo + ",")
                           .Execute();
                }

                UpdateInviteAndIndirectNum(customer.PSysNo);
                UpdateInviteAndIndirectNum(pSysNo);

                var op = GetModel(customer.PSysNo);
                if (op != null)
                    UpdateInviteAndIndirectNum(op.PSysNo);
                var pp = GetModel(pSysNo);
                if (pp != null)
                    UpdateInviteAndIndirectNum(pp.PSysNo);


                tran.Complete();
            }
            return true;
        }

        public override bool UpdateInviteAndIndirectNum(int customerSysNo)
        {
            var total = Context.Sql(@"Select Count(0)-1 From CrCustomer Where CHARINDEX(@0,CustomerSysNos) > 0", "," + customerSysNo + ",").QuerySingle<int>();
            var inviteTotal = Context.Sql(@"Select Count(0) From CrCustomer Where PSysNo=@0", customerSysNo).QuerySingle<int>();
            var indirectTotal = total - inviteTotal;

            int r = Context.Sql("Update CrCustomer set InviteTotal = @InviteTotal,IndirectTotal = @IndirectTotal where SysNo=@SysNo")
                    .Parameter("InviteTotal", inviteTotal)
                    .Parameter("IndirectTotal", indirectTotal)
                    .Parameter("SysNo", customerSysNo).Execute();

            return r > 0;
        }

        /// <summary>
        /// 更新是否为分销商
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="IsSellBusiness"></param>
        /// <remarks>2016-5-16 王耀发 创建</remarks>
        public override void UpdateIsSellBusiness(int SysNo, string IsSellBusiness)
        {
            Context.Update("CrCustomer").Column("IsSellBusiness", IsSellBusiness).Where("SysNo", SysNo).Execute();
        }

        /// <summary>
        /// 更新会员分销商等级
        /// </summary>
        /// <param name="id">会员id</param>
        /// <param name="gid">等级id</param>
        /// <returns></returns>
        public override bool UpdateSellBusinessGrade(int id, int gid)
        {
            var r = 0;
            var IsSellBusiness = gid > 0 ? "1" : "0";

            if (Context.Sql("Select Count(0) From CrSellBusinessGrade Where SysNo=@gid").Parameter("gid", gid).QuerySingle<int>() == 0)
                return false;

            using (var tran = new System.Transactions.TransactionScope())
            {
                try
                {
                    r = Context.Update("CrCustomer").Column("SellBusinessGradeId", gid).Where("SysNo", id).Execute();
                    UpdateIsSellBusiness(id, IsSellBusiness);
                    tran.Complete();
                }
                catch { r = 0; }
            }

            return r > 0;
        }

        /// <summary>
        /// 获取所有会员信息
        /// </summary>
        /// <returns>会员信息集合</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override IList<CrCustomer> GetAllCustomer()
        {
            const string strSql = @"select * from CrCustomer where Status = 1";
            var entity = Context.Sql(strSql)
                                .QueryMany<CrCustomer>();
            return entity;
        }
        /// <summary>
        /// 新增会员信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void CreateCrCustomer(List<CrCustomer> models)
        {
            foreach (CrCustomer model in models)
            {
                Context.Insert<CrCustomer>("CrCustomer", model)
                                        .AutoMap(x => x.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            }
        }

         /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="models">会员信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void UpdateExcelCrCustomer(List<CrCustomer> models)
        {
            foreach (CrCustomer model in models)
            {
                CBCrCustomer crdata = GetCrCustomer(model.Account);
                int SysNo = crdata.SysNo;
                Context.Update("CrCustomer")
                .Column("Name", model.Name)
                .Column("NickName", model.NickName)
                .Where("SysNo", SysNo)
                .Execute();
            }
        }
        /// <summary>
        /// 更新用户冻结佣金
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="amount">金额</param>
        /// <returns></returns>
        /// <remarks>2017-1-10 杨浩 创建</remarks>
        public override bool UpdateCustomerBrokerageFreeze(int customerSysNo, decimal amount)
        {
            string strSql = string.Format("update CrCustomer set BrokerageFreeze=BrokerageFreeze-{1} where sysNo = {0}", customerSysNo,amount);
            return Context.Sql(strSql)
                                .Execute()>0;
        }
    }
}