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

public class TwitchUiScript : MonoBehaviour
{
	// Token: 0x06000A88 RID: 2696 RVA: 0x00047B6C File Offset: 0x00045D6C
	public static bool IsEnabled()
	{
		return !(TwitchUiScript.instance == null) && TwitchUiScript.instance.holder.activeSelf;
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00047B8C File Offset: 0x00045D8C
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
		string text = StringUtility.Truncate(Translation.Get("TWITCH_UI_VOTE"), 50, "..");
		TwitchUiScript.instance.StartPoll(text, TwitchUiScript.instance.voteChoicesTranslated);
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00047CF0 File Offset: 0x00045EF0
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

	// Token: 0x06000A8B RID: 2699 RVA: 0x00047D48 File Offset: 0x00045F48
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

	// Token: 0x06000A8C RID: 2700 RVA: 0x00047F9D File Offset: 0x0004619D
	private void ImageSizeUpdateToText()
	{
		this.back.rectTransform.sizeDelta = this.text.GetRenderedValues() + new Vector2(40f, 40f);
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00047FD0 File Offset: 0x000461D0
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

	// Token: 0x06000A8E RID: 2702 RVA: 0x0004805C File Offset: 0x0004625C
	private bool CanStartPool()
	{
		if (this.ActivePoll != null)
		{
			return false;
		}
		TwitchSDKApi api = Twitch.API;
		return ((api != null) ? api.GetAuthState().MaybeResult : null).Scopes.Any((string a) => a == TwitchOAuthScope.Channel.ManagePolls.Scope) && TwitchMaster.IsLoggedInAndEnabled() && !TwitchUiScript.IsEnabled();
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x000480C8 File Offset: 0x000462C8
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

	// Token: 0x06000A90 RID: 2704 RVA: 0x0004813C File Offset: 0x0004633C
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

	// Token: 0x06000A91 RID: 2705 RVA: 0x000481CC File Offset: 0x000463CC
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

	// Token: 0x06000A92 RID: 2706 RVA: 0x00048280 File Offset: 0x00046480
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
						if (maybeResult.Info.Status == null)
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

	// Token: 0x06000A93 RID: 2707 RVA: 0x000483B8 File Offset: 0x000465B8
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
		if (authState == null || authState.Status != 3)
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
				pollChoices[i] = StringUtility.Truncate(pollChoices[i], 20, "..");
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

	// Token: 0x06000A94 RID: 2708 RVA: 0x000484C0 File Offset: 0x000466C0
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
				if (poll != null && !(poll.Info == null) && poll.Info.Status != 2 && poll.Info.Status != 3 && poll.Info.Status != 4 && poll.Info.Status != 5)
				{
					if (poll.Info.Status == null)
					{
						this.PrematureAbortCheckUpdate();
					}
					else if (poll.Info.Status == 1)
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

	// Token: 0x06000A95 RID: 2709 RVA: 0x0004863C File Offset: 0x0004683C
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

	// Token: 0x06000A96 RID: 2710 RVA: 0x000486ED File Offset: 0x000468ED
	private void Awake()
	{
		TwitchUiScript.instance = this;
		this.backStartPos = this.back.rectTransform.anchoredPosition;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0004870B File Offset: 0x0004690B
	private void OnDestroy()
	{
		if (TwitchUiScript.instance == this)
		{
			TwitchUiScript.instance = null;
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x00048720 File Offset: 0x00046920
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00048730 File Offset: 0x00046930
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

	public static TwitchUiScript instance;

	private const int VOTE_TIME = 30;

	private const string DOTS = "..";

	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	public GameObject holder;

	public Image back;

	public TextMeshProUGUI text;

	private Vector2 backStartPos;

	private List<string> optionsKeys = new List<string>(8);

	private List<string> optionsTranslated = new List<string>(8);

	private string[] voteChoicesTranslated = new string[8];

	private long[] choicesNumberOfVotes = new long[8];

	public bool showingResults;

	private StringBuilder _sb = new StringBuilder();

	private List<string> keysTemp = new List<string>(8);

	private int votingSeconds;

	private float votingTimer = 1f;

	private float showResultsTimer;

	private GameTask<Poll> ActivePoll;

	private GameplayMaster.GamePhase oldGamePhase = GameplayMaster.GamePhase.Undefined;
}
