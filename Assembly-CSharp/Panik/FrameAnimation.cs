using System;
using UnityEngine;

namespace Panik
{
	[CreateAssetMenu(fileName = "FrameAnimation", menuName = "Panik Arcade/FrameAnim/FrameAnimation", order = 1)]
	public class FrameAnimation : ScriptableObject
	{
		public Sprite[] frames;
	}
}
