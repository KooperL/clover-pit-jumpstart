using System;

namespace I2.Loc.SimpleJSON
{
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x060010AD RID: 4269 RVA: 0x00067513 File Offset: 0x00065713
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00067529 File Offset: 0x00065729
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0006753F File Offset: 0x0006573F
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

		// Token: 0x060010B4 RID: 4276 RVA: 0x000675C8 File Offset: 0x000657C8
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray { aItem });
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x000675EC File Offset: 0x000657EC
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass { { aKey, aItem } });
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0006760E File Offset: 0x0006580E
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00067619 File Offset: 0x00065819
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00067625 File Offset: 0x00065825
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00067630 File Offset: 0x00065830
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00067638 File Offset: 0x00065838
		public override string ToString()
		{
			return "";
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0006763F File Offset: 0x0006583F
		public override string ToString(string aPrefix)
		{
			return "";
		}

		// (get) Token: 0x060010BC RID: 4284 RVA: 0x00067648 File Offset: 0x00065848
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x00067664 File Offset: 0x00065864
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

		// (get) Token: 0x060010BE RID: 4286 RVA: 0x00067680 File Offset: 0x00065880
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x000676A4 File Offset: 0x000658A4
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

		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x000676C0 File Offset: 0x000658C0
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x000676EC File Offset: 0x000658EC
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

		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x00067708 File Offset: 0x00065908
		// (set) Token: 0x060010C3 RID: 4291 RVA: 0x00067724 File Offset: 0x00065924
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

		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x00067740 File Offset: 0x00065940
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0006775C File Offset: 0x0006595C
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
