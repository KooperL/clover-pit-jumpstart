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

		// (get) Token: 0x06001081 RID: 4225 RVA: 0x00066816 File Offset: 0x00064A16
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00066824 File Offset: 0x00064A24
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

		// Token: 0x06001083 RID: 4227 RVA: 0x00066882 File Offset: 0x00064A82
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

		// Token: 0x06001084 RID: 4228 RVA: 0x000668B0 File Offset: 0x00064AB0
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

		// Token: 0x06001085 RID: 4229 RVA: 0x000668F8 File Offset: 0x00064AF8
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

		// (get) Token: 0x06001086 RID: 4230 RVA: 0x00066964 File Offset: 0x00064B64
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

		// Token: 0x06001087 RID: 4231 RVA: 0x00066974 File Offset: 0x00064B74
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

		// Token: 0x06001088 RID: 4232 RVA: 0x00066984 File Offset: 0x00064B84
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

		// Token: 0x06001089 RID: 4233 RVA: 0x00066A38 File Offset: 0x00064C38
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

		// Token: 0x0600108A RID: 4234 RVA: 0x00066B10 File Offset: 0x00064D10
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
