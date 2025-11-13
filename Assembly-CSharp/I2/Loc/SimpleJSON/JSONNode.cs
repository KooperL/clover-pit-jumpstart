using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	public class JSONNode
	{
		// Token: 0x06001055 RID: 4181 RVA: 0x00066365 File Offset: 0x00064565
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// (get) Token: 0x0600105A RID: 4186 RVA: 0x00066371 File Offset: 0x00064571
		// (set) Token: 0x0600105B RID: 4187 RVA: 0x00066378 File Offset: 0x00064578
		public virtual string Value
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		// (get) Token: 0x0600105C RID: 4188 RVA: 0x0006637A File Offset: 0x0006457A
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0006637D File Offset: 0x0006457D
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0006638B File Offset: 0x0006458B
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0006638E File Offset: 0x0006458E
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x00066391 File Offset: 0x00064591
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// (get) Token: 0x06001061 RID: 4193 RVA: 0x00066394 File Offset: 0x00064594
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// (get) Token: 0x06001062 RID: 4194 RVA: 0x0006639D File Offset: 0x0006459D
		public IEnumerable<JSONNode> DeepChilds
		{
			get
			{
				foreach (JSONNode jsonnode in this.Childs)
				{
					foreach (JSONNode jsonnode2 in jsonnode.DeepChilds)
					{
						yield return jsonnode2;
					}
					IEnumerator<JSONNode> enumerator2 = null;
				}
				IEnumerator<JSONNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x000663AD File Offset: 0x000645AD
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x000663B4 File Offset: 0x000645B4
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// (get) Token: 0x06001065 RID: 4197 RVA: 0x000663BC File Offset: 0x000645BC
		// (set) Token: 0x06001066 RID: 4198 RVA: 0x000663DD File Offset: 0x000645DD
		public virtual int AsInt
		{
			get
			{
				int num = 0;
				if (int.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// (get) Token: 0x06001067 RID: 4199 RVA: 0x000663EC File Offset: 0x000645EC
		// (set) Token: 0x06001068 RID: 4200 RVA: 0x00066415 File Offset: 0x00064615
		public virtual float AsFloat
		{
			get
			{
				float num = 0f;
				if (float.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0f;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// (get) Token: 0x06001069 RID: 4201 RVA: 0x00066424 File Offset: 0x00064624
		// (set) Token: 0x0600106A RID: 4202 RVA: 0x00066455 File Offset: 0x00064655
		public virtual double AsDouble
		{
			get
			{
				double num = 0.0;
				if (double.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// (get) Token: 0x0600106B RID: 4203 RVA: 0x00066464 File Offset: 0x00064664
		// (set) Token: 0x0600106C RID: 4204 RVA: 0x00066492 File Offset: 0x00064692
		public virtual bool AsBool
		{
			get
			{
				bool flag = false;
				if (bool.TryParse(this.Value, out flag))
				{
					return flag;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = (value ? "true" : "false");
			}
		}

		// (get) Token: 0x0600106D RID: 4205 RVA: 0x000664A9 File Offset: 0x000646A9
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// (get) Token: 0x0600106E RID: 4206 RVA: 0x000664B1 File Offset: 0x000646B1
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x000664B9 File Offset: 0x000646B9
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x000664C1 File Offset: 0x000646C1
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x000664D4 File Offset: 0x000646D4
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || a == b;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x000664E7 File Offset: 0x000646E7
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x000664F3 File Offset: 0x000646F3
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x000664F9 File Offset: 0x000646F9
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00066504 File Offset: 0x00064704
		internal static string Escape(string aText)
		{
			string text = "";
			int i = 0;
			while (i < aText.Length)
			{
				char c = aText[i];
				switch (c)
				{
				case '\b':
					text += "\\b";
					break;
				case '\t':
					text += "\\t";
					break;
				case '\n':
					text += "\\n";
					break;
				case '\v':
					goto IL_00A3;
				case '\f':
					text += "\\f";
					break;
				case '\r':
					text += "\\r";
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							goto IL_00A3;
						}
						text += "\\\\";
					}
					else
					{
						text += "\\\"";
					}
					break;
				}
				IL_00B1:
				i++;
				continue;
				IL_00A3:
				text += c.ToString();
				goto IL_00B1;
			}
			return text;
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x000665D4 File Offset: 0x000647D4
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			string text = "";
			string text2 = "";
			bool flag = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				if (c <= ',')
				{
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
						case '\r':
							goto IL_0429;
						case '\v':
						case '\f':
							goto IL_0412;
						default:
							if (c != ' ')
							{
								goto IL_0412;
							}
							break;
						}
						if (flag)
						{
							text += aJSON[i].ToString();
						}
					}
					else if (c != '"')
					{
						if (c != ',')
						{
							goto IL_0412;
						}
						if (flag)
						{
							text += aJSON[i].ToString();
						}
						else
						{
							if (text != "")
							{
								if (jsonnode is JSONArray)
								{
									jsonnode.Add(text);
								}
								else if (text2 != "")
								{
									jsonnode.Add(text2, text);
								}
							}
							text2 = "";
							text = "";
						}
					}
					else
					{
						flag = !flag;
					}
				}
				else
				{
					if (c <= ']')
					{
						if (c != ':')
						{
							switch (c)
							{
							case '[':
								if (flag)
								{
									text += aJSON[i].ToString();
									goto IL_0429;
								}
								stack.Push(new JSONArray());
								if (jsonnode != null)
								{
									text2 = text2.Trim();
									if (jsonnode is JSONArray)
									{
										jsonnode.Add(stack.Peek());
									}
									else if (text2 != "")
									{
										jsonnode.Add(text2, stack.Peek());
									}
								}
								text2 = "";
								text = "";
								jsonnode = stack.Peek();
								goto IL_0429;
							case '\\':
								i++;
								if (flag)
								{
									char c2 = aJSON[i];
									if (c2 <= 'f')
									{
										if (c2 == 'b')
										{
											text += "\b";
											goto IL_0429;
										}
										if (c2 == 'f')
										{
											text += "\f";
											goto IL_0429;
										}
									}
									else
									{
										if (c2 == 'n')
										{
											text += "\n";
											goto IL_0429;
										}
										switch (c2)
										{
										case 'r':
											text += "\r";
											goto IL_0429;
										case 't':
											text += "\t";
											goto IL_0429;
										case 'u':
										{
											string text3 = aJSON.Substring(i + 1, 4);
											text += ((char)int.Parse(text3, NumberStyles.AllowHexSpecifier)).ToString();
											i += 4;
											goto IL_0429;
										}
										}
									}
									text += c2.ToString();
									goto IL_0429;
								}
								goto IL_0429;
							case ']':
								break;
							default:
								goto IL_0412;
							}
						}
						else
						{
							if (flag)
							{
								text += aJSON[i].ToString();
								goto IL_0429;
							}
							text2 = text;
							text = "";
							goto IL_0429;
						}
					}
					else if (c != '{')
					{
						if (c != '}')
						{
							goto IL_0412;
						}
					}
					else
					{
						if (flag)
						{
							text += aJSON[i].ToString();
							goto IL_0429;
						}
						stack.Push(new JSONClass());
						if (jsonnode != null)
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(stack.Peek());
							}
							else if (text2 != "")
							{
								jsonnode.Add(text2, stack.Peek());
							}
						}
						text2 = "";
						text = "";
						jsonnode = stack.Peek();
						goto IL_0429;
					}
					if (flag)
					{
						text += aJSON[i].ToString();
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (text != "")
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(text);
							}
							else if (text2 != "")
							{
								jsonnode.Add(text2, text);
							}
						}
						text2 = "";
						text = "";
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
				}
				IL_0429:
				i++;
				continue;
				IL_0412:
				text += aJSON[i].ToString();
				goto IL_0429;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00066A2A File Offset: 0x00064C2A
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x00066A2C File Offset: 0x00064C2C
		public void SaveToStream(Stream aData)
		{
			BinaryWriter binaryWriter = new BinaryWriter(aData);
			this.Serialize(binaryWriter);
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00066A47 File Offset: 0x00064C47
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00066A53 File Offset: 0x00064C53
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00066A5F File Offset: 0x00064C5F
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00066A6C File Offset: 0x00064C6C
		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00066ABC File Offset: 0x00064CBC
		public string SaveToBase64()
		{
			string text;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SaveToStream(memoryStream);
				memoryStream.Position = 0L;
				text = Convert.ToBase64String(memoryStream.ToArray());
			}
			return text;
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00066B08 File Offset: 0x00064D08
		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)aReader.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONBinaryTag.Class:
			{
				int num2 = aReader.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string text = aReader.ReadString();
					JSONNode jsonnode = JSONNode.Deserialize(aReader);
					jsonclass.Add(text, jsonnode);
				}
				return jsonclass;
			}
			case JSONBinaryTag.Value:
				return new JSONData(aReader.ReadString());
			case JSONBinaryTag.IntValue:
				return new JSONData(aReader.ReadInt32());
			case JSONBinaryTag.DoubleValue:
				return new JSONData(aReader.ReadDouble());
			case JSONBinaryTag.BoolValue:
				return new JSONData(aReader.ReadBoolean());
			case JSONBinaryTag.FloatValue:
				return new JSONData(aReader.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag.ToString());
			}
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00066C02 File Offset: 0x00064E02
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00066C0E File Offset: 0x00064E0E
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x00066C1A File Offset: 0x00064E1A
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00066C28 File Offset: 0x00064E28
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode jsonnode;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				jsonnode = JSONNode.Deserialize(binaryReader);
			}
			return jsonnode;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00066C60 File Offset: 0x00064E60
		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode jsonnode;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				jsonnode = JSONNode.LoadFromStream(fileStream);
			}
			return jsonnode;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00066C98 File Offset: 0x00064E98
		public static JSONNode LoadFromBase64(string aBase64)
		{
			return JSONNode.LoadFromStream(new MemoryStream(Convert.FromBase64String(aBase64))
			{
				Position = 0L
			});
		}
	}
}
