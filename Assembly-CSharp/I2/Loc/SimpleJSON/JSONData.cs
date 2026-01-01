using System;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020001FE RID: 510
	public class JSONData : JSONNode
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x00015D6A File Offset: 0x00013F6A
		// (set) Token: 0x060014CC RID: 5324 RVA: 0x00015D72 File Offset: 0x00013F72
		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00015D7B File Offset: 0x00013F7B
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00015D8A File Offset: 0x00013F8A
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00015D99 File Offset: 0x00013F99
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00015DA8 File Offset: 0x00013FA8
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x00015DB7 File Offset: 0x00013FB7
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00015DC6 File Offset: 0x00013FC6
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00015DC6 File Offset: 0x00013FC6
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x000850D8 File Offset: 0x000832D8
		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jsondata = new JSONData("");
			jsondata.AsInt = this.AsInt;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(4);
				aWriter.Write(this.AsInt);
				return;
			}
			jsondata.AsFloat = this.AsFloat;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(7);
				aWriter.Write(this.AsFloat);
				return;
			}
			jsondata.AsDouble = this.AsDouble;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(5);
				aWriter.Write(this.AsDouble);
				return;
			}
			jsondata.AsBool = this.AsBool;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(6);
				aWriter.Write(this.AsBool);
				return;
			}
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x04001459 RID: 5209
		private string m_Data;
	}
}
