using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Basic;
using Hyt.DataAccess.BaseInfo;
using Hyt.Model;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 前台地区信息业务逻辑
    /// </summary>
    public class BsAreaBo : BOBase<BsAreaBo>
    {
        /// <summary>
        /// 根据地区父级编号信息获取所有子级地区信息列表
        /// 只返回编号和名称,可以根据需要添加
        /// 不能更改已有的重命名
        /// </summary>
        /// <param name="parentSysNo">父级编号</param>
        /// <returns>子级地区信息列表</returns>
        /// <remarks>2013-08-13 黄波 创建</remarks>
        public IEnumerable GetChildArea(int parentSysNo) {

            var returnValue = BasicAreaBo.Instance.SelectArea((int)parentSysNo);

            //重构页面显示数据
            var items = from item in returnValue
                    select (new
                    {
                        No = item.SysNo,
                        Name = item.AreaName
                    });
            return items;
        }

        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <returns>省份列表</returns>
        /// <remarks>2013-12-18 黄波 创建</remarks>
        public IList<BsArea> GetRegion()
        {
            return BasicAreaBo.Instance.SelectArea(0);
        }

        /// <summary>
        /// 修改地区状态
        /// </summary>
        /// <param name="area">地区实体</param>
        /// <returns>返回受影响行数</returns>
        /// <remarks>2013-08-16 周瑜 创建</remarks>
        public int UpdateStatus(BsArea area)
        {
            if (area == null) return 0;
            var model= BasicAreaBo.Instance.GetArea(area.SysNo);
            if (model == null) return 0;
            model.Status = area.Status;
            model.LastUpdateBy = area.LastUpdateBy;
            model.LastUpdateDate = area.LastUpdateDate;
            return  BasicAreaBo.Instance.UpdateEntity(area)?1:0;
        }

        /// <summary>
        /// 获取地区详细信息
        /// </summary>
        /// <param name="areaSysno">地区编号</param>
        /// <returns>地区省市区详细信息</returns>
        /// <remarks>2013-08-22 唐永勤 创建</remarks>
        public Hyt.Model.Transfer.CBBsAreaDetail GetAreaDetail(int areaSysno)
        {
            BsArea provinceEntity,cityEntity,areaEntity;
            Hyt.Model.Transfer.CBBsAreaDetail entity = new Model.Transfer.CBBsAreaDetail();
           //优化 provinceEntity = Hyt.DataAccess.BaseInfo.IBsAreaDao.Instance.GetProvinceEntity(areaSysno, out cityEntity, out areaEntity);
            provinceEntity = BasicAreaBo.Instance.GetProvinceEntity(areaSysno, out cityEntity, out areaEntity);
            if (provinceEntity != null)
            {
                entity.Province = provinceEntity.AreaName;
                entity.ProvinceSysno = provinceEntity.SysNo;
            }
            if (cityEntity != null)
            {
                entity.City = cityEntity.AreaName;
                entity.CitySysno = cityEntity.SysNo;
            }
            if (areaEntity != null)
            {               
                entity.Region = areaEntity.AreaName;
                entity.RegionSysno = areaEntity.SysNo;
            }
            return entity;
        }

        /// <summary>
        /// 更具系统好获取地区信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2013-09-12 杨晗 创建</remarks>
        public BsArea GetArea(int sysNo)
        {
            return Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(sysNo);
            //return IBsAreaDao.Instance.GetArea(sysNo);
        }
    }
}
