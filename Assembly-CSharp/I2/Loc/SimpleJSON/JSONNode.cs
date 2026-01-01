using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020001F4 RID: 500
	public class JSONNode
	{
		// Token: 0x06001449 RID: 5193 RVA: 0x0000774E File Offset: 0x0000594E
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x17000160 RID: 352
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

		// Token: 0x17000161 RID: 353
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

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x0001593F File Offset: 0x00013B3F
		// (set) Token: 0x0600144F RID: 5199 RVA: 0x0000774E File Offset: 0x0000594E
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

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x00015946 File Offset: 0x00013B46
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x000146FE File Offset: 0x000128FE
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x000146FE File Offset: 0x000128FE
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x00015954 File Offset: 0x00013B54
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x00015957 File Offset: 0x00013B57
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x00015960 File Offset: 0x00013B60
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

		// Token: 0x06001457 RID: 5207 RVA: 0x00015970 File Offset: 0x00013B70
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x00015970 File Offset: 0x00013B70
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x00083DBC File Offset: 0x00081FBC
		// (set) Token: 0x0600145A RID: 5210 RVA: 0x00015977 File Offset: 0x00013B77
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

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x00083DE0 File Offset: 0x00081FE0
		// (set) Token: 0x0600145C RID: 5212 RVA: 0x00015986 File Offset: 0x00013B86
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

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x00083E0C File Offset: 0x0008200C
		// (set) Token: 0x0600145E RID: 5214 RVA: 0x00015995 File Offset: 0x00013B95
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

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x00083E40 File Offset: 0x00082040
		// (set) Token: 0x06001460 RID: 5216 RVA: 0x000159A4 File Offset: 0x00013BA4
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

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x000159BB File Offset: 0x00013BBB
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06001462 RID: 5218 RVA: 0x000159C3 File Offset: 0x00013BC3
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x000159CB File Offset: 0x00013BCB
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x000159D3 File Offset: 0x00013BD3
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x000159E6 File Offset: 0x00013BE6
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || a == b;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x000159F9 File Offset: 0x00013BF9
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x00015A05 File Offset: 0x00013C05
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00015A0B File Offset: 0x00013C0B
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x00083E70 File Offset: 0x00082070
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

		// Token: 0x0600146A RID: 5226 RVA: 0x00083F40 File Offset: 0x00082140
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

		// Token: 0x0600146B RID: 5227 RVA: 0x0000774E File Offset: 0x0000594E
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x00084398 File Offset: 0x00082598
		public void SaveToStream(Stream aData)
		{
			BinaryWriter binaryWriter = new BinaryWriter(aData);
			this.Serialize(binaryWriter);
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x00015A13 File Offset: 0x00013C13
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x00015A13 File Offset: 0x00013C13
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00015A13 File Offset: 0x00013C13
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x000843B4 File Offset: 0x000825B4
		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x00084404 File Offset: 0x00082604
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

		// Token: 0x06001472 RID: 5234 RVA: 0x00084450 File Offset: 0x00082650
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

		// Token: 0x06001473 RID: 5235 RVA: 0x00015A13 File Offset: 0x00013C13
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00015A13 File Offset: 0x00013C13
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00015A13 File Offset: 0x00013C13
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0008454C File Offset: 0x0008274C
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode jsonnode;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				jsonnode = JSONNode.Deserialize(binaryReader);
			}
			return jsonnode;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00084584 File Offset: 0x00082784
		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode jsonnode;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				jsonnode = JSONNode.LoadFromStream(fileStream);
			}
			return jsonnode;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00015A1F File Offset: 0x00013C1F
		public static JSONNode LoadFromBase64(string aBase64)
		{
			return JSONNode.LoadFromStream(new MemoryStream(Convert.FromBase64String(aBase64))
			{
				Position = 0L
			});
		}
	}
}
