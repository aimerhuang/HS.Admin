using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	/// <summary>
	/// 计划
	/// </summary>
	[Serializable]
	public class Campaign 
	{
		/**计划id */
		[XmlElement("campaign_id")]
			public long?  Campaign_id{ get; set; }

		/**商家id */
		[XmlElement("merchant_id")]
			public long?  Merchant_id{ get; set; }

		/**日限额 */
		[XmlElement("day_limit")]
			public double?  Day_limit{ get; set; }

		/**计划状态：online生效；offline未生效 */
		[XmlElement("plan_status")]
			public string  Plan_status{ get; set; }

		/**计划创建时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**计划更新时间 */
		[XmlElement("modified_time")]
			public string  Modified_time{ get; set; }

		/**计划生效--online */
		[XmlElement("p_l_a_n__s_t_a_t_u_s__o_n_l_i_n_e")]
			public string  P_l_a_n__s_t_a_t_u_s__o_n_l_i_n_e{ get; set; }

		/**计划未生效--offline */
		[XmlElement("p_l_a_n__s_t_a_t_u_s__o_f_f_l_i_n_e")]
			public string  P_l_a_n__s_t_a_t_u_s__o_f_f_l_i_n_e{ get; set; }

	}
}
