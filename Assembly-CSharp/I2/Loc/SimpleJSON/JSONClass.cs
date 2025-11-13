using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace I2.Loc.SimpleJSON
{
	public class JSONClass : JSONNode, IEnumerable
	{
		public override JSONNode this[string aKey]
		{
			get
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					return this.m_Dict[aKey];
				}
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = value;
					return;
				}
				this.m_Dict.Add(aKey, value);
			}
		}

		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return null;
				}
				return this.m_Dict.ElementAt(aIndex).Value;
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return;
				}
				string key = this.m_Dict.ElementAt(aIndex).Key;
				this.m_Dict[key] = value;
			}
		}

		// (get) Token: 0x06001098 RID: 4248 RVA: 0x00066FF2 File Offset: 0x000651F2
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00067000 File Offset: 0x00065200
		public override void Add(string aKey, JSONNode aItem)
		{
			if (string.IsNullOrEmpty(aKey))
			{
				this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
				return;
			}
			if (this.m_Dict.ContainsKey(aKey))
			{
				this.m_Dict[aKey] = aItem;
				return;
			}
			this.m_Dict.Add(aKey, aItem);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0006705E File Offset: 0x0006525E
		public override JSONNode Remove(string aKey)
		{
			if (!this.m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode jsonnode = this.m_Dict[aKey];
			this.m_Dict.Remove(aKey);
			return jsonnode;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0006708C File Offset: 0x0006528C
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.Count)
			{
				return null;
			}
			KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt(aIndex);
			this.m_Dict.Remove(keyValuePair.Key);
			return keyValuePair.Value;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x000670D4 File Offset: 0x000652D4
		public override JSONNode Remove(JSONNode aNode)
		{
			JSONNode jsonnode;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.Where((KeyValuePair<string, JSONNode> k) => k.Value == aNode).First<KeyValuePair<string, JSONNode>>();
				this.m_Dict.Remove(keyValuePair.Key);
				jsonnode = aNode;
			}
			catch
			{
				jsonnode = null;
			}
			return jsonnode;
		}

		// (get) Token: 0x0600109D RID: 4253 RVA: 0x00067140 File Offset: 0x00065340
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
				{
					yield return keyValuePair.Value;
				}
				Dictionary<string, JSONNode>.Enumerator enumerator = default(Dictionary<string, JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00067150 File Offset: 0x00065350
		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				yield return keyValuePair;
			}
			Dictionary<string, JSONNode>.Enumerator enumerator = default(Dictionary<string, JSONNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00067160 File Offset: 0x00065360
		public override string ToString()
		{
			string text = "{";
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text = string.Concat(new string[]
				{
					text,
					"\"",
					JSONNode.Escape(keyValuePair.Key),
					"\":",
					keyValuePair.Value
				});
			}
			text += "}";
			return text;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00067214 File Offset: 0x00065414
		public override string ToString(string aPrefix)
		{
			string text = "{ ";
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text = string.Concat(new string[]
				{
					text,
					"\"",
					JSONNode.Escape(keyValuePair.Key),
					"\" : ",
					keyValuePair.Value.ToString(aPrefix + "   ")
				});
			}
			text = text + "\n" + aPrefix + "}";
			return text;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x000672EC File Offset: 0x000654EC
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(2);
			aWriter.Write(this.m_Dict.Count);
			foreach (string text in this.m_Dict.Keys)
			{
				aWriter.Write(text);
				this.m_Dict[text].Serialize(aWriter);
			}
		}

		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>(StringComparer.Ordinal);
	}
}
