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

		// (get) Token: 0x06001073 RID: 4211 RVA: 0x00066549 File Offset: 0x00064749
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00066556 File Offset: 0x00064756
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00066564 File Offset: 0x00064764
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

		// Token: 0x06001076 RID: 4214 RVA: 0x00066592 File Offset: 0x00064792
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// (get) Token: 0x06001077 RID: 4215 RVA: 0x000665A2 File Offset: 0x000647A2
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

		// Token: 0x06001078 RID: 4216 RVA: 0x000665B2 File Offset: 0x000647B2
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

		// Token: 0x06001079 RID: 4217 RVA: 0x000665C4 File Offset: 0x000647C4
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

		// Token: 0x0600107A RID: 4218 RVA: 0x00066648 File Offset: 0x00064848
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

		// Token: 0x0600107B RID: 4219 RVA: 0x000666EC File Offset: 0x000648EC
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
