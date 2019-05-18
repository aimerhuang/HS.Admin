using System;
using System.Collections;
using System.Collections.Generic;
using Jayrock.Json;
using Extra.UpGrade.SDK.JingDong.Domain;

namespace Extra.UpGrade.SDK.JingDong.Parser
{
	/// <summary>
	/// Jd JSON响应通用读取器。
	/// </summary>
	public class JdJsonReader : IJdReader
	{
		private IDictionary json;

		public JdJsonReader (IDictionary json)
		{
			this.json = json;
		}

		public bool HasReturnField (object name)
		{
			return json.Contains (name);
		}

		public object GetPrimitiveObject (object name)
		{
			return json [name];
		}

		public object GetReferenceObject (object name, Type type, DJdConvert convert)
		{
			IDictionary dict = json [name] as IDictionary;
			if (dict != null && dict.Count > 0) {
				return convert (new JdJsonReader (dict), type);
			} else if (json [name].ToString () != null && type.ToString ().EndsWith ("[]")) {
				return Jayrock.Json.Conversion.JsonConvert.Import (type, json [name].ToString ());
			} else {
				return null;
			}
		}

		public IList GetListObjects (string listName, string itemName, Type type, DJdConvert convert)
		{
			IList listObjs = null;
			IDictionary jsonMap = json [listName] as IDictionary;
			if (jsonMap != null && jsonMap.Count > 0) {
				IList jsonList = jsonMap [itemName] as IList;
				if (jsonList != null && jsonList.Count > 0) {
					Type listType = typeof(List<>).MakeGenericType (new Type[] { type });
					listObjs = Activator.CreateInstance (listType) as IList;
					foreach (object item in jsonList) {
						if (typeof(IDictionary).IsAssignableFrom (item.GetType ())) { // object
							IDictionary subMap = item as IDictionary;
							object subObj = convert (new JdJsonReader (subMap), type);
							if (subObj != null) {
								listObjs.Add (subObj);
							}
						} else if (typeof(IList).IsAssignableFrom (item.GetType ())) { // list or array
							// TODO not support yet
						} else if (typeof(JsonNumber).IsAssignableFrom (item.GetType ())) { // long
							listObjs.Add (((JsonNumber)item).ToInt64 ());
						} else { // string, bool, other
							listObjs.Add (item);
						}
					}
				}
			}

			return listObjs;
		}
	}
}
