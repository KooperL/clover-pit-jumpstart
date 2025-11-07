using System;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	public class JSONData : JSONNode
	{
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00066BAC File Offset: 0x00064DAC
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x00066BB4 File Offset: 0x00064DB4
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

		// Token: 0x0600108E RID: 4238 RVA: 0x00066BBD File Offset: 0x00064DBD
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00066BCC File Offset: 0x00064DCC
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00066BDB File Offset: 0x00064DDB
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00066BEA File Offset: 0x00064DEA
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00066BF9 File Offset: 0x00064DF9
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00066C08 File Offset: 0x00064E08
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00066C24 File Offset: 0x00064E24
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00066C40 File Offset: 0x00064E40
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
