using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using Jayrock.Json.Conversion;

namespace Extra.UpGrade.SDK.JingDong.Parser
{
	/// <summary>
	/// Jd JSON响应通用解释器。
	/// </summary>
	public class JdJsonParser : IJdParser
	{
		private static readonly Dictionary<string, Dictionary<string, JdAttribute>> attrs = new Dictionary<string, Dictionary<string, JdAttribute>> ();

		#region IJdParser Members

		public T Parse<T> (string body) where T : JdResponse
		{
			T rsp = null;

			IDictionary json = JsonConvert.Import (body) as IDictionary;
			if (json != null) {
				IDictionary data = null;

				// 忽略根节点的名称
				foreach (object key in json.Keys) {
					data = json [key] as IDictionary;
					break;
				}

				if (data != null) {
					IJdReader reader = new JdJsonReader (data);
					try {
						rsp = (T)FromJson (reader, typeof(T));
					} catch (Exception ex) {
						throw ex;
					}
				}
			}

			if (rsp == null) {
				rsp = Activator.CreateInstance<T> ();
			}

			if (rsp != null) {
				rsp.Body = body;
			}

			return rsp;
		}

		#endregion

		private Dictionary<string, JdAttribute> GetJdAttributes (Type type)
		{
			Dictionary<string, JdAttribute> tas = null;
			bool inc = attrs.TryGetValue (type.FullName, out tas);

			if (inc && tas != null) { // 从缓存中获取类属性元数据
				return tas;
			} else { // 创建新的类属性元数据缓存
				tas = new Dictionary<string, JdAttribute> ();
			}

			PropertyInfo[] pis = type.GetProperties ();
			foreach (PropertyInfo pi in pis) {
				JdAttribute ta = new JdAttribute ();
				ta.Method = pi.GetSetMethod ();

				// 获取对象属性名称
				XmlElementAttribute[] xeas = pi.GetCustomAttributes (typeof(XmlElementAttribute), true) as XmlElementAttribute[];
				if (xeas != null && xeas.Length > 0) {
					ta.ItemName = xeas [0].ElementName;
				}

				// 获取列表属性名称
				if (ta.ItemName == null) {
					XmlArrayItemAttribute[] xaias = pi.GetCustomAttributes (typeof(XmlArrayItemAttribute), true) as XmlArrayItemAttribute[];
					if (xaias != null && xaias.Length > 0) {
						ta.ItemName = xaias [0].ElementName;
					}
					XmlArrayAttribute[] xaas = pi.GetCustomAttributes (typeof(XmlArrayAttribute), true) as XmlArrayAttribute[];
					if (xaas != null && xaas.Length > 0) {
						ta.ListName = xaas [0].ElementName;
					}
					if (ta.ListName == null) {
						continue;
					}
				}

				// 获取属性类型
				if (pi.PropertyType.IsGenericType) {
					Type[] types = pi.PropertyType.GetGenericArguments ();
					ta.ListType = types [0];
				} else {
					ta.ItemType = pi.PropertyType;
				}

				tas.Add (pi.Name, ta);
			}

			attrs [type.FullName] = tas;
			return tas;
		}

		public object FromJson (IJdReader reader, Type type)
		{
			object rsp = null;
			Dictionary<string, JdAttribute> pas = GetJdAttributes (type);

			Dictionary<string, JdAttribute>.Enumerator em = pas.GetEnumerator ();
			try {
				while (em.MoveNext ()) {
					KeyValuePair<string, JdAttribute> kvp = em.Current;
					JdAttribute ta = kvp.Value;
					string itemName = ta.ItemName;
					string listName = ta.ListName;
					if (!reader.HasReturnField (itemName) && (string.IsNullOrEmpty (listName) || !reader.HasReturnField (listName))) {
						continue;
					}
            	
					object value = null;
					if (ta.ListType != null) {
						value = reader.GetListObjects (listName, itemName, ta.ListType, FromJson);
					} else {
						if (typeof(string) == ta.ItemType) {
							object tmp = reader.GetPrimitiveObject (itemName);
							if (tmp != null) {
								value = tmp.ToString ();
							}
						} else if (typeof(long) == ta.ItemType) {
							object tmp = reader.GetPrimitiveObject (itemName);
							if (tmp != null) {
								value = ((IConvertible)tmp).ToInt64 (null);
							}
						} else if (typeof(bool) == ta.ItemType) {
							value = reader.GetPrimitiveObject (itemName);
						} else {
							value = reader.GetReferenceObject (itemName, ta.ItemType, FromJson);
						}
					}
            	
					if (value != null) {
						if (rsp == null) {
							rsp = Activator.CreateInstance (type);
						}
						ta.Method.Invoke (rsp, new object[] { value });
					}
				}
			} catch (Exception ex) {
				throw ex;
			}

			return rsp;
		}
	}
}
