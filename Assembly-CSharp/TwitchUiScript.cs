using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Panik;
using TMPro;
using TwitchSDK;
using TwitchSDK.Interop;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000100 RID: 256
public class TwitchUiScript : MonoBehaviour
{
	// Token: 0x06000C5B RID: 3163 RVA: 0x0001025F File Offset: 0x0000E45F
	public static bool IsEnabled()
	{
		return !(TwitchUiScript.instance == null) && TwitchUiScript.instance.holder.activeSelf;
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x00062048 File Offset: 0x00060248
	public static void Open(List<string> optionsKeys)
	{
		TwitchUiScript.instance.showingResults = false;
		TwitchUiScript.instance.holder.SetActive(true);
		TwitchUiScript.instance.optionsKeys.Clear();
		TwitchUiScript.instance.optionsTranslated.Clear();
		TwitchUiScript.instance.optionsKeys.AddRange(optionsKeys);
		for (int i = 0; i < optionsKeys.Count; i++)
		{
			TwitchUiScript.instance.optionsTranslated.Add(Translation.Get(optionsKeys[i]));
		}
		TwitchUiScript.instance.TextCompose(false);
		TwitchUiScript.instance.votingSeconds = 30;
		TwitchUiScript.instance.votingTimer = 1f;
		TwitchUiScript.instance.showResultsTimer = 5f;
		Array.Resize<string>(ref TwitchUiScript.instance.voteChoicesTranslated, TwitchUiScript.instance.optionsTranslated.Count - 1);
		Array.Resize<long>(ref TwitchUiScript.instance.choicesNumberOfVotes, TwitchUiScript.instance.voteChoicesTranslated.Length);
		for (int j = 0; j < TwitchUiScript.instance.voteChoicesTranslated.Length; j++)
		{
			TwitchUiScript.instance.voteChoicesTranslated[j] = TwitchUiScript.instance.optionsTranslated[j + 1];
			TwitchUiScript.instance.choicesNumberOfVotes[j] = 0L;
		}
		string text = Translation.Get("TWITCH_UI_VOTE").Truncate(50, "..");
		TwitchUiScript.instance.StartPoll(text, TwitchUiScript.instance.voteChoicesTranslated);
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x000621AC File Offset: 0x000603AC
	private int ChoicePercentageGet(int index)
	{
		long num = 0L;
		for (int i = 0; i < this.choicesNumberOfVotes.Length; i++)
		{
			num += this.choicesNumberOfVotes[i];
		}
		if (num == 0L)
		{
			return 0;
		}
		int num2 = (this.choicesNumberOfVotes[index] * 100L / num).CastToInt();
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num2 > 100)
		{
			num2 = 100;
		}
		return num2;
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00062204 File Offset: 0x00060404
	private void TextCompose(bool showVictoryResults)
	{
		if (!showVictoryResults)
		{
			this._sb.Clear();
			for (int i = 0; i < this.optionsTranslated.Count; i++)
			{
				if (i > 0)
				{
					this._sb.Append(" - ");
				}
				else
				{
					this._sb.Append("<size=150%>");
					this._sb.Append("<color=orange>");
					this._sb.Append("<size=100%>");
				}
				this._sb.Append(this.optionsTranslated[i]);
				if (i == 0)
				{
					this._sb.Append("</color>  (<color=yellow>");
					this._sb.Append(this.votingSeconds.ToString());
					this._sb.Append("\"");
					this._sb.Append("</color>)");
				}
				this._sb.Append("\n");
			}
			this.text.text = this._sb.ToString();
			this.text.ForceMeshUpdate(false, false);
			this.ImageSizeUpdateToText();
			return;
		}
		this.showResultsTimer = 5f;
		int num = -1;
		long num2 = -1L;
		for (int j = 0; j < this.choicesNumberOfVotes.Length; j++)
		{
			if (this.choicesNumberOfVotes[j] > num2)
			{
				num = j;
				num2 = this.choicesNumberOfVotes[j];
			}
		}
		if (num == -1 || num2 == 0L)
		{
			this.text.text = Translation.Get("TWITCH_UI_NOTHING");
			this.text.ForceMeshUpdate(false, false);
			this.ImageSizeUpdateToText();
			return;
		}
		this._sb.Clear();
		this._sb.Append("<color=orange>");
		this._sb.Append(Translation.Get("TWITCH_RESULT"));
		this._sb.Append("</color>");
		this._sb.Append("\n");
		this._sb.Append(this.voteChoicesTranslated[num]);
		this._sb.Append("  (");
		this._sb.Append(this.ChoicePercentageGet(num));
		this._sb.Append("%)");
		this.text.text = this._sb.ToString();
		this.text.ForceMeshUpdate(false, false);
		this.ImageSizeUpdateToText();
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x0001027F File Offset: 0x0000E47F
	private void ImageSizeUpdateToText()
	{
		this.back.rectTransform.sizeDelta = this.text.GetRenderedValues() + new Vector2(40f, 40f);
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x0006245C File Offset: 0x0006065C
	public static void Close(bool abortVote)
	{
		TwitchUiScript.instance.holder.SetActive(false);
		TwitchUiScript.instance.showingResults = false;
		if (abortVote)
		{
			Sound.Play("SoundTwitchVoteAbort", 1f, 1f);
			if (TwitchMaster.IsTwitchSupported() && TwitchUiScript.instance.ActivePoll != null)
			{
				try
				{
					TwitchUiScript.instance.ActivePoll.MaybeResult.DeletePoll();
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
				}
			}
			TwitchUiScript.instance.ActivePoll = null;
		}
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x000624E8 File Offset: 0x000606E8
	private bool CanStartPool()
	{
		if (this.ActivePoll != null)
		{
			return false;
		}
		TwitchSDKApi api = Twitch.API;
		return ((api != null) ? api.GetAuthState().MaybeResult : null).Scopes.Any((string a) => a == TwitchOAuthScope.Channel.ManagePolls.Scope) && TwitchMaster.IsLoggedInAndEnabled() && !TwitchUiScript.IsEnabled();
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x00062554 File Offset: 0x00060754
	public void PollStartTry()
	{
		if (!this.CanStartPool())
		{
			Sound.Play("SoundMenuError", 1f, 1f);
			CameraGame.Shake(1f);
			return;
		}
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		if (PhoneUiScript.IsEnabled())
		{
			this._CALLBACK_PoolPhone();
			return;
		}
		if (gamePhase == GameplayMaster.GamePhase.preparation)
		{
			this._CALLBACK_PoolCharms();
			return;
		}
		Sound.Play("SoundMenuError", 1f, 1f);
		CameraGame.Shake(1f);
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x000625C8 File Offset: 0x000607C8
	private void _CALLBACK_PoolCharms()
	{
		Sound.Play("SoundTwitchVoteBegin", 1f, 1f);
		this.keysTemp.Clear();
		this.keysTemp.Add("TWITCH_UI_VOTE");
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			if (!(StoreCapsuleScript.storePowerups[i] == null))
			{
				this.keysTemp.Add(StoreCapsuleScript.storePowerups[i].NameKeyGet());
			}
		}
		this.keysTemp.Add("TWITCH_UI_NOTHING");
		TwitchUiScript.Open(this.keysTemp);
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x00062658 File Offset: 0x00060858
	private void _CALLBACK_PoolPhone()
	{
		Sound.Play("SoundTwitchVoteBegin", 1f, 1f);
		this.keysTemp.Clear();
		this.keysTemp.Add("TWITCH_UI_VOTE");
		for (int i = GameplayData.PhoneAbilitiesNumber_Get() - 1; i >= 0; i--)
		{
			if (GameplayData.Instance._phone_AbilitiesToPick[i] != AbilityScript.Identifier.undefined && GameplayData.Instance._phone_AbilitiesToPick[i] != AbilityScript.Identifier.count)
			{
				this.keysTemp.Add(AbilityScript.NameGetKey(GameplayData.Instance._phone_AbilitiesToPick[i]));
			}
		}
		this.keysTemp.Add("TWITCH_UI_NOTHING");
		TwitchUiScript.Open(this.keysTemp);
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x0006270C File Offset: 0x0006090C
	private void VotingVisualUpdate()
	{
		if (!TwitchUiScript.IsEnabled())
		{
			return;
		}
		if (!TwitchMaster.IsLoggedInAndEnabled())
		{
			TwitchUiScript.Close(true);
			return;
		}
		if (this.showingResults)
		{
			this.showResultsTimer -= Tick.Time;
			if (this.showResultsTimer <= 0f && TwitchMaster.IsTwitchSupported())
			{
				if (TwitchUiScript.instance.ActivePoll != null)
				{
					try
					{
						Poll maybeResult = TwitchUiScript.instance.ActivePoll.MaybeResult;
						if (maybeResult.Info.Status == PollStatus.Active)
						{
							maybeResult.FinishPoll();
						}
					}
					catch (Exception ex)
					{
						GameplayMaster.TwitchAffiliationMessageBook();
						Debug.LogError(ex);
					}
				}
				TwitchUiScript.Close(false);
			}
			this.ImageSizeUpdateToText();
			TwitchUiScript.instance.ActivePoll = null;
			return;
		}
		if (this.votingSeconds > 0)
		{
			this.votingTimer -= Tick.Time;
			if (this.votingTimer <= 0f)
			{
				this.votingSeconds--;
				this.votingTimer += 1f;
				Sound.Play("SoundTwitchTimerTick", 1f, 1f + (20f - (float)this.votingSeconds) * 0.01f);
			}
			this.TextCompose(false);
		}
		this.ImageSizeUpdateToText();
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x00062844 File Offset: 0x00060A44
	public bool StartPoll(string pollTitle, string[] pollChoices)
	{
		if (!TwitchMaster.IsTwitchSupported())
		{
			return false;
		}
		if (this.ActivePoll != null)
		{
			return false;
		}
		TwitchSDKApi api = Twitch.API;
		AuthState authState = ((api != null) ? api.GetAuthState().MaybeResult : null);
		if (authState == null || authState.Status != AuthStatus.LoggedIn)
		{
			return false;
		}
		if (!authState.Scopes.Any((string a) => a == TwitchOAuthScope.Channel.ManagePolls.Scope))
		{
			return false;
		}
		if (pollChoices == null || pollChoices.Length == 0)
		{
			pollChoices = new string[] { "Error1", "no choices" };
		}
		for (int i = 0; i < pollChoices.Length; i++)
		{
			if (string.IsNullOrEmpty(pollChoices[i]))
			{
				pollChoices[i] = "Error " + i.ToString();
			}
			else
			{
				pollChoices[i] = pollChoices[i].Truncate(20, "..");
			}
		}
		this.ActivePoll = Twitch.API.NewPoll(new PollDefinition
		{
			Title = pollTitle,
			Choices = pollChoices,
			Duration = 30L
		});
		return true;
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x0006294C File Offset: 0x00060B4C
	private void PollUpdate()
	{
		if (!TwitchMaster.IsTwitchSupported())
		{
			return;
		}
		if (this.showingResults)
		{
			return;
		}
		if (!TwitchUiScript.IsEnabled())
		{
			return;
		}
		try
		{
			if (this.ActivePoll != null)
			{
				GameTask<Poll> activePoll = this.ActivePoll;
				Poll poll = ((activePoll != null) ? activePoll.MaybeResult : null);
				if (poll != null && !(poll.Info == null) && poll.Info.Status != PollStatus.Terminated && poll.Info.Status != PollStatus.Archived && poll.Info.Status != PollStatus.Moderated && poll.Info.Status != PollStatus.Invalid)
				{
					if (poll.Info.Status == PollStatus.Active)
					{
						this.PrematureAbortCheckUpdate();
					}
					else if (poll.Info.Status == PollStatus.Completed)
					{
						int i;
						Func<PollChoiceInfo, bool> <>9__0;
						int i2;
						for (i = 0; i < this.voteChoicesTranslated.Length; i = i2 + 1)
						{
							long[] array = this.choicesNumberOfVotes;
							int j = i;
							IEnumerable<PollChoiceInfo> choices = poll.Info.Choices;
							Func<PollChoiceInfo, bool> func;
							if ((func = <>9__0) == null)
							{
								func = (<>9__0 = (PollChoiceInfo a) => a.Title == this.voteChoicesTranslated[i]);
							}
							array[j] = choices.Where(func).Single<PollChoiceInfo>().Votes;
							i2 = i;
						}
						this.showingResults = true;
						this.TextCompose(true);
					}
				}
			}
		}
		catch (Exception ex)
		{
			GameplayMaster.TwitchAffiliationMessageBook();
			TwitchUiScript.Close(true);
			Debug.LogError(ex);
		}
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x00062AC8 File Offset: 0x00060CC8
	private void PrematureAbortCheckUpdate()
	{
		if (!TwitchUiScript.IsEnabled())
		{
			return;
		}
		if (this.showingResults)
		{
			return;
		}
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		bool flag = false;
		if (DialogueScript.IsEnabled())
		{
			flag = true;
		}
		else if (WCScript.IsPerformingAction())
		{
			flag = true;
		}
		else if (gamePhase != GameplayMaster.GamePhase.preparation && !PhoneUiScript.IsEnabled())
		{
			flag = true;
		}
		else if (GameplayMaster.DeathCountdownHasStarted())
		{
			flag = true;
		}
		else if (TerminalScript.IsLoggedIn())
		{
			flag = true;
		}
		else if (ToyPhoneUIScript.IsEnabled())
		{
			flag = true;
		}
		else if (DeckBoxUI.IsEnabled())
		{
			flag = true;
		}
		else if (MainMenuScript.IsEnabled())
		{
			flag = true;
		}
		else if (DrawersScript.IsAnyDrawerOpened())
		{
			flag = true;
		}
		else if (ScreenMenuScript.IsEnabled())
		{
			flag = true;
		}
		else if (MagazineUiScript.IsEnabled())
		{
			flag = true;
		}
		if (flag)
		{
			TwitchUiScript.Close(true);
		}
		this.oldGamePhase = gamePhase;
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x000102B0 File Offset: 0x0000E4B0
	private void Awake()
	{
		TwitchUiScript.instance = this;
		this.backStartPos = this.back.rectTransform.anchoredPosition;
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x000102CE File Offset: 0x0000E4CE
	private void OnDestroy()
	{
		if (TwitchUiScript.instance == this)
		{
			TwitchUiScript.instance = null;
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x000102E3 File Offset: 0x0000E4E3
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00062B7C File Offset: 0x00060D7C
	private void Update()
	{
		Vector2 zero = new Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			zero = Vector2.zero;
		}
		this.back.rectTransform.anchoredPosition = this.backStartPos + zero;
		this.VotingVisualUpdate();
		this.PollUpdate();
	}

	// Token: 0x04000D28 RID: 3368
	public static TwitchUiScript instance;

	// Token: 0x04000D29 RID: 3369
	private const int VOTE_TIME = 30;

	// Token: 0x04000D2A RID: 3370
	private const string DOTS = "..";

	// Token: 0x04000D2B RID: 3371
	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000D2C RID: 3372
	public GameObject holder;

	// Token: 0x04000D2D RID: 3373
	public Image back;

	// Token: 0x04000D2E RID: 3374
	public TextMeshProUGUI text;

	// Token: 0x04000D2F RID: 3375
	private Vector2 backStartPos;

	// Token: 0x04000D30 RID: 3376
	private List<string> optionsKeys = new List<string>(8);

	// Token: 0x04000D31 RID: 3377
	private List<string> optionsTranslated = new List<string>(8);

	// Token: 0x04000D32 RID: 3378
	private string[] voteChoicesTranslated = new string[8];

	// Token: 0x04000D33 RID: 3379
	private long[] choicesNumberOfVotes = new long[8];

	// Token: 0x04000D34 RID: 3380
	public bool showingResults;

	// Token: 0x04000D35 RID: 3381
	private StringBuilder _sb = new StringBuilder();

	// Token: 0x04000D36 RID: 3382
	private List<string> keysTemp = new List<string>(8);

	// Token: 0x04000D37 RID: 3383
	private int votingSeconds;

	// Token: 0x04000D38 RID: 3384
	private float votingTimer = 1f;

	// Token: 0x04000D39 RID: 3385
	private float showResultsTimer;

	// Token: 0x04000D3A RID: 3386
	private GameTask<Poll> ActivePoll;

	// Token: 0x04000D3B RID: 3387
	private GameplayMaster.GamePhase oldGamePhase = GameplayMaster.GamePhase.Undefined;
}
