﻿using System.Collections.Generic;
using Mopsicus.InfiniteScroll;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Simple horizontal scroll with different items width and "pull-to-refresh" option
public class Demo5 : MonoBehaviour {

	[SerializeField]
	private InfiniteScroll Scroll;

	[SerializeField]
	private int Count = 100;

	[SerializeField]
	private int PullCount = 25;

	private List<int> _list = new List<int> ();

	private List<int> _widths = new List<int> ();

	void Awake ()
	{
		Scroll.OnFill += OnFillItem;
		Scroll.OnWidth += OnWidthItem;
		Scroll.OnPull += OnPullItem;

		for (int i = 0; i < Count; i++)
		{
			_list.Add (i);
			_widths.Add (Random.Range (100, 200));
		}

		Scroll.InitData (_list.Count);
	}

	void OnFillItem (int index, GameObject item) {
		item.GetComponentInChildren<Text> ().text = _list[index].ToString ();
	}

	int OnWidthItem (int index) {
		return _widths[index];
	}

	void OnPullItem (InfiniteScroll.Direction direction) {
		int index = _list.Count;
		if (direction == InfiniteScroll.Direction.Top) {
			for (int i = 0; i < PullCount; i++) {
				_list.Insert (0, index);
				_widths.Insert (0, Random.Range (100, 200));
				index++;
			}
		} else {
			for (int i = 0; i < PullCount; i++) {
				_list.Add (index);
				_widths.Add (Random.Range (100, 200));
				index++;
			}
		}
		Scroll.ApplyDataTo (_list.Count, PullCount, direction);
	}

	public void SceneLoad (int index) {
		SceneManager.LoadScene (index);
	}

}