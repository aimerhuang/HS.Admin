using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Web;
using Hyt.Util;
using Hyt.Model;
using Hyt.Model.SearchAttribute;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 商品属性
    /// </summary>
    /// <remarks>2013-08-22 黄波 创建</remarks>
    public class PdAttributeDaoImpl : IPdAttributeDao
    {
        /// <summary>
        /// 根据分类编号获取分类下所有可作为搜索选项的属性以及属性选项列表
        /// </summary>
        /// <param name="categorySysNo">分类列表</param>
        /// <return>属性及属性选项列表</return>
        /// <remarks>2013-08-22 黄波 创建</remarks>
        public override IList<Hyt.Model.SearchAttributeAndOptions> GetSearchAttributeAndOptions(int categorySysNo)
        {
            string sql = @"select distinct  d.Sysno as AttributeOptionSysNo, d.attributetext as AttributeOptionText,c.sysno as AttributeSysNo,c.attributename  as AttributeName
                                    from PdCatAttributeGroupAso a   left join  PdAttributeGroupAssociation b 
                                                                                            on a.AttributeGroupSysNo=b.AttributeGroupSysNo
                                                                                         left join PdAttribute c 
                                                                                            on c.sysno=b.attributesysno
                                                                                         left join PdAttributeOption d
                                                                                            on d.attributesysno=c.sysno  
                                where a.ProductCategorySysNo=@ProductCategorySysNo and c.IsSearchKey=@IsSearchKey  and c.AttributeType = " + (int)Hyt.Model.WorkflowStatus.ProductStatus.商品属性类型.选项类型 + @"
                                order by c.sysno";

            var result = Context.Sql(sql)
                            .Parameter("ProductCategorySysNo", categorySysNo.ToString())
                            .Parameter("IsSearchKey", (int)Hyt.Model.WorkflowStatus.ProductStatus.是否用做关联属性.是)
                            .QueryMany<SearchAttribute>();

            var returnValue = new List<SearchAttributeAndOptions>();

            var resultDistinct = result.GroupBy(o => o.AttributeSysNo)
                .Select(
                        o => new SearchAttribute
                        {
                            AttributeSysNo = o.Key,
                            AttributeName = o.FirstOrDefault().AttributeName,
                            AttributeOptionSysNo = o.FirstOrDefault().AttributeOptionSysNo,
                            AttributeOptionText = o.FirstOrDefault().AttributeOptionText
                        }
                );

            SearchAttributeAndOptions searchAttributeAndOptions;
            List<SearchAttributeOption> searchAttributeOption;
            foreach (var item in resultDistinct)
            {
                searchAttributeAndOptions = new SearchAttributeAndOptions();
                searchAttributeAndOptions.AttributeName = item.AttributeName;
                searchAttributeAndOptions.AttributeSysNo = item.AttributeSysNo;

                searchAttributeOption = new List<SearchAttributeOption>();
                var attributeList = result.FindAll(o => o.AttributeSysNo == item.AttributeSysNo);
                foreach (var attribute in attributeList)
                {
                    searchAttributeOption.Add(new SearchAttributeOption
                    {
                        AttributeOptionSysNo = attribute.AttributeOptionSysNo,
                        AttributeOptionText = attribute.AttributeOptionText
                    });
                }
                searchAttributeAndOptions.Options = searchAttributeOption;
                returnValue.Add(searchAttributeAndOptions);
            }
            return returnValue;
        }
    }
}
