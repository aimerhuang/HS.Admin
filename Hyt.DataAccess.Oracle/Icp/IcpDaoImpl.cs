using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Icp;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Icp
{
    /// <summary>
    /// 取商检数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class IcpDaoImpl : IcpDao
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override Pager<CIcp> GoodsQuery(ParaIcpGoodsFilter filter)
        {
            const string sql = @"(select * from Icp
                                where 
                                (@0 is null or IcpType = @0) and
                                (@1 is null or MessageType = @1) and 
                                SourceSysNo = 0 and       
                                 (@2 is null or convert(nvarchar(50),SysNo) = @2 or MessageID = @2)
                                                                                                 
                                ) tb";

            var paras = new object[]
                {
                    filter.IcpType,
                    filter.MessageType,
                    filter.Condition
                };

            var dataList = Context.Select<CIcp>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CIcp>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override Pager<CIcp> OrderQuery(ParaIcpGoodsFilter filter)
        {
            const string sql = @"(select * from Icp
                                where 
                                (@0 is null or IcpType = @0) and
                                (@1 is null or MessageType = @1) and 
                                SourceSysNo <> 0 and         
                                (@2 is null or convert(nvarchar(50),SysNo) = @2 or convert(nvarchar(50),SourceSysNo) = @2 or MessageID = @2)
                                                                                                           
                                ) tb";

            var paras = new object[]
                {
                    filter.IcpType,
                    filter.MessageType,
                    filter.Condition
                };

            var dataList = Context.Select<CIcp>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CIcp>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 获取明细列表
        /// </summary>
        /// <param name="IcpGoodsSysNo">系统编号</param>
        /// <returns>明细列表</returns>
        /// <remarks>2016-03-24  王耀发 创建</remarks>
        public override Pager<CBIcpGoodsItem> IcpGoodsItemQuery(ParaIcpGoodsFilter filter)
        {
            const string sql = @"(select gi.SysNo,gi.IcpType, pd.ErpCode,pd.ProductName,i.MessageType,gi.MessageID,gi.CreatedDate,
                                 case isnull(i.PlatStatus,'') when '' then '待审核' when 10 then '通过' when 20 then '不通过' END as PlatStatus,case isnull(i.CiqStatus,'') when '' then '待审核' when 10 then '通过' when 20 then '不通过' END as CiqStatus
                                from
                                (
                                select * from IcpGoodsItem where IcpGoodsSysNo in (select SysNo from Icp where [Status] <> -1 and MessageType = '661105')
                                ) gi left join Icp i on gi.IcpGoodsSysNo = i.SysNo
                                left join PdProduct pd on gi.ProductSysNo = pd.SysNo
                                where 
                                (@0 is null or gi.IcpType = @0) and 
                                (@1 is null or pd.ErpCode = @1)
                                ) tb";
            var paras = new object[]
                {
                    filter.IcpType,
                    filter.Condition
                };

            var dataList = Context.Select<CBIcpGoodsItem>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBIcpGoodsItem>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2016-03-23  王耀发 创建</remarks>
        public override CIcp GetEntity(int sysNo)
        {

            return Context.Sql("select * from Icp where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<CIcp>();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="MessageID">编号</param>
        /// <param name="MessageType">类型</param>
        /// <returns>数据实体</returns>
        /// <remarks>2016-03-23  王耀发 创建</remarks>
        public override CIcp GetEntityByMessageIDType(string MessageType, string MessageID)
        {

            return Context.Sql("select * from Icp where MessageID=@MessageID and MessageType = @MessageType")
                   .Parameter("MessageID", MessageID)
                   .Parameter("MessageType", MessageType)
              .QuerySingle<CIcp>();
        }

       /// <summary>
       /// 获取数据
       /// </summary>
       /// <param name="MessageType">报文类型</param>
       /// <returns></returns>
        public override CIcp GetEntityByMType(string IcpType,string MessageType)
        {
            string sql = @"(select top 1 * from Icp where IcpType =" + IcpType + " and MessageType = '" + MessageType + "' order by SerialNumber desc) as t";
            CIcp entity = Context.Select<CIcp>("*")
            .From(sql)
            .QuerySingle();

            return entity;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override int Insert(CIcp entity)
        {
            entity.SysNo = Context.Insert("Icp", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override void Update(CIcp entity)
        {
            Context.Update("Icp", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取明细列表
        /// </summary>
        /// <param name="IcpGoodsSysNo">系统编号</param>
        /// <returns>明细列表</returns>
        /// <remarks>2016-03-24  王耀发 创建</remarks>
        public override List<CBIcpGoodsItem> GetListByIcpGoodsSysNo(int IcpGoodsSysNo)
        {
            return Context.Sql("select g.*,p.ErpCode,p.ProductName from IcpGoodsItem g left join PdProduct p on g.ProductSysNo = p.SysNo where g.IcpGoodsSysNo=@IcpGoodsSysNo")
                   .Parameter("IcpGoodsSysNo", IcpGoodsSysNo)
                  .QueryMany<CBIcpGoodsItem>();
        }

        /// <summary>
        /// 删除商检明细数据
        /// </summary>
        /// <param name="IcpGoodsSysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2016-03-24  王耀发 创建</remarks>
        public override void DeleteIcpGoodsItem(int IcpGoodsSysNo)
        {
            Context.Sql("Delete from IcpGoodsItem where IcpGoodsSysNo=@IcpGoodsSysNo")
                 .Parameter("IcpGoodsSysNo", IcpGoodsSysNo)
            .Execute();
        }

        /// <summary>
        /// 插入商检明细数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override int InsertIcpGoodsItem(CIcpGoodsItem entity)
        {
            entity.SysNo = Context.Insert("IcpGoodsItem", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新商检明细数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override void UpdateIcpGoodsItem(CIcpGoodsItem entity)
        {
            Context.Update("IcpGoodsItem", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override Pager<CBIcpGoodsItem> GetIcpProductList(ParaIcpGoodsItemFilter filter)
        {
            string where = " 1=1 ";
            switch (filter.IcpType)
            {
                case (int)Model.CommonEnum.商检.广州白云机场:
                    where += " and exists(select * from IcpBYJiChangGoodsInfo where ProductSysNo = p.SysNo) ";
                    break;
                case (int)Model.CommonEnum.商检.广州南沙:
                    where += "and exists(select * from IcpGZNanShaGoodsInfo where ProductSysNo = p.SysNo)";
                    break;
                default:
                    break;
            }
            if (filter.name != null)
            {
                where += " and (p.ErpCode like '%" + filter.name + "%' or p.ProductName like '%" + filter.name + "%')";
            }
            string sql = @"(select p.SysNo,p.ErpCode,p.ProductName from PdProduct p
                            where " + where + " and not exists(select * from IcpGoodsItem where ProductSysNo = p.SysNo and IcpGoodsSysNo in (select SysNo from Icp where SourceSysNo = 0 and [Status] <> -1 and IcpType = @0))) tb";
                         
            var paras = new object[]
                {
                    filter.IcpType
                };

            var dataList = Context.Select<CBIcpGoodsItem>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBIcpGoodsItem>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.SysNo desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 更新接收回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="DocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdateIcpDocRec(int SysNo, string DocRec)
        {
            Context.Update("Icp").Column("DocRec", DocRec).Where("SysNo", SysNo).Execute();
        }

        /// <summary>
        /// 更新单一窗口平台接收回执
        /// </summary>
        /// <param name="MessageID">消息ID</param>
        /// <param name="PlatDocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdatePlatDocRecByMessageID(string MessageID, string PlatDocRec, string PlatStatus)
        {
            Context.Update("Icp")
                .Column("PlatDocRec", PlatDocRec)
                .Column("PlatStatus", PlatStatus)
                .Where("MessageID", MessageID).Execute();

        }
        /// <summary>
        /// 更新商检接收回执
        /// </summary>
        /// <param name="MessageID">消息ID</param>
        /// <param name="CiqDocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdateCiqDocRecByMessageID(string MessageID, string CiqDocRec, string CiqStatus)
        {
            Context.Update("Icp")
                .Column("CiqDocRec", CiqDocRec)
                .Column("CiqStatus", CiqStatus)
                .Where("MessageID", MessageID).Execute();
        }
        /// <summary>
        /// 更新商检接收状态
        /// </summary>
        /// <param name="MessageID">消息ID</param>
        /// <param name="CiqDocRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdateStatus(string MessageID, int Status)
        {
            Context.Update("Icp")
                .Column("Status", Status)
                .Where("MessageID", MessageID).Execute();

        }

        /// <summary>
        /// 更新国检审核回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="CiqRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdateIcpCiqRec(int SysNo, string CiqRec)
        {
            Context.Update("Icp").Column("CiqRec", CiqRec).Where("SysNo", SysNo).Execute();
        }

        /// <summary>
        /// 更新国检审核回执
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <param name="CIQNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdateIcpGoodsItemCIQ(int IcpType,string EntGoodsNo, string CIQGRegStatus, string CIQNotes)
        {
            Context.Update("IcpGoodsItem")
                .Column("CIQGRegStatus", CIQGRegStatus)
                .Column("CIQNotes", CIQNotes)
                .Where("IcpType", IcpType)
                .Where("EntGoodsNo", EntGoodsNo).Execute();
        }

        /// <summary>
        /// 更新海关审核回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="CusRec">回执信息</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdateIcpCusRec(int SysNo, string CusRec)
        {
            Context.Update("Icp").Column("CusRec", CusRec).Where("SysNo", SysNo).Execute();
        }
        /// <summary>
        /// 更新海关审核回执
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="OpResult">状态</param>
        /// <param name="CustomsNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdateIcpGoodsItemCUS(int IcpType, string EntGoodsNo, string OpResult, string CustomsNotes)
        {
            Context.Update("IcpGoodsItem")
                .Column("OpResult", OpResult)
                .Column("CustomsNotes", CustomsNotes)
                .Where("IcpType", IcpType)
                .Where("EntGoodsNo", EntGoodsNo).Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public override IcpBYJiChangGoodsInfo GetIcpBYJiChangGoodsInfoEntityByPid(int ProductSysNo)
        {

            return Context.Sql("select a.* from IcpBYJiChangGoodsInfo a where a.ProductSysNo=@ProductSysNo")
                   .Parameter("ProductSysNo", ProductSysNo)
              .QuerySingle<IcpBYJiChangGoodsInfo>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public override int InsertIcpBYJiChangGoodsInfo(IcpBYJiChangGoodsInfo entity)
        {
            entity.SysNo = Context.Insert("IcpBYJiChangGoodsInfo", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 更新</remarks>
        public override int UpdateIcpBYJiChangGoodsInfoEntity(IcpBYJiChangGoodsInfo entity)
        {

            return Context.Update("IcpBYJiChangGoodsInfo", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public override IcpGZNanShaGoodsInfo GetIcpGZNanShaGoodsInfoEntityByPid(int ProductSysNo)
        {

            return Context.Sql("select a.* from IcpGZNanShaGoodsInfo a where a.ProductSysNo=@ProductSysNo")
                   .Parameter("ProductSysNo", ProductSysNo)
              .QuerySingle<IcpGZNanShaGoodsInfo>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public override int InsertIcpGZNanShaGoodsInfo(IcpGZNanShaGoodsInfo entity)
        {
            entity.SysNo = Context.Insert("IcpGZNanShaGoodsInfo", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 更新</remarks>
        public override int UpdateIcpGZNanShaGoodsInfoEntity(IcpGZNanShaGoodsInfo entity)
        {

            return Context.Update("IcpGZNanShaGoodsInfo", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 更新检验检疫商品备案编号
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <param name="CIQNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdateCIQGoodsNo(string EntGoodsNo, string CIQGoodsNo)
        {
            Context.Update("IcpBYJiChangGoodsInfo")
                .Column("CIQGoodsNo", CIQGoodsNo)
                .Where("EntGoodsNo", EntGoodsNo).Execute();
        }

        /// <summary>
        /// 更新检验检疫商品备案编号
        /// </summary>
        /// <param name="EntGoodsNo">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <param name="CIQNotes">说明</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdateCusGoodsNo(string EntGoodsNo, string CusGoodsNo)
        {
            Context.Update("IcpBYJiChangGoodsInfo")
                .Column("CusGoodsNo", CusGoodsNo)
                .Where("EntGoodsNo", EntGoodsNo).Execute();
        }

        /// <summary>
        /// 更新南沙检验检疫商品备案编号
        /// </summary>
        /// <param name="Gcode">商品自编号</param>
        /// <param name="CIQGRegStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override void UpdateNSCIQGoodsNo(string Gcode, string CIQGoodsNo)
        {
            Context.Update("IcpGZNanShaGoodsInfo")
                .Column("CIQGoodsNo", CIQGoodsNo)
                .Where("Gcode", Gcode).Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        /// <remarkss>2016-04-05 王耀发 创建</remarks>
        public override void UpdateEntGoodsNoByMessageID(string MessageID, string EntGoods)
        {
            Context.Sql("update IcpGoodsItem set EntGoodsNo = @EntGoods where MessageID = @MessageID")
                   .Parameter("EntGoods", EntGoods)
                   .Parameter("MessageID", MessageID)
              .Execute();
        }

        /// <summary>
        /// 获取所有商品备案信息
        /// </summary>
        /// <returns>商品备案信息集合</returns>
        /// <remarks>2015-12-15 王耀发 创建</remarks>
        public override IList<IcpGZNanShaGoodsInfo> GetAllGZNanShaGoodsInfo()
        {
            const string strSql = @"select * from IcpGZNanShaGoodsInfo";
            var entity = Context.Sql(strSql)
                                .QueryMany<IcpGZNanShaGoodsInfo>();
            return entity;
        }
        /// <summary>
        /// 新增商品备案信息
        /// </summary>
        /// <param name="models">商品备案信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void CreateExcelGZNanShaGoodsInfo(List<IcpGZNanShaGoodsInfo> models)
        {
            foreach (IcpGZNanShaGoodsInfo model in models)
            {
                IcpGZNanShaGoodsInfo GoodsInfoData = new IcpGZNanShaGoodsInfo();
                GoodsInfoData.ProductSysNo = model.ProductSysNo;

                GoodsInfoData.Gcode = model.Gcode;
                GoodsInfoData.Gname = model.Gname;
                GoodsInfoData.Spec = model.Spec;
                GoodsInfoData.HSCode = model.HSCode;
                GoodsInfoData.Unit = model.Unit;
                GoodsInfoData.Brand = model.Brand;
                GoodsInfoData.AssemCountry = model.AssemCountry;

                GoodsInfoData.SellWebSite = model.SellWebSite;
                GoodsInfoData.GoodsBarcode = model.GoodsBarcode;
                GoodsInfoData.GoodsDesc = model.GoodsDesc;
                GoodsInfoData.ComName = model.ComName;
                GoodsInfoData.Ingredient = model.Ingredient;
                GoodsInfoData.Additiveflag = model.Additiveflag;
                GoodsInfoData.Poisonflag = model.Poisonflag;
                GoodsInfoData.Remark = model.Remark;

                GoodsInfoData.CreatedBy = model.CreatedBy;
                GoodsInfoData.CreatedDate = model.CreatedDate;
                GoodsInfoData.LastUpdateBy = model.LastUpdateBy;
                GoodsInfoData.LastUpdateDate = model.LastUpdateDate;
                int InfoSysNo = Context.Insert<IcpGZNanShaGoodsInfo>("IcpGZNanShaGoodsInfo", GoodsInfoData)
                                       .AutoMap(o => o.SysNo)
                                       .ExecuteReturnLastId<int>("SysNo");
            }
        }

        /// <summary>
        /// 更新商品备案信息
        /// </summary>
        /// <param name="models">商品备案信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void UpdateExcelGZNanShaGoodsInfo(List<IcpGZNanShaGoodsInfo> models)
        {
            foreach (IcpGZNanShaGoodsInfo model in models)
            {
                string sql = @"update IcpGZNanShaGoodsInfo set Gcode = @Gcode,Gname = @Gname,Spec = @Spec,HSCode = @HSCode
                              ,Unit = @Unit,Brand = @Brand,AssemCountry = @AssemCountry,SellWebSite = @SellWebSite
                              ,GoodsBarcode = @GoodsBarcode,GoodsDesc = @GoodsDesc,ComName = @ComName,Ingredient = @Ingredient
                              ,Additiveflag = @Additiveflag,Poisonflag = @Poisonflag,Remark = @Remark
                              where ProductSysNo = @ProductSysNo";
                Context.Sql(sql)
                .Parameter("ProductSysNo", model.ProductSysNo)
                .Parameter("Gcode", model.Gcode)
                .Parameter("Gname", model.Gname)
                .Parameter("Spec", model.Spec)
                .Parameter("HSCode", model.HSCode)
                .Parameter("Unit", model.Unit)
                .Parameter("Brand", model.Brand)
                .Parameter("AssemCountry", model.AssemCountry)
                .Parameter("SellWebSite", model.SellWebSite)
                .Parameter("GoodsBarcode", model.GoodsBarcode)
                .Parameter("GoodsDesc", model.GoodsDesc)
                .Parameter("ComName", model.ComName)
                .Parameter("Ingredient", model.Ingredient)
                .Parameter("Additiveflag", model.Additiveflag)
                .Parameter("Poisonflag", model.Poisonflag)
                .Parameter("Remark", model.Remark).Execute();
            }
        }

        /// <summary>
        /// 根据商品ID获取启邦商品备案信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-12-13 周 创建</remarks>
        public override IcpQiBangGoodsInfo GetIcpQiBangGoodsInfoEntityByPid(int ProductSysNo)
        {

            return Context.Sql("select * from IcpQiBangGoodsInfo where ProductSysNo=@ProductSysNo")
                   .Parameter("ProductSysNo", ProductSysNo)
              .QuerySingle<IcpQiBangGoodsInfo>();
        }
        /// <summary>
        /// 新增启邦商品备案信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-12-13 周 创建</remarks>
        public override int InsertIcpBYJiChangGoodsInfo(IcpQiBangGoodsInfo entity)
        {
            entity.SysNo = Context.Insert("IcpQiBangGoodsInfo", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }
        /// <summary>
        /// 更新启邦商品备案信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2016-12-13 周 创建</remarks>
        public override int UpdateIcpBYJiChangGoodsInfoEntity(IcpQiBangGoodsInfo entity)
        {
            return Context.Update("IcpQiBangGoodsInfo", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }
    }
}
