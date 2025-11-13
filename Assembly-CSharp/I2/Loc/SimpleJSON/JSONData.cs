using System;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	public class JSONData : JSONNode
	{
		// (get) Token: 0x060010A3 RID: 4259 RVA: 0x00067388 File Offset: 0x00065588
		// (set) Token: 0x060010A4 RID: 4260 RVA: 0x00067390 File Offset: 0x00065590
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

		// Token: 0x060010A5 RID: 4261 RVA: 0x00067399 File Offset: 0x00065599
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x000673A8 File Offset: 0x000655A8
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x000673B7 File Offset: 0x000655B7
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x000673C6 File Offset: 0x000655C6
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x000673D5 File Offset: 0x000655D5
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x000673E4 File Offset: 0x000655E4
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00067400 File Offset: 0x00065600
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0006741C File Offset: 0x0006561C
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

		private string m_Data;
	}
}
