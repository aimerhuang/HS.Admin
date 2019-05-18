using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.BaseInfo;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Basic
{
    /// <summary>
    /// 地区信息数据访问实现
    /// </summary>
    /// <remarks>
    /// 2013-06-09 朱成果 创建
    /// </remarks>
    public class BasicAreaDaoImpl : IBsAreaDao
    {
        #region 操作

        /// <summary>
        /// 创建地区信息
        /// </summary>
        /// <param name="model">地区信息实体</param>
        /// <returns>创建的地区信息sysNo</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int Create(BsArea model)
        {
            if (model.LastUpdateDate == DateTime.MinValue)
            {
                model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            return Context.Insert("BsArea", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }
        public override List<BsArea> GetAllCity()
        {
            return Context.Sql("select * from BsArea where AreaLevel = 2")
                         .QueryMany<BsArea>();
        }
        /// <summary>
        /// 更新地区信息
        /// </summary>
        /// <param name="model">地区信息实体，根据sysno</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int Update(BsArea model)
        {
            return Context.Update("BsArea", model)
                          .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 删除地区信息
        /// </summary>
        /// <param name="sysNo">要删除的地区信息系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("BsArea")
                          .Where("SysNo", sysNo)
                          .Execute();
        }

        #endregion

        #region 查询

        /// <summary>
        /// 根据地区名称模糊递归出地区列表(指定参数可递归到顶级父节点、默认只递归子节点)
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <param name="getParent">是否获取父级节点</param>
        /// <returns>地区列表</returns>
        /// <remarks>2013-12-16 周唐炬 创建</remarks>
        public override List<BsArea> QueryRecursive(string name, bool getParent)
        {
            var sb = new StringBuilder();
            sb.Append(@"select distinct a.*
                                  from bsarea a
                                 start with charindex(a.areaname, @0) > 0");
            sb.Append(getParent
                          ? @"connect by prior a.parentsysno = a.sysno"
                          : @"connect by prior a.sysno = a.parentsysno");

            return Context.Sql(sb.ToString(), name)
                          .QueryMany<BsArea>();
        }

        /// <summary>
        /// 获取最大的显示顺序
        /// </summary>
        /// <returns>最大显示顺序，如果为空则显示为0</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int GetMaxDisplayOrder()
        {
            const string sql = @"select isnull(max(displayorder),0) from BsArea t  ";

            return Context.Sql(sql)
                          .QuerySingle<int>();
        }

        /// <summary>
        /// 获取地区模型
        /// </summary>
        /// <param name="sysNo">地区系统号</param>
        /// <returns>单个地区数据</returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override CBBsArea2 GetCbArea(int sysNo)
        {
            const string sql = @"select t.*,nvl(a.areaname,'无') parentname from BsArea t 
                                 left join BsArea a on t.parentsysno=a.sysno where t.sysno=@0";

            return Context.Sql(sql, sysNo)
                          .QuerySingle<CBBsArea2>();
        }

        /// <summary>
        /// 获取地区模型
        /// </summary>
        /// <param name="sysNo">地区系统号</param>
        /// <returns>单个地区数据</returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override BsArea GetArea(int sysNo)
        {
            return Context.Sql("select * from BsArea where sysno=@0", sysNo)
                          .QuerySingle<BsArea>();
        }

        /// <summary>
        /// 根据地区名称获取地区列表,模糊查询
        /// </summary>
        /// <param name="areaName">地区名称</param>
        /// <returns>地区列表</returns>
        /// <remarks>
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        public override IList<BsArea> GetArea(string areaName)
        {
            return Context.Sql("select * from BsArea where charindex(areaName,@0)>0", areaName)
                          .QueryMany<BsArea>();
            //return Context.Sql("select * from BsArea where AreaName=@0", areaName)
            //              .QueryMany<BsArea>();
        }

        /// <summary>
        /// 获取省
        /// </summary>
        /// <returns>省列表</returns>
        /// <remarks>
        /// 2013-06-08 朱成果 创建
        /// </remarks>
        public override IList<BsArea> LoadProvince()
        {
            return Context.Sql("select * from BsArea where Status=1 and AreaLevel=1 order by areaname Collate Chinese_PRC_Stroke_ci_as").QueryMany<BsArea>();
        }

        /// <summary>
        /// 获取市
        /// </summary>
        /// <param name="provinceSysNo">省编号</param>
        /// <returns>市列表</returns>
        /// <remarks>
        /// 2013-06-08 朱成果 创建
        /// </remarks>
        public override IList<BsArea> LoadCity(int provinceSysNo)
        {
            return
                Context.Sql(
                    "select * from BsArea where Status=1 and AreaLevel=2  and parentsysno=@0 order by areaname Collate Chinese_PRC_Stroke_ci_as",
                    provinceSysNo).QueryMany<BsArea>();
        }

        /// <summary>
        /// 获取区
        /// </summary>
        /// <param name="citySysNo">城市编号</param>
        /// <returns>地区列表</returns>
        /// <remarks>
        /// 2013-06-08 朱成果 创建
        /// </remarks>
        public override IList<BsArea> LoadArea(int citySysNo)
        {
            return
                Context.Sql(
                    "select * from BsArea where Status=1 and AreaLevel=3  and parentsysno=@0 order by areaname Collate Chinese_PRC_Stroke_ci_as",
                    citySysNo).QueryMany<BsArea>();
        }

        /// <summary>
        /// 获取省市区数据
        /// </summary>
        /// <param name="parentSysNo">地区父级系统号</param>
        /// <returns>省市区数据</returns>
        /// <remarks>
        /// 2013-06-13 杨晗 创建
        /// </remarks>
        public override IList<BsArea> SelectArea(int parentSysNo)
        {
            const int status = (int)BasicStatus.地区状态.有效;
            if (parentSysNo == 0)
            {
                return
                    Context.Sql(
                        "select * from BsArea where (parentsysno is null or parentsysno=0) and status=@0  order by areaname Collate Chinese_PRC_Stroke_ci_as", status)
                           .QueryMany<BsArea>();
            }
            return
                Context.Sql("select * from BsArea where parentsysno=@0 and status=@1 order by areaname Collate Chinese_PRC_Stroke_ci_as",
                            parentSysNo, status).QueryMany<BsArea>();
        }

        /// <summary>
        /// 查询下级地区，排除没有仓库的地区
        /// </summary>
        /// <param name="parentSysNo">上级地区编号</param>
        ///  <param name="warehouseType">仓库类型</param>
        /// <param name="deliveryTypeSysNo">配送方式编号</param>
        /// <returns>省市区数据</returns>
        /// <remarks> 2013-07-02 朱成果 创建 </remarks>
        /// <remarks> 2013-10-08 黄志勇 修改 </remarks>
        public override IList<BsArea> SelectAreaWithWarehouse(int parentSysNo, int? warehouseType, int? deliveryTypeSysNo)
        {
            string sql = string.Format(@"with tb1 as
                            (
                                select  WhWarehouse.sysno as WarehouseNo,AreaSysNo,bsarea.Areaname as AreaName,bsarea.parentsysno as CityNo
                                from WhWarehouse
                                inner join bsarea 
                                on bsarea.sysno=WhWarehouse.AreaSysNo
                                inner join WhWarehouseDeliveryType
                                on WhWarehouseDeliveryType.WarehouseSysNo = WhWarehouse.SysNo
                                where WhWarehouse.Status=1 {0}
                            )
                            select  distinct bsarea.*,bsarea.Areaname Collate Chinese_PRC_Stroke_ci_as from bsarea
                            inner join
                            (
                                select tb1.*,bsarea.Areaname as CityName,bsarea.parentsysno as ProvinceNo,pbsarea.parentsysno as CountryNo
                                from tb1 left outer join bsarea
                                on tb1.CityNo=bsarea.sysNo
								left outer join bsarea pbsarea
								on pbsarea.sysNo = bsarea.parentsysno
                            ) b
                            on  bsarea.sysno in(b.CityNo,b.AreaSysNo,b.ProvinceNo,b.CountryNo) ",
                            (deliveryTypeSysNo.HasValue ? "and WhWarehouseDeliveryType.DeliveryTypeSysNo=" + deliveryTypeSysNo.Value.ToString() : string.Empty));

            if (parentSysNo == 0)
            {
                return
                    Context.Sql(sql + " where parentsysno is null or parentsysno=0  order by bsarea.Areaname Collate Chinese_PRC_Stroke_ci_as")
                           .QueryMany<BsArea>();

            }
            return
                Context.Sql(sql + " where parentsysno=@0 order by bsarea.Areaname Collate Chinese_PRC_Stroke_ci_as", parentSysNo)
                       .QueryMany<BsArea>();
        }

        /// <summary>
        /// 获取所有地区,用于构建地区树
        /// </summary>
        /// <param></param>
        /// <returns>所有地区</returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public override IList<BsArea> GetAllAreaForTree()
        {
            return Context.Sql(@"select a.sysno ,a.parentsysno ,a.areaname 
                                from bsarea a
                                where a.arealevel = 3
                                union all
                                select a.sysno,a.parentsysno,a.areaname
                                from bsarea a
                                where a.arealevel = 2
                                union all
                                select a.sysno,a.parentsysno,a.areaname 
                                from bsarea a
                                where a.arealevel = 1")
                          .QueryMany<BsArea>();
        }

        /// <summary>
        /// 获取仓库的覆盖地区
        /// </summary>
        /// <param name="warehouseSysNo">创编系统编号</param>
        /// <returns>仓库的覆盖地区</returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public override IList<CBBsArea2> GetAreaByWarehouse(int warehouseSysNo)
        {
            return Context.Sql(@"
                        select distinct a.isdefault,b.* from whwarehousearea a inner join bsarea b
                        on a.areasysno = b.sysno
                        where a.warehousesysno = @sysno 
                        order by b.areaname Collate Chinese_PRC_Stroke_ci_as")
                       .Parameter("sysno", warehouseSysNo)

                          .QueryMany<CBBsArea2>();
        }

        /// <summary>
        /// 修改地区状态
        /// </summary>
        /// <param name="area">地区实体</param>
        /// <returns>返回受影响行数</returns>
        /// <remarks>2013-08-16 周瑜 创建</remarks>
        public override int UpdateStatus(BsArea area)
        {
            return Context.Sql("update bsarea set Status = @Status, LastUpdateBy = @LastUpdateBy, LastUpdateDate = @LastUpdateDate where SysNo = @SysNo")
                          .Parameter("Status", area.Status)
                          .Parameter("LastUpdateBy", area.LastUpdateBy)
                          .Parameter("LastUpdateDate", area.LastUpdateDate)
                          .Parameter("SysNo", area.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 获取地区模型
        /// </summary>
        /// <param name="sysNo">地区系统号</param>
        /// <param name="status">地区状态</param>
        /// <returns>单个地区数据</returns>
        /// <remarks>
        /// 2013-06-13 杨晗 创建
        /// </remarks>
        public override BsArea GetArea(int sysNo, BasicStatus.地区状态 status)
        {
            IList<BsArea> list =
                Context.Sql("select * from BsArea where sysno=@0 and status=@1 order by areaname Collate Chinese_PRC_Stroke_ci_as", sysNo, (int)status)
                       .QueryMany<BsArea>();
            if (list != null && list.Any())
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// 根据地区编号，获取省市区信息
        /// </summary>
        /// <param name="sysNo">地区编号</param>
        /// <param name="cityEntity">城市信息</param>
        /// <param name="areaEntity">地区信息</param>
        /// <returns>获取省市区信息</returns>
        /// <remarks>
        /// 2013-06-14 朱成果 创建
        /// </remarks>
        /// 
        public override BsArea GetProvinceEntity(int sysNo, out BsArea cityEntity, out BsArea areaEntity)
        {
            cityEntity = null;
            areaEntity = null;
            BsArea provinceEntity = null;
            BsArea model = GetArea(sysNo);
            //地区信息不存在
            while (model != null && model.AreaLevel >= 1)
            {
                switch (model.AreaLevel)
                {
                    //省
                    case 1:
                        provinceEntity = new BsArea
                            {
                                AreaCode = model.AreaCode,
                                NameAcronym = model.NameAcronym,
                                DisplayOrder = model.DisplayOrder,
                                ParentSysNo = model.ParentSysNo,
                                AreaName = model.AreaName,
                                AreaLevel = model.AreaLevel,
                                SysNo = model.SysNo
                            };
                        break;
                    //市
                    case 2:
                        cityEntity = new BsArea
                            {
                                AreaCode = model.AreaCode,
                                NameAcronym = model.NameAcronym,
                                DisplayOrder = model.DisplayOrder,
                                ParentSysNo = model.ParentSysNo,
                                AreaName = model.AreaName,
                                AreaLevel = model.AreaLevel,
                                SysNo = model.SysNo
                            };
                        break;
                    //区
                    case 3:
                        areaEntity = new BsArea
                            {
                                AreaCode = model.AreaCode,
                                NameAcronym = model.NameAcronym,
                                DisplayOrder = model.DisplayOrder,
                                ParentSysNo = model.ParentSysNo,
                                AreaName = model.AreaName,
                                AreaLevel = model.AreaLevel,
                                SysNo = model.SysNo
                            };
                        break;
                }
                model = GetArea(model.ParentSysNo);
            }
            return provinceEntity;
        }

        /// <summary>
        /// 根据区县编号判定是否支持百城当日达
        /// </summary>
        /// <param name="sysNo">区县编号</param>
        /// <returns>是：支持，否：不支持</returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        public override bool InDeliveryArea(int sysNo)
        {
            return
                Context.Sql("select count(1) from LgDeliveryScope where AreaSysNo=@0", sysNo).QuerySingle<int>() > 0;
        }

        /// <summary>
        /// 获取所有地区
        /// </summary>
        /// <returns>所有地区的集合</returns>
        /// <remarks>
        /// 2013-06-20 何方 创建
        /// </remarks>
        public override List<BsArea> GetAll()
        {
            //return Context.Sql("select * from BsArea order by  NLSSORT(areaname,'NLS_SORT=SCHINESE_PINYIN_M')").QueryMany<BsArea>();
            return Context.Sql("select * from BsArea order by  areaname Collate Chinese_PRC_Stroke_ci_as").QueryMany<BsArea>();

        }

        /// <summary>
        /// 获取所有子级地区
        /// </summary>
        /// <param name="parentSysNo">父级地区编号</param>
        /// <returns>所有子级地区</returns>
        /// <remarks> 
        /// 2013-08-12 郑荣华 创建
        /// </remarks>
        public override IList<BsArea> GetAreaList(int parentSysNo)
        {
            //return Context.Sql("select * from BsArea where parentsysno=@0 order by NLSSORT(areaname,'NLS_SORT=SCHINESE_PINYIN_M')", parentSysNo)
            //              .QueryMany<BsArea>();
            return Context.Sql("select * from BsArea where parentsysno=@0 order by areaname Collate Chinese_PRC_Stroke_ci_as", parentSysNo)
              .QueryMany<BsArea>();

        }

        /// <summary>
        /// 模糊查询字段地区名称,代码,,拼音
        /// </summary>
        /// <param name="keyword">关键词.</param>
        /// <returns>
        /// 所有地区的集合
        /// </returns>
        /// <remarks>
        /// 2013-06-20 何方 创建
        /// </remarks>
        public override IList<BsArea> Search(string keyword)
        {
            const string sql = @" select *   from bsarea 
                                    where
                                    (@keyword is null or charindex(areaname,@keyword) > 0 )
                                    or (@keyword is null or charindex(areacode,@keyword) > 0 )
                                    or (@keyword is null or charindex(nameacronym,@keyword) > 0 )";

            return Context.Sql(sql)
                          .Parameter("keyword", keyword)
                //.Parameter("keyword", keyword)
                //.Parameter("keyword", keyword)
                //.Parameter("keyword", keyword)
                //.Parameter("keyword", keyword)
                //.Parameter("keyword", keyword)
                          .QueryMany<BsArea>();
        }

        /// <summary>
        /// 淘宝地区与匹配商城地区
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="districtName">地区名称</param>
        /// <returns>匹配商城地区</returns>
        /// <remarks>2013-09-13 朱成果 创建</remarks>
        public override BsArea GetMatchDistrict(string cityName, string districtName)
        {

            return Context.Sql(@"select tb0.*
                                 from
                                 BsArea tb0
                                 inner join 
                                 BsArea tb1
                                 on tb0.parentsysno=tb1.sysno
                                 where tb0.AreaLevel=3 and charindex(tb0.areaname,@districtName) > 0 and charindex(tb1.areaname,@cityName) > 0 ")
                           .Parameter("districtName", districtName)
                           .Parameter("cityName", cityName).QuerySingle<BsArea>();
        }
        #endregion

    }
}
