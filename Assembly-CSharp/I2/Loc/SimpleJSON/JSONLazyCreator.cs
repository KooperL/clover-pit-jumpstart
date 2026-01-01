using System;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020001FF RID: 511
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x060014D5 RID: 5333 RVA: 0x00015DE2 File Offset: 0x00013FE2
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x00015DF8 File Offset: 0x00013FF8
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00015E0E File Offset: 0x0001400E
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

		// Token: 0x17000181 RID: 385
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

		// Token: 0x17000182 RID: 386
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

		// Token: 0x060014DC RID: 5340 RVA: 0x00085218 File Offset: 0x00083418
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray { aItem });
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x000851F4 File Offset: 0x000833F4
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass { { aKey, aItem } });
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00015E48 File Offset: 0x00014048
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00015E53 File Offset: 0x00014053
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00015E48 File Offset: 0x00014048
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00015E5F File Offset: 0x0001405F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0001593F File Offset: 0x00013B3F
		public override string ToString()
		{
			return "";
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0001593F File Offset: 0x00013B3F
		public override string ToString(string aPrefix)
		{
			return "";
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0008523C File Offset: 0x0008343C
		// (set) Token: 0x060014E5 RID: 5349 RVA: 0x00085258 File Offset: 0x00083458
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

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060014E6 RID: 5350 RVA: 0x00085274 File Offset: 0x00083474
		// (set) Token: 0x060014E7 RID: 5351 RVA: 0x00085298 File Offset: 0x00083498
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

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x000852B4 File Offset: 0x000834B4
		// (set) Token: 0x060014E9 RID: 5353 RVA: 0x000852E0 File Offset: 0x000834E0
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

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x000852FC File Offset: 0x000834FC
		// (set) Token: 0x060014EB RID: 5355 RVA: 0x00085318 File Offset: 0x00083518
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

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x00085334 File Offset: 0x00083534
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x00085350 File Offset: 0x00083550
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		// Token: 0x0400145A RID: 5210
		private JSONNode m_Node;

		// Token: 0x0400145B RID: 5211
		private string m_Key;
	}
}
