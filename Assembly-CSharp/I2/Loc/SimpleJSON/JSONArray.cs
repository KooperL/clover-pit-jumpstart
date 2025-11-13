using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	public class JSONArray : JSONNode, IEnumerable
	{
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
					return;
				}
				this.m_List[aIndex] = value;
			}
		}

		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.m_List.Add(value);
			}
		}

		// (get) Token: 0x0600108A RID: 4234 RVA: 0x00066D25 File Offset: 0x00064F25
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00066D32 File Offset: 0x00064F32
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00066D40 File Offset: 0x00064F40
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode jsonnode = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return jsonnode;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00066D6E File Offset: 0x00064F6E
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// (get) Token: 0x0600108E RID: 4238 RVA: 0x00066D7E File Offset: 0x00064F7E
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (JSONNode jsonnode in this.m_List)
				{
					yield return jsonnode;
				}
				List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00066D8E File Offset: 0x00064F8E
		public IEnumerator GetEnumerator()
		{
			foreach (JSONNode jsonnode in this.m_List)
			{
				yield return jsonnode;
			}
			List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00066DA0 File Offset: 0x00064FA0
		public override string ToString()
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text += jsonnode.ToString();
			}
			text += " ]";
			return text;
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00066E24 File Offset: 0x00065024
		public override string ToString(string aPrefix)
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text += jsonnode.ToString(aPrefix + "   ");
			}
			text = text + "\n" + aPrefix + "]";
			return text;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00066EC8 File Offset: 0x000650C8
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		private List<JSONNode> m_List = new List<JSONNode>();
	}
}
