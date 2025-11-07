using System;

namespace I2.Loc.SimpleJSON
{
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x06001096 RID: 4246 RVA: 0x00066D37 File Offset: 0x00064F37
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00066D4D File Offset: 0x00064F4D
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00066D63 File Offset: 0x00064F63
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray { value });
			}
		}

		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONClass { { aKey, value } });
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00066DEC File Offset: 0x00064FEC
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray { aItem });
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00066E10 File Offset: 0x00065010
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass { { aKey, aItem } });
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00066E32 File Offset: 0x00065032
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00066E3D File Offset: 0x0006503D
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00066E49 File Offset: 0x00065049
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00066E54 File Offset: 0x00065054
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00066E5C File Offset: 0x0006505C
		public override string ToString()
		{
			return "";
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00066E63 File Offset: 0x00065063
		public override string ToString(string aPrefix)
		{
			return "";
		}

		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x00066E6C File Offset: 0x0006506C
		// (set) Token: 0x060010A6 RID: 4262 RVA: 0x00066E88 File Offset: 0x00065088
		public override int AsInt
		{
			get
			{
				JSONData jsondata = new JSONData(0);
				this.Set(jsondata);
				return 0;
			}
			set
			{
				JSONData jsondata = new JSONData(value);
				this.Set(jsondata);
			}
		}

		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x00066EA4 File Offset: 0x000650A4
		// (set) Token: 0x060010A8 RID: 4264 RVA: 0x00066EC8 File Offset: 0x000650C8
		public override float AsFloat
		{
			get
			{
				JSONData jsondata = new JSONData(0f);
				this.Set(jsondata);
				return 0f;
			}
			set
			{
				JSONData jsondata = new JSONData(value);
				this.Set(jsondata);
			}
		}

		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00066EE4 File Offset: 0x000650E4
		// (set) Token: 0x060010AA RID: 4266 RVA: 0x00066F10 File Offset: 0x00065110
		public override double AsDouble
		{
			get
			{
				JSONData jsondata = new JSONData(0.0);
				this.Set(jsondata);
				return 0.0;
			}
			set
			{
				JSONData jsondata = new JSONData(value);
				this.Set(jsondata);
			}
		}

		// (get) Token: 0x060010AB RID: 4267 RVA: 0x00066F2C File Offset: 0x0006512C
		// (set) Token: 0x060010AC RID: 4268 RVA: 0x00066F48 File Offset: 0x00065148
		public override bool AsBool
		{
			get
			{
				JSONData jsondata = new JSONData(false);
				this.Set(jsondata);
				return false;
			}
			set
			{
				JSONData jsondata = new JSONData(value);
				this.Set(jsondata);
			}
		}

		// (get) Token: 0x060010AD RID: 4269 RVA: 0x00066F64 File Offset: 0x00065164
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// (get) Token: 0x060010AE RID: 4270 RVA: 0x00066F80 File Offset: 0x00065180
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		private JSONNode m_Node;

		private string m_Key;
	}
}
