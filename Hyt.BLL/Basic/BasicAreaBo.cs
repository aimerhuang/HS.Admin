using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.DataAccess.BaseInfo;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Logistics;

namespace Hyt.BLL.Basic
{

    /// <summary>
    /// 地区信息业务类
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 杨晗 创建
    /// </remarks>
    public class BasicAreaBo : BOBase<BasicAreaBo>
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
        public int Create(BsArea model)
        {
            model.DisplayOrder = 1;
            var lstAll = GetAll();
            if (lstAll != null && lstAll.Count > 0)
            {
                model.DisplayOrder = lstAll.Select(m => m.DisplayOrder).Max() + 1;
            }
            var r = IBsAreaDao.Instance.Create(model);
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "添加地区", LogStatus.系统日志目标类型.地区信息, r);
            //清缓存.
            MemoryProvider.Default.Remove(KeyConstant.AreaAll);
            return r;
        }
        public List<BsArea> GetAllCity()
        {
            var r = IBsAreaDao.Instance.GetAllCity();
            return r;
        }
        /// <summary>
        /// 更新地区信息,只更新了，编码，区名称，父级sysno
        /// </summary>
        /// <param name="originalModel">地区信息实体，根据sysno</param>
        /// <returns>成功true,失败false</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public bool Update(BsArea originalModel)
        {
            var model = GetArea(originalModel.SysNo);
            model.ParentSysNo = originalModel.ParentSysNo;
            var pmodel = GetArea(model.ParentSysNo);
            if (pmodel != null)
            {
                model.AreaLevel = pmodel.AreaLevel + 1;
            }
            model.AreaName = originalModel.AreaName;
            model.AreaCode = originalModel.AreaCode;
            model.LastUpdateBy = originalModel.LastUpdateBy;
            model.LastUpdateDate = DateTime.Now;
            var r = UpdateEntity(model);
            if (r)
            {
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改地区", LogStatus.系统日志目标类型.地区信息, model.SysNo);
            }
            return r;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2014-05-15 朱成果 创建
        /// </remarks> 
        public bool UpdateEntity(BsArea entity)
        {
            var r = IBsAreaDao.Instance.Update(entity) > 0;
            if (r)
            {
                //清缓存
                MemoryProvider.Default.Remove(KeyConstant.AreaAll);
            }
            return r;
        }

        /// <summary>
        /// 删除地区信息
        /// </summary>
        /// <param name="sysNo">要删除的地区信息系统编号</param>
        /// <returns>成功true,失败false</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public bool Delete(int sysNo)
        {
            var r = IBsAreaDao.Instance.Delete(sysNo) > 0;
            if (r)
            {
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除地区", LogStatus.系统日志目标类型.地区信息, sysNo);
                //清缓存
                MemoryProvider.Default.Remove(KeyConstant.AreaAll);
            }
            return r;
        }

        /// <summary>
        /// 改变地区信息状态，有效，无效
        /// </summary>
        /// <param name="sysNo">地区信息系统编号</param>
        /// <param name="status">地区信息状态</param>
        /// <param name="lastUpdateBy">更新人</param>
        /// <returns>成功true,失败false</returns>
        /// <remarks> 
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        public bool ChangeAreaStatus(int sysNo, BasicStatus.地区状态 status, int lastUpdateBy)
        {
            var model = GetArea(sysNo);
            model.Status = (int)status;
            model.LastUpdateBy = lastUpdateBy;
            model.LastUpdateDate = DateTime.Now;
            var r = UpdateEntity(model);
            if (r)
            {
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改地区状态", LogStatus.系统日志目标类型.地区信息, model.SysNo);
            }
            return r;
        }

        /// <summary>
        /// 交换显示地区的显示排序
        /// </summary>
        /// <param name="originalSysNo">交换源对象系统编号</param>
        /// <param name="objectiveSysNo">要进行位置交换的目标对象系统编号</param>
        /// <returns>返回： true 操作成功  false 操作失败</returns>
        /// <remarks>注意：该方法值适用于在同一父级中进行移动变更</remarks>
        /// <remarks> 
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        public bool AreaSwapDisplayOrder(int originalSysNo, int objectiveSysNo, int CurrentUserSysNo)
        {
            bool success = false;
            var original = GetArea(originalSysNo);
            var objective = GetArea(objectiveSysNo);

            //显示位置交换
            var objectIndex = objective.DisplayOrder;
            objective.DisplayOrder = original.DisplayOrder;
            original.DisplayOrder = objectIndex;
            original.CreatedBy = CurrentUserSysNo;
            original.CreatedDate = DateTime.Now;
            original.LastUpdateDate = DateTime.Now;
            original.LastUpdateBy = CurrentUserSysNo;
            success = UpdateEntity(original);

            //源对象更是是否成功
            if (success)
            {
                //更新成功就更新目标对象
                success = UpdateEntity(objective);
            }
            else
            {
                //更新失败，数据还原
                original.DisplayOrder = objective.DisplayOrder;
                UpdateEntity(original);
            }
            //返回结果
            return success;
        }

        #endregion

        #region 查询

        /// <summary>
        /// 根据地区名称模糊递归出地区列表(指定参数可递归到顶级父节点、默认只递归子节点)
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <param name="getParent">是否获取父级节点 true=向上递归查询， false=向下递归查询</param>
        /// <returns>地区列表</returns>
        /// <remarks>2013-12-16 周唐炬 创建</remarks>
        public List<BsArea> QueryRecursive(string name, bool getParent = false)
        {
            //优化 return IBsAreaDao.Instance.QueryRecursive(name, getParent);
            var alllst = GetAll();
            List<BsArea> lst = new List<BsArea>();
            var searchlst = alllst.Where(m => m.Status == 1 && string.IsNullOrEmpty(m.AreaName) == false && m.AreaName.IndexOf(name) > -1).ToList();
            if (searchlst != null)
            {
                searchlst.ForEach((item) =>
                {
                    if (!getParent)//向下级联
                    {
                        lst.Add(item);
                        lst.AddRange(GetDownList(item.SysNo, alllst));
                    }
                    else//向上级联
                    {
                        while (item != null)
                        {
                            lst.Add(item);
                            item = GetArea(item.ParentSysNo);
                        }
                    }
                });
            }
            return lst;
        }

        /// <summary>
        /// 据地区名称模糊递归出地区列表(包括父级和子级)
        /// </summary>
        /// <param name="keyword">地区名</param>
        /// <param name="list">地区表</param>
        /// <returns>地区列表</returns>
        /// <remarks>2014-08-13 余勇 创建</remarks>
        public List<BsArea> QueryAreas(string keyword, List<BsArea> list)
        {
            List<BsArea> lst = new List<BsArea>();
            keyword = keyword.ToLower(); //全部小写
            var searchlst = list.Where(m => m.Status == 1 && string.IsNullOrEmpty(m.AreaName) == false && m.AreaName.IndexOf(keyword) > -1).ToList();
            if (searchlst != null && searchlst.Count > 0)
            {
                searchlst.ForEach((item) =>
                {
                    if (!lst.Contains(item))
                    {
                        lst.Add(item);
                        lst.AddRange(GetDownList(item.SysNo, list));
                    }

                    while (item != null)
                    {
                        if (!lst.Contains(item))
                        {
                            lst.Add(item);
                        }
                        item = GetArea(item.ParentSysNo);
                    }
                });
            }
            return lst;
        }

        /// <summary>
        /// 获取向下级联数据
        /// </summary>
        /// <param name="parentID">父亲编号</param>
        /// <param name="allLst">所有地区信息</param>
        /// <returns>向下级联的地区信息</returns>
        /// <remarks>2014-05-15 朱成果 创建</remarks>
        private List<BsArea> GetDownList(int parentID, List<BsArea> allLst)
        {
            List<BsArea> lst = new List<BsArea>();
            var sublist = allLst.Where(m => m.Status == 1 && m.ParentSysNo == parentID).ToList();
            if (sublist != null && sublist.Count > 0)
            {
                lst.AddRange(sublist);
                foreach (var item in sublist)
                    lst.AddRange(GetDownList(item.SysNo, allLst));
            }
            return lst;
        }

        /// <summary>
        /// 获取省市区数据
        /// </summary>
        /// <param name="parentSysNo">地区父级系统号</param>
        /// <returns>省市区数据</returns>
        /// <remarks>
        /// 2013-06-13 杨晗 创建
        /// </remarks>
        public IList<BsArea> SelectArea(int parentSysNo)
        {
            //优化 return IBsAreaDao.Instance.SelectArea(parentSysNo);
            return GetAll().Where(m => m.Status == (int)BasicStatus.地区状态.有效 && m.ParentSysNo == parentSysNo).ToList();
        }

        /// <summary>
        /// 查询下级地区，排除没有仓库的地区
        /// </summary>
        /// <param name="parentSysNo">上级地区编号</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="deliveryTypeSysNo">配送方式编号</param>
        /// <returns>省市区数据</returns>
        /// <remarks> 2013-07-02 朱成果 创建 </remarks>
        /// <remarks> 2013-10-08 黄志勇 修改 </remarks>
        public IList<BsArea> SelectAreaWithWarehouse(int parentSysNo, int? warehouseType, int? deliveryTypeSysNo)
        {
            return IBsAreaDao.Instance.SelectAreaWithWarehouse(parentSysNo, warehouseType, deliveryTypeSysNo);
        }

        /// <summary>
        /// 获取地区全称
        /// </summary>
        /// <param name="areaSysNo">The area sys no.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/5 何方 创建
        /// </remarks>
        public string GetAreaFullName(int areaSysNo)
        {
            return GetAreaFullName(new int[] { areaSysNo })[0].Value;
        }

        /// <summary>
        /// 获取第三级地区全称
        /// </summary>
        /// <param name="areaSysNo">The level 3 area sys no.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-05 何方 创建
        /// </remarks>
        public IList<KeyValuePair<int, string>> GetAreaFullName(int[] areaSysNo)
        {
            BsArea cityEntity;
            BsArea stateEntity;
            var result = new List<KeyValuePair<int, string>>() { };
            foreach (var sysno in areaSysNo)
            {
                var area = GetProvinceEntity(sysno, out cityEntity, out stateEntity);
                if (area == null)
                {
                    result.Add(new KeyValuePair<int, string>(sysno, "地区:" + sysno + "不存在"));
                }
                else if (cityEntity == null)
                {

                    result.Add(new KeyValuePair<int, string>(sysno, "地区" + area.AreaName + "父级地区:" + area.ParentSysNo + "不存在"));
                }
                else if (stateEntity == null)
                {

                    result.Add(new KeyValuePair<int, string>(sysno, "地区" + cityEntity.AreaName + "父级地区:" + cityEntity.ParentSysNo + "不存在"));
                }
                else
                {
                    result.Add(new KeyValuePair<int, string>(sysno,
                                                             area.AreaName + cityEntity.AreaName +
                                                             stateEntity.AreaName));
                }
            }
            return result;
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
        public BsArea GetArea(int sysNo, BasicStatus.地区状态 status)
        {
            //优化 return IBsAreaDao.Instance.GetArea(sysNo, status);
            return GetAll().FirstOrDefault(m => m.SysNo == sysNo && m.Status == status.GetHashCode());//缓存中查找
        }

        /// <summary>
        /// 根据地区名称获取地区列表 模糊查询
        /// </summary>
        /// <param name="areaName">地区名称</param>
        /// <returns>地区列表</returns>
        /// <remarks>
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        public IList<BsArea> GetArea(string areaName)
        {
            //优化 return IBsAreaDao.Instance.GetArea(areaName);
            return GetAll().Where(m => m.AreaName.IndexOf(areaName) > -1).ToList();//缓存中查找
        }

        /// <summary>
        /// 根据地区名称获取地区列表
        /// </summary>
        /// <param name="areaName">地区名称</param>
        /// <returns>地区列表</returns>
        /// <remarks>
        /// 2016-07-18 王耀发 创建
        /// </remarks>
        public BsArea GetAreaByName(string areaName, int parentSysNo)
        {
            return GetAll().Where(m => m.AreaName == areaName && m.ParentSysNo == parentSysNo).First();//缓存中查找
        }

        /// <summary>
        /// 获取地区实体
        /// </summary>
        /// <param name="sysNo">地区系统号</param>
        /// <returns>单个地区数据</returns>
        /// <remarks>
        /// 2013-06-13 杨晗 创建
        /// </remarks>
        public BsArea GetArea(int sysNo)
        {
            //优化 return IBsAreaDao.Instance.GetArea(sysNo); 
            return GetAll().FirstOrDefault(m => m.SysNo == sysNo);//缓存中查找
        }

        /// <summary>
        /// 获取地区名称
        /// </summary>
        /// <param name="sysNo">地区系统号</param>
        /// <returns>单个地区数据</returns>
        /// <remarks>
        /// 2013-06-13 杨晗 创建
        /// </remarks>
        public string GetAreaName(int sysNo)
        {
            //优化 var area = IBsAreaDao.Instance.GetArea(sysNo);
            var area = GetArea(sysNo);
            return area == null ? "找不到编号为" + sysNo + "的地区" : area.AreaName;
        }

        /// <summary>
        /// 获取地区模型
        /// </summary>
        /// <param name="sysNo">地区系统号</param>
        /// <returns>单个地区数据</returns>
        /// <remarks>
        /// 2013-06-13 杨晗 创建
        /// </remarks>
        public CBBsArea2 GetCbArea(int sysNo)
        {
            //优化 return IBsAreaDao.Instance.GetCbArea(sysNo);
            var area = GetArea(sysNo);
            if (area == null) return null;
            CBBsArea2 arr2 = GetCBBsArea2(area);
            arr2.ParentName = "无";
            var areaTop1 = GetArea(area.ParentSysNo);
            if (areaTop1 != null)
            {
                arr2.ParentName = areaTop1.AreaName;
            }
            return arr2;
        }

        /// <summary>
        /// 获取地区信息扩展
        /// </summary>
        /// <param name="entity">地区信息</param>
        /// <returns>地区信息扩展</returns>
        /// <remarks>
        /// 2014-05-15 朱成果 创建
        /// </remarks>
        private CBBsArea2 GetCBBsArea2(BsArea entity)
        {
            if (entity == null) return null;
            CBBsArea2 arr2 = new CBBsArea2();
            var baseType = typeof(BsArea);
            var allPropertie = baseType.GetProperties();
            foreach (var propertie in allPropertie)
            {
                if (propertie.CanRead && propertie.CanWrite)
                {
                    propertie.SetValue(arr2, propertie.GetValue(entity, null), null);
                }
            }
            return arr2;
        }

        /// <summary>
        /// 根据地区编号，获取省市区信息
        /// </summary>
        /// <param name="sysNo">地区编号</param>
        /// <param name="cityEntity">城市信息</param>
        /// <param name="areaEntity">地区信息</param>
        /// <returns>区域信息</returns>
        /// <remarks>
        /// 2013-06-14 朱成果 创建
        /// </remarks>
        public BsArea GetProvinceEntity(int sysNo, out BsArea cityEntity, out BsArea areaEntity)
        {
            //优化 return IBsAreaDao.Instance.GetProvinceEntity(sysNo, out cityEntity, out areaEntity);
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

        public BsArea GetCountryEntity(int sysNo, out BsArea provinceEntity, out BsArea cityEntity, out BsArea areaEntity)
        {
            provinceEntity = null;
            cityEntity = null;
            areaEntity = null;
            BsArea countryEntity = null;
            BsArea model = GetArea(sysNo);
            //地区信息不存在
            while (model != null && model.AreaLevel >= 0)
            {
                switch (model.AreaLevel)
                {
                    //国家
                    case 0:
                        countryEntity = new BsArea
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
            return countryEntity;
        }

        /// <summary>
        ///先从缓存中获取 获取所有地区
        /// </summary>
        /// <returns>所有地区的集合</returns>
        /// <remarks>
        /// 2013-06-20 何方 创建
        /// </remarks>
        public List<BsArea> GetAll()
        {
            var list = MemoryProvider.Default.Get<List<BsArea>>(KeyConstant.AreaAll, () => IBsAreaDao.Instance.GetAll());
            return list;
        }

        /// <summary>
        /// 模糊查询字段地区名称,代码,,拼音
        /// </summary>
        /// <param name="keyword">关键词.</param>
        /// <returns>所有地区的集合</returns>
        /// <remarks>2013-06-20 何方 创建</remarks>
        public IList<BsArea> Search(string keyword)
        {

            // 优化  return IBsAreaDao.Instance.Search(keyword);
            return GetAll().Where(m => m.AreaName.IndexOf(keyword) > -1 || m.AreaCode.IndexOf(keyword) > -1 || m.NameAcronym.IndexOf(keyword) > -1).ToList();
        }

        /// <summary>
        /// 获取所有子级地区
        /// </summary>
        /// <param name="parentSysNo">父级地区编号</param>
        /// <returns>所有子级地区</returns>
        /// <remarks> 
        /// 2013-08-12 郑荣华 创建
        /// </remarks>
        public IList<BsArea> GetAreaList(int parentSysNo)
        {
            //优化 return IBsAreaDao.Instance.GetAreaList(parentSysNo);
            return GetAll().Where(m => m.ParentSysNo == parentSysNo).ToList();
        }

        /// <summary>
        /// 淘宝地区与匹配商城地区
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="districtName">地区名称</param>
        /// <returns></returns>
        /// <remarks>2013-09-13 朱成果 创建</remarks>
        public BsArea GetMatchDistrict(string cityName, string districtName)
        {
            //满足例如:淘宝的地址为“高新区"商城 为"高新南区"
            if (!string.IsNullOrEmpty(districtName) && districtName.LastIndexOf("区") == districtName.Length - 1 && districtName.Length > 1)
            {
                districtName = districtName.Substring(0, districtName.Length - 1);
            }
            //满足例如:淘宝的地址为“成都市"商城 为"成都"
            if (!string.IsNullOrEmpty(cityName) && cityName.LastIndexOf("市") == cityName.Length - 1 && cityName.Length > 1)
            {
                cityName = cityName.Substring(0, cityName.Length - 1);
            }
            //优化 return IBsAreaDao.Instance.GetMatchDistrict(cityName, districtName);
            var alllst = GetAll();
            var lstCityNo = alllst.Where(m => m.Status == 1 && m.AreaLevel == 2 && m.AreaName.IndexOf(cityName) > -1).Select(m => m.SysNo).ToList();
            var area = alllst.Where(m => m.Status == 1 && m.AreaLevel == 3 && m.AreaName.IndexOf(districtName) > -1 && lstCityNo.Contains(m.ParentSysNo)).FirstOrDefault();
            return area;
        }
        /// <summary>
        /// 有赞地区与匹配商城地区
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="districtName">地区名称</param>
        /// <returns></returns>
        /// <remarks>2016-07-15 杨浩 创建</remarks>
        public BsArea GetYouZanMatchDistrict(string cityName, string districtName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return null;


            districtName = districtName.Replace("\r\r", "").Replace("\n","");
            //优化 return IBsAreaDao.Instance.GetMatchDistrict(cityName, districtName);
            var alllst = GetAll();

            var lstCityNo = alllst.Where(m => m.Status == 1 && m.AreaLevel == 2 && m.AreaName.IndexOf(cityName) > -1).Select(m => m.SysNo).ToList();


            if (string.IsNullOrWhiteSpace(districtName.Trim()))           
                districtName = "其他区";

            var area = alllst.Where(m => m.Status == 1 && m.AreaLevel == 3 && m.AreaName.IndexOf(districtName) > -1 && lstCityNo.Contains(m.ParentSysNo)).FirstOrDefault();

            return area;
        }

        /// <summary>
        /// 海带地区与匹配商城地区
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="districtName">地区名称</param>
        /// <returns></returns>
        /// <remarks>2017-06-15 罗勤尧 创建</remarks>
        public BsArea GetHaiDaiMatchDistrict(string cityName, string districtName)
        {

            //优化 return IBsAreaDao.Instance.GetMatchDistrict(cityName, districtName);
            var alllst = GetAll();
            var lstCityNo = alllst.Where(m => m.Status == 1 && m.AreaLevel == 2 && m.AreaName.IndexOf(cityName) > -1).Select(m => m.SysNo).ToList();
            var area = alllst.Where(m => m.Status == 1 && m.AreaLevel == 3 && m.AreaName.IndexOf(districtName) > -1 && lstCityNo.Contains(m.ParentSysNo)).FirstOrDefault();
            return area;
        }
        #endregion

        /// <summary>
        /// 判断是否可以添加此地区
        /// </summary>
        /// <param name="areaName">地区名称</param>
        /// <returns>可以返回true,不可用返回false</returns>
        /// <remarks> 
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        public bool IsCanAddArea(string areaName)
        {
            return GetArea(areaName).Count <= 0;
        }

        /// <summary>
        /// 获取所有地区,用于构建地区树
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public IList<BsArea> GetAllAreaForTree()
        {

            //优化 return IBsAreaDao.Instance.GetAllAreaForTree();
            return GetAll().Where(m => m.AreaLevel <= 3).ToList();
        }

        /// <summary>
        /// 获取仓库的覆盖地区
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public IList<CBBsArea2> GetAreaByWarehouse(int warehouseSysNo)
        {
            //var cbbAreas = IBsAreaDao.Instance.GetAreaByWarehouse(warehouseSysNo);
            //foreach (var cb in cbbAreas)
            //{
            //    string name = GetAreaFullNames(cb.ParentSysNo);
            //    cb.FullName = name + "_" + cb.AreaName;
            //}
            // return cbbAreas;
            var warehouseArea = Hyt.BLL.Warehouse.WhWarehouseAreaBo.Instance.GetAllWhWarehouseArea().Where(m => m.WarehouseSysNo == warehouseSysNo).ToList();
            List<CBBsArea2> rr = new List<CBBsArea2>();
            if (warehouseArea != null)
            {
                warehouseArea.ForEach((item) =>
                {
                    if (!rr.Any(m => m.SysNo == item.AreaSysNo))
                    {
                        var entity = GetCBBsArea2(GetArea(item.AreaSysNo));
                        if (entity != null)
                        {
                            entity.IsDefault = item.IsDefault;
                            string name = GetAreaFullNames(entity.ParentSysNo);
                            entity.FullName = name + "_" + entity.AreaName;
                            rr.Add(entity);
                        }
                    }
                });
            }
            return rr;
        }

        /// <summary>
        /// 根据最后一级地区的父级编号查询全称
        /// </summary>
        /// <param name="parentSysNo">地区父级系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public string GetAreaFullNames(int parentSysNo)
        {
            var bsArea = GetArea(parentSysNo);
            if (bsArea.ParentSysNo != 0)
            {
                return GetAreaFullNames(bsArea.ParentSysNo) + "_" + bsArea.AreaName;
            }
            return bsArea.AreaName;
        }

        #region 判断一个坐标是否在百城当日达范围内

        /// <summary>
        /// 坐标点
        /// </summary>
        /// <remarks>2013－09-02 朱成果 创建</remarks>
        private struct PointD
        {
            private double _x;
            private double _y;

            public PointD(double x, double y)
            {
                _x = x;
                _y = y;
            }

            public double X
            {

                get { return _x; }
                set { _x = value; }
            }

            public double Y
            {

                get { return _y; }
                set { _y = value; }
            }

        }

        /// <summary>
        /// 判断一个坐标是否在百城当日达范围内
        /// </summary>
        /// <param name="x">经度</param>
        /// <param name="y">纬度</param>
        /// <param name="areaNo">城市编号（地区第二级)</param>
        /// <returns></returns>
        /// <remarks>2013－09-02 朱成果 创建</remarks>
        /// 
        [Obsolete("方法已废弃")]
        private bool IsPointInRegion(double x, double y, int areaNo)
        {
            bool result = false;

            var lst = LgDeliveryScopeBo.Instance.GetDeliveryScope(areaNo);
            if (lst != null)
            {
                foreach (LgDeliveryScope data in lst)
                {
                    if (!string.IsNullOrEmpty(data.MapScope))
                    {
                        PointD testPoint = new PointD(x, y);
                        string[] allPoint = data.MapScope.Split(';');
                        List<PointD> polygon = new List<PointD>();
                        for (int i = 0; i < allPoint.Length; i++)
                        {
                            string[] currectPoint = allPoint[i].Split(',');
                            if (currectPoint != null && currectPoint.Length == 2)
                            {
                                PointD pf = new PointD(double.Parse(currectPoint[0]), double.Parse(currectPoint[1]));
                                polygon.Add(pf);
                            }

                        }
                        result = PointInFences(testPoint, polygon.ToArray());
                        if (result) return result;
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// 射线法判断点是否在多边形内
        /// </summary>
        /// <param name="pnt">当前点</param>
        /// <param name="fencePnts">点范围</param>
        /// <returns></returns>
        /// <remarks>2013－09-02 朱成果 创建</remarks>
        private bool PointInFences(PointD pnt, PointD[] fencePnts)
        {
            int j = 0, cnt = 0;
            for (int i = 0; i < fencePnts.Length; i++)
            {
                j = (i == fencePnts.Length - 1) ? 0 : j + 1;
                if ((fencePnts[i].Y != fencePnts[j].Y) && (((pnt.Y >= fencePnts[i].Y) && (pnt.Y < fencePnts[j].Y)) || ((pnt.Y >= fencePnts[j].Y) && (pnt.Y < fencePnts[i].Y))) && (pnt.X < (fencePnts[j].X - fencePnts[i].X) * (pnt.Y - fencePnts[i].Y) / (fencePnts[j].Y - fencePnts[i].Y) + fencePnts[i].X)) cnt++;
            }
            return (cnt % 2 > 0) ? true : false;
        }
        public static WebClient wc = new WebClient();

        /// <summary>
        /// 根据百度地图api+市名+地名，获取精确经纬度
        /// </summary>
        /// <param name="key">百度key</param>
        /// <param name="city">市名，如：成都市</param>
        /// <param name="address">地名，如：天府广场</param>
        /// <returns>Item1：纬度，Item2：经度</returns>
        /// <remarks>2013－09-02 黄志勇 创建</remarks>
        public Tuple<double, double> GetCoord(string key, string city, string address)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    string uri =
                    string.Format(
                        "http://api.map.baidu.com/geocoder/v2/?ak={0}&callback=renderOption&output=xml&address={1}&city={2}",
                        key, city, address);
                    byte[] bResponse = wc.DownloadData(uri);
                    string strResponse = Encoding.UTF8.GetString(bResponse);
                    var root = new XmlDocument();
                    root.LoadXml(strResponse);
                    var lat = root.SelectSingleNode("//lat");
                    var lng = root.SelectSingleNode("//lng");
                    if (lat == null || lat.InnerText == "" || lng == null || lng.InnerText == "") return null;
                    Tuple<double, double> coordcoord = Tuple.Create(double.Parse(lat.InnerText), double.Parse(lng.InnerText));
                    return coordcoord;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 判断输入的地址是否在指定的城市百城当日达范围内
        /// </summary>
        /// <param name="areaNo">地区SysNo</param>
        /// <param name="city">城市名</param>
        /// <param name="address">要查找的地名</param>
        /// <returns>true:百城当日达范围内,false:百城当日达范围外</returns>
        /// <remarks>
        /// 2013－09-02 黄志勇 创建
        /// 测试用例：
        /// int areaNo = 901;
        /// string city = "成都";
        /// string address = "天府广场";
        /// bool inScope = IsPointInRegion(areaNo, city, address);
        /// </remarks>
        public bool IsPointInRegion(int areaNo, string city, string address)
        {
            var key = Hyt.Model.Map.MapRef.MapRefKey;// ConfigurationManager.AppSettings["BaiduMapKey"];
            var coord = GetCoord(key, city, address);
            if (!string.IsNullOrEmpty(key) && coord != null)
            {
                return Hyt.BLL.Logistics.LgDeliveryScopeBo.Instance.IsInScope(areaNo, new Coordinate() { X = coord.Item2, Y = coord.Item1 });
                // return IsPointInRegion(coord.Item2, coord.Item1, areaNo);
            }
            return true;
        }
        #endregion


        public BsArea GetCountryEntity(int sysNo, out BsArea countryEntity, out BsArea provinceEntity, out BsArea cityEntity, out BsArea areaEntity)
        {
            provinceEntity = null;
            cityEntity = null;
            areaEntity = null;
            countryEntity = null;
            BsArea model = GetArea(sysNo);
            //地区信息不存在
            while (model != null && model.AreaLevel >= 0)
            {
                switch (model.AreaLevel)
                {
                    //国家
                    case 0:
                        countryEntity = new BsArea
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
            return countryEntity;
        }

    }
}
